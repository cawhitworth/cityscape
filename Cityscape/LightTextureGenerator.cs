using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;

namespace Cityscape
{
    class LightTextureGenerator
    {
        public const int textureWidth = 256;
        public const int textureHeight = 256;
        private static Texture2D texture_= null;
        private static IGraphicsDeviceService deviceService_ = null;

        public static IGraphicsDeviceService deviceService
        {
            set
            {
                deviceService_ = value;
            }
        }

        public static Texture2D texture
        {
            get
            {
                if (texture_ == null && deviceService_ != null)
                {
                    texture_ = new Texture2D(deviceService_.GraphicsDevice, textureWidth, textureHeight, 0, TextureUsage.AutoGenerateMipMap, SurfaceFormat.Color);
                    Color[] textureData = new Color[textureWidth * textureHeight];
                    texture_.GetData<Color>(textureData);
                    for (int x = 0; x < textureWidth; x++)
                        for (int y = 0; y < textureHeight; y++)
                        {
                            float xDist = (float)(x - textureWidth / 2);
                            float yDist = (float)(y - textureHeight / 2);
                            xDist /= (float)(textureWidth / 2);
                            yDist /= (float)(textureHeight / 2);
                            float dist = (float)Math.Sqrt(xDist * xDist + yDist * yDist);
                            textureData[x + y * textureWidth] = new Color(1.0f, 1.0f, 1.0f, 1.0f-dist);
                        }
                    texture_.SetData(textureData);
                    texture_.Save("lighttexture_.png", ImageFileFormat.Png);
                }
                return texture_;
            }
        }
    }
}
