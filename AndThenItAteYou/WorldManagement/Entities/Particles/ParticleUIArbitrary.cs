using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities.Particles
{
    public class ParticleUIArbitrary : Particle, DelayedRenderable
    {
        public Texture2D texture;
        public float rotation;
        public float deltaRotation;
        public Color startColor;
        public Color endColor;
        public float velocityMultiplier;
        public bool flip;
        public ParticleUIArbitrary(Vector2 location, WorldBase world, Vector2 direction, int duration, Texture2D texture) : base(location, world, duration)
        {
            this.location = location;
            velocity = direction;
            this.texture = texture;
            startColor = world.decorator.colorManager.groundColor;
            endColor = world.decorator.colorManager.groundColor;
            width = texture.Width;
            height = texture.Height;
            rotation = 0;
            deltaRotation = 0;
            velocityMultiplier = 1;
            this.gravityMultiplier = -.1f;
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);
            rotation += deltaRotation;
            velocity *= velocityMultiplier;
            world.requestDelayedRender(this);
        }

        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            
        }

        public void draw(SpriteBatch batch, GameTime time, Point offset)
        {
            SpriteEffects effect = SpriteEffects.None;
            if (flip)
            {
                effect = SpriteEffects.FlipHorizontally;
            }

            Rectangle defaultRect = getCollisionBox().ToRect();
            batch.Draw(texture, new Rectangle(defaultRect.X, defaultRect.Y, (int)defaultRect.Width, (int)defaultRect.Height), null, Color.Lerp(startColor, endColor, getPercentageCompleted()) * (1 - getPercentageCompleted()), rotation, Vector2.Zero, effect, 0);
        }
    }
}
