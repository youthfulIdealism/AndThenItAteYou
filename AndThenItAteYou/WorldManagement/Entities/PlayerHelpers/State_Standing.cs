using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Survive.WorldManagement.Entities.PlayerHelpers
{
    public class State_Standing : PlayerState
    {
        public override void exit(Player player)
        {
            
        }

        public override Texture2D getTexture(Player player)
        {
            return player.texture_player_default_stand;
        }

        public override void enter(Player player)
        {
            
        }

        public override void update(Player player)
        {
            if (player.collideBottom)
            {
                if (stateActions.Contains(STATE_ACTIONS.THROW))
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
                else if (stateActions.Contains(STATE_ACTIONS.JUMP))
                {
                    switchState(player, new State_Jumping());
                }
                else if (stateActions.Contains(STATE_ACTIONS.RUN))
                {
                    switchState(player, new State_Running());
                }
            }else
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
