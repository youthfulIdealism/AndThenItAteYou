using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Survive.WorldManagement.Entities;
using Survive.WorldManagement.Entities.Progression;
using Survive.Input.ControlManagers;
using Survive.WorldManagement.Entities.Particles;
using Survive.Sound;

namespace Survive.Input.ControlManagers
{
    public class LevelUpControlManager : ControlManager
    {
        Player player;
        Card card1;
        Card card2;
        Rectangle card1Rect;
        Rectangle card2Rect;
        float levelUpAmount = 0;
        Random rand;
        public LevelUpControlManager(Player player, float amount)
        {
            this.player = player;
            levelUpAmount = amount;
            rand = new Random();

            if (player.cards[0] == null)
            {
                card1 = Card.potentialCards[rand.Next(Card.potentialCards.Count)];
            }else
            {
                card1 = player.cards[0];
            }

            if (player.cards[1] == null)
            {
                do
                {
                    card2 = Card.potentialCards[rand.Next(Card.potentialCards.Count)];
                } while (card2.Equals(card1));
            }
            else
            {
                card2 = player.cards[1];
            }

            card1Rect = new Rectangle(Game1.instance.graphics.PreferredBackBufferWidth / 3 - 100,
                Game1.instance.graphics.PreferredBackBufferHeight / 2 - 200,
                200,
                400
                );

            card2Rect = new Rectangle((Game1.instance.graphics.PreferredBackBufferWidth / 3) * 2 - 100,
                Game1.instance.graphics.PreferredBackBufferHeight / 2 - 200,
                200,
                400
                );
        }

        public override void acceptInput(GameTime time, KeyboardState currentKeyboardState, MouseState currentMouseState, KeyboardState prevKeyboardState, MouseState prevMouseState)
        {
            if(currentMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
            {
                if(card1Rect.Contains(currentMouseState.Position) && card1.level < Card.maxLevel)
                {
                    card1.upgrade(player, levelUpAmount);
                    player.currentControlManager = player.movementControlManger;
                    player.movementControlManger.switchTo(null);
                    player.cards[0] = card1;
                    spawnCardSelectionParticlesAndPlaySound(card1Rect, new Vector2(0, Game1.instance.graphics.PreferredBackBufferHeight), card1);
                }
                else if (card2Rect.Contains(currentMouseState.Position) && card2.level < Card.maxLevel)
                {
                    card2.upgrade(player, levelUpAmount);
                    player.currentControlManager = player.movementControlManger;
                    player.movementControlManger.switchTo(null);
                    if(player.cards[0] == null)
                    {
                        player.cards[0] = card2;
                        spawnCardSelectionParticlesAndPlaySound(card2Rect, new Vector2(0, Game1.instance.graphics.PreferredBackBufferHeight), card2);
                    }
                    else
                    {
                        player.cards[1] = card2;
                        spawnCardSelectionParticlesAndPlaySound(card2Rect, new Vector2(Game1.instance.graphics.PreferredBackBufferWidth, Game1.instance.graphics.PreferredBackBufferHeight), card2);
                    }
                    
                }
            }
        }

        private void spawnCardSelectionParticlesAndPlaySound(Rectangle bounds, Vector2 target, Card card)
        {
            Color startColor;
            Color endColor;
            if(card.overridesRewardParticleColor)
            {
                startColor = card.rewardParticleOverrideColorStart;
                endColor = card.rewardParticleOverrideColorEnd;
            }else
            {
                startColor = Color.White;
                endColor = Color.White;
            }

            if(card.overridesRewardParticleLocation)
            {
                target = card.rewardParticleOverrideLoc;
            }
            for (int i = 0; i < 500; i++)
            {
                Vector2 spawnLoc = new Vector2(bounds.X + (float)rand.NextDouble() * bounds.Width, bounds.Y + (float)rand.NextDouble() * bounds.Height);
                Vector2 direction = Vector2.Normalize(target - spawnLoc) * 15;
                ParticleUIArbitrary rewardParticle = new ParticleUIArbitrary(spawnLoc, player.world, direction, 250, Game1.texture_particle_glow);
                rewardParticle.width = 15;
                rewardParticle.height = 15;
                rewardParticle.gravityMultiplier = 0;
                rewardParticle.frictionMultiplier = 0;
                rewardParticle.startColor = startColor;
                rewardParticle.endColor = endColor;
                player.world.addEntity(rewardParticle);
            }
            ParticleUIArbitrary targetFlash = new ParticleUIArbitrary(target, player.world, new Vector2(), 150, Game1.texture_particle_glow);
            targetFlash.width = 100;
            targetFlash.height = 100;
            targetFlash.gravityMultiplier = 0;
            targetFlash.frictionMultiplier = 0;
            targetFlash.startColor = startColor * .1f;
            targetFlash.endColor = endColor;
            player.world.addEntity(targetFlash);

            SoundManager.getSound("card-pick").playWithVariance(0, 1, 0, SoundType.MONSTER);
        }

        public override void draw(SpriteBatch batch)
        {
            batch.Draw(Game1.block, new Rectangle(0, 0, Game1.instance.graphics.PreferredBackBufferWidth, Game1.instance.graphics.PreferredBackBufferHeight), Color.Black * .2f);
            if(card1.level >= Card.maxLevel)
            {
                batch.Draw(card1.cardTex, card1Rect, Color.Gray);
            }else
            {
                batch.Draw(card1.cardTex, card1Rect, Color.White);
            }

            if (card2.level >= Card.maxLevel)
            {
                batch.Draw(card2.cardTex, card2Rect, Color.Gray);
            }
            else
            {
                batch.Draw(card2.cardTex, card2Rect, Color.White);
            }

            batch.DrawString(Game1.gamefont_32, "" + Game1.decimalToBase6((int)Math.Floor(card1.level)), card1Rect.Location.ToVector2(), Color.Black);
            batch.DrawString(Game1.gamefont_32, "" + Game1.decimalToBase6((int)Math.Floor(card2.level)), card2Rect.Location.ToVector2(), Color.Black);
        }
    }
}
