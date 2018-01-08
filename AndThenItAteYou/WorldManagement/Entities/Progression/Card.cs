using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Survive.Input.InputManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities.Progression
{
    public abstract class Card
    {
        public const int maxLevel = 3;
        public static List<Card> potentialCards;

        public float level;
        public bool usable;
        public Texture2D iconTex;
        public Texture2D cardTex;
        public int charges = 3;

        public bool overridesRewardParticleLocation;
        public Vector2 rewardParticleOverrideLoc;
        public bool overridesRewardParticleColor;
        public Color rewardParticleOverrideColorStart;
        public Color rewardParticleOverrideColorEnd;

        public static void setUpCards()
        {
            potentialCards = new List<Card>();
            potentialCards.Add(new CardSpeed(0));
            potentialCards.Add(new CardJump(0));
            potentialCards.Add(new CardHealthBoost(0));
            potentialCards.Add(new CardHealthRegen(0));
            potentialCards.Add(new CardExplosive(0));
            potentialCards.Add(new CardBlink(0));
            potentialCards.Add(new CardSticky(0));
        }

        public Card(float level)
        {
            this.level = level;
            usable = false;
            charges = 3;

            overridesRewardParticleLocation = false;
            rewardParticleOverrideLoc = new Vector2();
            overridesRewardParticleColor = false;
            rewardParticleOverrideColorStart = Color.White;
            rewardParticleOverrideColorEnd = Color.White;
        }

        public abstract void use(PlayerBase user, WorldBase world, Vector2 location, GameTime time, BinaryInputManager inputManager);
        public abstract void upgrade(PlayerBase user, float amt);
    }
}
