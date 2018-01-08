using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive
{
    public abstract class SplashScreen
    {
        public abstract void acceptInput(GameTime time, KeyboardState currentKeyboardState, MouseState currentMouseState, KeyboardState prevKeyboardState, MouseState prevMouseState);
        public abstract void switchTo();
        public abstract bool canContinue();
        public abstract void draw(SpriteBatch batch);
        public abstract bool isBlockingDrawCalls();
    }
}
