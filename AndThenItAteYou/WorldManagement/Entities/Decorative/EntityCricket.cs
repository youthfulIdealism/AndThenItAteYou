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
    public class EntityCricket : Entity, AggroAble
    {
        
        Texture2D tex;
        int texCounter;

        public EntityCricket(Vector2 location, WorldBase world) : base(location, world)
        {
            this.width = 10;
            this.height = 10;
            tex = Game1.texture_entity_cricket[0];
            this.health = 1;
            texCounter = 0;
            this.blocksWeaponsOnHit = false;
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);

            if(!collideBottom)
            {
                texCounter++;
            }
            tex = Game1.texture_entity_cricket[(texCounter / 10) % Game1.texture_entity_cricket.Length];

            float distanceFromPlayer = Math.Abs(world.player.location.X - this.location.X);
            if(distanceFromPlayer > Chunk.tileDrawWidth * Chunk.tilesPerChunk * world.tileGenRadious)
            {
                world.killEntity(this);

            } else if(distanceFromPlayer < 50 * world.player.detectionRadiousModifier)
            {
                spook();
            }

            if(rand.NextDouble() < .01f)
            {
                spook();
            }
            

        }

        public void spook()
        {
            jump(1);
            walk(rand.Next(3) - 1);
        }

        public bool isAggrod()
        {
            return false;
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
