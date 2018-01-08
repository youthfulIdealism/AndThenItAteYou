using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Survive.WorldManagement.Entities;
using Survive.Input;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Survive.Input.InputManagers;

namespace Survive.WorldManagement.Inventory.Items
{
    public class Item_Childs_Drawing : Item
    {
        bool drawing = false;
        float counter;
        bool displayFlag = false;
        public Item_Childs_Drawing(int uses) : base(uses)
        {
            this.usesStandardControlScheme = false;
            if (texture == null)
            {
                texture = Game1.texture_item_childs_drawing;
            }
        }

        public override Item clone(int uses)
        {
            return new Item_Childs_Drawing(uses);
        }

        public override int use(PlayerBase user, WorldBase world, Vector2 location, GameTime time, BinaryInputManager inputManager)
        {
            //((Player)user).currentControlManager = new ImageDisplayControlManager(Game1.instance.childsDrawing, ((Player)user));

            if ((inputManager.isDown() && !inputManager.wasDown()) || (displayFlag && inputManager.isDown()))
            {
                displayFlag = true;
            }else
            {
                displayFlag = false;
            }

            drawing =/* currentMouseState.LeftButton == ButtonState.Pressed && */ inputManager.isDown() && displayFlag;
            counter += .16f;
            return 0;
        }

        public override void draw(SpriteBatch batch, PlayerBase user, Point offset, Color groundColor)
        {
            base.draw(batch, user, offset, groundColor);

            if(!drawing)
            {
                return;
            }

            

            if(user.world is World)
            {
                World world = (World)user.world;
                float distanceToLeftTeleporter = Math.Abs(user.location.X - (-world.teleporterChunkLoc * Chunk.tileDrawWidth * Chunk.tilesPerChunk));
                float distanceToRightTeleporter = Math.Abs(user.location.X - (world.teleporterChunkLoc * Chunk.tileDrawWidth * Chunk.tilesPerChunk));

                Texture2D childDrawingTex = Game1.childsDrawing;
                Rectangle drawChildDrawingRect = new Rectangle(
                    Game1.instance.graphics.PreferredBackBufferWidth / 2 - childDrawingTex.Width / 2,
                    Game1.instance.graphics.PreferredBackBufferHeight / 2 - childDrawingTex.Height / 2,
                    childDrawingTex.Width,
                    childDrawingTex.Height);
                batch.Draw(childDrawingTex, drawChildDrawingRect, Color.White);
                



                int direction = 0;
                if(distanceToLeftTeleporter < distanceToRightTeleporter)
                {
                    if(user.location.X - (-world.teleporterChunkLoc * Chunk.tileDrawWidth * Chunk.tilesPerChunk) > 0)
                    {
                        direction = -1;
                    }
                    else
                    {
                        direction = 1;
                    }
                }else
                {
                    if (user.location.X - (world.teleporterChunkLoc * Chunk.tileDrawWidth * Chunk.tilesPerChunk) > 0)
                    {
                        direction = -1;
                    }
                    else
                    {
                        direction = 1;
                    }
                }

                if(direction > 0)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Rectangle drawRect = new Rectangle(
                            (int)(drawChildDrawingRect.Right + i * 10 + 150),
                            (int)(Game1.instance.graphics.PreferredBackBufferHeight / 2),
                            30,
                            30
                            );
                        batch.Draw(Game1.arrow, drawRect, null, Color.White * (float)Math.Sin(-counter * .2f + i * .4f), 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                    }
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Rectangle drawRect = new Rectangle(
                            (int)(drawChildDrawingRect.Left + i * 10 - 150),
                            (int)(Game1.instance.graphics.PreferredBackBufferHeight / 2),
                            30,
                            30
                            );
                        batch.Draw(Game1.arrow, drawRect, Color.White * (float)Math.Sin(counter * .2f + i * .4f));
                    }
                }

                /*Rectangle childDrawingRect = new Rectangle(
                            (int)(user.location.X + offset.X + 200 * direction),
                            (int)(user.location.Y + offset.Y),
                            20,
                            20
                            );
                batch.Draw(Game1.texture_item_childs_drawing, childDrawingRect, Color.White);*/

            }

        }
    }
}
 