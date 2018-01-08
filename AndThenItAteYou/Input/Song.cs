using Microsoft.Xna.Framework.Input;
using Survive.Input.Data;
using Survive.Input.InputManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.Input
{
    public class Song
    {
        public static List<Song> availableSongs;
        public static Song AMAZINGGRACE1;
        public static Song AMAZINGGRACE2;
        public static Song AMAZINGGRACE3;
        public static Song OHSUZANNA1;
        public static Song OHSUZANNA2;

        public float correctTime { get; protected set; }
        public List<BinaryInputManager> notes { get; protected set; }

        static Song()
        {
            availableSongs = new List<Song>();
            List<BinaryInputManager> amazingGraceKeys1 = new List<BinaryInputManager>();
            float amazingGraceTime1 = 3 + 2; //2 seconds grace

            // 3 seconds
            amazingGraceKeys1.Add(KeyManagerEnumerator.letrA);
            amazingGraceKeys1.Add(KeyManagerEnumerator.letrH);
            amazingGraceKeys1.Add(KeyManagerEnumerator.letrE);
            amazingGraceKeys1.Add(KeyManagerEnumerator.letrE);
            amazingGraceKeys1.Add(KeyManagerEnumerator.letrQ);
            amazingGraceKeys1.Add(KeyManagerEnumerator.letrH);
            amazingGraceKeys1.Add(KeyManagerEnumerator.letrD);
            amazingGraceKeys1.Add(KeyManagerEnumerator.letrA);

            List<BinaryInputManager> amazingGraceKeys2 = new List<BinaryInputManager>();
            float amazingGraceTime2 = 3 + 2; //2 seconds grace

            amazingGraceKeys2.Add(KeyManagerEnumerator.letrA);
            amazingGraceKeys2.Add(KeyManagerEnumerator.letrH);
            amazingGraceKeys2.Add(KeyManagerEnumerator.letrE);
            amazingGraceKeys2.Add(KeyManagerEnumerator.letrE);
            amazingGraceKeys2.Add(KeyManagerEnumerator.letrQ);
            amazingGraceKeys2.Add(KeyManagerEnumerator.letrY);

            List<BinaryInputManager> amazingGraceKeys3 = new List<BinaryInputManager>();
            float amazingGraceTime3 = 3 + 2; //2 seconds grace
            amazingGraceKeys3.Add(KeyManagerEnumerator.letrE);
            amazingGraceKeys3.Add(KeyManagerEnumerator.letrY);
            amazingGraceKeys3.Add(KeyManagerEnumerator.letrE);
            amazingGraceKeys3.Add(KeyManagerEnumerator.letrY);
            amazingGraceKeys3.Add(KeyManagerEnumerator.letrE);
            amazingGraceKeys3.Add(KeyManagerEnumerator.letrH);
            amazingGraceKeys3.Add(KeyManagerEnumerator.letrA);
            amazingGraceKeys3.Add(KeyManagerEnumerator.letrD);
            amazingGraceKeys3.Add(KeyManagerEnumerator.letrH);
            amazingGraceKeys3.Add(KeyManagerEnumerator.letrH);
            amazingGraceKeys3.Add(KeyManagerEnumerator.letrD);
            amazingGraceKeys3.Add(KeyManagerEnumerator.letrA);

            AMAZINGGRACE1 = new Song(amazingGraceKeys1, amazingGraceTime1);
            availableSongs.Add(AMAZINGGRACE1);

            AMAZINGGRACE2 = new Song(amazingGraceKeys2, amazingGraceTime2);
            availableSongs.Add(AMAZINGGRACE2);

            AMAZINGGRACE3 = new Song(amazingGraceKeys3, amazingGraceTime3);
            availableSongs.Add(AMAZINGGRACE3);

            List<BinaryInputManager> ohSuzannaKeys1 = new List<BinaryInputManager>();
            float ohSuzannaTime1 = 5 + 3; //3 seconds grace
            ohSuzannaKeys1.Add(KeyManagerEnumerator.letrA);
            ohSuzannaKeys1.Add(KeyManagerEnumerator.letrD);
            ohSuzannaKeys1.Add(KeyManagerEnumerator.letrG);
            ohSuzannaKeys1.Add(KeyManagerEnumerator.letrQ);
            ohSuzannaKeys1.Add(KeyManagerEnumerator.letrQ);
            ohSuzannaKeys1.Add(KeyManagerEnumerator.letrE);
            ohSuzannaKeys1.Add(KeyManagerEnumerator.letrQ);
            ohSuzannaKeys1.Add(KeyManagerEnumerator.letrG);
            ohSuzannaKeys1.Add(KeyManagerEnumerator.letrA);
            ohSuzannaKeys1.Add(KeyManagerEnumerator.letrD);
            ohSuzannaKeys1.Add(KeyManagerEnumerator.letrG);
            ohSuzannaKeys1.Add(KeyManagerEnumerator.letrG);
            ohSuzannaKeys1.Add(KeyManagerEnumerator.letrD);
            ohSuzannaKeys1.Add(KeyManagerEnumerator.letrA);
            ohSuzannaKeys1.Add(KeyManagerEnumerator.letrD);


            List<BinaryInputManager> ohSuzannaKeys2 = new List<BinaryInputManager>();
            float ohSuzannaTime2 = 5 + 3; //3 seconds grace
            ohSuzannaKeys2.Add(KeyManagerEnumerator.letrA);
            ohSuzannaKeys2.Add(KeyManagerEnumerator.letrD);
            ohSuzannaKeys2.Add(KeyManagerEnumerator.letrG);
            ohSuzannaKeys2.Add(KeyManagerEnumerator.letrQ);
            ohSuzannaKeys2.Add(KeyManagerEnumerator.letrQ);
            ohSuzannaKeys2.Add(KeyManagerEnumerator.letrE);
            ohSuzannaKeys2.Add(KeyManagerEnumerator.letrQ);
            ohSuzannaKeys2.Add(KeyManagerEnumerator.letrG);
            ohSuzannaKeys2.Add(KeyManagerEnumerator.letrA);
            ohSuzannaKeys2.Add(KeyManagerEnumerator.letrA);
            ohSuzannaKeys2.Add(KeyManagerEnumerator.letrD);
            ohSuzannaKeys2.Add(KeyManagerEnumerator.letrG);
            ohSuzannaKeys2.Add(KeyManagerEnumerator.letrG);
            ohSuzannaKeys2.Add(KeyManagerEnumerator.letrD);
            ohSuzannaKeys2.Add(KeyManagerEnumerator.letrD);
            ohSuzannaKeys2.Add(KeyManagerEnumerator.letrA);
            OHSUZANNA1 = new Song(ohSuzannaKeys1, ohSuzannaTime1);
            availableSongs.Add(OHSUZANNA1);

            OHSUZANNA2 = new Song(ohSuzannaKeys2, ohSuzannaTime2);
            availableSongs.Add(OHSUZANNA2);
        }

        public Song(List<BinaryInputManager> notes, float correctTime)
        {
            this.correctTime = correctTime;
            this.notes = notes;
        }
    }
}
