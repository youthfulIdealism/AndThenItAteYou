using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Survive.Input;
using Survive.SplashScreens;
using Survive.Worldgen;
using Survive.WorldManagement.Entities;
using Survive.WorldManagement.Entities.Particles;
using Survive.WorldManagement.Entities.Worm;
using Survive.WorldManagement.Inventory;
using Survive.WorldManagement.Inventory.Items;
using Survive.WorldManagement.Procedurals;
using Survive.WorldManagement.Tile;
using Survive.WorldManagement.Tile.Tags;
using Survive.WorldManagement.Weather;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Survive.SplashScreens.PlayerSelectScreen;

namespace Survive.WorldManagement
{
    public class World : WorldBase
    {
        public enum WorldGenSubtype { DEFERRED, STANDARD, CENOTE }
        

        //amount the time counter increments every tick.
        public const float timeIncrement = .00007f;
        //public const float timeIncrement = .0001f;
        //public const float timeIncrement = .005f;

        public int sunAxis;
        

        private List<Cloud> clouds;
        private List<Cloud> removedClouds;

        private List<FogBall> fogBalls;
        private List<FogBall> removedFogBalls;

        private List<Rain> rains;
        private List<Rain> removedRains;

        public static UniverseProperties universeProperties;
        public PerlinNoise noise { get; private set; }

        public bool generatedEndCorpse;
        public int teleporterChunkLoc;

        public World(String seed, int difficulty) : base(difficulty)
        {
            noise = new PerlinNoise(seed);
            worldGenType = WorldGenType.GENERATED;

            decorator.rollMapGenerationSubtype(this);
            decorator.generateMapTypeDependentDecorations();
            
            //queuedDecorations = new Dictionary<Rectangle, Decoration>();
            //decorator.decorationManager.generateBunkers(this);

            this.difficulty = difficulty;
            teleporterChunkLoc = Math.Min(16, difficulty * 2 + 5);
            //teleporterChunkLoc = 1;

            //set time of day, wind strength, etc
            timeOfDay = .6f;

            //set up x-location of sun in sky
            sunAxis = (int)((Game1.instance.graphics.PreferredBackBufferWidth * .1f) + rand.Next((int)(Game1.instance.graphics.PreferredBackBufferWidth * .8f)));
            generatedEndCorpse = false;

            if(decorator.worldGenSubtype != WorldGenSubtype.CENOTE)
            {
                //set up clouds
                clouds = new List<Cloud>();
                removedClouds = new List<Cloud>();
                for (int i = 0; i < decorator.weatherManager.cloudyness / 2; i++)
                {
                    int startLoc = rand.Next(Game1.instance.graphics.PreferredBackBufferWidth);
                    int cloudY = rand.Next(Game1.instance.graphics.PreferredBackBufferHeight / 2) - 300;
                    int cloudWidth = 300 + rand.Next(100);
                    int cloudHeight = 200 + rand.Next(50);
                    int subClouds = 3 + rand.Next(6);
                    for (int k = 0; k < subClouds; k++)
                    {
                        AABB newCloudBounds = new AABB(startLoc + rand.Next(cloudWidth), cloudY + rand.Next(cloudHeight), cloudWidth / 2, cloudHeight / 2);
                        clouds.Add(new Cloud(newCloudBounds, rand));
                    }
                }

                fogBalls = new List<FogBall>();
                removedFogBalls = new List<FogBall>();

                rains = new List<Rain>();
                removedRains = new List<Rain>();
            }
            

            //put player in world
            player = new Player(new Vector2(40, noise.octavePerlin1D(0) * decorator.getTerrainMultiplier() * Chunk.tileDrawWidth), this);

            //addEntity(new WormHead(new Vector2(40, noise.octavePerlin1D(0) * decorator.getTerrainMultiplier() * Chunk.tileDrawWidth), this));

            entities.Add(player);
            //entities.Add(new EntityCentipedeHead(new Vector2(40, noise.octavePerlin1D(0) * decorator.getTerrainMultiplier() * Chunk.tileDrawWidth), this));

            groundColor = decorator.colorManager.groundColor;
        }

        public override void switchTo()
        {
            base.switchTo();
            Game1.instance.queuedSplashScreens.Add(new ProgressSplash(difficulty));
            if(player is Player)
            {
                ((Player)player).timeRemainingForSpawnParticles = Player.maxTimeRemainingForSpawnParticles;
            }
        }

