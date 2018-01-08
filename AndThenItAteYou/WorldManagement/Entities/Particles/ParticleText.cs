using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities.Particles
{
    public class ParticleText : Particle
    {
        public String text;
        public bool drawContrast;
        public SpriteFont font;
        public ParticleText(Vector2 location, WorldBase world, int duration, string text) : base(location, world, duration)
        {
            width = 200;
            height = 200;
            this.gravityMultiplier = 0;
            this.text = text;
            font = Game1.defaultFont;
        }

        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            if(drawContrast)
            {
                batch.DrawString(font, text, offset.ToVector2() + location + new Vector2(-1, 1), Color.White);
            }
            batch.DrawString(font, text, offset.ToVector2() + location, groundColor);
        }
    }
}
