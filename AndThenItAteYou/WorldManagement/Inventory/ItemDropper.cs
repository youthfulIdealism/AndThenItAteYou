using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Inventory
{
    public class ItemDropper
    {
        protected static Random rand;
        protected List<Item> droppedItem;
        protected List<int> dropTries;
        protected List<float> probabilities;
        protected List<Item> tools;
        public ItemDropper()
        {
            if(rand == null)
            {
                rand = new Random();
            }
            droppedItem = new List<Item>();
            dropTries = new List<int>();
            probabilities = new List<float>();
            tools = new List<Item>();
        }

        public void registerNewDrop(Item item, Item harvestTool, int numTries, float probability)
        {
            droppedItem.Add(item);
            dropTries.Add(numTries);
            probabilities.Add(probability);
            tools.Add(harvestTool);
        }

        public void drop(WorldBase world, Item harvestTool, Vector2 location)
        {
            for(int n = 0; n < droppedItem.Count; n++)
            {
                int accum = 0;
                for(int i = 0; i < dropTries[n]; i++)
                {
                    if((tools[n] == null || tools[n].GetType().Equals(harvestTool.GetType())) && rand.NextDouble() < probabilities[n])
                    {
                        accum++;
                    }
                }

                if(accum > 0)
                {
                    ItemDropEntity dropper = new ItemDropEntity(location/* + new Vector2(Chunk.tileDrawWidth / 2, -Chunk.tileDrawWidth / 2)*/, world, droppedItem[n].clone(accum));
                    dropper.velocity += new Vector2(rand.Next(1) - 2, -rand.Next(10));
                    world.addEntity(dropper);
                }
            }
            
        }
    }
}
