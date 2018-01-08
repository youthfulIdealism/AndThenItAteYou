using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Survive.Sound;
using Survive.WorldManagement.Entities.Particles;
using Survive.WorldManagement.Inventory;
using Survive.WorldManagement.Inventory.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities
{
    public class EntitySnare : UsableEntity, Weapon
    {

        Texture2D tex;
        Point drawOffset = new Point(0, 0);
        Entity parent;

        public EntitySnare(Vector2 location, WorldBase world, Entity parent) : base(location, world)
        {
            this.width = 30;
            this.height = 60;
            this.blocksWeaponsOnHit = false;
            tex = Game1.texture_entity_snare;
            this.parent = parent;
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);
            foreach (Entity entity in world.entities)
            {
                if (!(entity is Weapon) && !entity.Equals(parent) && !entity.Equals(this) && entity.blocksWeaponsOnHit && getCollisionBox().Intersects(entity.getCollisionBox()))
                {
                    entity.addStatusEffect(new StatusEffect(StatusEffect.status.SLOW, .06f, 500, false));
                    entity.damage(getDamage(), this, new Vector2());
                    SoundManager.getSound("grappling-hook").playWithVariance(0, .2f, 0, SoundType.MONSTER);
                    world.killEntity(this);
                }
            }

           
        }


        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            //base.draw(batch, time, offset);
            Rectangle defaultRect = getCollisionBox().ToRect();
            batch.Draw(tex, new Rectangle(defaultRect.X + offset.X + drawOffset.X, defaultRect.Y + offset.Y + drawOffset.Y, tex.Width, tex.Height), getDrawColor(groundColor, time));
        }

        public float getDamage()
        {
            return 30;
        }

        public Entity getParent()
        {
            return parent;
        }

        public override AABB getUseBounds()
        {
            return getCollisionBox();
        }

        public override void use(WorldBase world, Vector2 location, Entity user)
        {
            world.killEntity(this);
            world.addEntity(new ItemDropEntity(location, world, new Item_Snare(1)));
        }
    }
}
