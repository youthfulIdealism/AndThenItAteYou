using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Survive.Sound;
using Survive.WorldManagement.Entities.Particles;
using Survive.WorldManagement.Tile;
using Survive.WorldManagement.Tile.Tags;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities
{
    public abstract class Entity
    {
        public WorldBase world;
        public static Random rand;
        public bool blocksWeaponsOnHit = true;

        public float walkSpeed { get; set; }
        public float jumpForce { get; set; }
        public float width { get; set; }
        public float height { get; set; }
        public Vector2 location { get; set; }
        public Vector2 velocity { get; set; }
        public Vector2 impulse { get; set; }
        public bool collideTop;
        public bool collideBottom;
        public bool collideLeft;
        public bool collideRight;
        public int framesCollidingBottom { get; private set; } = 0;
        public long ticksExisted { get; protected set; }
        public float health { get; set; }
        public const float maxDamageImmunityTime = 30;
        public float remainingDamageImmunityTime { get; set; }
        public float frictionMultiplier { get; set; }
        public float gravityMultiplier { get; set; }
        public float windMultiplier { get; set; } = 1;
        public Dictionary<StatusEffect.status, SortedList<float, StatusEffect>> statusEffects;
        public TileType tileIn;

        public Entity(Vector2 location, WorldBase world)
        {
            if(rand == null)
            {
                rand = new Random();
            }
            this.location = location;
            this.world = world;
            impulse = new Vector2();
            velocity = new Vector2();
            width = 20;
            height = 46;
            health = 100;
            gravityMultiplier = 1;
            frictionMultiplier = 1;
            walkSpeed = 1;
            jumpForce = 16;
            statusEffects = new Dictionary<StatusEffect.status, SortedList<float, StatusEffect>>();
            blocksWeaponsOnHit = true;
        }

        public AABB getCollisionBox()
        {
            return new AABB((location.X - width / 2), (location.Y - height / 2), (width), (height));
        }

        public virtual void update(GameTime time)
        {
            foreach (SortedList<float, StatusEffect> statusList in statusEffects.Values)
            {
                List<float> removeKeys = new List<float>();
                foreach (StatusEffect effect in statusList.Values)
                {
                    effect.update(this);
                    if (effect.timeRemaining <= 0)
                    {
                        removeKeys.Add(effect.potency);
                    }
                }
                foreach (float fl in removeKeys)
                {
                    statusList.Remove(fl);
                }
            }

            StatusEffect healthRegen = getEffect(StatusEffect.status.HEALTHREGEN);
            float healthRegenAmt = 0;
            if (healthRegen != null)
            {
                healthRegenAmt = healthRegen.potency;
                if (rand.NextDouble() < .1f)
                {
                    ParticleHealth particle = new ParticleHealth(location, world, new Vector2(0, 0), 30);
                    world.addEntity(particle);
                }
                for (int i = 0; i < rand.Next((int)(healthRegenAmt) / 3); i++)
                {
                    ParticleHealth particle = new ParticleHealth(location, world, new Vector2(0, 0), 30);
                    world.addEntity(particle);
                }
            }

            StatusEffect poison = getEffect(StatusEffect.status.POISON);
            float poisonamt = 0;
            if (poison != null) { poisonamt = poison.potency; }
            spawnPoisonParticles(poisonamt);

            ticksExisted++;

            if (collideBottom && velocity.Y > 0) { velocity = new Vector2(velocity.X, 0); }
            if (collideLeft && velocity.X < 0) { velocity = new Vector2(0, velocity.Y); }
            if (collideRight && velocity.X > 0) { velocity = new Vector2(0, velocity.Y); }
            if (collideTop && velocity.Y < 0) { velocity = new Vector2(velocity.X, 0); }

            if(collideBottom)
            {
                framesCollidingBottom++;
            }else
            {
                framesCollidingBottom = 0;
            }

            prePhysicsUpdate(time);
            performPhysics(time);


            if (health <= 0)
            {
                world.killEntity(this);
            }

            remainingDamageImmunityTime--;
        }

        public virtual void performPhysics(GameTime time)
        {
            tileIn = world.getBlock(location);
            if(tileIn != null && !tileIn.tags.Contains(TagReferencer.WATER))
            {
                impulse += new Vector2(0, .5f * gravityMultiplier);
            }else
            {
                impulse += new Vector2(0, .05f * gravityMultiplier);
            }

            velocity += impulse;
            

            TileType tileOccupied = world.getBlock(location);
            TileType tileBelow = world.getBlock(location + new Vector2(0, Chunk.tileDrawWidth));
            if (tileOccupied != null)
            {
                velocity -= velocity * (1 - tileOccupied.getFrictionMultiplier()) * frictionMultiplier;

            }

            if (collideBottom && tileBelow != null)
            {
                velocity -= velocity * (1 - tileBelow.getFrictionMultiplier()) * frictionMultiplier;
            }

            Vector2 potentialLocation = location + velocity;

            collideBottom = false;
            collideLeft = false;
            collideRight = false;
            collideTop = false;
            location = world.tryMove(this, new AABB((potentialLocation.X - width / 2), (potentialLocation.Y - height / 2), (width), (height))).Center;
            impulse = new Vector2();

            if (tileIn != null && tileIn.tags.Contains(TagReferencer.WATER))
            {
                for (int i = 0; i < Math.Floor(velocity.Length() / 3); i++)
                {
                    world.addEntity(new ParticleBubble(new Vector2(location.X, location.Y + height / 2), world, new Vector2(), 75));
                }

                if(rand.NextDouble() <= .02f)
                {
                    world.addEntity(new ParticleBubble(location, world, new Vector2(), 75));
                }
            }
        }

        public virtual void prePhysicsUpdate(GameTime time)
        {



        }

        public virtual void fastForward(float time)
        {

        }

        public StatusEffect getEffect(StatusEffect.status type)
        {
            SortedList<float, StatusEffect> statusList;
            statusEffects.TryGetValue(type, out statusList);
            if(statusList == null || statusList.Count <= 0)
            {
                return null;
            }else
            {
                return statusList.ElementAt(statusList.Count - 1).Value;
            }
        }

        public virtual void walk(float directionAndVelocityAsPercentOfSpeed)
        {
            StatusEffect speed = getEffect(StatusEffect.status.SPEED);
            float bonusSpeed = 0;
            if(speed != null) { bonusSpeed = speed.potency; }

            StatusEffect slow = getEffect(StatusEffect.status.SLOW);
            float bonusSlow = 1;
            if (slow != null) { bonusSlow = slow.potency; }

            if (tileIn != null && !tileIn.tags.Contains(TagReferencer.WATER))
            {
                if (collideBottom)
                {
                    impulse += new Vector2(directionAndVelocityAsPercentOfSpeed * walkSpeed * (1 + bonusSpeed) * bonusSlow, 0);
                    spawnSpeedBoostParticles(bonusSpeed, directionAndVelocityAsPercentOfSpeed);

                }
                else
                {
                    impulse += new Vector2(.1f * (directionAndVelocityAsPercentOfSpeed * walkSpeed * (1 + bonusSpeed) * bonusSlow), 0);
                }
            }
            else
            {
                impulse += new Vector2(.15f * directionAndVelocityAsPercentOfSpeed * walkSpeed * (1 + bonusSpeed) * bonusSlow, 0);
                spawnSpeedBoostParticles(bonusSpeed, directionAndVelocityAsPercentOfSpeed);
            }


           
        }

        private void spawnSpeedBoostParticles(float bonusSpeed, float directionAndVelocityAsPercentOfSpeed)
        {
            if (bonusSpeed > 0)
            {
                //ensure speed boost particle baseline
                if (rand.NextDouble() < .1f)
                {
                    ParticleSpeedBoost particle = new ParticleSpeedBoost(location + new Vector2(0, this.height), world, new Vector2(directionAndVelocityAsPercentOfSpeed * -5, 0), 100);
                    world.addEntity(particle);
                }
                for (int i = 0; i < rand.Next((int)(bonusSpeed * 7)); i++)
                {
                    ParticleSpeedBoost particle = new ParticleSpeedBoost(location + new Vector2(0, this.height), world, new Vector2(directionAndVelocityAsPercentOfSpeed * -5, 0), 100);
                    world.addEntity(particle);
                }
            }
        }

        private void spawnPoisonParticles(float poisonAmt)
        {
            if (poisonAmt > 0)
            {
                //ensure speed boost particle baseline
                if (rand.NextDouble() < .1f)
                {
                    ParticleArbitrary particle = new ParticleArbitrary(location + new Vector2(rand.Next(20) - 10, rand.Next(30) - 10), world, new Vector2(), 100, Game1.texture_particle_bubble);
                    particle.startColor = world.decorator.colorManager.groundColor;
                    particle.startColor = Color.Green;
                    particle.width = 10;
                    particle.height = 10;
                    world.addEntity(particle);
                }
                for (int i = 0; i < rand.Next((int)poisonAmt); i++)
                {
                    ParticleArbitrary particle = new ParticleArbitrary(location + new Vector2(rand.Next(20) - 10, rand.Next(30) - 10), world, new Vector2(), 100, Game1.texture_particle_bubble);
                    particle.startColor = world.decorator.colorManager.groundColor;
                    particle.startColor = Color.Green;
                    particle.width = 10;
                    particle.height = 10;
                    world.addEntity(particle);
                }
            }
        }

        public virtual void jump(float directionAndVelocityAsPercentOfSpeed)
        {
            float bonusJump = 0;
            StatusEffect jump = getEffect(StatusEffect.status.JUMP);
            if (jump != null) { bonusJump = jump.potency; }

            StatusEffect slow = getEffect(StatusEffect.status.SLOW);
            if (slow != null) { bonusJump *= slow.potency; }

            if (collideBottom && framesCollidingBottom > 0)
            {
                impulse += new Vector2(0, -directionAndVelocityAsPercentOfSpeed * jumpForce * (1 + bonusJump));
                for(int i = 0; i < bonusJump * 20; i++)
                {
                    ParticleJumpBoost particle = new ParticleJumpBoost(location, world, new Vector2(), 100);
                    world.addEntity(particle);
                }
            }
            else
            {
                TileType tileBelow = world.getBlock(location + new Vector2(0, Chunk.tileDrawWidth));

                if (tileIn != null && !tileIn.tags.Contains(TagReferencer.WATER))
                {
                    impulse += new Vector2(0, -.05f);
                }
                else
                {
                    impulse += new Vector2(0, -.2f);
                    if((collideLeft || collideRight) && velocity.Y > -4.3f)
                    {
                        impulse += new Vector2(0, -1.5f);
                    }
                }
                
            }
        }

        public virtual void addStatusEffect(StatusEffect effect)
        {
            if (!statusEffects.ContainsKey(effect.effect))
            {
                statusEffects.Add(effect.effect, new SortedList<float, StatusEffect>());
            }

            if (statusEffects[effect.effect].ContainsKey(effect.potency))
            {
                statusEffects[effect.effect][effect.potency].timeRemaining += effect.timeRemaining;
            }
            else
            {
                statusEffects[effect.effect].Add(effect.potency, effect);
            }

        }

        public virtual void removeStatusEffect(StatusEffect effect)
        {
            if (statusEffects.ContainsKey(effect.effect))
            {
                statusEffects[effect.effect].Remove(effect.potency);
            }
        }

        public virtual Color getDrawColor(Color groundColor, GameTime time)
        {
            Color drawColor = groundColor;
            if (remainingDamageImmunityTime > 0)
            {
                drawColor = Color.Lerp(groundColor, Color.White, (float)(Math.Sin(time.TotalGameTime.Milliseconds * .02f)) / 2);
            }
            return drawColor;
        }

        public virtual void damage(float amt, Entity source, Vector2 force)
        {
            if(remainingDamageImmunityTime <= 0)
            {
                float damageResistance = 1;
                StatusEffect resist = getEffect(StatusEffect.status.DAMAGERESISTANCE);
                if (resist != null) { damageResistance = 1 / resist.potency; }

                health -= amt * damageResistance;
                remainingDamageImmunityTime = maxDamageImmunityTime;
                impulse += force * damageResistance;
                ParticleBlood.createBloodSpray(location, world, -Vector2.Normalize(force), rand, 50, 7, 7);
            }
        }

        public virtual void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            Rectangle defaultRect = getCollisionBox().ToRect();
            batch.Draw(Game1.block, new Rectangle(defaultRect.X + offset.X, defaultRect.Y + offset.Y, defaultRect.Width, defaultRect.Height), groundColor);
        }

        public float getStatusEffectLevel(StatusEffect.status effect, float defaultValue)
        {
            SortedList<float, StatusEffect> statusList;
            statusEffects.TryGetValue(effect, out statusList);
            float returnValue = defaultValue;
            if (statusList != null && statusList.Count > 0) { returnValue = statusList.ElementAt(statusList.Count - 1).Value.potency; }
            return returnValue;
        }

        public virtual void playHitSound()
        {
            if(blocksWeaponsOnHit)
            {
                SoundManager.getSound("projectile-impact-flesh").playWithVariance(0, 1f / Vector2.Distance(location, world.player.location) * 10, (location - world.player.location).X, SoundType.MONSTER);
            }
        }

    }
}

