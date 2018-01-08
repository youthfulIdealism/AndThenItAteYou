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
    public class ImageDisplayForTimeControlManager : ControlManager
    {
        Texture2D tex;
        PlayerBase user;
        int currentTime;
        public float scale;
        public ImageDisplayForTimeControlManager(Texture2D tex, PlayerBase user, int time)
        {
            this.tex = tex;
            this.user = user;
            currentTime = time;
        }

        public override void acceptInput(GameTime time, KeyboardState currentKeyboardState, MouseState currentMouseState, KeyboardState prevKeyboardState, MouseState prevMouseState)
        {
            currentTime--;
            if (currentTime <= 0)
            {
                user.currentControlManager = user.movementControlManger;
                user.movementControlManger.switchTo(switchedFrom);
            }
        }
        
        public override void draw(SpriteBatch batch)
        {
            batch.Draw(tex, new Rectangle((int)(Game1.instance.graphics.PreferredBackBufferWidth / 2 + (-tex.Width / 2) * scale), (int)(Game1.instance.graphics.PreferredBackBufferHeight / 2 + (-tex.Height / 2) * scale), (int)(tex.Width * scale), (int)(tex.Height * scale)), Color.White/* * ((float) currentTime / 200)*/);
        }
    }
}
