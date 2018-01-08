using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Survive.WorldManagement.Inventory;
using Survive.WorldManagement.Inventory.Items;
using Survive.WorldManagement.Tile;
using Survive.WorldManagement.Tile.ExtensionTileTypes;
using Survive.WorldManagement.Tile.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Procedurals
{
    public class TreeManager : IDisposable
    {
        public TileType trunk;
        public TileType treeTop;
        public TileType leaves;
        public TileType trunk2;
        public TileType treeTop2;
        public TileType leaves2;

        public TreeManager(ChunkDecorator decorator)
        {
            if(decorator.worldGenSubtype != World.WorldGenSubtype.CENOTE)
            {
                //build leaves
                Texture2D leafTexOne = decorator.content.Load<Texture2D>("Blocks/Variety/" + decorator.rand.Next(50));
                Texture2D leafTexTwo = decorator.content.Load<Texture2D>("Blocks/Variety/" + decorator.rand.Next(50));

                leaves = new TileType(new TileTag[] { TagReferencer.AIR, TagReferencer.Harvest, }, leafTexOne, false);
                leaves.harvestTicks = 5 + getRandValuePlusOrMinus(decorator.rand, Math.Max(decorator.metaDifficulty, 1));
                leaves2 = new TileType(new TileTag[] { TagReferencer.AIR, TagReferencer.Harvest, }, leafTexTwo, false);
                leaves2.harvestTicks = 5 + getRandValuePlusOrMinus(decorator.rand, Math.Max(decorator.metaDifficulty, 1));


                //build trunks
                generateTreeTrunk(decorator, out trunk, out treeTop);
                generateTreeTrunk(decorator, out trunk2, out treeTop2);
                decorator.foliage.AddRange(generateTrees(decorator, trunk, treeTop, leaves));
                decorator.foliageBiome2.AddRange(generateTrees(decorator, trunk2, treeTop2, leaves2));

                TileType.replaceTileInDictionary(TileTypeReferencer.REACTIVE_LEAVES_0.TILEID, leaves);
                TileType.replaceTileInDictionary(TileTypeReferencer.REACTIVE_LEAVES_1.TILEID, leaves2);
                TileType.replaceTileInDictionary(TileTypeReferencer.REACTIVE_TRUNK_0.TILEID, trunk);
                TileType.replaceTileInDictionary(TileTypeReferencer.REACTIVE_TRUNK_1.TILEID, trunk2);

                ItemDropper dropSticks = new ItemDropper();
                dropSticks.registerNewDrop(new Item_Stick(0), null, 1, .1f);
                dropSticks.registerNewDrop(new Item_Stick(0), new Item_Axe(1), 1, .2f);

                if(decorator.metaDifficulty == 0)
                {
                    dropSticks.registerNewDrop(new Item_Stick(0), null, 1, .4f);
                    dropSticks.registerNewDrop(new Item_Stick(0), new Item_Axe(1), 1, .4f);
                }

                HarvestDictionary.registerNewHarvestWithGhost(trunk, new ItemDropper[] { dropSticks });
                HarvestDictionary.registerNewHarvestWithGhost(trunk2, new ItemDropper[] { dropSticks });

            }
            else
            {
                decorator.foliage.Add(new Decoration(new Point(0, -6), new TileType[,] { { TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER } }));
                decorator.foliage.Add(new Decoration(new Point(0, -9), new TileType[,] { { TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER } }));
                decorator.foliage.Add(new Decoration(new Point(0, 6), new TileType[,] { { TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER } }));
                decorator.foliage.Add(new Decoration(new Point(0, 9), new TileType[,] { { TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER, TileTypeReferencer.LADDER } }));

                for(int i = 0; i < 3; i++)
                {
                    decorator.foliage.Add(new Decoration(new Point(0, 2), new TileType[,] { { TileTypeReferencer.STALAGMITE_TOP, TileTypeReferencer.STALAGMITE_MIDDLE, TileTypeReferencer.STALAGMITE_BOTTOM  } }));
                    decorator.foliageBiome2.Add(new Decoration(new Point(0, 2), new TileType[,] { { TileTypeReferencer.STALAGMITE_TOP, TileTypeReferencer.STALAGMITE_MIDDLE, TileTypeReferencer.STALAGMITE_BOTTOM } }));
                }
            }
            
        }

        private void generateTreeTrunk(ChunkDecorator decorator, out TileType treeTrunk, out TileType treeTop)
        {
            //we want to generate an appropriate variety of trunk images.
            int numGeneratedTrunks = 9;

            //set up the texture array and spritebatch
            RenderTarget2D[] trunks = new RenderTarget2D[numGeneratedTrunks];
            RenderTarget2D[] treeTops = new RenderTarget2D[numGeneratedTrunks];
            SpriteBatch batch = new SpriteBatch(Game1.instance.GraphicsDevice);

            //numPrimitives represents the number of different "basic shapes" used to generate the tree trunk.
            int numPrimitives = 1 + decorator.rand.Next(2);
            //each primitive gets N different "strings" of drawings.
            int[] branches = new int[numPrimitives];
            //each primitive gets to move N distance each pass along its "string".
            float[] jumpHeight = new float[numPrimitives];
            //each primitive can change direction N amount each pass along its string.
            float[] rotationRange = new float[numPrimitives];
            //each primitive can start rotated an arbitrary amount.
            float[] startDisplayRotation = new float[numPrimitives];
            float[] displayRotationRange = new float[numPrimitives];

            Texture2D[] primitives = new Texture2D[numPrimitives];

            for (int i = 0; i < numPrimitives; i++)
            {
                //select a random primitive texture
                primitives[i] = Game1.instance.primitives[decorator.rand.Next(Game1.instance.primitives.Count)];
                //select a random number of times for the primitive to draw a string on the texture.
                branches[i] = 2 + decorator.rand.Next(3);
                //select a random distance for the primitive to jump through each pass.
                jumpHeight[i] = 7 + decorator.rand.Next(5);
                //select a random rotation for each primitive to use on each pass.
                rotationRange[i] = (float)(decorator.rand.NextDouble() / 2);
                //select a random display rotation for each primitive to use
                startDisplayRotation[i] = (float)(decorator.rand.NextDouble() * Math.PI * 2);
                displayRotationRange[i] = (float)(decorator.rand.NextDouble() * Math.PI * 2);
            }

            //make the texture size a bit bigger than the normal block, so that
            //the generation has room to work without ugly cut-offs on the final texture.
            int texSizeTrunk = (int)(Chunk.tileDrawWidth * 1.5);
            for (int i = 0; i < numGeneratedTrunks; i++)
            {

                RenderTarget2D trunkImages = new RenderTarget2D(
                   Game1.instance.GraphicsDevice,
                   texSizeTrunk,
                   texSizeTrunk,
                   false,
                   Game1.instance.GraphicsDevice.PresentationParameters.BackBufferFormat,
                   DepthFormat.Depth24);

                
                Game1.instance.GraphicsDevice.SetRenderTarget(trunkImages);
                Game1.instance.GraphicsDevice.Clear(Color.Transparent);
                batch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

                for (int k = 0; k < numPrimitives; k++)
                {
                    for (int j = 0; j < branches[k]; j++)
                    {
                        //set up the start position and rotations.
                        Vector2 lastLocation = new Vector2((texSizeTrunk * .66f) - decorator.rand.Next((int)(texSizeTrunk * .33f)), 10);
                        float lastRotation = (float)((Math.PI * -1.5) + decorator.rand.NextDouble() * rotationRange[k] - rotationRange[k] / 2); //turns out that this number is more-or-less vertically aligned for some reason.

                        //stop drawing when we've reached the other side of the texture
                        while (lastLocation.Y < texSizeTrunk - 15)
                        {
                            //find the rotation location and rotation after a jump
                            float nextRotation = lastRotation + (float)(decorator.rand.NextDouble() * rotationRange[k] - rotationRange[k] / 2);
                            Vector2 nextLocation = lastLocation + vectorFromAngle(nextRotation) * jumpHeight[k];

                            //50% chance to flip the sprite
                            SpriteEffects effect = SpriteEffects.None;
                            if (decorator.rand.NextDouble() < .5f) { effect = SpriteEffects.FlipHorizontally; }

                            //calculate the overall location of the primitive given the parameters
                            Rectangle rect = new Rectangle((int)(nextLocation.X), (int)(nextLocation.Y), 20, 20);

                            //draw onto the trunk texture
                            batch.Draw(primitives[k], rect, null, Color.White, nextRotation + startDisplayRotation[k] + (float)decorator.rand.NextDouble() * displayRotationRange[k], Vector2.Zero, effect, 0);

                            lastLocation = nextLocation;
                            lastRotation = nextRotation;
                        }
                    }
                }


                batch.End();
                Game1.instance.GraphicsDevice.SetRenderTarget(null);

                trunks[i] = trunkImages;
            }

            //lastly, generate a texture for the branches.
            //make the texture size a bit bigger than the normal block, so that
            //the generation has room to work without ugly cut-offs on the final texture.
            int texSizeTop = (int)(Chunk.tileDrawWidth * 3);
            for (int i = 0; i < numGeneratedTrunks; i++)
            {

                RenderTarget2D treeTopImages = new RenderTarget2D(
                   Game1.instance.GraphicsDevice,
                   texSizeTop,
                   texSizeTop,
                   false,
                   Game1.instance.GraphicsDevice.PresentationParameters.BackBufferFormat,
                   DepthFormat.Depth24);

                //blah blah blah
                Game1.instance.GraphicsDevice.SetRenderTarget(treeTopImages);
                Game1.instance.GraphicsDevice.Clear(Color.Transparent);
                batch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

                for (int k = 0; k < numPrimitives; k++)
                {
                    for (int j = 0; j < branches[k]; j++)
                    {
                        //set up the start position and rotations.
                        //Vector2 lastLocation = new Vector2((texSize / 2) + rand.Next(texSize / 4) - texSize / 8, 10);
                        Vector2 lastLocation = new Vector2((texSizeTop * .66f) - decorator.rand.Next((int)(texSizeTop * .33f)), texSizeTop - 15);
                        float lastRotation = (float)((Math.PI * -1.5) + decorator.rand.NextDouble() * rotationRange[k] - rotationRange[k] / 2); //turns out that this number is more-or-less vertically aligned for some reason.

                        //stop drawing when we've reached the other side of the texture
                        while (lastLocation.Y > 15 && lastLocation.X > 15 && lastLocation.X < texSizeTop - 15)
                        {
                            //find the rotation location and rotation after a jump
                            float nextRotation = lastRotation + (float)(decorator.rand.NextDouble() * rotationRange[k] - rotationRange[k] / 2);
                            Vector2 nextLocation = lastLocation - vectorFromAngle(nextRotation) * jumpHeight[k];

                            //50% chance to flip the sprite
                            SpriteEffects effect = SpriteEffects.None;
                            if (decorator.rand.NextDouble() < .5f) { effect = SpriteEffects.FlipHorizontally; }

                            //calculate the overall location of the primitive given the parameters
                            Rectangle rect = new Rectangle((int)(nextLocation.X), (int)(nextLocation.Y), 20, 20);

                            //draw onto the trunk texture
                            batch.Draw(primitives[k], rect, null, Color.White, nextRotation + startDisplayRotation[k] + (float)decorator.rand.NextDouble() * displayRotationRange[k], Vector2.Zero, effect, 0);

                            lastLocation = nextLocation;
                            lastRotation = nextRotation;
                        }
                    }
                }


                batch.End();
                Game1.instance.GraphicsDevice.SetRenderTarget(null);

                treeTops[i] = treeTopImages;
            }

            //TODO: randomize parameters
            //treeTrunk = new RandomImageTile(new TileTag[] { TagReferencer.AIR, TagReferencer.Climbeable, TagReferencer.DRAWOUTSIDEOFBOUNDS }, trunks, false);
            //trunk.friction += .03f;

            //TODO: randomize parameters
            //treeTop = new RandomImageTile(new TileTag[] { TagReferencer.AIR, TagReferencer.Climbeable, TagReferencer.DRAWOUTSIDEOFBOUNDS }, treeTops, false);
            //treeTop.friction += .03f;

            treeTrunk = new RandomImageTileFromSpritesheet(new TileTag[] { TagReferencer.AIR, TagReferencer.Climbeable, TagReferencer.Harvest, TagReferencer.DRAWOUTSIDEOFBOUNDS }, trunks, texSizeTrunk, batch, false);
            treeTrunk.harvestTicks = 5 + getRandValuePlusOrMinus(decorator.rand, Math.Max(decorator.metaDifficulty, 1));
            treeTop = new RandomImageTileFromSpritesheet(new TileTag[] { TagReferencer.AIR, TagReferencer.Climbeable, TagReferencer.DRAWOUTSIDEOFBOUNDS }, treeTops, texSizeTop, batch, false);

            batch.Dispose();
        }

        private List<Decoration> generateTrees(ChunkDecorator decorator, TileType trunk, TileType treeTop, TileType leaves)
        {
            List<Decoration> generatedTrees = new List<Decoration>();

            int MODE_BULB = 0;
            int MODE_CONE = 1;
            int MODE_POCKETS = 2;

            int minTrunkHeight = 1 + decorator.rand.Next(8);
            int trunkHeightVariation = 1 + decorator.rand.Next(6);

            int minLeafWidth = decorator.rand.Next(8);
            int leafWidthVariation = decorator.rand.Next(6);

            int minLeafHeight = 1 + decorator.rand.Next(8);
            int leafHeightVariation = decorator.rand.Next(6);
            int mode = decorator.rand.Next(3);

            //used only in cone generation
            int coneMod = 1 + decorator.rand.Next(minLeafHeight);
            //used only in pocket generation
            int minPockets = 2 + decorator.rand.Next(4);
            int pocketsVariation = decorator.rand.Next(4);

            float minLeafFullness = .4f + (float)decorator.rand.NextDouble() * .6f;
            float leafFullnessVariation = (float)decorator.rand.NextDouble() * .6f;

            for (int i = 0; i < 12; i++)
            {
                int trunkHeight = minTrunkHeight + decorator.rand.Next(trunkHeightVariation);
                int leafWidth = minLeafWidth + decorator.rand.Next(leafWidthVariation);
                int leafHeight = minLeafHeight + decorator.rand.Next(leafHeightVariation);
                float leafFullness = minLeafFullness + (float)decorator.rand.NextDouble() * leafFullnessVariation;

                if (leafWidth % 2 == 0)
                {
                    leafWidth++;
                }

                TileType[,] tiles = new TileType[leafWidth, trunkHeight + leafHeight];

                for (int n = 0; n < trunkHeight + leafHeight; n++)
                {
                    tiles[leafWidth / 2, n] = trunk;
                }
                mode = MODE_BULB;
                if (mode == MODE_BULB)
                {
                    for (int x = 0; x < leafWidth; x++)
                    {
                        for (int y = 0; y < leafHeight; y++)
                        {
                            if (decorator.rand.NextDouble() < leafFullness)
                            {
                                tiles[x, y] = leaves;
                            }

                        }
                    }
                    tiles[leafWidth / 2, leafHeight] = treeTop;
                }
                else if (mode == MODE_CONE)
                {
                    int expansionIX = leafHeight / leafWidth;
                    int currentW = 0;
                    for (int y = 0; y < leafHeight + trunkHeight - 1; y++)
                    {
                        if (y % coneMod == 0)
                        {
                            currentW++;
                        }

                        for (int x = 0; x < currentW; x++)
                        {

                            if (decorator.rand.NextDouble() < leafFullness)
                            {
                                tiles[Math.Min(leafWidth - 1, leafWidth / 2 + x), y] = leaves;
                            }
                            if (decorator.rand.NextDouble() < leafFullness)
                            {
                                tiles[Math.Max(0, leafWidth / 2 - x), y] = leaves;
                            }

                        }
                    }

                    for (int n = 0; n < trunkHeight + leafHeight; n++)
                    {
                        if (decorator.rand.NextDouble() < .7)
                        {
                            tiles[leafWidth / 2, n] = trunk;
                        }

                    }
                }
                else if (mode == MODE_POCKETS)
                {
                    int pockets = minPockets + decorator.rand.Next(pocketsVariation);
                    for (int z = 0; z < pockets; z++)
                    {
                        int thisPocketWidth = (minLeafHeight + decorator.rand.Next(leafHeightVariation)) / 2;
                        int thisPocketHeight = (minLeafHeight + decorator.rand.Next(leafHeightVariation)) / 2;
                        int pocketX = leafWidth / 2 + (decorator.rand.Next(leafWidth) - decorator.rand.Next(leafWidth));
                        int pocketY = decorator.rand.Next(trunkHeight + leafHeight);
                        for (int x = 0; x < thisPocketWidth; x++)
                        {
                            for (int y = 0; y < thisPocketHeight; y++)
                            {
                                if (decorator.rand.NextDouble() < leafFullness)
                                {
                                    tiles[Math.Max(0, Math.Min(pocketX + x, leafWidth - 1)), Math.Max(0, Math.Min(pocketY + y, trunkHeight + leafHeight - 1))] = leaves;
                                }

                            }
                        }
                    }

                    for (int n = leafHeight; n < trunkHeight + leafHeight; n++)
                    {
                        if (decorator.rand.NextDouble() < .7)
                        {
                            tiles[leafWidth / 2, n] = trunk;
                        }

                    }
                }





                generatedTrees.Add(new Decoration(new Point(leafWidth / 2, trunkHeight + leafHeight - 1), tiles));
            }
            return generatedTrees;
        }
        
        public void Dispose()
        {
            if(leaves != null)
            {
                leaves.Dispose();
                trunk.Dispose();
                treeTop.Dispose();
                leaves2.Dispose();
                trunk2.Dispose();
                treeTop2.Dispose();
            }
            
        }

        public static float angleFromVector(Vector2 consider)
        {
            return (float)Math.Atan2(consider.Y, consider.X);

        }

        public static Vector2 vectorFromAngle(float angle)
        {
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        public int getRandValuePlusOrMinus(Random rand, int range)
        {
            return rand.Next(range * 2) - range;
        }
    }
}
