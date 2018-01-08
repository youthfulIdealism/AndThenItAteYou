using Microsoft.Xna.Framework;
using Survive.Sound;
using Survive.WorldManagement.Entities;
using Survive.WorldManagement.Entities.Particles;
using Survive.WorldManagement.Inventory;
using Survive.WorldManagement.Inventory.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Tile.Tags.OnUseTags
{
    public class Treasure : TileTag
    {
        static List<Item> treasures;

        static Treasure()
        {
            treasures = new List<Item>();
            treasures.Add(new Item_Sword(75));
            treasures.Add(new Item_Sword(75));
            treasures.Add(new Item_Pickaxe(200));
            treasures.Add(new Item_Snare(13));
            treasures.Add(new Item_Totem_Blank(1));
            treasures.Add(new Item_Axe(7));
            //treasures.Add(new Item_Scizors(200));
            treasures.Add(new Item_Spade(50));
            treasures.Add(new Item_Harpoon(2));
        }




        public override void onUse(WorldBase world, Item harvestTool, Vector2 location, TileType tileType, Entity user)
        {
            base.onUse(world, harvestTool, location, tileType, user);
            Random rand = new Random();
            world.placeTile(TileTypeReferencer.AIR, location);

            Item dropped = treasures[rand.Next(treasures.Count)];

            ItemDropper drop = new ItemDropper();
            drop.registerNewDrop(dropped.clone(dropped.uses), null, dropped.uses, 1);
            drop.drop(world, harvestTool, location);

            for(int i = 0; i < 7; i++)
            {
                world.addEntity(new ParticleTileBreak(location, world, new Vector2(), tileType, 150));
            }
        }
    }
}
