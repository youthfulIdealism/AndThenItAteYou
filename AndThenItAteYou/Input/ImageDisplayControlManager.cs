using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Survive.WorldManagement.Entities;
using Survive.Input.ControlManagers;

namespace Survive.Input
{
    public class ImageDisplayControlManager : ControlManager
    {
        Texture2D tex;
        PlayerBase user;
        public ImageDisplayControlManager(Texture2D tex, PlayerBase user)
        {
            this.tex = tex;
            this.user = user;
        }

        public override void acceptInput(GameTime time, KeyboardState currentKeyboardState, MouseState currentMouseState, KeyboardState prevKeyboardState, MouseState prevMouseState)
        {
            if(currentMouseState.LeftButton == ButtonState.Released)
            {
                user.currentControlManager = user.movementControlManger;
                user.movementControlManger.switchTo(switchedFrom);
            }
        }

        public override void draw(SpriteBatch batch)
        {
            batch.Draw(tex, new Rectangle(tex.Width / 2, tex.Height / 2, tex.Width, tex.Height), Color.White);
        }
    }
}
