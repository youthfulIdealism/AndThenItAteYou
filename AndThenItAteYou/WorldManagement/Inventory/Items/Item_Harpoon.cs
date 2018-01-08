using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Survive.Input;
using Survive.Input.InputManagers;
using Survive.Sound;
using Survive.WorldManagement.Entities;
using Survive.WorldManagement.Entities.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Inventory.Items
{
    public class Item_Harpoon : Item
    {
        public Item_Harpoon(int uses) : base(uses)
        {
            if(texture == null)
            {
                texture = Game1.texture_item_harpoon;
            }
        }

        public override Item clone(int uses)
        {
            return new Item_Harpoon(uses);
        }

        public override int use(PlayerBase user, WorldBase world, Vector2 location, GameTime time, BinaryInputManager inputManager)
        {
            base.use(user, world, location, time, inputManager);

            int used = 0;

            if (user is Player)
            {
                Player player = (Player)user;
                if (player.state.actionPermitted(STATE_ACTIONS.THROW))
                {
                    EntityHarpoon harpoon = new EntityHarpoon(user.location + new Vector2(0, -15), world, user);
                    harpoon.velocity += Vector2.Normalize(location - user.location) * 17;
                    world.addEntity(harpoon);
                    used++;
                    player.state.decorate(harpoon);
                    player.state.submitStateAction(STATE_ACTIONS.THROW);
                }
            }
            else
            {
                EntityHarpoon harpoon = new EntityHarpoon(user.location + new Vector2(0, -15), world, user);
                harpoon.velocity += Vector2.Normalize(location - user.location) * 17;
                world.addEntity(harpoon);
                used++;
            }

            SoundManager.getSound("spear-throw").playWithVariance(-.2f, .2f, 0, SoundType.MONSTER);
            return used;
        }
    }
}
