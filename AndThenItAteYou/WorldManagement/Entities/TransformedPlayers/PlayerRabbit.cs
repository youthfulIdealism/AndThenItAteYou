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
    public class PlayerRabbit : TransformedPlayer
    {
        Texture2D selectedFrame;
        const float frameSwitchPoint = 7;
        float frameSwitcher;
        int currentFrame;
        Point drawOffset = new Point(-0, -23);

        public bool isRunning = false;
        

        public PlayerRabbit(Vector2 location, WorldBase world, Player transformedFrom) : base(location, world, transformedFrom)
        {
            this.gravityMultiplier = .7f;
            this.jumpForce = 10f;
            this.walkSpeed = 1.4f;
            this.width = 20;
            this.height = 20;
            this.detectionRadiousModifier = .1f;
            this.detectionLevel = .1f;
            movementControlManger = new RabbitStandardControlManager(this);
            currentControlManager = movementControlManger;
            inventory.add(new Item_Totem_Blank(1));
            selectedFrame = Game1.texture_rabbit_run[0];

            keyedItems[0] = new Item_Totem_Blank(1);
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);
            transformedFrom.hunger -= .025f;

            if(collideBottom && isRunning)
            {
                frameSwitcher--;
                if (frameSwitcher <= 0)
                {
                    currentFrame = (currentFrame + 1) % Game1.texture_rabbit_run.Length;
                    frameSwitcher = frameSwitchPoint;
                    selectedFrame = Game1.texture_rabbit_run[currentFrame];
                }
            }else if(collideBottom)
            {
                selectedFrame = Game1.texture_rabbit;
            }else
            {
                selectedFrame = Game1.texture_rabbit_run[2];
            }

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

            Rectangle defaultRect = getCollisionBox().ToRect();
            batch.Draw(selectedFrame, new Rectangle(defaultRect.X + offset.X + drawOffset.X, defaultRect.Y + offset.Y + drawOffset.Y, selectedFrame.Width, selectedFrame.Height), null, getDrawColor(groundColor, time) * .5f, 0, Vector2.Zero, effect, 0);
        }

    }
}
