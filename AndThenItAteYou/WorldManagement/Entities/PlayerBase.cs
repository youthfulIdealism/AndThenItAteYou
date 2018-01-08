using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Survive.Input;
using Survive.Input.ControlManagers;
using Survive.WorldManagement.Entities.Particles;
using Survive.WorldManagement.Entities.Progression;
using Survive.WorldManagement.Entities.Speech;
using Survive.WorldManagement.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities
{
    public abstract class PlayerBase : Entity, DelayedRenderable
    {
        public Item[] keyedItems;
        public Card[] cards;

        public float hunger { get; set; }
        public float warmth { get; set; }
        public PlayerInventory inventory { get; set; }
        public ControlManager currentControlManager { get; set; }
        public ControlManager movementControlManger { get; set; }
        public CraftingScreen craftingControlManager { get; set; }
        public int cardCharges { get; set; } = 3;
        public bool drawUseIndicator;
        public float timeNextToAFire = 0;
        public bool isNextToAFire = false;
        public bool isNextToLight = false;
        float timeRemainingUntilNextBreath = 100;
        public int lastMoveDirection { get; set; }
        public Vector2 selectedLoc { get; set; }
        public Color indicatorColor;
        public bool drawInventory { get; set; }
        public SpeechBubbleManager speechManager { get; private set; }

        public const int reminderFrequency = 700;
        public int timeSinceLastHungerReminder = 0;
        public int timeSinceLastFrostReminder = 0;

        public float detectionLevel;
        public float detectionRadiousModifier;

        public float darknessScaleRestingPoint = 2;
        public const float maxDarknessScale = 5;
        public float darknessScale = 2;

        public String tutorialStringMovement;
        public String tutorialStringInventory;
        public String tutorialStringUseItems;
        public String tutorialStringPostKeybinds;
        public String tutorialStringCrafting;
        public String tutorialStringWeapon;
        public String rebindAndFinish;


        public PlayerBase(Vector2 location, WorldBase world) : base(location, world)
        {
            inventory = new Inventory.PlayerInventory(this);
            speechManager = new SpeechBubbleManager(this);
            drawUseIndicator = false;
            detectionLevel = 1;
            detectionRadiousModifier = 1;
            keyedItems = new Item[6];
            cards = new Card[2];
            warmth = 100;
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);

            inventory.update(1);

            pickUpItems();
            executeFireBehavior();
            performLightBehavior();
            manageHunger();
            manageCold();
            manageHealth();
            manageFrostyBreath();
            speechManager.update();
            manageSpeechQueues();



            float currentTemperature = world.getCurrentTemperature();
        }


        public virtual void pickUpItems()
        {
            foreach (ItemDropEntity item in world.itemEntities)
            {
                if (item.ticksExisted > 20 && item.getCollisionBox().Intersects(getCollisionBox()))
                {
                    world.killEntity(item);
                    inventory.add(item.item);
                }
            }
        }

        public virtual void executeFireBehavior()
        {
            isNextToAFire = false;
            foreach (Entity entity in world.entities)
            {
                if (entity is EntityFire)
                {
                    if (Vector2.Distance(this.location, entity.location) < 100)
                    {
                        timeNextToAFire += .5f;
                        if (timeNextToAFire > 100)
                        {
                            timeNextToAFire = 100;
                        }
                        isNextToAFire = true;
                        isNextToLight = true;
                        break;
                    }
                }
            }
            if (!isNextToAFire)
            {
                timeNextToAFire--;
                if (timeNextToAFire < 0)
                {
                    timeNextToAFire = 0;
                }
            }
        }

        public virtual void performLightBehavior()
        {
            isNextToLight = false;
            if(isNextToAFire)
            {
                isNextToLight = true;
            }
            if(isNextToLight == false)
            {
                if (world.decorator.worldGenSubtype == World.WorldGenSubtype.CENOTE)
                {
                    foreach (Entity entity in world.entities)
                    {
                        if (entity is EntityLamp && Vector2.Distance(this.location, entity.location) < 200)
                        {
                            isNextToLight = true;
                            break;
                        }
                    }
                }
            }
           


            if (isNextToLight)
            {
                darknessScale = Math.Min(darknessScale * 1.01f, maxDarknessScale);
            } else
            {
                if (darknessScale > darknessScaleRestingPoint)
                {
                    darknessScale = Math.Max(darknessScale * .99f, darknessScaleRestingPoint);
                }
            }

        }

        public virtual void manageHealth()
        {
            if (hunger <= 0)
            {
                health -= .01f;
            }
            else
            {
                health += .0025f;
            }

            if (warmth <= 0)
            {
                health -= .02f;
            }

            if (health > 100 + getStatusEffectLevel(StatusEffect.status.HEALTHBOOST, 0))
            {
                health = 100 + getStatusEffectLevel(StatusEffect.status.HEALTHBOOST, 0);
            }
        }

        public virtual void manageSpeechQueues()
        {
            timeSinceLastHungerReminder--;
            timeSinceLastFrostReminder--;


            if (timeSinceLastHungerReminder <= 0) { timeSinceLastHungerReminder = 0; }
            if (timeSinceLastFrostReminder <= 0) { timeSinceLastFrostReminder = 0; }

            if(hunger <= 10 && timeSinceLastHungerReminder <= 0)
            {
                speechManager.addSpeechBubble(Game1.texture_item_meat);
                timeSinceLastHungerReminder = reminderFrequency;
            }

            if (warmth <= 10 && timeSinceLastFrostReminder <= 0)
            {
                speechManager.addSpeechBubble(Game1.texture_item_fire);
                timeSinceLastFrostReminder = reminderFrequency;
            }
        }

        public virtual void manageHunger()
        {
            hunger -= .005f;
            if (hunger <= 0)
            {
                hunger = 0;
            }else if (hunger > 100)
            {
                hunger = 100;
            }
        }

        public virtual void manageCold()
        {
            float currentTemperature = world.getCurrentTemperature();
            if (currentTemperature < 0)
            {
                if (!isNextToAFire)
                {
                    warmth -= .03f;
                }
            }
            else if (currentTemperature > 0 || isNextToAFire)
            {
                warmth += .1f;
            }

            if (warmth > 100)
            {
                warmth = 100;
            }
            else if (warmth <= 0)
            {
                warmth = 0;
            }
        }

        public virtual void manageFrostyBreath()
        {
            float currentTemperature = world.getCurrentTemperature();
            if (currentTemperature <= .15f)
            {
                timeRemainingUntilNextBreath -= 1;
                if (timeRemainingUntilNextBreath <= 0)
                {
                    for (int i = 0; i < 2 + rand.Next(10); i++)
                    {
                        world.addEntity(new ParticleFrostBreath(location + new Vector2(lastMoveDirection * 10, -5), world, new Vector2(lastMoveDirection * 3, 0), 100));
                    }
                    timeRemainingUntilNextBreath = 100 + rand.Next(90);
                }

            }
        }

        public virtual void manageControlManager(GameTime time)
        {
            world.requestDelayedRender(this);
            currentControlManager.acceptInput(time, Keyboard.GetState(), Mouse.GetState(), Game1.lastKeyboardState, Game1.lastMouseState);
        }

        public virtual void drawHealthBar(SpriteBatch batch, GameTime time, Point offset)
        {
            int playerMaxHealth = 100 + (int)getStatusEffectLevel(StatusEffect.status.HEALTHBOOST, 0);
            drawBar(40, 20, health, playerMaxHealth, Color.Red, Game1.icon_health,  batch, time, health < 30);
        }

        public virtual void drawHungerBar(SpriteBatch batch, GameTime time, Point offset)
        {
            drawBar(40, 55, hunger, 100, Color.Orange, Game1.icon_food, batch, time, hunger < 30);
        }

        public virtual void drawColdBar(SpriteBatch batch, GameTime time, Point offset)
        {
            if (warmth < 100 && !isNextToAFire)
            {
                drawBar(40, 90, warmth, 100, Color.Blue, Game1.icon_warmth, batch, time, hunger < 30);
            }
        }

        public virtual void drawBar(int startX, int startY, float value, float maxVal, Color barColor, Texture2D icon, SpriteBatch batch, GameTime time, bool flash)
        {
            //draw the start bar and icon
            batch.Draw(Game1.ui_bar_start_back, new Rectangle(startX, startY, Game1.ui_bar_start.Width, Game1.ui_bar_start.Height), Color.DarkGray);
            batch.Draw(Game1.ui_bar_start, new Rectangle(startX, startY, Game1.ui_bar_start.Width, Game1.ui_bar_start.Height), Color.LightGray);
            batch.Draw(icon, new Rectangle(startX + 5, startY + 5, 20, 20), Color.LightGray);

            int i = 0;
            for(; i < maxVal - Game1.ui_bar_end.Width; i += Game1.ui_bar_middle.Width)
            {
                batch.Draw(Game1.ui_bar_middle_back, new Rectangle(startX + Game1.ui_bar_start.Width + i, startY + 7, Game1.ui_bar_middle.Width, Game1.ui_bar_middle.Height), Color.DarkGray);
                batch.Draw(Game1.ui_bar_middle, new Rectangle(startX + Game1.ui_bar_start.Width + i, startY + 7, Game1.ui_bar_middle.Width, Game1.ui_bar_middle.Height), Color.LightGray);
            }
            batch.Draw(Game1.ui_bar_end_back, new Rectangle(startX + i + Game1.ui_bar_start.Width, startY + 5, Game1.ui_bar_end.Width, Game1.ui_bar_end.Height), Color.DarkGray);
            batch.Draw(Game1.ui_bar_end, new Rectangle(startX + i + Game1.ui_bar_start.Width, startY + 5, Game1.ui_bar_end.Width, Game1.ui_bar_end.Height), Color.LightGray);

            batch.Draw(Game1.block, new Rectangle(startX + Game1.ui_bar_start.Width, startY + 11, (int)(95 * (maxVal / 100)), 15), Color.Black * .15f);
            if (flash)
            {
                batch.Draw(Game1.block, new Rectangle(startX + Game1.ui_bar_start.Width, startY + 11, (int)(95 * (value / 100)), 15), Color.Lerp(barColor, Color.White, (float)(Math.Sin(time.TotalGameTime.Milliseconds * .01f))));
            }
            else
            {
                batch.Draw(Game1.block, new Rectangle(startX + Game1.ui_bar_start.Width, startY + 11, (int)(95 * (value / 100)), 15), barColor);
            }
        }

        public virtual void drawSelectedTileIndicator(SpriteBatch batch, GameTime time, Point offset)
        {
            Point selPoint = world.worldLocToTileLoc(selectedLoc);

            //draw an indicator for which tile is currently selected
            Rectangle drawSelIndicatorRect = new Rectangle(selPoint.X * Chunk.tileDrawWidth + offset.X, selPoint.Y * Chunk.tileDrawWidth + offset.Y, Chunk.tileDrawWidth, Chunk.tileDrawWidth);
            batch.Draw(Game1.block, drawSelIndicatorRect, indicatorColor);
            if (drawUseIndicator)
            {
                batch.DrawString(Game1.gamefont_32, "E", drawSelIndicatorRect.Location.ToVector2(), Color.Black);
            }
        }


        public virtual void drawDarkness(SpriteBatch batch, GameTime time, Point offset)
        {
            if(world.decorator.worldGenSubtype == World.WorldGenSubtype.CENOTE)
            {
                int darknessWidth = (int)(Game1.Darkness.Width * darknessScale);
                int darknessHeight = (int)(Game1.Darkness.Height * darknessScale);

                Rectangle DarknessRectangle = new Rectangle(
                    Game1.instance.graphics.PreferredBackBufferWidth / 2 - darknessWidth / 2,
                    Game1.instance.graphics.PreferredBackBufferHeight / 2 - darknessHeight / 2,
                    darknessWidth,
                    darknessHeight
                );

                Rectangle L = new Rectangle(0, 0, DarknessRectangle.Left, Game1.instance.graphics.PreferredBackBufferHeight);
                Rectangle T = new Rectangle(L.Right, 0, DarknessRectangle.Width, DarknessRectangle.Top);
                Rectangle B = new Rectangle(L.Right, DarknessRectangle.Bottom, DarknessRectangle.Width, DarknessRectangle.Left);
                Rectangle R = new Rectangle(DarknessRectangle.Right, 0, Game1.instance.graphics.PreferredBackBufferWidth - DarknessRectangle.Right, Game1.instance.graphics.PreferredBackBufferHeight);


                batch.Draw(Game1.Darkness, DarknessRectangle, Color.Black);
                batch.Draw(Game1.block, L, Color.Black);
                batch.Draw(Game1.block, T, Color.Black);
                batch.Draw(Game1.block, B, Color.Black);
                batch.Draw(Game1.block, R, Color.Black);
            }
            
        }
        /**
         * 
         * in actuality, a UI drawer overriden to take advantage of delayedRender.
         * 
         * */
        public virtual void draw(SpriteBatch batch, GameTime time, Point offset)
        {
            drawDarkness(batch, time, offset);
            drawHealthBar(batch, time, offset);
            drawHungerBar(batch, time, offset);
            drawColdBar(batch, time, offset);
            drawSelectedTileIndicator(batch, time, offset);
            
            //draw the inventory
            if (drawInventory)
            {
                inventory.draw(batch, time, new Point());
            }

            if (currentControlManager is StandardPlayerControlManager)
            {
                StandardPlayerControlManager current = (StandardPlayerControlManager)currentControlManager;
                if (current.percentageTicksHarvesting > 0)
                {
                    batch.Draw(Game1.block, new Rectangle(Game1.instance.graphics.PreferredBackBufferWidth / 2, Game1.instance.graphics.PreferredBackBufferHeight / 2, 30, 20), Color.DarkGray);
                    batch.Draw(Game1.block, new Rectangle(Game1.instance.graphics.PreferredBackBufferWidth / 2 + 3, Game1.instance.graphics.PreferredBackBufferHeight / 2, 24, (int)(current.percentageTicksHarvesting * 20)), world.decorator.colorManager.groundColor);
                }
            }

            batch.Draw(Game1.texture_entity_fire_mask, new Rectangle(0, 0, Game1.instance.graphics.PreferredBackBufferWidth, Game1.instance.graphics.PreferredBackBufferHeight), Color.White * (timeNextToAFire / 100) * .9f);

            currentControlManager.draw(batch);


            if (cards[0] != null && cards[0].usable)
            {
                batch.Draw(Game1.abilityUIElement, new Rectangle(0, Game1.instance.graphics.PreferredBackBufferHeight - Game1.abilityUIElement.Height, Game1.abilityUIElement.Width, Game1.abilityUIElement.Height), Color.DarkGray);
                batch.Draw(cards[0].iconTex, new Rectangle(42, Game1.instance.graphics.PreferredBackBufferHeight - Game1.abilityUIElement.Height + 5, cards[0].iconTex.Width, cards[0].iconTex.Height), Color.White);

                for(int i = 0; i < cardCharges; i++)
                {
                    Color chargeColor = Color.DarkGray;
                    if(cards[0].charges - 1 < i)
                    {
                        chargeColor = Color.DarkGray * .5f;
                    }
                    batch.Draw(Game1.texture_item_charmstone, new Rectangle(20 + 40 * i, Game1.instance.graphics.PreferredBackBufferHeight - Game1.abilityUIElement.Height - 45, Game1.texture_item_charmstone.Width, Game1.texture_item_charmstone.Height), chargeColor);
                }
            }

            if (cards[1] != null && cards[1].usable)
            {
                batch.Draw(Game1.abilityUIElement, new Rectangle(Game1.instance.graphics.PreferredBackBufferWidth - Game1.abilityUIElement.Width, Game1.instance.graphics.PreferredBackBufferHeight - Game1.abilityUIElement.Height, Game1.abilityUIElement.Width, Game1.abilityUIElement.Height), null, Color.DarkGray, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                batch.Draw(cards[1].iconTex, new Rectangle(Game1.instance.graphics.PreferredBackBufferWidth + 5 - Game1.abilityUIElement.Width, Game1.instance.graphics.PreferredBackBufferHeight - Game1.abilityUIElement.Height + 5, cards[1].iconTex.Width, cards[1].iconTex.Height), Color.White);

                for (int i = 0; i < cardCharges; i++)
                {
                    Color chargeColor = Color.DarkGray;
                    if (cards[1].charges - 1 < i)
                    {
                        chargeColor = Color.DarkGray * .5f;
                    }
                    batch.Draw(Game1.texture_item_charmstone, new Rectangle(Game1.instance.graphics.PreferredBackBufferWidth - Game1.abilityUIElement.Width - 20 - 40 * i, Game1.instance.graphics.PreferredBackBufferHeight - Game1.abilityUIElement.Height - 45, Game1.texture_item_charmstone.Width, Game1.texture_item_charmstone.Height), chargeColor);
                }
            }

            //draw the items tied to keys
            if ((craftingControlManager == null || currentControlManager != craftingControlManager) && !(currentControlManager is GuitarControlManager))
            {
                int spaceBetweenKeyedItems = 20;
                int keyedItemWidth = Game1.UIInventory_KeyedItem.Width;
                int totalKeyedItemSpace = spaceBetweenKeyedItems * 3 + keyedItemWidth * keyedItems.Length;
                int keyedItemStartRenderingX = Game1.instance.graphics.PreferredBackBufferWidth / 2 - totalKeyedItemSpace / 2;
                for (int i = 0; i < keyedItems.Length; i++)
                {
                    Color keyedItemUIElementColor = Color.DarkGray;
                    if (Game1.keyBindManager.bindings["Inventory_" + i].isDown()) { keyedItemUIElementColor = Color.White; };

                    Rectangle drawUIElementRect = new Rectangle(
                        keyedItemStartRenderingX + keyedItemWidth * i + spaceBetweenKeyedItems * i,
                        Game1.instance.graphics.PreferredBackBufferHeight - Game1.UIInventory_KeyedItem.Height,
                        Game1.UIInventory_KeyedItem.Width,
                        Game1.UIInventory_KeyedItem.Height
                        );
                    batch.Draw(Game1.UIInventory_KeyedItem, drawUIElementRect, keyedItemUIElementColor);

                    if (keyedItems[i] != null)
                    {
                        Rectangle drawItemRect = new Rectangle(
                            drawUIElementRect.X + 15,
                            drawUIElementRect.Y + 15,
                            keyedItems[i].texture.Width,
                            keyedItems[i].texture.Height
                            );

                        Color color = Color.DarkGray * .5f;
                        if (inventory.getItemOfType(keyedItems[i]) != null)
                        {
                            color = Color.White;
                            batch.DrawString(Game1.gamefont_24, "" + Game1.decimalToBase6(inventory.getItemOfType(keyedItems[i]).uses), drawItemRect.Location.ToVector2() + new Vector2(-15, -15), Color.White);
                        }

                        batch.Draw(keyedItems[i].texture, drawItemRect, color);
                    }
                }
            }

            speechManager.draw(batch, time, offset);
        }
    }
}
