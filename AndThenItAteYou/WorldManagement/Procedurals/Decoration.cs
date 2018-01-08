using Microsoft.Xna.Framework;
using Survive.WorldManagement.Tile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Procedurals
{
    public class Decoration
    {
        public Point origin;
        public TileType[,] backgroundDecorationMap;
        public TileType[,] decorationMap;
        public Rectangle box;

        public Decoration(Point origin, TileType[,] decorationMap)
        {
            this.origin = origin;
            this.decorationMap = decorationMap;
            box = new Rectangle(-origin.X, -origin.Y, decorationMap.GetLength(0), decorationMap.GetLength(1));
        }

        public Decoration(Point origin, TileType[,] decorationMap, TileType[,] backgroundDecorationMap) : this(origin, decorationMap)
        {
            this.backgroundDecorationMap = backgroundDecorationMap;
        }
    }
}
