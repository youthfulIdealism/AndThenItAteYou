using Microsoft.Xna.Framework;
using Survive.WorldManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Inventory.Items
{
    public class Item_Guardian_Fang : Item
    {

        public Item_Guardian_Fang(int uses) : base(uses)
        {
            if(texture == null)
            {
                texture = Game1.texture_item_guardian_fang;
            }
        }

        public override Item clone(int uses)
        {
            return new Item_Guardian_Fang(uses);
        }
    }
}
