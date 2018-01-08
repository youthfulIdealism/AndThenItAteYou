using Survive.WorldManagement.Inventory;
using Survive.WorldManagement.Inventory.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement
{
    public class UniverseProperties
    {
        
        public static String seed;
        public static List<Item_Berry> availableBerries;
        public static List<int[]> berryTags;
        public static List<Item> queuedRewardItems;

        public UniverseProperties(String seed)
        {
            UniverseProperties.seed = seed;
            Random universeRandom = new Random(seed.GetHashCode());


            berryTags = new List<int[]>();
            berryTags.Add(Item_Berry.getRandomTags(universeRandom));
            berryTags.Add(Item_Berry.getRandomTags(universeRandom));
            berryTags.Add(Item_Berry.getRandomTags(universeRandom));
            berryTags.Add(Item_Berry.getRandomTags(universeRandom));

            availableBerries = new List<Item_Berry>();
            availableBerries.Add(new Item_Berry_Type_0(1));
            availableBerries.Add(new Item_Berry_Type_1(1));
            availableBerries.Add(new Item_Berry_Type_2(1));
            availableBerries.Add(new Item_Berry_Type_3(1));

            List<Item> rewardPrototype = new List<Item>();
            rewardPrototype.Add(new Item_Sword(1));
            rewardPrototype.Add(new Item_Scizors(1));
            rewardPrototype.Add(new Item_Axe(1));
            rewardPrototype.Add(new Item_Spade(1));

            queuedRewardItems = new List<Item>();
            while (rewardPrototype.Count > 0)
            {
                int rewardIndex = universeRandom.Next(rewardPrototype.Count);
                queuedRewardItems.Add(rewardPrototype[rewardIndex]);
                rewardPrototype.RemoveAt(rewardIndex);
            }

        }
    }
}
