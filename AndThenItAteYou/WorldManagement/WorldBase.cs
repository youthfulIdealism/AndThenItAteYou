using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Survive.WorldManagement.ContentProcessors;
using Survive.WorldManagement.Entities;
using Survive.WorldManagement.Entities.Particles;
using Survive.WorldManagement.Inventory;
using Survive.WorldManagement.Procedurals;
using Survive.WorldManagement.Tile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Survive.WorldManagement
{
    public abstract class WorldBase : IDisposable
    {
        public int pauseRequests = 0;
        public enum WorldGenType { BLANK, GENERATED, LOAD}
        public WorldGenType worldGenType;
        public WorldReader worldReader;
        public int tileGenRadious = 2;

        public Dictionary<Point, Chunk> chunks;

        public HashSet<Point> chunksQueuedForWorldGen;
        public HashSet<Point> chunksinWorldGen;
        protected List<Chunk> deQueuedChunk;
        protected List<Chunk> removeChunks;

        public HashSet<Point> chunksQueuedForPhysicsGen;
        public HashSet<Point> chunksinPhysicsGen;
        protected List<Chunk> deQueuedChunkPhysicsGen;

        public PlayerBase player;

        public Vector2 playerLoc;
        public Point drawOffset;
        public Random rand;
        public bool hasRedrawnChunkThisUpdate;

        public HashSet<Entity> entities;
        public HashSet<Entity> itemEntities;
        public HashSet<Entity> useableEntities;
        protected HashSet<Entity> addedEntities;
        protected HashSet<Entity> removedEntities;
        public HashSet<Entity> queuedEntites;
        protected HashSet<Entity> deQueuedEntities;
        public HashSet<Particle> particles;
        protected HashSet<Particle> addedParticles;
        protected HashSet<Particle> removedParticles;

        public Color groundColor;

        public int difficulty { get; set; }
        //0 = midnight, 1 = noon, 2 = midnight
        public float timeOfDay { get; set; }

        public float screenShakeRemaining { get; set; }
        public float screenShakeForce { get; set; }
        private Vector2 currentScreenShakeOffsetTarget;
        private Vector2 prevScreenShakeOffsetTarget;
        private float timeUntilNewScreenShakeVector;
        public float timeUntilNewScreenShakeVectorMarker;
        public ChunkDecorator decorator { get; protected set; }
        protected List<DelayedRenderable> delayedRenders;
        public bool spawnsEnemies;

        public HashSet<int> chunksSpawnedEntities;

        public WorldBase(int difficulty)
        {
            rand = new Random((difficulty + UniverseProperties.seed).GetHashCode() * (difficulty + 5));
            this.difficulty = difficulty;

            //set up the decoration maker/manager
            decorator = new ChunkDecorator(this);

            chunks = new Dictionary<Point, Chunk>();

            //set up threading for world generation
            chunksQueuedForWorldGen = new HashSet<Point>();
            chunksinWorldGen = new HashSet<Point>();
            deQueuedChunk = new List<Chunk>();

            //set up threading for physics baking
            chunksQueuedForPhysicsGen = new HashSet<Point>();
            chunksinPhysicsGen = new HashSet<Point>();
            deQueuedChunkPhysicsGen = new List<Chunk>();

            //set up entity and entity categories
            entities = new HashSet<Entity>();
            addedEntities = new HashSet<Entity>();
            removedEntities = new HashSet<Entity>();
            itemEntities = new HashSet<Entity>();
            useableEntities = new HashSet<Entity>();
            particles = new HashSet<Particle>();
            addedParticles = new HashSet<Particle>();
            removedParticles = new HashSet<Particle>();
            delayedRenders = new List<DelayedRenderable>();

            removeChunks = new List<Chunk>();

            groundColor = Color.Black;
            worldGenType = WorldGenType.BLANK;
            currentScreenShakeOffsetTarget = new Vector2();
            prevScreenShakeOffsetTarget = new Vector2();
            spawnsEnemies = true;

            queuedEntites = new HashSet<Entity>();
            deQueuedEntities = new HashSet<Entity>();

            chunksSpawnedEntities = new HashSet<int>();
        }

        public virtual void switchTo()
        {

        }

        public void requestDelayedRender(DelayedRenderable renderable)
        {
            delayedRenders.Add(renderable);
        }

        public virtual void update(GameTime time)
        {
            delayedRenders.Clear();

            //retrieve player location for chunk-loading reference, etc
            playerLoc = player.location;

            performChunkManagement(time);
            player.manageControlManager(time);
            if (pauseRequests == 0)
            {
                updateEntities(time);
                managePlayerDeath();
                manageScreenShake();
                
            }
        }

        public virtual void managePlayerDeath()
        {
            if(player.health <= 0)
            {
                Game1.instance.handlePlayerDeath();
            }
        }

        public virtual void transformPlayer(PlayerBase newPlayer)
        {
            Random rand = new Random();
            for (int i = 0; i < 40; i++)
            {
                ParticleSpeedBoost particle = new ParticleSpeedBoost(playerLoc + new Vector2(rand.Next(30) - 15, rand.Next(30) - 15), this, new Vector2((float)rand.NextDouble() * 2 - 1, (float)rand.NextDouble() * 2 - 1), 100);
               addEntity(particle);
            }
            newPlayer.velocity = player.velocity;
            addEntity(newPlayer);
            killEntity(player);
            player = newPlayer;
        }

        protected virtual void manageScreenShake()
        {
            screenShakeForce *= .9f;
            screenShakeRemaining--;
            timeUntilNewScreenShakeVector--;
            if (timeUntilNewScreenShakeVector <= 0 && screenShakeRemaining > 0)
            {
                prevScreenShakeOffsetTarget = currentScreenShakeOffsetTarget;
                currentScreenShakeOffsetTarget = new Vector2((float)(rand.NextDouble() - .5f), (float)(rand.NextDouble() - .5f)) * screenShakeForce;
                timeUntilNewScreenShakeVector = rand.Next(7);
                timeUntilNewScreenShakeVectorMarker = timeUntilNewScreenShakeVector + 3;
            }
        }

        public virtual Vector2 getCurrentScreenShakeOffset()
        {
            return Vector2.Lerp(prevScreenShakeOffsetTarget, currentScreenShakeOffsetTarget, 1 - (timeUntilNewScreenShakeVector / timeUntilNewScreenShakeVectorMarker));
        }

        public virtual void shakeScreen(float time, float force)
        {
            if(time > screenShakeRemaining)
            {
                screenShakeRemaining = time;
            }
            screenShakeForce += force;
        }

        public virtual void updateEntities(GameTime time)
        {
            //update all entities, then perform any add or remove cleanups.
            foreach (Entity entity in entities)
            {
                entity.update(time);
            }
            foreach (Entity entity in addedEntities)
            {
                entities.Add(entity);
                if (entity is ItemDropEntity)
                {
                    itemEntities.Add(entity);
                }
                if (entity is UsableEntity)
                {
                    useableEntities.Add(entity);
                }
            }
            foreach (Entity entity in removedEntities)
            {
                entities.Remove(entity);
                if (entity is ItemDropEntity)
                {
                    itemEntities.Remove(entity);
                }
                if (entity is UsableEntity)
                {
                    useableEntities.Remove(entity);
                }
            }
            addedEntities.Clear();
            removedEntities.Clear();


            foreach (Particle particle in particles)
            {
                particle.update(time);
            }
            foreach (Particle particle in addedParticles)
            {
                particles.Add(particle);
            }
            foreach (Particle particle in removedParticles)
            {
                particles.Remove(particle);
            }
            addedParticles.Clear();
            removedParticles.Clear();
        }

        public virtual void onQueueChunkForWorldGen(Point where)
        {

        }

        public virtual void onDeQueueChunk(Chunk chunk)
        {
            handleQueuedEntities(chunk);
        }

        protected virtual void handleCritterSpawn(Chunk chunk)
        {
            if (spawnsEnemies && !chunksSpawnedEntities.Contains(chunk.location.X))
            {
                for (int i = 0; i < 3; i++)
                {
                    if (rand.NextDouble() < decorator.spawnCritterChance)
                    {
                        //find the critter a random tile to stand on top of
                        int critterSpawnLoc = rand.Next(Chunk.tilesPerChunk);
                        //find the height of the tile (plus a little--we still need to bake the chunk's physics!)
                        //float groundLevel = noise.octavePerlin1D((float)(chunk.location.X * Chunk.tilesPerChunk + critterSpawnLoc) / 25) * decorator.getTerrainMultiplier() * Chunk.tileDrawWidth/* + seaLevel - 700*/;
                        float groundLevel = 0;
                        Vector2 spawnLoc = new Vector2(((chunk.location.X * Chunk.tilesPerChunk + critterSpawnLoc) * Chunk.tileDrawWidth), groundLevel);
                        this.addEntity(decorator.getCritterForChunk(spawnLoc));
                        chunksSpawnedEntities.Add(chunk.location.X);
                    }
                }

            }
        }

        protected virtual void handleQueuedEntities(Chunk chunk)
        {
            //at this point, we can safely generate any critters that shoulds spawn with the chunk.

            handleCritterSpawn(chunk);

            foreach (Entity e in queuedEntites)
            {
                if (e.getCollisionBox().Intersects(chunk.totalBox))
                {
                    addEntity(e);
                    deQueuedEntities.Add(e);
                }
            }

            foreach (Entity e in deQueuedEntities)
            {
                queuedEntites.Remove(e);
            }
            deQueuedEntities.Clear();
        }

        /**
            Perform a crude fast forward of the world. Intended for use if time goes by from, for example, crafting.
        */
        public virtual void fastForward(float timeAsPercentageOfDay)
        {
            
        }


        public virtual float getCurrentTemperature()
        {
            return 0;
        }

        /**
            Spawn an entity in the world
        */
        public virtual void addEntity(Entity entity)
        {
            if(entity is Particle)
            {
                addedParticles.Add((Particle)entity);
            }
            else
            {
                addedEntities.Add(entity);
            }
        }

        public virtual void killEntity(Entity entity)
        {
            if(entity is Particle)
            {
                removedParticles.Add((Particle)entity);
            }else if (!removedEntities.Contains(entity))
            {
                removedEntities.Add(entity);
            }
        }

        public virtual AABB tryMove(Entity entity, AABB attemptedDestination)
        {
            return attemptedDestination;
        }

        public virtual void trackPlayerMovementsWithCamera()
        {
            int playerTileLocX = (int)Math.Floor(playerLoc.X);
            int playerTileLocY = (int)Math.Floor(playerLoc.Y);
            drawOffset = new Point(-playerTileLocX, -playerTileLocY);
        }

        public void performChunkManagement(GameTime time)
        {
            //find the player's location and the draw offset
            int playerTileLocX = (int)Math.Floor(playerLoc.X);
            int playerTileLocY = (int)Math.Floor(playerLoc.Y);
            trackPlayerMovementsWithCamera();

            //find which chunk the player is in, ...
            int playerChunkLocX = (int)Math.Floor(playerLoc.X / (Chunk.tilesPerChunk * Chunk.tileDrawWidth));
            int playerChunkLocY = (int)Math.Floor(playerLoc.Y / (Chunk.tilesPerChunk * Chunk.tileDrawWidth));

            //...and use that information to queue any un-generated surrounding chunks.
            for (int x = playerChunkLocX - tileGenRadious; x <= playerChunkLocX + tileGenRadious; x++)
            {
                for (int y = playerChunkLocY - tileGenRadious; y <= playerChunkLocY + tileGenRadious; y++)
                {
                    if (!chunks.ContainsKey(new Point(x, y)) && !chunksQueuedForWorldGen.Contains(new Point(x, y)) && !chunksinWorldGen.Contains(new Point(x, y)))
                    {
                        Point queuePoint = new Point(x, y);
                        chunksQueuedForWorldGen.Add(queuePoint);
                        onQueueChunkForWorldGen(queuePoint);
                    }
                }
            }

            //try to acquire chunk lock. When it's free, add the contents of the chunk dequeue to the chunk dictionary and clean it out.
            if (Monitor.TryEnter(deQueuedChunk))
            {
                try
                {
                    foreach (Chunk chunk in deQueuedChunk)
                    {
                        chunks.Add(chunk.location, chunk);
                        chunksinWorldGen.Remove(chunk.location);
                        chunksQueuedForWorldGen.Remove(chunk.location);

                        onDeQueueChunk(chunk);
                    }
                    deQueuedChunk.Clear();
                }
                finally
                {
                    Monitor.Exit(deQueuedChunk);
                }
            }

            //iterate through the chunks queued for world generation. If one of them is not yet being generated, queue it!
            foreach (Point point in chunksQueuedForWorldGen)
            {
                if (!chunksinWorldGen.Contains(point))
                {
                    ChunkMultithreadingHelper chunkGenerator = new ChunkMultithreadingHelper(point, deQueuedChunk, this);
                    ThreadPool.QueueUserWorkItem(chunkGenerator.runWorldGenerate, worldGenType);
                    chunksinWorldGen.Add(point);
                    break;
                }
            }


            //Reset chunk draw flag. We only want to redraw one chunk per update.
            hasRedrawnChunkThisUpdate = false;
            bool hasRemovedChunkThisUpdate = false;

            //update chunks.
            foreach (Chunk chunk in chunks.Values)
            {
                //if a chunk is within disposal distance, add it to the disposal queue
                if (Math.Abs(playerChunkLocX - chunk.location.X) > tileGenRadious * 2 || Math.Abs(playerChunkLocY - chunk.location.Y) > tileGenRadious * 2)
                {
                    if (!hasRemovedChunkThisUpdate)
                    {
                        hasRemovedChunkThisUpdate = true;
                        removeChunks.Add(chunk);
                    }

                }
                else//don't bother updating unless the chunk is within an appropriate distance
                {
                    chunk.update(time);

                    //if the chunk needs any variety of redraw, redraw it, ...
                    if (chunk.needsReDraw && !hasRedrawnChunkThisUpdate)
                    {
                        chunk.reBakeTexture();
                        //... and set the flag so that we don't redraw more than one chunk.
                        hasRedrawnChunkThisUpdate = true;
                    }

                    //if the chunk needs to have rebuilt collisions and it is not currently being rebuilt, queue it for rebuilding.
                    if (chunk.needsReBuildCollisions && !chunksQueuedForPhysicsGen.Contains(chunk.location) && !chunksinPhysicsGen.Contains(chunk.location))
                    {
                        chunksQueuedForPhysicsGen.Add(chunk.location);
                    }
                }
            }

            Point chunkPushedIntoPhysicsGen = new Point(int.MinValue, int.MinValue);
                
            //queue a chunk for physics generation if it's not already queued.
            foreach (Point point in chunksQueuedForPhysicsGen)
            {
                if (!chunksinPhysicsGen.Contains(point))
                {
                    Chunk workingOn = chunks[point];
                    ChunkMultithreadingHelper chunkGenerator = new ChunkMultithreadingHelper(point, deQueuedChunkPhysicsGen, this);
                    ThreadPool.QueueUserWorkItem(chunkGenerator.runPhysicsUpdate, workingOn);
                    chunksinPhysicsGen.Add(point);
                    chunkPushedIntoPhysicsGen = point;
                    break;
                }
            }

            if(!chunkPushedIntoPhysicsGen.Equals(new Point(int.MinValue, int.MinValue)))
            {
                chunksQueuedForPhysicsGen.Remove(chunkPushedIntoPhysicsGen);
            }

            lock(deQueuedChunkPhysicsGen)
            {
                foreach (Chunk chunk in deQueuedChunkPhysicsGen)
                {
                    chunksinPhysicsGen.Remove(chunk.location);
                }
            }
            

            //delete any disposed chunks.
            foreach (Chunk chunk in removeChunks)
            {
                onRemoveChunk(chunk);
                chunks.Remove(chunk.location);
                chunk.Dispose();
            }
            removeChunks.Clear();
        }

        public virtual void onRemoveChunk(Chunk chunk)
        {

        }

        /**
            Convert a world location to a global tile location.
        */
        public Point worldLocToTileLoc(Vector2 position)
        {
            int tileLocX = (int)Math.Floor(position.X / Chunk.tileDrawWidth);
            int tileLocY = (int)Math.Floor(position.Y / Chunk.tileDrawWidth);
            return new Point(tileLocX, tileLocY);
        }

        public void placeTile(TileType block, Vector2 position)
        {

            int chunkLocX = (int)Math.Floor(position.X / (Chunk.tilesPerChunk * Chunk.tileDrawWidth));
            int chunkLocY = (int)Math.Floor(position.Y / (Chunk.tilesPerChunk * Chunk.tileDrawWidth));

            int tileLocX = (int)Math.Floor((position.X - (chunkLocX * Chunk.tilesPerChunk * Chunk.tileDrawWidth)) / Chunk.tileDrawWidth);
            int tileLocY = (int)Math.Floor((position.Y - (chunkLocY * Chunk.tilesPerChunk * Chunk.tileDrawWidth)) / Chunk.tileDrawWidth);

            Point chunkLoc = new Point(chunkLocX, chunkLocY);
            
            if (chunks.ContainsKey(chunkLoc))
            {
                Chunk chunk = chunks[chunkLoc];
                chunk.setTile(new Point(tileLocX, tileLocY), block);
                chunk.needsReBuildCollisions = true;
                chunk.needsReDraw = true;
                //Console.WriteLine("place sucessful");
            }
        }

        public void placeBackgroundTile(TileType block, Vector2 position)
        {

            int chunkLocX = (int)Math.Floor(position.X / (Chunk.tilesPerChunk * Chunk.tileDrawWidth));
            int chunkLocY = (int)Math.Floor(position.Y / (Chunk.tilesPerChunk * Chunk.tileDrawWidth));

            int tileLocX = (int)Math.Floor((position.X - (chunkLocX * Chunk.tilesPerChunk * Chunk.tileDrawWidth)) / Chunk.tileDrawWidth);
            int tileLocY = (int)Math.Floor((position.Y - (chunkLocY * Chunk.tilesPerChunk * Chunk.tileDrawWidth)) / Chunk.tileDrawWidth);

            Point chunkLoc = new Point(chunkLocX, chunkLocY);

            if (chunks.ContainsKey(chunkLoc))
            {
                Chunk chunk = chunks[chunkLoc];
                chunk.setBackgroundTile(new Point(tileLocX, tileLocY), block);
                chunk.needsReBuildCollisions = true;
                chunk.needsReDraw = true;
            }
        }

        /*
            ...should this exist?


            note: broken
         */
        /*public void placeBlockWithTilePosition(TileType block, Point position)
        {
            int chunkLocX = position.X / Chunk.tilesPerChunk;
            int chunkLocY = position.Y / Chunk.tilesPerChunk;
            int tileLocX = position.X - (chunkLocX * Chunk.tilesPerChunk);
            int tileLocY = position.Y - (chunkLocY * Chunk.tilesPerChunk);

            Point chunkLoc = new Point(chunkLocX, chunkLocY);
            if (chunks.ContainsKey(chunkLoc))
            {
                Chunk chunk = chunks[chunkLoc];
                chunk.setTile(new Point(tileLocX, tileLocY), block);
                chunk.needsReBuildCollisions = true;
                chunk.needsReDraw = true;
            }
        }*/


        public void useBlock(Vector2 position, PlayerBase player, Item harvestTool)
        {
            int chunkLocX = (int)Math.Floor(position.X / (Chunk.tilesPerChunk * Chunk.tileDrawWidth));
            int chunkLocY = (int)Math.Floor(position.Y / (Chunk.tilesPerChunk * Chunk.tileDrawWidth));

            int tileLocX = (int)Math.Floor((position.X - (chunkLocX * Chunk.tilesPerChunk * Chunk.tileDrawWidth)) / Chunk.tileDrawWidth);
            int tileLocY = (int)Math.Floor((position.Y - (chunkLocY * Chunk.tilesPerChunk * Chunk.tileDrawWidth)) / Chunk.tileDrawWidth);

            Point chunkLoc = new Point(chunkLocX, chunkLocY);
            
            if (chunks.ContainsKey(chunkLoc))
            {
                Chunk chunk = chunks[chunkLoc];
                //TileType tile = chunk.tiles[tileLocX, tileLocY];
                TileType tile = chunk.getTile(new Point(tileLocX, tileLocY));

                foreach (TileTag tag in tile.tags)
                {
                    tag.onUse(this, harvestTool, position, tile, player);
                }
            }
        }

        public TileType getBlock(Vector2 position)
        {
            int chunkLocX = (int)Math.Floor(position.X / (Chunk.tilesPerChunk * Chunk.tileDrawWidth));
            int chunkLocY = (int)Math.Floor(position.Y / (Chunk.tilesPerChunk * Chunk.tileDrawWidth));

            int tileLocX = (int)Math.Floor((position.X - (chunkLocX * Chunk.tilesPerChunk * Chunk.tileDrawWidth)) / Chunk.tileDrawWidth);
            int tileLocY = (int)Math.Floor((position.Y - (chunkLocY * Chunk.tilesPerChunk * Chunk.tileDrawWidth)) / Chunk.tileDrawWidth);

            Point chunkLoc = new Point(chunkLocX, chunkLocY);
            if (chunks.ContainsKey(chunkLoc))
            {
                Chunk chunk = chunks[chunkLoc];
                //TileType tile = chunk.tiles[tileLocX, tileLocY];
                TileType tile = chunk.getTile(new Point(tileLocX, tileLocY));

                return tile;
            }
            return null;
        }

        public TileType getBlock(Point position)
        {
            return getBlock(position.ToVector2() * Chunk.tileDrawWidth);
        }

        public TileType getBackgroundBlock(Vector2 position)
        {
            int chunkLocX = (int)Math.Floor(position.X / (Chunk.tilesPerChunk * Chunk.tileDrawWidth));
            int chunkLocY = (int)Math.Floor(position.Y / (Chunk.tilesPerChunk * Chunk.tileDrawWidth));

            int tileLocX = (int)Math.Floor((position.X - (chunkLocX * Chunk.tilesPerChunk * Chunk.tileDrawWidth)) / Chunk.tileDrawWidth);
            int tileLocY = (int)Math.Floor((position.Y - (chunkLocY * Chunk.tilesPerChunk * Chunk.tileDrawWidth)) / Chunk.tileDrawWidth);

            Point chunkLoc = new Point(chunkLocX, chunkLocY);
            if (chunks.ContainsKey(chunkLoc))
            {
                Chunk chunk = chunks[chunkLoc];
                //TileType tile = chunk.tiles[tileLocX, tileLocY];
                TileType tile = chunk.getBackgroundTile(new Point(tileLocX, tileLocY));

                return tile;
            }
            return null;
        }

        public TileType getBackgroundBlock(Point position)
        {
            return getBackgroundBlock(position.ToVector2() * Chunk.tileDrawWidth);
        }

        public virtual void Dispose()
        {
            foreach (Chunk c in chunks.Values)
            {
                c.Dispose();
            }
        }

        protected Point totalDrawOffset;
        public virtual void draw(SpriteBatch batch, GameTime time)
        {
            totalDrawOffset = drawOffset;
            totalDrawOffset += new Point(Game1.instance.graphics.PreferredBackBufferWidth / 2, (Game1.instance.graphics.PreferredBackBufferHeight / 2));
            if(screenShakeRemaining > 0)
            {
                totalDrawOffset += getCurrentScreenShakeOffset().ToPoint();
            }
            

            foreach (Chunk chunk in chunks.Values)
            {
                chunk.draw(batch, time, totalDrawOffset, groundColor);
            }
            foreach (Particle particle in particles)
            {
                particle.draw(batch, time, totalDrawOffset, groundColor);
            }
            foreach (Entity entity in entities)
            {
                entity.draw(batch, time, totalDrawOffset, groundColor);
            }

            batch.DrawString(Game1.gamefont_24, Game1.decimalToBase6(difficulty + 1), new Vector2(Game1.instance.graphics.PreferredBackBufferWidth - 30, 20), Color.White);
            batch.DrawString(Game1.gamefont_24, Game1.decimalToBase6(MetaData.prevDifficultyReached + 1), new Vector2(Game1.instance.graphics.PreferredBackBufferWidth - 30, 50), Color.White);

            //player.drawUI(batch, time, totalDrawOffset, groundColor);
        }

        public virtual void delayedRender(SpriteBatch batch, GameTime time)
        {
            foreach (DelayedRenderable renderable in delayedRenders)
            {
                renderable.draw(batch, time, totalDrawOffset);
            }
        }


        public class ChunkMultithreadingHelper
        {
            List<Chunk> deQueue;
            public Point point;
            WorldBase world;

            public ChunkMultithreadingHelper(Point point, List<Chunk> deQueue, WorldBase world)
            {
                this.deQueue = deQueue;
                this.point = point;
                this.world = world;
            }

            public void runWorldGenerate(Object context)
            {
                Chunk chunk = new Chunk(point, world);
                chunk.generate((WorldGenType)context);

                lock (deQueue)
                {
                    deQueue.Add(chunk);
                }
            }

            public void runPhysicsUpdate(Object context)
            {
                Chunk chunk = (Chunk)context;
                chunk.reBuildCollisions();

                lock (deQueue)
                {
                    deQueue.Add(chunk);
                }
            }

        }
    }
}
