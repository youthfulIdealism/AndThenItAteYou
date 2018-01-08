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
    public class EntityRopeSegment : UsableEntity
    {
        Texture2D texture;
        Entity parent;
        public EntityRopeSegment child;
        public EntityGrapplingHook hook;
        Entity pin;
        public bool isAnchor;

        public EntityRopeSegment(Vector2 location, WorldBase world, Entity parent, Entity pin, EntityGrapplingHook hook) : base(location, world)
        {
            this.parent = parent;
            this.pin = pin;
            this.hook = hook;
            this.blocksWeaponsOnHit = false;

            if (rand == null)
            {
                rand = new Random();
            }

            width = 20;
            height = 20;
            texture = Game1.texture_item_grappling_hook;
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);

            if(!isAnchor)
            {
                if (pin != null && Vector2.Distance(location, pin.location) > Chunk.tileDrawWidth * .3f)
                {
                    if(pin is Player)
                    {
                        Player player = (Player)pin;
                        Item_Rope item_rope = (Item_Rope)player.inventory.getItemOfType(new Item_Rope(1));
                        if (item_rope != null)
                        {
                            float maxRopeLength = item_rope.uses * Chunk.tileDrawWidth * 1.1f;
                            if (Vector2.Distance(location, hook.location) > maxRopeLength)
                            {
                                this.isAnchor = true;
                            }
                            else
                            {
                                EntityRopeSegment ropeSegment = new EntityRopeSegment(pin.location, world, this, pin, hook);
                                pin = null;
                                child = ropeSegment;
                                world.addEntity(ropeSegment);
                            }
                        }
                    }
                    else
                    {
                        EntityRopeSegment ropeSegment = new EntityRopeSegment(pin.location, world, this, pin, hook);
                        pin = null;
                        child = ropeSegment;
                        world.addEntity(ropeSegment);
                    }
                    
                }

                impulse += (parent.location - location) * .02f;
                if (child != null)
                {
                    impulse += (child.location - location) * .02f;
                }
            }else
            {
                velocity = new Vector2();
            }
            
        }

        public Entity getParent()
        {
            return parent;
        }

        public override AABB getUseBounds()
        {
            float useWidth = width + Chunk.tileDrawWidth * 2f;
            return new AABB((location.X - useWidth / 2), (location.Y - useWidth / 2), (useWidth), (useWidth));
        }

        public override void use(WorldBase world, Vector2 location, Entity user)
        {
            user.impulse += new Vector2(0, -.8f);
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
