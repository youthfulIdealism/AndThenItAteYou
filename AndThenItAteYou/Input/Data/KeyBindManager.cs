using Microsoft.Xna.Framework.Input;
using Survive.Input.InputManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.Input.Data
{
    public class KeyBindManager
    {
        public Dictionary<String, BinaryInputManager> bindings { get; private set; }

        public KeyBindManager()
        {
            bindings = new Dictionary<string, BinaryInputManager>();

            //Movement Bindings
            bindings.Add("Up", KeyManagerEnumerator.letrW);
            bindings.Add("Left", KeyManagerEnumerator.letrA);
            bindings.Add("Right", KeyManagerEnumerator.letrD);
            bindings.Add("Down", KeyManagerEnumerator.letrS);

            //World Interaction Bindings
            bindings.Add("Use", KeyManagerEnumerator.RMB);

            //Item Use Bindings
            bindings.Add("Inventory_0", KeyManagerEnumerator.LMB);
            bindings.Add("Inventory_1", KeyManagerEnumerator.letrQ);
            bindings.Add("Inventory_2", KeyManagerEnumerator.letrE);
            bindings.Add("Inventory_3", KeyManagerEnumerator.letrR);
            bindings.Add("Inventory_4", KeyManagerEnumerator.letrF);
            bindings.Add("Inventory_5", KeyManagerEnumerator.letrC);

            //PowerUp Bindings
            bindings.Add("Ability_0", KeyManagerEnumerator.num1);
            bindings.Add("Ability_1", KeyManagerEnumerator.num2);

            //Inventory Bindings
            bindings.Add("Inventory_Open", KeyManagerEnumerator.letrT);
            bindings.Add("Inventory_Display", KeyManagerEnumerator.utilLeftShift);
        }

        public void update(MouseState mouseState, KeyboardState keyboardState)
        {
            foreach(BinaryInputManager manager in bindings.Values)
            {
                manager.update(mouseState, keyboardState);
            }
        }

    }
}
