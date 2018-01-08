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
    public abstract class Item_Bottle : Item
    {
        public int[] tags;

        public Item_Bottle(int uses, int[] tags) : base(uses)
        {
            this.tags = tags;

            if(tags.Length > 2)
            {
                Console.WriteLine("incorrect berry tags. Should be 2.");
            }
        }

        public override int use(PlayerBase user, WorldBase world, Vector2 location, GameTime time, BinaryInputManager inputManager)
        {
            base.use(user, world, location, time, inputManager);
            int used = 1;

            Console.WriteLine("Bottle trace: " + tags[0] + ", " + tags[1]);

            if (tags[0] == Item_Berry.POISONED || tags[1] == Item_Berry.POISONED)
            {
                user.hunger -= 20;
                user.health -= 10;
            }

            if (tags[0] == Item_Berry.SPEEDY || tags[1] == Item_Berry.SPEEDY)
            {
                user.addStatusEffect(new StatusEffect(StatusEffect.status.SPEED, 1.2f, 350, false));
            }

            if (tags[0] == Item_Berry.LEAPING || tags[1] == Item_Berry.LEAPING)
            {
                user.addStatusEffect(new StatusEffect(StatusEffect.status.JUMP, .8f, 350, false));
            }
            if (tags[0] == Item_Berry.REGEN || tags[1] == Item_Berry.REGEN)
            {
                user.addStatusEffect(new StatusEffect(StatusEffect.status.HEALTHREGEN, 40, 550, false));
            }
            if (tags[0] == Item_Berry.FILLING || tags[1] == Item_Berry.FILLING)
            {
                user.hunger += 10;
            }
            else if (tags[0] == Item_Berry.NONE || tags[1] == Item_Berry.NONE)
            {
                user.hunger += 10;
            }

            SoundManager.getSound("drink").playWithVariance(0, .2f, 0, SoundType.MONSTER);
            return used;
        }

        public override int GetHashCode()
        {
            return tags[0].GetHashCode() + tags[1].GetHashCode();
        }
    }
}
