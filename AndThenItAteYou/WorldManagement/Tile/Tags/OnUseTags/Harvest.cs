using Microsoft.Xna.Framework;
using Survive.Sound;
using Survive.WorldManagement.Entities;
using Survive.WorldManagement.Entities.Particles;
using Survive.WorldManagement.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Tile.Tags.OnUseTags
{
    public class Harvest : TileTag
    {
        public override void onUse(WorldBase world, Item harvestTool, Vector2 location, TileType tileType, Entity user)
        {
            base.onUse(world, harvestTool, location, tileType, user);

            if (HarvestDictionary.hasGhostForTile(tileType))
            {
                world.placeTile(HarvestDictionary.getGhostForTile(tileType), location);
            }
            else
            {
                world.placeTile(TileTypeReferencer.AIR, location);
            }

            
            ItemDropper[] drops = HarvestDictionary.getHarvestsForTile(tileType);
            foreach(ItemDropper dropper in drops)
            {
                dropper.drop(world, harvestTool, location);
            }

            for(int i = 0; i < 7; i++)
            {
                world.addEntity(new ParticleTileBreak(location, world, new Vector2(), tileType, 150));
            }
        }
    }
}
