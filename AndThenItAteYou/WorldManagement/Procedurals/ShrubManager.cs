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
    public class ShrubManager : IDisposable
    {
        public TileType bush;
        public TileType grass;

        public TileType bush2;
        public TileType grass2;

        public ShrubManager(ChunkDecorator decorator)
        {
            if (decorator.worldGenSubtype != World.WorldGenSubtype.CENOTE)
            {

                //build grass and bushes
                grass = generateGrass(decorator);
                bush = generateBush(decorator);
                bush.harvestTicks += getRandValuePlusOrMinus(decorator.rand, Math.Max(decorator.metaDifficulty, 1) * 8);

                grass2 = generateGrass(decorator);
                bush2 = generateBush(decorator);
                bush2.harvestTicks += getRandValuePlusOrMinus(decorator.rand, Math.Max(decorator.metaDifficulty, 1) * 8);

                TileType.replaceTileInDictionary(TileTypeReferencer.REACTIVE_GRASS.TILEID, grass);
                TileType.replaceTileInDictionary(TileTypeReferencer.REACTIVE_BUSH_0.TILEID, bush);
                TileType.replaceTileInDictionary(TileTypeReferencer.REACTIVE_BUSH_1.TILEID, bush2);

                ItemDropper bushDrops = new ItemDropper();
                bushDrops.registerNewDrop(new Item_Grass(0), null, 1, 1f);
                ItemDropper bushDrops2 = new ItemDropper();
                bushDrops2.registerNewDrop(new Item_Grass(0), null, 1, 1f);

                if (decorator.world is World)
                {
                    if (decorator.rand.NextDouble() < .15)//chance to give the bush an interesting drop.
                    {
                        Item_Berry bushBerry = UniverseProperties.availableBerries[decorator.rand.Next(3)];

                        bushDrops.registerNewDrop(bushBerry, null, 1 + decorator.rand.Next(4), (float)(.05 + decorator.rand.NextDouble() * .3));
                    }
                    if (decorator.rand.NextDouble() < .15)//chance to give the bush an interesting drop.
                    {
                        Item_Berry bushBerry = UniverseProperties.availableBerries[decorator.rand.Next(3)];

                        bushDrops2.registerNewDrop(bushBerry, null, 1 + decorator.rand.Next(4), (float)(.05 + decorator.rand.NextDouble() * .3));
                    }
                }



                HarvestDictionary.registerNewHarvest(bush, new ItemDropper[] { bushDrops });
                HarvestDictionary.registerNewHarvest(bush2, new ItemDropper[] { bushDrops2 });

                for (int i = 0; i < 7 + decorator.rand.Next(7); i++)
                {
                    decorator.foliage.Add(new Decoration(new Point(0, 0), new TileType[,] { { bush }, }));
                    decorator.foliageBiome2.Add(new Decoration(new Point(0, 0), new TileType[,] { { bush2 }, }));
                }
            }else
            {
                for (int i = 0; i < 7 + decorator.rand.Next(7); i++)
                {
                    decorator.foliage.Add(new Decoration(new Point(0, 0), new TileType[,] { { TileTypeReferencer.STALAGMITE_TINY }, }));
                    decorator.foliageBiome2.Add(new Decoration(new Point(0, 0), new TileType[,] { { TileTypeReferencer.STALAGMITE_TINY }, }));
                }
            }

            //
        }

        public TileType generateGrass(ChunkDecorator decorator)
        {
            //we want to generate an appropriate variety of trunk images.
            int numGeneratedGrasses = 16;

            //set up the texture array and spritebatch
            RenderTarget2D[] grasses = new RenderTarget2D[numGeneratedGrasses];
            SpriteBatch batch = new SpriteBatch(Game1.instance.GraphicsDevice);

            //numPrimitives represents the number of different "basic shapes" used to generate the tree trunk.
            //int numPrimitives = 1 + rand.Next(2);
            //each primitive gets N different "strings" of drawings.
            //int[] branches = new int[numPrimitives];
            //each primitive gets to move N distance each pass along its "string".
            float jumpHeight = 1 + decorator.rand.Next(1);
            float bumpiness = 1 + decorator.rand.Next(7);
            //each primitive can change direction N amount each pass along its string.
            //each primitive can start rotated an arbitrary amount.
            float startDisplayRotation = (float)(decorator.rand.NextDouble() * Math.PI * 2);
            float displayRotationRange = (float)(decorator.rand.NextDouble() * Math.PI * 2);

            Texture2D primitives = Game1.instance.primitives[decorator.rand.Next(Game1.instance.primitives.Count)];

            //make the texture size a bit bigger than the normal block, so that
            //the generation has room to work without ugly cut-offs on the final texture.
            int texSize = (int)(Chunk.tileDrawWidth * 1.7);
            for (int i = 0; i < numGeneratedGrasses; i++)
            {

                RenderTarget2D grassImage = new RenderTarget2D(
                   Game1.instance.GraphicsDevice,
                   texSize,
                   texSize,
                   false,
                   Game1.instance.GraphicsDevice.PresentationParameters.BackBufferFormat,
                   DepthFormat.Depth24);

                //blah blah blah
                Game1.instance.GraphicsDevice.SetRenderTarget(grassImage);
                Game1.instance.GraphicsDevice.Clear(Color.Transparent);
                batch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);


                //set up the start position and rotations.
                //Vector2 lastLocation = new Vector2((texSize / 2) + rand.Next(texSize / 4) - texSize / 8, 10);
                Vector2 lastLocation = new Vector2(15, Chunk.tileDrawWidth + 19);

                //stop drawing when we've reached the other side of the texture
                for (int z = 0; z < 10; z++)
                {
                    while (lastLocation.X < texSize - 20)
                    {
                        //find the rotation location and rotation after a jump
                        Vector2 nextLocation = lastLocation + new Vector2(jumpHeight, 0);

                        //50% chance to flip the sprite
                        SpriteEffects effect = SpriteEffects.None;
                        if (decorator.rand.NextDouble() < .5f) { effect = SpriteEffects.FlipHorizontally; }

                        //calculate the overall location of the primitive given the parameters
                        Rectangle rect = new Rectangle((int)(nextLocation.X), (int)(nextLocation.Y + decorator.rand.NextDouble() * bumpiness), 7, 7);

                        //draw onto the trunk texture
                        batch.Draw(primitives, rect, null, Color.White, startDisplayRotation + (float)decorator.rand.NextDouble() * displayRotationRange, Vector2.Zero, effect, 0);

                        lastLocation = nextLocation;
                    }
                }

                batch.End();
                Game1.instance.GraphicsDevice.SetRenderTarget(null);

                grasses[i] = grassImage;
            }

            //TODO: randomize parameters
            //return new RandomImageTile(new TileTag[] { TagReferencer.AIR, TagReferencer.DRAWOUTSIDEOFBOUNDS }, grasses, false);
            RandomImageTileFromSpritesheet tile = new RandomImageTileFromSpritesheet(new TileTag[] { TagReferencer.AIR, TagReferencer.DRAWOUTSIDEOFBOUNDS }, grasses, texSize, batch, false);
            batch.Dispose();
            return tile;
        }

        public TileType generateBush(ChunkDecorator decorator)
        {
            //we want to generate an appropriate variety of trunk images.
            int numGeneratedBushes = 16;

            //set up the texture array and spritebatch
            RenderTarget2D[] bushes = new RenderTarget2D[numGeneratedBushes];
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
                jumpHeight[i] = 1 + decorator.rand.Next(5);
                //select a random rotation for each primitive to use on each pass.
                rotationRange[i] = (float)(decorator.rand.NextDouble() * Math.PI);
                //select a random display rotation for each primitive to use
                startDisplayRotation[i] = (float)(decorator.rand.NextDouble() * Math.PI * 2);
                displayRotationRange[i] = (float)(decorator.rand.NextDouble() * Math.PI * 2);
            }

            //make the texture size a bit bigger than the normal block, so that
            //the generation has room to work without ugly cut-offs on the final texture.
            int texSize = (int)(Chunk.tileDrawWidth * 1.5 * 2);
            for (int i = 0; i < numGeneratedBushes; i++)
            {

                RenderTarget2D bushImage = new RenderTarget2D(
                   Game1.instance.GraphicsDevice,
                   texSize,
                   texSize/* * 2*/,
                   false,
                   Game1.instance.GraphicsDevice.PresentationParameters.BackBufferFormat,
                   DepthFormat.Depth24);

                //blah blah blah
                Game1.instance.GraphicsDevice.SetRenderTarget(bushImage);
                Game1.instance.GraphicsDevice.Clear(Color.Transparent);
                batch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

                for (int k = 0; k < numPrimitives; k++)
                {
                    for (int j = 0; j < branches[k]; j++)
                    {
                        //set up the start position and rotations.
                        //Vector2 lastLocation = new Vector2((texSize / 2) + rand.Next(texSize / 4) - texSize / 8, 10);
                        Vector2 lastLocation = new Vector2((texSize * .66f) - decorator.rand.Next((int)(texSize * .33f)), texSize * /*1*/.7f);
                        float lastRotation = (float)((Math.PI * -1.5) + decorator.rand.NextDouble() * rotationRange[k] - rotationRange[k] / 2); //turns out that this number is more-or-less vertically aligned for some reason.

                        //stop drawing when we've reached the other side of the texture
                        while (lastLocation.Y > 15 && lastLocation.X > 15 && lastLocation.X < texSize - 15)
                        {
                            //find the rotation location and rotation after a jump
                            float nextRotation = lastRotation + (float)(decorator.rand.NextDouble() * rotationRange[k] - rotationRange[k] / 2);
                            Vector2 nextLocation = lastLocation - vectorFromAngle(nextRotation) * jumpHeight[k];

                            //50% chance to flip the sprite
                            SpriteEffects effect = SpriteEffects.None;
                            if (decorator.rand.NextDouble() < .5f) { effect = SpriteEffects.FlipHorizontally; }

                            //calculate the overall location of the primitive given the parameters
                            Rectangle rect = new Rectangle((int)(nextLocation.X), (int)(nextLocation.Y), 7, 7);

                            //draw onto the trunk texture
                            batch.Draw(primitives[k], rect, null, Color.White, nextRotation + startDisplayRotation[k] + (float)decorator.rand.NextDouble() * displayRotationRange[k], Vector2.Zero, effect, 0);

                            lastLocation = nextLocation;
                            lastRotation = nextRotation;
                        }
                    }
                }


                batch.End();
                Game1.instance.GraphicsDevice.SetRenderTarget(null);

                bushes[i] = bushImage;
            }

            //TODO: randomize parameters
            //RandomImageTile generatedBush = new RandomImageTile(new TileTag[] { TagReferencer.AIR, TagReferencer.Harvest, TagReferencer.DRAWOUTSIDEOFBOUNDS }, bushes, false);
            RandomImageTileFromSpritesheet tile = new RandomImageTileFromSpritesheet(new TileTag[] { TagReferencer.AIR, TagReferencer.Harvest, TagReferencer.DRAWOUTSIDEOFBOUNDS }, bushes, texSize, batch, false);
            batch.Dispose();
            return tile;
        }

        public void Dispose()
        {
            if(bush != null)
            {
                bush.Dispose();
                grass.texture.Dispose();

                bush2.Dispose();
                grass2.texture.Dispose();
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
