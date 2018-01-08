using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Survive.WorldManagement.IO;
using Survive.WorldManagement;
using Survive.Sound;


/*
 * 
 * 
 * 1: 52, 4, 44, 44
2: 100, 4, 44, 44
3: 148, 4, 44, 44
4: 192, 4, 44, 44
5: 244, 4, 44, 44
6: 292, 4, 44, 44
7: 340, 4, 44, 44
8: 388, 4, 44, 44
9: 436, 4, 44, 44
0: 484, 4, 44, 44
TAB: 4, 52, 68, 44
Q: 76, 52, 44, 44
W: 124, 4, 44, 44
E: 172, 4, 44, 44
R: 220, 4, 44, 44
T: 268, 4, 44, 44
Y: 316, 4, 44, 44
U: 364, 4, 44, 44
I: 412, 4, 44, 44
O: 460, 4, 44, 44
P: 508, 4, 44, 44
A: 88, 100, 44, 44
S: 136, 100, 44, 44
D: 184, 100, 44, 44
F: 232, 100, 44, 44
G: 280, 100, 44, 44
H: 328, 100, 44, 44
J: 376, 100, 44, 44
K: 424, 100, 44, 44
L: 472, 100, 44, 44
LSHIFT: 4, 148, 104, 44
Z: 112, 148, 44, 44
X: 160, 148, 44, 44
C: 208, 148, 44, 44
V: 256, 148, 44, 44
B: 304, 148, 44, 44
N: 352, 148, 44, 44
M: 400, 148, 44, 44
LControl: 4, 196, 68, 44
LSpace: 196, 196, 272, 44
Mouse1: 596, 63, 50, 48
Mouse2: 656, 63, 50, 48
 * 
 * */
namespace Survive.SplashScreens
{
    public class DeathScreen : SplashScreen
    {
        float currentTime = totalTime;
        const float totalTime = 500;

        float flashTime = 500;

        public DeathScreen()
        {
            
        }

        public override void acceptInput(GameTime time, KeyboardState currentKeyboardState, MouseState currentMouseState, KeyboardState prevKeyboardState, MouseState prevMouseState)
        {
            currentTime--;
            if(currentTime <= 0)
            {
                Game1.instance.returnToMainMenu();
            }
            flashTime *= .9f;
        }

        public override bool canContinue()
        {
            return currentTime <= 0;
        }

        public override void draw(SpriteBatch batch)
        {
            batch.Draw(
                Game1.block,
                new Rectangle(0,
                    0,
                    Game1.instance.graphics.PreferredBackBufferWidth,
                    Game1.instance.graphics.PreferredBackBufferHeight),
                Color.White * (flashTime / 500));
            batch.Draw(
                Game1.Skull,
                new Rectangle(Game1.instance.graphics.PreferredBackBufferWidth / 2 - Game1.Skull.Width / 2,
                    Game1.instance.graphics.PreferredBackBufferHeight / 2 - Game1.Skull.Height / 2,
                    Game1.Skull.Width,
                    Game1.Skull.Height),
                Color.White * (currentTime / totalTime + .1f));
        }

        public override bool isBlockingDrawCalls()
        {
            return true;
        }

        public override void switchTo()
        {
            SoundManager.getSound("death").play(0, .8f, 0, SoundType.AMBIENT);
        }
    }
}
