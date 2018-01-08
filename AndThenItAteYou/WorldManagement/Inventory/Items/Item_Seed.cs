using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Survive.Input;
using Survive.Input.InputManagers;
using Survive.Sound;
using Survive.WorldManagement.Entities;
using Survive.WorldManagement.Entities.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Inventory.Items
{
    public class Item_Seed : Item
    {

        public Item_Seed(int uses) : base(uses)
        {
            if(texture == null)
            {
                texture = Game1.texture_item_seed;
            }
        }

        public override Item clone(int uses)
        {
            return new Item_Seed(uses);
        }

        public override int use(PlayerBase user, WorldBase world, Vector2 location, GameTime time, BinaryInputManager inputManager)
        {
            base.use(user, world, location, time, inputManager);

            if(user.collideBottom)
            {
                EntitySeed seed = new EntitySeed(user.location, world);
                world.addEntity(seed);

                SoundManager.getSound("spear-throw").playWithVariance(0, .2f, 0, SoundType.MONSTER);

                return 1;
            }
            
            return 0;
        }
    }
}
