using Microsoft.Xna.Framework.Graphics;
using Survive.WorldManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Weather
{
    public class Cloud
    {
        public AABB bounds { get; set; }
        public Texture2D textureFront;
        public Texture2D textureBack;
        public Cloud(AABB location, Random rand)
        {
            this.bounds = location;
            textureFront = Game1.cloud[rand.Next(Game1.cloud.Length)];
            textureBack = Game1.cloud[rand.Next(Game1.cloud.Length)];
        }

        public void shift(float amt)
        {
            this.bounds = new AABB(bounds.X + amt, bounds.Y, bounds.Width, bounds.Height);
        }
    }
}
