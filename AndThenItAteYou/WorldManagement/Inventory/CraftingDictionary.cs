using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Inventory
{
    public class CraftingDictionary
    {
        private Dictionary<Type, HashSet<CraftingRecepie>> craftingDictionary;
        public CraftingDictionary()
        {
            craftingDictionary = new Dictionary<Type, HashSet<CraftingRecepie>>();

        }

        public void registerRecepie(CraftingRecepie recepie)
        {
            foreach(Item item in recepie.components)
            {
                if(!craftingDictionary.ContainsKey(item.GetType()))
                {
                    craftingDictionary.Add(item.GetType(), new HashSet<CraftingRecepie>());
                }
                if(!craftingDictionary[item.GetType()].Contains(recepie))
                {
                    craftingDictionary[item.GetType()].Add(recepie);
                }
                
            }
        }

        public HashSet<CraftingRecepie> getRecepies(Item item)
        {
            if (!craftingDictionary.ContainsKey(item.GetType()))
            {
                return null;
            }
            return craftingDictionary[item.GetType()];
        }

    }

}
