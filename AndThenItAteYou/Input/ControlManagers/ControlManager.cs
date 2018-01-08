using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.Input.ControlManagers
{
    public abstract class ControlManager
    {
        public ControlManager switchedFrom { get; set; }

        public abstract void acceptInput(GameTime time, KeyboardState currentKeyboardState, MouseState currentMouseState, KeyboardState prevKeyboardState, MouseState prevMouseState);
        public virtual void switchTo(ControlManager switchedFrom)
        {
            this.switchedFrom = switchedFrom;
        }

        public virtual void draw(SpriteBatch batch)
        {

        }
    }
}
