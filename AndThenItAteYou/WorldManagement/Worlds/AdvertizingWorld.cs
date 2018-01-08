using Microsoft.Xna.Framework;
using Survive.Input.Data;
using Survive.WorldManagement.Entities;
using Survive.WorldManagement.Entities.Particles;
using Survive.WorldManagement.Inventory;
using Survive.WorldManagement.Inventory.Items;
using Survive.WorldManagement.Tile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Worlds
{
    public class AdvertizingWorld : WorldFromDisk
    {

        public AdvertizingWorld() : base("Content\\Worlds\\advertisingWorld", 1)
        {
            spawnsEnemies = false;

            timeOfDay = 1f;
            timeIncrement = 0;

            this.decorator.colorManager.groundColor = new Color(0, 17, 38);
            this.decorator.colorManager.skyColorDay = new Color(212, 187, 159);
            //this.decorator.colorManager.groundColor = new Color
        }
    }
}
