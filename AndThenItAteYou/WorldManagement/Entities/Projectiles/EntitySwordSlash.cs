using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    public class EntitySwordSlash : Entity, Weapon
    {
        const float MaxTicksExisted = 17;
        Entity parent;
        HashSet<Entity> collidedEntitites;
        public bool canUnlockWarrior = false;
        static int wheeliesKilled;

        public EntitySwordSlash(Vector2 location, WorldBase world, Entity parent) : base(location, world)
        {
            this.parent = parent;

            if(rand == null)
            {
                rand = new Random();
            }

            width = 60;
            height = 40;
            this.gravityMultiplier = 0;
            collidedEntitites = new HashSet<Entity>();
            windMultiplier = 0;
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);

            foreach(Entity entity in world.entities)
            {
                if(!(entity is EntitySwordSlash) && !entity.Equals(parent) && entity.blocksWeaponsOnHit && !collidedEntitites.Contains(entity) && entity.getCollisionBox().Intersects(this.getCollisionBox()))
                {
                    entity.playHitSound();
                    entity.damage(getDamage(), this, Vector2.Normalize(velocity) * 12);
                    collidedEntitites.Add(entity);

                    if(entity is EntityWheelie && canUnlockWarrior && entity.health <= 0)
                    {
                        wheeliesKilled++;
                        if (wheeliesKilled == 2)
                        {
                            if (MetaData.unlockCharacter(4))
                            {
                                MetaData.playUnlockCharacterAlert(4, world, world.player.location);
                            }
                        }
                    }
                }
            }

            if(ticksExisted > MaxTicksExisted)
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
            return 60;
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
            Rectangle defaultRect = getCollisionBox().ToRect();
            batch.Draw(Game1.block, new Rectangle(defaultRect.X + offset.X, defaultRect.Y + offset.Y, (int)width, (int)height - 30), null, Color.White * .2f * (1 - (float)ticksExisted / MaxTicksExisted), 0/*(float)Math.Atan(velocity.Y / velocity.X)*/, Vector2.Zero, SpriteEffects.None, 0);
        }
    }
}
