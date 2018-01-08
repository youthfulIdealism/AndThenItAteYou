using Microsoft.Xna.Framework;
using Survive.Input.InputManagers;
using Survive.Sound;
using Survive.WorldManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Inventory.Items
{
    public class Item_Stone : Item
    {

        public Item_Stone(int uses) : base(uses)
        {
            if(texture == null)
            {
                texture = Game1.texture_item_stone;
            }
        }

        public override Item clone(int uses)
        {
            return new Item_Stone(uses);
        }

        public override int use(PlayerBase user, WorldBase world, Vector2 location, GameTime time, BinaryInputManager inputManager)
        {
            base.use(user, world, location, time, inputManager);

            Random rand = new Random();
            int used = 0;
            if (user is Player)
            {
                Player player = (Player)user;
                if (player.state.actionPermitted(STATE_ACTIONS.THROW))
                {
                    Entities.Projectiles.EntityRock rock = new Entities.Projectiles.EntityRock(user.location + new Vector2(0, -15), world, user);
                    rock.velocity += Vector2.Normalize(location - user.location) * (30 - rand.Next(7));
                    world.addEntity(rock);
                    used++;
                    player.state.decorate(rock);
                    player.state.submitStateAction(STATE_ACTIONS.THROW);
                    SoundManager.getSound("spear-throw").playWithVariance(0, .2f, 0, SoundType.MONSTER);
                }
            }
            else
            {
                Entities.Projectiles.EntityRock rock = new Entities.Projectiles.EntityRock(user.location + new Vector2(0, -15), world, user);
                rock.velocity += Vector2.Normalize(location - user.location) * (30 - rand.Next(7));
                world.addEntity(rock);
                used++;
                SoundManager.getSound("spear-throw").playWithVariance(0, .2f, 0, SoundType.MONSTER);
            }


            
            return used;
        }
    }
}
