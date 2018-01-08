using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities.Particles
{
    public class ParticleJumpBoost : Particle
    {
        public Texture2D texture;
        public Color color;
        public ParticleJumpBoost(Vector2 location, WorldBase world, Vector2 direction, int duration) : base(location, world, duration)
        {
            this.location += new Vector2(rand.Next(10) - 5, 0);
            velocity = new Vector2(direction.X + rand.Next(10) - 5, -7 + direction.Y);
            texture = Game1.texture_particle_jump;
            color = Color.Green;
            width = 20;
            height = 20;
            this.gravityMultiplier = -.1f;
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);

            velocity *= .95f;
        }

        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            Rectangle baseDrawRect = getCollisionBox().ToRect();
            batch.Draw(texture, new Rectangle(baseDrawRect.X + offset.X, baseDrawRect.Y + offset.Y, baseDrawRect.Width, baseDrawRect.Height), Color.Lerp(groundColor, color, getPercentageCompleted()) * (1 - getPercentageCompleted()));
        }
    }
}
