
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Survive.WorldManagement.ContentProcessors;
using Survive.WorldManagement.Entities;
using Survive.WorldManagement.Inventory;
using Survive.WorldManagement.Inventory.Items;
using Survive.WorldManagement.Tile;
using Survive.WorldManagement.Tile.ExtensionTileTypes;
using Survive.WorldManagement.Tile.Tags;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Survive.WorldManagement.World;
using static Survive.WorldManagement.WorldBase;

namespace Survive.WorldManagement.Procedurals
{
    public class ChunkDecorator : IDisposable
    {
        public static Random nonWorldRand;

        const int terrainRoughnessBase = 250;

        public WorldBase world;
        public int metaDifficulty;

        public List<Decoration> foliage;
        public List<Decoration> foliageBiome2;
        public List<Decoration> subTerranianFoliage;
        public List<Type> critters;
        public List<Type> monsters;
        public List<Type> decorativeCritters;
        public float foliageChance;
        public float spawnCritterChance = .3f;
        public float chanceCritterIsMonster = .1f;
        public float terrainRoughness { get; private set; }
        public float caveThreshold { get; protected set; }
        public Random rand;
        public float dayTemp = 1;
        public float nightTemp = -1;
        public bool isCity = false;
        public int waterLevel = int.MinValue;

        public ColorManager colorManager;
        public TreeManager treeManager;
        public ShrubManager shrubManager;
        public WeatherManager weatherManager;
        public AmbientSoundManager ambientSoundManager;
        public CritterManager critterManager;
        public DecorationManager decorationManager;
        public WorldGenSubtype worldGenSubtype;




        public ContentManager content;

        public ChunkDecorator(WorldBase world)
        {
            worldGenSubtype = WorldGenSubtype.STANDARD;

            metaDifficulty = Math.Max(0, world.difficulty + MetaData.difficultyModifier * MetaData.adaptiveDifficulty);
            /*Console.WriteLine("World-difficulty: " + world.difficulty);
            Console.WriteLine("Universe-difficulty: " + UniverseProperties.difficultyModifier);
            Console.WriteLine("Combined-difficulty: " + (world.difficulty + UniverseProperties.difficultyModifier));
            Console.WriteLine("Meta-difficulty: " + metaDifficulty);*/
            if (world.difficulty == Game1.findGirlWorld) { metaDifficulty = -1; }
            content = new ContentManager(Game1.instance.Content.ServiceProvider, Game1.instance.Content.RootDirectory);

           

            if (nonWorldRand == null)
            {
                nonWorldRand = new Random();
            }

            this.world = world;
            rand = world.rand;
            foliage = new List<Decoration>();
            foliageBiome2 = new List<Decoration>();
            subTerranianFoliage = new List<Decoration>();
            critters = new List<Type>();
            monsters = new List<Type>();
            decorativeCritters = new List<Type>();
            foliageChance = .35f + (float)(rand.NextDouble() * .1);
            terrainRoughness = (float)(.7f + rand.NextDouble() * .1 * metaDifficulty);
            spawnCritterChance -= (float)(rand.NextDouble() * .08 * (1f / Math.Max(1, metaDifficulty * 1.5f)));
            chanceCritterIsMonster += (float)metaDifficulty / 35;
            if(metaDifficulty <= 1)
            {
                chanceCritterIsMonster = 0;
            }

            if (metaDifficulty >= 1)
            {
                if (rand.NextDouble() < Math.Atan((float)metaDifficulty / 20))
                {
                    isCity = true;
                }
            }

            caveThreshold = .585f;
            if (metaDifficulty == 0) { caveThreshold -= .015f; }
            else if (metaDifficulty > 0) { caveThreshold += (float)(rand.NextDouble() * (world.difficulty + 2) * .1f * .01f); }

            dayTemp += (float)((rand.NextDouble() * (metaDifficulty) * .2) - (rand.NextDouble() * (metaDifficulty) * .2 * .5));
            nightTemp += (float)((rand.NextDouble() * (metaDifficulty) * .2) - (rand.NextDouble() * (metaDifficulty) * .2 * .5));

            colorManager = new ColorManager(this);
               
        }

        public void rollMapGenerationSubtype(World world)
        {
            //if (true)
            if (world.difficulty > 3 && rand.NextDouble() < Math.Atan((float)metaDifficulty / 25))
            {
                worldGenSubtype = WorldGenSubtype.CENOTE;
                waterLevel = 0;
                float waterAccumulator = 0;
                int samples = 200;
                for(int i = 0; i < samples; i++)
                {
                    waterAccumulator += world.noise.octavePerlin1D((float)(i) / 25) * getTerrainMultiplier();
                }
                waterAccumulator /= samples;
                waterLevel = (int)waterAccumulator - 1;
                isCity = false;
            }
            else
            {
                worldGenSubtype = WorldGenSubtype.STANDARD;
            }

            Console.WriteLine("World gen subtype: " + worldGenSubtype);
        }

        public void generateMapTypeDependentDecorations()
        {
            treeManager = new TreeManager(this);
            shrubManager = new ShrubManager(this);
            weatherManager = new WeatherManager(this);
            ambientSoundManager = new AmbientSoundManager(this);
            critterManager = new CritterManager(this);
            decorationManager = new DecorationManager(this);
        }



        public int getTerrainMultiplier()
        {
            return (int)(terrainRoughness * terrainRoughnessBase);
        }

        public Decoration getFoliage(Chunk chunk, int x, int y)
        {
            Random rand = new Random(chunk.location.X * Chunk.tilesPerChunk + x + chunk.location.Y * Chunk.tilesPerChunk + y);
            if(Math.Abs(chunk.location.X) > 4)
            {
                double foliageBiome2Chance = Math.Abs(chunk.location.X) - 4;
                if(rand.NextDouble() * 4 < foliageBiome2Chance)
                {
                    return foliageBiome2[rand.Next(foliageBiome2.Count)];
                }
            }
            

            return foliage[rand.Next(foliage.Count)];

        }

        public Decoration getSubTerranianFoliage(Chunk chunk, int x, int y)
        {
            Random rand = new Random(chunk.location.X * Chunk.tilesPerChunk + x + chunk.location.Y * Chunk.tilesPerChunk + y);

            return subTerranianFoliage[rand.Next(subTerranianFoliage.Count)];

        }

        public Entity getCritterForChunk(Vector2 location)
        {
            if(nonWorldRand.NextDouble() < chanceCritterIsMonster)
            {
                return (Entity)Activator.CreateInstance(monsters[nonWorldRand.Next(monsters.Count)], new Object[] { location, world });
            }else
            {
                return (Entity)Activator.CreateInstance(critters[nonWorldRand.Next(critters.Count)], new Object[] { location, world });
            }
        }

        public Entity getDecorativeCritterForChunk(Vector2 location)
        {
            return (Entity)Activator.CreateInstance(decorativeCritters[nonWorldRand.Next(decorativeCritters.Count)], new Object[] { location, world });
        }


        public static float angleFromVector(Vector2 consider)
        {
            return (float)Math.Atan2(consider.Y, consider.X);

        }

        public static Vector2 vectorFromAngle(float angle)
        {
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        public void Dispose()
        {
            treeManager.Dispose();
            shrubManager.Dispose();
            ambientSoundManager.Dispose();


            
            content.Dispose();
        }

        public int getRandValuePlusOrMinus(Random rand, int range)
        {
            return rand.Next(range * 2) - range;
        }
    }
}
