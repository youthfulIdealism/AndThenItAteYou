using Survive.WorldManagement.Entities;
using Survive.WorldManagement.Entities.Worm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Procedurals
{
    public class CritterManager
    {
        
        
        private static Type[] decorativeCritters = new Type[] { typeof(EntityCricket), typeof(EntityButterfly) };
        private static Type[] decorativeCrittersCave = new Type[] { typeof(EntityBat) };

        private static Type[] smallPreyCritters = new Type[] { typeof(EntityTurkey), typeof(EntityRabbit) };
        private static Type[] smallPreyCrittersCave = new Type[] { typeof(EntityFish) };

        private static Type[] medPreyCritters = new Type[] { typeof(EntityMoose), typeof(EntityTapir) };
        private static Type[] medPreyCrittersCave = new Type[] { typeof(EntityCrocodile) };

        private static Type[] easyMonsters = new Type[] { typeof(EntityGuardian), typeof(EntityWheelie), typeof(WormHead) };
        private static Type[] easyMonstersCave = new Type[] { typeof(EntityCentipedeHead)};


        public CritterManager(ChunkDecorator decorator)
        {
            Type decorativeCritter = null;
            Type smallCritter = null;
            Type mediumCritter = null;
            if (decorator.worldGenSubtype == World.WorldGenSubtype.CENOTE)
            {
                decorativeCritter = decorativeCrittersCave[decorator.rand.Next(decorativeCrittersCave.Length)];
                smallCritter = smallPreyCrittersCave[decorator.rand.Next(smallPreyCrittersCave.Length)];
                mediumCritter = medPreyCrittersCave[decorator.rand.Next(medPreyCrittersCave.Length)];
            } else
            {
                decorativeCritter = decorativeCritters[decorator.rand.Next(decorativeCritters.Length)];
                smallCritter = smallPreyCritters[decorator.rand.Next(smallPreyCritters.Length)];
                mediumCritter = medPreyCritters[decorator.rand.Next(medPreyCritters.Length)];
            }
            
            decorator.decorativeCritters.Add(decorativeCritter);
            decorator.critters.Add(mediumCritter);
            decorator.critters.Add(smallCritter);
            decorator.critters.Add(smallCritter);

            if (decorator.isCity)
            {
                decorator.monsters.Add(typeof(EntityConstable));
            }
            else
            {
                if (decorator.metaDifficulty >= 4)
                {
                    if (decorator.rand.NextDouble() < .8f)
                    {
                        selectEasyMonster(decorator);
                    }
                    else
                    {
                        selectDifficultMonster(decorator);
                    }
                }
                else
                {
                    selectEasyMonster(decorator);
                }
            }

        }
        private void selectDifficultMonster(ChunkDecorator decorator)
        {
            float subSelectedCritter = (float)decorator.rand.NextDouble();
            if (subSelectedCritter < .5f)
            {
                decorator.chanceCritterIsMonster *= .1f;
                decorator.monsters.Add(typeof(EntityOwl));
            }
            else
            {
                decorator.chanceCritterIsMonster *= .1f;
                decorator.monsters.Add(typeof(EntityAntlion));
            }
        }

        private void selectEasyMonster(ChunkDecorator decorator)
        {
            if (decorator.worldGenSubtype == World.WorldGenSubtype.CENOTE)
            {
                decorator.monsters.Add(easyMonstersCave[decorator.rand.Next(easyMonstersCave.Length)]);
            }
            else
            {
                decorator.monsters.Add(easyMonsters[decorator.rand.Next(easyMonsters.Length)]);
            }
                
        }
    }
}

