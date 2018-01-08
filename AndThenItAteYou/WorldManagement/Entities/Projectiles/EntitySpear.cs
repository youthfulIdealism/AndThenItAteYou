using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Survive.Sound;
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
    public class EntitySpear : Entity, Weapon
    {
        Texture2D texture;
        Entity parent;

        public EntitySpear(Vector2 location, WorldBase world, Entity parent) : base(location, world)
        {
            this.parent = parent;

            if(rand == null)
            {
                rand = new Random();
            }

            width = 20;
            height = 20;
            texture = Game1.texture_entity_spear;
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);

            bool droppingLoot = false;

            foreach(Entity entity in world.entities)
            {
                if(!entity.Equals(this) && !entity.Equals(parent) && entity.getCollisionBox().Intersects(this.getCollisionBox()))
                {
                    entity.playHitSound();
                    entity.damage(getDamage(), this, Vector2.Normalize(velocity) * 2);
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
            if (rand.NextDouble() < .9)
            {
                world.addEntity(new ItemDropEntity(location, world, new Item_Spear(1)));
            }else
            {
                SoundManager.getSound("spear-break").playWithVariance(0, 1f / Vector2.Distance(location, world.player.location) * 75, (location - world.player.location).X, SoundType.MONSTER);
            }
        }


        public float getDamage()
        {
            return 100;
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
            batch.Draw(texture, new Rectangle(defaultRect.X + offset.X, defaultRect.Y + offset.Y, (int)40, (int)40), null, groundColor, (float)Math.Atan(velocity.Y / velocity.X), Vector2.Zero, effect, 0);
        }
    }
}
