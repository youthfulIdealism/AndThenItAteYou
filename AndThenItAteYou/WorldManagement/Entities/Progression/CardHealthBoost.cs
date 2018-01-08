using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Survive.Input.InputManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities.Progression
{
    public class CardHealthBoost : Card
    {
        public bool isOn;
        public StatusEffect healthBoostEffect;

        public CardHealthBoost(float level) : base(level)
        {
            isOn = true;
            healthBoostEffect = new StatusEffect(StatusEffect.status.HEALTHBOOST, level * 30, 1, true);
            cardTex = Game1.texture_card_maxhealth;

            overridesRewardParticleLocation = true;
            rewardParticleOverrideLoc = new Vector2();
            overridesRewardParticleColor = true;
            rewardParticleOverrideColorStart = Color.White;
            rewardParticleOverrideColorEnd = Color.Red;
        }

        public override void upgrade(PlayerBase user, float amt)
        {
            user.removeStatusEffect(healthBoostEffect);
            level += amt;
            healthBoostEffect = new StatusEffect(StatusEffect.status.HEALTHBOOST, level * 30, 1, true);
            user.addStatusEffect(healthBoostEffect);
            user.health = 100 + level * 30;
        }

        public override void use(PlayerBase user, WorldBase world, Vector2 location, GameTime time, BinaryInputManager manager)
        {


        }

        public override bool Equals(object obj)
        {
            return obj is CardHealthBoost;
        }
    }
}
