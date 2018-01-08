using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities.Particles
{
    public class ParticleGunSparks : Particle
    {
        public Texture2D texture;
        public Color color;
        public Color deathColor;
        public ParticleGunSparks(Vector2 location, WorldBase world, Vector2 direction, int duration) : base(location, world, duration)
        {
            velocity = new Vector2((float)rand.NextDouble() * 10 + direction.X, (float)rand.NextDouble() * 10 - 5 + direction.Y);
            texture = Game1.texture_particle_glow;
            color = Color.Yellow;
            deathColor = Color.Red;
            width = 20;
            height = 20;
            this.gravityMultiplier = .1f;
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);

            velocity *= .9f;
        }

        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            Rectangle baseDrawRect = getCollisionBox().ToRect();
            batch.Draw(texture, new Rectangle(baseDrawRect.X + offset.X, baseDrawRect.Y + offset.Y, baseDrawRect.Width, baseDrawRect.Height), Color.Lerp(color, deathColor, getPercentageCompleted()) * (1 - getPercentageCompleted()));
        }
    }
}
