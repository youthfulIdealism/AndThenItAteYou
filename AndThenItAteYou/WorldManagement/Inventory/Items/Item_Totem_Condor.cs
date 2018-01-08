using Microsoft.Xna.Framework;
using Survive.WorldManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Survive.WorldManagement.Entities.TransformedPlayers;
using Survive.WorldManagement.Tile.Tags;
using Survive.Input;
using Survive.Input.InputManagers;

namespace Survive.WorldManagement.Inventory.Items
{
    public class Item_Totem_Condor : Item_Totem_Blank
    {

        public Item_Totem_Condor(int uses) : base(uses)
        {
            texture = Game1.texture_item_totem_condor;
        }

        public override Item clone(int uses)
        {
            return new Item_Totem_Condor(uses);
        }

        public override int use(PlayerBase user, WorldBase world, Vector2 location, GameTime time, BinaryInputManager inputManager)
        {
            PlayerCondor condor = new PlayerCondor(user.location, world, (Player)user);
            world.transformPlayer(condor);
            return base.use(user, world, location, time, inputManager);
        }
    }
}
