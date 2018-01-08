using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Survive.Sound;
using Survive.WorldManagement.Entities.Particles;
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
    public class EntityWheelie : Entity, AggroAble
    {

        const int texSwapPoint = 15;
        private int currentTexSwap = texSwapPoint;
        private int currentTexIndex;
        Vector2 lastPlayerLoc;
        Random random;
        Texture2D currentTex;
        //Texture2D jumpTex;
        //Texture2D attackTex;
        Point drawOffset = new Point(-25, -4);
        bool aggro;
        bool attacking = false;
        const int aimTime = 60;
        int currentAimTime = aimTime;

        int maxJumpChargeTime = 17;
        int currentJumpChargeTime = 0;
        bool chargingJump = false;

        const int maxTimeUntilJumpOrAttack = 100;
        int timeUntilJumpOrAttack = maxTimeUntilJumpOrAttack;
        SoundEffectInstance runSound;


        public EntityWheelie(Vector2 location, WorldBase world) : base(location, world)
        {
            aggro = false;
            this.width = 30;
            this.height = 70;
            random = new Random();
            currentTex = Game1.texture_entity_wheelie_stand[0];
            currentTexIndex = 0;
            //jumpTex = Game1.texture_entity_frog_jump;
            //attackTex = Game1.texture_entity_frog_attack;
            health = 75;
            walkSpeed = .65f;
            jumpForce = jumpForce * 1.3f;
            this.frictionMultiplier = .6f;
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);



            currentTexSwap--;
            if (currentTexSwap <= 0)
            {
                if(!aggro)
                {
                    currentTexIndex = (currentTexIndex + 1) % Game1.texture_entity_wheelie_stand.Length;
                    currentTex = Game1.texture_entity_wheelie_stand[currentTexIndex];
                    currentTexSwap = texSwapPoint;
                }else
                {
                    if(chargingJump)
                    {
                        currentTexIndex++;
                        currentTex = Game1.texture_entity_wheelie_charge_jump[(int)Math.Floor(((float)currentJumpChargeTime / maxJumpChargeTime) * Game1.texture_entity_wheelie_charge_jump.Count())];
                    }
                    else if(attacking)
                    {
                        currentTexIndex = (currentTexIndex + 1) % Game1.texture_entity_wheelie_stand.Length;
                        currentTex = Game1.texture_entity_wheelie_stand[currentTexIndex];
                        currentTexSwap = texSwapPoint;
                    }
                    else
                    {
                        currentTexIndex = (currentTexIndex + 1) % Game1.texture_entity_wheelie_run.Length;
                        currentTex = Game1.texture_entity_wheelie_run[currentTexIndex];
                        currentTexSwap = texSwapPoint;
                    }
                }
                
            }

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

            if(distanceFromPlayer >= 1000 && aggro)
            {
                aggro = false;
                world.decorator.ambientSoundManager.unStartle();

            }

            if (aggro)
            {
                //Console.WriteLine(velocity.Length());
                if (runSound != null && runSound.IsDisposed)
                {
                    runSound = null;
                }

                if(runSound == null)
                {
                    runSound = SoundManager.getSound("wheelie_walk").playWithVariance(0, 1f / distanceFromPlayer * 80, (location - world.player.location).X, SoundType.MONSTER);
                }

                if(collideBottom)
                {
                    if(runSound.State == SoundState.Paused)
                    {
                        runSound.Resume();
                    }
                }else if(runSound.State == SoundState.Playing)
                {
                    runSound.Pause();
                }
                runSound.Volume = (float)Math.Max(Math.Min( (1f / distanceFromPlayer * 10) * (velocity.Length() / 10), 1), 0);

                if (!chargingJump && !attacking)
                {
                    timeUntilJumpOrAttack--;
                    if(timeUntilJumpOrAttack < 0)
                    {
                        if(rand.NextDouble() < .7)
                        {
                            attacking = true;
                            SoundManager.getSound("wheelie_prepare_shot").playWithVariance(0, 1f / distanceFromPlayer * 200, 0, SoundType.MONSTER);
                        }
                        else
                        {
                            chargingJump = true;
                        }
                        timeUntilJumpOrAttack = maxTimeUntilJumpOrAttack;
                    }else if(collideLeft || collideRight)
                    {
                        chargingJump = true;
                    }


                    if (distanceFromPlayer >= 500)
                    {
                        float direction = 0;
                        if (world.player.location.X < this.location.X)
                        {
                            direction = -1;
                        }
                        else if (world.player.location.X > this.location.X)
                        {
                            direction = 1;
                        }
                        walk(direction);
                    }
                    else if (distanceFromPlayer <= 350)
                    {
                        float direction = 0;
                        if (world.player.location.X < this.location.X)
                        {
                            direction = 1;
                        }
                        else if (world.player.location.X > this.location.X)
                        {
                            direction = -1;
                        }
                        walk(direction);
                    }
                }
                else if(chargingJump)
                {
                    currentJumpChargeTime++;
                    if(currentJumpChargeTime >= maxJumpChargeTime)
                    {
                        currentJumpChargeTime = 0;
                        chargingJump = false;
                        jump(1);
                    }
                }else if(attacking)
                {
                    currentAimTime--;
                    if(currentAimTime <= 0)
                    {
                        EntityLaserBolt laserBolt = new EntityLaserBolt(location + new Vector2(0, -15), world, this);
                        laserBolt.velocity += Vector2.Normalize(world.player.location - (location + new Vector2(0, -15))) * 10;
                        laserBolt.baseDamage = 20;
                        laserBolt.possibleAdditionalDamage = 10;
                        world.addEntity(laserBolt);

                        attacking = false;
                        currentAimTime = aimTime;

                        SoundManager.getSound("gun-fire").playWithVariance(0, 1, location.X - world.player.location.X, SoundType.MONSTER);
                        currentAimTime = aimTime;
                        attacking = false;
                    }
                    
                }
                
            }

            if (health <= 0)
            {
                world.addEntity(new ItemDropEntity(location, world, new Item_Seed(1)));
                /*if(!hasDroppedCraftingRecepie)
                {
                    Game1.instance.crafting.registerRecepie(new CraftingRecepie(new Item_Spear_Fanged(1), .4f, new Item[] { new Item_Spear(1), new Item_Guardian_Fang(1) }, new int[] { 1, 1 }));
                    hasDroppedCraftingRecepie = true;
                }*/
                if(runSound != null && !runSound.IsDisposed)
                {
                    runSound.Stop();
                    runSound.Dispose();
                }
                world.decorator.ambientSoundManager.unStartle();
            }

            
        }

        

        public void spook()
        {
            if (!aggro)
            {
                aggro = true;
                world.decorator.ambientSoundManager.startle();
            }
        }

        public bool isAggrod()
        {
            return aggro;
        }

        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            //base.draw(batch, time, offset);

            Texture2D currentTex = null;
            if (aggro)
            {
                currentTex = this.currentTex;
                /*if(attacking)
                {
                    currentTex = attackTex;
                }
                else if(collideBottom)
                {
                    currentTex = standTex;
                }
                else
                {
                    currentTex = jumpTex;
                }*/

            }
            else
            {
                currentTex = this.currentTex;
            }
            SpriteEffects effect = SpriteEffects.None;
            if (velocity.X < 0)
            {
                effect = SpriteEffects.FlipHorizontally;
            }

            Rectangle defaultRect = getCollisionBox().ToRect();
            batch.Draw(currentTex, new Rectangle(defaultRect.X + offset.X + drawOffset.X, defaultRect.Y + offset.Y + drawOffset.Y, currentTex.Width, currentTex.Height), null, getDrawColor(groundColor, time), 0, Vector2.Zero, effect, 0);
        }
    }
}
