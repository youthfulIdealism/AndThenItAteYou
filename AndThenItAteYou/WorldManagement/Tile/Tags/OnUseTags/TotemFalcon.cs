using Microsoft.Xna.Framework;
using Survive.WorldManagement.Entities;
using Survive.WorldManagement.Entities.TransformedPlayers;
using Survive.WorldManagement.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Tile.Tags.OnUseTags
{
    public class TotemFalcon : TileTag
    {
        public override void onUse(WorldBase world, Item harvestTool, Vector2 location, TileType tileType, Entity user)
        {
            base.onUse(world, harvestTool, location, tileType, user);

            if(user is Player)
            {
                PlayerFalcon falcon = new PlayerFalcon(user.location, world, (Player)user);
                world.transformPlayer(falcon);
            }
        }
    }
}
