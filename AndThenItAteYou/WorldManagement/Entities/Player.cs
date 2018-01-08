using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Survive.Input;
using Survive.Input.ControlManagers;
using Survive.Sound;
using Survive.WorldManagement.Entities.Particles;
using Survive.WorldManagement.Entities.PlayerHelpers;
using Survive.WorldManagement.Entities.Progression;
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
using static Survive.Game1;

namespace Survive.WorldManagement.Entities
{
    public class Player : PlayerBase
    {

        public PlayerState state;
        public Texture2D selectedFrame;
        public PlayerAnimationPackage animationPackage;
        const int frameSwitchPoint = 5;
        Point drawOffset = new Point(-10, -15);
        public bool isRunning = false;

        public bool hasLeveledUpThisWorld;
        
        public GuitarControlManager guitarControlManager { get; set; }

        public Texture2D[] texture_run;
        public Texture2D[] texture_jump;
        public Texture2D[] texture_throw;
        public Texture2D[] texture_swing;
        public Texture2D texture_player_default_stand;


        bool wereFeetOnGround = false;
        Vector2 lastVelocity;
        float timePlayerWillNotGasp = 0;

        public const float maxTimeRemainingForSpawnParticles = 100;
        public float timeRemainingForSpawnParticles = maxTimeRemainingForSpawnParticles;
        public float facing;

        public Player(Vector2 location, WorldBase world) : base(location, world)
        {
            state = new State_Standing();
            state.enter(this);

            rand = new Random();
            width = 20;
            
            hunger = 100;
            health = 100;
            warmth = 100;

            craftingControlManager = new CraftingScreen(this);
            guitarControlManager = new GuitarControlManager(this);
            movementControlManger = new StandardPlayerControlManager(this);
            currentControlManager = movementControlManger;
            selectedFrame = texture_player_default_stand;

            hasLeveledUpThisWorld = false;
        }


        public override void prePhysicsUpdate(GameTime time)
        {
            base.prePhysicsUpdate(time);

            facing = velocity.X;

            state.update(this);

            if(timeRemainingForSpawnParticles > 0)
            {
                timeRemainingForSpawnParticles--;

                for(int i = 0; i < 3; i++)
                {
                    ParticleArbitrary particle = new ParticleArbitrary(new Vector2(location.X + rand.Next(10) - rand.Next(5), location.Y + rand.Next(20)), world, new Vector2((float)rand.NextDouble() - .5f, 0), 200, Game1.texture_particle_blood);
                    particle.width = 4;
                    particle.height = 4;
                    particle.gravityMultiplier = (float)rand.NextDouble() * -.4f;
                    particle.startColor = world.groundColor;
                    particle.endColor = world.groundColor;
                    world.addEntity(particle);
                }
            }

            /*if (collideBottom && isRunning)
            {
                frameSwitcher--;
                if (frameSwitcher <= 0)
                {
                    currentRunFrame = (currentRunFrame + 1) % texture_run.Length;
                    frameSwitcher = frameSwitchPoint;
                    selectedFrame = texture_run[currentRunFrame];
                }
            }
            else if (collideBottom)
            {
                selectedFrame = texture_player_default_stand;
                currentRunFrame = 0;
            }
            else
            {
                
                //currentJumpFrame = 0;
                //frameSwitcherJump
                if(velocity.Y > 0)
                {
                    frameSwitcherJump--;
                    if (frameSwitcherJump <= 0)
                    {
                        currentJumpFrame = Math.Min(currentJumpFrame + 1, texture_jump.Length - 1);
                        frameSwitcherJump = frameSwitchPoint;
                        selectedFrame = texture_jump[currentJumpFrame];
                    }
                }else
                {
                    currentJumpFrame = 0;
                }

                selectedFrame = texture_jump[currentJumpFrame];
            }*/

            //isRunning = false;

            selectedFrame = state.getTexture(this);

            timePlayerWillNotGasp--;

            if (wereFeetOnGround && !collideBottom)
            {
                bool playSlipSound = true;
                for(int i = 0; i < 9; i++)
                {
                    TileType tile = world.getBlock(location + new Vector2(0, i * Chunk.tileDrawWidth));
                    if (tile != null && tile.tags.Contains(TagReferencer.SOLID))
                    {
                        playSlipSound = false;
                        break;
                    }
                }

                if(playSlipSound && timePlayerWillNotGasp <= 0)
                {
                    timePlayerWillNotGasp = 700;
                    SoundManager.getSound("player-slip").playWithVariance(0, .2f, 0, SoundType.MONSTER);
                    for(int i = 0; i < 7; i++)
                    {
                        EntityDecorativeRock rock = new EntityDecorativeRock(location + new Vector2(0, 7), world);
                        rock.velocity += new Vector2(0, -rand.Next(7));
                        world.addEntity(rock);
                    }
                }
            }

            if (!wereFeetOnGround && collideBottom && lastVelocity.Y >= 14)
            {
                this.remainingDamageImmunityTime = 100;
                SoundManager.getSound("player-fall").playWithVariance(0, .45f, 0, SoundType.MONSTER);
            }

            if(timeNextToAFire >= 100)
            {
                timeNextToAFire = 100;
                if(currentControlManager != guitarControlManager)
                {
                    guitarControlManager.switchTo(currentControlManager);
                    currentControlManager = guitarControlManager;
                }
                
            }

            wereFeetOnGround = collideBottom;
            lastVelocity = velocity;
        }

        public override void walk(float directionAndVelocityAsPercentOfSpeed)
        {
            base.walk(directionAndVelocityAsPercentOfSpeed);
            isRunning = true;
        }

        public override void manageCold()
        {
            if (!currentControlManager.Equals(guitarControlManager))
            {
                base.manageCold();
            }
        }

        public override void manageHealth()
        {
            if (!currentControlManager.Equals(guitarControlManager))
            {
                base.manageHealth();
            }
        }

        public override void manageHunger()
        {
            if (!currentControlManager.Equals(guitarControlManager))
            {
                base.manageHunger();
            }
        }


        

        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            if(selectedFrame != null)
            {
                SpriteEffects effect = SpriteEffects.None;
                if (facing > 0)
                {
                    effect = SpriteEffects.FlipHorizontally;
                }

                //draw the player
                //Rectangle defaultRect = getCollisionBox().ToRect();
                //batch.Draw(Game1.block, new Rectangle(defaultRect.X + offset.X, defaultRect.Y + offset.Y, defaultRect.Width, defaultRect.Height), getDrawColor(groundColor, time));

                Rectangle defaultRect = getCollisionBox().ToRect();
                //batch.Draw(Game1.block, new Rectangle(defaultRect.X + offset.X, defaultRect.Y + offset.Y, defaultRect.Width, defaultRect.Height), Color.Red);
                batch.Draw(selectedFrame, new Rectangle(defaultRect.X + offset.X + drawOffset.X, defaultRect.Y + offset.Y + drawOffset.Y, selectedFrame.Width, selectedFrame.Height), null, getDrawColor(groundColor, time), 0, Vector2.Zero, effect, 0);
            }
            
        }
    }
}
