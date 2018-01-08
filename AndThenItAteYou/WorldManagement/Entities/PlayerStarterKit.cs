using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Survive.Sound;
using Survive.SplashScreens;
using Survive.WorldManagement.Entities.Progression;
using Survive.WorldManagement.Inventory;
using Survive.WorldManagement.Inventory.Items;
using Survive.WorldManagement.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities
{
    public class PlayerStarterKit
    {
        public Item[] kit;
        public Card[] starterCards;
        public Rectangle buttonRect;
        public PlayerAnimationPackage animations;
        public bool locked = true;
        public Texture2D unlockTex;

        public String tutorialStringMovement;
        public String tutorialStringInventory;
        public String tutorialStringUseItems;
        public String tutorialStringPostKeybinds;
        public String tutorialStringCrafting;
        public String tutorialStringWeapon;
        public String rebindAndFinish;


        public int id;

        public PlayerStarterKit(PlayerAnimationPackage animations, Item[] kit, Card[] starterCards, int id, Texture2D unlockTex)
        {
            this.animations = animations;
            this.kit = kit;
            this.starterCards = starterCards;
            this.id = id;
            this.unlockTex = unlockTex;
        }

        public void selectKit(WorldBase world)
        {
            if (!locked)
            {
                SoundManager.getSound("click").playWithVariance(0, .25f, 0, SoundType.MONSTER);

                foreach (Item item in kit)
                {
                    world.player.inventory.add(item.clone(item.uses));
                }

                if(starterCards[0] != null)
                {
                    ((Player)(world.player)).cards[0] = CardRegistrar.getCardFromIdPlayerAndLevel(CardRegistrar.getIDFromCard(starterCards[0]), starterCards[0].level, (Player)world.player);
                }

                if (starterCards[1] != null)
                {
                    ((Player)(world.player)).cards[1] = CardRegistrar.getCardFromIdPlayerAndLevel(CardRegistrar.getIDFromCard(starterCards[1]), starterCards[1].level, (Player)world.player);
                }
                
                


                animations.apply(((Player)(world.player)));
                
                world.player.tutorialStringMovement = tutorialStringMovement;
                world.player.tutorialStringInventory = tutorialStringInventory;
                world.player.tutorialStringUseItems = tutorialStringUseItems;
                world.player.tutorialStringPostKeybinds = tutorialStringPostKeybinds;
                world.player.tutorialStringCrafting = tutorialStringCrafting;
                world.player.tutorialStringWeapon = tutorialStringWeapon;
                world.player.rebindAndFinish = rebindAndFinish;

                if(world is TutorialWorld)
                {
                    ((TutorialWorld)world).processInstructionStrings();
                }
            }

        }
    }
}
