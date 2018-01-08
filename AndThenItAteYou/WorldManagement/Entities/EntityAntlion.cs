using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Survive.Sound;
using Survive.WorldManagement.Entities.Particles;
using Survive.WorldManagement.Entities.Projectiles;
using Survive.WorldManagement.Inventory;
using Survive.WorldManagement.Inventory.Items;
using Survive.WorldManagement.Tile;
using Survive.WorldManagement.Tile.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities
{
    public class EntityAntlion : Entity, AggroAble
    {
        const float coneRadious = 200;
        const int texSwapPoint = 15;
        private int currentTexSwap = texSwapPoint;
        private int currentTexIndex;
        Vector2 lastPlayerLoc;
        Random random;
        Texture2D standTex;
        Point drawOffset = new Point(0, 0);
        bool aggro;
        SoundEffectInstance currentSound;

        public EntityAntlion(Vector2 location, WorldBase world) : base(location, world)
        {
            aggro = false;
            this.width = 70;
            this.height = 70;
            random = new Random();
            standTex = Game1.texture_entity_antlion_stand[0];
            currentTexIndex = 0;
            health = 300;
            windMultiplier = 0;
            this.location = this.location + new Vector2(0, 200 + rand.Next(200));
        }

        public override void performPhysics(GameTime time)
        {
            //base.performPhysics(time);
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);

            currentTexSwap--;
            if (currentTexSwap <= 0)
            {
                currentTexIndex = (currentTexIndex + 1) % Game1.texture_entity_antlion_stand.Length;
                standTex = Game1.texture_entity_antlion_stand[currentTexIndex];
                currentTexSwap = texSwapPoint;
            }

            float distanceFromPlayer = Vector2.Distance(world.player.location, this.location);
            if (world.player.location.X > this.location.X - coneRadious && world.player.location.X < this.location.X + coneRadious)
            {
                spook();
                lastPlayerLoc = world.player.location;
            }

            if (aggro && world.player.location.Y <= location.Y)
            {
                //TODO: allow damaging of other entities
                if (world.player.getCollisionBox().Intersects(this.getCollisionBox()))
                {
                    world.player.damage(20, this, Vector2.Normalize(world.player.location - location) + new Vector2(0, -1) * 5f);
                }

                if(world.player.location.X > this.location.X - coneRadious && world.player.location.X < this.location.X + coneRadious)
                {
                    //suck in earth
                    if (rand.NextDouble() <= .06f)
                    {
                        Vector2 tileBelowPlayerLoc = world.player.location + new Vector2(0, Chunk.tileDrawWidth);
                        Vector2 tileRightPlayerLoc = world.player.location + new Vector2(Chunk.tileDrawWidth, 0);
                        Vector2 tileLeftPlayerLoc = world.player.location + new Vector2(-Chunk.tileDrawWidth, 0);
                        Vector2 tileBelowPlayerTwiceLoc = world.player.location + new Vector2(0, Chunk.tileDrawWidth * 2);
                        TileType tileBelowPlayer = world.getBlock(tileBelowPlayerLoc);
                        TileType tileBelowPlayerTwice = world.getBlock(tileBelowPlayerTwiceLoc);
                        TileType tileRightPlayer = world.getBlock(tileRightPlayerLoc);
                        TileType tileLeftPlayer = world.getBlock(tileLeftPlayerLoc);

                        if (tileBelowPlayer != null && Math.Abs(location.X - world.player.location.X) * 2 < Math.Abs(location.Y - world.player.location.Y))
                        {
                            if(tileBelowPlayer.tags.Contains(TagReferencer.SOLID))
                            {
                                for (int i = 0; i < 10; i++)
                                {
                                    world.addEntity(new ParticleTileBreak(tileBelowPlayerLoc, world, new Vector2(0, 0), tileBelowPlayer, 150));
                                }
                                world.placeTile(TileTypeReferencer.AIR, tileBelowPlayerLoc);
                                SoundManager.getSound(tileBelowPlayer.blockBreakSound).playWithVariance(0, .25f, 0, SoundType.MONSTER);
                            }
                           

                            if(tileBelowPlayerTwice != null && tileBelowPlayerTwice.tags.Contains(TagReferencer.SOLID))
                            {
                                world.placeTile(TileTypeReferencer.SLICK, tileBelowPlayerTwiceLoc);
                            }

                            if(rand.NextDouble() <= .5f && tileRightPlayer != null)
                            {
                                world.placeTile(TileTypeReferencer.AIR, tileRightPlayerLoc);
                            }
                            if (rand.NextDouble() <= .5f)
                            {
                                world.placeTile(TileTypeReferencer.AIR, tileLeftPlayerLoc);
                            }

                        }

                    }

                    //fire projectile that pulls the player
                    if (rand.NextDouble() <= .025f)
                    {
                        EntityAntlionSpit spit = new EntityAntlionSpit(location + new Vector2(width / 2, -15), world, this);
                        spit.velocity += Vector2.Normalize(world.player.location - (location + new Vector2(0, 15))) * 20 ;
                        world.addEntity(spit);
                    }
                }
                
            }

            if (aggro && distanceFromPlayer > 900)
            {
                world.decorator.ambientSoundManager.unStartle();
            }

            if (health <= 0)
            {

                world.decorator.ambientSoundManager.unStartle();
            }
        }

        

        public void spook()
        {
            if (!aggro)
            {
                world.decorator.ambientSoundManager.startle();
                aggro = true;
                for (int y = 0; y < 10; y++)
                {
                    if(y < 3)
                    {
                        Vector2 canalLoc = location + new Vector2(0, y * -Chunk.tileDrawWidth);
                        TileType canalTile = world.getBlock(canalLoc);
                        if (canalTile != null && canalTile.tags.Contains(TagReferencer.SOLID))
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                world.addEntity(new ParticleTileBreak(canalLoc, world, new Vector2(0, 0), canalTile, 150));
                            }
                            world.placeTile(TileTypeReferencer.AIR, canalLoc);
                        }
                    }
                    else
                    {
                        for (int x = -1; x <= 1; x++)
                        {
                            Vector2 canalLoc = location + new Vector2(x * Chunk.tileDrawWidth, y * -Chunk.tileDrawWidth);
                            TileType canalTile = world.getBlock(canalLoc);
                            if (canalTile != null && canalTile.tags.Contains(TagReferencer.SOLID))
                            {
                                for (int i = 0; i < 10; i++)
                                {
                                    world.addEntity(new ParticleTileBreak(canalLoc, world, new Vector2(0, 0), canalTile, 150));
                                }
                                world.placeTile(TileTypeReferencer.AIR, canalLoc);
                            }
                        }
                    }
                   
                }
            }
                
        }

        public bool isAggrod()
        {
            return aggro;
        }

        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            //base.draw(batch, time, offset, Color.Red);

            Texture2D currentTex = null;
             currentTex = standTex;

            SpriteEffects effect = SpriteEffects.None;
            if (velocity.X < 0)
            {
                effect = SpriteEffects.FlipHorizontally;
            }

            Rectangle defaultRect = getCollisionBox().ToRect();
            batch.Draw(currentTex, new Rectangle(defaultRect.X + offset.X + drawOffset.X, defaultRect.Y + offset.Y + drawOffset.Y, currentTex.Width, currentTex.Height), null, getDrawColor(groundColor, time), 0, Vector2.Zero, effect, 0);
        }
    }
}
