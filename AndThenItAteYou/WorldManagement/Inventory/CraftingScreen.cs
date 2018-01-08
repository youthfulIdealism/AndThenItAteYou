using Survive.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Survive.WorldManagement.Entities;
using Microsoft.Xna.Framework.Graphics;
using Survive.Sound;
using Survive.Input.ControlManagers;

namespace Survive.WorldManagement.Inventory
{
    public class CraftingScreen : ControlManager
    {
        public Player player;

        public Rectangle craftingRectangle;
        public Rectangle inventoryRectangle;

        private List<CraftingMenuEntry> craftingMenuEntries;
        public int craftingMenuOffset;
        public int prevMouseScrollValue;
        public int craftingEntryHeight;
        public float craftingMenuVelocity;

        public Rectangle[] keyedItemRects;
        private Item draggedItem = null;

        int highlightedItem = -1;

        public CraftingScreen(Player player)
        {
            this.player = player;
            int screenW = Game1.instance.graphics.PreferredBackBufferWidth;
            int screenH = Game1.instance.graphics.PreferredBackBufferHeight;
            int xBuffer = (int)(screenW * .15f/* * .3f*/);
            int yBuffer = (int)(screenH * .1f/* * .2f*/);

            int craftingRectX = xBuffer;
            int craftingRectY = yBuffer;
            int craftingRectWidth = (int)((screenW * .4f) - xBuffer) - (int)((screenW * .3f) - xBuffer) % 100;//make sure that the value is a multiple of 100, so that it can be drawn correctly.
            int craftingRectHeight = (int)(screenH - 100 - yBuffer * 2) - (int)(screenH - 100 - yBuffer * 2) % 100;//make sure that the value is a multiple of 100, so that it can be drawn correctly.
            craftingRectangle = new Rectangle(craftingRectX, craftingRectY, craftingRectWidth, craftingRectHeight);

            int k = craftingRectangle.Right;
            int inventoryRectY = yBuffer;
            int inventoryRectWidth = (int)(screenW - k - xBuffer) - (int)(screenW - k - xBuffer) % 100;//make sure that the value is a multiple of 100, so that it can be drawn correctly.
            int inventoryRectX = screenW - xBuffer - inventoryRectWidth;
            int inventoryRectHeight = (int)(screenH - 100 - yBuffer * 2) - (int)(screenH - 100 - yBuffer * 2) % 100;//make sure that the value is a multiple of 100, so that it can be drawn correctly.
            inventoryRectangle = new Rectangle(inventoryRectX, inventoryRectY, inventoryRectWidth, inventoryRectHeight);

            craftingMenuEntries = new List<CraftingMenuEntry>();

            craftingEntryHeight = 100;
            craftingMenuOffset = 10;
            craftingMenuVelocity = 0;


            keyedItemRects = new Rectangle[player.keyedItems.Length];
            int spaceBetweenKeyedItems = 20;
            int keyedItemWidth = Game1.UIInventory_KeyedItem.Width;
            int totalKeyedItemSpace = spaceBetweenKeyedItems * 3 + keyedItemWidth * player.keyedItems.Length;
            int keyedItemStartRenderingX = Game1.instance.graphics.PreferredBackBufferWidth / 2 - totalKeyedItemSpace / 2;
            for (int i = 0; i < player.keyedItems.Length; i++)
            {
                keyedItemRects[i] = new Rectangle(
                    keyedItemStartRenderingX + keyedItemWidth * i + spaceBetweenKeyedItems * i,
                    Game1.instance.graphics.PreferredBackBufferHeight - Game1.UIInventory_KeyedItem.Height,
                    Game1.UIInventory_KeyedItem.Width,
                    Game1.UIInventory_KeyedItem.Height
                    );
            }
        }