        /*public void queueDecoration(Point tileLocation, Decoration decoration)
        {
            queuedDecorations.Add(new Rectangle(decoration.box.X + tileLocation.X, decoration.box.Y + tileLocation.Y, decoration.box.Width, decoration.box.Height), decoration);
        }*/

        public override void update(GameTime time)
        {
            base.update(time);

            float lastTimeOfDay = timeOfDay;

            //increase time of day. If time of day wraps over into night, reset it, because 0 is midnight.
            timeOfDay += timeIncrement;
            if(timeOfDay >= 2)
            {
                timeOfDay = 0;
            }

            if (pauseRequests <= 0)
            {
                decorator.ambientSoundManager.manageAmbientSound(timeOfDay);
                handleWeather();
            }
           

            performChunkManagement(time);
        }

        private void handleWeather()
        {
            if (decorator.worldGenSubtype != WorldGenSubtype.CENOTE)
            {
                float cloudyness = decorator.weatherManager.cloudyness;
                float windStrength = decorator.weatherManager.windStrength;
                float rainyness = decorator.weatherManager.rainyness;


                //on random chance, generate a new cloud.
                if (clouds.Count < cloudyness)
                {
                    int startLoc = 0;


                    int cloudY = rand.Next(Game1.instance.graphics.PreferredBackBufferHeight / 2) - 300;
                    int cloudWidth = 300 + rand.Next(100);
                    int cloudHeight = 200 + rand.Next(50);
                    int subClouds = 3 + rand.Next(6);
                    if (windStrength > 0)
                    {
                        startLoc = -cloudWidth - 30;
                    }
                    else
                    {
                        startLoc = Game1.instance.graphics.PreferredBackBufferWidth + cloudWidth + 30;
                    }

                    for (int i = 0; i < subClouds; i++)
                    {
                        AABB newCloudBounds = new AABB(startLoc + rand.Next(cloudWidth), cloudY + rand.Next(cloudHeight), cloudWidth / 2, cloudHeight / 2);
                        clouds.Add(new Cloud(newCloudBounds, rand));
                    }
                }


                foreach (Cloud cloud in clouds)
                {
                    cloud.shift(windStrength * (1 - (float)cloud.bounds.Y / (Game1.instance.graphics.PreferredBackBufferHeight * .5f)));
                    if (windStrength > 0)
                    {
                        if (cloud.bounds.X > Game1.instance.graphics.PreferredBackBufferWidth)
                        {
                            removedClouds.Add(cloud);
                        }
                    }
                    else if (cloud.bounds.X < -cloud.bounds.Width)
                    {
                        removedClouds.Add(cloud);
                    }
                }
                foreach (Cloud cloud in removedClouds)
                {
                    clouds.Remove(cloud);
                }
                removedClouds.Clear();

                if (decorator.weatherManager.weather == WeatherManager.Weather.WINDY || decorator.weatherManager.weather == WeatherManager.Weather.STORMY)
                {
                    if (rand.NextDouble() < .5f)
                    {
                        Vector2 loc = new Vector2(playerLoc.X + rand.Next(Game1.instance.graphics.PreferredBackBufferWidth) - Game1.instance.graphics.PreferredBackBufferWidth / 2, playerLoc.Y + rand.Next(Game1.instance.graphics.PreferredBackBufferHeight) - (Game1.instance.graphics.PreferredBackBufferHeight + 400) / 2);
                        TileType tile = getBlock(loc);
                        if (tile != null && !tile.tags.Contains(TagReferencer.SOLID))
                        {
                            ParticleTileBreak particle = new ParticleTileBreak(loc, this, new Vector2(windStrength * 3, 0), tile, 200);
                            particle.gravityMultiplier = .07f;
                            particle.frictionMultiplier = .97f; //actually sets friction higher for this particle
                            particle.velocity = new Vector2(windStrength, 0);
                            addEntity(particle);
                        }

                    }

                    foreach (Particle particle in particles)
                    {
                        particle.velocity += new Vector2(windStrength * .1f, 0);
                    }

                    foreach (Entity entity in entities)
                    {
                        if (!entity.collideBottom)
                        {
                            entity.impulse += new Vector2(windStrength * .1f * entity.windMultiplier, 0);
                        }
                        else
                        {
                            entity.impulse += new Vector2(windStrength * .01f * entity.windMultiplier, 0);
                        }

                    }
                }
                else if (decorator.weatherManager.weather == WeatherManager.Weather.FOGGY)
                {
                    int numFogBallsOnScreen = 70;

                    if (fogBalls.Count < numFogBallsOnScreen)
                    {
                        Vector2 loc = new Vector2(playerLoc.X + rand.Next(Game1.instance.graphics.PreferredBackBufferWidth + 400) - (Game1.instance.graphics.PreferredBackBufferWidth + 200) / 2, playerLoc.Y + rand.Next(Game1.instance.graphics.PreferredBackBufferHeight) - (Game1.instance.graphics.PreferredBackBufferHeight) / 2);
                        int size = 400 + rand.Next(200);
                        AABB newFogBounds = new AABB(loc.X, loc.Y, size, size);
                        fogBalls.Add(new FogBall(newFogBounds, rand));
                    }

                    AABB screenBounds = new AABB(playerLoc.X - Game1.instance.graphics.PreferredBackBufferWidth / 2 - 400, playerLoc.Y - Game1.instance.graphics.PreferredBackBufferHeight / 2 - 200, Game1.instance.graphics.PreferredBackBufferWidth + 800, Game1.instance.graphics.PreferredBackBufferHeight + 400);
                    foreach (FogBall fog in fogBalls)
                    {
                        fog.update();
                        if (!fog.bounds.Intersects(screenBounds)) { removedFogBalls.Add(fog); }
                    }

                    foreach (FogBall fog in removedFogBalls)
                    {
                        fogBalls.Remove(fog);
                    }
                    removedFogBalls.Clear();
                }
                else if (decorator.weatherManager.weather == WeatherManager.Weather.RAINY)
                {
                    if (rand.NextDouble() < .3)
                    {
                        Vector2 loc = new Vector2(playerLoc.X + rand.Next(Game1.instance.graphics.PreferredBackBufferWidth + 400) - (Game1.instance.graphics.PreferredBackBufferWidth + 200) / 2, playerLoc.Y - (Game1.instance.graphics.PreferredBackBufferHeight) / 2 - 100);
                        addEntity(new ParticleRain(loc, this, new Vector2(0, 1f)));
                    }
                }
            }
        }

