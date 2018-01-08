using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Survive.Input.InputManagers;
using Survive.Sound;
using Survive.WorldManagement.Inventory.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities.Progression
{
    public class CardSpeed : Card
    {
        public bool isOn;

        public CardSpeed(float level) : base(level)
        {
            usable = true;
            cardTex = Game1.texture_card_speed;
            iconTex = Game1.icon_speed;
        }

        public override void upgrade(PlayerBase player, float amt)
        {
            level += amt;
            //speedEffect = new StatusEffect(StatusEffect.status.SPEED, level * .4f, 1, true);
            //owner.addStatusEffect(speedEffect);
        }

        public override void use(PlayerBase user, WorldBase world, Vector2 location, GameTime time, BinaryInputManager manager)
        {
            if (manager.click())
            {
                if (charges > 0)
                {
                    SoundManager.getSound("card-use-" + (level - 1)).playWithVariance(0, .5f, 0, SoundType.MONSTER);
                    user.addStatusEffect(new StatusEffect(StatusEffect.status.SPEED, level, (int)(300 * level), false));
                    charges--;
                }
                else
                {
                    user.speechManager.addSpeechBubble(Game1.texture_item_charmstone);
                }
            }
        }

        public override bool Equals(object obj)
        {
            return obj is CardSpeed;
        }
    }
}
