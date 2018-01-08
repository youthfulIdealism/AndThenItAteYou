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
    public class EntityFire : Entity
    {
        const int texSwapPoint = 15;
        private int currentTexSwap = texSwapPoint;
        private int glowSize = 0;
        public Texture2D currentTex;
        public int particleDuration = 1200;



        public EntityFire(Vector2 location, WorldBase world) : base(location, world)
        {
            this.location = location;
            this.world = world;
            width = 20;
            height = 20;
            health = 100;
            if(rand == null)
            {
                rand = new Random();
            }
            currentTex = Game1.texture_entity_fire[rand.Next(Game1.texture_entity_fire.Length)];
            this.blocksWeaponsOnHit = false;
        }

        public override void update(GameTime time)
        {
            //if the fire has been placed inside a block, move it out.
            TileType tileOn = world.getBlock(location);
            TileType tileOnBackground = world.getBackgroundBlock(location);
            if(tileOn != null && tileOn.tags.Contains(TagReferencer.SOLID))
            {
                TileType tileBelow = world.getBlock(location + new Vector2(0, Chunk.tileDrawWidth));
                TileType tileAbove = world.getBlock(location + new Vector2(0, -Chunk.tileDrawWidth));
                TileType tileRight = world.getBlock(location + new Vector2(Chunk.tileDrawWidth, 0));
                TileType tileLeft = world.getBlock(location + new Vector2(-Chunk.tileDrawWidth, 0));
                if (tileAbove != null && tileAbove.tags.Contains(TagReferencer.AIR))
                {
                    location = location + new Vector2(0, -Chunk.tileDrawWidth);
                }
                else if(tileRight != null && tileRight.tags.Contains(TagReferencer.AIR))
                {
                    location = location + new Vector2(Chunk.tileDrawWidth, 0);
                }
                else if (tileLeft != null && tileLeft.tags.Contains(TagReferencer.AIR))
                {
                    location = location + new Vector2(-Chunk.tileDrawWidth, 0);
                }
                else if (tileBelow != null && tileBelow.tags.Contains(TagReferencer.AIR))
                {
                    location = location + new Vector2(0, Chunk.tileDrawWidth); ;
                }
                tileOn = world.getBlock(location);
            }

            if (world is World && tileOnBackground != null)
            {
                if(world.decorator.weatherManager.weather == WeatherManager.Weather.RAINY && !tileOnBackground.tags.Contains(TagReferencer.Shelter) || tileOn.tags.Contains(TagReferencer.WATER))
                {
                    health--;
                }
            }
            


            impulse = new Vector2();
            ticksExisted++;
            currentTexSwap--;
            if(currentTexSwap <= 0)
            {
                Texture2D nextTex = Game1.texture_entity_fire[rand.Next(Game1.texture_entity_fire.Length)];
                while (nextTex == currentTex)
                {
                    nextTex = Game1.texture_entity_fire[rand.Next(Game1.texture_entity_fire.Length)];
                }
                currentTex = nextTex;
                currentTexSwap = texSwapPoint;
            }
            if(ticksExisted % 3 == 0)
            {
                glowSize = rand.Next(20);
            }

            velocity += impulse;

            //Spawn fire-ey particles
            world.addEntity(new ParticleGunFlash(location, world, 3));
            if(rand.NextDouble() < .1)
            {
                ParticleGunSparks sparks = new ParticleGunSparks(location, world, new Vector2(0, -1), 50);
                sparks.velocity = new Vector2((float)rand.NextDouble() * .5f - .25f, -1);
                sparks.gravityMultiplier = -.5f;
                sparks.width *= .5f;
                sparks.height *= .5f;
                sparks.color = Color.Yellow;
                sparks.deathColor = Color.Yellow;
                world.addEntity(sparks);
            }

            if (rand.NextDouble() < .25)
            {
                ParticleSpeedBoost particle = new ParticleSpeedBoost(location + new Vector2(-width / 4, 0), world, new Vector2((float)rand.NextDouble() - .5f, 0)/*velocity * -1*/, particleDuration);
                particle.gravityMultiplier = -.15f;
                particle.width = 20;
                particle.height = 20;
                particle.deltaSize = 1.002f;
                particle.opacityModifier = .4f;


                world.addEntity(particle);
            }


            //spread the fire
            if (rand.NextDouble() < .01f)
            {
                TileType tileBelow = world.getBlock(location + new Vector2(0, Chunk.tileDrawWidth));
                TileType tileAbove = world.getBlock(location + new Vector2(0, -Chunk.tileDrawWidth));
                TileType tileRight = world.getBlock(location + new Vector2(Chunk.tileDrawWidth, 0));
                TileType tileLeft = world.getBlock(location + new Vector2(-Chunk.tileDrawWidth, 0));
                
                //TODO: add spreadable fire
                /*if(tileBelow.tags.Contains(TagReferencer.FLAMMABLE))
                {
                    EntityFire fire = new EntityFire(location + new Vector2(0, Chunk.tileDrawWidth), world);
                    world.addEntity(fire);
                }
                if (tileAbove.tags.Contains(TagReferencer.FLAMMABLE))
                {
                    EntityFire fire = new EntityFire(location + new Vector2(0, -Chunk.tileDrawWidth), world);
                    world.addEntity(fire);
                }
                if (tileRight.tags.Contains(TagReferencer.FLAMMABLE))
                {
                    EntityFire fire = new EntityFire(location + new Vector2(Chunk.tileDrawWidth, 0), world);
                    world.addEntity(fire);
                }
                if (tileLeft.tags.Contains(TagReferencer.FLAMMABLE))
                {
                    EntityFire fire = new EntityFire(location + new Vector2(-Chunk.tileDrawWidth, 0), world);
                    world.addEntity(fire);
                }*/
            }
            collideBottom = false;
            collideLeft = false;
            collideRight = false;
            collideTop = false;

            /*foreach(Entity victim in world.entities)
            {
                if(!victim.Equals(this) && getCollisionBox().Intersects(victim.getCollisionBox()))
                {
                    victim.damage(3, this, Vector2.Normalize(world.player.location - location) + new Vector2(0, -1));
                }
            }*/

            if (health <= 0 || Vector2.Distance(location, world.player.location) > 2000)
            {
                world.killEntity(this);
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
            batch.Draw(Game1.texture_entity_fire_glow, new Rectangle(defaultRect.X + offset.X - totalGlowSize / 2, defaultRect.Y + offset.Y + 25 - totalGlowSize / 2, totalGlowSize, totalGlowSize), Color.White);
            batch.Draw(currentTex, new Rectangle(defaultRect.X + offset.X - totalWidthOverTwo, defaultRect.Y + offset.Y - totalWidthOverTwo, totalWidth, totalWidth), Color.White);
        }

    }
}

