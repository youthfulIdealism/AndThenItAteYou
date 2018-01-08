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
    public class Item_Sword : Item
    {

        public Item_Sword(int uses) : base(uses)
        {
            if(texture == null)
            {
                texture = Game1.texture_item_sword;
            }
        }

        public override Item clone(int uses)
        {
            return new Item_Sword(uses);
        }

        public override int use(PlayerBase user, WorldBase world, Vector2 location, GameTime time, BinaryInputManager inputManager)
        {
            base.use(user, world, location, time, inputManager);

            int used = 0;
            if(user is Player)
            {
                Player player = (Player)user;
                if (player.state.actionPermitted(STATE_ACTIONS.SWING))
                {
                    int dir = -1;
                    if (player.facing > 0) { dir = 1; }
                    EntitySwordSlash swordSlash = new EntitySwordSlash(user.location + new Vector2(dir * 35, 20), world, user);
                    swordSlash.velocity += Vector2.Normalize(location - user.location) * .1f;
                    world.addEntity(swordSlash);
                    used++;
                    player.state.decorate(swordSlash);
                    player.state.submitStateAction(STATE_ACTIONS.SWING);
                    SoundManager.getSound("sword-slash").playWithVariance(0, .2f, 0, SoundType.MONSTER);
                }
            }else
            {
                EntitySwordSlash swordSlash = new EntitySwordSlash(user.location + new Vector2(0, 20), world, user);
                swordSlash.velocity += Vector2.Normalize(location - user.location) * .1f;
                world.addEntity(swordSlash);
                used++;
                SoundManager.getSound("sword-slash").playWithVariance(0, .2f, 0, SoundType.MONSTER);
            }

            /*EntitySwordSlash swordSlash = new EntitySwordSlash(user.location + new Vector2(0, 20), world, user);
            swordSlash.velocity += Vector2.Normalize(location - user.location) * .1f;
            world.addEntity(swordSlash);*/

            

            return used;
        }
    }
}
