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
    public class Item_Snare : Item
    {

        public Item_Snare(int uses) : base(uses)
        {
            if(texture == null)
            {
                texture = Game1.texture_item_snare;
            }
        }

        public override Item clone(int uses)
        {
            return new Item_Snare(uses);
        }

        public override int use(PlayerBase user, WorldBase world, Vector2 location, GameTime time, BinaryInputManager inputManager)
        {
            base.use(user, world, location, time, inputManager);

            Entities.EntitySnare snare = new Entities.EntitySnare(user.location + new Vector2(0, -15), world, user);
            world.addEntity(snare);

            SoundManager.getSound("spear-throw").playWithVariance(0, .2f, 0, SoundType.MONSTER);
            return 1;
        }
    }
}
