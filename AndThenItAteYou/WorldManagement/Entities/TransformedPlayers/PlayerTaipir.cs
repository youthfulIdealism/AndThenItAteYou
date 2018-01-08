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
    public class PlayerTaipir : TransformedPlayer
    {
        Texture2D selectedFrame;
        const float frameSwitchPoint = 6;
        float frameSwitcher;
        int currentFrame;
        Point drawOffset = new Point(-0, -0);

        public bool isRunning = false;

        public PlayerTaipir(Vector2 location, WorldBase world, Player transformedFrom) : base(location, world, transformedFrom)
        {
            //this.gravityMultiplier = .3f;
            this.jumpForce = 10f;
            this.walkSpeed = .7f;
            this.width = 40;
            this.height = 40;
            this.detectionRadiousModifier = 1.5f;
            this.detectionLevel = 1.5f;
            movementControlManger = new TaiperStandardControlManager(this);
            currentControlManager = movementControlManger;
            inventory.add(new Item_Totem_Blank(1));
            keyedItems[0] = new Item_Totem_Blank(1);
            selectedFrame = Game1.texture_tapir[0];
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
                    currentFrame = (currentFrame + 1) % Game1.texture_tapir.Length;
                    frameSwitcher = frameSwitchPoint;
                    selectedFrame = Game1.texture_tapir[currentFrame];
                }
            }else if(collideBottom)
            {
                selectedFrame = Game1.texture_tapir[0];
            }else
            {
                selectedFrame = Game1.texture_tapir[2];
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

            if (currentControlManager is TaiperStandardControlManager)
            {
                TaiperStandardControlManager current = (TaiperStandardControlManager)currentControlManager;
                if (current.percentageTicksHarvesting > 0)
                {
                    batch.Draw(Game1.block, new Rectangle(Game1.instance.graphics.PreferredBackBufferWidth / 2, Game1.instance.graphics.PreferredBackBufferHeight / 2, 30, 20), Color.DarkGray);
                    batch.Draw(Game1.block, new Rectangle(Game1.instance.graphics.PreferredBackBufferWidth / 2 + 3, Game1.instance.graphics.PreferredBackBufferHeight / 2, 24, (int)(current.percentageTicksHarvesting * 20)), world.decorator.colorManager.groundColor);
                }
            }




            SpriteEffects effect = SpriteEffects.None;
            if (velocity.X > 0)
            {
                effect = SpriteEffects.FlipHorizontally;
            }

            Rectangle defaultRect = getCollisionBox().ToRect();
            batch.Draw(selectedFrame, new Rectangle(defaultRect.X + offset.X + drawOffset.X, defaultRect.Y + offset.Y + drawOffset.Y, selectedFrame.Width, selectedFrame.Height), null, getDrawColor(groundColor, time), 0, Vector2.Zero, effect, 0);
        }

    }
}
