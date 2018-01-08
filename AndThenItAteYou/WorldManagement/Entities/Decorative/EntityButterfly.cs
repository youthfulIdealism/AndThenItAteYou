using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Survive.Sound;
using Survive.WorldManagement.Inventory;
using Survive.WorldManagement.Inventory.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities
{
    public class EntityButterfly : Entity
    {
        
        Texture2D tex;
        int texCounter;
        Vector2 direction = new Vector2();
        int ticksGoingUp = 40;

        public EntityButterfly(Vector2 location, WorldBase world) : base(location, world)
        {
            this.width = 10;
            this.height = 10;
            tex = Game1.texture_entity_butterfly[0];
            this.health = 1;
            texCounter = 0;
            this.blocksWeaponsOnHit = false;
            this.gravityMultiplier = .05f;
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);
            ticksGoingUp--;
            if (collideBottom)
            {
                ticksGoingUp = 40;
            }

            if(ticksGoingUp > 0)
            {
                direction += new Vector2(0, -1);
            }

            texCounter++;
            tex = Game1.texture_entity_butterfly[(texCounter / 10) % Game1.texture_entity_butterfly.Length];
            direction += new Vector2((float)rand.NextDouble() * 2 - 1, (float)rand.NextDouble() * 2 - 1);

            float distanceFromPlayer = Math.Abs(world.player.location.X - this.location.X);
            if(distanceFromPlayer > Chunk.tileDrawWidth * Chunk.tilesPerChunk * world.tileGenRadious)
            {
                world.killEntity(this);

            }
            direction = Vector2.Normalize(direction) * .4f;
            impulse += direction;

        }

        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            SpriteEffects effect = SpriteEffects.None;
            if (velocity.X < 0)
            {
                effect = SpriteEffects.FlipHorizontally;
            }

            Rectangle defaultRect = getCollisionBox().ToRect();
            batch.Draw(tex, new Rectangle(defaultRect.X + offset.X, defaultRect.Y + offset.Y + 3, tex.Width, tex.Height), null, getDrawColor(groundColor, time), 0, Vector2.Zero, effect, 0);
        }
    }
}
