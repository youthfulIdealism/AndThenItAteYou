using Microsoft.Xna.Framework;
using Survive.WorldManagement.Tile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities
{
    public abstract class UsableEntity : Entity
    {
        public UsableEntity(Vector2 location, WorldBase world) : base(location, world)
        {

        }

        public abstract AABB getUseBounds();
        public abstract void use(WorldBase world, Vector2 location, Entity user);
    }
}
