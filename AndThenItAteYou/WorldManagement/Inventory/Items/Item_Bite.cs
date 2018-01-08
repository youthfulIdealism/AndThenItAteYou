using Microsoft.Xna.Framework;
using Survive.Input.InputManagers;
using Survive.Sound;
using Survive.WorldManagement.Entities;
using Survive.WorldManagement.Entities.TransformedPlayers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Inventory.Items
{
    public class Item_Bite : Item
    {

        public Item_Bite(int uses) : base(uses)
        {
            if(texture == null)
            {
                texture = Game1.texture_item_bite;
            }
        }

        public override Item clone(int uses)
        {
            return new Item_Bite(uses);
        }

        public override int use(PlayerBase user, WorldBase world, Vector2 location, GameTime time, BinaryInputManager inputManager)
        {
            base.use(user, world, location, time, inputManager);

            if(user is PlayerCrocodile)
            {
                ((PlayerCrocodile)user).attacking = true;
            }else
            {
                Logger.log("Tried to use crocodile bite item as a non-crocodile. User: " + user);
            }
            return 0;
        }
    }
}
