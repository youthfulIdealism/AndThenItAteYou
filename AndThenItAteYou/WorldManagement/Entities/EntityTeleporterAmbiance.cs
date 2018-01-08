using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Survive.Sound;
using Survive.WorldManagement.Entities.Particles;
using Survive.WorldManagement.Entities.Projectiles;
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
    public class EntityTeleporterAmbiance : Entity
    {
        Vector2 startLoc;
        Random random;
        public SoundEffectInstance currentSound;

        public EntityTeleporterAmbiance(Vector2 location, WorldBase world) : base(location, world)
        {
            startLoc = location;
            random = new Random();
            this.blocksWeaponsOnHit = false;
        }

        public override void damage(float amt, Entity source, Vector2 force)
        {
            
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);
            this.location = startLoc;

            float distanceFromPlayer = Vector2.Distance(world.player.location, this.location);
            if (currentSound == null || (currentSound != null && currentSound.IsDisposed))
            {
                currentSound = SoundManager.getSound("teleporter_ambiance").play(SoundType.AMBIENT);
            }
            currentSound.Volume = Math.Max(0, Math.Min(1, 1f / distanceFromPlayer * 75));

            if (distanceFromPlayer < Game1.instance.graphics.PreferredBackBufferWidth && rand.NextDouble() < .2f)
            {
                
                int xAddative = ((rand.Next(50) - 25) * (rand.Next(50) - 25)); 


                Vector2 particleLoc = location + new Vector2(xAddative, -rand.Next(200) + 50);
                TileType particleBlock = world.getBlock(particleLoc);
                if (particleBlock != null && !particleBlock.Equals(TileTypeReferencer.AIR))
                {
                    ParticleTileBreak particle = new ParticleTileBreak(particleLoc, world, new Vector2(0, 0), particleBlock, 75);
                    particle.gravityMultiplier = -.000001f;
                    particle.width = 5;
                    particle.height = 5;
                    world.addEntity(particle);
                }
            }
        }


        public override void playHitSound()
        {
            
        }

        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {

        }
    }
}
