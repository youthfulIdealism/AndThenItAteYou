using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities.Particles
{
    public class ParticleRecharge : Particle
    {
        public Texture2D texture;
        public Color color;
        public ParticleRecharge(Vector2 location, WorldBase world, Vector2 direction, int duration) : base(location, world, duration)
        {
            velocity = new Vector2((float)rand.NextDouble() * 4 + direction.X, (float)rand.NextDouble() * -2 + direction.Y);
            texture = Game1.texture_item_charmstone;
            color = Color.White * .5f;
            width = 20;
            height = 20;
            this.gravityMultiplier = -.1f;
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);

            velocity *= .9f;
        }

        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            Rectangle baseDrawRect = getCollisionBox().ToRect();


            batch.Draw(texture,
                new Rectangle(baseDrawRect.X + offset.X, baseDrawRect.Y + offset.Y, (int)width, (int)height),
                null,
                color * (1 - (float)ticksExisted / duration),
                0,
                new Vector2(texture.Width / 2, texture.Height / 2), SpriteEffects.None, 0);
        }
    }
}
