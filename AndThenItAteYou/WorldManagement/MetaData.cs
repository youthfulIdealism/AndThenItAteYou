using Microsoft.Xna.Framework;
using Survive.Sound;
using Survive.WorldManagement.Entities;
using Survive.WorldManagement.Entities.Particles;
using Survive.WorldManagement.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Survive.WorldManagement
{
    public static class MetaData
    {
        public static int prevDifficultyReached;
        public static int difficultyModifier;
        public static List<int> unlocks;
        public static int screenW;
        public static int screenH;
        public static int screenSetting;
        public static float audioSettingAmbient;
        public static float audioSettingMusic;
        public static float audioSettingMonster;
        public static int adaptiveDifficulty;

       
        static MetaData()
        {
            reset();
        }

        /**
        * Default values, to be overwritten when loading a save.
        * */
        public static void reset()
        {
            screenW = Screen.PrimaryScreen.Bounds.Width;
            screenH = Screen.PrimaryScreen.Bounds.Height;
            screenSetting = 1;

            difficultyModifier = -1;
            prevDifficultyReached = 0;
            unlocks = new int[] { 0 }.ToList();

            audioSettingAmbient = 1;
            audioSettingMusic = 1;
            audioSettingMonster = 1;
            adaptiveDifficulty = 1;
        }

        /**
         * 
         * unlocks the character at a given character index. Returns true if the character was unlocked,
         * returns false if the character was already unlocked.
         * 
         */
        public static bool unlockCharacter(int characterIndex)
        {
            bool alreadyUnlocked = MetaData.unlocks.Contains(characterIndex);
            if(!alreadyUnlocked)
            {
                MetaData.unlocks.Add(characterIndex);
                GameSaverAndLoader.saveMeta(Game1.selectedSaveSlot);
            }
            return !alreadyUnlocked;
        }

        /**
         * 
         * Plays the sound associated with a character unlock. Sprays reward particles everywhere.
         * 
         * */
        public static void playUnlockCharacterAlert(int characterIndex, WorldBase world, Vector2 playerLocation)
        {
            SoundManager.getSound("char-unlock").play(SoundType.MONSTER);
            Random rand = new Random();
            for (int i = 0; i < 7; i++)
            {
                ParticleArbitrary teleportParticle = new ParticleArbitrary(playerLocation + new Vector2(0, -100), world, new Vector2(), 100, Game1.texture_particle_blood);
                teleportParticle.gravityMultiplier = -.3f;
                teleportParticle.endColor = Color.White;
                world.addEntity(teleportParticle);
            }
            for (int i = 0; i < 7; i++)
            {
                ParticleArbitrary teleportParticle = new ParticleArbitrary(playerLocation + new Vector2(0, -100), world, new Vector2((float)(rand.NextDouble() - .5f) * 2, (float)(rand.NextDouble() - .5f) * 2), 200, Game1.texture_particle_blood);
                teleportParticle.gravityMultiplier = -.01f;
                teleportParticle.startColor = world.decorator.colorManager.groundColor;
                teleportParticle.endColor = Color.White;
                teleportParticle.width = 6;
                teleportParticle.height = 6;
                world.addEntity(teleportParticle);
            }
            ParticleArbitrary unlockParticle = new ParticleArbitrary(playerLocation + new Vector2(0, -100), world, new Vector2(), 300, PlayerKitRegistry.registry[characterIndex].animations.standTex);
            unlockParticle.gravityMultiplier = 0;
            world.addEntity(unlockParticle);
        }
    }
}
