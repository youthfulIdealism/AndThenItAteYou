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

namespace Survive.WorldManagement.Entities
{
    public class EntityMoose : Entity, AggroAble
    {
        Vector2 lastPlayerLoc;
        Random random;
        Texture2D standTex;
        Texture2D runTex;
        Point drawOffset = new Point(-25, -25);
        bool aggro;
        int aggroDirection;
        int collidingTimer;
        SoundEffectInstance fleeSound;

        const float frameSwitchPoint = 7;
        float frameSwitcher;
        int currentFrame;

        public EntityMoose(Vector2 location, WorldBase world) : base(location, world)
        {
            aggro = false;
            this.width = 50;
            this.height = 50;
            this.health = 105;
            random = new Random();
            standTex = Game1.texture_entity_moose_stand;
            runTex = Game1.texture_entity_moose_run[0];
            aggroDirection = random.Next(2) - 2;
            collidingTimer = 0;
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);

            float distanceFromPlayer = Vector2.Distance(world.player.location, this.location);

            if (aggro)
            {
                if(fleeSound == null || fleeSound.IsDisposed)
                {
                    fleeSound = SoundManager.getSound("elk-flee").playWithVariance(0, 1f / distanceFromPlayer * 75, (location - world.player.location).X, SoundType.MONSTER);
                }

                if(fleeSound != null && !fleeSound.IsDisposed)
                {
                    fleeSound.Volume = Math.Min(1, Math.Max(0, 1f / distanceFromPlayer * 75));
                }

                frameSwitcher--;
                if (frameSwitcher <= 0)
                {
                    currentFrame = (currentFrame + 1) % Game1.texture_entity_moose_run.Length;
                    frameSwitcher = frameSwitchPoint;
                    runTex = Game1.texture_entity_moose_run[currentFrame];
                }

                walk(aggroDirection);
                if(world.player.getCollisionBox().Intersects(this.getCollisionBox()))
                {
                    world.player.damage(17, this, (Vector2.Normalize(world.player.location - location) * 3 + new Vector2(0, -1)) * 3);
                }

                if(aggroDirection > 0 && collideLeft)
                {
                    collidingTimer += 2;
                }
                else if(aggroDirection < 0 && collideRight)
                {
                    collidingTimer -= 2;
                }

                if(Math.Abs(collidingTimer) >= width)
                {
                    world.killEntity(this);
                }
            }else
            {
                foreach (Entity entity in world.entities)
                {
                    if (entity is Weapon)
                    {
                        if (Vector2.Distance(entity.location, this.location) <= 100)
                        {
                            spook();
                            //fleeSound = SoundManager.getSound("elk-flee").playWithVariance(0, 1f / distanceFromPlayer * 75, (location - world.player.location).X, SoundType.MONSTER);
                        }
                    }
                }


                if (distanceFromPlayer <= 350 * world.player.detectionRadiousModifier)
                {

                    if ((distanceFromPlayer <= 60 * world.player.detectionRadiousModifier) || (Vector2.Distance(world.player.location, lastPlayerLoc) > 4 && random.NextDouble() < .05f * world.player.detectionLevel))
                    {
                        spook();
                        //fleeSound = SoundManager.getSound("elk-flee").playWithVariance(0, 1f / distanceFromPlayer * 75, (location - world.player.location).X, SoundType.MONSTER);
                    }

                    lastPlayerLoc = world.player.location;
                }
            }

            if(health <= 0)
            {
                world.addEntity(new ItemDropEntity(location, world, new Item_Meat(2 + random.Next(3))));
                if (fleeSound != null && !fleeSound.IsDisposed)
                {
                    fleeSound.Stop();
                }
            }

        }

        public void spook()
        {
            if (!aggro)
            {
                aggro = true;
                if (random.Next(2) == 0)
                {
                    aggroDirection = -1;
                }
                else
                {
                    aggroDirection = 1;
                }
            }
        }

        public override void damage(float amt, Entity source, Vector2 force)
        {
            base.damage(amt, source, force);
            aggroDirection *= -1;
        }

        public bool isAggrod()
        {
            return aggro;
        }

        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            //base.draw(batch, time, offset, Color.Red);

            Texture2D currentTex = null;
            if (aggro)
            {
                currentTex = runTex;
            }
            else
            {
                currentTex = standTex;
            }
            SpriteEffects effect = SpriteEffects.None;
            if (aggroDirection < 0)
            {
                effect = SpriteEffects.FlipHorizontally;
            }

            Rectangle defaultRect = getCollisionBox().ToRect();
            batch.Draw(currentTex, new Rectangle(defaultRect.X + offset.X + drawOffset.X + collidingTimer, defaultRect.Y + offset.Y + drawOffset.Y, currentTex.Width, currentTex.Height), null, getDrawColor(groundColor, time), 0, Vector2.Zero, effect, 0);
        }
    }
}
