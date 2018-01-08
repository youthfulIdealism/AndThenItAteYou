using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
    public class EntityGuardian : Entity, AggroAble
    {

        static bool hasDroppedCraftingRecepie;
        const int texSwapPoint = 7;//15
        private int currentTexSwap = texSwapPoint;
        private int currentTexIndex;
        Vector2 lastPlayerLoc;
        Random random;
        Texture2D standTex;
        Texture2D runTex;
        Point drawOffset = new Point(-25, -5);
        bool aggro;
        int jumps = 3;
        SoundEffectInstance currentSound;
        float directionChangeSpeed = .1f;
        float currentDirection = 0;

        public EntityGuardian(Vector2 location, WorldBase world) : base(location, world)
        {
            aggro = false;
            this.width = 70;
            this.height = 60;
            random = new Random();
            standTex = Game1.texture_entity_guardian_stand[0];
            currentTexIndex = 0;
            runTex = Game1.texture_entity_guardian_run[0];
            health = 305;
            //walkSpeed = .7f;
            walkSpeed = .8f;
            //frictionMultiplier = .85f;
        }

        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);

            currentTexSwap--;
            if (currentTexSwap <= 0)
            {
                if(aggro)
                {
                    currentTexIndex = (currentTexIndex + 1) % Game1.texture_entity_guardian_run.Length;
                    runTex = Game1.texture_entity_guardian_run[currentTexIndex];
                    currentTexSwap = texSwapPoint;
                }
                else
                {
                    currentTexIndex = (currentTexIndex + 1) % Game1.texture_entity_guardian_stand.Length;
                    standTex = Game1.texture_entity_guardian_stand[currentTexIndex];
                    currentTexSwap = texSwapPoint;
                }
                
            }

            foreach (Entity entity in world.entities)
            {
                if (entity is Weapon)
                {
                    if (Vector2.Distance(entity.location, this.location) <= 100)
                    {
                        spook();
                    }
                }
            }

            float distanceFromPlayer = Vector2.Distance(world.player.location, this.location);
            if (distanceFromPlayer <= 450 * world.player.detectionRadiousModifier)
            {

                if (Vector2.Distance(world.player.location, lastPlayerLoc) > 4 && random.NextDouble() < .05f * world.player.detectionLevel)
                {
                    spook();
                }

                lastPlayerLoc = world.player.location;
            }

            if (aggro)
            {
                //TODO: allow damaging of other entities
                if (world.player.getCollisionBox().Intersects(this.getCollisionBox()))
                {
                    world.player.damage(10, this, Vector2.Normalize(world.player.location - location) + new Vector2(0, -1) * 5f);
                }


                if (collideBottom)
                {
                    //float direction = 0;
                    if (world.player.location.X < this.location.X)
                    {
                        //direction = -1;
                        currentDirection = Math.Max(-1, currentDirection - directionChangeSpeed);
                    }
                    else if (world.player.location.X > this.location.X)
                    {
                        //direction = 1;
                        currentDirection = Math.Min(1, currentDirection + directionChangeSpeed);
                    }
                    walk(currentDirection);
                }
                

                if(collideLeft || collideRight)
                {
                    TileType cliffTile = world.getBlock(location + new Vector2(Chunk.tileDrawWidth * (currentDirection > 0 ? 1 : -1), Chunk.tileDrawWidth * -3));
                    if(collideBottom)
                    {
                        if(cliffTile != null && !cliffTile.tags.Contains(TagReferencer.SOLID))
                        {
                            jumps--;
                            //impulse += new Vector2(0, -15);
                            jump(1);
                        }
                        else
                        {
                            aggro = false;
                            world.decorator.ambientSoundManager.unStartle();
                            currentDirection = 0;
                        }
                        
                    }
                    if(jumps <= 0)
                    {
                        aggro = false;
                        world.decorator.ambientSoundManager.unStartle();
                        jumps = 3;
                        currentDirection = 0;
                    }
                }
                else
                {
                    jumps = 3;
                }

                if(currentSound == null)
                {
                    if(rand.NextDouble() < .1f) { currentSound = SoundManager.getSound("guardian-snarl").playWithVariance(0, 1f / distanceFromPlayer * 75, (location - world.player.location).X, SoundType.MONSTER); }
                }else if(currentSound.IsDisposed)
                {
                    currentSound = null;
                }
            }

            if (health <= 0)
            {
                world.decorator.ambientSoundManager.unStartle();
                world.addEntity(new ItemDropEntity(location, world, new Item_Guardian_Fang(1)));
                if(!hasDroppedCraftingRecepie)
                {
                    Game1.instance.crafting.registerRecepie(new CraftingRecepie(new Item_Spear_Fanged(1), .4f, new Item[] { new Item_Spear(1), new Item_Guardian_Fang(1) }, new int[] { 1, 1 }));
                    hasDroppedCraftingRecepie = true;
                }
            }
        }

        

        public void spook()
        {
            if (!aggro)
            {
                world.decorator.ambientSoundManager.startle();
                aggro = true;
            }
        }

        public bool isAggrod()
        {
            return aggro;
        }

        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            //base.draw(batch, time, offset);

            Texture2D currentTex = null;
            if (aggro)
            {
                currentTex = runTex;
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
