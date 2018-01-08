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
    public abstract class Item_Berry : Item
    {
        public static readonly int[] tagList = new int[]{ NONE, NONE, NONE, NONE, NONE, NONE, POISONED, SPEEDY, LEAPING, REGEN };
        public const int NONE = 0;
        public const int POISONED = 1;
        public const int FILLING = 2;
        public const int SPEEDY = 3;
        public const int LEAPING = 4;
        public const int REGEN = 4;

        public int[] tags;

        public Item_Berry(int uses, int[] tags) : base(uses)
        {
            this.tags = tags;

            if(tags.Length > 2)
            {
                Console.WriteLine("incorrect berry tags. Should be 2.");
            }
        }

        public static int[] getRandomTags(Random rand)
        {
            return new int[] { tagList[rand.Next(tagList.Length)], tagList[rand.Next(tagList.Length)] };
        }

        /*public override Item clone(int uses)
        {
            return new Item_Berry(uses, alias, tags);
        }*/

        public override int use(PlayerBase user, WorldBase world, Vector2 location, GameTime time, BinaryInputManager inputManager)
        {
            base.use(user, world, location, time, inputManager);
            int used = 1;

            Console.WriteLine("Berry trace: " + tags[0] + ", " + tags[1]);
            if (tags[0] == POISONED || tags[1] == POISONED)
            {
                user.hunger -= 10;
                user.health -= 5;
            }

            if (tags[0] == SPEEDY || tags[1] == SPEEDY)
            {
                user.addStatusEffect(new StatusEffect(StatusEffect.status.SPEED, .7f, 150, false));
            }

            if (tags[0] == LEAPING || tags[1] == LEAPING)
            {
                user.addStatusEffect(new StatusEffect(StatusEffect.status.JUMP, .6f, 150, false));
            }
            if (tags[0] == REGEN || tags[1] == REGEN)
            {
                user.addStatusEffect(new StatusEffect(StatusEffect.status.HEALTHREGEN, 20, 150, false));
            }
            if (tags[0] == FILLING || tags[1] == FILLING)
            {
                user.hunger += 10;
            }
            else if (tags[0] == NONE || tags[1] == NONE)
            {
                user.hunger += 5;
            }

            SoundManager.getSound("eat").playWithVariance(0, .2f, 0, .5f, .5f, 0, SoundType.MONSTER);

            return used;
        }

        public override int GetHashCode()
        {
            return tags[0].GetHashCode() + tags[1].GetHashCode();
        }
    }
}
