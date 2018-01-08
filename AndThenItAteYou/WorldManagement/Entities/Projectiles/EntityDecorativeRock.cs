using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Survive.Sound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities.Projectiles
{
    public class EntityDecorativeRock : Entity
    {
        Texture2D texture;
        float currentRotation;
        float deltaRotation;
        

        public EntityDecorativeRock(Vector2 location, WorldBase world) : base(location, world)
        {
            width = 7 + rand.Next(5);
            height = 7 + rand.Next(5);
            texture = Game1.texture_item_stone;
            currentRotation = (float)rand.NextDouble() * (float)Math.PI * 2;
            deltaRotation = (float)rand.NextDouble() * .4f - .2f;
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);

            currentRotation += deltaRotation;

            if (collideTop || collideBottom || ticksExisted >= 200)
            {
                world.killEntity(this);
                SoundManager.getSound("rock-fall").playWithVariance(0, .25f, (location - world.player.location).X, SoundType.MONSTER);
            }
        }

        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            SpriteEffects effect = SpriteEffects.None;
            if (velocity.X > 0)
            {
                effect = SpriteEffects.FlipHorizontally;
            }

            Rectangle defaultRect = getCollisionBox().ToRect();
            batch.Draw(texture, new Rectangle(defaultRect.X + offset.X, defaultRect.Y + offset.Y, (int)width, (int)height), null, groundColor, currentRotation, Vector2.Zero, effect, 0);
        }
    }
}
