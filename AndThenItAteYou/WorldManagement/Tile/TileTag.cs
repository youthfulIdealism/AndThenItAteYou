using Microsoft.Xna.Framework;
using Survive.WorldManagement.Entities;
using Survive.WorldManagement.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Tile
{
    public abstract class TileTag
    {
        protected static Dictionary<int, TileTag> tagRegistryByID;
        protected static Dictionary<Type, TileTag> tagRegistryByType;
        protected static int currentID;
        public int TAGID;

        public TileTag()
        {
            if (tagRegistryByID == null)
            {
                tagRegistryByID = new Dictionary<int, TileTag>();
                tagRegistryByType = new Dictionary<Type, TileTag>();
                currentID = 0;
            }

            TAGID = ++currentID;
            tagRegistryByID.Add(TAGID, this);
            tagRegistryByType.Add(this.GetType(), this);
        }

        public virtual void onUse(WorldBase world, Item harvestTool, Vector2 location, TileType tileType, Entity user)
        {

        }

        public static TileTag getTagByType(Type type)
        {
            return tagRegistryByType[type];
        }

        public static TileTag getTagByID(int id)
        {
            return tagRegistryByID[id];
        }

        
    }
}
