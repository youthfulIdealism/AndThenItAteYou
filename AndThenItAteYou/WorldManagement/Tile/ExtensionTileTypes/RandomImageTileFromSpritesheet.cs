using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Survive.WorldManagement.Tile.Tags;

namespace Survive.WorldManagement.Tile.ExtensionTileTypes
{
    public class RandomImageTileFromSpritesheet : TileType
    {
        public int textureCount;
        public int textureDimensions;
        public int widthInTiles;

        public RandomImageTileFromSpritesheet(TileTag[] tags, Texture2D[] tex, int tileWidthAndHeight, SpriteBatch batch, bool permanent) : base(tags, tex[0], permanent)
        {
            
            textureCount = tex.Length;
            textureDimensions = tileWidthAndHeight;
            
            widthInTiles = (int)Math.Ceiling(Math.Sqrt(tex.Length));
            int totalWidth = widthInTiles * tileWidthAndHeight;

            texture = new RenderTarget2D(
                   Game1.instance.GraphicsDevice,
                   totalWidth,
                   totalWidth,
                   false,
                   Game1.instance.GraphicsDevice.PresentationParameters.BackBufferFormat,
                   DepthFormat.Depth24);

            Game1.instance.GraphicsDevice.SetRenderTarget((RenderTarget2D)texture);
            Game1.instance.GraphicsDevice.Clear(Color.Transparent);

            for(int i = 0; i < textureCount; i++)
            {
                batch.Begin(SpriteSortMode.Deferred, null, null, null, Game1.scizzorRasterState);
                Rectangle destinationRectangle = new Rectangle(textureDimensions * (i % widthInTiles), textureDimensions * (i / widthInTiles), textureDimensions, textureDimensions);
                batch.GraphicsDevice.ScissorRectangle = destinationRectangle;

                batch.Draw(tex[i], destinationRectangle, Color.White);

                batch.End();
            }

            Game1.instance.GraphicsDevice.SetRenderTarget(null);
        }

        public RandomImageTileFromSpritesheet(TileTag[] tags, bool permanent, RandomImageTileFromSpritesheet copyFrom) : base(tags, copyFrom.texture, permanent)
        {
            textureCount = copyFrom.textureCount;
            textureDimensions = copyFrom.textureDimensions;
            widthInTiles = copyFrom.widthInTiles;
        }

        public override void draw(SpriteBatch batch, Point place, Color color)
        {
            Random rand = new Random(place.X + place.Y * 30);

            if (texture != null)
            {
                int selectedTile = rand.Next(textureCount);
                Rectangle sourceRectangle = new Rectangle(textureDimensions * (selectedTile % widthInTiles), textureDimensions * (selectedTile / widthInTiles), textureDimensions, textureDimensions);

                if (tags.Contains(TagReferencer.DRAWOUTSIDEOFBOUNDS))
                {
                    batch.Draw(texture, new Rectangle(place.X * Chunk.tileDrawWidth - (textureDimensions / 2 - Chunk.tileDrawWidth / 2), place.Y * Chunk.tileDrawWidth - (textureDimensions / 2 - Chunk.tileDrawWidth / 2), textureDimensions, textureDimensions), sourceRectangle, color);
                }
                else
                {
                    batch.Draw(texture, new Rectangle(place.X * Chunk.tileDrawWidth, place.Y * Chunk.tileDrawWidth, Chunk.tileDrawWidth, Chunk.tileDrawWidth), sourceRectangle, color);
                }

            }
        }
    }
}
