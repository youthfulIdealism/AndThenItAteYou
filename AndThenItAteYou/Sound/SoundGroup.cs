using Microsoft.Xna.Framework.Audio;
using Survive.WorldManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.Sound
{
    public class SoundGroup
    {
        public static Random rand;
        public List<SoundEffect> drawFrom;
        

        public SoundGroup(String basePath, int cieling)
        {
            drawFrom = new List<SoundEffect>();
            for (int i = 0; i <= cieling; i++)
            {
                drawFrom.Add(Game1.instance.Content.Load<SoundEffect>(basePath + i));
            }

            if(rand == null)
            {
                rand = new Random();
            }
        }

        public float getVolumeFromSoundType(SoundType soundType)
        {
            switch(soundType)
            {
                case SoundType.AMBIENT:
                    return MetaData.audioSettingAmbient;
                case SoundType.MONSTER:
                    return MetaData.audioSettingMonster;
                case SoundType.MUSIC:
                    return MetaData.audioSettingMusic;
            }
            return 1;
        }


        public SoundEffectInstance play(SoundType soundType)
        {
            SoundEffectInstance instance = drawFrom[rand.Next(drawFrom.Count)].CreateInstance();
            instance.Volume = 1 * getVolumeFromSoundType(soundType);
            instance.Pitch = 0;
            instance.Pan = 0;
            instance.Play();
            SoundManager.addSoundToManagement(instance);
            return instance;
        }

        public SoundEffectInstance play(float pitch, float volume, float pan, SoundType soundType)
        {
            SoundEffectInstance instance = drawFrom[rand.Next(drawFrom.Count)].CreateInstance();
            instance.Pitch = Math.Min(1, Math.Max(-1, pitch));
            instance.Volume = Math.Min(1, Math.Max(0, volume * getVolumeFromSoundType(soundType)));
            instance.Pan = Math.Min(1, Math.Max(-1, pan));
            instance.Play();
            SoundManager.addSoundToManagement(instance);
            return instance;
        }

        public SoundEffectInstance playWithVariance(SoundType soundType)
        {
            SoundEffectInstance instance = drawFrom[rand.Next(drawFrom.Count)].CreateInstance();
            instance.Pitch = Math.Min(1, Math.Max(-1, (float)(rand.NextDouble() * .1 - .05)));
            instance.Volume = Math.Min(1, Math.Max(0, (1 + (float)(rand.NextDouble() * .1 - .05)) * getVolumeFromSoundType(soundType)));
            instance.Pan = Math.Min(1, Math.Max(-1, (float)(rand.NextDouble() * .1 - .05)));
            instance.Play();
            SoundManager.addSoundToManagement(instance);
            return instance;
        }

        public SoundEffectInstance playWithVariance(float basePitch, float baseVolume, float basePan, SoundType soundType)
        {
            SoundEffectInstance instance = drawFrom[rand.Next(drawFrom.Count)].CreateInstance();
            instance.Pitch = Math.Min(1, Math.Max(-1, basePitch + (float)(rand.NextDouble() * .1 - .05)));
            instance.Volume = Math.Min(1, Math.Max(0, (baseVolume + (float)(rand.NextDouble() * .1 - .05)) * getVolumeFromSoundType(soundType)));
            instance.Pan = Math.Min(1, Math.Max(-1, basePan + (float)(rand.NextDouble() * .1 - .05)));
            instance.Play();
            SoundManager.addSoundToManagement(instance);
            return instance;
        }

        public SoundEffectInstance playWithVariance(float basePitch, float baseVolume, float basePan, float pitchVariance, float volumeVariance, float panVariance, SoundType soundType)
        {
            SoundEffectInstance instance = drawFrom[rand.Next(drawFrom.Count)].CreateInstance();
            instance.Pitch = Math.Min(1, Math.Max(-1, basePitch + (float)(rand.NextDouble() * pitchVariance - (pitchVariance / 2))));
            instance.Volume = Math.Min(1, Math.Max(0, (baseVolume + (float)(rand.NextDouble() * volumeVariance - (volumeVariance / 2))) * getVolumeFromSoundType(soundType)));
            instance.Pan = Math.Min(1, Math.Max(-1, basePan + (float)(rand.NextDouble() * panVariance - (panVariance / 2))));
            instance.Play();
            SoundManager.addSoundToManagement(instance);
            return instance;
        }







    }
}
