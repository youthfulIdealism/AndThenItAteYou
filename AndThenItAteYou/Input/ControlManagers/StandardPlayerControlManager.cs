using Survive.WorldManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Survive.WorldManagement;
using Survive.WorldManagement.Inventory;
using Survive.WorldManagement.Tile;
using Survive.WorldManagement.Tile.Tags;
using Microsoft.Xna.Framework.Graphics;
using Survive.Sound;
using Survive.SplashScreens;
using Survive.Input.InputManagers;
using Survive.WorldManagement.Entities.Projectiles;

namespace Survive.Input.ControlManagers
{
    public class StandardPlayerControlManager : BasicPlayerControlManager
    {

        public StandardPlayerControlManager(Player entity) : base(entity){ }
    }
}
