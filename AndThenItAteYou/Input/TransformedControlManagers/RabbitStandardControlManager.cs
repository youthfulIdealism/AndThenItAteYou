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
using Survive.WorldManagement.Entities.TransformedPlayers;
using Survive.Input.ControlManagers;
using Survive.Input.InputManagers;

namespace Survive.Input
{
    public class RabbitStandardControlManager : BasicPlayerControlManager
    {
        public RabbitStandardControlManager(PlayerRabbit entity) : base(entity) { }

        protected override void handleWorldInteract(GameTime time, KeyboardState currentKeyboardState, MouseState currentMouseState, KeyboardState prevKeyboardState, MouseState prevMouseState)
        {

        }

        protected override void handleInventoryOpen(GameTime time, KeyboardState currentKeyboardState, MouseState currentMouseState, KeyboardState prevKeyboardState, MouseState prevMouseState)
        {

        }
    }
}
