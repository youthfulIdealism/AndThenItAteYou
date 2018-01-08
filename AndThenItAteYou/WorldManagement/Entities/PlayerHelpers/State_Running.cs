using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Survive.WorldManagement.Entities.PlayerHelpers
{
    public class State_Running : PlayerState
    {
        const int frameSwitchPoint = 5;
        private float frameSwitcher = frameSwitchPoint;
        private int currentRunFrame;


        public override void exit(Player player)
        {
            
        }

        public override Texture2D getTexture(Player player)
        {
            return player.texture_run[currentRunFrame];
        }

        public override void enter(Player player)
        {
            
        }

        public override void update(Player player)
        {
            if(player.collideBottom)
            {
                if(stateActions.Contains(STATE_ACTIONS.THROW))
                {
                    State_Throwing to = new State_Throwing();
                    to.decorate(decoration);
                    switchState(player, to);
                }
                else if (stateActions.Contains(STATE_ACTIONS.SWING))
                {
                    State_Swinging to = new State_Swinging();
                    to.decorate(decoration);
                    switchState(player, to);
                }
                if (stateActions.Contains(STATE_ACTIONS.JUMP))
                {
                    switchState(player, new State_Jumping());
                }else if(!stateActions.Contains(STATE_ACTIONS.RUN))
                {
                    switchState(player, new State_Standing());
                }else
                {
                    frameSwitcher--;
                    if (frameSwitcher <= 0)
                    {
                        currentRunFrame = (currentRunFrame + 1) % player.texture_run.Length;
                        frameSwitcher = frameSwitchPoint;
                    }
                }
            }
            else
            {
                switchState(player, new State_Jumping());
            }

            stateActions.Clear();
        }

        public override bool actionPermitted(STATE_ACTIONS action)
        {
            return true;
        }
    }
}
