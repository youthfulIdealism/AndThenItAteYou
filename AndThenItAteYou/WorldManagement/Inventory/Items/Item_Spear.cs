﻿using Microsoft.Xna.Framework;
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
    public class Item_Spear : Item
    {

        public Item_Spear(int uses) : base(uses)
        {
            if(texture == null)
            {
                texture = Game1.texture_item_spear;
            }
        }

        public override Item clone(int uses)
        {
            return new Item_Spear(uses);
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
                    Entities.Projectiles.EntitySpear spear = new Entities.Projectiles.EntitySpear(user.location + new Vector2(0, -15), world, user);
                    spear.velocity += Vector2.Normalize(location - user.location) * 15;
                    world.addEntity(spear);
                    used++;
                    player.state.decorate(spear);
                    player.state.submitStateAction(STATE_ACTIONS.THROW);
                    SoundManager.getSound("spear-throw").playWithVariance(0, .2f, 0, SoundType.MONSTER);
                }
            }else
            {
                Entities.Projectiles.EntitySpear spear = new Entities.Projectiles.EntitySpear(user.location + new Vector2(0, -15), world, user);
                spear.velocity += Vector2.Normalize(location - user.location) * 15;
                world.addEntity(spear);
                used++;
                SoundManager.getSound("spear-throw").playWithVariance(0, .2f, 0, SoundType.MONSTER);
            }
                    

            
            return used;
        }
    }
}
