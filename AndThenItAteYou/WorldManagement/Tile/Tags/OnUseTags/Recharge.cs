using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Survive.WorldManagement.Entities;
using Survive.WorldManagement.Inventory;
using Survive.WorldManagement.Entities.Particles;
using Survive.WorldManagement.Entities.Progression;

namespace Survive.WorldManagement.Tile.Tags.OnUseTags
{
    public class Recharge : TileTag
    {
        public override void onUse(WorldBase world, Item harvestTool, Vector2 location, TileType tileType, Entity user)
        {
            base.onUse(world, harvestTool, location, tileType, user);
            if(user is PlayerBase)
            {
                bool destroy = false;

                PlayerBase player = ((PlayerBase)user);
                if (player.cards[0] != null)
                {
                    int chargeParticleCount = player.cardCharges - player.cards[0].charges;
                    player.cards[0].charges = player.cardCharges;
                    for (int i = 0; i < chargeParticleCount; i++)
                    {
                        world.addEntity(new ParticleRecharge(location, world, new Vector2(0, -1), 75));
                    }

                    if(player.cards[0] is CardHealthRegen)
                    {
                        destroy = true;
                    }

                }
                if (player.cards[1] != null)
                {
                    int chargeParticleCount = player.cardCharges - player.cards[1].charges;
                    player.cards[1].charges = player.cardCharges;
                    for (int i = 0; i < chargeParticleCount; i++)
                    {
                        world.addEntity(new ParticleRecharge(location, world, new Vector2(0, -1), 75));
                    }

                    if (player.cards[1] is CardHealthRegen)
                    {
                        destroy = true;
                    }
                }

                if(destroy)
                {
                    world.placeTile(TileTypeReferencer.AIR, location);
                    for (int i = 0; i < 7; i++)
                    {
                        world.addEntity(new ParticleTileBreak(location, world, new Vector2(), tileType, 150));
                    }
                }
            }
        }
    }
}
