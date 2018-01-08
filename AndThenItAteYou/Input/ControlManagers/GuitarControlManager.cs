using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Survive.WorldManagement.Entities;
using Microsoft.Xna.Framework.Graphics;
using System.Globalization;
using Survive.WorldManagement.Entities.Progression;
using Survive.Input.InputManagers;
using Survive.Input.Data;

namespace Survive.Input.ControlManagers
{
    public class GuitarControlManager : ControlManager
    {
        public Random rand;
        public Player player;
        public Dictionary<BinaryInputManager, SoundEffect> soundKeys;
        public Dictionary<BinaryInputManager, String> keyStrings;
        public Dictionary<String, bool> stringHighlights;
        public String[,] drawables;
        public int currentKey;
        public List<BinaryInputManager> queuedKeys;
        const int maxTimeNotFoundKey = 200;
        int timeNotFoundKey = maxTimeNotFoundKey;
        int currentSong;
        int errorCount;

        public float elapsedTimeSinceFirstNoteInSeconds;

        public GuitarControlManager(Player player)
        {
            this.player = player;
            drawables = new string[,]{
                { "Q", "W", "E", "R", "T", "Y", "U" },
                { "A", "S", "D", "F", "G", "H", "J" },
                { "Z", "X", "C", "V", "B", "N", "M" } };

            soundKeys = new Dictionary<BinaryInputManager, SoundEffect>();
            keyStrings = new Dictionary<BinaryInputManager, String>();

            rand = new Random();
            currentSong = rand.Next(Song.availableSongs.Count);

            soundKeys.Add(KeyManagerEnumerator.letrZ, Game1.celloNotes[0, 0]);
            soundKeys.Add(KeyManagerEnumerator.letrX, Game1.celloNotes[0, 1]);
            soundKeys.Add(KeyManagerEnumerator.letrC, Game1.celloNotes[0, 2]);
            soundKeys.Add(KeyManagerEnumerator.letrV, Game1.celloNotes[0, 3]);
            soundKeys.Add(KeyManagerEnumerator.letrB, Game1.celloNotes[0, 4]);
            soundKeys.Add(KeyManagerEnumerator.letrN, Game1.celloNotes[0, 5]);
            soundKeys.Add(KeyManagerEnumerator.letrM, Game1.celloNotes[0, 6]);

            soundKeys.Add(KeyManagerEnumerator.letrA, Game1.celloNotes[1, 0]);
            soundKeys.Add(KeyManagerEnumerator.letrS, Game1.celloNotes[1, 1]);
            soundKeys.Add(KeyManagerEnumerator.letrD, Game1.celloNotes[1, 2]);
            soundKeys.Add(KeyManagerEnumerator.letrF, Game1.celloNotes[1, 3]);
            soundKeys.Add(KeyManagerEnumerator.letrG, Game1.celloNotes[1, 4]);
            soundKeys.Add(KeyManagerEnumerator.letrH, Game1.celloNotes[1, 5]);
            soundKeys.Add(KeyManagerEnumerator.letrJ, Game1.celloNotes[1, 6]);

            soundKeys.Add(KeyManagerEnumerator.letrQ, Game1.celloNotes[2, 0]);
            soundKeys.Add(KeyManagerEnumerator.letrW, Game1.celloNotes[2, 1]);
            soundKeys.Add(KeyManagerEnumerator.letrE, Game1.celloNotes[2, 2]);
            soundKeys.Add(KeyManagerEnumerator.letrR, Game1.celloNotes[2, 3]);
            soundKeys.Add(KeyManagerEnumerator.letrT, Game1.celloNotes[2, 4]);
            soundKeys.Add(KeyManagerEnumerator.letrY, Game1.celloNotes[2, 5]);
            soundKeys.Add(KeyManagerEnumerator.letrU, Game1.celloNotes[2, 6]);

            keyStrings.Add(KeyManagerEnumerator.letrZ, "Z");
            keyStrings.Add(KeyManagerEnumerator.letrX, "X");
            keyStrings.Add(KeyManagerEnumerator.letrC, "C");
            keyStrings.Add(KeyManagerEnumerator.letrV, "V");
            keyStrings.Add(KeyManagerEnumerator.letrB, "B");
            keyStrings.Add(KeyManagerEnumerator.letrN, "N");
            keyStrings.Add(KeyManagerEnumerator.letrM, "M");

            keyStrings.Add(KeyManagerEnumerator.letrA, "A");
            keyStrings.Add(KeyManagerEnumerator.letrS, "S");
            keyStrings.Add(KeyManagerEnumerator.letrD, "D");
            keyStrings.Add(KeyManagerEnumerator.letrF, "F");
            keyStrings.Add(KeyManagerEnumerator.letrG, "G");
            keyStrings.Add(KeyManagerEnumerator.letrH, "H");
            keyStrings.Add(KeyManagerEnumerator.letrJ, "J");

            keyStrings.Add(KeyManagerEnumerator.letrQ, "Q");
            keyStrings.Add(KeyManagerEnumerator.letrW, "W");
            keyStrings.Add(KeyManagerEnumerator.letrE, "E");
            keyStrings.Add(KeyManagerEnumerator.letrR, "R");
            keyStrings.Add(KeyManagerEnumerator.letrT, "T");
            keyStrings.Add(KeyManagerEnumerator.letrY, "Y");
            keyStrings.Add(KeyManagerEnumerator.letrU, "U");

            stringHighlights = new Dictionary<string, bool>();

            foreach (String str in keyStrings.Values)
            {
                stringHighlights.Add(str, false);
            }

            
        }

