using Microsoft.Xna.Framework;
using Survive.Input.Data;
using Survive.SplashScreens;
using Survive.WorldManagement.Entities;
using Survive.WorldManagement.Entities.Progression;
using Survive.WorldManagement.Entities.TransformedPlayers;
using Survive.WorldManagement.Inventory;
using Survive.WorldManagement.Inventory.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Survive.Game1;

namespace Survive.WorldManagement.IO
{
    public class GameSaverAndLoader
    {
        static string basePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Saved Games\\Survive\\";

        public static void saveMeta(int saveSlot)
        {
            saveMeta(basePath + saveSlot + "\\");
        }

        public static void saveMeta(String path)
        {
            Logger.log("Saving metagame at " + path);
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(path + "meta.txt"))
                {
                    file.WriteLine(MetaData.difficultyModifier);
                    file.WriteLine(MetaData.prevDifficultyReached);
                    foreach (int i in MetaData.unlocks)
                    {
                        file.WriteLine(i);
                    }
                    file.WriteLine(int.MinValue);
                    file.WriteLine(MetaData.screenW);
                    file.WriteLine(MetaData.screenH);
                    file.WriteLine(MetaData.screenSetting);
                    file.WriteLine(MetaData.audioSettingAmbient);
                    file.WriteLine(MetaData.audioSettingMonster);
                    file.WriteLine(MetaData.audioSettingMusic);
                    file.WriteLine(MetaData.adaptiveDifficulty);
                }
            }
            catch (Exception exception)
            {
                Logger.log(exception.ToString());
            }
        }

        public static void saveKeyBinds(int saveSlot)
        {
            saveKeyBinds(basePath + saveSlot + "\\");
        }

        public static void saveKeyBinds(String path)
        {
            Logger.log("Saving keybinds at " + path);
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(path + "keybinds.txt"))
                {
                    foreach (String action in Game1.keyBindManager.bindings.Keys)
                    {
                        file.WriteLine(action + "|" + KeyManagerEnumerator.reverseAssociations[Game1.keyBindManager.bindings[action]]);
                    }

                }
            }
            catch (Exception exception)
            {
                Logger.log(exception.ToString());
            }
        }

        public static void save(int saveSlot, WorldBase world)
        {
            save(basePath + saveSlot + "\\", world);
        }

        public static void save(String path, WorldBase world)
        {
            Logger.log("Saving game at " + path);
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }



                using (System.IO.StreamWriter file = new System.IO.StreamWriter(path + "save.txt"))
                {
                    Entity playerBase = world.player;
                    Console.WriteLine("1: " + playerBase);
                    if (playerBase is TransformedPlayer)
                    {
                        Console.WriteLine("Transformed");
                        while (!(playerBase is Player))
                        {
                            Console.WriteLine("up: " + playerBase);
                            playerBase = ((TransformedPlayer)playerBase).transformedFrom;
                        }
                    }
                    Player player = (Player)playerBase;

                    file.WriteLine(UniverseProperties.seed);
                    file.WriteLine(world.difficulty);
                    file.WriteLine(player.animationPackage.id);
                    file.WriteLine(player.location.X);
                    file.WriteLine(player.location.Y);
                    file.WriteLine(player.health);
                    file.WriteLine(player.hunger);
                    file.WriteLine(player.warmth);
                    Inventory.PlayerInventory inventory = player.inventory;
                    foreach (Item item in inventory.items)
                    {
                        file.WriteLine("Inventory:" + ItemRegistrar.getIDFromItem(item) + "," + item.getRemainingUses());
                    }

                    for (int i = 0; i < player.cards.Length; i++)
                    {
                        if (player.cards[i] != null)
                        {
                            file.WriteLine("Card:" + CardRegistrar.getIDFromCard(player.cards[i]) + "," + player.cards[i].level);
                        }

                    }
                    file.WriteLine("Keyed Items:");
                    for (int i = 0; i < player.keyedItems.Length; i++)
                    {
                        if(player.keyedItems[i] != null)
                        {
                            file.WriteLine(ItemRegistrar.getIDFromItem(player.keyedItems[i]));
                        }else
                        {
                            file.WriteLine(-1);
                        }
                        
                    }

                }
            }
            catch (Exception exception)
            {
                Logger.log(exception.ToString());
            }
        }

        public static bool doesSaveExist(int slot)
        {
            return doesSaveExist(basePath + slot + "\\");
        }

        public static bool doesSaveExist(String path)
        {
            return Directory.Exists(path) && File.Exists(path + "save.txt");
        }

        public static void load(int saveSlot)
        {
            load(basePath + saveSlot + "\\");
        }

        public static void load(String path)
        {
            Logger.log("Loading game at " + path);
            bool foundWorld = false;
            WorldBase world = null;
            try
            {


                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                else if (File.Exists(path + "save.txt"))
                {
                    foundWorld = true;
                    using (System.IO.StreamReader file = new System.IO.StreamReader(path + "save.txt"))
                    {
                        World.universeProperties = new UniverseProperties(file.ReadLine());
                        int difficulty = int.Parse(file.ReadLine());

                        world = Game1.instance.getWorldBasedOnDifficulty(difficulty);
                        Player player = ((Player)world.player);
                        PlayerAnimationPackage animationPackage = PlayerAnimationPackage.animationPackageKey[int.Parse(file.ReadLine())];
                        animationPackage.apply(player);
                        Inventory.PlayerInventory inventory = player.inventory;
                        float playerX = float.Parse(file.ReadLine());
                        float playerY = float.Parse(file.ReadLine());
                        player.location = new Vector2(playerX, playerY);
                        player.health = float.Parse(file.ReadLine());
                        player.hunger = float.Parse(file.ReadLine());
                        player.warmth = float.Parse(file.ReadLine());

                        int currentCard = 0;
                        string line = file.ReadLine();
                        while (!line.StartsWith("Keyed Items:"))
                        {
                            if (line.StartsWith("Inventory:"))
                            {
                                line = line.Replace("Inventory:", "");
                                string[] entry = line.Split(',');
                                int id = int.Parse(entry[0]);
                                int count = int.Parse(entry[1]);
                                inventory.add(ItemRegistrar.getItemFromIdAndCount(id, count));
                            }
                            else if (line.StartsWith("Card:"))
                            {
                                line = line.Replace("Card:", "");
                                string[] entry = line.Split(',');
                                int id = int.Parse(entry[0]);
                                float level = float.Parse(entry[1]);
                                player.cards[currentCard] = CardRegistrar.getCardFromIdPlayerAndLevel(id, level, player);
                                currentCard++;
                            }
                            line = file.ReadLine();
                        }
                        for(int i = 0; i < player.keyedItems.Length; i++)
                        {
                            int itemData = int.Parse(file.ReadLine());
                            if(itemData != -1)
                            {
                                player.keyedItems[i] = ItemRegistrar.getItemFromIdAndCount(itemData, 1);
                            }
                        }


                    }
                    File.Delete(path + "save.txt");
                }
            }
            catch (Exception e)
            {
                Logger.log(e.ToString());
            }


            if (!foundWorld)
            {
                world = Game1.instance.getWorldBasedOnDifficulty(0);
                Game1.instance.queuedSplashScreens.Add(new PlayerSelectScreen(MetaData.unlocks.ToArray(), world));
            }
            Card.setUpCards();


            Game1.instance.switchWorlds(world);

        }

        public static void deleteSave(int saveSlot)
        {
            deleteSave(basePath + saveSlot + "\\");
        }

        public static void deleteSave(String path)
        {
            Logger.log("Deleting save at " + path);
            try
            {
                File.Delete(path + "save.txt");
            }
            catch (Exception e)
            {
                Logger.log(e.ToString());
            }
        }

        public static void loadMeta(int saveSlot)
        {
            loadMeta(basePath + saveSlot + "\\");
        }

        public static void loadMeta(String path)
        {
            Logger.log("Loading metagame at " + path);
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                else if (File.Exists(path + "meta.txt"))
                {
                    using (System.IO.StreamReader file = new System.IO.StreamReader(path + "meta.txt"))
                    {
                        MetaData.difficultyModifier = int.Parse(file.ReadLine());
                        MetaData.prevDifficultyReached = int.Parse(file.ReadLine());

                        List<int> unlockedChars = new List<int>();
                        int current = int.Parse(file.ReadLine());
                        while (current != int.MinValue)
                        {
                            unlockedChars.Add(current);
                            current = int.Parse(file.ReadLine());
                        }
                        MetaData.unlocks = unlockedChars;
                        MetaData.screenW = int.Parse(file.ReadLine());
                        MetaData.screenH = int.Parse(file.ReadLine());
                        MetaData.screenSetting = int.Parse(file.ReadLine());
                        MetaData.audioSettingAmbient = float.Parse(file.ReadLine());
                        MetaData.audioSettingMonster = float.Parse(file.ReadLine());
                        MetaData.audioSettingMusic = float.Parse(file.ReadLine());
                        MetaData.adaptiveDifficulty = int.Parse(file.ReadLine());
                    }
                }
                else
                {
                    
                }
            }
            catch (Exception e)
            {
                Logger.log(e.ToString());
            }
        }

        public static void loadKeyBinds(int saveSlot)
        {
            loadKeyBinds(basePath + saveSlot + "\\");
        }

        public static void loadKeyBinds(String path)
        {
            Logger.log("Loading keybinds at " + path);
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                else if (File.Exists(path + "keybinds.txt"))
                {
                    using (System.IO.StreamReader file = new System.IO.StreamReader(path + "keybinds.txt"))
                    {
                        String nextLine = file.ReadLine();
                        while (nextLine != null)
                        {
                            string[] parsed = nextLine.Split('|');
                            Game1.keyBindManager.bindings[parsed[0]] = KeyManagerEnumerator.stringAssociations[parsed[1]];
                            nextLine = file.ReadLine();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.log(e.ToString());
            }
        }
    }
}
