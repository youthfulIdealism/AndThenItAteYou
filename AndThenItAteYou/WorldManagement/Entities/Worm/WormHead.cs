using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Survive.Sound;
using Survive.WorldManagement.Entities.Particles;
using Survive.WorldManagement.Tile;
using Survive.WorldManagement.Tile.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities.Worm
{
    public class WormHead : Entity, AggroAble
    {
        bool aggro = false;
        Vector2 lastPlayerLoc;

        Vector2 resting = new Vector2(float.MinValue, float.MinValue);
        Vector2 latchLoc;

        bool wasPlayerCollidingBottomLastRound;
        bool wasUnderground = true;

        const float frameSwitchPoint = 7;
        float frameSwitcher;
        int currentFrame;
        Texture2D currentTex;
        Point drawOffset = new Point(20, 20);

        public WormHead(Vector2 location, WorldBase world) : base(location, world)
        {
            location = location + new Vector2(0, 200);//The worm isn't interesting unless buried in the earth
            width = 40;
            height = 40;
            walkSpeed = .5f;
            wasPlayerCollidingBottomLastRound = false;
            latchLoc = resting;

            int bodySegmentSpacing = (int)(width * .9f);
            int numSegments = 8 + rand.Next(8);
            health += numSegments * 10;
            Entity last = this;
            for (int i = 0; i < numSegments; i++)
            {
                WormBodySegment bodySegment = new WormBodySegment(location + new Vector2(i * bodySegmentSpacing, 0), world, last);
                last = bodySegment;
                world.addEntity(bodySegment);
            }
            currentTex = Game1.texture_worm_head[0];
            windMultiplier = 0;
        }

        public bool isAggrod()
        {
            return aggro;
        }

        public override void performPhysics(GameTime time)
        {
            tileIn = world.getBlock(location);
            if (tileIn == null)
            {

            }else if(tileIn.tags.Contains(TagReferencer.SOLID))
            {
                
            }
            else
            {
                impulse += new Vector2(0, .3f * gravityMultiplier);
            }

            velocity += impulse;


            TileType tileBelow = world.getBlock(location + new Vector2(0, Chunk.tileDrawWidth));
            if (tileIn != null)
            {
                if(tileIn.tags.Contains(TagReferencer.SOLID))
                {
                    velocity -= velocity * (1 - tileIn.getFrictionMultiplier()) * frictionMultiplier * .1f;
                }
                else
                {
                    velocity -= velocity * (1 - tileIn.getFrictionMultiplier()) * frictionMultiplier * 5f;
                }
                

            }

            location = location + velocity;
            
            impulse = new Vector2();

            if (tileIn != null && tileIn.tags.Contains(TagReferencer.WATER))
            {
                for (int i = 0; i < Math.Floor(velocity.Length() / 3); i++)
                {
                    world.addEntity(new ParticleBubble(new Vector2(location.X, location.Y + height / 2), world, new Vector2(), 75));
                }

                if (rand.NextDouble() <= .02f)
                {
                    world.addEntity(new ParticleBubble(location, world, new Vector2(), 75));
                }
            }
        }


        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);

            frameSwitcher--;
            if (frameSwitcher <= 0)
            {
                currentFrame = (currentFrame + 1) % Game1.texture_worm_head.Length;
                frameSwitcher = frameSwitchPoint;
                currentTex = Game1.texture_worm_head[currentFrame];
            }

            float distanceFromPlayer = Vector2.Distance(world.player.location, this.location);
            if (distanceFromPlayer <= 500 * world.player.detectionRadiousModifier)
            {

                if (Vector2.Distance(world.player.location, lastPlayerLoc) > 4 && rand.NextDouble() < .05f * world.player.detectionLevel)
                {
                    spook();
                }

                lastPlayerLoc = world.player.location;
            }

            if (aggro && distanceFromPlayer >= 800)
            {
                aggro = false;
                world.decorator.ambientSoundManager.unStartle();
            }

            if (world.player.getCollisionBox().Intersects(this.getCollisionBox()))
            {
                world.player.damage(10, this, Vector2.Normalize(world.player.location - location) + new Vector2(0, -1) * 5f);
            }


            if (aggro)
            {
                float speedBoost = this.getStatusEffectLevel(StatusEffect.status.SPEED, 1);

                if(tileIn != null)
                {
                    bool isUnderground = !tileIn.tags.Contains(TagReferencer.AIR);
                    if (wasUnderground != isUnderground)
                    {
                        world.shakeScreen(40, 80);
                        sprayEarth();
                        SoundManager.getSound("worm-leap").playWithVariance(0, 1f / distanceFromPlayer * 150, (location - world.player.location).X, .2f, .1f, .1f, SoundType.MONSTER);
                    }
                    wasUnderground = isUnderground;
                }
                

                if (!latchLoc.Equals(resting))
                {
                    vibrateGround(latchLoc + new Vector2(0, 120));
                    if (Vector2.Distance(location, latchLoc) < 100)
                    {
                        latchLoc = resting;
                    }

                    impulse += Vector2.Normalize(latchLoc - location) * speedBoost * walkSpeed;
                }
                else
                {
                    Vector2 target = world.player.location + new Vector2(0, 300);
                    impulse += Vector2.Normalize(target - location) * speedBoost * walkSpeed;

                    if (world.player.collideBottom && !wasPlayerCollidingBottomLastRound)
                    {
                        latchLoc = world.player.location + new Vector2(0, -100);
                    }
                }

                wasPlayerCollidingBottomLastRound = world.player.collideBottom;
            }

            if (health <= 0)
            {
                world.decorator.ambientSoundManager.unStartle();
            }
        }

        private void sprayEarth()
        {
            for(int i = 0; i < 20; i++)
            {
                ParticleArbitrary dirtclod = new ParticleArbitrary(location, world, new Vector2((float)rand.NextDouble() * 4 - 2, -(float)rand.NextDouble() * 15), 200, Game1.texture_item_stone);
                dirtclod.width = 10;
                dirtclod.height = 10;
                dirtclod.rotation = (float)(rand.NextDouble() * Math.PI * 2);
                dirtclod.deltaRotation = (float)(rand.NextDouble() * .1 -.05);
                dirtclod.gravityMultiplier = 1;
                world.addEntity(dirtclod);
            }
        }

        private void vibrateGround(Vector2 loc)
        {
            for (int i = 0; i < 3; i++)
            {
                ParticleArbitrary dirtclod = new ParticleArbitrary(loc + new Vector2((float)rand.NextDouble() * 80 - 40, 0), world, new Vector2(0, -(float)rand.NextDouble() * 1.5f), 50, Game1.texture_item_stone);
                dirtclod.width = 10;
                dirtclod.height = 10;
                dirtclod.rotation = (float)(rand.NextDouble() * Math.PI * 2);
                dirtclod.deltaRotation = (float)(rand.NextDouble() * .1 - .05);
                dirtclod.gravityMultiplier = 1;
                world.addEntity(dirtclod);
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


        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            //base.draw(batch, time, offset, groundColor);

            SpriteEffects effect = SpriteEffects.None;
            /*if (velocity.X < 0)
            {
                effect = SpriteEffects.FlipHorizontally;
            }*/


            Rectangle defaultRect = getCollisionBox().ToRect();
            batch.Draw(currentTex, new Rectangle(defaultRect.X + offset.X + drawOffset.X, defaultRect.Y + offset.Y + drawOffset.Y, currentTex.Width, currentTex.Height), null, getDrawColor(groundColor, time), (float)Math.Atan(velocity.Y / velocity.X), new Vector2(currentTex.Width / 2, currentTex.Height / 2), effect, 0);
        }
    }
}
