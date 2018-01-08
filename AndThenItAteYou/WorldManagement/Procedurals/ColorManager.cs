using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Procedurals
{
    public class ColorManager
    {
        public static Color[] primaries { get; private set; }

        public Color skyColorDay;
        public Color skyColorNight;
        public Color groundColor;
        public ChunkDecorator decorator;

        public ColorManager(ChunkDecorator decorator)
        {
            this.decorator = decorator;
            if (primaries == null)
            {
                primaries = new Color[] { Color.Blue, Color.Red, Color.Yellow };
            }


            int chosenColorScheme = decorator.rand.Next(4);
            switch (chosenColorScheme)
            {
                case 0:
                    setColorsComplimentary(decorator);
                    break;
                case 1:
                    setColorsTriadic(decorator);
                    break;
                case 2:
                    setColorsAnalagous(decorator);
                    break;
                case 3:
                    setColorsSplitCompliment(decorator);
                    break;
            }
        }

        public Color getRandomStartColor(ChunkDecorator decorator)
        {
            int startPoint = decorator.rand.Next(3);
            float spin = (float)decorator.rand.NextDouble();
            Color start = Color.Lerp(primaries[startPoint], primaries[(startPoint + 1 + decorator.rand.Next(3)) % 3], spin);
            return start;
        }

        public void setColorsComplimentary(ChunkDecorator decorator)
        {
            Color start = getRandomStartColor(decorator);

            float h;
            float s;
            float v;
            RGBtoHSV(start, out h, out s, out v);
            Color compliment = HSVtoRGB((h + 180) % 360, s, v);

            skyColorDay = Color.Lerp(start, Color.White, .75f);
            groundColor = compliment;

            if (decorator.world.difficulty < 1 || decorator.rand.NextDouble() < .9)
            {
                groundColor = Color.Lerp(groundColor, Color.Black, .85f);
            }
            else
            {
                skyColorDay = Color.Lerp(skyColorDay, Color.Black, .9f);
                skyColorNight = Color.Lerp(skyColorNight, Color.Black, .9f);
            }
        }

        public void setColorsTriadic(ChunkDecorator decorator)
        {
            Color start = getRandomStartColor(decorator);

            float h;
            float s;
            float v;
            RGBtoHSV(start, out h, out s, out v);
            Color triad1 = HSVtoRGB((h + 120) % 360, s, v);
            Color triad2 = HSVtoRGB((h + 360 - 120) % 360, s, v);

            groundColor = start;
            skyColorDay = Color.Lerp(triad1, Color.White, .3f);
            skyColorNight = Color.Lerp(triad2, Color.Black, .5f);


            if (decorator.world.difficulty < 1 || decorator.rand.NextDouble() < .9)
            {
                groundColor = Color.Lerp(groundColor, Color.Black, .85f);
            }
            else
            {
                skyColorDay = Color.Lerp(skyColorDay, Color.Black, .9f);
                skyColorNight = Color.Lerp(skyColorNight, Color.Black, .9f);
            }
        }

        public void setColorsAnalagous(ChunkDecorator decorator)
        {
            Color start = getRandomStartColor(decorator);

            float h;
            float s;
            float v;
            RGBtoHSV(start, out h, out s, out v);
            Color analogue1 = HSVtoRGB((h + 30) % 360, s, v);
            Color analogue2 = HSVtoRGB((h + 360 - 30) % 360, s, v);

            groundColor = start;
            skyColorDay = Color.Lerp(analogue1, Color.White, .3f);
            skyColorNight = Color.Lerp(analogue2, Color.Black, .5f);


            if (decorator.world.difficulty < 1 || decorator.rand.NextDouble() < .9)
            {
                groundColor = Color.Lerp(groundColor, Color.Black, .85f);
            }
            else
            {
                skyColorDay = Color.Lerp(skyColorDay, Color.Black, .7f);
                skyColorNight = Color.Lerp(skyColorNight, Color.Black, .7f);
            }
        }

        public void setColorsSplitCompliment(ChunkDecorator decorator)
        {
            Color start = getRandomStartColor(decorator);

            float h;
            float s;
            float v;
            RGBtoHSV(start, out h, out s, out v);
            Color SC1 = HSVtoRGB((h + 130) % 360, s, v);
            Color SC2 = HSVtoRGB((h + 360 - 130) % 360, s, v);

            groundColor = start;
            skyColorDay = Color.Lerp(SC1, Color.White, .3f);
            skyColorNight = Color.Lerp(SC2, Color.Black, .5f);


            if (decorator.world.difficulty < 1 || decorator.rand.NextDouble() < .9)
            {
                groundColor = Color.Lerp(groundColor, Color.Black, .85f);
            }
            else
            {
                skyColorDay = Color.Lerp(skyColorDay, Color.Black, .7f);
                skyColorNight = Color.Lerp(skyColorNight, Color.Black, .7f);
            }
        }

        public void setColorsTetradic(ChunkDecorator decorator)
        {
            Color start = getRandomStartColor(decorator);

            float h;
            float s;
            float v;
            RGBtoHSV(start, out h, out s, out v);
            Color SC1 = HSVtoRGB((h + 130) % 360, s, v);
            Color SC2 = HSVtoRGB((h + 360 - 130) % 360, s, v);

            groundColor = start;
            skyColorDay = Color.Lerp(SC1, Color.White, .3f);
            skyColorNight = Color.Lerp(SC2, Color.Black, .5f);


            if (decorator.world.difficulty < 1 || decorator.rand.NextDouble() < .9)
            {
                groundColor = Color.Lerp(groundColor, Color.Black, .8f + (float)(decorator.rand.NextDouble() * .2));
            }
            else
            {
                skyColorDay = Color.Lerp(skyColorDay, Color.Black, .7f);
                skyColorNight = Color.Lerp(skyColorNight, Color.Black, .7f);
            }
        }

        public Color getSkyColorGivenTimeOfDay(float timeOfDay)
        {
            Color skyColor = Color.Black;
            /*if(decorator.world.worldGenType == WorldBase.WorldGenType.GENERATED_CENOTE)
            {

            }else
            {

            }*/
            if (timeOfDay < 1)
            {
                skyColor = Color.Lerp(skyColorNight, skyColorDay, timeOfDay);
            }
            else if (timeOfDay < 2)
            {
                skyColor = Color.Lerp(skyColorDay, skyColorNight, 1 - (2 - timeOfDay));
            }

            return skyColor;
        }

        public Color getFrontCloudColorGivenTimeOfDay(float timeOfDay)
        {
            Color cloudDark = Color.Lerp(skyColorNight, Color.Black, .3f);
            Color cloudLight = Color.Lerp(skyColorDay, Color.White, .2f);
            Color frontCloudColor = Color.White;
            if (timeOfDay < 1)
            {
                frontCloudColor = Color.Lerp(cloudDark, cloudLight, timeOfDay);
            }
            else if (timeOfDay < 2)
            {
                frontCloudColor = Color.Lerp(cloudLight, cloudDark, 1 - (2 - timeOfDay));
            }
            return frontCloudColor;
        }

        public Color getBackCloudColorGivenTimeOfDay(float timeOfDay)
        {
            Color cloudDark = Color.Lerp(skyColorNight, Color.Black, .3f);
            Color backCloudColor = Color.Black;
            if (timeOfDay < 1)
            {
                backCloudColor = Color.Lerp(cloudDark, skyColorDay, timeOfDay + .2f);
            }
            else if (timeOfDay < 2)
            {
                backCloudColor = Color.Lerp(skyColorDay, cloudDark, 1 - (2 - timeOfDay));
            }

            return backCloudColor;
        }

        public static void RGBtoHSV(Color color, out float h, out float s, out float v)
        {


            float r = ((float)color.R) / 255;
            float g = ((float)color.G) / 255;
            float b = ((float)color.B) / 255;

            float min, max, delta;
            min = Math.Min(Math.Min(r, g), b);
            max = Math.Max(Math.Max(r, g), b);
            v = max;               // v
            delta = max - min;
            if (max != 0)
                s = delta / max;       // s
            else
            {
                // r = g = b = 0		// s = 0, v is undefined
                s = 0;
                h = -1;
                return;
            }
            if (r == max)
                h = (g - b) / delta;       // between yellow & magenta
            else if (g == max)
                h = 2 + (b - r) / delta;   // between cyan & yellow
            else
                h = 4 + (r - g) / delta;   // between magenta & cyan
            h *= 60;               // degrees
            if (h < 0)
                h += 360;
        }

        public static Color HSVtoRGB(float h, float s, float v)
        {
            int i;
            float f, p, q, t;
            if (s == 0)
            {
                return new Color((int)(v * 255), (int)(v * 255), (int)(v * 255));
            }
            h /= 60;            // sector 0 to 5
            i = (int)Math.Floor(h);
            f = h - i;          // factorial part of h
            p = v * (1 - s);
            q = v * (1 - s * f);
            t = v * (1 - s * (1 - f));
            switch (i)
            {
                case 0:
                    return new Color((int)(v * 255), (int)(t * 255), (int)(p * 255));
                case 1:
                    return new Color((int)(q * 255), (int)(v * 255), (int)(p * 255));
                case 2:
                    return new Color((int)(p * 255), (int)(v * 255), (int)(t * 255));
                case 3:
                    return new Color((int)(p * 255), (int)(q * 255), (int)(v * 255));
                case 4:
                    return new Color((int)(t * 255), (int)(p * 255), (int)(v * 255));
                default:
                    return new Color((int)(v * 255), (int)(p * 255), (int)(q * 255));
            }
        }

    }
}
