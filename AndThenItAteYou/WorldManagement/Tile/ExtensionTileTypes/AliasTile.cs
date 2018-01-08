using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Survive.WorldManagement.Inventory;

namespace Survive.WorldManagement.Tile.ExtensionTileTypes
{
    public class AliasTile : TileType
    {
        public TileType aliased { get; protected set; }

        public AliasTile(TileType aliased) : base(aliased.tags.ToArray(), null, false)
        {
            this.aliased = aliased;
        }

        public override float getFrictionMultiplier()
        {
            return aliased.getFrictionMultiplier();
        }

        public override void harvest(TileType tileType, Item harvestTool, Vector2 location, WorldBase world)
        {
            aliased.harvest(tileType, harvestTool, location, world);
        }

        public override void draw(SpriteBatch batch, Point place, Color color)
        {
            aliased.draw(batch, place, color);
        }
    }
}
