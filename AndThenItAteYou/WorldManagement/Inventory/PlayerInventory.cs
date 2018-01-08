using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Survive.WorldManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Inventory
{
    public class PlayerInventory : DelayedRenderable
    {
        public Entity holder;
        public List<Item> items;
        public int currentItem;
        public Rectangle bounds;
        private int numDrawItems;
        private int spaceBetweenEntries;
        private int drawOffset;
        private float shiftVelocity = 0;

        public PlayerInventory(Entity holder)
        {
            currentItem = 0;
            drawOffset = 0;
            items = new List<Item>();
            this.holder = holder;

            int width = (int)((Game1.instance.graphics.PreferredBackBufferWidth * .8f) - (Game1.instance.graphics.PreferredBackBufferWidth * .8f) % 100);
            int height = 100;

            bounds = new Rectangle(
                Game1.instance.graphics.PreferredBackBufferWidth / 2 - width / 2,
                Game1.instance.graphics.PreferredBackBufferHeight - 20 - Game1.UIInventory_KeyedItem.Height - height,
                width,
                height
                );

            spaceBetweenEntries = 140;
            numDrawItems = (int)Math.Floor((double)(width / spaceBetweenEntries));
        }

        public void consume(Item item, int amount)
        {
            item.uses -= amount;
            if(item.uses <= 0)
            {
                items.Remove(item);
                currentItem = currentItem % items.Count;
            }

            
        }

        public void add(Item item)
        {
            bool added = false;
            foreach(Item i in items)
            {
                if(item.isGroupable(i))
                {
                    added = true;
                    i.uses += item.uses;
                    break;
                }
            }
            if(!added)
            {
                items.Add(item);
            }
            
        }

        public Item getItemOfType(Item item)
        {
            foreach (Item i in items)
            {
                if (item.isGroupable(i))
                {
                    return i;
                }
            }
            return null;
        }

        public void incSelected()
        {
            shiftVelocity += 10;
        }

        public void decSelected()
        {
            shiftVelocity -= 10;
        }

        public Item current()
        {
            return items[currentItem];
        }

        

        public void update(float tpf)
        {
            drawOffset += (int)shiftVelocity;
            shiftVelocity *= .93f;
            if(Math.Abs(shiftVelocity) < .8f && Math.Abs((drawOffset) % spaceBetweenEntries) > 15) {
                if(shiftVelocity < 0)
                {
                    drawOffset -= (int)((drawOffset) % spaceBetweenEntries * .05f);
                }
                else if(shiftVelocity > 0)
                {
                    drawOffset += (int)((drawOffset) % spaceBetweenEntries * .05f);
                }
                
            }
            
            while(drawOffset > spaceBetweenEntries)
            {
                drawOffset -= spaceBetweenEntries;
                currentItem--;
                if(currentItem < 0) { currentItem = items.Count - 1; }
            }
            while (drawOffset < spaceBetweenEntries)
            {
                drawOffset += spaceBetweenEntries;
                currentItem = (currentItem + 1) % items.Count;
            }
        }

        public void draw(SpriteBatch batch, GameTime time, Point offset)
        {
            batch.Draw(Game1.block, bounds, Color.DarkGray);

            //draw the top inventory borders (but not the corners)
            for (int i = 1; i < (bounds.Width / 100) - 1; i++)
            {
                batch.Draw(Game1.UIInventory_Edge, destinationRectangle: new Rectangle(bounds.X + (100 * i), bounds.Y + Game1.UIInventory_Edge.Width, Game1.UIInventory_Edge.Width, Game1.UIInventory_Edge.Height), color: Color.White, rotation: (float)(-Math.PI / 2), effects: SpriteEffects.FlipHorizontally);
            }

            //draw the bottom inventory borders (but not the corners)
            for (int i = 1; i < (bounds.Width / 100) - 1; i++)
            {
                batch.Draw(Game1.UIInventory_Edge, destinationRectangle: new Rectangle(bounds.X + (100 * i), bounds.Bottom/* - Game1.instance.UIInventory_Edge.Width*/, Game1.UIInventory_Edge.Width, Game1.UIInventory_Edge.Height), color: Color.White, rotation: (float)(-Math.PI / 2));
            }


            //draw the inventory top left corner
            batch.Draw(Game1.UIInventory_TopLeftCorner, destinationRectangle: new Rectangle(bounds.Left - Game1.UIInventory_TopLeftCorner.Width / 2, bounds.Top - Game1.UIInventory_TopLeftCorner.Height / 2, Game1.UIInventory_TopLeftCorner.Width, Game1.UIInventory_TopLeftCorner.Height), color: Color.White);
            batch.Draw(Game1.UIInventory_TopLeftCorner, destinationRectangle: new Rectangle(bounds.Right - Game1.UIInventory_TopLeftCorner.Width / 2, bounds.Y - Game1.UIInventory_TopLeftCorner.Height / 2, Game1.UIInventory_TopLeftCorner.Width, Game1.UIInventory_TopLeftCorner.Height), color: Color.White, effects: SpriteEffects.FlipHorizontally);
            batch.Draw(Game1.UIInventory_StandardCorner, destinationRectangle: new Rectangle(bounds.Left, bounds.Bottom - Game1.UIInventory_StandardCorner.Height, Game1.UIInventory_StandardCorner.Width, Game1.UIInventory_StandardCorner.Height), color: Color.White, effects: SpriteEffects.FlipVertically);
            batch.Draw(Game1.UIInventory_StandardCorner, destinationRectangle: new Rectangle(bounds.Right - Game1.UIInventory_StandardCorner.Width, bounds.Bottom - Game1.UIInventory_StandardCorner.Height, Game1.UIInventory_StandardCorner.Width, Game1.UIInventory_StandardCorner.Height), color: Color.White, effects: SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically);

            for (int i = 0; i < numDrawItems; i++)
            {
                int cumulativeIndex = (currentItem + i) % items.Count;
                if (cumulativeIndex < 0) { cumulativeIndex += items.Count; }

                if (items[cumulativeIndex].texture != null)
                {

                    int drawX = (int)(((bounds.X + i * (spaceBetweenEntries) + drawOffset) % (bounds.X + bounds.Width)));
                    if (drawX < bounds.X) { drawX += (bounds.X + bounds.Width); }
                    drawX -= spaceBetweenEntries / 2;

                    Rectangle itemRect = new Rectangle(
                    drawX,
                    bounds.Y + bounds.Height / 2 - 10 - 7,
                    20,
                    20
                    );

                    batch.Draw(items[cumulativeIndex].texture, itemRect, Color.White);
                    batch.DrawString(Game1.gamefont_24, "" + Game1.decimalToBase6(items[cumulativeIndex].uses), itemRect.Location.ToVector2() + new Vector2(-20, -30), Color.White);
                }
            }
        }


        
    }
}
