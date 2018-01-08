using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Survive.WorldManagement.Tile;
using Survive.WorldManagement.Tile.ExtensionTileTypes;
using Survive.WorldManagement.Tile.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement
{
    public static class TileTypeReferencer
    {
        public static TileType DIRT;
        public static TileType AIR;
        public static TileType CAVE;
        public static TileType MUSHROOMS;
        public static TileType STONES;
        public static TileType PORTAL;
        public static TileType PEDESTAL;
        public static TileType PILLAR_HIGH;
        public static TileType PILLAR_LOW;
        public static TileType PILLAR_MID;
        public static TileType POT;
        public static TileType STONE_WALL;
        public static TileType STONE_WINDOW;
        public static TileType TAPESTRY;
        public static TileType STONE_WALL_BROKEN_LEFT;
        public static TileType STONE_WALL_BROKEN_RIGHT;
        public static TileType TOTEM_QUETZOATL;
        public static TileType TOTEM_RABBIT;
        public static TileType TOTEM_TAPIR;
        public static TileType TOTEM_CROCODILE;
        public static TileType TOTEM_CONDOR;
        public static TileType TOTEM_FALCON;
        public static TileType CHARMSTONE;
        public static TileType SLIME;
        public static TileType REACTIVE_GRASS;
        public static TileType REACTIVE_BUSH_0;
        public static TileType REACTIVE_BUSH_1;
        public static TileType REACTIVE_LEAVES_0;
        public static TileType REACTIVE_LEAVES_1;
        public static TileType REACTIVE_TRUNK_0;
        public static TileType REACTIVE_TRUNK_1;
        public static TileType SLICK;
        public static TileType LADDER;
        public static TileType TELEPORTER_0;
        public static TileType TELEPORTER_1;
        public static TileType TELEPORTER_2;
        public static TileType TELEPORTER_3;
        public static TileType TELEPORTER_4;
        public static TileType TELEPORTER_5;
        public static TileType STALAGMITE_BOTTOM;
        public static TileType STALAGMITE_MIDDLE;
        public static TileType STALAGMITE_TOP;
        public static TileType STALAGMITE_TINY;
        public static TileType CAVE_FLOOR;
        public static TileType WATER;
        public static TileType LAMP;
        public static TileType ALTAR;
        public static TileType CHEST;

        public static void Load(ContentManager Content)
        {
            //DO NOT MODIFY WITHOUT ORDER HELP
            //DO NOT MODIFY WITHOUT ORDER HELP
            //DO NOT MODIFY WITHOUT ORDER HELP
            DIRT = new TileType(new TileTag[] { TagReferencer.SOLID, TagReferencer.Shelter }, Content.Load<Texture2D>("Blocks/ground"), true);
            //DIRT.friction = .2f;
            DIRT.friction = .17f;
            DIRT.harvestTicks *= 4;
            DIRT.blockBreakSound = "rock-break";
            AIR = new TileType(new TileTag[] { TagReferencer.AIR }, Content.Load<Texture2D>("Blocks/air"), true);
            AIR.friction = .027f;
            CAVE = new TileType(new TileTag[] { TagReferencer.AIR, TagReferencer.Shelter }, Content.Load<Texture2D>("Blocks/cave"), true);
            MUSHROOMS = new RandomImageTile(new TileTag[] { TagReferencer.AIR, TagReferencer.Harvest }, new Texture2D[] { Content.Load<Texture2D>("Blocks/mushrooms_0"), Content.Load<Texture2D>("Blocks/mushrooms_1"), Content.Load<Texture2D>("Blocks/mushrooms_2"), Content.Load<Texture2D>("Blocks/mushrooms_3") }, true);
            STONES = new RandomImageTile(new TileTag[] { TagReferencer.AIR, TagReferencer.Harvest }, new Texture2D[] { Content.Load<Texture2D>("Blocks/stones_0"), Content.Load<Texture2D>("Blocks/stones_1"), Content.Load<Texture2D>("Blocks/stones_2") }, true);
            STONES.blockBreakSound = "rock-break";
            PORTAL = new RandomImageTile(new TileTag[] { TagReferencer.AIR, TagReferencer.Teleporter }, new Texture2D[] { Content.Load<Texture2D>("Blocks/portal_0"), Content.Load<Texture2D>("Blocks/portal_1"), Content.Load<Texture2D>("Blocks/portal_3") }, true);
            PORTAL.friction += .1f;
            PEDESTAL = new TileType(new TileTag[] { TagReferencer.SOLID, TagReferencer.Shelter }, Content.Load<Texture2D>("Blocks/portal_base"), true);
            PEDESTAL.friction = .4f;

            PILLAR_HIGH = new TileType(new TileTag[] { TagReferencer.AIR }, Content.Load<Texture2D>("City/pillar_high"), true);
            PILLAR_LOW = new TileType(new TileTag[] { TagReferencer.AIR }, Content.Load<Texture2D>("City/pillar_low"), true);
            PILLAR_MID = new TileType(new TileTag[] { TagReferencer.AIR }, Content.Load<Texture2D>("City/pillar_mid"), true);
            POT = new RandomImageTile(new TileTag[] { TagReferencer.AIR, TagReferencer.Harvest }, new Texture2D[] { Content.Load<Texture2D>("City/pot_0"), Content.Load<Texture2D>("City/pot_1"), Content.Load<Texture2D>("City/pot_2"), Content.Load<Texture2D>("City/pot_3") }, true);
            POT.blockBreakSound = "rock-break";
            STONE_WALL = new TileType(new TileTag[] { TagReferencer.AIR, TagReferencer.Shelter }, Content.Load<Texture2D>("City/stoneWall"), true);
            STONE_WINDOW = new TileType(new TileTag[] { TagReferencer.AIR, TagReferencer.Shelter }, Content.Load<Texture2D>("City/stoneWindow"), true);
            TAPESTRY = new TileType(new TileTag[] { TagReferencer.AIR }, Content.Load<Texture2D>("City/tapestry"), true);
            STONE_WALL_BROKEN_LEFT = new RandomImageTile(new TileTag[] { TagReferencer.AIR, TagReferencer.Shelter }, new Texture2D[] { Content.Load<Texture2D>("City/stoneWallBrokenLeft_0"), Content.Load<Texture2D>("City/stoneWallBrokenLeft_1"), Content.Load<Texture2D>("City/stoneWallBrokenLeft_2"), Content.Load<Texture2D>("City/stoneWallBrokenLeft_3") }, true);
            STONE_WALL_BROKEN_RIGHT = new RandomImageTile(new TileTag[] { TagReferencer.AIR, TagReferencer.Shelter }, new Texture2D[] { Content.Load<Texture2D>("City/stoneWallBrokenRight_0"), Content.Load<Texture2D>("City/stoneWallBrokenRight_1"), Content.Load<Texture2D>("City/stoneWallBrokenRight_2"), Content.Load<Texture2D>("City/stoneWallBrokenRight_3") }, true);
            REACTIVE_GRASS = new TileType(new TileTag[] { TagReferencer.AIR }, Content.Load<Texture2D>("Blocks/block_reactive_grass"), true);
            TOTEM_QUETZOATL = new TileType(new TileTag[] { TagReferencer.AIR}, Content.Load<Texture2D>("Blocks/totemblock"), true);
            TOTEM_RABBIT = new TileType(new TileTag[] { TagReferencer.AIR, TagReferencer.TotemRabbit }, Content.Load<Texture2D>("Blocks/totemblock_rabbit"), true);
            TOTEM_TAPIR = new TileType(new TileTag[] { TagReferencer.AIR, TagReferencer.TotemTaipir }, Content.Load<Texture2D>("Blocks/totemblock_tapir"), true);
            CHARMSTONE = new RandomImageTile(new TileTag[] { TagReferencer.AIR, TagReferencer.Recharge }, new Texture2D[] { Content.Load<Texture2D>("City/charmstone_0"), Content.Load<Texture2D>("City/charmstone_1"), Content.Load<Texture2D>("City/charmstone_2"), Content.Load<Texture2D>("City/charmstone_3") }, true);
            CHARMSTONE.blockBreakSound = "rock-break";
            SLIME = new TileType(new TileTag[] { TagReferencer.AIR, TagReferencer.Harvest }, Content.Load<Texture2D>("Blocks/sticky"), true);
            SLIME.friction += .3f;
            SLIME.harvestTicks = SLIME.harvestTicks / 2;
            REACTIVE_BUSH_0 = new TileType(new TileTag[] { TagReferencer.AIR }, Content.Load<Texture2D>("Blocks/grass_0"), true);
            REACTIVE_BUSH_1 = new TileType(new TileTag[] { TagReferencer.AIR }, Content.Load<Texture2D>("Blocks/grass_1"), true);
            REACTIVE_LEAVES_0 = new TileType(new TileTag[] { TagReferencer.AIR }, Content.Load<Texture2D>("Blocks/leaves_0"), true);
            REACTIVE_LEAVES_1 = new TileType(new TileTag[] { TagReferencer.AIR }, Content.Load<Texture2D>("Blocks/leaves_1"), true);
            REACTIVE_TRUNK_0 = new TileType(new TileTag[] { TagReferencer.AIR }, Content.Load<Texture2D>("Blocks/trunk_0"), true);
            REACTIVE_TRUNK_1 = new TileType(new TileTag[] { TagReferencer.AIR }, Content.Load<Texture2D>("Blocks/trunk_1"), true);

            SLICK = new TileType(new TileTag[] { TagReferencer.SOLID, TagReferencer.Shelter }, Content.Load<Texture2D>("Blocks/ground"), true);
            SLICK.friction = .07f;

            LADDER = new TileType(new TileTag[] { TagReferencer.AIR, TagReferencer.Climbeable }, Content.Load<Texture2D>("Blocks/ladder"), true);

            TELEPORTER_0 = new TileType(new TileTag[] { TagReferencer.SOLID, TagReferencer.Shelter }, Content.Load<Texture2D>("Blocks/teleporter_base"), true);
            TELEPORTER_0.friction += .4f;
            TELEPORTER_0.blockBreakSound = "rock-break";
            TELEPORTER_0.harvestTicks *= 6;
            TELEPORTER_1 = new TileType(new TileTag[] { TagReferencer.AIR, TagReferencer.Teleporter }, Content.Load<Texture2D>("Blocks/teleporter_0"), true);
            TELEPORTER_1.friction += .1f;
            TELEPORTER_2 = new TileType(new TileTag[] { TagReferencer.AIR, TagReferencer.Teleporter }, Content.Load<Texture2D>("Blocks/teleporter_1"), true);
            TELEPORTER_2.friction += .1f;
            TELEPORTER_3 = new TileType(new TileTag[] { TagReferencer.AIR, TagReferencer.Teleporter }, Content.Load<Texture2D>("Blocks/teleporter_2"), true);
            TELEPORTER_3.friction += .1f;
            TELEPORTER_4 = new TileType(new TileTag[] { TagReferencer.SOLID, TagReferencer.Shelter }, Content.Load<Texture2D>("Blocks/teleporter_top"), true);
            TELEPORTER_4.blockBreakSound = "rock-break";
            TELEPORTER_4.harvestTicks *= 6;
            TELEPORTER_4.friction += .1f;
            TELEPORTER_5 = new TileType(new TileTag[] { TagReferencer.AIR, TagReferencer.Shelter }, Content.Load<Texture2D>("Blocks/teleporter_ornament"), true);

            STALAGMITE_BOTTOM = new RandomImageTile(new TileTag[] { TagReferencer.AIR }, new Texture2D[] { Content.Load<Texture2D>("Blocks/stalagmite_bottom_0"), Content.Load<Texture2D>("Blocks/stalagmite_bottom_1"), Content.Load<Texture2D>("Blocks/stalagmite_bottom_2") }, true);
            STALAGMITE_MIDDLE = new RandomImageTile(new TileTag[] { TagReferencer.AIR }, new Texture2D[] { Content.Load<Texture2D>("Blocks/stalagmite_middle_0"), Content.Load<Texture2D>("Blocks/stalagmite_middle_1"), Content.Load<Texture2D>("Blocks/stalagmite_middle_2") }, true);
            STALAGMITE_TOP = new RandomImageTile(new TileTag[] { TagReferencer.AIR }, new Texture2D[] { Content.Load<Texture2D>("Blocks/stalagmite_top_0"), Content.Load<Texture2D>("Blocks/stalagmite_top_1"), Content.Load<Texture2D>("Blocks/stalagmite_top_2") }, true);
            STALAGMITE_TINY = new RandomImageTile(new TileTag[] { TagReferencer.AIR }, Game1.instance.loadTextureRange("Blocks/stalagmite_tiny_", 5), true);
            CAVE_FLOOR = new RandomImageTile(new TileTag[] { TagReferencer.AIR }, Game1.instance.loadTextureRange("Blocks/cave_floor_", 8), true);
            WATER = new TileType(new TileTag[] { TagReferencer.WATER }, Content.Load<Texture2D>("Blocks/water"), true);
            WATER.friction += .05f;
            LAMP = new TileType(new TileTag[] { TagReferencer.AIR, TagReferencer.Lamp }, Content.Load<Texture2D>("Blocks/lamp"), true);
            ALTAR = new TileType(new TileTag[] { TagReferencer.AIR, TagReferencer.Harvest }, Content.Load<Texture2D>("Blocks/altar"), true);
            CHEST = new TileType(new TileTag[] { TagReferencer.AIR, TagReferencer.treasure }, Content.Load<Texture2D>("Blocks/chest"), true);

            TOTEM_CROCODILE = new TileType(new TileTag[] { TagReferencer.AIR, TagReferencer.TotemCrocodile }, Content.Load<Texture2D>("Blocks/totemblock_crocodile"), true);
            TOTEM_CONDOR = new TileType(new TileTag[] { TagReferencer.AIR, TagReferencer.TotemCondor }, Content.Load<Texture2D>("Blocks/totemblock_condor"), true);
            TOTEM_FALCON = new TileType(new TileTag[] { TagReferencer.AIR, TagReferencer.TotemFalcon }, Content.Load<Texture2D>("Blocks/totemblock_falcon"), true);
            //DO NOT MODIFY WITHOUT ORDER HELP
            //DO NOT MODIFY WITHOUT ORDER HELP
            //DO NOT MODIFY WITHOUT ORDER HELP
            //DO NOT MODIFY WITHOUT ORDER HELP
        }
    }
}
