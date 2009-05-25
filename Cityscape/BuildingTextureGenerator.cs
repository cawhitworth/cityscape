using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Cityscape
{
    class BuildingTextureGenerator 
    {
        public const int textureWidth = 512;
        public const int textureHeight = 512;
        public const int windowHeight = 8;
        public const int windowWidth = 8;

        static Random rand = new Random();

        public static float StoryXMultiplier
        {
            get { return 1.0f / (float)(textureWidth / windowWidth); }
        }

        public static float StoryYMultiplier
        {
            get { return 1.0f / (float)(textureHeight / windowHeight); }
        }

        static byte ModColor(byte component, int divisor)
        {
            int comp = (int)component;
            comp -= rand.Next( component / divisor );
            if (comp > 255) comp = 255;
            if (comp < 0) comp = 0;
            return (byte)comp;
        }

        public static Texture2D MakeTexture(GraphicsDevice device)
        {

            Texture2D texture = new Texture2D(device, textureWidth, textureHeight, 0, TextureUsage.AutoGenerateMipMap, SurfaceFormat.Color);
            Color[] textureData = new Color[textureWidth*textureHeight];
            texture.GetData<Color>(textureData);
            for (int index = 0; index < textureWidth * textureHeight; index++) textureData[index].PackedValue = 0xff000000;

            // 8x8 windows?

            for (int x = 0; x < textureWidth / windowWidth; x++)
            {
                for (int y = 0; y < textureHeight / windowHeight; y++)
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
                            textureData[ (x*windowWidth) + xx + ((y * windowHeight) + yy) * textureWidth] = col;
                        }
                    }

                    // Add noise

                    int points = rand.Next(16) + 16;
                    for(int point = 0; point < points; point ++)
                    {
                        int xx = rand.Next(6) + 1;
                        int yy = rand.Next(4) + 1;
                        Color c = textureData[ (x * windowWidth) + xx + ((y * windowHeight) + yy) * textureWidth];
                        c.R = ModColor(c.R, 2); c.G = c.B = c.R;
                        c.G = ModColor(c.G, 4);
                        c.B = ModColor(c.B, 4);
                        textureData[ (x * windowWidth) + xx + ((y * windowHeight) + yy) * textureWidth] = c;

                    }
                }
            }

            texture.SetData<Color>(textureData);
            texture.Save("texture.png", ImageFileFormat.Png);
            return texture;
        }
    }
}
