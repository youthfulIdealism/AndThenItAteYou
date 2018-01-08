using Survive.WorldManagement.Entities.Progression;
using Survive.WorldManagement.Inventory;
using Survive.WorldManagement.Inventory.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities
{
    public static class PlayerKitRegistry
    {
        static PlayerKitRegistry()
        {
            registry = new Dictionary<int, PlayerStarterKit>();
            registry.Add(0, new PlayerStarterKit(
               Game1.player_default_animations,
               new Item[] { new Item_Rope(6), new Item_Mushroom(12), new Item_Hook_Better(1), new Item_Totem_Blank(1)},
               //new Card[] { new CardBlink(3), null},
               new Card[2],
               0,
               null));
            registry[0].tutorialStringMovement = "Move left and right with <Left> and <Right>.\nPress <Up> to jump.";
            registry[0].tutorialStringInventory = "Press <Inventory_Open> to open your inventory.\nDrag and drop items to put them in the slots at the bottom of the screen.\nDrop your grappling hook into the first slot.";
            registry[0].tutorialStringUseItems = "Press and hold <Inventory_0> to use the item in the first slot.\nUse the grappling hook to attach \na rope to the top of the cliff.\nClimb the rope with <Use>.";
            registry[0].tutorialStringPostKeybinds = "Harvest some rocks.";
            registry[0].tutorialStringCrafting = "Open your inventory with <Inventory_Open>.\nOn the left there is a list of recipies you can make.\nIf the recepie is red, you don't have an ingredient.\nTo make a spear, you need a stick.\nTo climb a tree and search the truck for sticks,\nstand over the trunk and press <Use>.";
            registry[0].tutorialStringWeapon = "Once you have a stick, open your inventory again.\nClick on the recipie to make a spear.\nDrop the spear in a slot and practice with it.\nNow you can hunt!\nYou can make rope for your grappling hook using grass.";
            registry[0].rebindAndFinish = "Progress through the game by using teleporters (<Use>).\nIf you don't like the keybinds, press escape.\nYou can change them by pressing the button\nthat looks like a gear.\nEach unlockable character has its own tutorial.";

            registry.Add(1, (new PlayerStarterKit(
                Game1.player_hunter_animations,
                new Item[] { new Item_Rope(3), new Item_Bow(100), new Item_Arrow(6), new Item_Snare(6), new Item_Hook_Better(1), new Item_Totem_Blank(1) },
                new Card[2],
                1,
                Game1.unlock_huntress)));
            registry[1].tutorialStringMovement = "This character starts with a bow, arrows, and snares.";
            registry[1].tutorialStringInventory = "Press <Inventory_Open> to open your inventory.\nDrag and drop items to put them in the slots at the bottom of the screen.\nDrop your grappling hook into the first slot.";
            registry[1].tutorialStringUseItems = "Use the grappling hook to climb to the top of the cliff.";
            registry[1].tutorialStringPostKeybinds = "";
            registry[1].tutorialStringCrafting = "Open your inventory with <Inventory_Open>.\nYou can place snares by using them.\nA creature who touches a snare will be slowed.";
            registry[1].tutorialStringWeapon = "Placing a snare and chasing\n an animal into it can give you an easy shot.";
            registry[1].rebindAndFinish = "Progress through the game by using teleporters (<Use>).\nIf you don't like the keybinds, press escape.\nYou can change them by pressing the button\nthat looks like a gear.";

            registry.Add(2, (new PlayerStarterKit(
                Game1.player_shaman_animations,
                new Item[] { new Item_Totem_Blank(1), new Item_Totem_Falcon(1), new Item_Totem_Rabbit(1), new Item_Totem_Tapir(1), new Item_Mushroom(20)},
                new Card[2],
                2,
                Game1.unlock_shaman)));

            registry[2].tutorialStringMovement = "This character starts with a variety of totems.";
            registry[2].tutorialStringInventory = "Press <Inventory_Open> to open your inventory.\nDrag and drop items to put them in the slots at the bottom of the screen.\nDrop your bird totem into the first slot.";
            registry[2].tutorialStringUseItems = "Press <Inventory_0> to use the item in the first slot.\nTurn into a bird and fly to the top of the cliff.";
            registry[2].tutorialStringPostKeybinds = "Use the human totem to return to human form.";
            registry[2].tutorialStringCrafting = "Using totems costs food.\nTo restore food, use the tapir totem\nand eat shrubs with <Use>.";
            registry[2].tutorialStringWeapon = "The rabbit totem turns you into a rabbit.\nRabbits are small, quick, and hard for\nmonsters to detect. Turning\ninto a rabbit makes you sneaky!";
            registry[2].rebindAndFinish = "Progress through the game by using teleporters (<Use>).\nIf you don't like the keybinds, press escape.\nYou can change them by pressing the button\nthat looks like a gear.";

            registry.Add(3, (new PlayerStarterKit(
                Game1.player_girl_animations,
                new Item[] { new Item_Hook_Better(1) },
                new Card[2],
                2,
                Game1.unlock_girl)));
            registry[3].tutorialStringMovement = "This character is extremely difficult!";
            registry[3].tutorialStringInventory = "You start with minimal resources, and have to make some rope before climbing the cliff.";
            registry[3].tutorialStringUseItems = "";
            registry[3].tutorialStringPostKeybinds = "Harvest some rocks.";
            registry[3].tutorialStringCrafting = "To make a spear, you need a stick, rocks, and rope.";
            registry[3].tutorialStringWeapon = "";
            registry[3].rebindAndFinish = "Progress through the game by using teleporters (<Use>).\nIf you don't like the keybinds, press escape.\nYou can change them by pressing the button\nthat looks like a gear.";

            registry.Add(4, (new PlayerStarterKit(
                Game1.player_warrior_animations,
                new Item[] { new Item_Hook_Better(1), new Item_Rope(2), new Item_Hook_Better(1), new Item_Macuhatil(1),/* new Item_Spear_Fanged(4) */},
                new Card[] { new CardBlink(1), null },
                2,
                Game1.unlock_warrior)));
            registry[4].tutorialStringMovement = "This character starts with melee weaponry and teleportation.";
            registry[4].tutorialStringInventory = "Put your sword in an inventory slot. Teleporting (<Ability_0>)\nwith a sword will attack along the\npath of the teleportation.\nKilling a creature will recharge your teleport by one.";
            registry[4].tutorialStringUseItems = "Climb to the top of the cliff with your grappling hook.";
            registry[4].tutorialStringPostKeybinds = "";
            registry[4].tutorialStringCrafting = "";
            registry[4].tutorialStringWeapon = "";
            registry[4].rebindAndFinish = "Progress through the game by using teleporters (<Use>).\nIf you don't like the keybinds, press escape.\nYou can change them by pressing the button\nthat looks like a gear.";
        }

        public static Dictionary<int, PlayerStarterKit> registry;

    }
}
