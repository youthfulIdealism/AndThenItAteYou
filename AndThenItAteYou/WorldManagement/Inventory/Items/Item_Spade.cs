using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Survive.Input.InputManagers;
using Survive.WorldManagement.Entities;
using Survive.WorldManagement.Tile;
using Survive.WorldManagement.Tile.Tags;
using Survive.Sound;
using Microsoft.Xna.Framework.Graphics;
using Survive.WorldManagement.Entities.Particles;

namespace Survive.WorldManagement.Inventory.Items
{
    public class Item_Spade : Item
    {
        public Point harvestLocation;
        int ticksHarvesting = 0;
        int maxTicksHarvestingTime = 0;

        public Item_Spade(int uses) : base(uses)
        {
            if (texture == null)
            {
                texture = Game1.texture_item_spade;
            }
            this.usesStandardControlScheme = false;
        }

        public override Item clone(int uses)
        {
            return new Item_Spade(uses);
        }

        public override int use(PlayerBase user, WorldBase world, Vector2 location, GameTime time, BinaryInputManager inputManager)
        {
            if (inputManager.isDown())
            {

                TileType selectedBlock = user.world.getBlock(user.location);
                TileType belowBlock = user.world.getBlock(location);
                if (selectedBlock != null && (selectedBlock.TILEID == TileTypeReferencer.REACTIVE_BUSH_0.TILEID || selectedBlock.TILEID == TileTypeReferencer.REACTIVE_BUSH_0.TILEID))
                {
                    if (!inputManager.wasDown() || !user.world.worldLocToTileLoc(user.location).Equals(harvestLocation))
                    {
                        harvestLocation = user.world.worldLocToTileLoc(user.location);
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
                        if (ticksHarvesting > selectedBlock.harvestTicks - 1)
                        {
                            //user.world.useBlock(user.location, user, this);
                            user.world.placeTile(TileTypeReferencer.AIR, user.location);

                            ItemDropEntity dropper = new ItemDropEntity(user.location, world, new Item_Spud(1));
                            Random random = new Random();
                            dropper.velocity += new Vector2(random.Next(1) - 2, -random.Next(10));
                            world.addEntity(dropper);

                            ticksHarvesting = 0;

                            for (int i = 0; i < 7; i++)
                            {
                                world.addEntity(new ParticleTileBreak(user.location, world, new Vector2(), selectedBlock, 150));
                            }
                        }
                    }
                }
                else if (location.Y > user.location.Y + Chunk.tileDrawWidth / 2 && belowBlock != null && belowBlock.tags.Contains(TagReferencer.SOLID))
                {

                    if (!inputManager.wasDown() || !user.world.worldLocToTileLoc(location).Equals(harvestLocation))
                    {
                        harvestLocation = user.world.worldLocToTileLoc(location);
                        ticksHarvesting = 0;
                        maxTicksHarvestingTime = belowBlock.harvestTicks;
                    }

                    if (ticksHarvesting == 0)
                    {
                        SoundManager.getSound(belowBlock.blockBreakSound).playWithVariance(0, .05f, 0, SoundType.MONSTER);
                    }


                    ticksHarvesting++;
                    if (ticksHarvesting > belowBlock.harvestTicks - 1)
                    {
                        user.world.placeTile(TileTypeReferencer.AIR, location);
                        ticksHarvesting = 0;
                        for (int i = 0; i < 7; i++)
                        {
                            world.addEntity(new ParticleTileBreak(location, world, new Vector2(), belowBlock, 150));
                        }
                        return 1;
                    }
                }else
                {
                    ticksHarvesting = 0;
                }
            }
            else
            {
                ticksHarvesting = 0;
            }

            return 0;
        }

        public override void draw(SpriteBatch batch, PlayerBase user, Point offset, Color groundColor)
        {
            if (ticksHarvesting > 0)
            {
                batch.Draw(Game1.block, new Rectangle(Game1.instance.graphics.PreferredBackBufferWidth / 2, Game1.instance.graphics.PreferredBackBufferHeight / 2, 30, 20), Color.DarkGray);
                batch.Draw(Game1.block, new Rectangle(Game1.instance.graphics.PreferredBackBufferWidth / 2 + 3, Game1.instance.graphics.PreferredBackBufferHeight / 2, 24, (int)((float)ticksHarvesting / maxTicksHarvestingTime * 20)), Color.Black);
            }
        }
    }
}