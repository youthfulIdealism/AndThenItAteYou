using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Survive.Input;
using Survive.Input.InputManagers;
using Survive.WorldManagement.Entities;
using Survive.WorldManagement.Entities.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Inventory.Items
{
    public class Item_Fire : Item
    {
        public Item_Fire(int uses) : base(uses)
        {
            if(texture == null)
            {
                texture = Game1.texture_item_fire;
            }
        }

        public override Item clone(int uses)
        {
            return new Item_Fire(uses);
        }

        public override int use(PlayerBase user, WorldBase world, Vector2 location, GameTime time, BinaryInputManager inputManager)
        {
            base.use(user, world, location, time, inputManager);

            Point selPoint = world.worldLocToTileLoc(location);
            Vector2 fireLoc  = new Vector2(selPoint.X * Chunk.tileDrawWidth + Chunk.tileDrawWidth / 2, selPoint.Y * Chunk.tileDrawWidth + Chunk.tileDrawWidth / 2);

            EntityFire fire = new EntityFire(fireLoc, world);
            world.addEntity(fire);

            return 1;
        }
    }
}
