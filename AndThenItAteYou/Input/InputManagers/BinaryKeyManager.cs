using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace Survive.Input.InputManagers
{
    public class BinaryKeyManager : BinaryInputManager
    {
        public Keys key;
        KeyboardState currentKeyboardState;
        KeyboardState prevKeyboardState;

        public BinaryKeyManager(Keys key)
        {
            this.key = key;
        }

        public bool click()
        {
            return prevKeyboardState.IsKeyDown(key) && currentKeyboardState.IsKeyUp(key);
        }

        public bool isDown()
        {
            return currentKeyboardState.IsKeyDown(key);
        }

        public bool wasDown()
        {
            return prevKeyboardState.IsKeyDown(key);
        }

        public void update(MouseState mouse, KeyboardState keyboard)
        {
            prevKeyboardState = currentKeyboardState;
            currentKeyboardState = keyboard;
        }

        
    }
}
