using Survive.WorldManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Survive.WorldManagement;
using Survive.WorldManagement.Inventory;
using Survive.WorldManagement.Tile;
using Survive.WorldManagement.Tile.Tags;
using Microsoft.Xna.Framework.Graphics;
using Survive.Sound;
using Survive.WorldManagement.Entities.TransformedPlayers;
using Survive.Input.ControlManagers;
using Survive.Input.InputManagers;
using Survive.WorldManagement.Entities.Particles;

namespace Survive.Input
{
    public class TaiperStandardControlManager : BasicPlayerControlManager
    {
        public TaiperStandardControlManager(PlayerTaipir entity) : base(entity) { }

        protected override void handleInventoryOpen(GameTime time, KeyboardState currentKeyboardState, MouseState currentMouseState, KeyboardState prevKeyboardState, MouseState prevMouseState)
        {
            
        }

        protected override void handleWorldInteract(GameTime time, KeyboardState currentKeyboardState, MouseState currentMouseState, KeyboardState prevKeyboardState, MouseState prevMouseState)
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
                            Console.WriteLine(selectedBlock.Equals(TileTypeReferencer.REACTIVE_BUSH_0) || selectedBlock.Equals(TileTypeReferencer.REACTIVE_BUSH_1));
                            if (selectedBlock.Equals(TileTypeReferencer.REACTIVE_BUSH_0) || selectedBlock.Equals(TileTypeReferencer.REACTIVE_BUSH_1))
                            {

                                //entity.world.useBlock(entity.location, entity, entity.inventory.items[entity.inventory.currentItem]);
                                entity.world.placeTile(TileTypeReferencer.AIR, entity.location);
                                ((PlayerTaipir)entity).transformedFrom.hunger += 13;
                                ticksHarvesting = 0;
                                for (int i = 0; i < 7; i++)
                                {
                                    entity.world.addEntity(new ParticleTileBreak(entity.location, entity.world, new Vector2(), selectedBlock, 150));
                                }

                            }
                            else
                            {
                                entity.world.useBlock(entity.location, entity, entity.inventory.items[entity.inventory.currentItem]);

                                ticksHarvesting = 0;
                            }
                            /*
                             * 
                             * 
                             * */



                        }
                    }
                }

            }
            else { ticksHarvesting = 0; }
        }
    }
}
