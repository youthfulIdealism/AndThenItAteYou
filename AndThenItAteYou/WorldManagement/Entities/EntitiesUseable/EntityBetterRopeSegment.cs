using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Survive.WorldManagement.Inventory;
using Survive.WorldManagement.Inventory.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities.Projectiles
{
    public class EntityBetterRopeSegment : UsableEntity
    {
        public Vector2 startLoc;
        public EntityBetterRopeSegment parent;
        public EntityBetterRopeSegment child;
        public bool isAnchor;

        public EntityBetterRopeSegment(Vector2 location, WorldBase world, EntityBetterRopeSegment parent) : base(location, world)
        {
            this.parent = parent;

            if(rand == null)
            {
                rand = new Random();
            }

            width = 20;
            height = 20;
            startLoc = location;
            this.frictionMultiplier = 2;
            this.gravityMultiplier = .5f;
            blocksWeaponsOnHit = false;
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);

            if(!isAnchor)
            {
                impulse += (parent.location - location) * .025f;
                if (child != null)
                {
                    impulse += (child.location - location) * .025f;
                }
            }else
            {
                velocity = new Vector2();
                impulse = new Vector2();
                location = startLoc;
            }

            if(Vector2.Distance(location, world.player.location) > world.tileGenRadious * Chunk.tilesPerChunk * Chunk.tileDrawWidth * 1.5f)
            {
                world.killEntity(this);
                EntityBetterRopeSegment current = parent;
                while(current != null)
                {
                    world.killEntity(this);
                    current = current.parent;
                }

                current = child;
                while (current != null)
                {
                    world.killEntity(this);
                    current = current.child;
                }
            }
            
        }

        public Entity getParent()
        {
            return parent;
        }

        public override AABB getUseBounds()
        {
            //float useWidth = width + Chunk.tileDrawWidth * 2f;
            //return new AABB((location.X - useWidth / 2), (location.Y - useWidth / 2), (useWidth), (useWidth));
            if(parent != null)
            {
                float x = Math.Min(location.X, parent.location.X);
                float y = Math.Min(location.Y, parent.location.Y);
                float ux = Math.Max(location.X, parent.location.X);
                float uy = Math.Max(location.Y, parent.location.Y);
                return new AABB(x - 5, y - 5, ux - x + 10, uy - y + 10);
            }
            return new AABB(0, 0, 0, 0);
        }

        public override void use(WorldBase world, Vector2 location, Entity user)
        {
            if (user.velocity.Y > 0)
            {
                user.impulse += new Vector2(0, -2f);
            }
            if (user.velocity.Y > -4f || isAnchor)
            {
                user.impulse += new Vector2(0, -.8f);
            }
                
            impulse += new Vector2(0, 1.2f);
        }

        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            Rectangle defaultRect = getCollisionBox().ToRect();
            Rectangle useRect = getUseBounds().ToRect();
            if(parent != null)
            {
                Game1.DrawLine(batch, location + offset.ToVector2(), parent.location + offset.ToVector2(), groundColor);
            }
           
        }

        
    }
}
