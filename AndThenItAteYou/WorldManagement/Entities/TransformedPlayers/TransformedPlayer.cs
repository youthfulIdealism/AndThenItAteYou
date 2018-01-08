using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities.TransformedPlayers
{
    public abstract class TransformedPlayer : PlayerBase
    {
        public Player transformedFrom;

        public TransformedPlayer(Vector2 location, WorldBase world, Player transformedFrom) : base(location, world)
        {
            this.transformedFrom = transformedFrom;
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);
            transformedFrom.location = location;
            transformedFrom.velocity = velocity;
        }

        public override void manageHunger()
        {
            transformedFrom.manageHunger();
            hunger = transformedFrom.hunger;
            if(hunger <= 0)
            {
                world.transformPlayer(transformedFrom);
            }
        }

        public override void pickUpItems()
        {
            transformedFrom.pickUpItems();
        }
    }
}
