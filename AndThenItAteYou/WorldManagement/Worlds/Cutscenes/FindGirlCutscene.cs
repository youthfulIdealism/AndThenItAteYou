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
    public class FindGirlCutscene : CutsceneWorld
    {
        AnimatedEntity animatedSister;
        AnimatedEntity animatedPlayer;
        SoundEffectInstance musicInstance;
        bool hasMusicStarted;

        public FindGirlCutscene(WorldBase returnTo) : base("Content\\Cutscene\\FindGirlCutscene\\cutsceneFindSisterWorld", returnTo)
        {
            decorator.ambientSoundManager.requestMusicStop();

            SoundEffect music = content.Load<SoundEffect>("Sounds/Music/boy_finds_sister_0");
            musicInstance = music.CreateInstance();
            musicInstance.Volume = MetaData.audioSettingMusic;

            //set up the next world
            EntityGirl girl = new EntityGirl(new Vector2(100, ((World)returnTo).noise.octavePerlin1D(100) * returnTo.decorator.getTerrainMultiplier() * Chunk.tileDrawWidth), returnTo);
            girl.touchedPlayer = true;
            returnTo.addEntity(girl);


            cutsceneDuration = 800;
            cameraCommands.Add(new MovementCommand(new Vector2(200, 0), new Vector2(200, 0), 650));//pause on the scene to let the player process it

            animatedPlayer = new AnimatedEntity();
            animatedSister = new AnimatedEntity();
            animatedEntities.Add(animatedPlayer);
            animatedEntities.Add(animatedSister);




            animatedPlayer.animations.Add(new Animation(150, 150, new Texture2D[] { Game1.player_default_animations.standTex }, true));
            animatedPlayer.animations[0].drawFlipped = true;
            animatedPlayer.movement.Add(new MovementCommand(new Vector2(0, 27), new Vector2(0, 27), 150));
            animatedPlayer.animations.Add(new Animation(7 * 7, 100, Game1.player_default_animations.runTex, true));
            animatedPlayer.animations[1].drawFlipped = true;
            animatedPlayer.movement.Add(new MovementCommand(new Vector2(0, 27), new Vector2(140, 27), 100));
            animatedPlayer.animations.Add(new Animation(75, content.loadTextureRange("Cutscene/FindGirlCutscene/player_hug_", 3), false));
            animatedPlayer.animations[2].drawFlipped = true;
            animatedPlayer.movement.Add(new MovementCommand(new Vector2(140, 27), new Vector2(140, 27), 75));
            animatedPlayer.animations.Add(new Animation(100, new Texture2D[] { content.Load<Texture2D>("Cutscene/FindGirlCutscene/player_hug_3") }, false));
            animatedPlayer.animations[3].drawFlipped = true;
            animatedPlayer.movement.Add(new MovementCommand(new Vector2(140, 27), new Vector2(140, 27), 100));
            animatedPlayer.animations.Add(new Animation(7 * 7, 150, Game1.player_default_animations.runTex, true));
            animatedPlayer.animations[4].drawFlipped = true;
            animatedPlayer.movement.Add(new MovementCommand(new Vector2(140, 27), new Vector2(475, 27), 150));

            animatedSister.animations.Add(new Animation(150, 150, new Texture2D[] { Game1.player_girl_animations.standTex }, true));
            animatedSister.animations[0].drawFlipped = false;
            animatedSister.movement.Add(new MovementCommand(new Vector2(300, 25), new Vector2(300, 25), 150));
            animatedSister.animations.Add(new Animation(7 * 7, 100, Game1.player_girl_animations.runTex, true));
            animatedSister.animations[1].drawFlipped = false;
            animatedSister.movement.Add(new MovementCommand(new Vector2(300, 25), new Vector2(160, 25), 100));
            animatedSister.animations.Add(new Animation(75, content.loadTextureRange("Cutscene/FindGirlCutscene/girl_hug_", 3), false));
            animatedSister.animations[2].drawFlipped = false;
            animatedSister.movement.Add(new MovementCommand(new Vector2(160, 25), new Vector2(160, 25), 75));
            animatedSister.animations.Add(new Animation(100, new Texture2D[] { content.Load<Texture2D>("Cutscene/FindGirlCutscene/girl_hug_3") }, false));
            animatedSister.animations[3].drawFlipped = false;
            animatedSister.movement.Add(new MovementCommand(new Vector2(160, 25), new Vector2(160, 25), 100));
            animatedSister.animations.Add(new Animation(7 * 7, 200, Game1.player_girl_animations.runTex, true));
            animatedSister.animations[4].drawFlipped = true;
            animatedSister.movement.Add(new MovementCommand(new Vector2(160, 25), new Vector2(475, 25), 200));

            for (int i = 0; i < 300; i++)
            {
                ParticleArbitrary particle = new ParticleArbitrary(new Vector2(-10 + rand.Next(20), 50 - rand.Next(40)), this, new Vector2((float)rand.NextDouble(), 0), 300, Game1.texture_particle_blood);
                particle.width = 4;
                particle.height = 4;
                particle.gravityMultiplier = (float)rand.NextDouble() * -.4f;
                particle.startColor = groundColor;
                particle.endColor = groundColor;
                this.addEntity(particle);
            }
        }

        bool spawnedSisterTeleport = false;
        bool spawnedBrotherTeleport = false;

        public override void update(GameTime time)
        {
            base.update(time);

            if (cutsceneDuration < 750 && !hasMusicStarted)
            {
                musicInstance.Play();
                hasMusicStarted = true;
            }

            if (!spawnedSisterTeleport && animatedSister.isDone())
            {
                spawnedSisterTeleport = true;
                for (int i = 0; i < 300; i++)
                {
                    ParticleArbitrary particle = new ParticleArbitrary(new Vector2(475 + rand.Next(20), 50 - rand.Next(40)), this, new Vector2((float)rand.NextDouble(), 0), 200, Game1.texture_particle_blood);
                    particle.width = 4;
                    particle.height = 4;
                    particle.gravityMultiplier = (float)rand.NextDouble() * -.4f;
                    particle.startColor = groundColor;
                    particle.endColor = groundColor;
                    this.addEntity(particle);
                }
                
            }

            if (!spawnedBrotherTeleport && animatedPlayer.isDone())
            {
                spawnedBrotherTeleport = true;
                for (int i = 0; i < 300; i++)
                {
                    ParticleArbitrary particle = new ParticleArbitrary(new Vector2(475 + rand.Next(20), 50 - rand.Next(40)), this, new Vector2((float)rand.NextDouble(), 0), 200, Game1.texture_particle_blood);
                    particle.width = 4;
                    particle.height = 4;
                    particle.gravityMultiplier = (float)rand.NextDouble() * -.4f;
                    particle.startColor = groundColor;
                    particle.endColor = groundColor;
                    this.addEntity(particle);
                }

            }

            if(Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.finishCutscene();
            }
        }

    }


        
}
