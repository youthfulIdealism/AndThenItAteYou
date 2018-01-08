using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities
{
    public class StatusEffect
    {
        public enum status { SPEED, JUMP, HEALTHBOOST, HEALTHREGEN, DAMAGERESISTANCE, SLOW, POISON };
        public int timeRemaining { get; set; }
        public bool permanent { get; set; }
        public float potency { get; private set; }
        public status effect { get; set; }

        public StatusEffect(status effect, float potency, int timeRemaining, bool permanent)
        {
            this.effect = effect;
            this.potency = potency;
            this.timeRemaining = timeRemaining;
            this.permanent = permanent;
        }

        public virtual void update(Entity parent)
        {
            if(!permanent)
            {
                timeRemaining--;
                //discard status if time runs out.
            }

            switch (effect)
            {
                case status.HEALTHREGEN:
                    parent.health += potency * .05f;
                    break;
                case status.POISON:
                    parent.damage(potency, null, Vector2.Zero);
                    break;
            }
        }

        public StatusEffect clone()
        {
            return new StatusEffect(effect,  potency, timeRemaining, permanent);
        }
        
    }
}
