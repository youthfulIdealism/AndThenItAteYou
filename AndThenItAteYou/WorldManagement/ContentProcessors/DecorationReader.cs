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
    public class DecorationReader
    {
        public static Decoration readDecorationFromContent(String path)
        {
            return readDecoration("Content/" + path);
        }

        public static Decoration readDecoration(String path)
        {
            if (File.Exists(path))
            {
                int decorationWidth;
                int decorationHeight;
                Point decorationOffset;
                TileType[,] tileData;
                TileType[,] backgroundTileData;
                using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
                {
                    decorationWidth = reader.ReadInt32();
                    decorationHeight = reader.ReadInt32();
                    decorationOffset = new Point(reader.ReadInt32(), -reader.ReadInt32());
                    tileData = new TileType[decorationWidth, decorationHeight];
                    backgroundTileData = new TileType[decorationWidth, decorationHeight];

                    int numReads = 4;

                    for (int x = 0; x < decorationWidth; x++)
                    {
                        for (int y = 0; y < decorationHeight; y++)
                        {
                            int readValue = reader.ReadInt32();
                            if(readValue != -1)
                            {
                                tileData[x, y] = TileType.getTileFromID(readValue);
                            }
                            numReads++;
                        }
                    }

                    for (int x = 0; x < decorationWidth; x++)
                    {
                        for (int y = 0; y < decorationHeight; y++)
                        {
                            int readValue = reader.ReadInt32();
                            if (readValue != -1)
                            {
                                backgroundTileData[x, y] = TileType.getTileFromID(readValue);
                            }
                            numReads++;
                        }
                    }
                }

                
                return new Decoration(decorationOffset, tileData, backgroundTileData);
            }else
            {
                throw new Exception("DECORATION " + path + " NOT FOUND.");
            }
        }

        public static void writeDecoration(WorldBase world, Point lowerLeft, Point origin, Point topRight, String name)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Saved Games/MapData", name), FileMode.Create)))
            {
                writer.Write(topRight.X - lowerLeft.X + 1);//write decoration width
                writer.Write(lowerLeft.Y - topRight.Y + 1);//write decoration height
                writer.Write(origin.X - lowerLeft.X);//write decoration offset x
                writer.Write(topRight.Y - origin.Y);//write decoration offset y

                int numWrites = 4;
                for(int x = lowerLeft.X; x <= topRight.X; x++)
                {
                    for (int y = topRight.Y; y <= lowerLeft.Y; y++)
                    {
                        if(world.getBlock(new Point(x, y)) == TileTypeReferencer.AIR)
                        {
                            writer.Write(-1);
                        }else
                        {
                            writer.Write(world.getBlock(new Point(x, y)).TILEID);
                        }
                        numWrites++;
                    }
                }

                for (int x = lowerLeft.X; x <= topRight.X; x++)
                {
                    for (int y = topRight.Y; y <= lowerLeft.Y; y++)
                    {
                        TileType type = world.getBackgroundBlock(new Point(x, y));
                        if(type == null)
                        {
                            writer.Write(-1);
                        }else
                        {
                            writer.Write(type.TILEID);
                        }
                        numWrites++;
                    }
                }

            }
        }
    }
}
