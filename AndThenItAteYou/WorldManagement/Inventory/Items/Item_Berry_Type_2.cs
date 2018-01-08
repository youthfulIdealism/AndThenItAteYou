﻿using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Inventory.Items
{
    public class Item_Berry_Type_2 : Item_Berry
    {

        public Item_Berry_Type_2(int uses) : base(uses, UniverseProperties.berryTags[2])
        {
            if (texture == null)
            {
                texture = Game1.texture_berries[2];
            }
            this.tags = UniverseProperties.berryTags[2];

            if (tags.Length > 2)
            {
                Console.WriteLine("incorrect berry tags. Should be 2.");
            }
        }

        public override Item clone(int uses)
        {
            return new Item_Berry_Type_2(uses);
        }

    }
}