        protected override void handleCritterSpawn(Chunk chunk)
        {
            //at this point, we can safely generate any critters that shouls spawn with the chunk.
            if (this.spawnsEnemies && !chunksSpawnedEntities.Contains(chunk.location.X))
            {
                for(int i = 0; i < 3; i++)
                {
                    if(rand.NextDouble() < decorator.spawnCritterChance)
                    {
                        //find the critter a random tile to stand on top of
                        int critterSpawnLoc = rand.Next(Chunk.tilesPerChunk);
                        //find the height of the tile (plus a little--we still need to bake the chunk's physics!)
                        //float groundLevel = noise.octavePerlin1D((float)(chunk.location.X * Chunk.tilesPerChunk + critterSpawnLoc) / 25) * decorator.getTerrainMultiplier() * Chunk.tileDrawWidth/* + seaLevel - 700*/;
                        float groundLevel = noise.octavePerlin1D((float)(chunk.location.X * Chunk.tilesPerChunk + critterSpawnLoc) / 25) * decorator.getTerrainMultiplier() * Chunk.tileDrawWidth;
                        Vector2 spawnLoc = new Vector2(((chunk.location.X * Chunk.tilesPerChunk + critterSpawnLoc) * Chunk.tileDrawWidth), groundLevel);
                        Entity critter = decorator.getCritterForChunk(spawnLoc);
                        critter.location = critter.location - new Vector2(0, critter.height);
                        this.addEntity(critter);
                        chunksSpawnedEntities.Add(chunk.location.X);
                    }
                }
            }
        }