        public override void switchTo(ControlManager switchedFrom)
        {
            base.switchTo(switchedFrom);
            currentKey = 0;
            queuedKeys = new List<BinaryInputManager>();
            queuedKeys.AddRange(Song.availableSongs[currentSong].notes);
            elapsedTimeSinceFirstNoteInSeconds = 0;
            errorCount = 0;
            player.world.decorator.ambientSoundManager.requestMusicStop();
        }

        public override void acceptInput(GameTime time, KeyboardState currentKeyboardState, MouseState currentMouseState, KeyboardState prevKeyboardState, MouseState prevMouseState)
        {
            timeNotFoundKey--;
            var keys = new List<string>(stringHighlights.Keys);
            foreach (String key in keys)
            {
                stringHighlights[key] = false;
            }
            foreach (BinaryInputManager key in soundKeys.Keys)
            {
                key.update(currentMouseState, currentKeyboardState);
                if (key.isDown())
                {
                    if(!key.wasDown())
                    {
                        soundKeys[key].Play();
                        if (queuedKeys[currentKey] == key)
                        {
                            currentKey++;
                            timeNotFoundKey = maxTimeNotFoundKey;
                        }
                        else
                        {
                            errorCount++;
                        }
                    }
                    stringHighlights[keyStrings[key]] = true;
                }
            }

            if (currentKey > 0)
            {
                elapsedTimeSinceFirstNoteInSeconds += (float)time.ElapsedGameTime.Milliseconds / 1000;
            }

            if (currentKey >= queuedKeys.Count)
            {
                if(!player.hasLeveledUpThisWorld && (player.cards[0] == null || player.cards[1] == null || player.cards[0].level < Card.maxLevel || player.cards[1].level < Card.maxLevel))
                {
                    player.currentControlManager = new LevelUpControlManager((Player)player, 1f);
                    player.hasLeveledUpThisWorld = true;
                }
                else
                {
                    player.currentControlManager = player.movementControlManger;
                    player.movementControlManger.switchTo(null);
                }
                
                player.isNextToAFire = false;
                player.timeNextToAFire = 0;
                player.world.timeOfDay = .5f;
                player.health += 20;
                if(player.world.getCurrentTemperature() < 0)
                {
                    player.world.timeOfDay += .1f;
                }

                if(elapsedTimeSinceFirstNoteInSeconds <= Song.availableSongs[currentSong].correctTime && errorCount <= 1)
                {
                    currentSong = rand.Next(Song.availableSongs.Count);
                }

                foreach (Entity entity in player.world.entities)
                {
                    if (entity is EntityFire)
                    {
                        if (Vector2.Distance(player.location, entity.location) < 200)
                        {
                            player.world.killEntity(entity);
                        }
                    }
                }

                player.world.decorator.ambientSoundManager.cancelMusicStopRequest();
            }

            if(Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                player.currentControlManager = player.movementControlManger;
                player.movementControlManger.switchTo(null);
                player.timeNextToAFire = 0;
                player.world.decorator.ambientSoundManager.cancelMusicStopRequest();
            }
        }

        public override void draw(SpriteBatch batch)
        {
            int screenW = Game1.instance.graphics.PreferredBackBufferWidth;
            int screenH = Game1.instance.graphics.PreferredBackBufferHeight;


            batch.Draw(Game1.guitar, new Rectangle(screenW / 2 - Game1.guitar.Width / 2, screenH / 2 + 75, Game1.guitar.Width, Game1.guitar.Height), Color.White * .35f);
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 7; j++)
                {
                    Vector2 drawPos = new Vector2(screenW / 2 - 350 + j * 100 + i * 50, screenH / 2 + i * 75);
                    String str = drawables[i, j];
                    Color color = Color.White;
                    if(stringHighlights[str])
                    {
                        color = Color.Green;
                    }

                    if(timeNotFoundKey <= 0 && keyStrings[queuedKeys[currentKey]].Equals(str))
                    {
                        color = Color.Yellow;
                    }

                    batch.DrawString(Game1.gamefont_72, str, drawPos, color);
                }
            }

            //sheetMusic
            batch.Draw(Game1.sheetMusic, new Rectangle(screenW / 2 - Game1.sheetMusic.Width / 2, 25, Game1.sheetMusic.Width, Game1.sheetMusic.Height), Color.White);
            int runningTotal = 0;
            for(int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (runningTotal >= queuedKeys.Count)
                    {
                        return;
                    }

                    Vector2 drawPos = new Vector2(screenW / 2 - Game1.sheetMusic.Width / 2 + 60 + j * 60, 100 + i * 60);
                    String str = keyStrings[queuedKeys[runningTotal]];
                    Color color = Color.White;
                    if (runningTotal < currentKey)
                    {
                        color = Color.Green;
                    }

                    if(timeNotFoundKey <= 0 && runningTotal == currentKey)
                    {
                        color = Color.Yellow;
                    }

                    batch.DrawString(Game1.gamefont_32, str, drawPos, color);
                    runningTotal++;
                }
            }
        }
    }
}
