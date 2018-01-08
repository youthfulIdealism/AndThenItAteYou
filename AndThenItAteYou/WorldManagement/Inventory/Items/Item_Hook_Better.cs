using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Survive.WorldManagement.Entities;
using Survive.WorldManagement.Entities.Projectiles;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Survive.WorldManagement.Tile;
using Survive.WorldManagement.Tile.Tags;
using Survive.Sound;
using Survive.Input;
using Survive.Input.InputManagers;
using Survive.WorldManagement.Entities.Speech;

namespace Survive.WorldManagement.Inventory.Items
{
    public class Item_Hook_Better : Item
    {
        bool drawing;
        bool hookWouldBeAnchored = false;
        int hookWouldBeAnchoredAt = 0;
        int ropeThatWouldBeConsumed = 0;
        int ropeSegmentsInGreen = 0;
        public List<Vector2> ropeIntermediaryPoints;
        Vector2 prevOrigin;
        Vector2 prevTarget;

        public Item_Hook_Better(int uses) : base(uses)
        {
            usesStandardControlScheme = false;
            if (texture == null)
            {
                texture = Game1.texture_item_grappling_hook;
            }
            drawing = false;
            ropeIntermediaryPoints = new List<Vector2>();
            prevOrigin = new Vector2();
            prevTarget = new Vector2();
        }

        public override Item clone(int uses)
        {
            return new Item_Hook_Better(uses);
        }

        public override int use(PlayerBase user, WorldBase world, Vector2 location, GameTime time, BinaryInputManager inputManager)
        {
            base.use(user, world, location, time, inputManager);

            Item_Rope item_rope = (Item_Rope)user.inventory.getItemOfType(new Item_Rope(1));
            

            if (!prevOrigin.Equals(user.location) || !prevTarget.Equals(location))
            {
                //recalculate the grappling hook trajectory
                hookWouldBeAnchored = false;
                ropeIntermediaryPoints.Clear();
                hookWouldBeAnchoredAt = 0;
                ropeThatWouldBeConsumed = 0;
                if (item_rope != null)
                {
                    Vector2 position = user.location + new Vector2(0, -5);
                    Vector2 velocity = Vector2.Normalize(location - user.location) * 25;
                    float ropeIncrementDistance = 2.5f;

                    ropeIntermediaryPoints.Add(position);
                    int ropeCount = item_rope.uses;
                    for (int i = 0; i < ropeCount * 2 + 10; i++)
                    {
                        TileType tileIn = world.getBlock(position);
                        if (tileIn != null)
                        {
                            velocity += new Vector2(0, .5f) * ropeIncrementDistance;
                            velocity *= .975f;
                            position += velocity * ropeIncrementDistance;

                            ropeIntermediaryPoints.Add(position);

                            if (tileIn.tags.Contains(TagReferencer.SOLID) && !hookWouldBeAnchored)
                            {
                                hookWouldBeAnchored = true;
                                hookWouldBeAnchoredAt = i + 1;
                                ropeThatWouldBeConsumed = hookWouldBeAnchoredAt / 2;
                                ropeSegmentsInGreen = ropeCount * 2;
                            }
                        }
                        else
                        {
                            //the rope has extended off the map; simply stop calculating.
                            break;
                        }


                    }
                }
            }



            if (!inputManager.isDown() && inputManager.wasDown() && item_rope != null)
            {
                if (hookWouldBeAnchored && ropeThatWouldBeConsumed <= item_rope.uses)
                {
                    EntityBetterRopeSegment previousRopeSegment = new EntityBetterRopeSegment(ropeIntermediaryPoints[0], world, null);
                    previousRopeSegment.isAnchor = true;
                    world.addEntity(previousRopeSegment);
                    EntityBetterRopeSegment nthRopeSegment = null;
                    for (int i = 1; i < hookWouldBeAnchoredAt; i++)
                    {
                        nthRopeSegment = new EntityBetterRopeSegment(ropeIntermediaryPoints[i], world, previousRopeSegment);
                        previousRopeSegment.child = nthRopeSegment;
                        world.addEntity(nthRopeSegment);
                        previousRopeSegment = nthRopeSegment;
                    }
                    previousRopeSegment.isAnchor = true;
                    user.inventory.consume(item_rope, ropeThatWouldBeConsumed);

                    SoundManager.getSound("grappling-hook").playWithVariance(0, .2f, 0, SoundType.MONSTER);
                }
                else
                {
                    //do a flash?
                }
            }
            drawing = inputManager.isDown();

            if(inputManager.isDown() && !inputManager.wasDown() && item_rope == null) { user.speechManager.addSpeechBubble(new SpeechBubble(Game1.texture_item_rope)); }

            prevOrigin = user.location;
            prevTarget = location;
            return 0;
        }

        public override void draw(SpriteBatch batch, PlayerBase user, Point offset, Color groundColor)
        {
            if(drawing)
            {
                Item_Rope item_rope = (Item_Rope)user.inventory.getItemOfType(new Item_Rope(1));
                if (item_rope != null)
                {
                    Color usedColor = Color.Red;
                    if (hookWouldBeAnchored && ropeThatWouldBeConsumed <= item_rope.uses)
                    {
                        usedColor = Color.Green;
                    }
                    else if (hookWouldBeAnchored && ropeThatWouldBeConsumed > item_rope.uses)
                    {
                        usedColor = Color.Yellow;
                    }
                    int countTo = ropeIntermediaryPoints.Count;
                    if(hookWouldBeAnchored) { countTo = Math.Min(hookWouldBeAnchoredAt, ropeIntermediaryPoints.Count); }
                    for (int i = 1; i < countTo; i++)
                    {
                        if(i > ropeSegmentsInGreen)
                        {
                            usedColor = Color.Red;
                        }
                        Game1.DrawLine(batch, ropeIntermediaryPoints[i] + offset.ToVector2(), ropeIntermediaryPoints[i - 1] + offset.ToVector2(),  usedColor);
                    }
                    Vector2 drawLoc = user.location + offset.ToVector2();
                    batch.Draw(Game1.texture_item_rope, new Rectangle((int)drawLoc.X, (int)drawLoc.Y - 20, Game1.texture_item_rope.Width, Game1.texture_item_rope.Height), usedColor);
                    batch.DrawString(Game1.gamefont_24, Game1.decimalToBase6(hookWouldBeAnchoredAt / 2) + "", user.location + offset.ToVector2() + new Vector2(25, -20), usedColor);
                }
               
            }
        }
    }
}