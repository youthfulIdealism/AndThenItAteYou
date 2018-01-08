using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Inventory
{
    public class CraftingRecepie
    {
        public Item[] components;
        public Item output;
        public int[] costs;
        public float time;

        public CraftingRecepie(Item output, float timeAsPercentageOfDay, Item[] components, int[] costs)
        {
            this.output = output;
            this.components = components;
            this.costs = costs;
            this.time = timeAsPercentageOfDay;
        }

        

        public override int GetHashCode()
        {
            int code = 0;
            foreach (Item item in components)
            {
                code += item.GetType().GetHashCode();
            }
            return code;
        }
    }
}