        public override void onDeQueueChunk(Chunk chunk)
        {
            base.onDeQueueChunk(chunk);

            /*foreach(Rectangle rect in queuedDecorations.Keys)
            {
                Console.WriteLine(rect + " vs " + chunk.tileBox + " :: " + rect.Intersects(chunk.tileBox) + " | " + chunk.tileBox.Intersects(rect));
                if(rect.Intersects(chunk.tileBox))
                {
                    Console.WriteLine("DECORATING THE CRAP OUT OF THINGS");
                    Decoration decoration = queuedDecorations[rect];
                    Rectangle intersectionRectangle = Rectangle.Intersect(chunk.tileBox, rect);

                    for (int nx = 0; nx < intersectionRectangle.Width; nx++)
                    {
                        for (int ny = 0; ny < intersectionRectangle.Height; ny++)
                        {
                            Point placeTileLoc = new Point((intersectionRectangle.Left + nx) * Chunk.tileDrawWidth, (intersectionRectangle.Top + ny) * Chunk.tileDrawWidth);
                            int getTileFromX = intersectionRectangle.Left - rect.Left + nx;
                            int getTileFromY = rect.Bottom - intersectionRectangle.Bottom  + ny;
                            if (decoration.decorationMap[getTileFromX, getTileFromY] != null)
                            {
                                placeTile(decoration.decorationMap[getTileFromX, getTileFromY], new Vector2(placeTileLoc.X, placeTileLoc.Y));
                            }

                            if (decoration.backgroundDecorationMap != null && decoration.backgroundDecorationMap[getTileFromX, getTileFromY] != null)
                            {
                                placeBackgroundTile(decoration.backgroundDecorationMap[getTileFromX, getTileFromY], new Vector2(placeTileLoc.X, placeTileLoc.Y));
                            }
                            
                            

                        }
                    }
                }


            }*/


            


            if (Math.Abs(chunk.location.X) == teleporterChunkLoc)
            {
                float tileGroundLevel = (noise.octavePerlin1D((float)(chunk.location.X * Chunk.tilesPerChunk) / 25) * decorator.getTerrainMultiplier() * Chunk.tileDrawWidth);
                this.placeTile(TileTypeReferencer.TELEPORTER_0, new Vector2(chunk.location.X * Chunk.tilesPerChunk * Chunk.tileDrawWidth, tileGroundLevel));
                this.placeTile(TileTypeReferencer.TELEPORTER_1, new Vector2(chunk.location.X * Chunk.tilesPerChunk * Chunk.tileDrawWidth, tileGroundLevel - Chunk.tileDrawWidth));
                this.placeTile(TileTypeReferencer.TELEPORTER_2, new Vector2(chunk.location.X * Chunk.tilesPerChunk * Chunk.tileDrawWidth, tileGroundLevel - Chunk.tileDrawWidth * 2));
                this.placeTile(TileTypeReferencer.TELEPORTER_3, new Vector2(chunk.location.X * Chunk.tilesPerChunk * Chunk.tileDrawWidth, tileGroundLevel - Chunk.tileDrawWidth * 3));
                this.placeTile(TileTypeReferencer.TELEPORTER_4, new Vector2(chunk.location.X * Chunk.tilesPerChunk * Chunk.tileDrawWidth, tileGroundLevel - Chunk.tileDrawWidth * 4));
                this.placeTile(TileTypeReferencer.TELEPORTER_5, new Vector2(chunk.location.X * Chunk.tilesPerChunk * Chunk.tileDrawWidth, tileGroundLevel - Chunk.tileDrawWidth * 5));
                this.addEntity(new EntityTeleporterAmbiance(new Vector2(chunk.location.X * Chunk.tilesPerChunk * Chunk.tileDrawWidth, tileGroundLevel), this));
            }
        }

        /**
            Perform a crude fast forward of the world. Intended for use if time goes by.
        */
        public override void fastForward(float timeAsPercentageOfDay)
        {
            base.fastForward(timeAsPercentageOfDay);
            timeOfDay += timeAsPercentageOfDay;
            if (timeOfDay >= 2)
            {
                timeOfDay = 0;
            }

            foreach (Entity entity in entities)
            {
                entity.fastForward(timeAsPercentageOfDay);
            }
        }

        public override float getCurrentTemperature()
        {
            if(timeOfDay < 1)
            {
                return decorator.nightTemp * (1 - timeOfDay) + decorator.dayTemp * timeOfDay;
            }
            else if(timeOfDay < 2)
            {
                return decorator.dayTemp * (2 - timeOfDay) + decorator.nightTemp * (timeOfDay - 1);
            }
            return 0;
        }



