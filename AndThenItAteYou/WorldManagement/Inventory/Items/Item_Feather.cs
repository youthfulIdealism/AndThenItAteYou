﻿using Microsoft.Xna.Framework;
using Survive.WorldManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Inventory.Items
{
    public class Item_Feather : Item
    {
        public Item_Feather(int uses) : base(uses)
        {
            if(texture == null)
            {
                texture = Game1.texture_item_feather;
            }
        }

        public override Item clone(int uses)
        {
            return new Item_Feather(uses);
        }
    }
}
