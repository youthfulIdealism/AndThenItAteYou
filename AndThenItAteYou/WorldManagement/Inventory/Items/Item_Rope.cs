using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Survive.WorldManagement.Entities;
using Survive.WorldManagement.Tile.Tags;
using Survive.WorldManagement.Entities.Speech;
using Survive.Input.InputManagers;

namespace Survive.WorldManagement.Inventory.Items
{
    public class Item_Rope : Item
    {

        public Item_Rope(int uses) : base(uses)
        {
            if (texture == null)
            {
                texture = Game1.texture_item_rope;
            }
        }

        public override Item clone(int uses)
        {
            return new Item_Rope(uses);
        }

        public override int use(PlayerBase user, WorldBase world, Vector2 location, GameTime time, BinaryInputManager inputManager)
        {
            (((Player)user).speechManager).addSpeechBubble(new SpeechBubble(Game1.texture_item_grappling_hook));
            return base.use(user, world, location, time, inputManager);
        }
    }
}