        /**
            Returns the AABB resulting from attempting to move to a destination, modified by the physics of a chunk.

            Don't move your entity directly! set its location based on the results of this call.

            TODO: There's an edge-case physics glitch which will a: fling the player high into the air, or b: cause small objects to fall through the ground. Fix it.
        */
        public override AABB tryMove(Entity entity, AABB destination)
        {
            AABB currentEntityRect = entity.getCollisionBox();

            AABB movementRect = AABB.Union(currentEntityRect, destination);

            //ensure that an entity moving through an unloaded chunk does not, in fact, move
            foreach (Point point in chunksQueuedForWorldGen)
            {
                Rectangle chunkRect = new Rectangle(point.X * Chunk.tilesPerChunk * Chunk.tileDrawWidth, point.Y * Chunk.tilesPerChunk * Chunk.tileDrawWidth, Chunk.tilesPerChunk * Chunk.tileDrawWidth, Chunk.tilesPerChunk * Chunk.tileDrawWidth);
                if (movementRect.Intersects(chunkRect))
                {
                    return currentEntityRect;
                }
            }

            foreach (Point point in chunksinWorldGen)
            {
                Rectangle chunkRect = new Rectangle(point.X * Chunk.tilesPerChunk * Chunk.tileDrawWidth, point.Y * Chunk.tilesPerChunk * Chunk.tileDrawWidth, Chunk.tilesPerChunk * Chunk.tileDrawWidth, Chunk.tilesPerChunk * Chunk.tileDrawWidth);
                if (movementRect.Intersects(chunkRect))
                {
                    return currentEntityRect;
                }
            }

            foreach (Chunk chunk in chunks.Values)
            {
                //check to ensure that the chunk is close enough to entity to affect physics. If not, skip the chunk.
                if (movementRect.Intersects(chunk.totalBox))
                {
                    //iterate through the collision boxes, to see if any of them affect physics.
                    foreach (Rectangle chunkRect in chunk.collisionBoxes)
                    {
                        //if the movement box consumes by the entity's location... TODO: add reverse-contains for aabb
                        if (chunkRect.Contains(destination.ToRect()))
                        {
                            destination = new AABB(destination.X, chunkRect.Y - destination.Height, destination.Width, destination.Height);
                        }
                        else if (movementRect.Intersects(chunkRect))//if the collision box intersects the entity's intended location, ...
                        {
                            AABB intersection = AABB.Intersect(movementRect, chunkRect);



                            if (intersection.Height > intersection.Width)//the collision is on the side
                            {

                                if (destination.Center.X > intersection.Center.X)
                                {
                                    entity.collideRight = true;
                                    destination = new AABB(intersection.Right, destination.Y, destination.Width, destination.Height);



                                }
                                else
                                {
                                    entity.collideLeft = true;
                                    destination = new AABB(intersection.Left - destination.Width, destination.Y, destination.Width, destination.Height);




                                }



                            }
                            else//if(interection.Height < interection.Width)
                            {


                                if (destination.Center.Y > intersection.Center.Y)
                                {
                                    entity.collideTop = true;
                                    destination = new AABB(destination.X, intersection.Y + intersection.Height, destination.Width, destination.Height);



                                }
                                else
                                {
                                    entity.collideBottom = true;
                                    destination = new AABB(destination.X, intersection.Y - destination.Height, destination.Width, destination.Height);




                                }



                            }

                            movementRect = AABB.Union(currentEntityRect, destination);














































                        }
                    }
                }
            }

            return destination;
        }

        public virtual void audioShock()
        {

        }

        public override void draw(SpriteBatch batch, GameTime time)
        {
            Color skyColor = decorator.colorManager.getSkyColorGivenTimeOfDay(timeOfDay);

            batch.Draw(Game1.block, new Rectangle(0, 0, Game1.instance.graphics.PreferredBackBufferWidth, Game1.instance.graphics.PreferredBackBufferHeight), skyColor);

            drawWeatherUnderLayer(batch, time);

            base.draw(batch, time);

            drawWeatherOverLayer(batch, time);

            delayedRender(batch, time);
        }

