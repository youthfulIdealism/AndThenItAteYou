using Microsoft.Xna.Framework;
using Survive.Sound;
using Survive.SplashScreens;
using Survive.WorldManagement.Entities;
using Survive.WorldManagement.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Tile.Tags.OnUseTags
{
    public class Teleporter : TileTag
    {
        public override void onUse(WorldBase world, Item harvestTool, Vector2 location, TileType tileType, Entity user)
        {
            base.onUse(world, harvestTool, location, tileType, user);

            bool canProgress = true;

            if(Game1.isInTutorial)
            {
                SoundManager.getSound("teleporter_teleport").play(SoundType.AMBIENT);
                Game1.instance.returnToMainMenu();
                return;
            }

            //prevent progression if you've left the girl behind.
            foreach(Entity entity in world.entities)
            {
                if(entity is EntityGirl)
                {
                    if(Vector2.Distance(user.location, entity.location) > 100)
                    {
                        canProgress = false;
                    }
                    break;
                }
            }

            if(canProgress)
            {
                SoundManager.getSound("teleporter_teleport").play(SoundType.AMBIENT);
                Game1.instance.progress(world.difficulty);
            }
            

        }
    }
}
