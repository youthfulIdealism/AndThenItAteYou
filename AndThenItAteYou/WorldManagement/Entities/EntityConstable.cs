using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Survive.Sound;
using Survive.WorldManagement.Entities.Projectiles;
using Survive.WorldManagement.Inventory;
using Survive.WorldManagement.Inventory.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities
{
    public class EntityConstable : Entity, AggroAble
    {
        const int texSwapPoint = 15;
        private static Vector2 bulletOffsetLoc = new Vector2(-25, -110);
        Vector2 lastPlayerLoc;
        Random random;
        Texture2D towerTex;
        Texture2D eyeballTex;
        Point drawOffset = new Point(-25, 0);
        bool aggro;
        int maxTimeAiming = 55;
        int timeSpentAiming = 0;
        Vector2 aimLoc;
        SoundEffectInstance currentSound;

        public EntityConstable(Vector2 location, WorldBase world) : base(location, world)
        {
            aggro = false;
            this.width = 50;
            this.height = 300;
            random = new Random();
            towerTex = Game1.texture_entity_constable_tower;
            eyeballTex = Game1.texture_entity_constable_eye;
            health = 150;
            this.walkSpeed = .2f;
            this.jumpForce = 11;
            this.location += new Vector2(0, -100);
            this.blocksWeaponsOnHit = false;
            windMultiplier = 0;
        }

        public override void damage(float amt, Entity source, Vector2 force)
        {
            //base.damage(amt, source, force);
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);

            foreach (Entity entity in world.entities)
            {
                if (entity is Weapon)
                {
                    if (Vector2.Distance(entity.location, this.location) <= 100)
                    {
                        spook();
                    }
                }
            }

            float distanceFromPlayer = Vector2.Distance(world.player.location, this.location);
            if (distanceFromPlayer <= 700 * world.player.detectionRadiousModifier)
            {

                if (Vector2.Distance(world.player.location, lastPlayerLoc) > 4 && random.NextDouble() < .05f * world.player.detectionLevel)
                {
                    spook();
                }

                lastPlayerLoc = world.player.location;
            }

            if (aggro)
            {

                /*if (currentSound == null)
                {
                    if (rand.NextDouble() < .05f) { currentSound = SoundManager.getSound("constable-talk").playWithVariance(0, 1f / distanceFromPlayer * 50, (location - world.player.location).X); }
                }
                else*/ if (currentSound != null && currentSound.IsDisposed)
                {
                    currentSound = null;
                }

                if (distanceFromPlayer < 600 || timeSpentAiming > 0)
                {
                    if(timeSpentAiming == 0)
                    {
                        aimLoc = world.player.location;
                        currentSound = SoundManager.getSound("constable-talk").playWithVariance(0, 1f / distanceFromPlayer * 60, (location - world.player.location).X, SoundType.MONSTER);
                    }
                    else if(timeSpentAiming > maxTimeAiming)
                    {
                        timeSpentAiming = -1;

                        EntityLaserBolt laserBolt = new EntityLaserBolt(location + bulletOffsetLoc, world, this);
                        laserBolt.velocity += Vector2.Normalize(aimLoc - (location + bulletOffsetLoc) ) * 25;
                        laserBolt.baseDamage = 15;
                        laserBolt.possibleAdditionalDamage = 10;
                        world.addEntity(laserBolt);

                        if(currentSound != null)
                        {
                            currentSound.Stop();
                            currentSound.Dispose();
                        }
                        
                        SoundManager.getSound("gun-fire").playWithVariance(0, 1, (location - world.player.location).X, SoundType.MONSTER);
                    }



                    timeSpentAiming++;
                    
                }else
                {
                    /*float direction = 0;
                    if (world.player.location.X < this.location.X)
                    {
                        direction = -1;
                    }
                    else if (world.player.location.X > this.location.X)
                    {
                        direction = 1;
                    }
                    walk(direction);

                    if (collideLeft || collideRight)
                    {
                        if (collideBottom)
                        {
                            jumps--;
                            jump(1);
                        }
                        if (jumps <= 0)
                        {
                            aggro = false;
                            jumps = 3;
                        }
                    }
                    else
                    {
                        jumps = 3;
                    }*/
                }
                

                
            }

            if (health <= 0)
            {
                world.addEntity(new ItemDropEntity(location, world, new Item_Bullet(1)));
                SoundManager.getSound("constable-die").playWithVariance(0, 1f / distanceFromPlayer * 75, (location - world.player.location).X, SoundType.MONSTER);
            }
        }

        public void spook()
        {
            if (!aggro)
            {
                aggro = true;
            }
        }

        public bool isAggrod()
        {
            return aggro;
        }

        public override void playHitSound()
        {
            SoundManager.getSound("projectile-impact-metal").playWithVariance(0, 1f / Vector2.Distance(location, world.player.location) * 75, (location - world.player.location).X, SoundType.MONSTER);
        }

        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            //base.draw(batch, time, offset);

            /*Texture2D currentTex = null;
            if (aggro)
            {
                if(timeSpentAiming > 0)
                {
                    currentTex = aimTex;
                }
                else
                {
                    currentTex = runTex;
                }
                
            }
            else
            {
                currentTex = standTex;
            }*/
            SpriteEffects effect = SpriteEffects.None;
            if (velocity.X < 0)
            {
                effect = SpriteEffects.FlipHorizontally;
            }

            if(timeSpentAiming > 0)
            {
                Game1.DrawLine(batch, location + bulletOffsetLoc + offset.ToVector2(), aimLoc + offset.ToVector2(), 10, Color.Green * ((float) timeSpentAiming / maxTimeAiming));
            }


            Rectangle defaultRect = getCollisionBox().ToRect();
            batch.Draw(towerTex, new Rectangle(defaultRect.X + offset.X + drawOffset.X, defaultRect.Y + offset.Y + drawOffset.Y, towerTex.Width, towerTex.Height), null, getDrawColor(groundColor, time), 0, Vector2.Zero, effect, 0);
        }
    }
}
