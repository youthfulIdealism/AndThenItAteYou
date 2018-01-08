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
    public class EntityFrogSpit : Entity, Weapon
    {
        Texture2D texture;
        Entity parent;

        public EntityFrogSpit(Vector2 location, WorldBase world, Entity parent) : base(location, world)
        {
            this.parent = parent;

            if(rand == null)
            {
                rand = new Random();
            }

            width = 20;
            height = 20;
            texture = Game1.texture_entity_frog_spit;
            this.gravityMultiplier = .05f;
            this.frictionMultiplier *= .2f;
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);

            foreach(Entity entity in world.entities)
            {
                if(!entity.Equals(this) && !entity.Equals(parent) && entity.getCollisionBox().Intersects(this.getCollisionBox()))
                {
                    entity.playHitSound();
                    entity.damage(getDamage(), this, new Vector2() + Vector2.Normalize(velocity) * 20);
                    if (entity.blocksWeaponsOnHit)
                    {
                        dropLootAndKill();
                    }
                }
            }


            for(int i = 0; i < 3; i++)
            {
                world.addEntity(new ParticleSpit(location, world, new Vector2(), 20));
            }

            if(collideBottom || collideLeft || collideRight || collideTop)
            {
                dropLootAndKill();
            }
        }

        public void dropLootAndKill()
        {
            world.killEntity(this);
            for (int i = 0; i < 20; i++)
            {
                world.addEntity(new ParticleSpit(location, world, new Vector2((float)rand.NextDouble() * 5 - 2.5f, (float)rand.NextDouble() * 5), 50));
            }
        }


        public float getDamage()
        {
            return 20 + rand.Next(10);
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
