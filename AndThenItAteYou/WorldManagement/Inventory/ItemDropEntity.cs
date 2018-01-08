using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Survive.WorldManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Inventory
{
    public class ItemDropEntity : Entity
    {
        public Item item { get; private set; }
        public Color pickupColor = Color.White;


        public ItemDropEntity(Vector2 location, WorldBase world, Item item) : base(location, world)
        {
            width = 20;
            height = 20;
            this.item = item;
            this.blocksWeaponsOnHit = false;
        }

        public override void damage(float amt, Entity source, Vector2 force)
        {
            impulse += force;
        }

        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            if(item.texture != null)
            {
                Rectangle defaultRect = getCollisionBox().ToRect();
                batch.Draw(item.texture, new Rectangle(defaultRect.X + offset.X, defaultRect.Y + offset.Y, defaultRect.Width, defaultRect.Height), /*groundColor*/Color.Lerp(groundColor, pickupColor, (float)Math.Sin(time.TotalGameTime.Milliseconds * .005f) * .1f));
            }else
            {
                Console.WriteLine("item error: texture not found.");
            }
            
        }
    }
}
