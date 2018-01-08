using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Survive.Input.ControlManagers;
using Survive.Input.InputManagers;
using Survive.Sound;
using Survive.SplashScreens;
using Survive.WorldManagement;
using Survive.WorldManagement.Entities;
using Survive.WorldManagement.Entities.Projectiles;
using Survive.WorldManagement.Inventory;
using Survive.WorldManagement.Tile;
using Survive.WorldManagement.Tile.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.Input
{
    public class BasicPlayerControlManager : ControlManager
    {
        public PlayerBase entity { get; set; }
        public TileTag TagReference { get; private set; }
        public Point harvestLocation;
        protected int ticksHarvesting = 0;
        protected int maxTicksHarvestingTime = 0;

        protected int prevScroll;
        protected int timeFrozen = 0;
        protected const int switchTolerance = 15;

        public BasicPlayerControlManager(PlayerBase entity)
        {
            this.entity = entity;

            prevScroll = Mouse.GetState().ScrollWheelValue;
        }

        public override void switchTo(ControlManager switchedFrom)
        {
            base.switchTo(switchedFrom);
            timeFrozen = switchTolerance;
        }

        protected virtual void handleStandardMovement(GameTime time, KeyboardState currentKeyboardState, MouseState currentMouseState, KeyboardState prevKeyboardState, MouseState prevMouseState)
        {
            if (Game1.keyBindManager.bindings["Left"].isDown())
            {
                if(entity is Player)
                {
                    Player player = (Player)entity;
                    if (player.state.actionPermitted(STATE_ACTIONS.RUN) && !player.collideRight)
                    {
                        entity.walk(-1);
                        entity.lastMoveDirection = -1;
                        ((Player)entity).state.submitStateAction(STATE_ACTIONS.RUN);
                    }
                    
                }else
                {
                    entity.walk(-1);
                    entity.lastMoveDirection = -1;
                }
            }
            else if (Game1.keyBindManager.bindings["Right"].isDown())
            {
                if (entity is Player)
                {
                    Player player = (Player)entity;
                    if (player.state.actionPermitted(STATE_ACTIONS.RUN) && !player.collideLeft)
                    {
                        entity.walk(1);
                        entity.lastMoveDirection = 1;
                        ((Player)entity).state.submitStateAction(STATE_ACTIONS.RUN);
                    }
                }else
                {
                    entity.walk(1);
                    entity.lastMoveDirection = 1;
                }
            }

            if (Game1.keyBindManager.bindings["Up"].isDown())
            {
                
                if (entity is Player)
                {
                    Player player = (Player)entity;
                    if (player.state.actionPermitted(STATE_ACTIONS.JUMP))
                    {
                        entity.jump(1);
                        ((Player)entity).state.submitStateAction(STATE_ACTIONS.JUMP);
                    }
                }else
                {
                    entity.jump(1);
                }
            }
            else if (Game1.keyBindManager.bindings["Down"].isDown())
            {
                if (!entity.collideBottom)
                {
                    entity.impulse += new Vector2(0, .1f);
                }
            }
        }

        protected virtual void handlePauseMenu(GameTime time, KeyboardState currentKeyboardState, MouseState currentMouseState, KeyboardState prevKeyboardState, MouseState prevMouseState)
        {
            if (currentKeyboardState.IsKeyDown(Keys.Escape) && prevKeyboardState.IsKeyUp(Keys.Escape) && Game1.instance.queuedSplashScreens.Count <= 0)
            {
                Game1.instance.queuedSplashScreens.Add(new PauseScreen(0, entity.world));
            }
        }

        protected virtual void handleWorldInteract(GameTime time, KeyboardState currentKeyboardState, MouseState currentMouseState, KeyboardState prevKeyboardState, MouseState prevMouseState)
        {
            if (Game1.keyBindManager.bindings["Use"].isDown())
            {
                TileType selectedBlock = entity.world.getBlock(entity.location);
                if (selectedBlock != null)
                {
                    if (!Game1.keyBindManager.bindings["Use"].wasDown() || !entity.world.worldLocToTileLoc(entity.location).Equals(harvestLocation))
                    {
                        harvestLocation = entity.world.worldLocToTileLoc(entity.location);
                        ticksHarvesting = 0;
                        maxTicksHarvestingTime = selectedBlock.harvestTicks;
                    }

                    if (selectedBlock.tags.Contains(TagReferencer.Harvest))
                    {
                        if (ticksHarvesting == 0)
                        {
                            SoundManager.getSound(selectedBlock.blockBreakSound).playWithVariance(0, .05f, 0, SoundType.MONSTER);
                        }


                        ticksHarvesting++;
                        if (ticksHarvesting > selectedBlock.harvestTicks)
                        {
                            entity.world.useBlock(entity.location, entity, entity.inventory.items[entity.inventory.currentItem]);
                            ticksHarvesting = 0;
                        }
                    }
                    else
                    {
                        entity.world.useBlock(entity.location, entity, entity.inventory.items[entity.inventory.currentItem]);
                    }
                }

                bool affectedByRope = false;
                foreach (UsableEntity ue in entity.world.useableEntities)
                {

                    if (ue.getUseBounds().Intersects(entity.getCollisionBox()))
                    {

                        if (ue is EntityBetterRopeSegment)
                        {
                            if (!affectedByRope)
                            {
                                ue.use(entity.world, entity.location, entity);
                                affectedByRope = true;
                            }
                        }
                        else
                        {
                            ue.use(entity.world, entity.location, entity);
                        }
                    }
                }

            }
            else { ticksHarvesting = 0; }
        }

        protected virtual void handleInventoryOpen(GameTime time, KeyboardState currentKeyboardState, MouseState currentMouseState, KeyboardState prevKeyboardState, MouseState prevMouseState)
        {
            if (Game1.keyBindManager.bindings["Inventory_Open"].click())
            {
                entity.currentControlManager = entity.craftingControlManager;
                entity.currentControlManager.switchTo(entity.currentControlManager);
            }
        }

        protected virtual void handleInventoryDisplay(GameTime time, KeyboardState currentKeyboardState, MouseState currentMouseState, KeyboardState prevKeyboardState, MouseState prevMouseState)
        {
            entity.drawInventory = false;
            if (Game1.keyBindManager.bindings["Inventory_Display"].isDown())
            {
                entity.drawInventory = true;
            }
        }

        protected virtual void handleKeyedItemUpdate(GameTime time, KeyboardState currentKeyboardState, MouseState currentMouseState, KeyboardState prevKeyboardState, MouseState prevMouseState)
        {
            updateKeyedItem(Game1.keyBindManager.bindings["Inventory_0"], 0, time);
            updateKeyedItem(Game1.keyBindManager.bindings["Inventory_1"], 1, time);
            updateKeyedItem(Game1.keyBindManager.bindings["Inventory_2"], 2, time);
            updateKeyedItem(Game1.keyBindManager.bindings["Inventory_3"], 3, time);
            updateKeyedItem(Game1.keyBindManager.bindings["Inventory_4"], 4, time);
            updateKeyedItem(Game1.keyBindManager.bindings["Inventory_5"], 5, time);
        }

        protected virtual void handleCardUpdate(GameTime time, KeyboardState currentKeyboardState, MouseState currentMouseState, KeyboardState prevKeyboardState, MouseState prevMouseState)
        {
            if (entity.cards[0] != null && entity.cards[0].usable)
            {
                entity.cards[0].use(entity, entity.world, entity.selectedLoc, time, Game1.keyBindManager.bindings["Ability_0"]);
            }

            if (entity.cards[1] != null && entity.cards[1].usable)
            {
                entity.cards[1].use(entity, entity.world, entity.selectedLoc, time, Game1.keyBindManager.bindings["Ability_1"]);
            }
        }

        protected virtual void handleScrollUpdate(GameTime time, KeyboardState currentKeyboardState, MouseState currentMouseState, KeyboardState prevKeyboardState, MouseState prevMouseState)
        {
            if (prevScroll < Mouse.GetState().ScrollWheelValue)
            {
                entity.inventory.decSelected();
            }
            else if (prevScroll > Mouse.GetState().ScrollWheelValue)
            {
                entity.inventory.incSelected();
            }
            prevScroll = Mouse.GetState().ScrollWheelValue;
        }

        public virtual void handleLookAt(GameTime time, KeyboardState currentKeyboardState, MouseState currentMouseState, KeyboardState prevKeyboardState, MouseState prevMouseState)
        {
            Vector2 lookVec = (Mouse.GetState().Position.ToVector2() - new Vector2(Game1.instance.graphics.PreferredBackBufferWidth / 2, Game1.instance.graphics.PreferredBackBufferHeight / 2));
            entity.selectedLoc = entity.location + lookVec;
            if ((entity.location - entity.selectedLoc).Length() > Chunk.tileDrawWidth)
            {
                entity.selectedLoc = entity.location + (Vector2.Normalize(lookVec) * Chunk.tileDrawWidth);
            }
        }

        public override void acceptInput(GameTime time, KeyboardState currentKeyboardState, MouseState currentMouseState, KeyboardState prevKeyboardState, MouseState prevMouseState)
        {
            Game1.keyBindManager.update(Mouse.GetState(), Keyboard.GetState());

            timeFrozen--;
            if (timeFrozen > 0)
            {
                return;
            }


            handleStandardMovement(time, currentKeyboardState, currentMouseState, prevKeyboardState, prevMouseState);
            handlePauseMenu(time, currentKeyboardState, currentMouseState, prevKeyboardState, prevMouseState);
            handleWorldInteract(time, currentKeyboardState, currentMouseState, prevKeyboardState, prevMouseState);
            handleInventoryOpen(time, currentKeyboardState, currentMouseState, prevKeyboardState, prevMouseState);
            handleInventoryDisplay(time, currentKeyboardState, currentMouseState, prevKeyboardState, prevMouseState);
            handleKeyedItemUpdate(time, currentKeyboardState, currentMouseState, prevKeyboardState, prevMouseState);
            handleCardUpdate(time, currentKeyboardState, currentMouseState, prevKeyboardState, prevMouseState);
            handleScrollUpdate(time, currentKeyboardState, currentMouseState, prevKeyboardState, prevMouseState);
            handleLookAt(time, currentKeyboardState, currentMouseState, prevKeyboardState, prevMouseState);

        }

        private void updateKeyedItem(BinaryInputManager manager, int keyedItemSlot, GameTime time)
        {
            if (entity.keyedItems[keyedItemSlot] != null)
            {
                Item item = entity.inventory.getItemOfType(entity.keyedItems[keyedItemSlot]);
                if (item != null)
                {
                    if (item.usesStandardControlScheme)
                    {
                        if (manager.click())
                        {
                            int amtConsumed = item.use(entity, entity.world, entity.selectedLoc, time, manager);
                            entity.inventory.consume(item, amtConsumed);
                        }
                    }
                    else
                    {
                        int amtConsumed = item.use(entity, entity.world, entity.selectedLoc, time, manager);
                        entity.inventory.consume(item, amtConsumed);
                    }
                }
            }
        }

        public float percentageTicksHarvesting
        {
            get { return (float)ticksHarvesting / maxTicksHarvestingTime; }
        }

        public override void draw(SpriteBatch batch)
        {
            base.draw(batch);

            for (int i = 0; i < entity.keyedItems.Length; i++)
            {
                if (entity.keyedItems[i] != null)
                {
                    Item item = entity.inventory.getItemOfType(entity.keyedItems[i]);
                    if (item != null)
                    {
                        item.draw(batch, entity, (-entity.world.playerLoc).ToPoint() + new Point(Game1.instance.graphics.PreferredBackBufferWidth / 2, (Game1.instance.graphics.PreferredBackBufferHeight / 2)), Color.White);
                    }
                }
            }
        }
    }
}
