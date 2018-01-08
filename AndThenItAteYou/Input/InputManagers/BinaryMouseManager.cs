using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.Input.InputManagers
{
    public class BinaryMouseManager : BinaryInputManager
    {
        private MouseState currentMouseState;
        private MouseState lastMouseState;
        public bool leftMouseButton;

        public BinaryMouseManager(bool leftMouseButton)
        {
            this.leftMouseButton = leftMouseButton;
        }

        public bool click()
        {
            if(leftMouseButton)
            {
                return lastMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released;
            }
            else
            {
                return lastMouseState.RightButton == ButtonState.Pressed && currentMouseState.RightButton == ButtonState.Released;
            }
        }

        public bool isDown()
        {
            
            if (leftMouseButton)
            {
                return currentMouseState.LeftButton == ButtonState.Pressed;
            }
            else
            {
                return currentMouseState.RightButton == ButtonState.Pressed;
            }
        }

        public void update(MouseState mouse, KeyboardState keyboard)
        {
            lastMouseState = currentMouseState;
            currentMouseState = mouse;
        }

        public bool wasDown()
        {
            if (leftMouseButton)
            {
                return lastMouseState.LeftButton == ButtonState.Pressed;
            }
            else
            {
                return lastMouseState.RightButton == ButtonState.Pressed;
            }
        }
    }
}
