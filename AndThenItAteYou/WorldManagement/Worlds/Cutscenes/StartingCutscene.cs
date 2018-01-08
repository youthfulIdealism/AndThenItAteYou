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
    public class StartingCutscene : CutsceneWorld
    {
        AnimatedEntity animatedSister;
        AnimatedEntity animatedPlayer;
        SoundEffectInstance musicInstance;

        public StartingCutscene(WorldBase returnTo) : base("Content\\Cutscene\\StartingCutscene\\cutsceneWorld", returnTo)
        {
            decorator.ambientSoundManager.requestMusicStop();

            cutsceneDuration = 1200;
            cameraCommands.Add(new MovementCommand(new Vector2(1, -40), new Vector2(1, -40), 150));//pause on the scene to let the player process it
            cameraCommands.Add(new MovementCommand(new Vector2(1, -40), new Vector2(-300, -40), 150));//scroll over to the monsters
            cameraCommands.Add(new MovementCommand(new Vector2(-300, -40), new Vector2(-300, -40), 75));//pause on the monsters
            cameraCommands.Add(new MovementCommand(new Vector2(-300, -40), new Vector2(1000, -40), 600));//scroll to the right as the monsters chase the kids

            animatedPlayer = new AnimatedEntity();
            AnimatedEntity animatedMonster1 = new AnimatedEntity();
            AnimatedEntity animatedMonster2 = new AnimatedEntity();
            AnimatedEntity animatedFather = new AnimatedEntity();
            AnimatedEntity animatedMother = new AnimatedEntity();
            animatedSister = new AnimatedEntity();
            animatedEntities.Add(animatedPlayer);
            animatedEntities.Add(animatedMonster1);
            animatedEntities.Add(animatedMonster2);
            animatedEntities.Add(animatedFather);
            animatedEntities.Add(animatedMother);
            animatedEntities.Add(animatedSister);

            SoundEffect music = content.Load<SoundEffect>("Sounds/Music/intro_0");
            musicInstance = music.CreateInstance();
            musicInstance.Volume = MetaData.audioSettingMusic;
            


            animatedPlayer.animations.Add(new Animation(500, 500, new Texture2D[] { Game1.player_default_animations.standTex }, true));
            animatedPlayer.animations[0].drawFlipped = false;
            animatedPlayer.movement.Add(new MovementCommand(new Vector2(100, 27), new Vector2(100, 27), 500));
            animatedPlayer.animations.Add(new Animation(7 * 7, 400, Game1.player_default_animations.runTex, true));
            animatedPlayer.animations[1].drawFlipped = true;
            animatedPlayer.movement.Add(new MovementCommand(new Vector2(100, 27), new Vector2(900, 27), 400));

            animatedSister.animations.Add(new Animation(500, 500, new Texture2D[] { Game1.player_girl_animations.standTex }, true));
            animatedSister.animations[0].drawFlipped = false;
            animatedSister.movement.Add(new MovementCommand(new Vector2(150, 25), new Vector2(150, 25), 500));
            animatedSister.animations.Add(new Animation(7 * 7, 250, Game1.player_girl_animations.runTex, true));
            animatedSister.animations[1].drawFlipped = true;
            animatedSister.movement.Add(new MovementCommand(new Vector2(150, 25), new Vector2(675, 25), 250));

            animatedMother.animations.Add(new Animation(40, 350, content.loadTextureRange("Cutscene/StartingCutscene/mother_stand_", 0), true));
            animatedMother.movement.Add(new MovementCommand(new Vector2(180, 27), new Vector2(180, 27), 350));
            animatedMother.animations.Add(new Animation(222, content.loadTextureRange("Cutscene/StartingCutscene/mother_block_", 2), false));
            animatedMother.movement.Add(new MovementCommand(new Vector2(180, 27), new Vector2(180, 27), 222));
            animatedMother.animations.Add(new Animation(20, content.loadTextureRange("Cutscene/StartingCutscene/mother_attack_", 0), false));
            animatedMother.movement.Add(new MovementCommand(new Vector2(180, 27), new Vector2(175, 27), 20));
            animatedMother.animations.Add(new Animation(20, content.loadTextureRange("Cutscene/StartingCutscene/mother_fall_", 4), false));
            animatedMother.movement.Add(new MovementCommand(new Vector2(175, 27), new Vector2(185, 27), 20));
            animatedMother.animations.Add(new Animation(1000, content.loadTextureRange("Cutscene/StartingCutscene/mother_fallen_", 0), false));
            animatedMother.movement.Add(new MovementCommand(new Vector2(185, 27), new Vector2(185, 27), 1000));

            animatedFather.animations.Add(new Animation(40, 350, content.loadTextureRange("Cutscene/StartingCutscene/father_guitar_", 4), true));
            animatedFather.movement.Add(new MovementCommand(new Vector2(0, 27), new Vector2(0, 27), 600));
            animatedFather.animations.Add(new Animation(50, content.loadTextureRange("Cutscene/StartingCutscene/father_stand_", 9), false));
            animatedFather.movement.Add(new MovementCommand(new Vector2(0, 27), new Vector2(0, 27), 200));
            animatedFather.animations.Add(new Animation(5 * 10, 1000, content.loadTextureRange("Cutscene/StartingCutscene/father_stab_", 8), true));
            animatedFather.movement.Add(new MovementCommand(new Vector2(0, 27), new Vector2(0, 27), 1000));
            entities.Add(new EntityFire(new Vector2(47, 47), this));

            animatedMonster1.animations.Add(new Animation(21, 325, Game1.texture_entity_guardian_stand, true));
            animatedMonster1.movement.Add(new MovementCommand(new Vector2(-300, 25), new Vector2(-300, 25), 325));
            animatedMonster1.animations.Add(new Animation(7 * 5, 65, Game1.texture_entity_guardian_run, true));
            animatedMonster1.movement.Add(new MovementCommand(new Vector2(-300, 25), new Vector2(-75, 25), 65));
            animatedMonster1.animations.Add(new Animation(5 * 10, 1000, content.loadTextureRange("Cutscene/StartingCutscene/guardian_cower_", 8), true));
            animatedMonster1.movement.Add(new MovementCommand(new Vector2(-75, 25), new Vector2(-75, 25), 1000));

            animatedMonster2.animations.Add(new Animation(21, 325, Game1.texture_entity_guardian_stand, true));
            animatedMonster2.movement.Add(new MovementCommand(new Vector2(-400, 25), new Vector2(-400, 25), 325));
            animatedMonster2.animations.Add(new Animation(7 * 5, 600, Game1.texture_entity_guardian_run, true));
            animatedMonster2.movement.Add(new MovementCommand(new Vector2(-400, 25), new Vector2(800, 25), 600));

            animatedMonster2.animations.Add(new Animation(21, 50, Game1.texture_entity_guardian_stand, true));//the action is over. The monster pauses to catch its breath, then returns to fight the father.
            animatedMonster2.movement.Add(new MovementCommand(new Vector2(800, 25), new Vector2(800, 25), 50));
            Animation returnAnim = new Animation(7 * 5, 600, Game1.texture_entity_guardian_run, true);
            returnAnim.drawFlipped = true;
            animatedMonster2.animations.Add(returnAnim);
            animatedMonster2.movement.Add(new MovementCommand(new Vector2(800, 25), new Vector2(-400, 25), 600));


        }

        bool spawnedSisterTeleport = false;
        bool spawnedBrotherTeleport = false;
        bool hasMusicStarted = false;

        public override void update(GameTime time)
        {
            base.update(time);

            if(cutsceneDuration < 1100 && !hasMusicStarted)
            {
                musicInstance.Play();
                hasMusicStarted = true;
            }

            if(!spawnedSisterTeleport && animatedSister.isDone())
            {
                spawnedSisterTeleport = true;
                for (int i = 0; i < 300; i++)
                {
                    ParticleArbitrary particle = new ParticleArbitrary(new Vector2(675 + rand.Next(20), 50 - rand.Next(40)), this, new Vector2((float)rand.NextDouble(), 0), 200, Game1.texture_particle_blood);
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
                    ParticleArbitrary particle = new ParticleArbitrary(new Vector2(900 + rand.Next(20), 50 - rand.Next(40)), this, new Vector2((float)rand.NextDouble(), 0), 200, Game1.texture_particle_blood);
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
