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
    public class TotemTapir : TileTag
    {
        static TotemTapir() { usedTapir = false; }
        public static bool usedTapir;

        public override void onUse(WorldBase world, Item harvestTool, Vector2 location, TileType tileType, Entity user)
        {
            base.onUse(world, harvestTool, location, tileType, user);

            if(user is Player)
            {
                PlayerTaipir taipir = new PlayerTaipir(user.location, world, (Player)user);
                world.transformPlayer(taipir);
                usedTapir = true;

                if (usedTapir && TotemRabbit.usedRabbit)
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
