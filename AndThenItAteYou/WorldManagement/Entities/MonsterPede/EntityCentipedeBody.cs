using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Survive.Sound;
using Survive.WorldManagement.Entities.Projectiles;
using Survive.WorldManagement.Inventory;
using Survive.WorldManagement.Inventory.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities
{
    public class EntityCentipedeBody : Entity
    {
        const int texSwapPoint = 7;
        int texSwapCounter;
        public int currentTex;
        Vector2 rotationVector;
        Texture2D tex;
        Point drawOffset = new Point(45, 0);
        Entity parent;

        public EntityCentipedeBody(Vector2 location, WorldBase world, Entity parent) : base(location, world)
        {
            this.width = 30;
            this.height = 30;
            currentTex = 0;
            tex = Game1.texture_monsterpede_body[0];
            health = 150;
            this.walkSpeed = .2f;
            this.parent = parent;
            this.gravityMultiplier = 0;
        }

        public override void performPhysics(GameTime time)
        {

        }

        public override void prePhysicsUpdate(GameTime time)
        {
            texSwapCounter++;
            if(texSwapCounter >= texSwapPoint)
            {
                texSwapCounter = 0;
                currentTex = (currentTex + 1) % Game1.texture_monsterpede_body.Length;
                tex = Game1.texture_monsterpede_body[currentTex];
            }

            Vector2 pullVector = Vector2.Normalize(parent.location - location);
            if (Vector2.Distance(parent.location, location) > width)
            {
                location = parent.location - pullVector * width;
                rotationVector = pullVector;
            }

            if(parent.health <= 0 || !world.entities.Contains(parent))
            {
                world.killEntity(this);
            }
        }

        public override void damage(float amt, Entity source, Vector2 force)
        {
            parent.damage(amt, source, force);
        }

        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            //base.draw(batch, time, offset, Color.Red);

            /*Texture2D currentTex = null;
            if (aggro)
            {
                if(timeSpentAiming > 0)
                {
                    currentTex = aimTex;
                }
                else
                {
                    currentTex = runTex;
                }
                
            }
            else
            {
                currentTex = standTex;
            }*/
            SpriteEffects effect = SpriteEffects.None;
            if (rotationVector.X > 0)
            {
                effect = SpriteEffects.FlipHorizontally;
            }


            Rectangle defaultRect = getCollisionBox().ToRect();
            //batch.Draw(tex, new Rectangle(defaultRect.X + offset.X + drawOffset.X, defaultRect.Y + offset.Y + drawOffset.Y, tex.Width, tex.Height), null, getDrawColor(groundColor, time), 0, Vector2.Zero, effect, 0);
            batch.Draw(tex, new Rectangle(defaultRect.X + offset.X, defaultRect.Y + offset.Y, (int)tex.Width, (int)tex.Height), null, groundColor, (float)Math.Atan(rotationVector.Y / rotationVector.X), new Vector2(tex.Width / 2, tex.Height / 2), effect, 0);
        }
    }
}
