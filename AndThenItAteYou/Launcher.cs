using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Survive.WorldManagement;
using Survive.WorldManagement.IO;
using System;
using System.Windows.Forms;

namespace Survive
{
    public partial class Launcher : Form
    {
        bool playing_audio;
        SoundEffect testSoundAmbient;
        SoundEffect testSoundMonster;
        SoundEffect testSoundMusic;

        SoundEffectInstance soundAmbientInstance;
        SoundEffectInstance soundMonsterInstance;
        SoundEffectInstance soundMusicInstance;

        Game1 game;
        ContentManager content;

        public Launcher()
        {

            GameSaverAndLoader.loadMeta(0);
            InitializeComponent();
            setUpBasedOnSaveSlot(0);
            

            game = new Game1();
            content = new ContentManager(Game1.instance.Content.ServiceProvider, Game1.instance.Content.RootDirectory);

            testSoundAmbient = content.Load<SoundEffect>("Sounds/Ambiance/none-1");
            soundAmbientInstance = testSoundAmbient.CreateInstance();
            soundAmbientInstance.IsLooped = true;

            testSoundMonster = content.Load<SoundEffect>("Sounds/Wav/Gun_0");
            soundMonsterInstance = testSoundMonster.CreateInstance();
            soundMonsterInstance.IsLooped = true;

            testSoundMusic = content.Load<SoundEffect>("Sounds/Music/song1_journey_0");
            soundMusicInstance = testSoundMusic.CreateInstance();
            soundMusicInstance.IsLooped = true;


            toolTip.SetToolTip(start_game_button, "Start the game with the current settings.");
            toolTip.SetToolTip(trackbar_sound_monster, "Sets the volume for the player, monsters, UI, etc.");
            toolTip.SetToolTip(trackbar_sound_ambient, "Sets the volume for the background noise.");
            toolTip.SetToolTip(trackbar_sound_music, "Sets the volume for the music.");
            toolTip.SetToolTip(button_audio_test, "Starts or stops audio so that you can adjust volume levels by ear.");
            toolTip.SetToolTip(difficulty_hard, "Turns off adaptive difficulty. The game will no longer get harder as you get better.");
            toolTip.SetToolTip(difficulty_very_hard, "Turns on adaptive difficulty. The game will will try to match its difficulty to your skill level.");

            playing_audio = false;
            this.saveSlot.SelectedIndex = 0;
        }

        public void setUpBasedOnSaveSlot(int slot)
        {
            Console.WriteLine("slot " + slot);
            GameSaverAndLoader.loadMeta(Game1.selectedSaveSlot);
            this.screenWBox.Text = MetaData.screenW + "";
            this.screenHBox.Text = MetaData.screenH + "";
            this.fullscreenOptionsBox.SelectedIndex = MetaData.screenSetting;
            this.trackbar_sound_ambient.Value = (int)(MetaData.audioSettingAmbient * 100);
            this.trackbar_sound_monster.Value = (int)(MetaData.audioSettingMonster * 100);
            this.trackbar_sound_music.Value = (int)(MetaData.audioSettingMusic * 100);
            

            if (MetaData.adaptiveDifficulty == 1)
            {
                difficulty_very_hard.Checked = true;
            }
            else
            {
                difficulty_hard.Checked = true;
            }
        }


        private void Launcher_Load(object sender, EventArgs e)
        {

        }

