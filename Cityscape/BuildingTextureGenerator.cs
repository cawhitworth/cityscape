using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Cityscape
{
    class BuildingTextureGenerator 
    {
        public static Texture2D MakeTexture(GraphicsDevice device)
        {
            int height = 1024;
            int width = 1024;
            Texture2D texture = new Texture2D(device, width, height, 0, TextureUsage.AutoGenerateMipMap, SurfaceFormat.Color);
            Color[] textureData = new Color[width*height];
            texture.GetData<Color>(textureData);
            int xx, yy;
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++ )
                {
                    xx = x % 128;
                    yy = y % 128;
                    if ((xx < 64 && yy < 64) || (xx > 64 && yy > 64))
                        textureData[x + y * width] = Color.Black;
                    else
                        textureData[x + y * width] = Color.White;
                }

            texture.SetData<Color>(textureData);
            texture.Save("texture.png", ImageFileFormat.Png);
            return texture;
        }
    }
}
