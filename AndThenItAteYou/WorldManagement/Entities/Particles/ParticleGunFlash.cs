using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities.Particles
{
    public class ParticleGunFlash : Particle
    {
        public Texture2D texture;
        public Color color;
        public ParticleGunFlash(Vector2 location, WorldBase world, int duration) : base(location, world, duration)
        {
            texture = Game1.texture_particle_glow;
            width = 200;
            height = 200;
            color = Color.White;
            this.gravityMultiplier = 0;
        }

        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            Rectangle baseDrawRect = getCollisionBox().ToRect();
            batch.Draw(texture, new Rectangle(baseDrawRect.X + offset.X, baseDrawRect.Y + offset.Y, baseDrawRect.Width, baseDrawRect.Height), color * (1 - getPercentageCompleted()));
        }
    }
}
