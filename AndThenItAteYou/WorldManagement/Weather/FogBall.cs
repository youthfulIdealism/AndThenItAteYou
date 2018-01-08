using Survive.WorldManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Weather
{
    public class FogBall
    {
        public AABB bounds { get; set; }
        public float rotation;
        public float rotationDirection;
        public float timeExisted;
        public FogBall(AABB location, Random rand)
        {
            this.bounds = location;
            rotation = (float)(rand.NextDouble() * Math.PI * 2);
            rotationDirection = (float)(rand.NextDouble() * .02 - .01);
            timeExisted = 0;
        }

        public void update()
        {
            timeExisted = Math.Min(timeExisted + .02f, 1);
            rotation += rotationDirection;
        }
    }
}
