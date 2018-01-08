using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Inventory.Items
{
    public class Item_Stick : Item
    {

        public Item_Stick(int uses) : base(uses)
        {
            if (texture == null)
            {
                texture = Game1.texture_stick;
            }
        }

        public override Item clone(int uses)
        {
            return new Item_Stick(uses);
        }
    }
}