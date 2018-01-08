using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Survive.WorldManagement.Inventory;
using Survive.WorldManagement.Inventory.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities
{
    public class EntityRemains : UsableEntity
    {
        bool flashing = false;
        Texture2D standTex;
        HashSet<Item> items;

        public EntityRemains(Vector2 location, WorldBase world, HashSet<Item> items, bool flash) : base(location, world)
        {
            this.width = 50;
            this.height = 50;
            standTex = Game1.texture_entity_remains;
            this.items = items;
            flashing = flash;
            this.blocksWeaponsOnHit = false;
            windMultiplier = 0;
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);
            
        }

        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {

            Rectangle defaultRect = getCollisionBox().ToRect();
            batch.Draw(standTex, new Rectangle(defaultRect.X + offset.X, defaultRect.Y + offset.Y, standTex.Width, standTex.Height), null, groundColor, 0, Vector2.Zero, SpriteEffects.None, 0);

            if(flashing)
            {
                batch.DrawString(Game1.gamefont_24, "E", location + offset.ToVector2() + new Vector2(0, -25 + (float)(Math.Sin(time.TotalGameTime.Milliseconds * .005f)) * 5), Color.White);
            }
            
            //
        }

        public override AABB getUseBounds()
        {
            return getCollisionBox();
        }

        public override void use(WorldBase world, Vector2 location, Entity user)
        {
            world.killEntity(this);
            Random rand = new Random();
            foreach(Item item in items)
            {
                ItemDropEntity drop = new ItemDropEntity(location, world, item);
                drop.velocity += new Vector2(rand.Next(1) - 2, -rand.Next(10));
                world.addEntity(drop);
            }
        }

        public override void damage(float amt, Entity source, Vector2 force)
        {
            impulse += force;
        }
    }
}
