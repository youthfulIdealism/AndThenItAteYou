using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities.Speech
{
    public class SpeechBubble
    {
        public const int wh = 30;
        public const int maxDuration = 400;
        public Texture2D bubbleTex;
        public Texture2D communicated;
        public int ticksRemaining;

        public SpeechBubble(Texture2D communicated)
        {
            bubbleTex = Game1.SpeechBubble;
            this.communicated = communicated;
            ticksRemaining = maxDuration;
        }

        public void update()
        {
            ticksRemaining--;
        }


        public void draw(SpriteBatch batch, GameTime time, Point offset)
        {
            batch.Draw(bubbleTex, new Rectangle(offset.X, offset.Y, wh, wh), Color.White);
            batch.Draw(communicated, new Rectangle(offset.X + wh / 2 - 10, offset.Y + wh / 2 - 10, 20, 20), Color.Black);
        }
    }
}
