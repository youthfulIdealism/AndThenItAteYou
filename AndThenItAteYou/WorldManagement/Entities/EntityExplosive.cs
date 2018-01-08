using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Survive.Sound;
using Survive.WorldManagement.Entities.Particles;
using Survive.WorldManagement.Inventory;
using Survive.WorldManagement.Inventory.Items;
using Survive.WorldManagement.Tile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities
{
    public class EntityExplosive : Entity
    {
        public int timeBeforeExplosion = 17;
        public int radious = 0;
        bool stuck = false;
        Vector2 stuckLoc;


        public EntityExplosive(Vector2 location, WorldBase world) : base(location, world)
        {
            this.width = 20;
            this.height = 20;
            this.blocksWeaponsOnHit = false;
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);

            ParticleSpeedBoost particle = new ParticleSpeedBoost(location + new Vector2(-width / 2, height / 2), world, new Vector2()/*velocity * -1*/, 100);
            world.addEntity(particle);

            if ((collideBottom || collideLeft || collideRight || collideTop) && !stuck)
            {
                stuck = true;
                stuckLoc = location;
            }

            if(stuck)
            {
                impulse = new Vector2();
                velocity = new Vector2();
                location = stuckLoc;

                timeBeforeExplosion--;

                if (timeBeforeExplosion % 16 == 0)
                {
                    world.addEntity(new ParticleGunFlash(location + new Vector2(-20, -20), world, 3));
                }

                if (timeBeforeExplosion <= 0)
                {
                    SoundManager.getSound("explode").playWithVariance(0, 1f / Vector2.Distance(location, world.player.location) * 70, (location - world.player.location).X, SoundType.MONSTER);

                    for (int x = -radious; x <= radious; x++)
                    {
                        for (int y = -radious; y <= radious; y++)
                        {
                            Vector2 tileLoc = location + new Vector2(x * Chunk.tileDrawWidth, y * Chunk.tileDrawWidth);
                            TileType destroyedTileType = world.getBlock(tileLoc);
                            if (destroyedTileType != null)
                            {
                                world.addEntity(new ParticleTileBreak(tileLoc, world, new Vector2(), destroyedTileType, 150));
                                world.placeTile(TileTypeReferencer.AIR, tileLoc);
                            }
                        }
                    }

                    world.addEntity(new ParticleGunFlash(location, world, 3));
                    for (int i = 0; i < 16; i++)
                    {
                        world.addEntity(new ParticleGunSparks(location, world, new Vector2(), 50));
                    }

                    foreach (Entity entity in world.entities)
                    {
                        if (Vector2.Distance(entity.location, this.location) < radious * Chunk.tileDrawWidth)
                        {
                            entity.damage(40, this, Vector2.Normalize(entity.location - this.location) * 40);
                        }
                    }

                    world.shakeScreen(50, 400);
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
            batch.Draw(Game1.texture_entity_bomb, new Rectangle(defaultRect.X + offset.X, defaultRect.Y + offset.Y, (int)20, (int)20), null, groundColor, (float)Math.Atan(velocity.Y / velocity.X), Vector2.Zero, effect, 0);
        }
    }
}
