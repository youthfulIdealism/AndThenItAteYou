using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.Input.InputManagers
{
    public interface BinaryInputManager
    {
        bool isDown();
        bool wasDown();
        bool click();
        void update(MouseState mouse, KeyboardState keyboard);
    }
}
