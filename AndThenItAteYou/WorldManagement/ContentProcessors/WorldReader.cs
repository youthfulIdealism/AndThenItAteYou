using Microsoft.Xna.Framework;
using Survive.WorldManagement.Procedurals;
using Survive.WorldManagement.Tile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.ContentProcessors
{
    public class WorldReader
    {
        String path;
        public WorldReader(String path)
        {
            this.path = path;
        }

        public Decoration readChunk(Point chunkLoc)
        {
            string lookPlace = path + "\\" + chunkLoc.X + "-" + chunkLoc.Y + ".cnk";
            if (File.Exists(lookPlace))
            {
                TileType[,] tileData;
                TileType[,] backgroundTileData;
                using (BinaryReader reader = new BinaryReader(File.Open(lookPlace, FileMode.Open)))
                {
                    tileData = new TileType[Chunk.tilesPerChunk, Chunk.tilesPerChunk];
                    backgroundTileData = new TileType[Chunk.tilesPerChunk, Chunk.tilesPerChunk];

                    for (int x = 0; x < Chunk.tilesPerChunk; x++)
                    {
                        for (int y = 0; y < Chunk.tilesPerChunk; y++)
                        {
                            int readValue = reader.ReadInt32();
                            if(readValue != -1)
                            {
                                tileData[x, y] = TileType.getTileFromID(readValue);
                            }
                        }
                    }

                    for (int x = 0; x < Chunk.tilesPerChunk; x++)
                    {
                        for (int y = 0; y < Chunk.tilesPerChunk; y++)
                        {
                            int readValue = reader.ReadInt32();
                            if (readValue != -1)
                            {
                                backgroundTileData[x, y] = TileType.getTileFromID(readValue);
                            }
                        }
                    }
                }

                
                return new Decoration(new Point(), tileData, backgroundTileData);
            }else
            {
                //throw new Exception("DECORATION " + path + " NOT FOUND.");
                Logger.log("chunk " + lookPlace + " not found");
                return null;
            }
        }

        public void writeWorld(WorldBase world, String name)
        {
            for (int kx = -world.tileGenRadious; kx <= world.tileGenRadious; kx++)
            {
                for (int ky = -world.tileGenRadious; ky <= world.tileGenRadious; ky++)
                {
                    Directory.CreateDirectory(name + "\\");
                    using (BinaryWriter writer = new BinaryWriter(File.Open(name + "\\" + kx + "-" + ky + ".cnk", FileMode.Create)))
                    {
                        for (int x = 0; x <= Chunk.tilesPerChunk; x++)
                        {
                            for (int y = 0; y <= Chunk.tilesPerChunk; y++)
                            {
                                TileType type = world.getBlock(new Point(Chunk.tilesPerChunk * kx + x, Chunk.tilesPerChunk * ky + y));
                                if (type != null)
                                {
                                    writer.Write(type.TILEID);
                                }
                                else
                                {
                                    writer.Write(-1);
                                }
                            }
                        }

                        for (int x = 0; x <= Chunk.tilesPerChunk; x++)
                        {
                            for (int y = 0; y <= Chunk.tilesPerChunk; y++)
                            {
                                TileType type = world.getBackgroundBlock(new Point(Chunk.tilesPerChunk * kx + x, Chunk.tilesPerChunk * ky + y));
                                if (type != null)
                                {
                                    writer.Write(type.TILEID);
                                }
                                else
                                {
                                    writer.Write(-1);
                                }

                            }
                        }
                    }
                }
            }
        }


        public void writeChunk(Chunk chunk)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(path + "\\" + chunk.location.X + "-" + chunk.location.Y + ".cnk", FileMode.Create)))
            {
                for (int x = 0; x < Chunk.tilesPerChunk; x++)
                {
                    for (int y = 0; y < Chunk.tilesPerChunk; y++)
                    {
                        TileType type = chunk.getTile(new Point(x, y));
                        if (type != null)
                        {
                            writer.Write(type.TILEID);
                        }
                        else
                        {
                            writer.Write(-1);
                        }
                    }
                }

                for (int x = 0; x < Chunk.tilesPerChunk; x++)
                {
                    for (int y = 0; y < Chunk.tilesPerChunk; y++)
                    {
                        TileType type = chunk.getBackgroundTile(new Point(x, y));
                        if (type != null)
                        {
                            writer.Write(type.TILEID);
                        }
                        else
                        {
                            writer.Write(-1);
                        }

                    }
                }
            }
        }


    }
}
