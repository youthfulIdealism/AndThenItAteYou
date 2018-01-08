using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Survive.WorldManagement.Entities.Particles;
using Survive.WorldManagement.Procedurals;
using Survive.WorldManagement.Tile;
using Survive.WorldManagement.Tile.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities
{
    public class EntityLamp : Entity
    {
        private int glowSize = 0;
        

        public EntityLamp(Vector2 location, WorldBase world) : base(location, world)
        {
            this.location = location;
            this.world = world;
            width = 20;
            height = 20;
            health = 100;
            windMultiplier = 0;
            if (rand == null)
            {
                rand = new Random();
            }
            this.blocksWeaponsOnHit = false;
        }

        public override void update(GameTime time)
        {
            impulse = new Vector2();
            ticksExisted++;
            if(ticksExisted % 3 == 0)
            {
                glowSize = rand.Next(20);
            }

            velocity += impulse;



            foreach(Entity victim in world.entities)
            {
                if(!victim.Equals(this) && victim is EntityLamp && getCollisionBox().Intersects(victim.getCollisionBox()))
                {
                    victim.damage(3, this, Vector2.Normalize(world.player.location - location) + new Vector2(0, -1));
                }
            }

            if (health <= 0 || Vector2.Distance(location, world.player.location) > 2000)
            {
                world.killEntity(this);
            }

            if(rand.NextDouble() < .02)
            {
                ParticleGunSparks spark = new ParticleGunSparks(location, world, new Vector2(), 50);
                spark.color = Color.White;
                spark.deathColor = Color.Blue;
                world.addEntity(spark);
            }
        }

        public override void damage(float amt, Entity source, Vector2 force)
        {
            
        }

        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            Rectangle defaultRect = getCollisionBox().ToRect();
            int totalGlowSize = 300 + glowSize;
            int totalWidth = (int)(50 * ((float)health / 100));
            int totalWidthOverTwo = totalWidth / 2;
            batch.Draw(Game1.texture_particle_glow, new Rectangle(defaultRect.X + offset.X - totalGlowSize, defaultRect.Y + offset.Y + 25 - totalGlowSize, totalGlowSize * 2, totalGlowSize * 2), Color.White * .2f);
            batch.Draw(Game1.texture_particle_glow, new Rectangle(defaultRect.X + offset.X - totalGlowSize / 2, defaultRect.Y + offset.Y + 25 - totalGlowSize / 2, totalGlowSize, totalGlowSize), Color.White);
            batch.Draw(Game1.texture_particle_glow, new Rectangle(defaultRect.X + offset.X - totalGlowSize / 4, defaultRect.Y + offset.Y + 25 - totalGlowSize / 4, totalGlowSize / 2, totalGlowSize / 2), Color.LightBlue);
        }

    }
}

