using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Survive.Sound;
using Survive.WorldManagement.Entities.Particles;
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
    public class EntitySticky : Entity
    {
        public int timeBeforeExplosion = 150;
        public int radious = 0;
        bool struckGround = false;
        Vector2 stuckLoc;


        public EntitySticky(Vector2 location, WorldBase world) : base(location, world)
        {
            this.width = 20;
            this.height = 20;
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);

            ParticleSpeedBoost particle = new ParticleSpeedBoost(location + new Vector2(-width / 2, height / 2), world, new Vector2()/*velocity * -1*/, 100);
            world.addEntity(particle);

            if ((collideBottom || collideLeft || collideRight || collideTop) && !struckGround)
            {
                struckGround = true;
                stuckLoc = location;
            }

            if (struckGround)
            {
                timeBeforeExplosion--;
                if (timeBeforeExplosion % 16 == 0)
                {
                    world.addEntity(new ParticleGunFlash(location, world, 3));
                }

                if (timeBeforeExplosion <= 0)
                {
                    SoundManager.getSound("explode_sticky").playWithVariance(0, 1f / Vector2.Distance(location, world.player.location) * 70, (location - world.player.location).X, SoundType.MONSTER);
                    for (int x = -radious; x <= radious; x++)
                    {
                        for (int y = -radious; y <= radious; y++)
                        {
                            Vector2 tileLoc = location + new Vector2(x * Chunk.tileDrawWidth, y * Chunk.tileDrawWidth);
                            Vector2 tileBelowLoc = location + new Vector2(x * Chunk.tileDrawWidth, (y + 1) * Chunk.tileDrawWidth);

                            if (world.getBlock(tileLoc) == null || world.getBlock(tileBelowLoc) == null) { continue; }

                            if (world.getBlock(tileLoc).tags.Contains(TagReferencer.AIR) && world.getBlock(tileBelowLoc).tags.Contains(TagReferencer.SOLID))
                            {
                                world.placeTile(TileTypeReferencer.SLIME, tileLoc);
                            }
                            //world.addEntity(new ParticleTileBreak(tileLoc, world, new Vector2(), world.getBlock(tileLoc), 150));


                        }
                    }

                    world.addEntity(new ParticleGunFlash(location, world, 3));
                    for (int i = 0; i < 16; i++)
                    {
                        world.addEntity(new ParticleGunSparks(location, world, new Vector2(), 50));
                    }

                    world.killEntity(this);
                }
            }
        }

        
        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            SpriteEffects effect = SpriteEffects.None;
            if (velocity.X > 0)
            {
                effect = SpriteEffects.FlipHorizontally;
            }

            Rectangle defaultRect = getCollisionBox().ToRect();
            batch.Draw(Game1.texture_entity_bomb_sticky, new Rectangle(defaultRect.X + offset.X, defaultRect.Y + offset.Y, (int)20, (int)20), null, groundColor, (float)Math.Atan(velocity.Y / velocity.X), Vector2.Zero, effect, 0);
        }
    }
}
