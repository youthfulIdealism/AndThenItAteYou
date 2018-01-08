using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Survive.Input;
using Survive.Input.InputManagers;
using Survive.Sound;
using Survive.WorldManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Inventory.Items
{
    public class Item_Mushroom : Item
    {

        public Item_Mushroom(int uses) : base(uses)
        {
            if (texture == null)
            {
                texture = Game1.texture_mushroom;
            }
        }

        public override Item clone(int uses)
        {
            return new Item_Mushroom(uses);
        }

        public override int use(PlayerBase user, WorldBase world, Vector2 location, GameTime time, BinaryInputManager inputManager)
        {
            base.use(user, world, location, time, inputManager);
            int used = 1;

            user.hunger += 10;
            SoundManager.getSound("eat").playWithVariance(0, .2f, 0, .5f, .5f, 0, SoundType.MONSTER);

            return used;
        }
    }
}
