using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Inventory.Items
{
    public static class ItemRegistrar
    {
        private static int registerNumber;
        private static Dictionary<int, Type> itemDictionary;
        private static Dictionary<Type, int> reverseItemDictionary;

        static ItemRegistrar()
        {
            registerNumber = 0;
            itemDictionary = new Dictionary<int, Type>();
            reverseItemDictionary = new Dictionary<Type, int>();

            ItemRegistrar.registerItemType(typeof(Item_Arrow));
            ItemRegistrar.registerItemType(typeof(Item_Axe));
            ItemRegistrar.registerItemType(typeof(Item_Berry));
            ItemRegistrar.registerItemType(typeof(Item_Berry_Type_0));
            ItemRegistrar.registerItemType(typeof(Item_Berry_Type_1));
            ItemRegistrar.registerItemType(typeof(Item_Berry_Type_2));
            ItemRegistrar.registerItemType(typeof(Item_Berry_Type_3));
            ItemRegistrar.registerItemType(typeof(Item_Bow));
            ItemRegistrar.registerItemType(typeof(Item_Bullet));
            ItemRegistrar.registerItemType(typeof(Item_Childs_Drawing));
            ItemRegistrar.registerItemType(typeof(Item_Feather));
            ItemRegistrar.registerItemType(typeof(Item_Fire));
            ItemRegistrar.registerItemType(typeof(Item_Grass));
            ItemRegistrar.registerItemType(typeof(Item_Guardian_Fang));
            ItemRegistrar.registerItemType(typeof(Item_Knife));
            ItemRegistrar.registerItemType(typeof(Item_Laser_Gun));
            ItemRegistrar.registerItemType(typeof(Item_Meat));
            ItemRegistrar.registerItemType(typeof(Item_Mushroom));
            ItemRegistrar.registerItemType(typeof(Item_Rope));
            ItemRegistrar.registerItemType(typeof(Item_Scizors));
            ItemRegistrar.registerItemType(typeof(Item_Spear));
            ItemRegistrar.registerItemType(typeof(Item_Spear_Fanged));
            ItemRegistrar.registerItemType(typeof(Item_Stick));
            ItemRegistrar.registerItemType(typeof(Item_Stone));
            ItemRegistrar.registerItemType(typeof(Item_Sword));
            ItemRegistrar.registerItemType(typeof(Item_Bottle_Type_0));
            ItemRegistrar.registerItemType(typeof(Item_Bottle_Type_1));
            ItemRegistrar.registerItemType(typeof(Item_Bottle_Type_2));
            ItemRegistrar.registerItemType(typeof(Item_Bottle_Type_3));
            ItemRegistrar.registerItemType(typeof(Item_Hook_Better));
            ItemRegistrar.registerItemType(typeof(Item_Totem_Blank));
            ItemRegistrar.registerItemType(typeof(Item_Totem_Rabbit));
            ItemRegistrar.registerItemType(typeof(Item_Totem_Tapir));
            ItemRegistrar.registerItemType(typeof(Item_Totem_Condor));
            ItemRegistrar.registerItemType(typeof(Item_Totem_Falcon));
            ItemRegistrar.registerItemType(typeof(Item_Charmstone));
            ItemRegistrar.registerItemType(typeof(Item_Spade));
            ItemRegistrar.registerItemType(typeof(Item_Spud));
            ItemRegistrar.registerItemType(typeof(Item_Ladder));
            ItemRegistrar.registerItemType(typeof(Item_Snare));
            ItemRegistrar.registerItemType(typeof(Item_Pickaxe));
            ItemRegistrar.registerItemType(typeof(Item_Bite));
            ItemRegistrar.registerItemType(typeof(Item_Macuhatil));
            ItemRegistrar.registerItemType(typeof(Item_Seed));
        }

        public static void registerItemType(Type type)
        {
            registerNumber++;
            itemDictionary.Add(registerNumber, type);
            reverseItemDictionary.Add(type, registerNumber);
        }

        public static Type getTypeFromID(int id)
        {
            return itemDictionary[id];
        }

        public static int getIDFromType(Type type)
        {
            return reverseItemDictionary[type];
        }

        public static int getIDFromItem(Item item)
        {
            return reverseItemDictionary[item.GetType()];
        }

        public static Item getItemFromIdAndCount(int id, int count)
        {
            return (Item)Activator.CreateInstance(getTypeFromID(id), new Object[] { count });
        }
    }
}
