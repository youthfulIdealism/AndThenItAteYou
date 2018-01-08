using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Survive.WorldManagement.Entities;
using Survive.WorldManagement.Inventory;

namespace Survive.WorldManagement.Tile.Tags.OnUseTags
{
    public class Climbeable : TileTag
    {
        public override void onUse(WorldBase world, Item harvestTool, Vector2 location, TileType tileType, Entity user)
        {
            base.onUse(world, harvestTool, location, tileType, user);
            if (user.velocity.Y > 0)
            {
                user.impulse += new Vector2(0, -2f);
            }
            else if (user.velocity.Y > -4f)
            {
                user.impulse += new Vector2(0, -.71f);
                //user.impulse += new Vector2(0, -.8f);
            }
                
            //impulse += new Vector2(0, 1.2f);
            //user.impulse += new Vector2(0, -.71f);
        }
    }
}
