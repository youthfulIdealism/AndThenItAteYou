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
    public class EntityRock : Entity, Weapon
    {
        Texture2D texture;
        Entity parent;

        float currentRotation;
        float deltaRotation;

        public EntityRock(Vector2 location, WorldBase world, Entity parent) : base(location, world)
        {
            this.parent = parent;
            width = 10;
            height = 10;
            texture = Game1.texture_item_stone;
            currentRotation = (float)rand.NextDouble() * (float)Math.PI * 2;
            deltaRotation = (float)rand.NextDouble() * .4f - .2f;
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);

            currentRotation += deltaRotation;

            bool droppingLoot = false;

            foreach(Entity entity in world.entities)
            {
                if(!entity.Equals(this) && !entity.Equals(parent) && entity.getCollisionBox().Intersects(this.getCollisionBox()))
                {
                    entity.playHitSound();
                    entity.damage(getDamage(), this, Vector2.Normalize(velocity) * 2);
                    entity.addStatusEffect(new StatusEffect(StatusEffect.status.SLOW, .1f, 30, false));
                    if (entity.blocksWeaponsOnHit)
                    {
                        droppingLoot = true;
                    }
                }
            }

            if(collideBottom || collideLeft || collideRight || collideTop)
            {
                droppingLoot = true;
            }

            if(droppingLoot)
            {
                dropLootAndKill();
            }
        }

        public void dropLootAndKill()
        {
            world.killEntity(this);
            if (rand.NextDouble() < .4)
            {
                world.addEntity(new ItemDropEntity(location, world, new Item_Stone(1)));
            }
        }


        public float getDamage()
        {
            return 15;
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
            batch.Draw(texture, new Rectangle(defaultRect.X + offset.X, defaultRect.Y + offset.Y, (int)width, (int)height), null, groundColor, currentRotation, Vector2.Zero, effect, 0);
        }
    }
}
