using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Survive.WorldManagement.Worlds.CutsceneTools;
using Microsoft.Xna.Framework.Content;
using Survive.WorldManagement.Entities;

namespace Survive.WorldManagement.Worlds
{
    public class CutsceneWorld : WorldFromDisk
    {
        protected float cutsceneDuration;
        protected List<MovementCommand> cameraCommands;
        protected HashSet<AnimatedEntity> animatedEntities;
        protected WorldBase returnTo;
        protected ContentManager content;


        public Vector2 cameraLoc = new Vector2();

        public CutsceneWorld(string path, WorldBase returnTo) : base(path, 0)
        {
            spawnsEnemies = false;
            timeOfDay = 1f;
            timeIncrement = 0;
            cameraCommands = new List<MovementCommand>();
            animatedEntities = new HashSet<AnimatedEntity>();
            cutsceneDuration = 10000;
            this.returnTo = returnTo;
            content = new ContentManager(Game1.instance.Content.ServiceProvider, Game1.instance.Content.RootDirectory);
        }




        //Do not draw UI elements
        public override void delayedRender(SpriteBatch batch, GameTime time) {}

        //Do not track player with camera
        public override void trackPlayerMovementsWithCamera(){ }

        public override void update(GameTime time)
        {
            entities.Remove(player);//DO NOT UPDATE OR DRAW THE PLAYER! also, find a less dumb way to not update or draw the player.
            updateCameraCommands();
            List<AnimatedEntity> finishedEntities = new List<AnimatedEntity>();
            foreach(AnimatedEntity entity in animatedEntities)
            {
                entity.update();
                if(entity.isDone())
                {
                    finishedEntities.Add(entity);
                }
            }
            foreach (AnimatedEntity entity in finishedEntities)
            {
                animatedEntities.Remove(entity);
            }


                drawOffset = new Point((int)-cameraLoc.X, (int)-cameraLoc.Y);
            base.update(time);

            cutsceneDuration--;
            if (cutsceneDuration < 0)
            {
                finishCutscene();
            }
        }

        protected void updateCameraCommands()
        {
            if(cameraCommands.Count > 0)
            {
                cameraLoc = cameraCommands[0].update();
                if (cameraCommands[0].isDone())
                {
                    cameraCommands.RemoveAt(0);
                }
            }
            
        }

        protected virtual void finishCutscene()
        {
            Game1.instance.switchWorlds(returnTo);
            this.Dispose();
        }

        public override void draw(SpriteBatch batch, GameTime time)
        {
            base.draw(batch, time);
            foreach (AnimatedEntity entity in animatedEntities)
            {
                entity.draw(batch, time, totalDrawOffset, groundColor);
            }
        }


        public override void Dispose()
        {
            content.Dispose();
            base.Dispose();
        }
    }
}
