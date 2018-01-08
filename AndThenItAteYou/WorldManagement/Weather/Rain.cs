using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Weather
{
    public class Rain
    {
        public Vector2 location { get; set; }
        public Vector2 angle;
        public float completion;

        public Rain(Vector2 location, Vector2 angle)
        {
            this.location = location;
            this.angle = angle;
        }

        public void update()
        {
            if (completion <= 1)
            {
                completion = Math.Min(completion + .02f, 2);
            }
            else
            {
                completion = Math.Min(completion + .08f, 2);
            }

        }
    }
}
