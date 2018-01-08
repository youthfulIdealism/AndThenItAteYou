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
    public class PauseScreen : SplashScreen
    {
        Rectangle saveAndExitRect;
        Rectangle continueRect;
        Rectangle optionsMenuRect;
        Rectangle suicideRect;
        Texture2D saveAndExitTexture;
        Texture2D continueTexture;
        Texture2D optionsTexture;
        Texture2D suicideTex;
        bool ready = false;
        WorldBase world;

        public PauseScreen(int saveslot, WorldBase world)
        {
            this.world = world;
            saveAndExitTexture = Game1.SaveAndExitButton;
            continueTexture = Game1.ContinueButton;
            optionsTexture = Game1.OptionButton;
            suicideTex = Game1.SuicideButton;
            int screenW = Game1.instance.graphics.PreferredBackBufferWidth;
            int screenH = Game1.instance.graphics.PreferredBackBufferHeight;
            saveAndExitRect = new Rectangle(screenW / 5 - saveAndExitTexture.Width / 2, screenH / 2 - saveAndExitTexture.Height, saveAndExitTexture.Width, saveAndExitTexture.Height);
            continueRect = new Rectangle((screenW / 5) * 2 - continueTexture.Width / 2, screenH / 2 - continueTexture.Height, continueTexture.Width, continueTexture.Height);
            optionsMenuRect = new Rectangle((screenW / 5) * 3 - optionsTexture.Width / 2, screenH / 2 - optionsTexture.Height, optionsTexture.Width, optionsTexture.Height);
            suicideRect = new Rectangle((screenW / 5) * 4 - suicideTex.Width / 2, screenH / 2 - suicideTex.Height, suicideTex.Width, suicideTex.Height);
        }

        public override void acceptInput(GameTime time, KeyboardState currentKeyboardState, MouseState currentMouseState, KeyboardState prevKeyboardState, MouseState prevMouseState)
        {
            if (currentMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
            {
                if (saveAndExitRect.Contains(currentMouseState.Position))
                {
                    SoundManager.getSound("click").playWithVariance(0, .25f, 0, SoundType.MONSTER);
                    ready = true;
                    GameSaverAndLoader.save(Game1.selectedSaveSlot, world);
                    Game1.instance.Exit();
                }
                else if (continueRect.Contains(currentMouseState.Position))
                {
                    SoundManager.getSound("click").playWithVariance(0, .25f, 0, SoundType.MONSTER);
                    keepGoing();
                }
                else if (optionsMenuRect.Contains(currentMouseState.Position))
                {
                    SoundManager.getSound("click").playWithVariance(0, .25f, 0, SoundType.MONSTER);
                    keepGoing();
                    Game1.instance.queuedSplashScreens.Add(new SettingsScreen(0, world));
                } else if (suicideRect.Contains(currentMouseState.Position))
                {
                    SoundManager.getSound("click").playWithVariance(0, .25f, 0, SoundType.MONSTER);
                    ready = true;
                    Game1.instance.queuedSplashScreens.Add(new SuicideScreen(0, world));
                }
            }

            if (currentKeyboardState.IsKeyDown(Keys.Escape) && prevKeyboardState.IsKeyUp(Keys.Escape))
            {
                keepGoing();
            }
        }


        public void keepGoing()
        {
            SoundManager.resume();
            
            
            
            ready = true;
        }

        public override bool canContinue()
        {
            return ready;
        }

        public override void draw(SpriteBatch batch)
        {
            batch.Draw(Game1.block, new Rectangle(0, 0, Game1.instance.graphics.PreferredBackBufferWidth, Game1.instance.graphics.PreferredBackBufferHeight), Color.Black * .5f);
            batch.Draw(saveAndExitTexture, saveAndExitRect, Color.White);
            batch.Draw(optionsTexture, optionsMenuRect, Color.White);
            batch.Draw(continueTexture, continueRect, Color.White);
            batch.Draw(suicideTex, suicideRect, Color.White);
        }

        public override void switchTo()
        {
            SoundManager.pause();
            if (world is World)
            {
                world.decorator.ambientSoundManager.pause();
            }
        }

        public override bool isBlockingDrawCalls()
        {
            return false;
        }
    }
}
