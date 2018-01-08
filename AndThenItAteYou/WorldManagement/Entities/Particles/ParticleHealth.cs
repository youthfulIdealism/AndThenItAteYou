using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities.Particles
{
    public class ParticleHealth : Particle
    {
        public Texture2D texture;
        //public Color color;
        public float rotation;
        public float deltaRotation;
        public ParticleHealth(Vector2 location, WorldBase world, Vector2 direction, int duration) : base(location, world, duration)
        {
            this.location += new Vector2((float)rand.NextDouble() * 100 - 50, (float)rand.NextDouble() * 100 - 50);
            velocity = new Vector2((float)rand.NextDouble() * 2 + direction.X, (float)rand.NextDouble() * -2 + direction.Y);
            texture = Game1.icon_health;
            //color = Color.White * .5f;
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
            /*batch.Draw(texture,
                new Vector2(),
                new Rectangle(baseDrawRect.X + offset.X, baseDrawRect.Y + offset.Y, baseDrawRect.Width, baseDrawRect.Height),
                new Rectangle(0, 0, texture.Width, texture.Height),
                new Vector2(),
                0,
                
                SpriteEffects.None);*/

            batch.Draw(texture,
                new Rectangle(baseDrawRect.X + offset.X, baseDrawRect.Y + offset.Y, (int)width, (int)height),
                null,
                Color.Lerp(groundColor, Color.Red, (/*1 - */(float)ticksExisted / duration)),
                rotation,
                /*Vector2.Zero*/new Vector2(texture.Width / 2, texture.Height / 2), SpriteEffects.None, 0);
        }
    }
}
