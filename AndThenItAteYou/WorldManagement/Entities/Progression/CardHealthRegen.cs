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
    public class CardHealthRegen : Card
    {
        public bool isOn;

        public CardHealthRegen(float level) : base(level)
        {
            isOn = true;
            cardTex = Game1.texture_card_regen;
            iconTex = Game1.icon_regen;
            usable = true;
        }

        public override void upgrade(PlayerBase player, float amt)
        {
            //owner.removeStatusEffect(regenEffect);
            level += amt;
            //regenEffect = new StatusEffect(StatusEffect.status.HEALTHREGEN, level * 3, 1, true);
            //owner.addStatusEffect(regenEffect);
        }

        public override void use(PlayerBase user, WorldBase world, Vector2 location, GameTime time, BinaryInputManager manager)
        {
            if (manager.click())
            {
                if (charges > 0)
                {
                    SoundManager.getSound("card-use-" + (level - 1)).playWithVariance(0, .5f, 0, SoundType.MONSTER);
                    user.addStatusEffect(new StatusEffect(StatusEffect.status.HEALTHREGEN, level/* * 3*/, 300, false));
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
            return obj is CardHealthRegen;
        }
    }
}
