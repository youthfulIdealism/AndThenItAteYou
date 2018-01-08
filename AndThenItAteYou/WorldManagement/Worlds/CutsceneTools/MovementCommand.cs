using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Worlds.CutsceneTools
{
    public class MovementCommand
    {
        float current;
        float duration;
        Vector2 start;
        Vector2 finish;
        
        public MovementCommand(Vector2 start, Vector2 finish, float duration)
        {
            this.duration = duration;
            this.start = start;
            this.finish = finish;
        }

        public Vector2 update()
        {
            current++;
            return Vector2.Lerp(start, finish, current / duration);
        }

        public bool isDone()
        {
            return current >= duration;
        }
    }
}
