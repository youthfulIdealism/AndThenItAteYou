using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Survive.WorldManagement.Tile;
using Survive.WorldManagement.Tile.Tags;
using Survive.Sound;
using Survive.WorldManagement.Entities.Particles;

namespace Survive.WorldManagement.Entities
{
    public class EntitySeed : Entity
    {
        const float MaxTimeUntilNextPlacement = 40;
        float timeUntilNextPlacement = 0;
        int remainingTrunkTiles;
        const int maxLeafRadious = 2;
        int currentLeafRadious = 0;

        Vector2 trunkLoc;
        Vector2 startLoc;
        public EntitySeed(Vector2 location, WorldBase world) : base(location, world)
        {
            startLoc = location;
            trunkLoc = startLoc/* + new Vector2(0, -Chunk.tileDrawWidth)*/;
            this.blocksWeaponsOnHit = false;
            remainingTrunkTiles = 6;
        }

        public override void update(GameTime time)
        {
            timeUntilNextPlacement--;

            if (timeUntilNextPlacement <= 0)
            {
                TileType tileOn = world.getBlock(trunkLoc);
                if (tileOn != null)
                {
                    if (remainingTrunkTiles > 0)
                    {
                        if (tileOn.tags.Contains(TagReferencer.AIR) && !tileOn.tags.Contains(TagReferencer.Teleporter))
                        {
                            SoundManager.getSound(world.decorator.treeManager.trunk.blockBreakSound).playWithVariance(0, 1f / Vector2.Distance(trunkLoc, world.playerLoc) * 75, (location - world.player.location).X, SoundType.AMBIENT);
                            remainingTrunkTiles--;

                            world.placeTile(world.decorator.treeManager.trunk, trunkLoc);
                            trunkLoc += new Vector2(0, -Chunk.tileDrawWidth);
                        }
                        else
                        {
                            world.killEntity(this);
                        }
                    }
                    else if (remainingTrunkTiles == 0)
                    {
                        if (tileOn.tags.Contains(TagReferencer.AIR) && !tileOn.tags.Contains(TagReferencer.Teleporter))
                        {
                            SoundManager.getSound(world.decorator.treeManager.trunk.blockBreakSound).playWithVariance(0, 1f / Vector2.Distance(trunkLoc, world.playerLoc) * 75, (location - world.player.location).X, SoundType.AMBIENT);
                            remainingTrunkTiles--;

                            world.placeTile(world.decorator.treeManager.treeTop, trunkLoc);
                        }
                        else
                        {
                            world.killEntity(this);
                        }
                    }
                    else
                    {
                        if (currentLeafRadious < maxLeafRadious)
                        {
                            SoundManager.getSound(world.decorator.treeManager.leaves.blockBreakSound).playWithVariance(0, 1f / Vector2.Distance(trunkLoc, world.playerLoc) * 75, (location - world.player.location).X, SoundType.AMBIENT);
                            currentLeafRadious++;
                            TileType consideredTile = world.getBlock(location);

                            for (int x = -currentLeafRadious; x <= currentLeafRadious; x++)
                            {
                                for (int y = -currentLeafRadious; y <= currentLeafRadious; y++)
                                {
                                    Vector2 candidateLoc = trunkLoc + new Vector2(x * Chunk.tileDrawWidth, y * Chunk.tileDrawWidth);
                                    consideredTile = world.getBlock(candidateLoc);
                                    if (consideredTile != null && consideredTile.tags.Contains(TagReferencer.AIR) && !consideredTile.tags.Contains(TagReferencer.Teleporter) && !consideredTile.Equals(world.decorator.treeManager.treeTop) && !consideredTile.Equals(world.decorator.treeManager.trunk))
                                    {
                                        world.placeTile(world.decorator.treeManager.leaves, candidateLoc);
                                        world.addEntity(new ParticleTileBreak(candidateLoc, world, new Vector2(), world.decorator.treeManager.leaves, 100));
                                    }

                                }
                            }
                        }
                        else
                        {
                            world.killEntity(this);
                        }
                    }
                }


                timeUntilNextPlacement = MaxTimeUntilNextPlacement;
            }
            else
            {
                if (remainingTrunkTiles >= 0)
                {
                    world.addEntity(new ParticleTileBreak(trunkLoc, world, new Vector2(), world.decorator.treeManager.trunk, 100));
                }
            }
            
        }


        public override void playHitSound() {}
        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor) { }
    }
}
