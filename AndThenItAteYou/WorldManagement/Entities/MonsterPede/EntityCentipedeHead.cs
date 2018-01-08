using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Survive.Sound;
using Survive.WorldManagement.Entities.Projectiles;
using Survive.WorldManagement.Inventory;
using Survive.WorldManagement.Inventory.Items;
using Survive.WorldManagement.Tile;
using Survive.WorldManagement.Tile.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities
{
    public class EntityCentipedeHead : Entity, AggroAble
    {
        const int texSwapPoint = 15;
        int texSwapCounter;
        int currentTex = 0;
        Vector2 lastPlayerLoc;
        Texture2D tex;
        Point drawOffset = new Point(10, 0);
        bool aggro;
        SoundEffectInstance currentSound;

        public EntityCentipedeHead(Vector2 location, WorldBase world) : base(location, world)
        {
            aggro = false;
            this.width = 30;
            this.height = 30;
            tex = Game1.texture_monsterpede_head[0];
            health = 150;
            this.walkSpeed = .15f;
            this.location += new Vector2(0, -100);

            int bodySegmentSpacing = (int)(width * .9f);
            int numSegments = 3 + rand.Next(3);
            health += numSegments * 10;
            Entity last = this;
            for (int i = 0; i < numSegments; i++)
            {
                EntityCentipedeBody bodySegment = new EntityCentipedeBody(location + new Vector2(i * bodySegmentSpacing, 0), world, last);
                last = bodySegment;
                bodySegment.currentTex = i % Game1.texture_monsterpede_body.Length;
                world.addEntity(bodySegment);
            }

            EntityCentipedeEnd endSegment = new EntityCentipedeEnd(location + new Vector2(numSegments * bodySegmentSpacing, 0), world, last);
            world.addEntity(endSegment);

            this.frictionMultiplier = .3f;
        }

        public override void walk(float directionAndVelocityAsPercentOfSpeed)
        {
            base.walk(directionAndVelocityAsPercentOfSpeed);
            if(collideLeft || collideRight)
            {
                impulse += new Vector2(0, -1);
            }
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);

            texSwapCounter++;
            if(texSwapCounter >= texSwapPoint)
            {
                texSwapCounter = 0;
                currentTex = (currentTex + 1) % Game1.texture_monsterpede_head.Length;
                tex = Game1.texture_monsterpede_head[currentTex];
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
            if (distanceFromPlayer <= 500 * world.player.detectionRadiousModifier)
            {

                if (Vector2.Distance(world.player.location, lastPlayerLoc) > 4 && rand.NextDouble() < .05f * world.player.detectionLevel)
                {
                    spook();
                }

                lastPlayerLoc = world.player.location;
            }

            if (aggro)
            {
                //TODO: allow damaging of other entities
                if (world.player.getCollisionBox().Intersects(this.getCollisionBox()))
                {
                    //TODO: add poison
                    world.player.damage(3, this, Vector2.Normalize(world.player.location - location) + new Vector2(0, -1) * 5f);
                    if(world.player.getStatusEffectLevel(StatusEffect.status.POISON, 0) == 0)
                    {
                        world.player.addStatusEffect(new StatusEffect(StatusEffect.status.POISON, 1, 600, false));
                    }
                }


                if (world.player.location.X < this.location.X)
                {
                    walk(-1);
                }
                else if (world.player.location.X > this.location.X)
                {
                    walk(1);
                }

                TileType currentTile = world.getBlock(location);
                bool currentTileIsWater = currentTile != null && currentTile.tags.Contains(TagReferencer.WATER);

                if (currentTileIsWater && location.Y > world.player.location.Y - 50)
                {
                    jump(.5f);
                }

                if (currentSound == null)
                {
                    if (rand.NextDouble() < .06f) { currentSound = SoundManager.getSound("centipede-talk").playWithVariance(0, 1f / distanceFromPlayer * 75, (location - world.player.location).X, SoundType.MONSTER); }
                }
                else if (currentSound.IsDisposed)
                {
                    currentSound = null;
                }

                if(distanceFromPlayer >= 1000)
                {
                    aggro = false;
                    world.decorator.ambientSoundManager.unStartle();
                }

            }

            if (health <= 0)
            {
                world.decorator.ambientSoundManager.unStartle();
                if (currentSound != null && !currentSound.IsDisposed)
                {
                    currentSound.Stop();
                    currentSound.Dispose();
                }
                
                //world.addEntity(new ItemDropEntity(location, world, new Item_Bullet(1)));
                //SoundManager.getSound("constable-die").playWithVariance(0, 1f / distanceFromPlayer * 75, (location - world.player.location).X);
            }
        }

        public void spook()
        {
            if (!aggro)
            {
                //UniverseProperties.passedChallenges.Add(1);
                aggro = true;
                world.decorator.ambientSoundManager.startle();
            }
        }

        public bool isAggrod()
        {
            return aggro;
        }

        /*public override void playHitSound()
        {
            SoundManager.getSound("projectile-impact-metal").playWithVariance(0, 1f / Vector2.Distance(location, world.player.location) * 75, (location - world.player.location).X);
        }*/

        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            //base.draw(batch, time, offset, Color.Red);

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
            Rectangle defaultRect = getCollisionBox().ToRect();

            SpriteEffects effect = SpriteEffects.None;
            if (velocity.X < 0)
            {
                effect = SpriteEffects.FlipHorizontally;
                defaultRect = new Rectangle(defaultRect.X - tex.Width / 2, defaultRect.Y, defaultRect.Width, defaultRect.Height);
            }


            
            batch.Draw(tex, new Rectangle(defaultRect.X + offset.X + drawOffset.X, defaultRect.Y + offset.Y + drawOffset.Y, tex.Width, tex.Height), null, getDrawColor(groundColor, time), 0, new Vector2(tex.Width / 2, tex.Height / 2), effect, 0);
        }
    }
}
