using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities.PlayerHelpers
{
    public abstract class PlayerState
    {
        public Object decoration;
        public HashSet<STATE_ACTIONS> stateActions;

        public PlayerState()
        {
            stateActions = new HashSet<STATE_ACTIONS>();
        }

        public abstract void enter(Player player);
        public abstract void exit(Player player);
        public abstract void update(Player player);
        public abstract Texture2D getTexture(Player player);
        public abstract bool actionPermitted(STATE_ACTIONS action);

        public void submitStateAction(STATE_ACTIONS action)
        {
            stateActions.Add(action);
        }

        protected void switchState(Player player, PlayerState other)
        {
            exit(player);
            player.state = other;
            other.enter(player);
        }

        public void decorate(Object obj)
        {
            decoration = obj;
        }
    }
}
