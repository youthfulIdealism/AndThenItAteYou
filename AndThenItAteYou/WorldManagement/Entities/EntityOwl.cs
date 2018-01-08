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
    public class EntityOwl : Entity, AggroAble
    {

        const int texSwapPoint = 20;
        private int currentTexSwap = texSwapPoint;
        private int currentTexIndex;
        Vector2 lastPlayerLoc;
        Random random;
        Texture2D standTex;
        Texture2D attackTex;
        Texture2D retreatTex;
        Point drawOffset = new Point(-75, -75);
        bool aggro;

        int state = 0;
        const int STANDING = 0;
        const int ATTACKING = 1;
        const int RETREATING = 2;
        int randomAdditionalTime = 0;

        public EntityOwl(Vector2 location, WorldBase world) : base(location, world)
        {
            aggro = false;
            this.width = 150;
            this.height = 150;
            random = new Random();
            standTex = Game1.texture_entity_owl_stand[0];
            currentTexIndex = 0;
            attackTex = Game1.texture_entity_owl_attack;
            retreatTex = Game1.texture_entity_owl_retreat;
            health = 600;
            walkSpeed = .7f;
            this.gravityMultiplier = 0;
            windMultiplier = .1f;
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);

            currentTexSwap--;
            if (currentTexSwap <= 0)
            {
                currentTexIndex = (currentTexIndex + 1) % Game1.texture_entity_owl_stand.Length;
                standTex = Game1.texture_entity_owl_stand[currentTexIndex];
                currentTexSwap = texSwapPoint;
            }

            foreach (Entity entity in world.entities)
            {
                if (entity is Weapon && !aggro)
                {
                    if (Vector2.Distance(entity.location, this.location) <= 200)
                    {
                        spook();
                    }
                }
            }

            float distanceFromPlayer = Vector2.Distance(world.player.location, this.location);

            if (aggro)
            {
                //TODO: allow damaging of other entities
                if (world.player.getCollisionBox().Intersects(this.getCollisionBox()))
                {
                    Vector2 throwPlayerAmt = velocity;
                    if(throwPlayerAmt.Length() > 0) { throwPlayerAmt = Vector2.Normalize(velocity); }//ensure that player is thrown a real distance, instead of normalizing 0.
                    world.player.damage(25, this, (throwPlayerAmt + new Vector2(0, -2)) * 20f);
                }

                if (state == ATTACKING && (collideBottom || collideLeft || collideRight))
                {
                    world.shakeScreen(50, 400);
                    SoundManager.getSound("owl-attack").playWithVariance(0, 1f / distanceFromPlayer * 100, (location - world.player.location).X, SoundType.MONSTER);
                }




                if (collideBottom || collideLeft || collideRight/* || !isLocationAir*/)
                {
                    state = RETREATING;
                    randomAdditionalTime = rand.Next(4000);

                    for (int k = -1; k <= 1; k++)
                    {

                        Vector2 belowLoc = location + new Vector2(k * Chunk.tileDrawWidth, Chunk.tileDrawWidth * 2);
                        TileType belowBlock = world.getBlock(belowLoc);
                        if (belowBlock != null && !belowBlock.Equals(TileTypeReferencer.AIR))
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                world.addEntity(new ParticleTileBreak(belowLoc, world, new Vector2(Vector2.Normalize(velocity).X * 15, -10), belowBlock, 150));
                            }
                            world.placeTile(TileTypeReferencer.AIR, belowLoc);
                            SoundManager.getSound(belowBlock.blockBreakSound).playWithVariance(0, .25f, 0, SoundType.MONSTER);
                        }
                    }
                }
                else if (Vector2.Distance(this.location, world.player.location) >= 2000 + randomAdditionalTime)
                {
                    state = ATTACKING;
                }

                TileType currentBlock = world.getBlock(location);
                if (currentBlock != null)
                {
                    if (!currentBlock.Equals(TileTypeReferencer.AIR))
                    {
                        this.velocity *= .8f;
                        for (int i = 0; i < 10; i++)
                        {
                            world.addEntity(new ParticleTileBreak(location, world, new Vector2(Vector2.Normalize(velocity).X * 15, -10), currentBlock, 150));
                        }
                        world.placeTile(TileTypeReferencer.AIR, location);
                        SoundManager.getSound(currentBlock.blockBreakSound).playWithVariance(0, .25f, 0, SoundType.MONSTER);
                    }
                }

                if (state == RETREATING)
                {
                    float direction = 0;
                    if (world.player.location.X < this.location.X)
                    {
                        direction = 1;
                    }
                    else if (world.player.location.X > this.location.X)
                    {
                        direction = -1;
                    }
                    walk(direction);
                    this.impulse += new Vector2(direction, -2) * .3f;
                }
                else if (state == ATTACKING)
                {
                    this.impulse += Vector2.Normalize(world.player.location - location) * .235f;
                }
            }
            else
            {
                if (distanceFromPlayer <= 650 * world.player.detectionRadiousModifier)
                {

                    if (Vector2.Distance(world.player.location, lastPlayerLoc) > 4 && random.NextDouble() < .05f * world.player.detectionLevel)
                    {
                        spook();
                    }

                    lastPlayerLoc = world.player.location;
                }
            }
        }

        

        public void spook()
        {
            if (!aggro)
            {
                aggro = true;
            }
            state = RETREATING;
            randomAdditionalTime = rand.Next(4000);
            world.decorator.ambientSoundManager.startle();
        }

        public bool isAggrod()
        {
            return aggro;
        }

        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            //base.draw(batch, time, offset, Color.Green);

            Texture2D currentTex = null;
            if (aggro)
            {
                if(state == ATTACKING)
                {
                    currentTex = attackTex;
                }
                else if(state == RETREATING)
                {
                    currentTex = retreatTex;
                }
                
            }
            else
            {
                currentTex = standTex;
            }
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
