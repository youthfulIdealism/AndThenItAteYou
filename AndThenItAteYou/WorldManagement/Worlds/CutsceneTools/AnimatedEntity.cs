using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Worlds.CutsceneTools
{
    //TODO: this isn't actually an entity. Rename?
    public class AnimatedEntity
    {
        public List<MovementCommand> movement;
        public List<Animation> animations;
        public Vector2 location;
        public Texture2D drawTex;
        public bool drawFlipped;
        public Point drawOffset;


        public AnimatedEntity()
        {
            movement = new List<MovementCommand>();
            animations = new List<Animation>();
        }

        public void update()
        {
            if (animations.Count > 0)
            {
                drawTex = animations[0].update();
                drawFlipped = animations[0].drawFlipped;
                if (animations[0].isDone())
                {
                    animations.RemoveAt(0);
                }
            }

            if (movement.Count > 0)
            {
                location = movement[0].update();
                if(movement[0].isDone())
                {
                    movement.RemoveAt(0);
                }
            }

            
        }

        public bool isDone()
        {
            return movement.Count == 0 && animations.Count == 0;
        }

        public virtual void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            if(drawTex != null)
            {
                SpriteEffects effect = SpriteEffects.None;
                if (drawFlipped)
                {
                    effect = SpriteEffects.FlipHorizontally;
                }

                Rectangle drawRect = new Rectangle(
                (int)(offset.X + location.X - drawTex.Width / 2),
                (int)(offset.Y + location.Y - drawTex.Height / 2),
                drawTex.Width,
                drawTex.Height
                );

                batch.Draw(drawTex, drawRect, null, groundColor, 0, Vector2.Zero, effect, 0);
            }else
            {
                Logger.log("Trying to draw null texture in AnimatedEntity");
            }
            
        }


        public class Animation
        {
            public float current;
            public float totalTicksExisted;
            public float durationLooping;
            public float duration;
            public Texture2D[] textures;
            bool loops;
            public bool drawFlipped { get; set; }

            public Animation(float duration, Texture2D[] textures, bool loops)
            {
                this.duration = duration;
                this.durationLooping = duration;
                this.textures = textures;
                this.loops = loops;
            }

            public Animation(float duration, float durationLooping, Texture2D[] textures, bool loops)
            {
                this.duration = duration;
                this.durationLooping = durationLooping;
                this.textures = textures;
                this.loops = loops;
            }


            public Texture2D update()
            {
                current++;
                totalTicksExisted++;
                if (loops && current >= duration)
                {
                    current = 0;
                }
                return textures[(int)Math.Min((int)(current / duration * textures.Length), textures.Length - 1)];
            }

            public bool isDone()
            {
                return totalTicksExisted >= durationLooping;
            }

        }
    }
}
