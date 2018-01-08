using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Survive.Input;
using Survive.Input.InputManagers;
using Survive.WorldManagement.Entities;
using Survive.WorldManagement.Entities.TransformedPlayers;
using Survive.WorldManagement.Tile.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Inventory.Items
{
    public class Item_Totem_Blank : Item
    {

        public Item_Totem_Blank(int uses) : base(uses)
        {
            if(texture == null)
            {
                texture = Game1.texture_item_totem_blank;
            }
        }

        public override Item clone(int uses)
        {
            return new Item_Totem_Blank(uses);
        }

        public override int use(PlayerBase user, WorldBase world, Vector2 location, GameTime time, BinaryInputManager inputManager)
        {

            if (user is Player)
            {
                if(world.getBlock(user.location) != null)
                {
                    if (world.getBlock(user.location).tags.Contains(TagReferencer.TotemRabbit) && user.inventory.getItemOfType(new Item_Totem_Rabbit(1)) == null)
                    {
                        user.inventory.add(new Item_Totem_Rabbit(1));
                        replaceKeyedItem(user, new Item_Totem_Rabbit(1));
                        return 1;
                    }
                    else if (world.getBlock(user.location).tags.Contains(TagReferencer.TotemTaipir) && user.inventory.getItemOfType(new Item_Totem_Tapir(1)) == null)
                    {
                        user.inventory.add(new Item_Totem_Tapir(1));
                        replaceKeyedItem(user, new Item_Totem_Tapir(1));
                        return 1;
                    }
                    else if (world.getBlock(user.location).tags.Contains(TagReferencer.TotemCrocodile) && user.inventory.getItemOfType(new Item_Totem_Crocodile(1)) == null)
                    {
                        user.inventory.add(new Item_Totem_Crocodile(1));
                        replaceKeyedItem(user, new Item_Totem_Crocodile(1));
                        return 1;
                    }
                    else if (world.getBlock(user.location).tags.Contains(TagReferencer.TotemCondor) && user.inventory.getItemOfType(new Item_Totem_Condor(1)) == null)
                    {
                        user.inventory.add(new Item_Totem_Condor(1));
                        replaceKeyedItem(user, new Item_Totem_Condor(1));
                        return 1;
                    }
                    else if (world.getBlock(user.location).tags.Contains(TagReferencer.TotemFalcon) && user.inventory.getItemOfType(new Item_Totem_Falcon(1)) == null)
                    {
                        user.inventory.add(new Item_Totem_Falcon(1));
                        replaceKeyedItem(user, new Item_Totem_Falcon(1));
                        return 1;
                    }
                    else
                    {

                        return 0;
                    }
                }
                
            }else if(user is TransformedPlayer)
            {
                world.transformPlayer(((TransformedPlayer)user).transformedFrom);
                return 0;
            }

            return 0;
            
        }

        private void replaceKeyedItem(PlayerBase user, Item item)
        {
            for(int i = 0; i < user.keyedItems.Length; i++)
            {
                if(user.keyedItems[i] != null && user.keyedItems[i] is Item_Totem_Blank)
                {
                    user.keyedItems[i] = item.clone(1);
                    break;
                }
            }

        }
    }
}
