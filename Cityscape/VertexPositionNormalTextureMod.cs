using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Cityscape
{
    public struct VertexPositionNormalTextureMod
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 TexCoords;
        public Vector3 ColorMod;

        public VertexPositionNormalTextureMod(Vector3 Position,
                                              Vector3 Normal,
                                              Vector2 TexCoords,
                                              Vector3 ColorMod)
        {
            this.Position = Position;
            this.Normal = Normal;
            this.TexCoords = TexCoords;
            this.ColorMod = ColorMod;
        }

        public static readonly VertexElement[] VertexElements =
        {
            new VertexElement(0, 0, VertexElementFormat.Vector3, VertexElementMethod.Default, VertexElementUsage.Position, 0) ,
            new VertexElement(0, sizeof(float) * 3, VertexElementFormat.Vector3, VertexElementMethod.Default, VertexElementUsage.Normal, 0) ,
            new VertexElement(0, sizeof(float) * (3 + 3), VertexElementFormat.Vector2, VertexElementMethod.Default, VertexElementUsage.TextureCoordinate, 0) ,
            new VertexElement(0, sizeof(float) * (3 + 3 + 2), VertexElementFormat.Vector3, VertexElementMethod.Default, VertexElementUsage.TextureCoordinate, 1)
        };

        public static readonly int SizeInBytes = sizeof(float) * (3 + 3 + 2 + 3);
    }
}