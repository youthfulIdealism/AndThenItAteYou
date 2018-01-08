using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Survive.Input;
using Survive.Input.InputManagers;
using Survive.WorldManagement.Entities;
using Survive.WorldManagement.Entities.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Inventory.Items
{
    public class Item_Scizors : Item
    {

        public Item_Scizors(int uses) : base(uses)
        {
            if(texture == null)
            {
                texture = Game1.texture_item_scizors;
            }
        }

        public override Item clone(int uses)
        {
            return new Item_Scizors(uses);
        }

        public override int use(PlayerBase user, WorldBase world, Vector2 location, GameTime time, BinaryInputManager inputManager)
        {
            base.use(user, world, location, time, inputManager);

            bool showUse = true;

            foreach (UsableEntity ue in user.world.useableEntities)
            {
                if (ue is EntityRopeSegment && ue.getUseBounds().Intersects(user.getCollisionBox()))
                {
                    EntityRopeSegment currentRopeSegment = (EntityRopeSegment)ue;
                    int numRecoveredRopes = 0;
                    while(currentRopeSegment.child != null)
                    {
                        world.killEntity(currentRopeSegment);
                        numRecoveredRopes++;
                        currentRopeSegment = currentRopeSegment.child;
                    }
                    world.killEntity(currentRopeSegment);
                    world.addEntity(new ItemDropEntity(user.location, world, new Item_Rope((int)((float)numRecoveredRopes * .75f))));

                    break;
                }

                if (ue is EntityBetterRopeSegment && ue.getUseBounds().Intersects(user.getCollisionBox()))
                {
                    showUse = false;
                    EntityBetterRopeSegment currentRopeSegment = (EntityBetterRopeSegment)ue;
                    int numRecoveredRopes = 0;
                    while (currentRopeSegment.child != null)
                    {
                        world.killEntity(currentRopeSegment);
                        numRecoveredRopes++;
                        currentRopeSegment = currentRopeSegment.child;
                    }

                    currentRopeSegment = (EntityBetterRopeSegment)ue;
                    while (currentRopeSegment.parent != null)
                    {
                        world.killEntity(currentRopeSegment);
                        numRecoveredRopes++;
                        currentRopeSegment = currentRopeSegment.parent;
                    }

                    world.killEntity(currentRopeSegment);
                    world.addEntity(new ItemDropEntity(user.location, world, new Item_Rope((int)((float)numRecoveredRopes * .75f))));

                    break;
                }
            }

            if(showUse)
            {
                user.speechManager.addSpeechBubble(Game1.texture_item_rope);
            }

            return 0;
        }
    }
}
