using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Survive.Input;
using Survive.Worldgen;
using Survive.WorldManagement.ContentProcessors;
using Survive.WorldManagement.Entities;
using Survive.WorldManagement.Inventory;
using Survive.WorldManagement.Inventory.Items;
using Survive.WorldManagement.Procedurals;
using Survive.WorldManagement.Tile;
using Survive.WorldManagement.Weather;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Survive.WorldManagement
{
    public class WorldFromDisk : WorldBase
    {
        public float timeIncrement = .0001f;

        public int sunAxis;
        public float windStrength;

        private List<Cloud> clouds;
        private List<Cloud> removedClouds;

        
        public int teleporterChunkLoc;


        public WorldFromDisk(String path, int difficulty) : base(difficulty)
        {
            this.worldGenType = WorldGenType.LOAD;
            
            decorator.generateMapTypeDependentDecorations();

            
            worldReader = new WorldReader(path);
            teleporterChunkLoc = difficulty * 2 + 10;

            //set time of day, wind strength, etc
            timeOfDay = .6f;
            windStrength = (float)(rand.NextDouble() * .1f) - .05f;

            //set up x-location of sun in sky
            sunAxis = (int)((Game1.instance.graphics.PreferredBackBufferWidth * .1f) + rand.Next((int)(Game1.instance.graphics.PreferredBackBufferWidth * .8f)));

            //set up clouds
            clouds = new List<Cloud>();
            removedClouds = new List<Cloud>();
            for (int i = 0; i < 6; i++)
            {
                int startLoc = rand.Next(Game1.instance.graphics.PreferredBackBufferWidth);
                int cloudY = rand.Next(Game1.instance.graphics.PreferredBackBufferHeight) - 300;
                int cloudWidth = 600 + rand.Next(100);
                int cloudHeight = 400 + rand.Next(50);
                int subClouds = 3 + rand.Next(6);
                for (int k = 0; k < subClouds; k++)
                {
                    AABB newCloudBounds = new AABB(startLoc + rand.Next(cloudWidth), cloudY + rand.Next(cloudHeight), cloudWidth / 2, cloudHeight / 2);
                    clouds.Add(new Cloud(newCloudBounds, rand));
                }
            }

            //put player in world
            player = new Player(new Vector2(), this);
            entities.Add(player);

            groundColor = decorator.colorManager.groundColor;
        }


        public override void update(GameTime time)
        {
            base.update(time);

            //increase time of day. If time of day wraps over into night, reset it, because 0 is midnight.
            timeOfDay += timeIncrement;
            if(timeOfDay >= 2)
            {
                timeOfDay = 0;
            }

            //allow wind to shift TODO: make wind shift + effects govorned by world decorator
            if(rand.NextDouble() < .001f)
            {
                windStrength += (float)(rand.NextDouble() * .1f) - .05f;
            }

            //on random chance, generate a new cloud.
            if (rand.NextDouble() < .002f)
            {
                int startLoc = 0;
                if(windStrength > 0)
                {
                    startLoc = -300;
                }else
                {
                    startLoc = Game1.instance.graphics.PreferredBackBufferWidth - 300;
                }

                int cloudY = rand.Next(Game1.instance.graphics.PreferredBackBufferHeight) + 300;
                int cloudWidth = 600 + rand.Next(100);
                int cloudHeight = 400 + rand.Next(50);
                int subClouds = 3 + rand.Next(6);
                for(int i = 0; i < subClouds; i++)
                {
                    AABB newCloudBounds = new AABB(startLoc + rand.Next(cloudWidth), cloudY + rand.Next(cloudHeight), cloudWidth / 2, cloudHeight / 2);
                    clouds.Add(new Cloud(newCloudBounds, rand));
                }
            }

            
            foreach(Cloud cloud in clouds)
            {
                cloud.shift(windStrength);
                if (windStrength > 0)
                {
                    if(cloud.bounds.X > Game1.instance.graphics.PreferredBackBufferWidth)
                    {
                        removedClouds.Add(cloud);
                    }
                }
                else if(cloud.bounds.X < 0)
                {
                    removedClouds.Add(cloud);
                }
            }
            foreach (Cloud cloud in removedClouds)
            {
                clouds.Remove(cloud);
            }
            removedClouds.Clear();

           

            performChunkManagement(time);



            


            
        }


        public override void onDeQueueChunk(Chunk chunk)
        {
            base.onDeQueueChunk(chunk);

            
        }

        /**
            Perform a crude fast forward of the world. Intended for use if time goes by from, for example, crafting.
        */
        public override void fastForward(float timeAsPercentageOfDay)
        {
            base.fastForward(timeAsPercentageOfDay);
            timeOfDay += timeAsPercentageOfDay;
            if (timeOfDay >= 2)
            {
                timeOfDay = 0;
            }

            foreach (Entity entity in entities)
            {
                entity.fastForward(timeAsPercentageOfDay);
            }
        }

        public override float getCurrentTemperature()
        {
            if(timeOfDay < 1)
            {
                return decorator.nightTemp * (1 - timeOfDay) + decorator.dayTemp * timeOfDay;
            }
            else if(timeOfDay < 2)
            {
                return decorator.dayTemp * (2 - timeOfDay) + decorator.nightTemp * (timeOfDay - 1);
            }
            return 0;
        }



        public override AABB tryMove(Entity entity, AABB destination)
        {
            AABB currentEntityRect = entity.getCollisionBox();

            AABB movementRect = AABB.Union(currentEntityRect, destination);


            foreach (Chunk chunk in chunks.Values)
            {
                //check to ensure that the chunk is close enough to entity to affect physics. If not, skip the chunk.
                if (movementRect.Intersects(chunk.totalBox))
                {
                    //iterate through the collision boxes, to see if any of them affect physics.
                    foreach (Rectangle chunkRect in chunk.collisionBoxes)
                    {
                        AABB intersection = AABB.Intersect(movementRect, chunkRect);
                        //if the movement box consumes by the entity's location... TODO: add reverse-contains for aabb
                        if (chunkRect.Contains(destination.ToRect()))
                        {
                            destination = new AABB(destination.X, chunkRect.Y - destination.Height, destination.Width, destination.Height);
                        }
                        else if (movementRect.Intersects(chunkRect))//if the collision box intersects the entity's intended location, ...
                        {
                            //AABB intersection = AABB.Intersect(movementRect, chunkRect);



                            if (intersection.Height > intersection.Width)
                            {

                                if (destination.Center.X > intersection.Center.X)
                                {
                                    entity.collideRight = true;
                                    destination = new AABB(intersection.Right, destination.Y, destination.Width, destination.Height);



                                }
                                else
                                {
                                    entity.collideLeft = true;
                                    destination = new AABB(intersection.Left - destination.Width, destination.Y, destination.Width, destination.Height);




                                }



                            }
                            else//if(interection.Height < interection.Width)
                            {


                                if (destination.Center.Y > intersection.Center.Y)
                                {
                                    entity.collideTop = true;
                                    destination = new AABB(destination.X, intersection.Y + intersection.Height, destination.Width, destination.Height);



                                }
                                else
                                {
                                    entity.collideBottom = true;
                                    destination = new AABB(destination.X, intersection.Y - destination.Height, destination.Width, destination.Height);




                                }



                            }

                            movementRect = AABB.Union(currentEntityRect, destination);














































                        }
                    }
                }
            }

            /*foreach (Chunk chunk in chunks.Values)
            {
                //check to ensure that the chunk is close enough to entity to affect physics. If not, skip the chunk.
                if (movementRect.Intersects(chunk.totalBox))
                {
                    //iterate through the collision boxes, to see if any of them affect physics.
                    foreach (Rectangle chunkRect in chunk.collisionBoxes)
                    {
                        //if the movement box consumes by the entity's location... TODO: add reverse-contains for aabb
                        if(chunkRect.Contains(attemptedDestination.ToRect()))
                        {
                            attemptedDestination.Y -= attemptedDestination.Bottom - attemptedDestination.Top;
                            return attemptedDestination;
                        }
                        else if (movementRect.Intersects(chunkRect))//if the collision box intersects the entity's intended location, ...
                        {
                            //find the intersection rectangle. This will define how the entity is redirected.
                            AABB intersectionRectangle = AABB.Intersect(movementRect, chunkRect);

                            //if the intersection rectangle's width is significantly greater than its height, the entity is colliding on the top or the bottom.
                            if (intersectionRectangle.Width > intersectionRectangle.Height + 3)//the + 3 helps handle an edge case that keeps tall entities from climbing cliffs.
                            {
                                //if (intersectionRectangle.Y < attemptedDestination.Center.Y)
                                if (intersectionRectangle.Y < attemptedDestination.Center.Y)
                                {
                                    entity.collideTop = true;
                                    attemptedDestination.Y += intersectionRectangle.Height;
                                    movementRect = AABB.Union(currentEntityRect, attemptedDestination);
                                }
                                //else if (intersectionRectangle.Center.Y > attemptedDestination.Center.Y)
                                else if (intersectionRectangle.Bottom > movementRect.Top)
                                {
                                    entity.collideBottom = true;
                                    //attemptedDestination.Y -= movementRect.Bottom - intersectionRectangle.Top;
                                    attemptedDestination.Y -= intersectionRectangle.Height;
                                    movementRect = AABB.Union(currentEntityRect, attemptedDestination);
                                }
                            }
                            //otherwise, if the intersection rectangle's height is significantly greater than its width, the entity is colliding on the left or right.
                            else if (intersectionRectangle.Width < intersectionRectangle.Height )
                            {
                                if (intersectionRectangle.Center.X > attemptedDestination.Center.X)
                                {
                                    entity.collideLeft = true;
                                    attemptedDestination.X -= attemptedDestination.Right - intersectionRectangle.Left;
                                    movementRect = AABB.Union(currentEntityRect, attemptedDestination);
                                }
                                else if (intersectionRectangle.Center.X < attemptedDestination.Center.X)
                                {
                                    entity.collideRight = true;
                                    attemptedDestination.X -= attemptedDestination.Left - intersectionRectangle.Right;
                                    movementRect = AABB.Union(currentEntityRect, attemptedDestination);
                                }
                            }
                        }
                    }
                }


            }*/

            return destination;
        }

        public override void draw(SpriteBatch batch, GameTime time)
        {
            Color skyColor = decorator.colorManager.getSkyColorGivenTimeOfDay(timeOfDay);

            batch.Draw(Game1.block, new Rectangle(0, 0, Game1.instance.graphics.PreferredBackBufferWidth, Game1.instance.graphics.PreferredBackBufferHeight), skyColor);

            int sunY = (int)(Game1.instance.graphics.PreferredBackBufferHeight - Game1.instance.graphics.PreferredBackBufferHeight * (timeOfDay));
            batch.Draw(Game1.texture_sun, new Rectangle(sunAxis, sunY, 300, 300), Color.White);

            float starRenderAmt = 0;
            if (timeOfDay < 1)
            {
                starRenderAmt =  -(1 - timeOfDay) + timeOfDay;
            }
            else if (timeOfDay < 2)
            {
                starRenderAmt =  (2 - timeOfDay) + -(timeOfDay - 1);
            }
            starRenderAmt = 1 - starRenderAmt;
            starRenderAmt *= starRenderAmt * starRenderAmt;
            batch.Draw(Game1.texture_sky, new Rectangle(0, 0, Game1.instance.graphics.PreferredBackBufferWidth, Game1.instance.graphics.PreferredBackBufferHeight), Color.White * starRenderAmt);

            Color backCloudColor = decorator.colorManager.getBackCloudColorGivenTimeOfDay(timeOfDay);
            Color frontCloudColor = decorator.colorManager.getFrontCloudColorGivenTimeOfDay(timeOfDay);
            int cloudBorderSize = 7;

            foreach (Cloud cloud in clouds)
            {
                Rectangle innerDrawRect = cloud.bounds.ToRect();
                innerDrawRect.Inflate(-cloudBorderSize / 2, -cloudBorderSize);
                innerDrawRect.X += (int)((innerDrawRect.X - sunAxis) * .05f);
                innerDrawRect.Y += (int)Math.Max(-cloudBorderSize, Math.Min(cloudBorderSize, (((float)(innerDrawRect.Y - sunY)))));

                batch.Draw(cloud.textureFront, cloud.bounds.ToRect(), backCloudColor);
                batch.Draw(cloud.textureFront, innerDrawRect, frontCloudColor);
            }

            base.draw(batch, time);
            delayedRender(batch, time);
        }

        public override void Dispose()
        {
            base.Dispose();
            decorator.Dispose();
        }
    }
}
