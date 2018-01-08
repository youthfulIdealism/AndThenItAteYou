using Survive.WorldManagement.Tile.Tags.OnUseTags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Tile.Tags
{
    public static class TagReferencer
    {
        public static TileTag SOLID = new SOLID();
        public static TileTag AIR = new AIR();
        public static TileTag DRAWOUTSIDEOFBOUNDS = new DRAWOUTSIDEOFBOUNDS();
        public static TileTag FLAMMABLE = new FLAMMABLE();
        public static TileTag Climbeable = new Climbeable();
        public static TileTag Harvest = new Harvest();
        public static TileTag Teleporter = new Teleporter();
        public static TileTag TotemRabbit = new TotemRabbit();
        public static TileTag TotemTaipir = new TotemTapir();
        public static TileTag TotemCrocodile = new TotemCrocodile();
        public static TileTag TotemCondor = new TotemCondor();
        public static TileTag TotemFalcon = new TotemFalcon();
        public static TileTag Shelter = new SHELTER();
        public static TileTag Recharge = new Recharge();
        public static TileTag WATER = new WATER();
        public static TileTag Lamp = new Lamp();
        public static TileTag treasure = new Treasure();
        //private static bool hasRegisteredTags = false;
        /*public static void setUp()
        {
            if(!hasRegisteredTags)
            {
                hasRegisteredTags = true;
            }
        }*/
    }
}
