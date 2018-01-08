using Microsoft.Xna.Framework;
using Survive.Input;
using Survive.WorldManagement.Entities.Particles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Survive.WorldManagement.Inventory.Items;
using Survive.Sound;

namespace Survive.WorldManagement.Entities.TransformedPlayers
{
    public class PlayerCrocodile : TransformedPlayer
    {

        const int texSwapPoint = 5;//15
        private int currentTexSwap;
        private int currentTexIndex;
        Texture2D standTex;
        Texture2D runTex;
        Texture2D attackTex;

        Texture2D selectedFrame;
        Point drawOffset = new Point(0, -10);

        float maxAttackTime = 30;
        float attackTime = 0;
        public bool attacking = false;
        public bool isRunning = false;
        public bool wasRunning = false;

        public PlayerCrocodile(Vector2 location, WorldBase world, Player transformedFrom) : base(location, world, transformedFrom)
        {
            //this.gravityMultiplier = .3f;
            this.jumpForce = 11f;
            this.walkSpeed = .4f;
            this.width = 75;
            this.height = 25;
            this.detectionRadiousModifier = 1.5f;
            this.detectionLevel = 1.5f;
            movementControlManger = new CrocodileStandardControlManager(this);
            currentControlManager = movementControlManger;
            inventory.add(new Item_Bite(1));
            inventory.add(new Item_Totem_Blank(1));

            keyedItems[0] = new Item_Bite(1);
            keyedItems[1] = new Item_Totem_Blank(1);
            
            selectedFrame = Game1.texture_entity_croc_stand[0];
            walkSpeed = .4f;
            jumpForce = 11;
            this.addStatusEffect(new StatusEffect(StatusEffect.status.HEALTHREGEN, 2, 1, true));
            standTex = Game1.texture_entity_croc_stand[0];
            currentTexIndex = 0;
            runTex = Game1.texture_entity_croc_walk[0];
            attackTex = Game1.texture_entity_croc_attack[0];
            currentTexSwap = texSwapPoint;
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);
            wasRunning = false;
            transformedFrom.hunger -= .07f;

            currentTexSwap--;
            if (currentTexSwap <= 0)
            {
                if (attacking)
                {
                    attackTime++;
                    if (attackTime > maxAttackTime)
                    {
                        SoundManager.getSound("croc-bite").playWithVariance(0, .5f, (location - world.player.location).X, SoundType.MONSTER);
                        attackTime = 0;
                        attacking = false;
                        attackTex = Game1.texture_entity_croc_attack[0];
                    }
                    else
                    {
                        currentTexIndex = (int)Math.Floor(attackTime / maxAttackTime * Game1.texture_entity_croc_attack.Length);
                        attackTex = Game1.texture_entity_croc_attack[Math.Min(currentTexIndex, Game1.texture_entity_croc_attack.Length - 1)];
                    }

                }
                else if (isRunning)
                {
                    currentTexIndex = (currentTexIndex + 1) % Game1.texture_entity_croc_walk.Length;
                    runTex = Game1.texture_entity_croc_walk[currentTexIndex];
                    currentTexSwap = texSwapPoint;
                }
                else
                {
                    currentTexIndex = (currentTexIndex + 1) % Game1.texture_entity_croc_stand.Length;
                    standTex = Game1.texture_entity_croc_stand[currentTexIndex];
                    currentTexSwap = texSwapPoint;
                }
            }
            if(isRunning)
            {
                wasRunning = true;
            }

            if (attacking && attackTime > maxAttackTime / 2)
            {
                foreach (Entity entity in world.entities)
                {
                    if (!entity.Equals(this) && entity.getCollisionBox().Intersects(this.getCollisionBox()))
                    {
                        entity.damage(70, this, Vector2.Normalize(entity.location - location) + new Vector2(0, -1) * 5f);
                    }
                }
            }

            /*
             * if (world.player.getCollisionBox().Intersects(this.getCollisionBox()))
                {
                    //world.player.damage(16, this, Vector2.Normalize(world.player.location - location) + new Vector2(0, -1) * 5f);
                    if(attacking && attackTime > maxAttackTime / 2)
                    {
                       
                    }else
                    {
                        attacking = true;
                    }
                }
             * */

            isRunning = false;
        }

        public override void walk(float directionAndVelocityAsPercentOfSpeed)
        {
            base.walk(directionAndVelocityAsPercentOfSpeed);
            isRunning = true;
        }


        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            //base.draw(batch, time, offset, Color.Red);

            if (drawInventory)
            {
                inventory.draw(batch, time, new Point());
            }

            drawHealthBar(batch, time, offset);
            drawHungerBar(batch, time, offset);
            drawColdBar(batch, time, offset);
            drawSelectedTileIndicator(batch, time, offset);




            SpriteEffects effect = SpriteEffects.None;
            if (velocity.X > 0)
            {
                effect = SpriteEffects.FlipHorizontally;
            }


            if (attacking)
            {
                selectedFrame = attackTex;
            }
            else if(wasRunning)
            {
                selectedFrame = runTex;
            }
            else
            {
                selectedFrame = standTex;
            }

            Rectangle defaultRect = getCollisionBox().ToRect();
            batch.Draw(selectedFrame, new Rectangle(defaultRect.X + offset.X + drawOffset.X, defaultRect.Y + offset.Y + drawOffset.Y, selectedFrame.Width, selectedFrame.Height), null, getDrawColor(groundColor, time), 0, Vector2.Zero, effect, 0);
        }

    }
}
