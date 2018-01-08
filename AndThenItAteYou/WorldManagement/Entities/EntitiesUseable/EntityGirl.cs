using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Survive.Input;
using Survive.WorldManagement.Entities.Projectiles;
using Survive.WorldManagement.Entities.Speech;
using Survive.WorldManagement.Inventory;
using Survive.WorldManagement.Inventory.Items;
using Survive.WorldManagement.Tile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities
{
    public class EntityGirl : UsableEntity
    {
        Texture2D selectedFrame;
        Texture2D standTex;
        public Texture2D[] texture_run;
        Point drawOffset = new Point(-10, -15);
        public bool isRunning = false;
        public bool touchedPlayer = false;
        const int frameSwitchPoint = 5;
        float frameSwitcher;
        int currentRunFrame;
        float direction = 0;
        const int checkForPlayerDistanceInterval = 300;
        int checkForPlayerDistanceCounter = 0;

        public EntityGirl(Vector2 location, WorldBase world) : base(location, world)
        {
            this.width = 50;
            this.height = 50;
            standTex = Game1.texture_entity_girl_stand;
            texture_run = Game1.texture_entity_girl_run;
            selectedFrame = standTex;
            this.walkSpeed = .6f;
            this.jumpForce = 14;
            this.blocksWeaponsOnHit = false;
        }

        public override void playHitSound()
        {
            
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);
            isRunning = false;

            

            if(touchedPlayer)
            {
                checkForPlayerDistanceCounter++;
                if (checkForPlayerDistanceCounter > checkForPlayerDistanceInterval)
                {
                    if (Vector2.Distance(location, world.player.location) > 300)
                    {
                        world.player.speechManager.addSpeechBubble(new SpeechBubble(Game1.icon_girl));
                    }
                    checkForPlayerDistanceCounter = 0;
                }


                if (world.player.location.X < this.location.X)
                {
                    direction = -1;
                }
                else if (world.player.location.X > this.location.X)
                {
                    direction = 1;
                }

                if (Math.Abs(location.X - world.player.location.X) >= 25)
                {
                    walk(direction);
                }
                    


                if(world.player.location.Y < this.location.Y || collideLeft || collideRight)
                {
                    AABB collisionBox = getCollisionBox();
                    collisionBox = new AABB(collisionBox.X - 20, collisionBox.Y - 20, collisionBox.Width + 40, collisionBox.Height + 40);
                    foreach (UsableEntity ue in world.useableEntities)
                    {
                        
                        if ((ue is EntityRopeSegment || ue is EntityBetterRopeSegment) && ue.getUseBounds().Intersects(collisionBox))
                        {
                            impulse += new Vector2(0, -.71f);
                            break;
                        }
                    }

                    TileType currentTile = world.getBlock(location);
                    if(currentTile != null && currentTile.tags.Contains(Tile.Tags.TagReferencer.Climbeable))
                    {
                        impulse += new Vector2(0, -.71f);
                    }else
                    {
                        jump(1);
                    }
                }
            }





            if(collideBottom && isRunning)
            {
                frameSwitcher--;
                if (frameSwitcher <= 0)
                {
                    currentRunFrame = (currentRunFrame + 1) % texture_run.Length;
                    frameSwitcher = frameSwitchPoint;
                    selectedFrame = texture_run[currentRunFrame];
                }
            }else
            {
                selectedFrame = standTex;
            }
        }

        public override void damage(float amt, Entity source, Vector2 force)
        {
            //base.damage(amt, source, force);
        }

        public override void walk(float directionAndVelocityAsPercentOfSpeed)
        {
            isRunning = true;
            base.walk(directionAndVelocityAsPercentOfSpeed);
        }

        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            SpriteEffects effect = SpriteEffects.None;
            if(direction > 0)
            {
                effect = SpriteEffects.FlipHorizontally;
            }
            
            Rectangle defaultRect = getCollisionBox().ToRect();
            batch.Draw(selectedFrame, new Rectangle(defaultRect.X + offset.X + drawOffset.X, defaultRect.Y + offset.Y + drawOffset.Y, selectedFrame.Width, selectedFrame.Height), null, groundColor, 0, Vector2.Zero, effect, 0);

            if(!touchedPlayer)
            {
                batch.DrawString(Game1.gamefont_24, "E", location + offset.ToVector2() + new Vector2(0, -25 + (float)(Math.Sin(time.TotalGameTime.Milliseconds * .005f)) * 5), Color.White);
            }
           

        }

        public override AABB getUseBounds()
        {
            return getCollisionBox();
        }

        public override void use(WorldBase world, Vector2 location, Entity user)
        {
            if(!touchedPlayer)
            {
                touchedPlayer = true;
                ImageDisplayForTimeControlManager imageDisplayer = new ImageDisplayForTimeControlManager(Game1.finds_sister, ((Player)user), 500);
                imageDisplayer.scale = .25f;
                ((Player)user).currentControlManager = imageDisplayer;

                Game1.instance.crafting.registerRecepie(new CraftingRecepie(new Item_Ladder(2), .4f, new Item[] { new Item_Stick(1), new Item_Rope(1) }, new int[] { 1, 1 }));
            }
        }
    }
}
