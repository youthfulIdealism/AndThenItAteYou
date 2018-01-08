using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities.Particles
{
    public class ParticleRain : Particle
    {
        public Texture2D texture;
        public float completion;
        public Vector2 angle;
        public ParticleRain(Vector2 location, WorldBase world, Vector2 angle) : base(location, world, int.MaxValue)
        {
            this.gravityMultiplier = 0;
            this.angle = angle;
            completion = 0;
        }

        public override void update(GameTime time)
        {
            base.update(time);
            this.velocity = Vector2.Zero;
            this.impulse = Vector2.Zero;
            if (completion <= 1)
            {
                completion = Math.Min(completion + .04f, 2);
            }
            else
            {
                completion = Math.Min(completion + .08f, 2);
            }

            if(completion >= 1.999)
            {
                world.killEntity(this);
            }
        }

        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            //Rectangle baseDrawRect = getCollisionBox().ToRect();
            //batch.Draw(texture, new Rectangle(baseDrawRect.X + offset.X, baseDrawRect.Y + offset.Y, baseDrawRect.Width, baseDrawRect.Height), color * (1 - getPercentageCompleted()));
            if (completion <= 1)
            {
                for(int i = 0; i < 8 * completion; i++)
                {
                    batch.Draw(Game1.rain,
                       new Rectangle((int)location.X + offset.X, (int)location.Y + offset.Y + i * 176, (int)10, (int)(176)),
                       new Rectangle(0, 0, 10, (int)(Game1.rain.Height/* * completion*/)),
                       groundColor,
                       (float)(Math.Atan(angle.Y / angle.X) - Math.PI / 2),
                       Vector2.Zero, SpriteEffects.None, 0);
                }
                
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    batch.Draw(Game1.rain,
                       new Rectangle((int)location.X + offset.X, (int)location.Y + offset.Y + i * 176, (int)10, (int)(176)),
                       new Rectangle(0, 0, 10, (int)(Game1.rain.Height/* * completion*/)),
                       groundColor * (2 - completion),
                       (float)(Math.Atan(angle.Y / angle.X) - Math.PI / 2),
                       Vector2.Zero, SpriteEffects.None, 0);
                }
            }
        }
    }
}
