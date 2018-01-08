using Microsoft.Xna.Framework;
using Survive.WorldManagement.ContentProcessors;
using Survive.WorldManagement.Entities;
using Survive.WorldManagement.Inventory;
using Survive.WorldManagement.Inventory.Items;
using Survive.WorldManagement.Tile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Procedurals
{
    public class DecorationManager
    {
        public static HashSet<Item> corpseTableRare;
        public static HashSet<Item> corpseTableCommon;

        public DecorationManager(ChunkDecorator decorator)
        {
            for (int i = 0; i < 1 + decorator.rand.Next((Math.Max(decorator.metaDifficulty, 1)) * 8); i++)
            {
                decorator.foliage.Add(new Decoration(new Point(0, 0), new TileType[,] { { TileTypeReferencer.STONES } }));
                decorator.foliageBiome2.Add(new Decoration(new Point(0, 0), new TileType[,] { { TileTypeReferencer.STONES } }));
            }

            for (int i = 0; i < 5; i++)
            {
                decorator.subTerranianFoliage.Add(new Decoration(new Point(0, 0), new TileType[,] { { TileTypeReferencer.MUSHROOMS } }));
                decorator.subTerranianFoliage.Add(new Decoration(new Point(0, 0), new TileType[,] { { TileTypeReferencer.STONES } }));
            }
            decorator.subTerranianFoliage.Add(new Decoration(new Point(0, 0), new TileType[,] { { TileTypeReferencer.CHEST } }));

            if (decorator.metaDifficulty >= 1)
            {
                TileType[] potentialTotems = new TileType[] {  TileTypeReferencer.TOTEM_RABBIT, TileTypeReferencer.TOTEM_TAPIR, TileTypeReferencer.TOTEM_CROCODILE, TileTypeReferencer.TOTEM_CONDOR, TileTypeReferencer.TOTEM_FALCON, };
                TileType chosenTotem = potentialTotems[decorator.rand.Next(potentialTotems.Length)];
                decorator.foliage.Add(new Decoration(new Point(0, 0), new TileType[,] { { chosenTotem } }));
                decorator.foliageBiome2.Add(new Decoration(new Point(0, 0), new TileType[,] { { chosenTotem } }));
            }

            decorator.subTerranianFoliage.Add(new Decoration(new Point(0, 0), new TileType[,] { { TileTypeReferencer.CHARMSTONE } }));
            decorator.subTerranianFoliage.Add(new Decoration(new Point(0, 0), new TileType[,] { { TileTypeReferencer.CHARMSTONE } }));
            
            decorator.subTerranianFoliage.Add(new Decoration(new Point(0, 0), new TileType[,] { { TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER } }));
            decorator.subTerranianFoliage.Add(new Decoration(new Point(0, -6), new TileType[,] { { TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER } }));
            decorator.subTerranianFoliage.Add(new Decoration(new Point(0, -9), new TileType[,] { { TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER } }));

            if(decorator.worldGenSubtype == World.WorldGenSubtype.CENOTE)
            {
                decorator.foliage.Add(new Decoration(new Point(0, 0), new TileType[,] { { TileTypeReferencer.LAMP } }));
                decorator.foliageBiome2.Add(new Decoration(new Point(0, 0), new TileType[,] { { TileTypeReferencer.LAMP } }));
                decorator.foliage.Add(new Decoration(new Point(0, 0), new TileType[,] { { TileTypeReferencer.LAMP } }));
                decorator.foliageBiome2.Add(new Decoration(new Point(0, 0), new TileType[,] { { TileTypeReferencer.LAMP } }));

                decorator.foliage.Add(new Decoration(new Point(0, 0), new TileType[,] { { TileTypeReferencer.ALTAR } }));
                decorator.foliageBiome2.Add(new Decoration(new Point(0, 0), new TileType[,] { { TileTypeReferencer.ALTAR } }));
                decorator.foliage.Add(new Decoration(new Point(0, 0), new TileType[,] { { TileTypeReferencer.LAMP } }));
                decorator.foliageBiome2.Add(new Decoration(new Point(0, 0), new TileType[,] { { TileTypeReferencer.ALTAR } }));
                
            }


            if (decorator.isCity)
            {
                decorator.foliage.Add(DecorationReader.readDecorationFromContent("Decorations/testdecoration.map"));
                decorator.foliageBiome2.Add(DecorationReader.readDecorationFromContent("Decorations/testdecoration.map"));
                decorator.foliage.Add(DecorationReader.readDecorationFromContent("Decorations/city_segment_0"));
                decorator.foliageBiome2.Add(DecorationReader.readDecorationFromContent("Decorations/city_segment_0"));
                decorator.foliage.Add(DecorationReader.readDecorationFromContent("Decorations/city_segment_1"));
                decorator.foliageBiome2.Add(DecorationReader.readDecorationFromContent("Decorations/city_segment_1"));

                decorator.foliage.Add(DecorationReader.readDecorationFromContent("Decorations/city_segment_2"));
                decorator.foliageBiome2.Add(DecorationReader.readDecorationFromContent("Decorations/city_segment_2"));
                decorator.foliage.Add(DecorationReader.readDecorationFromContent("Decorations/city_segment_3"));
                decorator.foliageBiome2.Add(DecorationReader.readDecorationFromContent("Decorations/city_segment_3"));
                decorator.foliage.Add(DecorationReader.readDecorationFromContent("Decorations/city_segment_4"));
                decorator.foliageBiome2.Add(DecorationReader.readDecorationFromContent("Decorations/city_segment_4"));
                decorator.foliage.Add(DecorationReader.readDecorationFromContent("Decorations/city_segment_5"));
                decorator.foliageBiome2.Add(DecorationReader.readDecorationFromContent("Decorations/city_segment_5"));
                decorator.foliage.Add(DecorationReader.readDecorationFromContent("Decorations/city_segment_6"));
                decorator.foliageBiome2.Add(DecorationReader.readDecorationFromContent("Decorations/city_segment_6"));
                decorator.foliage.Add(DecorationReader.readDecorationFromContent("Decorations/city_segment_7"));
                decorator.foliageBiome2.Add(DecorationReader.readDecorationFromContent("Decorations/city_segment_7"));

                corpseTableRare = new HashSet<Item>();
                corpseTableRare.Add(new Item_Bullet(1));
                corpseTableRare.Add(new Item_Laser_Gun(15));

                corpseTableCommon = new HashSet<Item>();
                corpseTableCommon.Add(new Item_Bullet(1));
                decorator.critters.Add(typeof(prePopulatedCorpseRare));
                decorator.critters.Add(typeof(prePopulatedCorpseCommon));
                decorator.critters.Add(typeof(prePopulatedCorpseCommon));

            }


            
        }

        public class prePopulatedCorpseRare : EntityRemains
        {
            public prePopulatedCorpseRare(Vector2 location, WorldBase world) : base(location, world, corpseTableRare, false)
            {

            }
        }

        public class prePopulatedCorpseCommon : EntityRemains
        {
            public prePopulatedCorpseCommon(Vector2 location, WorldBase world) : base(location, world, corpseTableCommon, false)
            {

            }
        }
    }
}
