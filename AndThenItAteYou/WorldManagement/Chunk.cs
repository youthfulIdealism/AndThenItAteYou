using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Survive.Worldgen;
using Survive.WorldManagement.Procedurals;
using Survive.WorldManagement.Tile;
using Survive.WorldManagement.Tile.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement
{
    public class Chunk : IDisposable
    {
        public const int tilesPerChunk = 20;
        public const int tileDrawWidth = 50;
        public bool needsReDraw { get; set; }
        public bool needsReBuildCollisions { get; set; }
        public static SpriteBatch chunkRenderBatch { get; set; }


        //0 = air.
        //1 = ground.
        //2 = cave
        //3 = rope
        //TODO: replace this with an array of tile objects.
        private TileType[,] tiles;
        private TileType[,] backgroundTiles;
        public Point location;
        WorldBase world;
        RenderTarget2D texture;
        public List<Rectangle> collisionBoxes;
        public List<Rectangle> collisionBoxWorkBuffer;
        public Rectangle totalBox { get; private set; }
        public Rectangle tileBox { get; private set; }
        Random random;

        bool generated = false;

        public Chunk(Point loc, WorldBase world)
        {
            if(chunkRenderBatch == null)
            {
                chunkRenderBatch = new SpriteBatch(Game1.instance.GraphicsDevice);
            }

            this.location = loc;
            this.world = world;
            tiles = new TileType[tilesPerChunk, tilesPerChunk];
            backgroundTiles = new TileType[tilesPerChunk, tilesPerChunk];
            needsReDraw = true;
            needsReBuildCollisions = true;

            collisionBoxes = new List<Rectangle>();
            collisionBoxWorkBuffer = new List<Rectangle>();
            totalBox = new Rectangle(location.X * tilesPerChunk * tileDrawWidth, location.Y * tilesPerChunk * tileDrawWidth, tilesPerChunk * tileDrawWidth, tilesPerChunk * tileDrawWidth);
            tileBox = new Rectangle(location.X * tilesPerChunk, location.Y * tilesPerChunk, tilesPerChunk, tilesPerChunk);
        }

        public void generate(WorldBase.WorldGenType type)
        {
            lock (tiles)
            {
                if (type.Equals(WorldBase.WorldGenType.BLANK))
                {
                    generateBlank();
                }
                else if (type.Equals(WorldBase.WorldGenType.GENERATED))
                {
                    
                    if(world is World)
                    {
                        if (world.decorator.worldGenSubtype == World.WorldGenSubtype.CENOTE)
                        {
                            generateProceduralCenote();
                        }
                        else
                        {
                            generateProcedural();
                        }
                    }else
                    {
                        generateProcedural();
                    }
                    
                }
                else if (type.Equals(WorldBase.WorldGenType.LOAD))
                {
                    generateLoad();
                }
            }
        }

        public void generateProcedural()
        {
            //TODO: remove cast
            World world = (World)this.world;
            if (generated)
            {
                Logger.log("Procedural chunk generation threading error");
            }
            generated = true;

            random = new Random(location.X * 500 + location.Y);
            Dictionary<Point, Decoration> queuedDecorations = new Dictionary<Point, Decoration>();
            int terrainMultipler = world.decorator.getTerrainMultiplier();
            float caveThreshold = world.decorator.caveThreshold;

            PerlinNoise perlin = world.noise;
            for (int x = 0; x < tilesPerChunk; x++)
            {
                float groundLevel = perlin.octavePerlin1D((float)(location.X * tilesPerChunk + x) / 25) * terrainMultipler;
                for (int y = 0; y < tilesPerChunk; y++)
                {
                    if ((location.Y * tilesPerChunk + y) > groundLevel)
                    {
                        float current = perlin.octavePerlin((float)(location.X * tilesPerChunk + x) / 25, (float)(location.Y * tilesPerChunk + y) / 25);

                        if (current > caveThreshold || location.X == 0 || location.X == -1)
                        {
                            tiles[x, y] = TileTypeReferencer.DIRT;
                            backgroundTiles[x, y] = TileTypeReferencer.CAVE;
                        }
                        else
                        {
                            backgroundTiles[x, y] = TileTypeReferencer.CAVE;
                            tiles[x, y] = TileTypeReferencer.AIR;
                            if ((location.Y * tilesPerChunk + y) > groundLevel + 5 && random.NextDouble() < .23 && perlin.octavePerlin((float)(location.X * tilesPerChunk + x) / 25, (float)(location.Y * tilesPerChunk + y + 1) / 25) > caveThreshold)
                            {
                                Decoration foliage = world.decorator.getSubTerranianFoliage(this, x, y);

                                if (new Rectangle(0, 0, tilesPerChunk, tilesPerChunk).Contains(new Rectangle(x + foliage.box.X, y + foliage.box.Y, foliage.box.Width, foliage.box.Height)))
                                {
                                    queuedDecorations.Add(new Point(x, y), foliage);
                                }
                            }
                        }

                    }
                    else
                    {
                        if ((location.Y * tilesPerChunk + y) <= groundLevel + 1 && (location.Y * tilesPerChunk + y) > groundLevel - 1)
                        {
                            float currentDown = perlin.octavePerlin((float)(location.X * tilesPerChunk + x) / 25, (float)(location.Y * tilesPerChunk + y + 1) / 25);
                            if (currentDown > caveThreshold)
                            {
                                backgroundTiles[x, y] = world.decorator.shrubManager.grass;
                                if (random.NextDouble() < world.decorator.foliageChance)
                                {
                                    Decoration foliage = world.decorator.getFoliage(this, x, y);

                                    if (new Rectangle(0, 0, tilesPerChunk, tilesPerChunk).Contains(new Rectangle(x + foliage.box.X, y + foliage.box.Y, foliage.box.Width, foliage.box.Height)))
                                    {
                                        queuedDecorations.Add(new Point(x, y), foliage);
                                    }
                                }
                            }
                        }
                        tiles[x, y] = TileTypeReferencer.AIR;


                    }
                }
            }

            foreach (Point point in queuedDecorations.Keys)
            {
                Decoration decoration = queuedDecorations[point];
                for (int nx = 0; nx < decoration.box.Width; nx++)
                {
                    for (int ny = 0; ny < decoration.box.Height; ny++)
                    {
                        if (decoration.decorationMap[nx, ny] != null)
                        {
                            tiles[point.X + decoration.box.X + nx, point.Y + decoration.box.Y + ny] = decoration.decorationMap[nx, ny];
                        }

                        if(decoration.backgroundDecorationMap != null && decoration.backgroundDecorationMap[nx, ny] != null)
                        {
                            backgroundTiles[point.X + decoration.box.X + nx, point.Y + decoration.box.Y + ny] = decoration.backgroundDecorationMap[nx, ny];
                        }


                    }
                }
            }

            reBuildCollisions();
        }

        public void generateProceduralCenote()
        {
            //TODO: remove cast
            World world = (World)this.world;
            if (generated)
            {
                Logger.log("Procedural chunk generation threading error");
            }
            generated = true;

            random = new Random(location.X * 500 + location.Y);
            Dictionary<Point, Decoration> queuedDecorations = new Dictionary<Point, Decoration>();
            int terrainMultipler = world.decorator.getTerrainMultiplier();
            float caveThreshold = world.decorator.caveThreshold;
            float waterLevel = world.decorator.waterLevel;

            PerlinNoise perlin = world.noise;
            for (int x = 0; x < tilesPerChunk; x++)
            {
                float groundLevel = perlin.octavePerlin1D((float)(location.X * tilesPerChunk + x) / 25) * terrainMultipler;
                float cielingLevel = Math.Min(groundLevel - 5 + perlin.octavePerlin1D((float)(location.X * tilesPerChunk + x + 200) / 25) * terrainMultipler, groundLevel - 5);
                
                for (int y = 0; y < tilesPerChunk; y++)
                {
                    float totalY = (location.Y * tilesPerChunk + y);
                    if (totalY > groundLevel || totalY < cielingLevel)
                    {
                        float current = perlin.octavePerlin((float)(location.X * tilesPerChunk + x) / 25, (float)(location.Y * tilesPerChunk + y) / 25);

                        if (current > caveThreshold || location.X == 0 || location.X == -1)
                        {
                            tiles[x, y] = TileTypeReferencer.DIRT;
                        }
                        else
                        {
                            if(totalY > waterLevel)
                            {
                                tiles[x, y] = TileTypeReferencer.WATER;
                            }
                            else
                            {
                                tiles[x, y] = TileTypeReferencer.AIR;
                            }
                        }

                    }
                    else
                    {
                        if (totalY > waterLevel)
                        {
                            tiles[x, y] = TileTypeReferencer.WATER;
                        }
                        else
                        {
                            tiles[x, y] = TileTypeReferencer.AIR;
                        }
                    }

                    //coat the floor in floor
                    if(tiles[x,y] == TileTypeReferencer.AIR)
                    {
                        float currentDown = perlin.octavePerlin((float)(location.X * tilesPerChunk + x) / 25, (float)(location.Y * tilesPerChunk + y + 1) / 25);
                        if (currentDown > caveThreshold && (totalY + 1 > groundLevel || totalY + 1 < cielingLevel) && totalY <= waterLevel)
                        {
                            tiles[x, y] = TileTypeReferencer.CAVE_FLOOR;

                            if (random.NextDouble() < world.decorator.foliageChance)
                            {
                                Decoration foliage = world.decorator.getFoliage(this, x, y);

                                if (new Rectangle(0, 0, tilesPerChunk, tilesPerChunk).Contains(new Rectangle(x + foliage.box.X, y + foliage.box.Y, foliage.box.Width, foliage.box.Height)))
                                {
                                    queuedDecorations.Add(new Point(x, y), foliage);
                                }
                            }
                        }
                    }

                    backgroundTiles[x, y] = TileTypeReferencer.CAVE;
                }
            }

            foreach (Point point in queuedDecorations.Keys)
            {
                Decoration decoration = queuedDecorations[point];
                for (int nx = 0; nx < decoration.box.Width; nx++)
                {
                    for (int ny = 0; ny < decoration.box.Height; ny++)
                    {
                        if (decoration.decorationMap[nx, ny] != null)
                        {
                            tiles[point.X + decoration.box.X + nx, point.Y + decoration.box.Y + ny] = decoration.decorationMap[nx, ny];
                        }

                        if (decoration.backgroundDecorationMap != null && decoration.backgroundDecorationMap[nx, ny] != null)
                        {
                            backgroundTiles[point.X + decoration.box.X + nx, point.Y + decoration.box.Y + ny] = decoration.backgroundDecorationMap[nx, ny];
                        }


                    }
                }
            }

            reBuildCollisions();
        }

        public void generateBlank()
        {
            if (generated)
            {
                Logger.log("threading error in generate chunk blank");
            }
            generated = true;

            for (int x = 0; x < tilesPerChunk; x++)
            {
                for (int y = 0; y < tilesPerChunk; y++)
                {
                    tiles[x, y] = TileTypeReferencer.AIR;
                }
            }

            reBuildCollisions();
        }

        public void generateLoad()
        {
            if (generated)
            {
                Logger.log("threading error in generate chunk from load");
            }
            Decoration mapData = world.worldReader.readChunk(location);
            if(mapData != null)
            {
                generated = true;
                for (int x = 0; x < tilesPerChunk; x++)
                {
                    for (int y = 0; y < tilesPerChunk; y++)
                    {
                        tiles[x, y] = mapData.decorationMap[x, y];
                        backgroundTiles[x, y] = mapData.backgroundDecorationMap[x, y];
                    }
                }
                reBuildCollisions();
            }
            else
            {
                generateBlank();
            }

            

            
        }

        public void update(GameTime time)
        {
            lock (collisionBoxWorkBuffer)
            {
                if(collisionBoxWorkBuffer.Count > 0)
                {
                    collisionBoxes = collisionBoxWorkBuffer;
                    collisionBoxWorkBuffer = new List<Rectangle>();
                }
                
            }
        }
        
        public void reBuildCollisions()
        {
            lock (tiles)
            {
                HashSet<Rectangle> temp = new HashSet<Rectangle>();
                //accumulate rectangles from left to right to minimize collision rectangle count
                for (int y = 0; y < tilesPerChunk; y++)
                {
                    for (int x = 0; x < tilesPerChunk; x++)
                    {
                        if (tiles[x, y].tags.Contains(TagReferencer.SOLID))
                        {
                            int startX = x;
                            int endX = x;


                            while (endX < tilesPerChunk && tiles[endX, y].tags.Contains(TagReferencer.SOLID))
                            {
                                if (endX < tilesPerChunk)
                                {
                                    endX++;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            int worldX = (location.X * tilesPerChunk + startX) * tileDrawWidth;
                            int width = (endX - startX) * tileDrawWidth;
                            int worldY = (location.Y * tilesPerChunk + y) * tileDrawWidth;
                            temp.Add(new Rectangle(worldX, worldY, width, tileDrawWidth));
                            x = endX;
                        }


                    }
                }

                lock (collisionBoxWorkBuffer)
                {
                    collisionBoxWorkBuffer = temp.ToList();
                }

                needsReBuildCollisions = false;
            }
        }

        public void reBakeTexture()
        {
            //ensure that we perform ONLY a redraw OR a rebackdraw
            if (needsReDraw)
            {
                lock (tiles)
                {
                    if (texture != null) { texture.Dispose(); }
                    texture = new RenderTarget2D(
                        Game1.instance.GraphicsDevice,
                        tileDrawWidth * tilesPerChunk,
                        tileDrawWidth * tilesPerChunk,
                        false,
                        Game1.instance.GraphicsDevice.PresentationParameters.BackBufferFormat,
                        DepthFormat.Depth24);

                    Game1.instance.GraphicsDevice.SetRenderTarget(texture);

                    // Draw the scene
                    Game1.instance.GraphicsDevice.Clear(Color.Transparent);

                    chunkRenderBatch.Begin();

                    for (int x = 0; x < tilesPerChunk; x++)
                    {
                        for (int y = 0; y < tilesPerChunk; y++)
                        {
                            if (backgroundTiles[x, y] != null)
                            {
                                backgroundTiles[x, y].draw(chunkRenderBatch, new Point(x, y), Color.White);
                            }
                            tiles[x, y].draw(chunkRenderBatch, new Point(x, y), Color.White);
                        }
                    }

                    chunkRenderBatch.End();

                    Game1.instance.GraphicsDevice.SetRenderTarget(null);

                    needsReDraw = false;
                }
            }
        }

        public void setTile(Point loc, TileType type)
        {
            lock (tiles)
            {
                tiles[loc.X, loc.Y] = type;
            }
        }

        public TileType getTile(Point loc)
        {
            lock (tiles)
            {
                return tiles[loc.X, loc.Y];
            }
        }

        public void setBackgroundTile(Point loc, TileType type)
        {
            lock (backgroundTiles)
            {
                backgroundTiles[loc.X, loc.Y] = type;
            }
        }

        public TileType getBackgroundTile(Point loc)
        {
            lock (backgroundTiles)
            {
                return backgroundTiles[loc.X, loc.Y];
            }
        }

        public void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            if(texture != null)
            {
                batch.Draw(texture, new Rectangle(location.X * tileDrawWidth * tilesPerChunk + offset.X, location.Y * tileDrawWidth * tilesPerChunk + offset.Y, tileDrawWidth * tilesPerChunk, tileDrawWidth * tilesPerChunk), groundColor);
                //
                /*foreach (Rectangle rect in collisionBoxes)
                {
                    batch.Draw(Game1.block, new Rectangle(rect.X + offset.X + 2, rect.Y + offset.Y + 2, rect.Width - 4, rect.Height - 4), Color.Red);
                }*/
            }



        }

        public void Dispose()
        {
            if(texture != null)
            {
                texture.Dispose();
            }
            
        }
    }
}