        public void updateRecepieList()
        {
            List<Item> items = player.inventory.items;
            HashSet<CraftingRecepie> recepies = new HashSet<CraftingRecepie>();
            foreach (Item item in items)
            {
                if (Game1.instance.crafting.getRecepies(item) != null)
                {
                    foreach (CraftingRecepie recepie in Game1.instance.crafting.getRecepies(item))
                    {
                        if (!recepies.Contains(recepie))
                        {
                            recepies.Add(recepie);
                        }
                    }
                }
            }
            List<CraftingRecepie> recepieList = recepies.ToList();

            craftingMenuEntries.Clear();

            int counter = 0;
            
            foreach (CraftingRecepie recepie in recepieList)
            {
                craftingMenuEntries.Add(new CraftingMenuEntry(this, recepie));
                counter++;
            }
            craftingMenuEntries.Sort(new craftingRecepieComparer());
        }

        private class craftingRecepieComparer : IComparer<CraftingMenuEntry>
        {
            public int Compare(CraftingMenuEntry x, CraftingMenuEntry y)
            {
                if(x.craftable && !y.craftable)
                {
                    return -1;
                }
                if (y.craftable && !x.craftable)
                {
                    return 1;
                }
                return 0;
            }
        }

        private class CraftingMenuEntry
        {
            public CraftingRecepie recepie;
            Rectangle baseBounds;
            Rectangle currentBounds;
            int resultStartX;
            int arrowStartX;
            bool highlighted;
            public bool craftable{ get; private set; }

            public CraftingMenuEntry(CraftingScreen screen, CraftingRecepie recepie)
            {
                this.recepie = recepie;
                resultStartX = (int)(screen.craftingRectangle.X + screen.craftingRectangle.Width * .1f);
                arrowStartX = (int)(screen.craftingRectangle.X + screen.craftingRectangle.Width * .6f);
                highlighted = false;
                craftable = true;
                for (int i = 0; i < recepie.components.Length; i++)
                {
                    Item item = recepie.components[i];
                    int cost = recepie.costs[i];
                    if (screen.player.inventory.getItemOfType(item) == null || screen.player.inventory.getItemOfType(item).uses < cost)
                    {
                        craftable = false;
                    }
                }
            }

            public void update(CraftingScreen screen, Point mousePos, int slotNumber)
            {
                baseBounds = new Rectangle(
                    (int)(screen.craftingRectangle.X + screen.craftingRectangle.Width * .05f),
                    (int)(screen.craftingRectangle.Y + screen.craftingRectangle.Height * .05f) + screen.craftingEntryHeight * slotNumber,
                    (int)(screen.craftingRectangle.Width * .9f),
                    (int)(screen.craftingEntryHeight)
                    );

                currentBounds = new Rectangle(baseBounds.X, baseBounds.Y + screen.craftingMenuOffset, baseBounds.Width, baseBounds.Height);
                if (containsPoint(mousePos))
                {
                    highlighted = true;
                }
                else
                {
                    highlighted = false;
                }
            }

            public bool containsPoint(Point point)
            {
                return currentBounds.Contains(point);
            }

            public void draw(SpriteBatch batch, CraftingScreen screen)
            {
                if (highlighted)
                {
                    batch.Draw(Game1.block, currentBounds, Color.Black * .2f);
                }

                bool craftable = true;

                for (int i = 0; i < recepie.components.Length; i++)
                {

                    Item item = recepie.components[i];
                    int cost = recepie.costs[i];
                    Color color = Color.White;
                    if (screen.player.inventory.getItemOfType(item) == null || screen.player.inventory.getItemOfType(item).uses < cost)
                    {
                        color = Color.Red;
                        craftable = false;
                    }

                    Rectangle itemRect = new Rectangle(
                            arrowStartX - 40 - 40 * i,
                            currentBounds.Y + currentBounds.Height / 2 - item.texture.Height / 2,
                            item.texture.Width,
                            item.texture.Height
                        );

                    batch.Draw(item.texture, itemRect, color);

                    batch.DrawString(Game1.gamefont_24, "" + Game1.decimalToBase6(cost), itemRect.Location.ToVector2() + new Vector2(-20, -20), color);
                }

                Color outputColor = Color.White;
                if (!craftable)
                {
                    outputColor = Color.Red;
                }

                batch.Draw(Game1.UIInventory_Arrow,
                    new Rectangle(
                        arrowStartX,
                        currentBounds.Y + currentBounds.Height / 2 - Game1.UIInventory_Arrow.Height / 2,
                        Game1.UIInventory_Arrow.Width,
                        Game1.UIInventory_Arrow.Height),
                    outputColor);

                Rectangle outputRect = new Rectangle(
                            arrowStartX + 25 + Game1.UIInventory_Arrow.Width,
                            currentBounds.Y + currentBounds.Height / 2 - recepie.output.texture.Height / 2,
                            recepie.output.texture.Width,
                            recepie.output.texture.Height
                        );
                batch.Draw(recepie.output.texture, outputRect, outputColor);
                batch.DrawString(Game1.gamefont_24, "" + Game1.decimalToBase6(recepie.output.uses), outputRect.Location.ToVector2() + new Vector2(-20, -20), outputColor);



            }
        }

