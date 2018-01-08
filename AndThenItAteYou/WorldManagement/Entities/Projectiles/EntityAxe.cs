using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
    public class EntityAxe : Entity, Weapon
    {
        Texture2D texture;
        Entity parent;
        SoundEffectInstance sound;
        float lastSoundVolume = 1;

        float currentRotation;
        float deltaRotation;

        public EntityAxe(Vector2 location, WorldBase world, Entity parent) : base(location, world)
        {
            this.parent = parent;

            if(rand == null)
            {
                rand = new Random();
            }

            width = 10;
            height = 10;
            texture = Game1.texture_item_axe;
            currentRotation = (float)rand.NextDouble() * (float)Math.PI * 2;
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);
            if(velocity.X > 0)
            {
                deltaRotation = .3f;
            }
            else if(velocity.X < 0)
            {
                deltaRotation = -.3f;
            }
            currentRotation += deltaRotation;

            if (sound == null || sound.IsDisposed)
            {
                sound = SoundManager.getSound("spear-throw").playWithVariance(0, lastSoundVolume, 0, SoundType.MONSTER);
                lastSoundVolume *= .5f;
            }


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
            if (rand.NextDouble() < .3)
            {
                world.addEntity(new ItemDropEntity(location, world, new Item_Axe(1)));
            }
        }


        public float getDamage()
        {
            return 150;
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
            batch.Draw(texture, new Rectangle(defaultRect.X + offset.X, defaultRect.Y + offset.Y, Game1.texture_item_stone.Width, Game1.texture_item_stone.Height), null, groundColor, currentRotation, Vector2.Zero, effect, 0);
        }
    }
}
