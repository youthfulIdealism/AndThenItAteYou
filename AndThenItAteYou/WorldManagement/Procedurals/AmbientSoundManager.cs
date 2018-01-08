using Microsoft.Xna.Framework.Audio;
using Survive.Sound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Procedurals
{
    public class AmbientSoundManager : IDisposable
    {
        public class WorldSongCollection
        {
            public String[] spookMusic;
            public String[] tenseMusic;
            public String[] calmMusic;
            public WorldSongCollection(String[] spookMusic, String[] tenseMusic, String[] calmMusic)
            {
                this.spookMusic = spookMusic;
                this.tenseMusic = tenseMusic;
                this.calmMusic = calmMusic;
            }
        }

        string baseAmbienceString = "Sounds/Ambiance/";
        string baseMusicString = "Sounds/Music/";
        public static string[] nightSounds = new string[] { "night-0", "night-1" };
        public static string[] daySoundsNoWeather = new string[] { "none-0", "none-1", "none-2", "none-3", "none-4", "none-5" };
        public static string[] daySoundsRain = new string[] { "rain-0" };
        public static string[] daySoundsUrban = new string[] { "urban-0" };
        public static string[] daySoundsWind = new string[] { "wind-0", "wind-1" };
        public static string[] caveSounds = new string[] { "cave_0"};
        public bool disposed = false;

        public static WorldSongCollection[] songs = new WorldSongCollection[] {
            new WorldSongCollection(  new string[]{"startle_0","startle_1","startle_2","startle_3","startle_4"  }, new string[]{"song1_tension_0" }, new string[]{"song1_journey_0", "song1_journey_1", "song1_journey_2", "song1_journey_3", "song1_journey_4", "song1_journey_5" }),
            new WorldSongCollection( new string[]{"startle_0","startle_1","startle_2","startle_3","startle_4" }, new string[]{"song2_tension_0" }, new string[]{"song2_journey_0", "song2_journey_1", "song2_journey_2", "song2_journey_3", "song2_journey_4", "song2_journey_5"}),
            new WorldSongCollection( new string[]{"startle_0","startle_1","startle_2","startle_3","startle_4" }, new string[]{"song3_tension_0" }, new string[]{"song3_journey_0", "song3_journey_1", "song3_journey_2", "song3_journey_3", "song3_journey_4", "song3_journey_5"})
        };

        

        //public SoundEffect daySound;
        public SoundEffectInstance daySoundInstance;

        //public SoundEffect nightSound;
        public SoundEffectInstance nightSoundInstance;

        public float worldNoiseMultiplier = 1f;
        public float musicMultiplier = 0f;
        public SoundEffectInstance musicSoundInstance;
        public SoundEffectInstance spookSoundInstance;
        public SoundEffectInstance tenseSoundInstance;
        public ChunkDecorator decorator;
        public int worldSongIndex;

        private int musicStopRequests = 0;
        private int musicTenseRequests = 0;
        private float tenseMusicFader = 1;

        public AmbientSoundManager(ChunkDecorator decorator)
        {
            this.decorator = decorator;
            string selectedDaySound = null;
            string selectedNightSound = null;

            if(decorator.worldGenSubtype != World.WorldGenSubtype.CENOTE)
            {
                switch (decorator.weatherManager.weather)
                {

                    case WeatherManager.Weather.NONE:
                        selectedDaySound = daySoundsNoWeather[decorator.rand.Next(daySoundsNoWeather.Length)];
                        break;
                    case WeatherManager.Weather.WINDY:
                        selectedDaySound = daySoundsWind[decorator.rand.Next(daySoundsWind.Length)];
                        break;
                    case WeatherManager.Weather.FOGGY:
                        selectedDaySound = daySoundsNoWeather[decorator.rand.Next(daySoundsNoWeather.Length)];
                        break;
                    case WeatherManager.Weather.RAINY:
                        selectedDaySound = daySoundsRain[decorator.rand.Next(daySoundsRain.Length)];
                        break;
                }
                selectedNightSound = nightSounds[decorator.rand.Next(nightSounds.Length)];
            }else
            {
                selectedDaySound = caveSounds[decorator.rand.Next(caveSounds.Length)];
                selectedNightSound = caveSounds[decorator.rand.Next(caveSounds.Length)];
            }




            SoundEffect daySound = decorator.content.Load<SoundEffect>(baseAmbienceString + selectedDaySound);
            SoundEffect nightSound = decorator.content.Load<SoundEffect>(baseAmbienceString + selectedNightSound);

            daySoundInstance = daySound.CreateInstance();
            nightSoundInstance = nightSound.CreateInstance();
            daySoundInstance.IsLooped = true;
            nightSoundInstance.IsLooped = true;
            daySoundInstance.Play();
            nightSoundInstance.Play();

            SoundManager.addSoundToManagement(daySoundInstance);
            SoundManager.addSoundToManagement(nightSoundInstance);

            worldSongIndex = decorator.rand.Next(songs.Length);
            manageAmbientSound(decorator.world.timeOfDay);
        }

        public void manageAmbientSound(float timeOfDay)
        {
            if(disposed)
            {
                return;
            }
            if (tenseSoundInstance != null)
            {
                //I've added the sounds to the soundManager. This may not be necessary.
                if (tenseSoundInstance.State == SoundState.Stopped)
                {
                    tenseSoundInstance.Dispose();
                }
                if (tenseSoundInstance.IsDisposed)
                {
                    tenseSoundInstance = null;
                }
            }
            if(musicTenseRequests > 0)
            {
                tenseMusicFader = (float)Math.Min(tenseMusicFader + .007f, 1);
            }else
            {
                tenseMusicFader *= .95f;
            }

            if(tenseSoundInstance != null)
            {
                tenseSoundInstance.Volume = tenseMusicFader * MetaData.audioSettingMusic;
            }

            if (tenseSoundInstance == null && musicTenseRequests > 0)
            {
                tenseSoundInstance = decorator.content.Load<SoundEffect>(baseMusicString + songs[worldSongIndex].tenseMusic[ChunkDecorator.nonWorldRand.Next(songs[worldSongIndex].tenseMusic.Length)]).CreateInstance();
                tenseSoundInstance.Play();
                tenseSoundInstance.Volume = tenseMusicFader * MetaData.audioSettingMusic;
                
                SoundManager.addSoundToManagement(tenseSoundInstance);
            }

            if(tenseSoundInstance != null && musicTenseRequests <= 0)
            {
                
                tenseSoundInstance.Volume = tenseMusicFader * MetaData.audioSettingMusic;

                if (tenseMusicFader <= .01f)
                {
                    tenseSoundInstance.Stop();
                }
            }


            if (musicSoundInstance != null)
            {
                //I've added the sounds to the soundManager. This may not be necessary.
                if (musicSoundInstance.State == SoundState.Stopped)
                {
                    musicSoundInstance.Dispose();
                }

                if (musicSoundInstance.IsDisposed)
                {
                    musicSoundInstance = null;
                }
            }
            

            if (musicSoundInstance == null || musicSoundInstance.IsDisposed)
            {
                if(ChunkDecorator.nonWorldRand.NextDouble() < .0007f)
                {
                    musicSoundInstance = decorator.content.Load<SoundEffect>(baseMusicString + songs[worldSongIndex].calmMusic[ChunkDecorator.nonWorldRand.Next(songs[worldSongIndex].calmMusic.Length)]).CreateInstance();
                    musicSoundInstance.Play();
                    SoundManager.addSoundToManagement(musicSoundInstance);
                }
            }

            if(musicSoundInstance != null)
            {
                worldNoiseMultiplier = Math.Min(Math.Max(worldNoiseMultiplier * .999f, .25f), 1);
                if(musicStopRequests > 0)
                {
                    musicMultiplier = musicMultiplier * .99f;
                }
                else
                {
                    musicMultiplier = (1 - worldNoiseMultiplier) * MetaData.audioSettingMusic;
                }
                musicSoundInstance.Volume = musicMultiplier;
            }
            else
            {
                worldNoiseMultiplier = Math.Min(Math.Max(worldNoiseMultiplier * 1.005f, .25f), 1);
                if (musicStopRequests > 0)
                {
                    musicMultiplier = 0;
                }
                else
                {
                    musicMultiplier = (1 - worldNoiseMultiplier) * MetaData.audioSettingMusic;
                }
            }
            


            float ambientNoiseShifter = timeOfDay % 1;
            if (timeOfDay <= 1)
            {
                daySoundInstance.Volume = Math.Min(1, Math.Max(0, ambientNoiseShifter * worldNoiseMultiplier * MetaData.audioSettingAmbient));
                nightSoundInstance.Volume = Math.Min(1, Math.Max(0, (1f - ambientNoiseShifter) * worldNoiseMultiplier * MetaData.audioSettingAmbient));
            }
            else if (timeOfDay > 1)
            {
                daySoundInstance.Volume = Math.Min(1, Math.Max(0, (1f - ambientNoiseShifter) * worldNoiseMultiplier * MetaData.audioSettingAmbient));
                nightSoundInstance.Volume = Math.Min(1, Math.Max(0, ambientNoiseShifter * worldNoiseMultiplier * MetaData.audioSettingAmbient));
            }
        }

        public void pause()
        {
            daySoundInstance.Pause();
            nightSoundInstance.Pause();

            if (musicSoundInstance != null)
            {
                musicSoundInstance.Pause();
            }
        }

        public void resume()
        {
            if (daySoundInstance != null)
            {
                daySoundInstance.Resume();
            }
            if (daySoundInstance != null)
            {
                nightSoundInstance.Resume();
            }

            if (musicSoundInstance != null)
            {
                musicSoundInstance.Resume();
            }
        }

        public void requestMusicStop()
        {
            musicStopRequests++;
        }

        public void cancelMusicStopRequest()
        {
            musicStopRequests--;
        }

        public void requestMusicTense()
        {
            musicTenseRequests++;
        }

        public void cancelMusiccTenseRequest()
        {
            musicTenseRequests--;
        }
        
        public void playSpookSound()
        {
            if (disposed)
            {
                return;
            }
            if (spookSoundInstance != null)
            {
                //I've added the sounds to the soundManager. This may not be necessary.
                if (spookSoundInstance.State == SoundState.Stopped)
                {
                    spookSoundInstance.Dispose();
                }
                if (spookSoundInstance.IsDisposed)
                {
                    spookSoundInstance = null;
                }
            }
            if(spookSoundInstance == null)
            {
                spookSoundInstance = decorator.content.Load<SoundEffect>(baseMusicString + songs[worldSongIndex].spookMusic[ChunkDecorator.nonWorldRand.Next(songs[worldSongIndex].spookMusic.Length)]).CreateInstance();
                spookSoundInstance.Play();
                spookSoundInstance.Volume = MetaData.audioSettingMusic * .7f;
                SoundManager.addSoundToManagement(spookSoundInstance);
            }
        }

        public void startle()
        {
            playSpookSound();
            requestMusicTense();
            requestMusicStop();
        }

        public void unStartle()
        {
            cancelMusicStopRequest();
            cancelMusiccTenseRequest();
        }

        public void Dispose()
        {
            disposed = true;
            if (daySoundInstance != null)
            {
                daySoundInstance.Stop();
            }

            if (nightSoundInstance != null)
            {
                nightSoundInstance.Stop();
            }

            if(musicSoundInstance != null)
            {
                musicSoundInstance.Stop();
            }
        }

        
    }
}
