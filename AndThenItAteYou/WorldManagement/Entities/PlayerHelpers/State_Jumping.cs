using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Survive.WorldManagement.Entities.PlayerHelpers
{
    public class State_Jumping : PlayerState
    {

        const int frameSwitchPoint = 5;
        private float frameSwitcherJump = frameSwitchPoint;
        private int currentJumpFrame;

        public override void exit(Player player)
        {
            
        }

        public override Texture2D getTexture(Player player)
        {
            return player.texture_jump[currentJumpFrame];
        }

        public override void enter(Player player)
        {
            
        }

        public override void update(Player player)
        {
            if (player.collideBottom)
            {
                switchState(player, new State_Running());
            }
            else
            {
                if (stateActions.Contains(STATE_ACTIONS.THROW))
                {
                    State_Throwing to = new State_Throwing();
                    to.decorate(decoration);
                    switchState(player, to);
                }else if (stateActions.Contains(STATE_ACTIONS.SWING))
                {
                    State_Swinging to = new State_Swinging();
                    to.decorate(decoration);
                    switchState(player, to);
                }
                else if (player.velocity.Y > 0)
                {
                    frameSwitcherJump--;
                    if (frameSwitcherJump <= 0)
                    {
                        currentJumpFrame = Math.Min(currentJumpFrame + 1, player.texture_jump.Length - 1);
                        frameSwitcherJump = frameSwitchPoint;
                    }
                }
                else
                {
                    currentJumpFrame = 0;
                }
            }


            stateActions.Clear();
        }

        public override bool actionPermitted(STATE_ACTIONS action)
        {
            return true;
        }
    }
}
