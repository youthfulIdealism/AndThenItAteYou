using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Survive.WorldManagement.Entities.PlayerHelpers
{
    public class State_Throwing : PlayerState
    {
        const float throwDuration = 30;
        float currentThrow;

        public override void exit(Player player)
        {
            
        }

        public override Texture2D getTexture(Player player)
        {
            return player.texture_throw[(int)Math.Min(Math.Ceiling(currentThrow / throwDuration * player.texture_throw.Length), player.texture_throw.Length - 1)];
        }

        public override void enter(Player player)
        {
            
        }

        public override void update(Player player)
        {
            currentThrow++;

            if (decoration is Entity)
            {
                player.facing = ((Entity)decoration).velocity.X;
            }

            if (currentThrow >= throwDuration)
            {
                switchState(player, new State_Standing());
            }

            stateActions.Clear();
        }

        public override bool actionPermitted(STATE_ACTIONS action)
        {
            return currentThrow >= throwDuration;
        }
    }
}
