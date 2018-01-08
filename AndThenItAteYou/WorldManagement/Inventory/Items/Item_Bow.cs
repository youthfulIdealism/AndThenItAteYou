using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Survive.WorldManagement.Entities;
using Survive.WorldManagement.Entities.Projectiles;
using Microsoft.Xna.Framework.Input;
using Survive.Sound;
using Survive.Input;
using Survive.Input.InputManagers;

namespace Survive.WorldManagement.Inventory.Items
{
    public class Item_Bow : Item
    {

        const string myAlias = "QQ QO\nQQ";
        public Item_Bow(int uses) : base(uses)
        {
            if (texture == null)
            {
                texture = Game1.texture_bow;
            }
        }

        public override Item clone(int uses)
        {
            return new Item_Bow(uses);
        }

        public override int use(PlayerBase user, WorldBase world, Vector2 location, GameTime time, BinaryInputManager inputManager)
        {
            base.use(user, world, location, time, inputManager);

            int used = 0;

            Item_Arrow item_arrow = (Item_Arrow)user.inventory.getItemOfType(new Item_Arrow(1));
            if (item_arrow != null)
            {
                user.inventory.consume(item_arrow, 1);

                EntityArrow arrow = new EntityArrow(user.location + new Vector2(0, -15), world, user);
                arrow.velocity += Vector2.Normalize(location - user.location) * 30;
                world.addEntity(arrow);
                used = 1;

                SoundManager.getSound("bow-throw").playWithVariance(0, .2f, 0, SoundType.MONSTER);
            }else
            {
                user.speechManager.addSpeechBubble(Game1.texture_entity_arrow);
            }



            return used;
        }
    }
}