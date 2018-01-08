using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive
{
    public static class Extensions
    {
        public static Texture2D[] loadTextureRange(this ContentManager Content, String prefix, int amount)
        {
            Texture2D[] receptacle = new Texture2D[amount + 1];
            for (int i = 0; i <= amount; i++)
            {
                receptacle[i] = Content.Load<Texture2D>(prefix + i);
            }
            return receptacle;
        }
    }
}
