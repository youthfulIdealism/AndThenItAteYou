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
    public class CardExplosive : Card
    {
        public bool isOn;

        public CardExplosive(float level) : base(level)
        {
            usable = true;
            isOn = true;
            cardTex = Game1.texture_card_explosive;
            iconTex = Game1.icon_explosion;
        }

        public override void upgrade(PlayerBase player, float amt)
        {
            level += amt;
        }

        public override void use(PlayerBase user, WorldBase world, Vector2 location, GameTime time, BinaryInputManager manager)
        {
            if (manager.click())
            {
                if (charges > 0)
                {
                    SoundManager.getSound("card-use-" + (level - 1)).playWithVariance(0, .5f, 0, SoundType.MONSTER);
                    EntityExplosive explosive = new EntityExplosive(user.location + new Vector2(0, -15), world);
                    explosive.radious = (int)level + 1;
                    explosive.timeBeforeExplosion += 16 * (int)level;
                    explosive.velocity += Vector2.Normalize(location - user.location) * 22;
                    world.addEntity(explosive);
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
            return obj is CardExplosive;
        }
    }
}
