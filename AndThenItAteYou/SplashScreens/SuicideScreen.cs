using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Survive.WorldManagement.IO;
using Survive.WorldManagement;
using Survive.Sound;

namespace Survive.SplashScreens
{
    public class SuicideScreen : SplashScreen
    {
        Rectangle suicideRect;
        Rectangle continueRect;
        Texture2D suicideTexture;
        Texture2D continueTexture;
        
        bool ready = false;
        WorldBase world;

        public SuicideScreen(int saveslot, WorldBase world)
        {
            this.world = world;
            suicideTexture = Game1.SuicideButton;
            continueTexture = Game1.ContinueButton;
            int screenW = Game1.instance.graphics.PreferredBackBufferWidth;
            int screenH = Game1.instance.graphics.PreferredBackBufferHeight;
            suicideRect = new Rectangle(screenW / 3 - suicideTexture.Width / 2, screenH / 2 - suicideTexture.Height, suicideTexture.Width, suicideTexture.Height);
            continueRect = new Rectangle((screenW / 3) * 2 - continueTexture.Width / 2, screenH / 2 - continueTexture.Height, continueTexture.Width, continueTexture.Height);
        }

        public override void acceptInput(GameTime time, KeyboardState currentKeyboardState, MouseState currentMouseState, KeyboardState prevKeyboardState, MouseState prevMouseState)
        {
            if (currentMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
            {
                if (suicideRect.Contains(currentMouseState.Position))
                {
                    ready = true;
                    world.player.health = -100;
                    SoundManager.getSound("click").playWithVariance(0, .25f, 0, SoundType.MONSTER);
                }
                else if (continueRect.Contains(currentMouseState.Position))
                {
                    ready = true;
                    Game1.instance.queuedSplashScreens.Add(new PauseScreen(0, world));
                    SoundManager.getSound("click").playWithVariance(0, .25f, 0, SoundType.MONSTER);
                }
            }

            if (currentKeyboardState.IsKeyDown(Keys.Escape) && prevKeyboardState.IsKeyUp(Keys.Escape))
            {
                ready = true;
                Game1.instance.queuedSplashScreens.Add(new PauseScreen(0, world));
                SoundManager.getSound("click").playWithVariance(0, .25f, 0, SoundType.MONSTER);
            }
        }

        public override bool canContinue()
        {
            return ready;
        }

        public override void draw(SpriteBatch batch)
        {
            batch.Draw(Game1.block, new Rectangle(0, 0, Game1.instance.graphics.PreferredBackBufferWidth, Game1.instance.graphics.PreferredBackBufferHeight), Color.Black * .5f);
            batch.Draw(suicideTexture, suicideRect, Color.White);
            batch.Draw(continueTexture, continueRect, Color.White);
        }

        public override void switchTo()
        {
            
        }

        public override bool isBlockingDrawCalls()
        {
            return false;
        }
    }
}
