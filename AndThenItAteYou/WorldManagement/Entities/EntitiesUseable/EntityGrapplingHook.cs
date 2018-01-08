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
    public class EntityGrapplingHook : UsableEntity
    {
        Texture2D texture;
        Entity parent;
        Entity child;
        Entity pin;
        bool anchored = false;
        Vector2 pinLocation;

        public EntityGrapplingHook(Vector2 location, WorldBase world, Entity parent, Entity pin) : base(location, world)
        {
            this.parent = parent;
            this.pin = pin;
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

            if(collideBottom || collideLeft || collideRight || collideTop)
            {
                if(!anchored)
                {
                    pinLocation = location;
                    anchored = true;
                    EntityRopeSegment endRopeSegment = (EntityRopeSegment)child;
                    while(endRopeSegment != null && endRopeSegment.child != null)
                    {
                        endRopeSegment = endRopeSegment.child;
                    }

                    if (endRopeSegment != null)
                    {
                        endRopeSegment.isAnchor = true;


                        Player player = (Player)parent;
                        Item_Rope item_rope = (Item_Rope)player.inventory.getItemOfType(new Item_Rope(1));
                        if (item_rope != null)
                        {
                            int theoreticallyConsumedRopes = (int)(Vector2.Distance(location, endRopeSegment.location) / (Chunk.tileDrawWidth * 1.1f));
                            int consumedRopes = Math.Min(item_rope.uses, theoreticallyConsumedRopes); // ensure that the number of ropes consumed does not exceed the number of ropes the player has.
                            player.inventory.consume(item_rope, consumedRopes);
                        }
                        else
                        {
                            Console.WriteLine("grappling hook error: the player does not have rope!");
                        }
                    }


                    /*Player player = (Player)pin;
                   Item_Rope item_rope = (Item_Rope)player.inventory.getItemOfType(new Item_Rope(1));
                   float maxRopeLength = item_rope.uses * Chunk.tileDrawWidth * 1.1f;
                   if (Vector2.Distance(location, hook.location) > maxRopeLength)
                   {
                       this.isAnchor = true;
                   }
                  if (item_rope != null)
                   {
                       player.inventory.consume(item_rope, 1);

                       EntityRopeSegment ropeSegment = new EntityRopeSegment(pin.location, world, this, pin, hook);
                       pin = null;
                       child = ropeSegment;
                       world.addEntity(ropeSegment);
                   }else
                   {
                       this.isAnchor = true;
                   }*/

                }

            }

            if (anchored) { location = pinLocation; velocity = new Vector2(); }
            else if(child != null)
            {
                if (parent is Player && ((Player)parent).inventory.getItemOfType(new Item_Rope(1))  == null)
                {
                    impulse += (child.location - location) * .01f;
                }
                else
                {
                    impulse += (child.location - location) * .001f;
                }
                
            }

            if (pin != null && Vector2.Distance(pin.location, location) > Chunk.tileDrawWidth)
            {
                if (parent is Player && pin != null)
                {
                    EntityRopeSegment ropeSegment = new EntityRopeSegment(location, world, this, pin, this);
                    this.pin = null;
                    this.child = ropeSegment;
                    world.addEntity(ropeSegment);
                }
                else
                {
                    EntityRopeSegment ropeSegment = new EntityRopeSegment(location, world, this, pin, this);
                    this.pin = null;
                    this.child = ropeSegment;
                    world.addEntity(ropeSegment);
                }
            }
        }

        

        public Entity getParent()
        {
            return parent;
        }

        public override AABB getUseBounds()
        {
            float useWidth = width + Chunk.tileDrawWidth * 1.3f;
            return new AABB((location.X - useWidth / 2), (location.Y - useWidth / 2), (useWidth), (useWidth));
        }

        public override void use(WorldBase world, Vector2 location, Entity user)
        {
            user.impulse += new Vector2(0, -.8f);
        }

        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            SpriteEffects effect = SpriteEffects.None;
            if (velocity.X > 0)
            {
                effect = SpriteEffects.FlipHorizontally;
            }

            Rectangle defaultRect = getCollisionBox().ToRect();
            batch.Draw(Game1.texture_entity_grappling_hook, new Rectangle(defaultRect.X + offset.X, defaultRect.Y + offset.Y, (int)20, (int)20), null, groundColor, (float)Math.Atan(velocity.Y / velocity.X), Vector2.Zero, effect, 0);
        }
    }
}
