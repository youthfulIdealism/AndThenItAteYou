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
using Survive.Input;
using Survive.WorldManagement.Entities;
using static Survive.Game1;
using Survive.Input.InputManagers;
using Survive.Input.Data;
using Survive.WorldManagement.Worlds;

namespace Survive.SplashScreens
{


    public class SettingsScreen : SplashScreen
    {
        bool ready;
        Texture2D keyboardTexture;
        Rectangle keyboardRect;

        WorldBase world;

        Dictionary<Rectangle, BinaryInputManager> inputMap;
        Dictionary<BinaryInputManager, Rectangle> reverseInputMap;
        Dictionary<Rectangle, String> actionMap;
        Dictionary<String, Rectangle> reverseActionMap;

        Rectangle moveLeftIcon;
        Rectangle moveRightIcon;
        Rectangle jumpIcon;
        Rectangle viewInventoryIcon;
        Rectangle buildIcon;
        Rectangle[] keyedItemRects;
        Rectangle cardRect0;
        Rectangle cardRect1;
        Rectangle useRect;

        Point mouseLoc;

        const int frameSwitchPoint = 5;
        int frameSwitcher = frameSwitchPoint;
        int currentFrame = 0;
        Texture2D runTex;
        Texture2D jumpTex;
        int jumpTexOffset;

        Rectangle selectedRect;
        bool inputSelected = false;//Toggle. True is that the player dragged from input (keyboard/mouse), false is that the player dragged from action (move left/right etc)
        bool drawLineToMouse = false;