        private void drawWeatherUnderLayer(SpriteBatch batch, GameTime time)
        {
            if (decorator.worldGenSubtype != WorldGenSubtype.CENOTE)
            {
                int sunY = (int)(Game1.instance.graphics.PreferredBackBufferHeight - Game1.instance.graphics.PreferredBackBufferHeight * (timeOfDay));
                batch.Draw(Game1.texture_sun, new Rectangle(sunAxis, sunY, 300, 300), Color.White);

                float starRenderAmt = 0;
                if (timeOfDay < 1)
                {
                    starRenderAmt = -(1 - timeOfDay) + timeOfDay;
                }
                else if (timeOfDay < 2)
                {
                    starRenderAmt = (2 - timeOfDay) + -(timeOfDay - 1);
                }
                starRenderAmt = 1 - starRenderAmt;
                starRenderAmt *= starRenderAmt * starRenderAmt;
                batch.Draw(Game1.texture_sky, new Rectangle(0, 0, Game1.instance.graphics.PreferredBackBufferWidth, Game1.instance.graphics.PreferredBackBufferHeight), Color.White * starRenderAmt);

                Color backCloudColor = decorator.colorManager.getBackCloudColorGivenTimeOfDay(timeOfDay);
                Color frontCloudColor = decorator.colorManager.getFrontCloudColorGivenTimeOfDay(timeOfDay);
                int cloudBorderSize = 7;

                foreach (Cloud cloud in clouds)
                {
                    batch.Draw(cloud.textureFront, cloud.bounds.ToRect(), backCloudColor);
                }

                foreach (Cloud cloud in clouds)
                {
                    Rectangle innerDrawRect = cloud.bounds.ToRect();
                    innerDrawRect.Inflate(-cloudBorderSize / 2, -cloudBorderSize);
                    innerDrawRect.X += (int)((innerDrawRect.X - sunAxis) * .05f);
                    innerDrawRect.Y += (int)Math.Max(-cloudBorderSize, Math.Min(cloudBorderSize, (((float)(innerDrawRect.Y - sunY)))));

                    batch.Draw(cloud.textureBack, innerDrawRect, Color.Lerp(frontCloudColor, Color.White, (float)innerDrawRect.Y / Game1.instance.graphics.PreferredBackBufferHeight));
                }
                
                batch.Draw(Game1.texture_sun, new Rectangle(sunAxis + 25, sunY + 25, 250, 250), Color.White * .7f);//draw a second so that it looks like some sun comes through the clouds

                if (decorator.weatherManager.weather == WeatherManager.Weather.RAINY)
                {
                    foreach (Rain rain in rains)
                    {
                        if (rain.completion <= 1)
                        {
                            batch.Draw(Game1.rain,
                               new Rectangle((int)rain.location.X, (int)rain.location.Y, (int)10, (int)(Game1.rain.Height * rain.completion)),
                               new Rectangle(0, 0, 10, (int)(Game1.rain.Height * rain.completion)),
                               groundColor,
                               (float)(Math.Atan(rain.angle.Y / rain.angle.X) - Math.PI / 2),
                               Vector2.Zero, SpriteEffects.None, 0);
                        }
                        else
                        {
                            batch.Draw(Game1.rain,
                               new Rectangle((int)rain.location.X, (int)rain.location.Y, (int)10, (int)(Game1.rain.Height)),
                               new Rectangle(0, 0, 10, (int)(Game1.rain.Height)),
                               groundColor * (2 - rain.completion),
                               (float)(Math.Atan(rain.angle.Y / rain.angle.X) - Math.PI / 2),
                               Vector2.Zero, SpriteEffects.None, 0);
                        }

                    }
                }
            }
        }

        private void drawWeatherOverLayer(SpriteBatch batch, GameTime time)
        {
            if (decorator.worldGenSubtype != WorldGenSubtype.CENOTE)
            {
                if (decorator.weatherManager.weather == WeatherManager.Weather.FOGGY)
                {
                    Color skyColor = decorator.colorManager.getSkyColorGivenTimeOfDay(timeOfDay);
                    batch.Draw(Game1.block, new Rectangle(0, 0, Game1.instance.graphics.PreferredBackBufferWidth, Game1.instance.graphics.PreferredBackBufferHeight), skyColor * .7f);

                    foreach (FogBall fog in fogBalls)
                    {
                        batch.Draw(Game1.fog,
                            new Rectangle((int)(fog.bounds.X + totalDrawOffset.X), (int)(fog.bounds.Y + totalDrawOffset.Y), (int)fog.bounds.Width, (int)fog.bounds.Height),
                            null,
                            skyColor * fog.timeExisted,
                            fog.rotation,
                            /*Vector2.Zero*/new Vector2(Game1.fog.Width / 2, Game1.fog.Height / 2), SpriteEffects.None, 0);
                    }
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            decorator.Dispose();
        }
    }
}
