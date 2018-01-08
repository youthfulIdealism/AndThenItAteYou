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
using Survive.WorldManagement.Tile;
using Survive.WorldManagement.Entities.Particles;
using Survive.Sound;
using Survive.Input.Data;
using Survive.WorldManagement.Worlds;

namespace Survive.SplashScreens
{
    public class MainMenu : SplashScreen
    {
        static bool hasAddedPotionsToHarvestDictionary;
        private Rectangle tutorialRect;
        private Rectangle newGameRect;
        private Rectangle continueRect;
        private Rectangle exitRect;
        MouseState currentMouseState;
        bool readyToContinue = false;
        bool allowResume = false;

        

        public MainMenu()
        {
            int screenW = Game1.instance.graphics.PreferredBackBufferWidth;
            int screenH = Game1.instance.graphics.PreferredBackBufferHeight;
            int screenWidthOverFour = Game1.instance.graphics.PreferredBackBufferWidth / 4;
            int screenHeightOverTwo = Game1.instance.graphics.PreferredBackBufferHeight / 2;
            tutorialRect = new Rectangle(screenWidthOverFour - 50, screenHeightOverTwo, 75, 40);
            newGameRect = new Rectangle(screenWidthOverFour * 2 - 50, screenHeightOverTwo, 112, 40);
            continueRect = new Rectangle(screenWidthOverFour * 3 - 50, screenHeightOverTwo, 85, 40);
            allowResume = GameSaverAndLoader.doesSaveExist(Game1.selectedSaveSlot);
            exitRect = new Rectangle(screenW - 30 - 10, 10, 30,  30);
        }
        
        public override void acceptInput(GameTime time, KeyboardState currentKeyboardState, MouseState currentMouseState, KeyboardState prevKeyboardState, MouseState prevMouseState)
        {
           this.currentMouseState = currentMouseState;
           if (currentMouseState.LeftButton.Equals(ButtonState.Pressed) && prevMouseState.LeftButton.Equals(ButtonState.Released))
           {
                if(tutorialRect.Contains(currentMouseState.Position))
                {
                    SoundManager.getSound("click").playWithVariance(0, .25f, 0, SoundType.MONSTER);

                    Game1.instance.world = null;
                    World.universeProperties = new UniverseProperties("" + new Random(System.DateTime.Now.Millisecond).NextDouble() + "" + new Random(System.DateTime.Now.Minute).NextDouble() + "" + new Random((int)System.DateTime.Now.Ticks).NextDouble() + "" + new Random().NextDouble());
                    WorldBase world = new TutorialWorld();
                    Game1.instance.queuedSplashScreens.Add(new PlayerSelectScreen(MetaData.unlocks.ToArray(), world));
                    Card.setUpCards();

                    readyToContinue = true;
                    Game1.isInTutorial = true;
                }
                if (newGameRect.Contains(currentMouseState.Position))
                {
                    SoundManager.getSound("click").playWithVariance(0, .25f, 0, SoundType.MONSTER);

                    Game1.instance.world = null;
                    World.universeProperties = new UniverseProperties("" + new Random(System.DateTime.Now.Millisecond).NextDouble() + "" + new Random(System.DateTime.Now.Minute).NextDouble() + "" + new Random((int)System.DateTime.Now.Ticks).NextDouble() + "" + new Random().NextDouble());
                    WorldBase world = Game1.instance.getWorldBasedOnDifficulty(0);
                    Game1.instance.queuedSplashScreens.Add(new PlayerSelectScreen(MetaData.unlocks.ToArray(), world));
                    Card.setUpCards();

                    handleBottleAdditions();

                    Game1.instance.world = null;

                    readyToContinue = true;
                    Game1.isInTutorial = false;
                }
                if (continueRect.Contains(currentMouseState.Position) && allowResume)
                {
                    SoundManager.getSound("click").playWithVariance(0, .25f, 0, SoundType.MONSTER);
                    GameSaverAndLoader.load(Game1.selectedSaveSlot);
                   

                    handleBottleAdditions();
                    readyToContinue = true;
                    Game1.isInTutorial = false;
                }
                if(exitRect.Contains(currentMouseState.Position))
                {
                    Game1.instance.Exit();
                }
            }
        }

        public void handleBottleAdditions()
        {
            if(!hasAddedPotionsToHarvestDictionary)
            {
                ItemDropper dropTreasuresRarely = new ItemDropper();
                dropTreasuresRarely.registerNewDrop(new Item_Rope(0), null, 1, .01f);
                dropTreasuresRarely.registerNewDrop(new Item_Bottle_Type_0(0), null, 1, .01f);
                dropTreasuresRarely.registerNewDrop(new Item_Bottle_Type_1(0), null, 1, .01f);
                dropTreasuresRarely.registerNewDrop(new Item_Bottle_Type_2(0), null, 1, .01f);
                dropTreasuresRarely.registerNewDrop(new Item_Bottle_Type_3(0), null, 1, .01f);
                dropTreasuresRarely.registerNewDrop(new Item_Meat(0), null, 1, .01f);
                HarvestDictionary.registerNewHarvest(TileTypeReferencer.POT, new ItemDropper[] { dropTreasuresRarely });

                hasAddedPotionsToHarvestDictionary = true;
            }
        }

        public override bool canContinue()
        {
            return readyToContinue;
        }

        public override void draw(SpriteBatch batch)
        {
            Color resumeColor = Color.DarkGray;

            if(allowResume)
            {
                resumeColor = Color.White;
            }

            batch.Draw(Game1.UIMainMenu_tutorial, tutorialRect, Color.White);
            batch.Draw(Game1.UIMainMenu_new, newGameRect, Color.White);
            batch.Draw(Game1.UIMainMenu_continue, continueRect, resumeColor);
            batch.Draw(Game1.texture_x, exitRect, Color.White);

            if (tutorialRect.Contains(currentMouseState.Position))
            {
                batch.Draw(Game1.block, tutorialRect, Color.White * .2f);
            }
            if (newGameRect.Contains(currentMouseState.Position))
            {
                batch.Draw(Game1.block, newGameRect, Color.White * .2f);
            }
            if (continueRect.Contains(currentMouseState.Position) && allowResume)
            {
                batch.Draw(Game1.block, continueRect, Color.White * .2f);
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
