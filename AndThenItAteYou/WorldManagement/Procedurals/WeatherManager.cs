using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Survive.WorldManagement.World;

namespace Survive.WorldManagement.Procedurals
{
    public class WeatherManager
    {
        public enum Weather { NONE, WINDY, FOGGY, RAINY, STORMY }
        public static Weather[] weatherByProbability = new Weather[] { Weather.NONE, Weather.NONE, Weather.NONE, Weather.NONE, Weather.NONE, Weather.WINDY, Weather.FOGGY, Weather.RAINY };
        public Weather weather;
        public float windStrength;
        public int cloudyness;
        public int rainyness = 25;


        public WeatherManager(ChunkDecorator decorator)
        {

            if (decorator.world is World)
            {


                World wd = ((World)decorator.world);
                float windDir = 0;
                if (decorator.metaDifficulty > 0 && decorator.world.difficulty != Game1.findGirlWorld && decorator.worldGenSubtype != WorldGenSubtype.CENOTE)
                {
                    Weather selectedWeather = weatherByProbability[decorator.rand.Next(weatherByProbability.Length)];
                    weather = selectedWeather;
                    cloudyness = (3 + decorator.rand.Next(6)) * (3 + decorator.rand.Next(6));
                    
                    switch (selectedWeather)
                    {

                        case Weather.NONE:
                            if (decorator.rand.NextDouble() <= .5) { windDir = .2f; } else { windDir = -.2f; }
                            windStrength = (float)(decorator.rand.NextDouble() * .1f) + windDir;
                            break;
                        case Weather.WINDY:
                            if (decorator.rand.NextDouble() <= .5) { windDir = 1; } else { windDir = -1f; }
                            windStrength = (float)(decorator.rand.NextDouble() * .07f) + windDir;

                            if (windStrength > .9f)
                            {
                                windStrength = .9f;
                            }
                            else if (windStrength < -.9f)
                            {
                                windStrength = -.9f;
                            }

                            break;
                        case Weather.FOGGY:
                            windStrength = 0;
                            break;
                        case Weather.RAINY:
                            if (decorator.rand.NextDouble() <= .5) { windDir = .05f; } else { windDir = -.05f; }
                            windStrength = windDir;
                            cloudyness += cloudyness / 2;
                            break;
                    }
                }
                else
                {
                    weather = Weather.NONE;
                    if (decorator.rand.NextDouble() <= .5) { windDir = .2f; } else { windDir = -.2f; }
                    windStrength = (float)(decorator.rand.NextDouble() * .1f) + windDir;
                    cloudyness = (3 + decorator.rand.Next(6)) * (3 + decorator.rand.Next(6));
                }
            }
        }
    }
}
