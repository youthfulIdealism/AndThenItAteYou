using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Survive.Sound;
using Survive.WorldManagement.Inventory;
using Survive.WorldManagement.Inventory.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities
{
    public class EntityTurkey : Entity, AggroAble
    {
        Vector2 lastPlayerLoc;
        Random random;
        Texture2D standTex;
        Texture2D runTex;
        Point drawOffset = new Point(-25, -25);
        bool aggro;
        int aggroDirection;
        SoundEffectInstance fleeSound;

        const float frameSwitchPoint = 5;
        float frameSwitcher;
        int currentFrame;

        public EntityTurkey(Vector2 location, WorldBase world) : base(location, world)
        {
            aggro = false;
            this.width = 50;
            this.height = 50;
            random = new Random();
            standTex = Game1.texture_entity_turkey_stand;
            runTex = Game1.texture_entity_turkey_fly[0];
            aggroDirection = random.Next(2) - 2;
            this.health = 25;
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);

            float distanceFromPlayer = Vector2.Distance(world.player.location, this.location);

            

            if (aggro)
            {
                frameSwitcher--;
                if (frameSwitcher <= 0)
                {
                    currentFrame = (currentFrame + 1) % Game1.texture_entity_turkey_fly.Length;
                    frameSwitcher = frameSwitchPoint;
                    runTex = Game1.texture_entity_turkey_fly[currentFrame];
                }

                StatusEffect slow = getEffect(StatusEffect.status.SLOW);
                float bonusSlow = 1;
                if (slow != null) { bonusSlow = slow.potency; }

                impulse += new Vector2(aggroDirection * .25f, -1f * bonusSlow);

                if(distanceFromPlayer >= 2000)
                {
                    world.killEntity(this);
                }

                if(collideLeft || collideRight)
                {
                    aggroDirection *= -1;
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
                            fleeSound = SoundManager.getSound("turkey-spook").playWithVariance(0, 1f / distanceFromPlayer * 75, (location - world.player.location).X, SoundType.MONSTER);
                        }
                    }
                }


                if (distanceFromPlayer <= 250 * world.player.detectionRadiousModifier)
                {

                    if ((distanceFromPlayer <= 60 * world.player.detectionRadiousModifier) || (Vector2.Distance(world.player.location, lastPlayerLoc) > 4 && random.NextDouble() < .05f * world.player.detectionLevel))
                    {
                        spook();
                        fleeSound = SoundManager.getSound("turkey-spook").playWithVariance(0, 1f / distanceFromPlayer * 75, (location - world.player.location).X, SoundType.MONSTER);
                    }

                    lastPlayerLoc = world.player.location;
                }
            }

            if(health <= 0)
            {
                world.addEntity(new ItemDropEntity(location, world, new Item_Meat(1)));
                world.addEntity(new ItemDropEntity(location, world, new Item_Feather(3)));
                if(fleeSound != null && !fleeSound.IsDisposed)
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
            batch.Draw(currentTex, new Rectangle(defaultRect.X + offset.X + drawOffset.X, defaultRect.Y + offset.Y + drawOffset.Y, currentTex.Width, currentTex.Height), null, getDrawColor(groundColor, time), 0, Vector2.Zero, effect, 0);
        }
    }
}
