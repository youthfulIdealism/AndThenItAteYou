using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Survive.WorldManagement.Entities.Particles;
using Survive.WorldManagement.Inventory;
using Survive.WorldManagement.Inventory.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities.Projectiles
{
    public class EntityHarpoon : Entity, Weapon
    {
        Texture2D texture;
        Entity parent;
        Entity hooked;
        float hookDistance;
        Vector2 rotationVector;

        public EntityHarpoon(Vector2 location, WorldBase world, Entity parent) : base(location, world)
        {
            this.parent = parent;

            if(rand == null)
            {
                rand = new Random();
            }

            width = 20;
            height = 20;
            texture = Game1.texture_entity_harpoon;
            rotationVector = new Vector2();
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);

            bool droppingLoot = false;

            if(hooked == null)
            {
                rotationVector = velocity;

                foreach (Entity entity in world.entities)
                {
                    if (!entity.Equals(this) && !entity.Equals(parent) && entity.getCollisionBox().Intersects(this.getCollisionBox()))
                    {
                        entity.playHitSound();
                        entity.damage(getDamage(), this, Vector2.Normalize(velocity) * 2);
                        if (entity.blocksWeaponsOnHit)
                        {
                            hooked = entity;
                            hookDistance = Vector2.Distance(parent.location, entity.location);
                            break;
                        }
                    }
                }

                if (collideBottom || collideLeft || collideRight || collideTop)
                {
                    droppingLoot = true;
                }

            }
            else
            {
                location = hooked.location;
                Vector2 pullVector = Vector2.Normalize(parent.location - hooked.location) * 2;
                rotationVector = -pullVector;
                if (Vector2.Distance(parent.location, hooked.location) > hookDistance)
                {
                    hooked.impulse += pullVector;
                    
                }

                if(hooked.health <= 0 || !world.entities.Contains(hooked))
                {
                    droppingLoot = true;
                }
            }
            

            

            if(droppingLoot)
            {
                dropLootAndKill();
            }
        }

        public void dropLootAndKill()
        {
            world.killEntity(this);
            world.addEntity(new ItemDropEntity(location, world, new Item_Harpoon(1)));
        }


        public float getDamage()
        {
            return 20 + rand.Next(20);
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
            batch.Draw(texture, new Rectangle(defaultRect.X + offset.X, defaultRect.Y + offset.Y, (int)texture.Width, (int)texture.Height), null, groundColor, (float)Math.Atan(rotationVector.Y / rotationVector.X), Vector2.Zero, effect, 0);

            if(hooked != null)
            {
                Game1.DrawLine(batch, parent.location + offset.ToVector2(), location + offset.ToVector2(), 2, groundColor);
            }
            
        }
    }
}
