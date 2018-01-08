using Microsoft.Xna.Framework;
using Survive.Input.InputManagers;
using Survive.WorldManagement.Entities;
using Survive.WorldManagement.Tile;
using Survive.WorldManagement.Tile.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Inventory.Items
{
    public class Item_Ladder : Item
    {
        public Item_Ladder(int uses) : base(uses)
        {
            if(texture == null)
            {
                texture = Game1.texture_item_ladder;
            }
        }

        public override Item clone(int uses)
        {
            return new Item_Ladder(uses);
        }

        public override int use(PlayerBase user, WorldBase world, Vector2 location, GameTime time, BinaryInputManager inputManager)
        {
            int consumed = 0;

            TileType currentTile = world.getBlock(location);
            if(currentTile != null && !currentTile.tags.Contains(TagReferencer.Climbeable) && !currentTile.tags.Contains(TagReferencer.Teleporter))
            {
                consumed = 1;
                world.placeTile(TileTypeReferencer.LADDER, location);
            }


            return consumed;
        }
    }
}
