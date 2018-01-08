using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive
{
    public interface DelayedRenderable
    {

        void draw(SpriteBatch batch, GameTime time, Point offset);

    }
}
