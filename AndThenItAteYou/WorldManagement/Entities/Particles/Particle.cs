using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities.Particles
{
    public abstract class Particle : Entity
    {
        public int duration { get; private set; }

        public Particle(Vector2 location, WorldBase world, int duration) : base(location, world)
        {
            if(rand == null)
            {
                rand = new Random();
            }
            this.duration = duration;
        }

        public float getPercentageCompleted()
        {
            return (float)ticksExisted / (float)duration;
        }

        public override void update(GameTime time)
        {
            ticksExisted++;
            if(ticksExisted > duration)
            {
                world.killEntity(this);
            }

            prePhysicsUpdate(time);

            impulse += new Vector2(0, .5f * gravityMultiplier);

            velocity += impulse;

            location = location + velocity;

            impulse = new Vector2();
        }
    }
}
