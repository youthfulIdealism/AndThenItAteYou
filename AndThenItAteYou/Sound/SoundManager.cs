using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.Sound
{
    public static class SoundManager
    {
        public static bool hasSetUp { get; private set; } = false;
        public static List<SoundEffectInstance> currentlyPlayingSounds;
        public static Dictionary<String, SoundGroup> soundDictionary;


        public static void Initialize()
        {
            if(!hasSetUp)
            {
                currentlyPlayingSounds = new List<SoundEffectInstance>();
                soundDictionary = new Dictionary<string, SoundGroup>();


                hasSetUp = true;
            }
            else
            {
                Console.WriteLine("ERROR: Tried to Initialize SoundManager more than once!");
            }
        }

        public static void Load(ContentManager content)
        {
            soundDictionary.Add("wind", new SoundGroup("Sounds/Effects/wind-", 3));
            soundDictionary.Add("turkey-spook", new SoundGroup("Sounds/Effects/turkey-spook-", 2));
            soundDictionary.Add("sword-slash", new SoundGroup("Sounds/Effects/sword-slash-", 0));
            soundDictionary.Add("spear-throw", new SoundGroup("Sounds/Effects/spear-throw-", 0));
            soundDictionary.Add("spear-break", new SoundGroup("Sounds/Effects/spear-break_", 0));
            soundDictionary.Add("projectile-impact-metal", new SoundGroup("Sounds/Effects/projectile-impact-metal-", 0));
            soundDictionary.Add("projectile-impact-flesh", new SoundGroup("Sounds/Effects/projectile-impact-flesh-", 3));
            soundDictionary.Add("player-slip", new SoundGroup("Sounds/Effects/player-slip-", 0));
            soundDictionary.Add("player-fall", new SoundGroup("Sounds/Effects/player-fall-", 0));
            soundDictionary.Add("owl-attack", new SoundGroup("Sounds/Wav/Owl_Impact_", 0));
            soundDictionary.Add("jellyfish", new SoundGroup("Sounds/Effects/jellyfish-", 0));
            soundDictionary.Add("guardian-snarl", new SoundGroup("Sounds/Effects/guardian-snarl-", 3));
            soundDictionary.Add("grappling-hook", new SoundGroup("Sounds/Effects/grappling-hook-", 0));
            soundDictionary.Add("frog-spit", new SoundGroup("Sounds/Effects/frog-spit-", 0));
            soundDictionary.Add("frog-hawk-loogie", new SoundGroup("Sounds/Effects/frog-hawk-loogie-", 0));
            soundDictionary.Add("elk-flee", new SoundGroup("Sounds/Effects/elk_flee_", 2));
            soundDictionary.Add("eat", new SoundGroup("Sounds/Effects/eat-", 0));
            soundDictionary.Add("drink", new SoundGroup("Sounds/Effects/drink-", 0));
            soundDictionary.Add("craft-sucess", new SoundGroup("Sounds/Effects/craft-sucess-", 0));
            soundDictionary.Add("craft-fail", new SoundGroup("Sounds/Effects/craft-fail-", 0));
            soundDictionary.Add("constable-talk", new SoundGroup("Sounds/Wav/Constable_Prepare_Shot_", 0));
            soundDictionary.Add("bow-throw", new SoundGroup("Sounds/Effects/bow-throw-", 0));
            soundDictionary.Add("block-break", new SoundGroup("Sounds/Effects/block-break-", 3));
            soundDictionary.Add("rock-break", new SoundGroup("Sounds/Effects/rock-break-", 3));
            soundDictionary.Add("gun-fire", new SoundGroup("Sounds/Wav/Gun_", 0));
            soundDictionary.Add("death", new SoundGroup("Sounds/Wav/Death_", 0));
            soundDictionary.Add("wheelie_prepare_shot", new SoundGroup("Sounds/Wav/Wheelie_Prepare_Shot_", 0));
            soundDictionary.Add("wheelie_walk", new SoundGroup("Sounds/Wav/Wheelie_Walk_", 0));
            soundDictionary.Add("click", new SoundGroup("Sounds/Wav/Click_", 0));
            soundDictionary.Add("teleporter_ambiance", new SoundGroup("Sounds/Wav/Teleporter_Ambience_", 0));
            soundDictionary.Add("teleporter_teleport", new SoundGroup("Sounds/Wav/Teleport_", 0));
            soundDictionary.Add("centipede-talk", new SoundGroup("Sounds/Effects/centipede-", 0));
            soundDictionary.Add("char-unlock", new SoundGroup("Sounds/Effects/char-unlock-", 0));
            soundDictionary.Add("croc-bite", new SoundGroup("Sounds/Effects/croc-bite-", 0));
            soundDictionary.Add("explode", new SoundGroup("Sounds/Effects/explode_", 0));
            soundDictionary.Add("explode_sticky", new SoundGroup("Sounds/Effects/explode_sticky_", 0));
            soundDictionary.Add("card-pick", new SoundGroup("Sounds/Effects/card_pick_", 0));
            soundDictionary.Add("card-use-0", new SoundGroup("Sounds/Effects/card_use_l0_", 0));
            soundDictionary.Add("card-use-1", new SoundGroup("Sounds/Effects/card_use_l1_", 0));
            soundDictionary.Add("card-use-2", new SoundGroup("Sounds/Effects/card_use_l2_", 0));
            soundDictionary.Add("worm-leap", new SoundGroup("Sounds/Effects/worm_leap_", 0));
            soundDictionary.Add("rock-fall", new SoundGroup("Sounds/Wav/rock_", 5));
        }

        public static void UnLoad()
        {
            foreach (SoundEffectInstance effect in currentlyPlayingSounds)
            {
                effect.Stop();
            }
            foreach (SoundEffectInstance effect in currentlyPlayingSounds)
            {
                effect.Dispose();
            }
        }

        public static SoundGroup getSound(String key)
        {
            return soundDictionary[key];
        }

        public static void update()
        {
            List<SoundEffectInstance> terminatedSounds = new List<SoundEffectInstance>();
            foreach(SoundEffectInstance effect in currentlyPlayingSounds)
            {
                if(effect.State == SoundState.Stopped)
                {
                    terminatedSounds.Add(effect);
                }
            }

            foreach (SoundEffectInstance effect in terminatedSounds)
            {
                currentlyPlayingSounds.Remove(effect);
                effect.Dispose();
            }
        }

        public static void pause()
        {
            foreach (SoundEffectInstance effect in currentlyPlayingSounds)
            {
                effect.Pause();
            }
        }

        public static void resume()
        {
            foreach (SoundEffectInstance effect in currentlyPlayingSounds)
            {
                effect.Resume();
            }
        }

        public static void addSoundToManagement(SoundEffectInstance effect)
        {
            currentlyPlayingSounds.Add(effect);
        }
    }
}
