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
    public class EntityTapir : Entity, AggroAble
    {
        Vector2 lastPlayerLoc;
        Random random;
        Texture2D selectedTex;
        Point drawOffset = new Point(-10, -5);
        bool aggro;
        int aggroDirection;
        int collidingTimer;
        const float frameSwitchPoint = 7;
        float frameSwitcher;
        int currentFrame;

        public EntityTapir(Vector2 location, WorldBase world) : base(location, world)
        {
            aggro = false;
            this.walkSpeed = .7f;
            this.width = 50;
            this.height = 40;
            random = new Random();
            selectedTex = Game1.texture_tapir[0];
            aggroDirection = random.Next(2) - 2;
            collidingTimer = 0;
            this.health = 125;
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);

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
            if (distanceFromPlayer <= 250 * world.player.detectionRadiousModifier)
            {
                
                if ((distanceFromPlayer <= 60 * world.player.detectionRadiousModifier) || (Vector2.Distance(world.player.location, lastPlayerLoc) > 4 && random.NextDouble() < .05f * world.player.detectionLevel))
                {
                    spook();
                }

                lastPlayerLoc = world.player.location;
            }

            if (collideBottom && aggro)
            {
                frameSwitcher--;
                if (frameSwitcher <= 0)
                {
                    currentFrame = (currentFrame + 1) % Game1.texture_tapir.Length;
                    frameSwitcher = frameSwitchPoint;
                    selectedTex = Game1.texture_tapir[currentFrame];
                }
            }
            else if (collideBottom)
            {
                selectedTex = Game1.texture_tapir[0];
            }
            else
            {
                selectedTex = Game1.texture_tapir[2];
            }

            if (aggro)
            {
                walk(aggroDirection);

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
            }

            if(health <= 0)
            {
                world.addEntity(new ItemDropEntity(location, world, new Item_Meat(3)));
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
            //base.draw(batch, time, offset, Color.Red);

            SpriteEffects effect = SpriteEffects.None;
            if (aggroDirection > 0)
            {
                effect = SpriteEffects.FlipHorizontally;
            }

            Rectangle defaultRect = getCollisionBox().ToRect();
            batch.Draw(selectedTex, new Rectangle(defaultRect.X + offset.X + drawOffset.X + collidingTimer, defaultRect.Y + offset.Y + drawOffset.Y, selectedTex.Width, selectedTex.Height), null, getDrawColor(groundColor, time), 0, Vector2.Zero, effect, 0);
        }
    }
}
