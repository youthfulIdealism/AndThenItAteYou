using Survive.WorldManagement.Inventory;
using Survive.WorldManagement.Tile.ExtensionTileTypes;
using Survive.WorldManagement.Tile.Tags;
using Survive.WorldManagement.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Tile
{
    public static class HarvestDictionary
    {
        private static Dictionary<TileType, ItemDropper[]> harvests = new Dictionary<TileType, ItemDropper[]>();
        private static Dictionary<TileType, TileType> harvestGhosts = new Dictionary<TileType, TileType>();

        public static ItemDropper[] getHarvestsForTile(TileType type)
        {
            if(harvests.ContainsKey(type))
            {
                return harvests[type];
            }else
            {
                return new ItemDropper[] { };
            }
        }

        public static void registerNewHarvest(TileType type, ItemDropper[] dropper)
        {
            if(harvests.ContainsKey(type))
            {
                Logger.log("WARNING: Adding new harvest to existing tile.");
                List<ItemDropper> resizedHarvests = harvests[type].ToList();
                resizedHarvests.AddRange(dropper.ToList());
                harvests[type] = resizedHarvests.ToArray();
            }
            else
            {
                harvests.Add(type, dropper);
            }
            
        }

        public static bool hasGhostForTile(TileType type)
        {
            return harvestGhosts.ContainsKey(type);
        }

        public static TileType getGhostForTile(TileType type)
        {
            return harvestGhosts[type];
        }

        public static void registerNewHarvestWithGhost(TileType type, ItemDropper[] dropper)
        {
            registerNewHarvest(type, dropper);

            List<TileTag> tagCLone = new List<TileTag>();
            foreach(TileTag tag in type.tags)
            {
                if(tag != TagReferencer.Harvest)
                {
                    tagCLone.Add(tag);
                }
            }

            if (type is RandomImageTile)
            {
                harvestGhosts.Add(type, new RandomImageTile(tagCLone.ToArray(), ((RandomImageTile)type).textures.ToArray(), TileType.getTileFromID(type.TILEID) != null));
            }else if(type is RandomImageTileFromSpritesheet)
            {
                harvestGhosts.Add(type, new RandomImageTileFromSpritesheet(tagCLone.ToArray(), TileType.getTileFromID(type.TILEID) != null, (RandomImageTileFromSpritesheet)type));
            }else if(type is AliasTile)
            {
                harvestGhosts.Add(type, new AliasTile(((AliasTile)type).aliased));
            }
            else if (type is TileType)
            {
                harvestGhosts.Add(type, new TileType(tagCLone.ToArray(), type.texture, TileType.getTileFromID(type.TILEID) != null));
            }else
            {
                Logger.log("Invalid tile class when registering a ghost harvest.");
            }

        }
    }
}
