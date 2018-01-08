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
using Survive.WorldManagement.Inventory;
using Survive.WorldManagement.Entities.Progression;
using Survive.WorldManagement.Inventory.Items;
using Survive.WorldManagement.Entities;
using static Survive.Game1;
using Survive.Sound;
using Survive.WorldManagement.Worlds;
using Survive.WorldManagement.Worlds.Cutscenes;

namespace Survive.SplashScreens
{
    public class PlayerSelectScreen : SplashScreen
    {
        static PlayerSelectScreen()
        {
            Random rand = new Random();
        }

        bool drawArrowHint = true;
        bool readyToContinue = false;
        WorldBase world;

        public PlayerSelectScreen(int[] availableKits, WorldBase world)
        {
            this.world = world;
            Random rand = new Random();

            foreach(int i in availableKits)
            {
                PlayerKitRegistry.registry[i].locked = false;
            }

            


            int kitDrawInterval = (int)(Game1.instance.graphics.PreferredBackBufferWidth * .75f) / PlayerKitRegistry.registry.Count;
            for(int i = 0; i < PlayerKitRegistry.registry.Count; i++)
            {
                Rectangle buttonRect = new Rectangle(
                    (int)(Game1.instance.graphics.PreferredBackBufferWidth * .25f * .5f) + kitDrawInterval * i,
                    Game1.instance.graphics.PreferredBackBufferHeight / 2,
                    50,
                    90
                    );
                PlayerKitRegistry.registry[i].buttonRect = buttonRect;

            }
            drawArrowHint = availableKits.Length == 1;
        }
        MouseState currentMouseState;
        public override void acceptInput(GameTime time, KeyboardState currentKeyboardState, MouseState currentMouseState, KeyboardState prevKeyboardState, MouseState prevMouseState)
        {
           this.currentMouseState = currentMouseState;
           if (currentMouseState.LeftButton.Equals(ButtonState.Pressed) && prevMouseState.LeftButton.Equals(ButtonState.Released))
           {
                foreach (PlayerStarterKit kit in PlayerKitRegistry.registry.Values)
                {
                    if(kit.buttonRect.Contains(currentMouseState.Position) && !kit.locked)
                    {
                        if(GameSaverAndLoader.doesSaveExist(Game1.selectedSaveSlot))
                        {
                            GameSaverAndLoader.deleteSave(Game1.selectedSaveSlot);
                        }

                        if (kit.id == 0 && !(world is TutorialWorld))
                        {
                            /*Dictionary<Rectangle, string> images = new Dictionary<Rectangle, string>();

                            List<String> challengeImages = UniverseProperties.challangeListToStringList();

                            int imageWidth = 1000;
                            int imageHeight = 400;
                            for (int i = 0; i < 6; i++)
                            {
                                images.Add(new Rectangle(imageWidth * i, Game1.instance.graphics.PreferredBackBufferHeight / 2 - imageHeight / 2, imageWidth, imageHeight), "StorySplashes/story_" + i);
                            }

                            Game1.instance.queuedSplashScreens.Add(new ScrollingSplashScreen(images, new List<string>()));*/
                            StartingCutscene startingCutscene = new StartingCutscene(world);
                            kit.selectKit(startingCutscene);
                            Game1.instance.switchWorlds(startingCutscene);
                            
                        }else
                        {
                            kit.selectKit(world);
                            Game1.instance.switchWorlds(world);
                        }

                        readyToContinue = true;
                        break;
                    }
                }
           }
        }

        public override bool canContinue()
        {
            return readyToContinue;
        }

        float arrowHintOscillator = 0;

        public override void draw(SpriteBatch batch)
        {
            //batch.Draw(saveAndExitTexture, saveAndExitRect, Color.White);
            //batch.Draw(continueTexture, continueRect, Color.White);
            foreach(PlayerStarterKit kit in PlayerKitRegistry.registry.Values)
            {

                
                //batch.Draw(Game1.block, kit.buttonRect, Color.Red);
                Rectangle drawRect = new Rectangle(
                    kit.buttonRect.X - (kit.animations.standTex.Width - kit.buttonRect.Width) / 2,
                    kit.buttonRect.Y - (kit.animations.standTex.Height - kit.buttonRect.Height) / 2,
                    kit.animations.standTex.Width,
                    kit.animations.standTex.Height
                    );

                
                

                if (currentMouseState != null && kit.buttonRect.Contains(currentMouseState.Position))
                {
                    batch.Draw(kit.animations.standTex, new Rectangle((int)(drawRect.X - (int)(drawRect.Width * .1f)), (int)(drawRect.Y - (int)(drawRect.Height * .1f)), (int)(drawRect.Width * 1.2f), (int)(drawRect.Height * 1.2f)), Color.White * .5f);
                }

                batch.Draw(kit.animations.standTex, drawRect, Color.White);

                if(kit.locked)
                {
                    Rectangle lockRect = new Rectangle(drawRect.Center.X - Game1.charLock.Width / 2, drawRect.Top + Game1.charLock.Height + 70, Game1.charLock.Width, Game1.charLock.Height);
                    if(lockRect.Contains(currentMouseState.Position))
                    {
                        if(kit.unlockTex != null)
                        {
                            batch.Draw(kit.unlockTex, new Rectangle(lockRect.Center.X - kit.unlockTex.Width / 2, lockRect.Center.Y - kit.unlockTex.Height / 2, kit.unlockTex.Width, kit.unlockTex.Height), Color.White);
                        }
                        
                    }
                    else
                    {
                        batch.Draw(Game1.charLock, lockRect, Color.White);
                    }
                }else
                {
                    if (drawArrowHint)
                    {
                        arrowHintOscillator += .1f;
                        batch.Draw(Game1.downArrow, new Rectangle(drawRect.X + drawRect.Width / 2 - Game1.downArrow.Width / 2, drawRect.Y - Game1.downArrow.Height - 20 + (int)(Math.Sin(arrowHintOscillator) * 10), Game1.downArrow.Width, Game1.downArrow.Height), Color.White);
                    }
                }
            }
        }

        public override void switchTo()
        {
            
        }

        public override bool isBlockingDrawCalls()
        {
            return true;
        }
    }
}
