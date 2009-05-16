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
            int height = 512;
            int width = 512;
            int windowHeight = 8;
            int windowWidth = 8;

            Texture2D texture = new Texture2D(device, width, height, 0, TextureUsage.AutoGenerateMipMap, SurfaceFormat.Color);
            Color[] textureData = new Color[width*height];
            texture.GetData<Color>(textureData);
            for (int index = 0; index < width * height; index++) textureData[index].PackedValue = 0xff202020;

            // 8x8 windows?

            Random rand = new Random();
            for (int x = 0; x < width / windowWidth; x++)
            {
                for (int y = 0; y < height / windowHeight; y++)
                {
                    bool light = rand.NextDouble() > 0.8;
                    float shade = (float) rand.NextDouble() * 0.2f;
                    if (light)
                        shade += 0.75f;
                    Color col = new Color(shade, shade, shade, 1.0f);

                    for (int xx = 1; xx < windowWidth - 1; xx++)
                    {
                        for (int yy = 1; yy < windowHeight - 1; yy++)
                        {
                            textureData[ (x*windowWidth) + xx + ((y * windowHeight) + yy) * width] = col;
                        }
                    }

                }
            }

            texture.SetData<Color>(textureData);
            texture.Save("texture.png", ImageFileFormat.Png);
            return texture;
        }
    }
}
