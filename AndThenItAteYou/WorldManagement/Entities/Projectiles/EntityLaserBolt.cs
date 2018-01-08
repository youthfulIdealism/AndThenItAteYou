using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Survive.WorldManagement.Entities.Particles;
using Survive.WorldManagement.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities.Projectiles
{
    public class EntityLaserBolt : Entity, Weapon
    {
        Texture2D texture;
        Entity parent;
        int glowamt = 0;
        public int baseDamage { get; set; }
        public int possibleAdditionalDamage { get; set; }

        public EntityLaserBolt(Vector2 location, WorldBase world, Entity parent) : base(location, world)
        {
            this.parent = parent;

            if(rand == null)
            {
                rand = new Random();
            }

            width = 20;
            height = 20;
            texture = Game1.texture_entity_laser_bolt[0];
            gravityMultiplier = 0;
            frictionMultiplier = 0;
            baseDamage = 100;
            possibleAdditionalDamage = 75;
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);
            glowamt = rand.Next(70);

            foreach(Entity entity in world.entities)
            {
                if(!entity.Equals(this) && !entity.Equals(parent) && entity.getCollisionBox().Intersects(this.getCollisionBox()))
                {
                    entity.playHitSound();
                    entity.damage(getDamage(), this, Vector2.Normalize(velocity) * 2);
                    if (entity.blocksWeaponsOnHit)
                    {
                        dropLootAndKill();
                    }
                }
            }

            if(collideBottom || collideLeft || collideRight || collideTop)
            {
                dropLootAndKill();
            }

            if(Vector2.Distance(this.location, parent.location) >= 2000)
            {
                world.killEntity(this);
            }
        }

        public void dropLootAndKill()
        {
            world.killEntity(this);
            Vector2 sparkDirection = Vector2.Normalize(velocity) * -16;
            for(int i = 0; i < 10; i++)
            {
                world.addEntity(new ParticleGunSparks(location, world, sparkDirection, 50));
            }
            world.addEntity(new ParticleGunFlash(location, world, 3));
        }


        public float getDamage()
        {
            return baseDamage + rand.Next(possibleAdditionalDamage);
        }

        public Entity getParent()
        {
            return parent;
        }

        public override void damage(float amt, Entity source, Vector2 force)
        {
            
        }

        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            SpriteEffects effect = SpriteEffects.None;
            if (velocity.X < 0)
            {
                effect = SpriteEffects.FlipHorizontally;
            }
            
            Rectangle defaultRect = getCollisionBox().ToRect();
            batch.Draw(Game1.texture_particle_glow, new Rectangle(defaultRect.X + offset.X - glowamt / 2, defaultRect.Y + offset.Y - glowamt / 2, (int)width + glowamt, (int)height + glowamt), Color.Yellow);
            batch.Draw(Game1.texture_particle_glow, new Rectangle(defaultRect.X + offset.X - glowamt / 4, defaultRect.Y + offset.Y - glowamt / 4, (int)width + glowamt / 2, (int)height + glowamt / 2), Color.White);
            batch.Draw(texture, new Rectangle(defaultRect.X + offset.X - (int)width / 4, defaultRect.Y + offset.Y + (int)height / 4, (int)width / 2, (int)height / 2), null, groundColor, (float)Math.Atan(velocity.Y / velocity.X), Vector2.Zero, effect, 0);
            
        }
    }
}
