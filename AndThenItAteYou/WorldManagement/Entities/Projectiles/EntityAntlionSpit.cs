using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Survive.WorldManagement.Entities.Particles;
using Survive.WorldManagement.Inventory;
using Survive.WorldManagement.Inventory.Items;
using Survive.WorldManagement.Tile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities.Projectiles
{
    public class EntityAntlionSpit : Entity, Weapon
    {
        Texture2D texture;
        Entity parent;

        const int maxTimeAlive = 200;

        public EntityAntlionSpit(Vector2 location, WorldBase world, Entity parent) : base(location, world)
        {
            this.parent = parent;

            if(rand == null)
            {
                rand = new Random();
            }

            width = 20;
            height = 20;
            texture = Game1.texture_entity_frog_spit;
            //this.gravityMultiplier = .05f;
            this.frictionMultiplier = 0;
        }

        public override void performPhysics(GameTime time)
        {
            impulse += new Vector2(0, .5f * gravityMultiplier);



            velocity += impulse;

            TileType tileOccupied = world.getBlock(location);
            if (tileOccupied != null)
            {
                velocity -= velocity * (1 - tileOccupied.getFrictionMultiplier()) * frictionMultiplier;

            }

            Vector2 potentialLocation = location + velocity;

            collideBottom = false;
            collideLeft = false;
            collideRight = false;
            collideTop = false;
            location = potentialLocation;
            impulse = new Vector2();
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);

            foreach(Entity entity in world.entities)
            {
                if(!entity.Equals(this) && !entity.Equals(parent) && entity.getCollisionBox().Intersects(this.getCollisionBox()))
                {
                    entity.playHitSound();
                    entity.damage(getDamage(), this,  (Vector2.Normalize(new Vector2(parent.location.X - entity.location.X, -1))) * 5);
                    if(!entity.blocksWeaponsOnHit)
                    {
                        dropLootAndKill();
                    }
                    
                }
            }

            if(ticksExisted >= maxTimeAlive)
            {
                dropLootAndKill();
            }
        }

        public void dropLootAndKill()
        {
            world.killEntity(this);
        }


        public float getDamage()
        {
            return 0;
        }

        public Entity getParent()
        {
            return parent;
        }

        public override void damage(float amt, Entity source, Vector2 force)
        {
            impulse += force;
        }

        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            SpriteEffects effect = SpriteEffects.None;
            if (velocity.X > 0)
            {
                effect = SpriteEffects.FlipHorizontally;
            }

            Rectangle defaultRect = getCollisionBox().ToRect();
            batch.Draw(texture, new Rectangle(defaultRect.X + offset.X, defaultRect.Y + offset.Y, (int)width, (int)height), null, groundColor, (float)Math.Atan(velocity.Y / velocity.X), Vector2.Zero, effect, 0);
        }
    }
}