        private void start_game_button_Click(object sender, EventArgs e)
        {
            if (validateAndSave(Game1.selectedSaveSlot))
            {
                using (game)
                {
                    game.refreshGraphicSettings();
                    soundAmbientInstance.Stop();
                    soundMonsterInstance.Stop();
                    soundMusicInstance.Stop();
                    content.Dispose();
                    this.Hide();

                    Logger.log("Setting (Adaptive Difficulty): " + MetaData.adaptiveDifficulty);
                    Logger.log("Setting (Ambient Volume): " + MetaData.audioSettingAmbient);
                    Logger.log("Setting (Monster Volume): " + MetaData.audioSettingMonster);
                    Logger.log("Setting (Music Volume): " + MetaData.audioSettingMusic);
                    Logger.log("Setting (Difficulty Modifier): " + MetaData.difficultyModifier);
                    Logger.log("Setting (Previous Difficulty Reached): " + MetaData.prevDifficultyReached);
                    Logger.log("Setting (Screen W): " + MetaData.screenW);
                    Logger.log("Setting (Screen H): " + MetaData.screenH);
                    Logger.log("Setting (Screen Setting): " + MetaData.screenSetting);
                    foreach(int i in MetaData.unlocks)
                    {
                        Logger.log("Setting (Character Unlock): " + i);
                    }





                    game.Run();
                    this.Close();
                }
            }
        }

        private bool validateAndSave(int slot)
        {
            bool canSave = true;
            int w = 0;
            int h = 0;
            int fs = this.fullscreenOptionsBox.SelectedIndex;
            string displayError = "";

            try
            {
                w = int.Parse(this.screenWBox.Text);
                if (w < 0 || w > 1920)
                {
                    throw new FormatException();
                }
            }
            catch (FormatException ex)
            {
                Logger.log("Exception when parsing width: " + ex.ToString());
                canSave = false;
                displayError += "\nInvalid selection for screen width.";
            }

            try
            {
                h = int.Parse(this.screenHBox.Text);
                if (h < 0 || h > 1080)
                {
                    throw new FormatException();
                }
            }
            catch (FormatException ex)
            {
                Logger.log("Exception when parsing height: " + ex.ToString());
                canSave = false;
                displayError += "\nInvalid selection for screen height.";
            }

            if (difficulty_very_hard.Checked)
            {
                MetaData.adaptiveDifficulty = 1;
            }
            else
            {
                MetaData.adaptiveDifficulty = 0;
            }


            if (canSave)
            {
                MetaData.screenW = w;
                MetaData.screenH = h;
                MetaData.screenSetting = fs;
                GameSaverAndLoader.saveMeta(slot);

            }
            else
            {
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show("You put something dumb in one of the option boxes. Try something else?" + displayError, "Invalid launch parameters", buttons);
            }

            return canSave;
        }

        private void button_test_audio_click(object sender, EventArgs e)
        {
            playing_audio = !playing_audio;
            if(playing_audio)
            {
                
                soundAmbientInstance.Play();
                soundMonsterInstance.Play();
                soundMusicInstance.Play();

                this.button_audio_test.Text = "Stop";
            }
            else
            {
                soundAmbientInstance.Pause();
                soundMonsterInstance.Pause();
                soundMusicInstance.Pause();

                this.button_audio_test.Text = "Play";
            }
        }

        private void trackbar_sound_ambient_Scroll(object sender, EventArgs e)
        {
            MetaData.audioSettingAmbient = (float)trackbar_sound_ambient.Value / 100;
            soundAmbientInstance.Volume = MetaData.audioSettingAmbient;
        }

        private void trackbar_sound_music_Scroll(object sender, EventArgs e)
        {
            MetaData.audioSettingMusic = (float)trackbar_sound_music.Value / 100;
            soundMusicInstance.Volume = MetaData.audioSettingMusic;
        }

        private void trackbar_sound_monster_Scroll(object sender, EventArgs e)
        {
            MetaData.audioSettingMonster = (float)trackbar_sound_monster.Value / 100;
            soundMonsterInstance.Volume = MetaData.audioSettingMonster;
        }

        private void saveSlot_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(validateAndSave(Game1.selectedSaveSlot))
            {
                MetaData.reset();
                Game1.selectedSaveSlot = this.saveSlot.SelectedIndex;
                setUpBasedOnSaveSlot(Game1.selectedSaveSlot);
            }
        }
    }
}
