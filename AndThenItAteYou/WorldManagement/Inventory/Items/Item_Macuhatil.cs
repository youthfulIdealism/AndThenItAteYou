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
    public class Item_Macuhatil : Item
    {

        public Item_Macuhatil(int uses) : base(uses)
        {
            if(texture == null)
            {
                texture = Game1.texture_item_macuhatil;
            }
        }

        public override Item clone(int uses)
        {
            return new Item_Macuhatil(uses);
        }

        public override int use(PlayerBase user, WorldBase world, Vector2 location, GameTime time, BinaryInputManager inputManager)
        {
            base.use(user, world, location, time, inputManager);

            /*EntityMacuhatilSlash macuhatilSlash = new EntityMacuhatilSlash(user.location + new Vector2(0, 20), world, user);
            macuhatilSlash.velocity += Vector2.Normalize(location - user.location) * .1f;
            world.addEntity(macuhatilSlash);*/
            if (user is Player)
            {
                Player player = (Player)user;
                if (player.state.actionPermitted(STATE_ACTIONS.SWING))
                {
                    int dir = -1;
                    if (player.facing > 0) { dir = 1; }
                    EntityMacuhatilSlash macuhatilSlash = new EntityMacuhatilSlash(user.location + new Vector2(dir * 35, 20), world, user);
                    macuhatilSlash.velocity += Vector2.Normalize(location - user.location) * .1f;
                    player.state.decorate(macuhatilSlash);
                    world.addEntity(macuhatilSlash);
                    player.state.submitStateAction(STATE_ACTIONS.SWING);
                    SoundManager.getSound("sword-slash").playWithVariance(-.5f, .2f, 0, SoundType.MONSTER);
                }
            }
            else
            {
                EntityMacuhatilSlash macuhatilSlash = new EntityMacuhatilSlash(user.location + new Vector2(0, 20), world, user);
                macuhatilSlash.velocity += Vector2.Normalize(location - user.location) * .1f;
                world.addEntity(macuhatilSlash);
                SoundManager.getSound("sword-slash").playWithVariance(-.5f, .2f, 0, SoundType.MONSTER);
            }

            

            return 0;
        }
    }
}
