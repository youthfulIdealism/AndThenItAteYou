using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Survive.WorldManagement.Entities.Particles;
using Survive.WorldManagement.Inventory;
using Survive.WorldManagement.Inventory.Items;
using Survive.WorldManagement.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities.Projectiles
{
    public class EntityArrow : Entity, Weapon
    {
        static int guardiansKilled;
        Texture2D texture;
        Entity parent;

        public EntityArrow(Vector2 location, WorldBase world, Entity parent) : base(location, world)
        {
            this.parent = parent;

            if(rand == null)
            {
                rand = new Random();
            }

            width = 20;
            height = 20;
            texture = Game1.texture_entity_arrow;
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);

            foreach(Entity entity in world.entities)
            {
                if(!entity.Equals(this) && !entity.Equals(parent) && entity.getCollisionBox().Intersects(this.getCollisionBox()))
                {
                    entity.playHitSound();
                    entity.damage(getDamage(), this, new Vector2());
                    if (entity.blocksWeaponsOnHit)
                    {
                        dropLootAndKill();
                    }
                    if(entity is EntityGuardian && entity.health <= 0)
                    {
                        
                        guardiansKilled++;
                        if (guardiansKilled == 3)
                        {
                            if(MetaData.unlockCharacter(1))
                            {
                                MetaData.playUnlockCharacterAlert(1, world, world.player.location);
                            }
                        }
                    }
                }
            }

            if(collideBottom || collideLeft || collideRight || collideTop)
            {
                dropLootAndKill();
            }
        }

        public void dropLootAndKill()
        {
            world.killEntity(this);
            if (rand.NextDouble() < .5)
            {
                world.addEntity(new ItemDropEntity(location, world, new Item_Arrow(1)));
            }
        }


        public float getDamage()
        {
            return 75;
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
