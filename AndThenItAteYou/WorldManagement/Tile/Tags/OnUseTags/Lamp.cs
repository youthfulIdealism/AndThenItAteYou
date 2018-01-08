using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Survive.WorldManagement.Entities;
using Survive.WorldManagement.Inventory;

namespace Survive.WorldManagement.Tile.Tags.OnUseTags
{
    public class Lamp : TileTag
    {
        public override void onUse(WorldBase world, Item harvestTool, Vector2 location, TileType tileType, Entity user)
        {
            base.onUse(world, harvestTool, location, tileType, user);

            Vector2 lampSpawnLoc = location + new Vector2(Chunk.tileDrawWidth / 2, -Chunk.tileDrawWidth);
            bool canSpawn = true;
            foreach(Entity entity in world.entities)
            {
                if(entity is EntityLamp && Vector2.Distance(lampSpawnLoc, entity.location) < Chunk.tileDrawWidth)
                {
                    canSpawn = false;
                    break;
                }
            }
            

            if(canSpawn)
            {
                world.addEntity(new EntityLamp(lampSpawnLoc, world));
            }
        }
    }
}
