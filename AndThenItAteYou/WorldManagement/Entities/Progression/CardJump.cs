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
    public class CardJump : Card
    {
        public bool isOn;
        //public StatusEffect jumpEffect;

        public CardJump(float level) : base(level)
        {
            //isOn = true;
            //jumpEffect = new StatusEffect(StatusEffect.status.JUMP, level * .24f, 1, true);
            //player.addStatusEffect(jumpEffect);
            cardTex = Game1.texture_card_jump;
            iconTex = Game1.icon_jump;
            usable = true;
        }

        public override void upgrade(PlayerBase user, float amt)
        {
            //owner.removeStatusEffect(jumpEffect);
            level += amt;
            //jumpEffect = new StatusEffect(StatusEffect.status.JUMP, level * .24f, 1, true);
            //owner.addStatusEffect(jumpEffect);
        }

        public override void use(PlayerBase user, WorldBase world, Vector2 location, GameTime time, BinaryInputManager manager)
        {
            if (manager.click())
            {
                if (charges > 0)
                {
                    SoundManager.getSound("card-use-" + (level - 1)).playWithVariance(0, .5f, 0, SoundType.MONSTER);
                    user.addStatusEffect(new StatusEffect(StatusEffect.status.JUMP, level/* * .48f*/, 100, false));
                    user.jump(1);
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
            return obj is CardJump;
        }
    }
}
