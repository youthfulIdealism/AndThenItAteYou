using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Survive.SplashScreens
{
    public class ScrollingSplashScreen : SplashScreen
    {
        float scrollRate;
        float scrollPosition;
        float finishPosition;
        public ContentManager content;
        public Texture2D background;
        public Dictionary<Rectangle, Texture2D> displayedImages;
        public Dictionary<Vector2, String> creditStrings;
        private bool isFinished = false;

        public ScrollingSplashScreen(Dictionary<Rectangle, string> imagePaths, List<String> creditStrings)
        {
            scrollPosition = -1000;
            content = new ContentManager(Game1.instance.Content.ServiceProvider, Game1.instance.Content.RootDirectory);
            background = content.Load<Texture2D>("StorySplashes/stonewrap");
            Random rand = new Random();

            this.creditStrings = new Dictionary<Vector2, String>();
            displayedImages = new Dictionary<Rectangle, Texture2D>();

            foreach (Rectangle rect in imagePaths.Keys)
            {
                displayedImages[rect] = content.Load<Texture2D>(imagePaths[rect]);
            }

            int counter = 0;
            foreach(String str in creditStrings)
            {
                int creditStringY = 20;
                if (rand.NextDouble() <= .5)
                {
                    creditStringY = Game1.instance.graphics.PreferredBackBufferHeight - 40;
                }
                this.creditStrings.Add(new Vector2(400 * counter, creditStringY), str);
                counter++;
            }

            scrollRate = 3;

            finishPosition = 0;
            foreach(Rectangle rect in imagePaths.Keys)
            {
                if(rect.Right > finishPosition) { finishPosition = rect.Right; }
            }
            
            if(finishPosition < creditStrings.Count * 400) { finishPosition = creditStrings.Count * 400; }
            finishPosition -= Game1.instance.graphics.PreferredBackBufferWidth;
        }


        public override void acceptInput(GameTime time, KeyboardState currentKeyboardState, MouseState currentMouseState, KeyboardState prevKeyboardState, MouseState prevMouseState)
        {
            
            if (currentKeyboardState.IsKeyDown(Keys.Space))
            {
                scrollPosition += scrollRate * 7;
            }
            else
            {
                scrollPosition += scrollRate;
            }

            if(scrollPosition > finishPosition || currentKeyboardState.IsKeyDown(Keys.Escape))
            {
                isFinished = true;
            }
            
            
        }

        public override bool canContinue()
        {
            return isFinished;
        }

        public override void draw(SpriteBatch batch)
        {
            int scrollPos = (int)scrollPosition;
            for(int i = -1; i <= 1; i++) 
            {
                batch.Draw(background, new Rectangle(background.Width * i - (scrollPos % background.Width), Game1.instance.graphics.PreferredBackBufferHeight / 2 - background.Height / 2, background.Width, background.Height), Color.White);
            }

            foreach (Rectangle rect in displayedImages.Keys)
            {
                batch.Draw(displayedImages[rect], new Rectangle(-scrollPos + rect.X, rect.Y, displayedImages[rect].Width, displayedImages[rect].Height), Color.Black);
            }

            foreach (Vector2 loc in creditStrings.Keys)
            {
                //batch.Draw(displayedImages[loc], new Rectangle(-scrollPos + rect.X, rect.Y, rect.Width, rect.Height), Color.Black);
                batch.DrawString(Game1.defaultFont, creditStrings[loc], loc + new Vector2(-scrollPos, 0), Color.White);
            }
        }

        public override bool isBlockingDrawCalls()
        {
            return true;
        }

        public override void switchTo()
        {
            
        }
    }
}
