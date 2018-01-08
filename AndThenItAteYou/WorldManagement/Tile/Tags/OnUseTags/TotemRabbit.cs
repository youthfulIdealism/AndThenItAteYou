using Microsoft.Xna.Framework;
using Survive.WorldManagement.Entities;
using Survive.WorldManagement.Entities.TransformedPlayers;
using Survive.WorldManagement.Inventory;
using Survive.WorldManagement.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Tile.Tags.OnUseTags
{
    public class TotemRabbit : TileTag
    {
        static TotemRabbit() { usedRabbit = false; }
        public static bool usedRabbit;
        public override void onUse(WorldBase world, Item harvestTool, Vector2 location, TileType tileType, Entity user)
        {
            base.onUse(world, harvestTool, location, tileType, user);

            if(user is Player)
            {
                PlayerRabbit rabbit = new PlayerRabbit(user.location, world, (Player)user);
                world.transformPlayer(rabbit);
                usedRabbit = true;

                if (TotemTapir.usedTapir)
                {
                    if (MetaData.unlockCharacter(2))
                    {
                        MetaData.playUnlockCharacterAlert(2, world, world.player.location);
                    }
                }
            }
        }
    }
}
