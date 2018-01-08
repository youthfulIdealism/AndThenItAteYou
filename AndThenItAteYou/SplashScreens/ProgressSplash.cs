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
    public class ProgressSplash : SplashScreen
    {

        public ContentManager content;
        public Texture2D backgroundTexture;
        public Texture2D playerIcon;
        const int maxTime = 200;
        int timeLeft = maxTime;
        int level;

        public ProgressSplash(int level)
        {
            content = new ContentManager(Game1.instance.Content.ServiceProvider, Game1.instance.Content.RootDirectory);
            backgroundTexture = content.Load<Texture2D>("ProgressSplash");
            playerIcon = content.Load<Texture2D>("ProgressSplash_player");
            this.level = level;
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
            if(backgroundTexture != null && !backgroundTexture.IsDisposed)
            {
                Rectangle backgroundRectangle = new Rectangle(Game1.instance.graphics.PreferredBackBufferWidth / 2 - backgroundTexture.Width / 2, Game1.instance.graphics.PreferredBackBufferHeight / 2 - backgroundTexture.Height / 2, backgroundTexture.Width, backgroundTexture.Height);

                batch.Draw(backgroundTexture, backgroundRectangle, Color.White * ((float)timeLeft / (maxTime * .3f)));

                /*Rectangle iconRectangle = new Rectangle(
                    backgroundRectangle.X + 45 + (50 * (level % 6)),
                    backgroundRectangle.Y + 35 + (75 * (int)Math.Floor((float)level / 6)),
                    playerIcon.Width,
                    playerIcon.Height
                    );*/

                Rectangle iconRectangle = new Rectangle(
                    backgroundRectangle.X + 45 + 50 * level,
                    backgroundRectangle.Y + 35,
                    playerIcon.Width,
                    playerIcon.Height
                    );
                batch.Draw(playerIcon, iconRectangle, Color.White * ((float)timeLeft / (maxTime * .3f)));
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
