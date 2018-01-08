using Microsoft.Xna.Framework;
using Survive.Input.Data;
using Survive.WorldManagement.Entities;
using Survive.WorldManagement.Entities.Particles;
using Survive.WorldManagement.Inventory;
using Survive.WorldManagement.Inventory.Items;
using Survive.WorldManagement.Tile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Worlds
{
    public class TutorialWorld : WorldFromDisk
    {

        public TutorialWorld() : base("Content\\Worlds\\tutorial", 0)
        {
            spawnsEnemies = false;

            timeOfDay = 1f;
            timeIncrement = 0;
            
            if(player.inventory.getItemOfType(new Item_Rope(0)) != null)
            {
                player.inventory.add(new Item_Rope(12));
            }
        }

        public override void update(GameTime time)
        {
            base.update(time);
            player.health = 100;
            player.hunger = 100;
        }

        public void processInstructionStrings()
        {
            foreach(Entity entity in particles)
            {
                if(entity is ParticleText)
                {
                    killEntity(entity);
                }
            }

            addEntity(new ParticleText(new Vector2(-50, 60), this, int.MaxValue, processInstructionString(player.tutorialStringMovement)));
            addEntity(new ParticleText(new Vector2(480, -40), this, int.MaxValue, processInstructionString(player.tutorialStringInventory)));
            addEntity(new ParticleText(new Vector2(1250, 20), this, int.MaxValue, processInstructionString(player.tutorialStringUseItems)));
            addEntity(new ParticleText(new Vector2(1750, -400), this, int.MaxValue, processInstructionString("Items in slots can be used with these keys:\nSlot 1: <Inventory_0>. Slot 2: <Inventory_1>.\nSlot 3: <Inventory_2>. Slot 4: <Inventory_3>.\nSlot 5: <Inventory_4>. Slot 6: <Inventory_5>.\n\nYou can harvest things by standing over\n   them and holding <Use>.\n" + player.tutorialStringPostKeybinds)));
            addEntity(new ParticleText(new Vector2(2200, -550), this, int.MaxValue, processInstructionString(player.tutorialStringCrafting)));
            addEntity(new ParticleText(new Vector2(3260, -550), this, int.MaxValue, processInstructionString(player.tutorialStringWeapon)));
            addEntity(new ParticleText(new Vector2(3800, -650), this, int.MaxValue, processInstructionString(player.rebindAndFinish)));
            queuedEntites.Add(new EntityTurkey(new Vector2(648, 29), this));
            queuedEntites.Add(new EntityTurkey(new Vector2(2210, -433), this));
            queuedEntites.Add(new EntityTurkey(new Vector2(3532, -271), this));
            queuedEntites.Add(new EntityTapir(new Vector2(3648, -237), this));
            queuedEntites.Add(new EntityTeleporterAmbiance(new Vector2(4126, -323), this));
        }

        public String processInstructionString(String input)
        {
            bool gatheringReplacementValues = false;
            StringBuilder accumulatedString = new StringBuilder();
            StringBuilder replaceableString = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                char current = input[i];
                if (gatheringReplacementValues)
                {
                    if (current.Equals('>'))
                    {
                        gatheringReplacementValues = false;
                        accumulatedString.Append(KeyManagerEnumerator.reverseAssociations[Game1.keyBindManager.bindings[replaceableString.ToString()]]);
                        replaceableString.Clear();
                    }
                    else
                    {
                        replaceableString.Append(current);
                    }
                }
                else
                {
                    if (current.Equals('<'))
                    {
                        gatheringReplacementValues = true;
                    }
                    else
                    {
                        accumulatedString.Append(current);
                    }
                }
            }

            return accumulatedString.ToString();
        }

        public override void Dispose()
        {
            base.Dispose();
            foreach (Entity entity in entities)
            {
                if(entity is EntityTeleporterAmbiance)
                {
                    ((EntityTeleporterAmbiance)entity).currentSound.Stop();
                }
            }
        }

    }
}
