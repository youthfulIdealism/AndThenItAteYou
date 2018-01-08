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

namespace Survive.WorldManagement.Inventory.Items
{
    public class Item_Axe : Item
    {
        public Point harvestLocation;
        int ticksHarvesting = 0;
        int maxTicksHarvestingTime = 0;

        public Item_Axe(int uses) : base(uses)
        {
            if (texture == null)
            {
                texture = Game1.texture_item_axe;
            }
            this.usesStandardControlScheme = false;
        }

        public override Item clone(int uses)
        {
            return new Item_Axe(uses);
        }

        public override int use(PlayerBase user, WorldBase world, Vector2 location, GameTime time, BinaryInputManager inputManager)
        {
            int consumed = 0;
            if (inputManager.isDown())
            {

                TileType selectedBlock = user.world.getBlock(user.location);
                if (selectedBlock != null && (selectedBlock.TILEID == TileTypeReferencer.REACTIVE_TRUNK_0.TILEID || selectedBlock.TILEID == TileTypeReferencer.REACTIVE_TRUNK_1.TILEID))
                {
                    if (!inputManager.wasDown() || !user.world.worldLocToTileLoc(user.location).Equals(harvestLocation))
                    {
                        harvestLocation = user.world.worldLocToTileLoc(user.location);
                        ticksHarvesting = 0;
                        maxTicksHarvestingTime = selectedBlock.harvestTicks;
                    }

                    if (ticksHarvesting == 0)
                    {
                        SoundManager.getSound(selectedBlock.blockBreakSound).playWithVariance(0, .05f, 0, SoundType.MONSTER);
                    }


                    ticksHarvesting++;
                    if (ticksHarvesting > selectedBlock.harvestTicks)
                    {
                        //user.world.useBlock(user.location, user, this);
                        ticksHarvesting = 0;
                        consumed = 1;
                        for (int x = -3; x <= 3; x++)
                        {
                            for (int y = -7; y <= 0; y++)
                            {
                                Vector2 potentialTrunkLoc = user.location + new Vector2(x * Chunk.tileDrawWidth, y * Chunk.tileDrawWidth);
                                TileType potentialTrunk = user.world.getBlock(potentialTrunkLoc);
                                if (potentialTrunk != null && (potentialTrunk.Equals(TileTypeReferencer.REACTIVE_TRUNK_0) || potentialTrunk.Equals(TileTypeReferencer.REACTIVE_TRUNK_1)))
                                {
                                    world.useBlock(potentialTrunkLoc, user, this);
                                }
                            }
                        }
                    }

                    /*else
                    {
                        world.useBlock(user.location, user, this);
                    }*/
                }
                else
                {
                    if(!inputManager.wasDown())
                    {
                        base.use(user, world, location, time, inputManager);

                        if (user is Player)
                        {
                            Player player = (Player)user;
                            if (player.state.actionPermitted(STATE_ACTIONS.THROW))
                            {
                                Entities.Projectiles.EntityAxe axe = new Entities.Projectiles.EntityAxe(user.location + new Vector2(0, -15), world, user);
                                axe.velocity += Vector2.Normalize(location - user.location) * 15;
                                world.addEntity(axe);

                                SoundManager.getSound("spear-throw").playWithVariance(0, .2f, 0, SoundType.MONSTER);
                                consumed = 1;

                                player.state.decorate(axe);
                                player.state.submitStateAction(STATE_ACTIONS.THROW);
                            }
                        }
                        else
                        {
                            Entities.Projectiles.EntityAxe axe = new Entities.Projectiles.EntityAxe(user.location + new Vector2(0, -15), world, user);
                            axe.velocity += Vector2.Normalize(location - user.location) * 15;
                            world.addEntity(axe);
                            SoundManager.getSound("spear-throw").playWithVariance(0, .2f, 0, SoundType.MONSTER);
                            consumed = 1;
                        }





                        /*Entities.Projectiles.EntityAxe axe = new Entities.Projectiles.EntityAxe(user.location + new Vector2(0, -15), world, user);
                        axe.velocity += Vector2.Normalize(location - user.location) * 15;
                        world.addEntity(axe);

                        SoundManager.getSound("spear-throw").playWithVariance(0, .2f, 0, SoundType.MONSTER);
                        consumed = 1;*/
                    }
                }
            }
            else { ticksHarvesting = 0; }

            return consumed;
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