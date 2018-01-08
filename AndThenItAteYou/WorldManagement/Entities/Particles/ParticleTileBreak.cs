using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Survive.WorldManagement.Tile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities.Particles
{
    public class ParticleTileBreak : Particle
    {
        public Texture2D texture;
        //public Color color;
        public float rotation;
        public float deltaRotation;
        int sourceX;
        int sourceY;
        public ParticleTileBreak(Vector2 location, WorldBase world, Vector2 direction, TileType tile, int duration) : base(location, world, duration)
        {
            velocity = direction + new Vector2((float)rand.NextDouble() * 4 - 2, (float)rand.NextDouble() * -5f);
            texture = tile.texture;
            width = 20;
            height = 20;
            rotation = (float)(rand.NextDouble() * Math.PI * 2);
            deltaRotation = (float)(rand.NextDouble() * .15 - .0525);

            sourceX = rand.Next(texture.Width - (int)width);
            sourceY = rand.Next(texture.Height - (int)width);
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);

            velocity *= frictionMultiplier;
            deltaRotation *= .99f;
            rotation += deltaRotation;
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
                new Rectangle(sourceX, sourceY, (int)width, (int)height),
                groundColor * (1 - (float)ticksExisted / duration),
                rotation,
                Vector2.Zero/*new Vector2(texture.Width / 2, texture.Height / 2)*/, SpriteEffects.None, 0);
        }
    }
}
