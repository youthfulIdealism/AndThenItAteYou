using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Survive.WorldManagement.Tile.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities.Particles
{
    public class ParticleBubble : Particle
    {
        public Texture2D texture;
        public Color color;

        public ParticleBubble(Vector2 location, WorldBase world, Vector2 direction, int duration) : base(location, world, duration)
        {
            velocity = direction * rand.Next(5) + new Vector2((float)rand.NextDouble() - .5f, -(float)rand.NextDouble() * 2);
            texture = Game1.texture_particle_bubble;
            width = 2 + rand.Next(8);
            height = 2 + rand.Next(8);
            this.gravityMultiplier = -.5f;
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);

            tileIn = world.getBlock(location);
            if (tileIn == null || !tileIn.tags.Contains(TagReferencer.WATER))
            {
                world.killEntity(this);
            }
        }

        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            Rectangle baseDrawRect = getCollisionBox().ToRect();
            //batch.Draw(texture, new Rectangle(baseDrawRect.X + offset.X, baseDrawRect.Y + offset.Y, baseDrawRect.Width, baseDrawRect.Height), Color.Lerp(color, Color.Red, getPercentageCompleted()) * (1 - getPercentageCompleted()));
            float currentWidth = (1 - this.getPercentageCompleted()) * width;
            float currentHeight = (1 - this.getPercentageCompleted()) * height;

            batch.Draw(texture,
                new Rectangle(baseDrawRect.X + offset.X, baseDrawRect.Y + offset.Y, (int)currentWidth, (int)currentHeight),
                null,
                world.decorator.colorManager.getSkyColorGivenTimeOfDay(world.timeOfDay),
                0,
                new Vector2(texture.Width / 2, texture.Height / 2), SpriteEffects.None, 0);
        }
    }
}
