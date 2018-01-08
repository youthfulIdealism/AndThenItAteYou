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

namespace Survive.WorldManagement.Entities.TransformedPlayers
{
    public class PlayerCondor : TransformedPlayer
    {
        Texture2D selectedFrame;
        const float frameSwitchPoint = 15;
        float frameSwitcher;
        int currentFrame;
        Point drawOffset = new Point(-25, 25);

        const int flapFrameSwitchPoint = 5;
        const int totalFlap = flapFrameSwitchPoint * 5;
        bool flapping = false;

        public PlayerCondor(Vector2 location, WorldBase world, Player transformedFrom) : base(location, world, transformedFrom)
        {
            this.gravityMultiplier = .05f;
            this.jumpForce = .08f;
            this.walkSpeed = .05f;
            this.frictionMultiplier = .5f;
            this.width = 50;
            this.height = 50;
            movementControlManger = new CondorStandardControlManager(this);
            currentControlManager = movementControlManger;
            inventory.add(new Item_Totem_Blank(1));
            keyedItems[0] = new Item_Totem_Blank(1);
            selectedFrame = Game1.texture_condor[0];
        }

        public override void walk(float directionAndVelocityAsPercentOfSpeed)
        {
            float bonusSpeed = 0;
            SortedList<float, StatusEffect> speedList;
            statusEffects.TryGetValue(StatusEffect.status.SPEED, out speedList);
            if (speedList != null && speedList.Count > 0) { bonusSpeed = speedList.ElementAt(speedList.Count - 1).Value.potency; }
            impulse += new Vector2(directionAndVelocityAsPercentOfSpeed * walkSpeed * (1 + bonusSpeed), 0);
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

        public override void jump(float directionAndVelocityAsPercentOfSpeed)
        {
            if(!flapping)
            {
                flapping = true;
                frameSwitcher = flapFrameSwitchPoint;
                currentFrame = 0;
            }
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);
            transformedFrom.hunger -= .005f;

            frameSwitcher--;
            if (flapping)
            {
                float bonusJump = 0;
                SortedList<float, StatusEffect> jumpList;
                statusEffects.TryGetValue(StatusEffect.status.JUMP, out jumpList);
                if (jumpList != null && jumpList.Count > 0) { bonusJump = jumpList.ElementAt(jumpList.Count - 1).Value.potency; }
                impulse += new Vector2(0, -jumpForce * (1 + bonusJump));
                for (int i = 0; i < bonusJump * 3; i++)
                {
                    ParticleJumpBoost particle = new ParticleJumpBoost(location, world, new Vector2(), 100);
                    world.addEntity(particle);
                }


                transformedFrom.hunger -= .3f;
                if (frameSwitcher <= 0)
                {
                    if (currentFrame + 1 == Game1.texture_condor_flap.Length)
                    {
                        flapping = false;
                    }else
                    {
                        currentFrame = (currentFrame + 1) % Game1.texture_condor_flap.Length;
                        frameSwitcher = flapFrameSwitchPoint;
                        selectedFrame = Game1.texture_condor_flap[currentFrame];
                    }
                    

                    
                }
            }
            else
            {
                if (frameSwitcher <= 0)
                {
                    currentFrame = (currentFrame + 1) % Game1.texture_condor.Length;
                    frameSwitcher = frameSwitchPoint;
                    selectedFrame = Game1.texture_condor[currentFrame];
                }
            }
            
           

        }


        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {

            if (drawInventory)
            {
                inventory.draw(batch, time, new Point());
            }

            drawHealthBar(batch, time, offset);
            drawHungerBar(batch, time, offset);
            drawColdBar(batch, time, offset);
            drawSelectedTileIndicator(batch, time, offset);




            SpriteEffects effect = SpriteEffects.None;
            if (velocity.X < 0)
            {
                effect = SpriteEffects.FlipHorizontally;
            }

            Rectangle defaultRect = getCollisionBox().ToRect();
            batch.Draw(selectedFrame, new Rectangle(defaultRect.X + offset.X + drawOffset.X, defaultRect.Y + offset.Y + drawOffset.Y, selectedFrame.Width, selectedFrame.Height), null, getDrawColor(groundColor, time), 0, Vector2.Zero, effect, 0);
        }

    }
}
