using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Survive.WorldManagement.Entities.Particles;
using Survive.WorldManagement.Tile;
using Survive.WorldManagement.Tile.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities.Worm
{
    public class WormBodySegment : Entity
    {
        Entity parent;
        Point drawOffset = new Point(20, 20);

        public WormBodySegment(Vector2 location, WorldBase world, Entity parent) : base(location, world)
        {
            this.parent = parent;
            width = 40;
            height = 40;
            walkSpeed = .5f;
        }

        public override void performPhysics(GameTime time)
        {
            
        }

        public override void damage(float amt, Entity source, Vector2 force)
        {
            parent.damage(amt, source, force);
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);

            Vector2 pullVector = Vector2.Normalize(parent.location - location);
            if (Vector2.Distance(parent.location, location) > width / 2)
            {
                location = parent.location - pullVector * width / 2;
            }

            if (parent.health <= 0 || !world.entities.Contains(parent))
            {
                world.killEntity(this);
            }
        }


        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            //base.draw(batch, time, offset, groundColor);

            /*SpriteEffects effect = SpriteEffects.None;
            if (velocity.X < 0)
            {
                effect = SpriteEffects.FlipHorizontally;
            }*/


            Rectangle defaultRect = getCollisionBox().ToRect();
            batch.Draw(Game1.texture_worm_body, new Rectangle(defaultRect.X + offset.X + drawOffset.X, defaultRect.Y + offset.Y + drawOffset.Y, Game1.texture_worm_body.Width, Game1.texture_worm_body.Height), null, getDrawColor(groundColor, time), 0, new Vector2(Game1.texture_worm_body.Width / 2, Game1.texture_worm_body.Height / 2), SpriteEffects.None, 0);
        }
    }
}
