using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Survive.Input;
using Survive.Input.InputManagers;
using Survive.WorldManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Inventory
{
    /**
        Item aliases: Defined by char used, where QWERTYUIOP are the 0th, 1st, 2nd, etc chars used. Use the LangGen program to find the appropriate symbols.
    */
    public abstract class Item
    {
        public Texture2D texture { get; set; }
        public int uses { get; set; }
        public bool usesStandardControlScheme;

        public Item(int uses)
        {
            this.uses = uses;
            usesStandardControlScheme = true;
        }

        public abstract Item clone(int uses);

        public virtual int getRemainingUses()
        {
            return uses;
        }

        public virtual int use(PlayerBase user, WorldBase world, Vector2 location, GameTime time, BinaryInputManager inputManager)
        {
            return 0;
        }

        public virtual bool isGroupable(Item other)
        {
            return other.GetType().Equals(this.GetType());
        }

        public virtual void draw(SpriteBatch batch, PlayerBase user, Point offset, Color groundColor)
        {

        }
    }
}
