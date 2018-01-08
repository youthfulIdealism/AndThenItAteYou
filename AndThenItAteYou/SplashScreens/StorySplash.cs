using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Survive.SplashScreens
{
    public class StorySplash : SplashScreen
    {

        public ContentManager content;
        public Texture2D texture;
        const int maxTime = 200;
        int timeLeft = maxTime;

        public StorySplash(String imagePath)
        {
            content = new ContentManager(Game1.instance.Content.ServiceProvider, Game1.instance.Content.RootDirectory);
            texture = content.Load<Texture2D>(imagePath);
        }

        public override void acceptInput(GameTime time, KeyboardState currentKeyboardState, MouseState currentMouseState, KeyboardState prevKeyboardState, MouseState prevMouseState)
        {
            timeLeft--;
            if(timeLeft < 0) { content.Dispose(); }
        }

        public override bool canContinue()
        {
            return timeLeft < 0;
        }

        public override void draw(SpriteBatch batch)
        {
            if(texture != null && !texture.IsDisposed)
            {
                batch.Draw(texture, new Rectangle(Game1.instance.graphics.PreferredBackBufferWidth / 2 - texture.Width / 2, Game1.instance.graphics.PreferredBackBufferHeight / 2 - texture.Height / 2, texture.Width, texture.Height), Color.White * ((float)timeLeft / (maxTime * .3f)));
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
