using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Survive.Input;
using Survive.Sound;
using Survive.WorldManagement.Entities.Particles;
using Survive.WorldManagement.Entities.Progression;
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
using static Survive.SplashScreens.PlayerSelectScreen;

namespace Survive.WorldManagement.Entities
{
    public class Survivor : UsableEntity
    {
        public Texture2D selectedFrame;
        Point drawOffset = new Point(-10, -15);
        PlayerStarterKit kit;

        public Survivor(Vector2 location, WorldBase world, PlayerStarterKit kit) : base(location, world)
        {
            this.kit = kit;
            width = 20;
            selectedFrame = kit.animations.standTex;
        }

        public override void damage(float amt, Entity source, Vector2 force)
        {
            //base.damage(amt, source, force);
        }
        

        public override void draw(SpriteBatch batch, GameTime time, Point offset, Color groundColor)
        {
            SpriteEffects effect = SpriteEffects.None;
            if (velocity.X > 0)
            {
                effect = SpriteEffects.FlipHorizontally;
            }

            //draw the player
            //Rectangle defaultRect = getCollisionBox().ToRect();
            //batch.Draw(Game1.block, new Rectangle(defaultRect.X + offset.X, defaultRect.Y + offset.Y, defaultRect.Width, defaultRect.Height), getDrawColor(groundColor, time));

            Rectangle defaultRect = getCollisionBox().ToRect();
            //batch.Draw(Game1.block, new Rectangle(defaultRect.X + offset.X, defaultRect.Y + offset.Y, defaultRect.Width, defaultRect.Height), Color.Red);
            batch.Draw(selectedFrame, new Rectangle(defaultRect.X + offset.X + drawOffset.X, defaultRect.Y + offset.Y + drawOffset.Y, selectedFrame.Width, selectedFrame.Height), null, getDrawColor(groundColor, time), 0, Vector2.Zero, effect, 0);
            batch.DrawString(Game1.gamefont_24, "E", location + offset.ToVector2() + new Vector2(0, -25 + (float)(Math.Sin(time.TotalGameTime.Milliseconds * .005f)) * 5), Color.White);
        }

        public override AABB getUseBounds()
        {
            return getCollisionBox();
        }

        public override void use(WorldBase world, Vector2 location, Entity user)
        {
            MetaData.unlocks.Add(kit.id);
            world.killEntity(this);
        }
    }
}
