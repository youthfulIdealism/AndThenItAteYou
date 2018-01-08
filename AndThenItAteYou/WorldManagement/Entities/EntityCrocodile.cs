using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Survive.Sound;
using Survive.WorldManagement.Entities.Particles;
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
    public class EntityCrocodile : Entity, AggroAble
    {
        const int texSwapPoint = 5;//15
        private int currentTexSwap = texSwapPoint;
        private int currentTexIndex;
        Vector2 lastPlayerLoc;
        Texture2D standTex;
        Texture2D runTex;
        Texture2D attackTex;
        Point drawOffset = new Point(0, -10);
        bool aggro;
        int jumps = 3;
        float directionChangeSpeed = .1f;
        float currentDirection = 0;
        SoundEffectInstance currentSound;

        float maxAttackTime = 30;
        float attackTime = 0;
        bool attacking = false;

        public EntityCrocodile(Vector2 location, WorldBase world) : base(location, world)
        {
            aggro = false;
            this.width = 75;
            this.height = 25;
            standTex = Game1.texture_entity_croc_stand[0];
            currentTexIndex = 0;
            runTex = Game1.texture_entity_croc_walk[0];
            attackTex = Game1.texture_entity_croc_attack[0];
            health = 150;
            //walkSpeed = .7f;
            walkSpeed = .4f;
            jumpForce = 11;
            //frictionMultiplier = .85f;
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);

            float distanceFromPlayer = Vector2.Distance(world.player.location, this.location);
            
            currentTexSwap--;
            if (currentTexSwap <= 0)
            {
                if(aggro)
                {
                    if(attacking)
                    {
                        attackTime++;
                        if(attackTime > maxAttackTime)
                        {
                            attackTime = 0;
                            attacking = false;
                            attackTex = Game1.texture_entity_croc_attack[0];
                        }
                        else
                        {
                            if (currentSound == null)
                            {
                                if(currentTexIndex == 1)
                                {
                                    currentSound = SoundManager.getSound("croc-bite").playWithVariance(0, 1f / distanceFromPlayer * 130, (location - world.player.location).X, SoundType.MONSTER);
                                }
                            }
                            else if (currentSound.IsDisposed)
                            {
                                currentSound = null;
                            }
                            currentTexIndex = (int)Math.Floor(attackTime / maxAttackTime * Game1.texture_entity_croc_attack.Length);
                            attackTex = Game1.texture_entity_croc_attack[Math.Min(currentTexIndex, Game1.texture_entity_croc_attack.Length - 1)];
                        }

                    }
                    else
                    {
                        currentTexIndex = (currentTexIndex + 1) % Game1.texture_entity_croc_walk.Length;
                        runTex = Game1.texture_entity_croc_walk[currentTexIndex];
                        currentTexSwap = texSwapPoint;
                    }
                    
                }
                else
                {
                    currentTexIndex = (currentTexIndex + 1) % Game1.texture_entity_croc_stand.Length;
                    standTex = Game1.texture_entity_croc_stand[currentTexIndex];
                    currentTexSwap = texSwapPoint;
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

            
            if (distanceFromPlayer <= 450 * world.player.detectionRadiousModifier)
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
                    //world.player.damage(16, this, Vector2.Normalize(world.player.location - location) + new Vector2(0, -1) * 5f);
                    if(attacking && attackTime > maxAttackTime / 2)
                    {
                        world.player.damage(7, this, Vector2.Normalize(world.player.location - location) + new Vector2(0, -1) * 5f);
                    }else
                    {
                        attacking = true;
                    }
                }

                TileType currentTile = world.getBlock(location);
                bool currentTileIsWater = currentTile != null && currentTile.tags.Contains(TagReferencer.WATER);


                if (/*collideBottom && */!attacking)
                {
                    if (world.player.location.X < this.location.X)
                    {
                        currentDirection = Math.Max(-1, currentDirection - directionChangeSpeed);
                    }
                    else if (world.player.location.X > this.location.X)
                    {
                        currentDirection = Math.Min(1, currentDirection + directionChangeSpeed);
                    }
                    if(currentTileIsWater)
                    {
                        walk(currentDirection * 1.5f);
                    }
                    else
                    {
                        walk(currentDirection);
                    }
                    
                }

                

                if ((collideLeft || collideRight || (currentTileIsWater && location.Y > world.player.location.Y - 50) ) && !attacking)
                {
                    if(collideBottom || currentTileIsWater)
                    {
                        if (!currentTileIsWater)
                        {
                            jumps--;
                        }
                        jump(1);
                    }
                    if(jumps <= 0)
                    {
                        aggro = false;
                        jumps = 3;
                        currentDirection = 0;
                    }
                }
                else
                {
                    jumps = 3;
                }
            }

            if (health <= 0)
            {
                world.addEntity(new ItemDropEntity(location, world, new Item_Meat(1 + rand.Next(2))));
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

        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            //base.draw(batch, time, offset, Color.Red);

            Texture2D currentTex = null;
            if (aggro)
            {
                if(attacking)
                {
                    currentTex = attackTex;
                }
                else
                {
                    currentTex = runTex;
                }
            }
            else
            {
                currentTex = standTex;
            }
            SpriteEffects effect = SpriteEffects.None;
            if (velocity.X > 0)
            {
                effect = SpriteEffects.FlipHorizontally;
            }

            Rectangle defaultRect = getCollisionBox().ToRect();
            batch.Draw(currentTex, new Rectangle(defaultRect.X + offset.X + drawOffset.X, defaultRect.Y + offset.Y + drawOffset.Y, currentTex.Width, currentTex.Height), null, getDrawColor(groundColor, time), 0, Vector2.Zero, effect, 0);
        }
    }
}
