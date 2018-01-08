using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.WorldManagement.Entities
{
    public class PlayerAnimationPackage
    {
        public Texture2D standTex;
        public Texture2D[] runTex;
        public Texture2D[] jumpTex;
        public Texture2D[] swingTex;
        public Texture2D[] throwTex;
        public static Dictionary<int, PlayerAnimationPackage> animationPackageKey;
        public static int animationIDCounter;
        public int id;

        public PlayerAnimationPackage(Texture2D standTex, Texture2D[] runTex, Texture2D[] jumpTex, Texture2D[] throwTex, Texture2D[] swingTex)
        {
            if (animationPackageKey == null)
            {
                animationPackageKey = new Dictionary<int, PlayerAnimationPackage>();
            }
            id = animationIDCounter++;
            animationPackageKey.Add(id, this);

            this.standTex = standTex;
            this.runTex = runTex;
            this.jumpTex = jumpTex;
            this.swingTex = swingTex;
            this.throwTex = throwTex;
        }

        public void apply(Player player)
        {
            player.texture_player_default_stand = standTex;
            player.texture_run = runTex;
            player.texture_jump = jumpTex;
            player.texture_swing = swingTex;
            player.texture_throw = throwTex;
            player.selectedFrame = standTex;
            player.animationPackage = this;
        }
    }
}
