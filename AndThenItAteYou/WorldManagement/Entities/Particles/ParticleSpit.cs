﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities.Particles
{
    public class ParticleSpit : Particle
    {
        public Texture2D texture;
        public Color color;
        float rotation;
        float deltarotation;

        public ParticleSpit(Vector2 location, WorldBase world, Vector2 direction, int duration) : base(location, world, duration)
        {
            velocity = direction * rand.Next(3) + new Vector2((float)rand.NextDouble() - .5f, -(float)rand.NextDouble() * 2);
            texture = Game1.texture_particle_blood;
            width = 10 + rand.Next(8);
            height = 10 + rand.Next(8);
            rotation = (float)(rand.NextDouble() * Math.PI * 2);
            deltarotation = (float)(rand.NextDouble() * .02);
            if(rand.NextDouble() <= .5f) { deltarotation *= -1; }
            this.gravityMultiplier = .9f;
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);
            rotation += deltarotation;
        }

        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            Rectangle baseDrawRect = getCollisionBox().ToRect();
            //batch.Draw(texture, new Rectangle(baseDrawRect.X + offset.X, baseDrawRect.Y + offset.Y, baseDrawRect.Width, baseDrawRect.Height), Color.Lerp(color, Color.Red, getPercentageCompleted()) * (1 - getPercentageCompleted()));
            float currentWidth = (1 - this.getPercentageCompleted()) * width;
            float currentHeight = (1 - this.getPercentageCompleted()) * height;

            batch.Draw(texture,
                new Rectangle(baseDrawRect.X + offset.X, baseDrawRect.Y + offset.Y, (int)currentWidth, (int)currentHeight),
                null,
                groundColor,
                rotation,
                new Vector2(texture.Width / 2, texture.Height / 2), SpriteEffects.None, 0);
        }
    }
}