        public SettingsScreen(int saveslot, WorldBase world)
        {
            this.world = world;
            keyboardTexture = Game1.KeyboardTex;
            int screenW = Game1.instance.graphics.PreferredBackBufferWidth;
            int screenH = Game1.instance.graphics.PreferredBackBufferHeight;

            int offsetX = (int)(screenW * .1f);
            int offsetY = (int)(screenH * .1f);

            keyboardRect = new Rectangle(screenW / 2 - keyboardTexture.Width / 2, offsetY, keyboardTexture.Width, keyboardTexture.Height);
            moveLeftIcon = new Rectangle(screenW / 2 - 100, keyboardRect.Bottom + offsetY, 20, 46);
            moveRightIcon = new Rectangle(moveLeftIcon.Right + 15, keyboardRect.Bottom + offsetY, 20, 46);
            jumpIcon = new Rectangle(moveRightIcon.Right + 15, keyboardRect.Bottom + offsetY, 20, 46);
            
            viewInventoryIcon = new Rectangle(jumpIcon.Right + 20, keyboardRect.Bottom + offsetY, Game1.backpackTex.Width, Game1.backpackTex.Height);
            buildIcon = new Rectangle(viewInventoryIcon.X, viewInventoryIcon.Bottom + 15, Game1.UIInventory_BuildButton.Width, Game1.UIInventory_BuildButton.Height);
            useRect = new Rectangle(viewInventoryIcon.X, buildIcon.Bottom + 15, Game1.handTex.Width, Game1.handTex.Height);
            //cardRect0 = new Rectangle(viewInventoryIcon.Right + 15, offsetY, Game1.abilityUIElement.Width, Game1.abilityUIElement.Height);
            //cardRect1 = new Rectangle(cardRect0.Right + 15, offsetY, Game1.abilityUIElement.Width, Game1.abilityUIElement.Height);
            cardRect0 = new Rectangle(0, Game1.instance.graphics.PreferredBackBufferHeight - Game1.abilityUIElement.Height, Game1.abilityUIElement.Width, Game1.abilityUIElement.Height);
            cardRect1 = new Rectangle(Game1.instance.graphics.PreferredBackBufferWidth - Game1.abilityUIElement.Width, Game1.instance.graphics.PreferredBackBufferHeight - Game1.abilityUIElement.Height, Game1.abilityUIElement.Width, Game1.abilityUIElement.Height);


            keyedItemRects = new Rectangle[world.player.keyedItems.Length];
            int keyedItemWidth = Game1.UIInventory_KeyedItem.Width;
            int spaceBetweenKeyedItems = 20;
            int totalKeyedItemSpace = spaceBetweenKeyedItems * 3 + keyedItemWidth * world.player.keyedItems.Length;
            int keyedItemStartRenderingX = Game1.instance.graphics.PreferredBackBufferWidth / 2 - totalKeyedItemSpace / 2;
            for (int i = 0; i < world.player.keyedItems.Length; i++)
            {
                Rectangle drawUIElementRect = new Rectangle(
                        keyedItemStartRenderingX + keyedItemWidth * i + spaceBetweenKeyedItems * i,
                        Game1.instance.graphics.PreferredBackBufferHeight - Game1.UIInventory_KeyedItem.Height,
                        Game1.UIInventory_KeyedItem.Width,
                        Game1.UIInventory_KeyedItem.Height
                        );

                keyedItemRects[i] = drawUIElementRect;
            }

            actionMap = new Dictionary<Rectangle, string>();
            reverseActionMap = new Dictionary<string, Rectangle>();
            registerAction(moveLeftIcon, "Left");
            registerAction(moveRightIcon, "Right");
            registerAction(jumpIcon, "Up");
            for (int i = 0; i < world.player.keyedItems.Length; i++) { registerAction(keyedItemRects[i], "Inventory_" + i); }
            registerAction(viewInventoryIcon, "Inventory_Display");
            registerAction(buildIcon, "Inventory_Open");
            registerAction(cardRect0, "Ability_0");
            registerAction(cardRect1, "Ability_1");
            registerAction(useRect, "Use");
            //handTex

            //set up input map
            inputMap = new Dictionary<Rectangle, BinaryInputManager>();
            reverseInputMap = new Dictionary<BinaryInputManager, Rectangle>();

             //Number Keys
            registerInput(new Rectangle(keyboardRect.X + 52, keyboardRect.Y + 4, 44, 44), KeyManagerEnumerator.num1);
            registerInput(new Rectangle(keyboardRect.X + 100, keyboardRect.Y + 4, 44, 44), KeyManagerEnumerator.num2);
            registerInput(new Rectangle(keyboardRect.X + 148, keyboardRect.Y + 4, 44, 44), KeyManagerEnumerator.num3);
            registerInput(new Rectangle(keyboardRect.X + 196, keyboardRect.Y + 4, 44, 44), KeyManagerEnumerator.num4);
            registerInput(new Rectangle(keyboardRect.X + 244, keyboardRect.Y + 4, 44, 44), KeyManagerEnumerator.num5);
            registerInput(new Rectangle(keyboardRect.X + 292, keyboardRect.Y + 4, 44, 44), KeyManagerEnumerator.num6);
            registerInput(new Rectangle(keyboardRect.X + 340, keyboardRect.Y + 4, 44, 44), KeyManagerEnumerator.num7);
            registerInput(new Rectangle(keyboardRect.X + 388, keyboardRect.Y + 4, 44, 44), KeyManagerEnumerator.num8);
            registerInput(new Rectangle(keyboardRect.X + 436, keyboardRect.Y + 4, 44, 44), KeyManagerEnumerator.num9);
            registerInput(new Rectangle(keyboardRect.X + 484, keyboardRect.Y + 4, 44, 44), KeyManagerEnumerator.num0);

            //Row 1
            registerInput(new Rectangle(keyboardRect.X + 4, keyboardRect.Y + 52, 68, 44), KeyManagerEnumerator.utilTab);
            registerInput(new Rectangle(keyboardRect.X + 76, keyboardRect.Y + 52, 44, 44), KeyManagerEnumerator.letrQ);
            registerInput(new Rectangle(keyboardRect.X + 124, keyboardRect.Y + 52, 44, 44), KeyManagerEnumerator.letrW);
            registerInput(new Rectangle(keyboardRect.X + 172, keyboardRect.Y + 52, 44, 44), KeyManagerEnumerator.letrE);
            registerInput(new Rectangle(keyboardRect.X + 220, keyboardRect.Y + 52, 44, 44), KeyManagerEnumerator.letrR);
            registerInput(new Rectangle(keyboardRect.X + 268, keyboardRect.Y + 52, 44, 44), KeyManagerEnumerator.letrT);
            registerInput(new Rectangle(keyboardRect.X + 316, keyboardRect.Y + 52, 44, 44), KeyManagerEnumerator.letrY);
            registerInput(new Rectangle(keyboardRect.X + 364, keyboardRect.Y + 52, 44, 44), KeyManagerEnumerator.letrU);
            registerInput(new Rectangle(keyboardRect.X + 412, keyboardRect.Y + 52, 44, 44), KeyManagerEnumerator.letrI);
            registerInput(new Rectangle(keyboardRect.X + 460, keyboardRect.Y + 52, 44, 44), KeyManagerEnumerator.letrO);
            registerInput(new Rectangle(keyboardRect.X + 508, keyboardRect.Y + 52, 44, 44), KeyManagerEnumerator.letrP);

            //Row 2
            registerInput(new Rectangle(keyboardRect.X + 88, keyboardRect.Y + 100, 44, 44), KeyManagerEnumerator.letrA);
            registerInput(new Rectangle(keyboardRect.X + 136, keyboardRect.Y + 100, 44, 44), KeyManagerEnumerator.letrS);
            registerInput(new Rectangle(keyboardRect.X + 184, keyboardRect.Y + 100, 44, 44), KeyManagerEnumerator.letrD);
            registerInput(new Rectangle(keyboardRect.X + 232, keyboardRect.Y + 100, 44, 44), KeyManagerEnumerator.letrF);
            registerInput(new Rectangle(keyboardRect.X + 280, keyboardRect.Y + 100, 44, 44), KeyManagerEnumerator.letrG);
            registerInput(new Rectangle(keyboardRect.X + 328, keyboardRect.Y + 100, 44, 44), KeyManagerEnumerator.letrH);
            registerInput(new Rectangle(keyboardRect.X + 376, keyboardRect.Y + 100, 44, 44), KeyManagerEnumerator.letrJ);
            registerInput(new Rectangle(keyboardRect.X + 424, keyboardRect.Y + 100, 44, 44), KeyManagerEnumerator.letrK);
            registerInput(new Rectangle(keyboardRect.X + 472, keyboardRect.Y + 100, 44, 44), KeyManagerEnumerator.letrL);

            //Row 3
            registerInput(new Rectangle(keyboardRect.X + 4, keyboardRect.Y + 148, 104, 44), KeyManagerEnumerator.utilLeftShift);
            registerInput(new Rectangle(keyboardRect.X + 112, keyboardRect.Y + 148, 44, 44), KeyManagerEnumerator.letrZ);
            registerInput(new Rectangle(keyboardRect.X + 160, keyboardRect.Y + 148, 44, 44), KeyManagerEnumerator.letrX);
            registerInput(new Rectangle(keyboardRect.X + 208, keyboardRect.Y + 148, 44, 44), KeyManagerEnumerator.letrC);
            registerInput(new Rectangle(keyboardRect.X + 256, keyboardRect.Y + 148, 44, 44), KeyManagerEnumerator.letrV);
            registerInput(new Rectangle(keyboardRect.X + 304, keyboardRect.Y + 148, 44, 44), KeyManagerEnumerator.letrB);
            registerInput(new Rectangle(keyboardRect.X + 352, keyboardRect.Y + 148, 44, 44), KeyManagerEnumerator.letrN);
            registerInput(new Rectangle(keyboardRect.X + 400, keyboardRect.Y + 148, 44, 44), KeyManagerEnumerator.letrM);

            //Bottom Row
            registerInput(new Rectangle(keyboardRect.X + 4, keyboardRect.Y + 196, 68, 44), KeyManagerEnumerator.utilLeftControl);
            registerInput(new Rectangle(keyboardRect.X + 196, keyboardRect.Y + 196, 272, 44), KeyManagerEnumerator.utilSpace);
            
            //Mouse
            registerInput(new Rectangle(keyboardRect.X + 596, keyboardRect.Y + 63, 50, 48), KeyManagerEnumerator.LMB);
            registerInput(new Rectangle(keyboardRect.X + 656, keyboardRect.Y + 63, 50, 48), KeyManagerEnumerator.RMB);
        }

