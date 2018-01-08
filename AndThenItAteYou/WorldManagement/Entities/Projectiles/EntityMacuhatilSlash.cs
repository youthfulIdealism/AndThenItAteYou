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
    public class EntityMacuhatilSlash : Entity, Weapon
    {
        const float MaxTicksExisted = 17;
        Entity parent;
        HashSet<Entity> collidedEntitites;

        public EntityMacuhatilSlash(Vector2 location, WorldBase world, Entity parent) : base(location, world)
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
                if(!(entity is EntityMacuhatilSlash) && !entity.Equals(parent) && entity.blocksWeaponsOnHit && !collidedEntitites.Contains(entity) && entity.getCollisionBox().Intersects(this.getCollisionBox()))
                {
                    float entityHealthBefore = entity.health;
                    entity.playHitSound();
                    entity.damage(getDamage(), this, Vector2.Normalize(velocity) * 12);
                    collidedEntitites.Add(entity);

                    if(entityHealthBefore > 0 && entity.health <= 0 && parent is Player)
                    {
                        Player player = (Player)parent;
                        if (player.cards[0] != null)
                        {
                            if(player.cards[0].charges < player.cardCharges)
                            {
                                player.cards[0].charges++;
                                world.addEntity(new ParticleRecharge(location, world, new Vector2(0, -1), 75));
                            }
                        }
                        if (player.cards[1] != null)
                        {
                            if (player.cards[1].charges < player.cardCharges)
                            {
                                player.cards[1].charges++;
                                world.addEntity(new ParticleRecharge(location, world, new Vector2(0, -1), 75));
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
