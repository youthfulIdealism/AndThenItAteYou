using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Survive.WorldManagement.Entities;
using Survive.WorldManagement.Entities.Particles;
using Survive.WorldManagement.Worlds.CutsceneTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Survive.WorldManagement.Worlds.CutsceneTools.AnimatedEntity;

namespace Survive.WorldManagement.Worlds.Cutscenes
{
    public class CreditsCutscene : CutsceneWorld
    {
        SoundEffectInstance musicInstance;
        bool hasMusicStarted;

        public CreditsCutscene(WorldBase returnTo) : base("Content\\Cutscene\\creditsCutScene\\cutsceneCreditWorld", returnTo)
        {
            decorator.ambientSoundManager.requestMusicStop();

            int distanceBetweenCreditEntries = 150;

            SoundEffect music = content.Load<SoundEffect>("Sounds/Music/credits_0");
            musicInstance = music.CreateInstance();
            musicInstance.Volume = MetaData.audioSettingMusic;

            List<String> creditsString = new List<string>();
            creditsString.Add("");
            creditsString.Add("");
            creditsString.Add("");
            creditsString.Add("");
            creditsString.Add("");
            creditsString.Add("");
            creditsString.Add("");
            creditsString.Add("");
            creditsString.Add("Credits");
            creditsString.Add("                Programming, Design");
            creditsString.Add("Samuel Richardson");
            creditsString.Add("");
            creditsString.Add("                Art");
            creditsString.Add("Alexander 'Brotenko' Fassbender");
            creditsString.Add("Aaron Richardson");
            creditsString.Add("Samuel Richardson");
            creditsString.Add("");
            creditsString.Add("                Music");
            creditsString.Add("Aaron Richardson");
            creditsString.Add("");
            creditsString.Add("                Special Thanks");
            creditsString.Add("");
            //creditsString.Add("All the people who played my game before release,\nwho put up with bugs and incomplete design,\nyou helped make the game what it is today.\n\nYou people are awesome.");
            creditsString.Add("You people are awesome.\n\nyou helped make the game what it is today.\nwho put up with bugs and incomplete design,\nAll the people who played my game before release,");
            creditsString.Add("");
            creditsString.Add("Sven 'Hoxeel' Lewandowski");
            creditsString.Add("Family and Friends");
            creditsString.Add("My local library");
            creditsString.Add("Even though missing someone was inevitable, I'm sorry.\nSpecial Thanks to the person(s) I forgot to credit.");
            creditsString.Add("");
            creditsString.Add("with my game.\ndear player, for spending time\n...and special thanks to you,");
            creditsString.Add("");

            int counter = 0;
            foreach(string credit in creditsString)
            {
                ParticleText text = new ParticleText(new Vector2(-300, distanceBetweenCreditEntries * -counter), this, int.MaxValue, credit);
                //text.drawContrast = true;
                text.font = content.Load<SpriteFont>("Temp_2");
                addEntity(text);
                counter++;
            }

            

            cutsceneDuration = creditsString.Count * 125;
            cameraCommands.Add(new MovementCommand(new Vector2(0, -400), new Vector2(0, -400), 350));
            cameraCommands.Add(new MovementCommand(new Vector2(0, -400), new Vector2(0, -400 + - distanceBetweenCreditEntries * creditsString.Count - 1200), creditsString.Count * 130));

            AnimatedEntity animatedPlayer = new AnimatedEntity();
            AnimatedEntity animatedFather = new AnimatedEntity();
            AnimatedEntity animatedMother = new AnimatedEntity();
            AnimatedEntity animatedSister = new AnimatedEntity();
            animatedEntities.Add(animatedPlayer);
            animatedEntities.Add(animatedFather);
            animatedEntities.Add(animatedMother);
            animatedEntities.Add(animatedSister);




            animatedPlayer.animations.Add(new Animation(7 * 7, 500, Game1.player_default_animations.runTex, true));
            animatedPlayer.animations[0].drawFlipped = true;
            animatedPlayer.movement.Add(new MovementCommand(new Vector2(-1000, 27), new Vector2(-50, 27), 500));

            animatedSister.animations.Add(new Animation(7 * 7, 500, Game1.player_girl_animations.runTex, true));
            animatedSister.animations[0].drawFlipped = true;
            animatedSister.movement.Add(new MovementCommand(new Vector2(-1100, 25), new Vector2(-100, 25), 500));

            animatedMother.animations.Add(new Animation(40, 50, content.loadTextureRange("Cutscene/StartingCutscene/mother_stand_", 0), true));
            animatedMother.movement.Add(new MovementCommand(new Vector2(100, 27), new Vector2(100, 27), 500));
            

            animatedFather.animations.Add(new Animation(40, 500, content.loadTextureRange("Cutscene/StartingCutscene/father_guitar_", 4), true));
            animatedFather.movement.Add(new MovementCommand(new Vector2(-100, 27), new Vector2(-100, 27), 500));

            EntityFire fire = new EntityFire(new Vector2(0, 47), this);
            entities.Add(fire);


            //timeIncrement = .0003f;
        }

        public override void update(GameTime time)
        {
            base.update(time);

            if (!hasMusicStarted)
            {
                musicInstance.Play();
                hasMusicStarted = true;
            }

            /*if(Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.finishCutscene();
            }*/

            if (cutsceneDuration <= 300 && MetaData.unlockCharacter(3))
            {
                MetaData.playUnlockCharacterAlert(3, this, this.cameraLoc);
            }
        }

        protected override void finishCutscene()
        {
            Game1.instance.returnToMainMenu();
            this.Dispose();
        }

    }


        
}
