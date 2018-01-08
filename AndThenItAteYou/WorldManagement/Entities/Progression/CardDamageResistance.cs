using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Survive.Input.InputManagers;
using Survive.Sound;
using Survive.WorldManagement.Entities.Projectiles;
using Survive.WorldManagement.Inventory.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities.Progression
{
    public class CardDamageResistance : Card
    {
        public bool isOn;

        public CardDamageResistance(float level) : base(level)
        {
            usable = true;
            isOn = true;
            cardTex = Game1.texture_card_immunity;
            iconTex = Game1.icon_immunity;
        }

        public override void upgrade(PlayerBase player, float amt)
        {
            level += amt;
        }

        public override void use(PlayerBase user, WorldBase world, Vector2 location, GameTime time, BinaryInputManager manager)
        {
            if (manager.click())
            {
                Item_Charmstone item_charmstone = (Item_Charmstone)user.inventory.getItemOfType(new Item_Charmstone(1));
                if (item_charmstone != null)
                {
                    user.addStatusEffect(new StatusEffect(StatusEffect.status.DAMAGERESISTANCE, level + 1, 500, false));
                    user.inventory.consume(item_charmstone, 1);
                }else
                {
                    user.speechManager.addSpeechBubble(Game1.texture_item_charmstone);
                }
            }
        }

        public override bool Equals(object obj)
        {
            return obj is CardDamageResistance;
        }
    }
}
