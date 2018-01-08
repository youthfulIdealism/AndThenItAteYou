using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities.Speech
{
    public class SpeechBubbleManager
    {
        public List<SpeechBubble> displayedBubbles;
        public Entity parent;

        public SpeechBubbleManager(Entity parent)
        {
            displayedBubbles = new List<SpeechBubble>();
            this.parent = parent;
        }

        public void addSpeechBubble(SpeechBubble item)
        {
            displayedBubbles.Add(item);
        }

        public void addSpeechBubble(Texture2D tex)
        {
            displayedBubbles.Add(new SpeechBubble(tex));
        }

        public void update()
        {
            List<SpeechBubble> removedBubbles = new List<SpeechBubble>();
            foreach (SpeechBubble bubble in displayedBubbles)
            {
                bubble.update();
                if (bubble.ticksRemaining <= 0)
                {
                    removedBubbles.Add(bubble);
                }
            }

            foreach (SpeechBubble bubble in removedBubbles)
            {
                displayedBubbles.Remove(bubble);
            }
        }

        public void draw(SpriteBatch batch, GameTime time, Point offset)
        {
            for(int i = 0; i < displayedBubbles.Count; i++)
            {
                displayedBubbles[i].draw(batch, time, offset + parent.location.ToPoint() + new Point((int)parent.width + i * SpeechBubble.wh + 5, -(int)parent.height));
            }
        }


    }
}
