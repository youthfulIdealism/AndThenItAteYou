using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Survive.WorldManagement.Tile.ExtensionTileTypes
{
    public class RandomImageTile : TileType
    {
        public List<Texture2D> textures;

        public RandomImageTile(TileTag[] tags, Texture2D[] tex, bool permanent) : base(tags, tex[0], permanent)
        {
            textures = tex.ToList();
        }

        public override void draw(SpriteBatch batch, Point place, Color color)
        {
            Random rand = new Random(place.X + place.Y * 30);
            texture = textures[rand.Next(textures.Count)];
            base.draw(batch, place, color);
        }

        public override void Dispose()
        {
            if(texture != null)
            {
                texture.Dispose();
            }
            
            foreach(Texture2D tex in textures)
            {
                tex.Dispose();
            }
        }
    }
}