        private void registerInput(Rectangle rect, BinaryInputManager manager)
        {
            inputMap.Add(rect, manager);
            reverseInputMap.Add(manager, rect);
        }

        private void registerAction(Rectangle rect, string action)
        {
            actionMap.Add(rect, action);
            reverseActionMap.Add(action, rect);
        }

        public override void acceptInput(GameTime time, KeyboardState currentKeyboardState, MouseState currentMouseState, KeyboardState prevKeyboardState, MouseState prevMouseState)
        {
            //jumpTexOffset++;

            frameSwitcher--;
            if (frameSwitcher <= 0)
            {
                frameSwitcher = frameSwitchPoint;
                currentFrame++;
                
                if (world.player is Player)
                {
                    PlayerAnimationPackage animationPackage = ((Player)world.player).animationPackage;

                    runTex = animationPackage.runTex[currentFrame % animationPackage.runTex.Length];
                    jumpTex = animationPackage.jumpTex[0];
                    jumpTexOffset = -(currentFrame % 20);
                }
                
            }

            mouseLoc = currentMouseState.Position;
            if (currentMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
            {
                if (keyboardRect.Contains(mouseLoc))
                {
                    foreach (Rectangle keyRect in inputMap.Keys)
                    {
                        if (keyRect.Contains(mouseLoc))
                        {
                            inputSelected = true;
                            selectedRect = keyRect;
                        }
                    }
                }else
                {
                    foreach(Rectangle rect in actionMap.Keys)
                    {
                        if(rect.Contains(mouseLoc))
                        {
                            inputSelected = false;
                            selectedRect = rect;
                            break;
                        }
                    }
                }
            }

            if(currentMouseState.LeftButton == ButtonState.Pressed && selectedRect != Rectangle.Empty)
            {
                drawLineToMouse = true;
            }

            if (currentMouseState.LeftButton == ButtonState.Released)
            {
                if(selectedRect != Rectangle.Empty)
                {
                    if(inputSelected)
                    {
                        foreach(Rectangle rect in actionMap.Keys)
                        {
                            if(rect.Contains(mouseLoc))
                            {
                                Game1.keyBindManager.bindings[actionMap[rect]] = inputMap[selectedRect];
                            }
                        }
                    }else
                    {
                        foreach (Rectangle rect in inputMap.Keys)
                        {
                            if (rect.Contains(mouseLoc))
                            {
                                Game1.keyBindManager.bindings[actionMap[selectedRect]] = inputMap[rect];
                            }
                        }
                    }
                }

                selectedRect = Rectangle.Empty;
                drawLineToMouse = false;
            }

            if (currentKeyboardState.IsKeyDown(Keys.Escape) && prevKeyboardState.IsKeyUp(Keys.Escape))
            {
                keepGoing();
            }
        }


        public void keepGoing()
        {
            if (world is TutorialWorld)
            {
                ((TutorialWorld)world).processInstructionStrings();
            }
            SoundManager.resume();
            world.decorator.ambientSoundManager.resume();
            ready = true;
            GameSaverAndLoader.saveKeyBinds(Game1.selectedSaveSlot);
        }

        public override bool canContinue()
        {
            return ready;
        }

        public override void draw(SpriteBatch batch)
        {
            batch.Draw(keyboardTexture, keyboardRect, Color.White * .7f);
            foreach (Rectangle keyRect in inputMap.Keys)
            {
                if(keyRect.Contains(mouseLoc))
                {
                    batch.Draw(Game1.block, keyRect, Color.White * .3f);
                }
            }

            KeyBindManager bindManager = Game1.keyBindManager;
            foreach (string str in bindManager.bindings.Keys)
            {
                if (reverseInputMap.ContainsKey(bindManager.bindings[str]) && reverseActionMap.ContainsKey(str))
                {
                    Rectangle inputRectangle = reverseInputMap[bindManager.bindings[str]];
                    Rectangle actionRectangle = reverseActionMap[str];
                    if (inputRectangle.Contains(mouseLoc) || actionRectangle.Contains(mouseLoc))
                    {
                        Game1.DrawLine(batch, inputRectangle.Center.ToVector2(), actionRectangle.Center.ToVector2(), Color.RosyBrown);
                        batch.Draw(Game1.block, inputRectangle, Color.RosyBrown * .5f);
                        batch.Draw(Game1.block, actionRectangle, Color.RosyBrown * .5f);
                    }
                    batch.Draw(Game1.block, reverseInputMap[bindManager.bindings[str]], Color.White * .5f);
                }

            }


            if (runTex != null)
            {
                batch.Draw(runTex, new Rectangle(moveLeftIcon.X - runTex.Width / 2 + moveLeftIcon.Width / 2, moveLeftIcon.Y - runTex.Height / 2 + moveLeftIcon.Height / 2, runTex.Width, runTex.Height), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                batch.Draw(runTex, new Rectangle(moveRightIcon.X - runTex.Width / 2 + moveRightIcon.Width / 2, moveRightIcon.Y - runTex.Height / 2 + moveRightIcon.Height / 2, runTex.Width, runTex.Height), null, Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            }

            if(jumpTex != null)
            {
                batch.Draw(jumpTex, new Rectangle(jumpIcon.X - jumpTex.Width / 2 + jumpIcon.Width / 2, jumpIcon.Y + jumpTexOffset - jumpTex.Height / 2 + jumpIcon.Height / 2, jumpTex.Width, jumpTex.Height), null, Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            }
            
            batch.Draw(Game1.backpackTex, viewInventoryIcon, Color.White);
            batch.Draw(Game1.UIInventory_BuildButton, buildIcon, Color.White);

            
            for (int i = 0; i < world.player.keyedItems.Length; i++)
            {
                batch.Draw(Game1.UIInventory_KeyedItem, keyedItemRects[i], Color.DarkGray);

                if (world.player.keyedItems[i] != null)
                {
                    Rectangle drawItemRect = new Rectangle(
                         keyedItemRects[i].X + 15,
                         keyedItemRects[i].Y + 15,
                         world.player.keyedItems[i].texture.Width,
                         world.player.keyedItems[i].texture.Height
                        );

                    batch.Draw(world.player.keyedItems[i].texture, drawItemRect, Color.White * .5f);
                }
            }

            if (world.player.cards[0] != null || world.player.cards[1] != null || reverseInputMap[Game1.keyBindManager.bindings["Ability_0"]].Contains(mouseLoc))
            {
                batch.Draw(Game1.abilityUIElement, cardRect0, Color.White);
            }
            if (world.player.cards[0] != null || world.player.cards[1] != null || reverseInputMap[Game1.keyBindManager.bindings["Ability_1"]].Contains(mouseLoc))
            {
                batch.Draw(Game1.abilityUIElement, cardRect1, null, Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            }

            
            batch.Draw(Game1.handTex, useRect, Color.White);

            if (world.player.cards[0] != null && world.player.cards[0].usable)
            {
                batch.Draw(world.player.cards[0].iconTex, new Rectangle(cardRect0.X + 42, cardRect0.Y + 5, world.player.cards[0].iconTex.Width, world.player.cards[0].iconTex.Height), Color.White);
            }

            if (world.player.cards[1] != null && world.player.cards[1].usable)
            {
                batch.Draw(world.player.cards[1].iconTex, new Rectangle(cardRect1.X + 42, cardRect1.Y + 5, world.player.cards[1].iconTex.Width, world.player.cards[1].iconTex.Height), Color.White);
            }

            if (drawLineToMouse)
            {
                batch.Draw(Game1.block, selectedRect, Color.RosyBrown * .7f);
                Game1.DrawLine(batch, mouseLoc.ToVector2(), selectedRect.Center.ToVector2(), Color.RosyBrown);
            }

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
            return true;
        }



        
    }
}
