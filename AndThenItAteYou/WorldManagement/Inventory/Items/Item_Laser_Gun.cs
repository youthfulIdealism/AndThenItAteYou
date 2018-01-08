using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Survive.WorldManagement.Entities;
using Survive.WorldManagement.Entities.Projectiles;
using Microsoft.Xna.Framework.Input;
using Survive.Input;
using Survive.Input.InputManagers;
using Survive.Sound;

namespace Survive.WorldManagement.Inventory.Items
{
    public class Item_Laser_Gun : Item
    {

        public Item_Laser_Gun(int uses) : base(uses)
        {
            if (texture == null)
            {
                texture = Game1.texture_item_laser_gun;
            }
        }

        public override Item clone(int uses)
        {
            return new Item_Laser_Gun(uses);
        }

        public override int use(PlayerBase user, WorldBase world, Vector2 location, GameTime time, BinaryInputManager inputManager)
        {
            base.use(user, world, location, time, inputManager);

            int used = 0;

            Item_Bullet item_bullet = (Item_Bullet)user.inventory.getItemOfType(new Item_Bullet(1));
            if (item_bullet != null)
            {
                user.inventory.consume(item_bullet, 1);

                EntityLaserBolt laserBolt = new EntityLaserBolt(user.location + new Vector2(0, -15), world, user);
                laserBolt.velocity += Vector2.Normalize(location - user.location) * 30;
                world.addEntity(laserBolt);
                used = 1;

                SoundManager.getSound("gun-fire").playWithVariance(0, 1, 0, SoundType.MONSTER);
            }


            return used;
        }
    }
}