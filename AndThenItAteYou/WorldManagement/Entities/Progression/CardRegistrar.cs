using Survive.WorldManagement.Entities;
using Survive.WorldManagement.Entities.Progression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Inventory.Items
{
    public static class CardRegistrar
    {
        private static int registerNumber;
        private static Dictionary<int, Type> cardDictionary;
        private static Dictionary<Type, int> reverseCardDictionary;

        static CardRegistrar()
        {
            registerNumber = 0;
            cardDictionary = new Dictionary<int, Type>();
            reverseCardDictionary = new Dictionary<Type, int>();

            registerCardType(typeof(CardJump));
            registerCardType(typeof(CardSpeed));
            registerCardType(typeof(CardHealthBoost));
            registerCardType(typeof(CardHealthRegen));
            registerCardType(typeof(CardBlink));
            registerCardType(typeof(CardExplosive));
            registerCardType(typeof(CardSticky));
        }

        private static void registerCardType(Type type)
        {
            registerNumber++;
            cardDictionary.Add(registerNumber, type);
            reverseCardDictionary.Add(type, registerNumber);
        }

        public static Type getTypeFromID(int id)
        {
            return cardDictionary[id];
        }

        public static int getIDFromType(Type type)
        {
            return reverseCardDictionary[type];
        }

        public static int getIDFromCard(Card card)
        {
            return reverseCardDictionary[card.GetType()];
        }

        public static Card getCardFromIdPlayerAndLevel(int id, float level, Player owner)
        {
            return (Card)Activator.CreateInstance(getTypeFromID(id), new Object[] { level });
        }
    }
}
