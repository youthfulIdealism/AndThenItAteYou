using Microsoft.Xna.Framework;
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
    public class EntityFrog : Entity, AggroAble
    {
        const int texSwapPoint = 30;
        private int currentTexSwap = texSwapPoint;
        private int currentTexIndex;
        Vector2 lastPlayerLoc;
        Random random;
        Texture2D standTex;
        Texture2D jumpTex;
        Texture2D attackTex;
        Point drawOffset = new Point(-25, 0);
        bool aggro;
        float timeOnGround = 0;
        bool attacking = false;
        const int aimTime = 30;
        int currentAimTime = aimTime;


        public EntityFrog(Vector2 location, WorldBase world) : base(location, world)
        {
            aggro = false;
            this.width = 70;
            this.height = 70;
            random = new Random();
            standTex = Game1.texture_entity_frog_stand[0];
            currentTexIndex = 0;
            jumpTex = Game1.texture_entity_frog_jump;
            attackTex = Game1.texture_entity_frog_attack;
            health = 75;
            walkSpeed = 10f;
            jumpForce = jumpForce * 2;
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);

            currentTexSwap--;
            if (currentTexSwap <= 0)
            {
                currentTexIndex = (currentTexIndex + 1) % Game1.texture_entity_frog_stand.Length;
                standTex = Game1.texture_entity_frog_stand[currentTexIndex];
                currentTexSwap = texSwapPoint;
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

            if (aggro)
            {
                //TODO: allow damaging of other entities
                /*if (world.player.getCollisionBox().Intersects(this.getCollisionBox()))
                {
                    world.player.damage(3, this, Vector2.Normalize(world.player.location - location) + new Vector2(0, -1) * 5f);
                }*/

                if(collideBottom)
                {
                    if (timeOnGround == 0)
                    {
                        currentAimTime = aimTime;
                        attacking = rand.NextDouble() < .5f;
                        if(attacking)
                        {
                            SoundManager.getSound("frog-hawk-loogie").playWithVariance(0, 1f / distanceFromPlayer * 75, 0, SoundType.MONSTER);
                        }
                        
                    }
                    timeOnGround++;
                }
                else
                {
                    timeOnGround = 0;
                }
                if(attacking)
                {
                    currentAimTime--;
                    if(currentAimTime <= 0)
                    {
                        EntityFrogSpit spit = new EntityFrogSpit(location + new Vector2(0, -15), world, this);
                        spit.velocity += Vector2.Normalize(world.player.location - (location + new Vector2(0, -15))) * 20;
                        world.addEntity(spit);
                        attacking = false;
                        currentAimTime = aimTime;

                        SoundManager.getSound("frog-spit").playWithVariance(0, 1f / distanceFromPlayer * 75, 0, SoundType.MONSTER);
                    }
                }
                else
                {
                    if (timeOnGround >= 50)
                    {
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
                            jump(1);
                        }
                        else if (distanceFromPlayer <= 300)
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
                            jump(1);
                        }
                    }
                }
            }

            if (health <= 0)
            {
                /*world.addEntity(new ItemDropEntity(location, world, new Item_Guardian_Fang(1)));
                if(!hasDroppedCraftingRecepie)
                {
                    Game1.instance.crafting.registerRecepie(new CraftingRecepie(new Item_Spear_Fanged(1), .4f, new Item[] { new Item_Spear(1), new Item_Guardian_Fang(1) }, new int[] { 1, 1 }));
                    hasDroppedCraftingRecepie = true;
                }*/
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
            //base.draw(batch, time, offset);

            Texture2D currentTex = null;
            if (aggro)
            {

                if(attacking)
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
                }
                
            }
            else
            {
                currentTex = standTex;
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