        public override void switchTo(ControlManager switchedFrom)
        {
            base.switchTo(switchedFrom);
            updateRecepieList();
            prevMouseScrollValue = Mouse.GetState().ScrollWheelValue;
            player.world.pauseRequests++;
            craftingMenuOffset = 0;
            craftingMenuVelocity = 0;
        }

        public override void acceptInput(GameTime time, KeyboardState currentKeyboardState, MouseState currentMouseState, KeyboardState prevKeyboardState, MouseState prevMouseState)
        {
            Game1.keyBindManager.update(Mouse.GetState(), Keyboard.GetState());

            if (Game1.keyBindManager.bindings["Inventory_Open"].click() || (currentKeyboardState.IsKeyDown(Keys.Escape) && prevKeyboardState.IsKeyUp(Keys.Escape)))
            {
                player.currentControlManager = player.movementControlManger;
                player.world.pauseRequests--;
            }
            

            if(craftingRectangle.Contains(currentMouseState.Position))
            {
                if(!craftingRectangle.Contains(prevMouseState.Position))
                {
                    prevMouseScrollValue = currentMouseState.ScrollWheelValue;
                }
                if (currentMouseState.ScrollWheelValue < prevMouseScrollValue)
                {
                    craftingMenuVelocity -= 5;
                }
                else if (currentMouseState.ScrollWheelValue > prevMouseScrollValue)
                {
                    craftingMenuVelocity += 5;
                }
            }
            craftingMenuOffset = (int)Math.Max(craftingRectangle.Y - craftingEntryHeight * craftingMenuEntries.Count, Math.Min( (craftingRectangle.Y + craftingRectangle.Height / 4), craftingMenuOffset + craftingMenuVelocity));


            craftingMenuVelocity *= .9f;
            int ix = 0;
            foreach(CraftingMenuEntry entry in craftingMenuEntries)
            {
                entry.update(this, currentMouseState.Position, ix);
                ix++;
                if (draggedItem == null && entry.containsPoint(currentMouseState.Position) && currentMouseState.LeftButton == ButtonState.Released && prevMouseState.LeftButton == ButtonState.Pressed)
                {
                    CraftingRecepie selectedRecepie = entry.recepie;

                    bool valid = true;

                    //validate that the player has enough stuff in his inventory
                    for (int i = 0; i < selectedRecepie.components.Length; i++)
                    {
                        //TODO: shift into a simple inversion of value instead of this if/else crap
                        if (player.inventory.getItemOfType(selectedRecepie.components[i]) == null || player.inventory.getItemOfType(selectedRecepie.components[i]).uses < selectedRecepie.costs[i])
                        {
                            valid = false;
                            break;
                        }
                    }

                    if (valid)
                    {
                        for (int i = 0; i < selectedRecepie.components.Length; i++)
                        {
                            player.inventory.consume(player.inventory.getItemOfType(selectedRecepie.components[i]), selectedRecepie.costs[i]);
                        }

                        player.inventory.add(selectedRecepie.output.clone(selectedRecepie.output.uses));
                        updateRecepieList();
                        SoundManager.getSound("craft-sucess").playWithVariance(0, .5f, 0, SoundType.MONSTER);
                        break;
                    }
                    else
                    {
                        SoundManager.getSound("craft-fail").playWithVariance(0, .5f, 0, SoundType.MONSTER);
                    }
                }
            }

            highlightedItem = -1;

            if (inventoryRectangle.Contains(currentMouseState.Position))
            {
                //TODO: make this more elegant.
                int bufferBetweenInventoryItemSlots = 5;
                int inventorySlotSize = Game1.UIInventory_ItemHolder.Width;
                int minimumInventoryslotOffsets = 20;

                int numInventorySlotsHorizontal = (inventoryRectangle.Width - minimumInventoryslotOffsets * 2) / (inventorySlotSize + bufferBetweenInventoryItemSlots);
                int inventorySlotsOffsetX = inventoryRectangle.X + minimumInventoryslotOffsets;

                int numInventorySlotsVertical = (inventoryRectangle.Height - minimumInventoryslotOffsets * 2) / (inventorySlotSize + bufferBetweenInventoryItemSlots);
                int inventorySlotsOffsetY = inventoryRectangle.Y + minimumInventoryslotOffsets;

                int slotX = (currentMouseState.Position.X - inventorySlotsOffsetX) / (inventorySlotSize + bufferBetweenInventoryItemSlots);
                int slotY = (currentMouseState.Position.Y - inventorySlotsOffsetY) / (inventorySlotSize + bufferBetweenInventoryItemSlots);

                highlightedItem = slotY * numInventorySlotsHorizontal + slotX;
            }
            

            if (currentMouseState.LeftButton == ButtonState.Released)
            {
                if (draggedItem != null)
                {
                    
                    for (int i = 0; i < player.keyedItems.Length; i++)
                    {
                        if(keyedItemRects[i].Contains(currentMouseState.Position))
                        {
                            for (int q = 0; q < player.keyedItems.Length; q++)
                            {
                                if(player.keyedItems[q] != null && player.keyedItems[q].GetType().Equals(draggedItem.GetType()))
                                {
                                    player.keyedItems[q] = null;
                                }
                            }
                            player.keyedItems[i] = draggedItem.clone(1);
                        }
                    }
                }
                draggedItem = null;
            }
            else if(inventoryRectangle.Contains(currentMouseState.Position) && currentMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
            {
                int selectedInventoryItem = highlightedItem;
                if(selectedInventoryItem < player.inventory.items.Count)
                {
                    draggedItem = player.inventory.items[selectedInventoryItem];
                }
            }

            

            prevMouseScrollValue = Mouse.GetState().ScrollWheelValue;
        }

        


        public override void draw(SpriteBatch batch)
        {
            //draw the background for the crafting menu
            batch.Draw(Game1.block, craftingRectangle, Color.DarkGray * .9f);
            //batch.Draw(Game1.UIInventoryBackground, craftingRectangle, new Rectangle(0, 0, craftingRectangle.Width, craftingRectangle.Height), Color.DarkGray);

            //draw the left-side crafting menu borders (but not the corners)
            for (int i = 0; i < (craftingRectangle.Height / 100); i++)
            {
                batch.Draw(Game1.UIInventory_Edge, new Rectangle(craftingRectangle.X, craftingRectangle.Y + (100 * i), Game1.UIInventory_Edge.Width, Game1.UIInventory_Edge.Height), Color.White);
            }

            //draw the right-side crafting menu borders (but not the corners)
            for (int i = 0; i < (craftingRectangle.Height / 100); i++)
            {
                //batch.Draw(Game1.instance.UIInventory_Edge, new Rectangle(craftingRectangle.X, craftingRectangle.Y + (100 * i), Game1.instance.UIInventory_Edge.Width, Game1.instance.UIInventory_Edge.Height), Color.White);
                batch.Draw(Game1.UIInventory_Edge, destinationRectangle: new Rectangle(craftingRectangle.Right - Game1.UIInventory_Edge.Width, craftingRectangle.Y + (100 * i), Game1.UIInventory_Edge.Width, Game1.UIInventory_Edge.Height), color: Color.White, effects: SpriteEffects.FlipHorizontally);
            }

            //draw the top crafting menu borders (but not the corners)
            for (int i = 1; i < (craftingRectangle.Width / 100); i++)
            {
                //batch.Draw(Game1.instance.UIInventory_Edge, new Rectangle(craftingRectangle.X, craftingRectangle.Y + (100 * i), Game1.instance.UIInventory_Edge.Width, Game1.instance.UIInventory_Edge.Height), Color.White);
                batch.Draw(Game1.UIInventory_Edge, destinationRectangle: new Rectangle(craftingRectangle.X + (100 * i), craftingRectangle.Y + Game1.UIInventory_Edge.Width, Game1.UIInventory_Edge.Width, Game1.UIInventory_Edge.Height), color: Color.White, rotation: (float)(-Math.PI / 2), effects: SpriteEffects.FlipHorizontally);
            }

            //draw the bottom crafting menu borders (but not the corners)
            for (int i = 1; i < (craftingRectangle.Width / 100); i++)
            {
                batch.Draw(Game1.UIInventory_Edge, destinationRectangle: new Rectangle(craftingRectangle.X + (100 * i), craftingRectangle.Bottom, Game1.UIInventory_Edge.Width, Game1.UIInventory_Edge.Height), color: Color.White, rotation: (float)(-Math.PI / 2));
            }

            //draw the crafting menu top left corner
            batch.Draw(Game1.UIInventory_TopLeftCorner, new Rectangle(craftingRectangle.Left - Game1.UIInventory_TopLeftCorner.Width / 2, craftingRectangle.Top - Game1.UIInventory_TopLeftCorner.Height / 2, Game1.UIInventory_TopLeftCorner.Width, Game1.UIInventory_TopLeftCorner.Height), Color.White);
            batch.Draw(Game1.UIInventory_StandardCorner, destinationRectangle: new Rectangle(craftingRectangle.Right - Game1.UIInventory_StandardCorner.Width, craftingRectangle.Y, Game1.UIInventory_StandardCorner.Width, Game1.UIInventory_StandardCorner.Height), color: Color.White, effects: SpriteEffects.FlipHorizontally);
            batch.Draw(Game1.UIInventory_StandardCorner, destinationRectangle: new Rectangle(craftingRectangle.Left, craftingRectangle.Bottom - Game1.UIInventory_StandardCorner.Height, Game1.UIInventory_StandardCorner.Width, Game1.UIInventory_StandardCorner.Height), color: Color.White, effects: SpriteEffects.FlipVertically);
            batch.Draw(Game1.UIInventory_StandardCorner, destinationRectangle: new Rectangle(craftingRectangle.Right - Game1.UIInventory_StandardCorner.Width, craftingRectangle.Bottom - Game1.UIInventory_StandardCorner.Height, Game1.UIInventory_StandardCorner.Width, Game1.UIInventory_StandardCorner.Height), color: Color.White, effects: SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically);


            //draw the background for the inventory borders (but not the corners)
            batch.Draw(Game1.block, inventoryRectangle, Color.DarkGray * .9f);
            //batch.Draw(Game1.UIInventoryBackground, inventoryRectangle, new Rectangle(0, 0, inventoryRectangle.Width, inventoryRectangle.Height), Color.DarkGray);

            //draw the left-side inventory borders (but not the corners)
            for (int i = 1; i < (inventoryRectangle.Height / 100) - 1; i++)
            {
                batch.Draw(Game1.UIInventory_Edge, new Rectangle(inventoryRectangle.X, inventoryRectangle.Y + (100 * i), Game1.UIInventory_Edge.Width, Game1.UIInventory_Edge.Height), Color.White);
            }

            //draw the right-side inventory borders (but not the corners)
            for (int i = 1; i < (inventoryRectangle.Height / 100) - 1; i++)
            {
                batch.Draw(Game1.UIInventory_Edge, destinationRectangle: new Rectangle(inventoryRectangle.Right - Game1.UIInventory_Edge.Width, inventoryRectangle.Y + (100 * i), Game1.UIInventory_Edge.Width, Game1.UIInventory_Edge.Height), color: Color.White, effects: SpriteEffects.FlipHorizontally);
            }


            //draw the top inventory borders (but not the corners)
            for (int i = 1; i < (inventoryRectangle.Width / 100) - 1; i++)
            {
                batch.Draw(Game1.UIInventory_Edge, destinationRectangle: new Rectangle(inventoryRectangle.X + (100 * i), inventoryRectangle.Y + Game1.UIInventory_Edge.Width, Game1.UIInventory_Edge.Width, Game1.UIInventory_Edge.Height), color: Color.White, rotation: (float)(-Math.PI / 2), effects: SpriteEffects.FlipHorizontally);
            }

            //draw the bottom inventory borders (but not the corners)
            for (int i = 1; i < (inventoryRectangle.Width / 100) - 1; i++)
            {
                batch.Draw(Game1.UIInventory_Edge, destinationRectangle: new Rectangle(inventoryRectangle.X + (100 * i), inventoryRectangle.Bottom/* - Game1.instance.UIInventory_Edge.Width*/, Game1.UIInventory_Edge.Width, Game1.UIInventory_Edge.Height), color: Color.White, rotation: (float)(-Math.PI / 2));
            }


            //draw the inventory top left corner
            batch.Draw(Game1.UIInventory_StandardCorner, new Rectangle(inventoryRectangle.Left, inventoryRectangle.Top, Game1.UIInventory_StandardCorner.Width, Game1.UIInventory_StandardCorner.Height), Color.White);
            batch.Draw(Game1.UIInventory_TopLeftCorner, destinationRectangle: new Rectangle(inventoryRectangle.Right - Game1.UIInventory_TopLeftCorner.Width / 2, inventoryRectangle.Y - Game1.UIInventory_TopLeftCorner.Height / 2, Game1.UIInventory_TopLeftCorner.Width, Game1.UIInventory_TopLeftCorner.Height), color: Color.White, effects: SpriteEffects.FlipHorizontally);
            batch.Draw(Game1.UIInventory_StandardCorner, destinationRectangle: new Rectangle(inventoryRectangle.Left, inventoryRectangle.Bottom - Game1.UIInventory_StandardCorner.Height, Game1.UIInventory_StandardCorner.Width, Game1.UIInventory_StandardCorner.Height), color: Color.White, effects: SpriteEffects.FlipVertically);
            batch.Draw(Game1.UIInventory_StandardCorner, destinationRectangle: new Rectangle(inventoryRectangle.Right - Game1.UIInventory_StandardCorner.Width, inventoryRectangle.Bottom - Game1.UIInventory_StandardCorner.Height, Game1.UIInventory_StandardCorner.Width, Game1.UIInventory_StandardCorner.Height), color: Color.White, effects: SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically);


            int bufferBetweenInventoryItemSlots = 5;
            int inventorySlotSize = Game1.UIInventory_ItemHolder.Width;
            int minimumInventoryslotOffsets = 20;

            int numInventorySlotsHorizontal = (inventoryRectangle.Width - minimumInventoryslotOffsets * 2) / (inventorySlotSize + bufferBetweenInventoryItemSlots);
            int inventorySlotsOffsetX = inventoryRectangle.X + minimumInventoryslotOffsets;

            int numInventorySlotsVertical = (inventoryRectangle.Height - minimumInventoryslotOffsets * 2) / (inventorySlotSize + bufferBetweenInventoryItemSlots);
            int inventorySlotsOffsetY = inventoryRectangle.Y + minimumInventoryslotOffsets;

            //draw the boxes for items
            for (int x = 0; x < numInventorySlotsHorizontal; x++)
            {
                for (int y = 0; y < numInventorySlotsVertical; y++)
                {
                    Rectangle inventoryBoxRect = new Rectangle(
                        inventorySlotsOffsetX + x * (inventorySlotSize + bufferBetweenInventoryItemSlots),
                        inventorySlotsOffsetY + y * (inventorySlotSize + bufferBetweenInventoryItemSlots),
                        inventorySlotSize,
                        inventorySlotSize
                        );

                    batch.Draw(Game1.UIInventory_ItemHolder, inventoryBoxRect, Color.White);
                }
            }

            if(highlightedItem >= 0)
            {
                Rectangle highlightedItemBlock = new Rectangle(
                        inventorySlotsOffsetX + highlightedItem % numInventorySlotsHorizontal * (inventorySlotSize + bufferBetweenInventoryItemSlots),
                        inventorySlotsOffsetY + highlightedItem / numInventorySlotsHorizontal * (inventorySlotSize + bufferBetweenInventoryItemSlots),
                        inventorySlotSize,
                        inventorySlotSize
                        );

                batch.Draw(Game1.block, highlightedItemBlock, Color.Black * .15f);
            }
            


            int counter = 0;

            //draw the items themselves
            foreach(Item item in player.inventory.items)
            {
                int slotX = counter % numInventorySlotsHorizontal;
                int slotY = counter / numInventorySlotsHorizontal;

                Rectangle itemRect = new Rectangle(
                        inventorySlotsOffsetX + slotX * (inventorySlotSize + bufferBetweenInventoryItemSlots) - item.texture.Width / 2 + inventorySlotSize / 2 + 5,
                        inventorySlotsOffsetY + slotY * (inventorySlotSize + bufferBetweenInventoryItemSlots) - item.texture.Height / 2 + inventorySlotSize / 2 + 5,
                        item.texture.Width,
                        item.texture.Height
                        );

                batch.Draw(item.texture, itemRect, Color.White);
                batch.DrawString(Game1.gamefont_24, "" + Game1.decimalToBase6(item.uses), itemRect.Location.ToVector2() + new Vector2(-15, -15), Color.White);

                counter++;
            }

            Rectangle screenRect = batch.GraphicsDevice.ScissorRectangle;

            batch.End();
            
            
            batch.Begin(SpriteSortMode.Deferred, null, null, null, Game1.scizzorRasterState);
            batch.GraphicsDevice.ScissorRectangle = new Rectangle(craftingRectangle.X, craftingRectangle.Y + 10, craftingRectangle.Width, craftingRectangle.Height - 20);

            foreach (CraftingMenuEntry entry in craftingMenuEntries)
            {
                entry.draw(batch, this);
            }

            

            batch.End();
            batch.Begin(SpriteSortMode.Deferred, null, null, null, Game1.scizzorRasterState);

            //Reset scissor rectangle to the saved value
            batch.GraphicsDevice.ScissorRectangle = screenRect;

            if (draggedItem != null)
            {
                Rectangle drawItemRect = new Rectangle(
                        Mouse.GetState().Position.X + 15,
                        Mouse.GetState().Position.Y + 15,
                        draggedItem.texture.Width,
                        draggedItem.texture.Height
                        );
                batch.Draw(draggedItem.texture, drawItemRect, Color.White);
            }

            //Draw keyed items
            for (int i = 0; i < player.keyedItems.Length; i++)
            {
                batch.Draw(Game1.UIInventory_KeyedItem, keyedItemRects[i], Color.DarkGray);
                if(keyedItemRects[i].Contains(Mouse.GetState().Position))
                {
                    batch.Draw(Game1.block, new Rectangle(keyedItemRects[i].X, keyedItemRects[i].Y, keyedItemRects[i].Width, keyedItemRects[i].Width), Color.White * .15f);
                }

                if (player.keyedItems[i] != null)
                {
                    Rectangle drawItemRect = new Rectangle(
                        keyedItemRects[i].X + 15,
                        keyedItemRects[i].Y + 15,
                        player.keyedItems[i].texture.Width,
                        player.keyedItems[i].texture.Height
                        );

                    Color color = Color.DarkGray;
                    if (player.inventory.getItemOfType(player.keyedItems[i]) != null)
                    {
                        color = Color.White;
                        batch.DrawString(Game1.gamefont_24, "" + Game1.decimalToBase6(player.inventory.getItemOfType(player.keyedItems[i]).uses), drawItemRect.Location.ToVector2() + new Vector2(-15, -15), Color.White);
                    }

                    batch.Draw(player.keyedItems[i].texture, drawItemRect, color);
                }
            }
        }
    }
}
