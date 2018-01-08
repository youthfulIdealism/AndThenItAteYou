using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Survive.WorldManagement.Entities.PlayerHelpers
{
    public class State_Swinging : PlayerState
    {
        const float swingDuration = 20;
        float currentSwing;

        public override void exit(Player player)
        {
            
        }

        public override Texture2D getTexture(Player player)
        {
            return player.texture_swing[(int)Math.Min(Math.Ceiling(currentSwing / swingDuration * player.texture_swing.Length), player.texture_swing.Length - 1)];
        }

        public override void enter(Player player)
        {
            
        }

        public override void update(Player player)
        {
            currentSwing++;

            if (decoration is Entity)
            {
                player.facing = ((Entity)decoration).velocity.X;
            }

            if(currentSwing >= swingDuration)
            {
                switchState(player, new State_Standing());
            }

            stateActions.Clear();
        }

        public override bool actionPermitted(STATE_ACTIONS action)
        {
            return false;
        }
    }
}
