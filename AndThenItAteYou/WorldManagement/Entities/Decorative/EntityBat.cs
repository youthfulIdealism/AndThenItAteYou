using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Survive.Sound;
using Survive.WorldManagement.Inventory;
using Survive.WorldManagement.Inventory.Items;
using Survive.WorldManagement.Tile;
using Survive.WorldManagement.Tile.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities
{
    public class EntityBat : Entity
    {
        
        Texture2D tex;
        int texCounter;
        Vector2 direction = new Vector2();
        int ticksGoingVerticalDirection;
        int verticalDirection;

        public EntityBat(Vector2 location, WorldBase world) : base(location, world)
        {
            this.width = 10;
            this.height = 10;
            tex = Game1.texture_entity_bat[0];
            this.health = 1;
            texCounter = 0;
            this.blocksWeaponsOnHit = false;
            this.gravityMultiplier = .05f;
            this.ticksGoingVerticalDirection = 0;
            verticalDirection = 0;
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);
            ticksGoingVerticalDirection--;
            TileType currentTile = world.getBlock(location);
            if (collideBottom || (currentTile != null && currentTile.tags.Contains(TagReferencer.WATER)))
            {
                ticksGoingVerticalDirection = 10;
                verticalDirection = -1;
            }else if(collideTop)
            {
                ticksGoingVerticalDirection = 10;
                verticalDirection = 1;
            }

            if(ticksGoingVerticalDirection > 0)
            {
                direction += new Vector2(0, verticalDirection);
            }

            texCounter++;
            tex = Game1.texture_entity_bat[(texCounter / 10) % Game1.texture_entity_bat.Length];
            direction += new Vector2((float)rand.NextDouble() * 2 - 1, (float)rand.NextDouble() * 2 - 1);

            float distanceFromPlayer = Math.Abs(world.player.location.X - this.location.X);
            if(distanceFromPlayer > Chunk.tileDrawWidth * Chunk.tilesPerChunk * world.tileGenRadious)
            {
                world.killEntity(this);

            }
            direction = Vector2.Normalize(direction) * .3f;
            impulse += direction;

        }

        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            SpriteEffects effect = SpriteEffects.None;
            if (velocity.X > 0)
            {
                effect = SpriteEffects.FlipHorizontally;
            }

            Rectangle defaultRect = getCollisionBox().ToRect();
            batch.Draw(tex, new Rectangle(defaultRect.X + offset.X, defaultRect.Y + offset.Y + 3, tex.Width, tex.Height), null, getDrawColor(groundColor, time), 0, Vector2.Zero, effect, 0);
        }
    }
}
