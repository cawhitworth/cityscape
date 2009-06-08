using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Cityscape
{
    public partial class BuildingBuilder
    {
        /// <summary>
        /// Add an X-Z plane (non-infinite)
        /// </summary>
        /// <param name="verts">Vertex list</param>
        /// <param name="indices">Index list</param>
        /// <param name="position">Corner of the plane</param>
        /// <param name="dimensions">X,Z dimensions of the plane</param>
        public static void AddPlane(ref List<VertexPositionNormalTextureMod> verts, ref List<int> indices,
                                    Vector3 position,
                                    Vector2 dimensions)
        {
            Vector2 texCoord = new Vector2(0.0f, 0.0f);
            Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);
            Vector3 mod = new Vector3(1.0f, 1.0f, 1.0f);
            int baseIndex = verts.Count();
            verts.Add(new VertexPositionNormalTextureMod(position, up, texCoord, mod));
            verts.Add(new VertexPositionNormalTextureMod(position + new Vector3(0.0f, 0.0f, dimensions.Y), up, texCoord, mod));
            verts.Add(new VertexPositionNormalTextureMod(position + new Vector3(dimensions.X, 0.0f, 0.0f), up, texCoord, mod));
            verts.Add(new VertexPositionNormalTextureMod(position + new Vector3(dimensions.X, 0.0f, dimensions.Y), up, texCoord, mod));
            indices.Add(baseIndex); indices.Add(baseIndex + 1); indices.Add(baseIndex + 2);
            indices.Add(baseIndex + 2); indices.Add(baseIndex + 1); indices.Add(baseIndex + 3);
        }

        /// <summary>
        /// Add a blank (untextured) panel
        /// </summary>
        /// <param name="verts">Vertex list</param>
        /// <param name="indices">Index list</param>
        /// <param name="position">"Bottom" corner of the panel</param>
        /// <param name="stories">Width/Depth and Height of the panel</param>
        /// <param name="XZ">X-Z orientation (see AddPanel for further details)</param>
        public static void AddBlankPanel(ref List<VertexPositionNormalTextureMod> verts, ref List<int> indices,
                                    Vector3 position,
                                    Vector2 stories,
                                    Vector2 XZ)
        {
            AddPanel(ref verts, ref indices, position, stories, new Vector2(0.0f, 0.0f), XZ, false, new Vector3(0.0f, 0.0f, 0.0f), Stretch.None);
        }

        /// <summary>
        /// Add a panel, sized by stories
        /// </summary>
        /// <param name="verts">Vertex list</param>
        /// <param name="indices">Index list</param>
        /// <param name="position">Bottom corner</param>
        /// <param name="stories">Dimensions in stories</param>
        /// <param name="textOrigin">Texture origin</param>
        /// <param name="XZ">X-Z orientation (see other AddPanel for further details)</param>
        /// <param name="textured">Is this panel textured?</param>
        /// <param name="colorMod">Colour modulation multiplier</param>
        /// <param name="stretch">Window stretch</param>
        public static void AddPanel(ref List<VertexPositionNormalTextureMod> verts, ref List<int> indices,
                                    Vector3 position,
                                    Vector2 stories,
                                    Vector2 textOrigin,
                                    Vector2 XZ,
                                    bool textured,
                                    Vector3 colorMod,
                                    Stretch stretch)
        {
            Vector2 text2;
            float xScale = 1.0f, yScale = 1.0f;

            switch(stretch)
            {
                case Stretch.Horizontal:
                    xScale = 0.5f;
                    break;
                case Stretch.Vertical:
                    yScale = 0.5f;
                    break;
            }

            if (textured)
                text2 = textOrigin + new Vector2(stories.X * xScale * BuildingTextureGenerator.StoryXMultiplier,
                                                 stories.Y * yScale * BuildingTextureGenerator.StoryYMultiplier);
            else
                text2 = textOrigin;


            AddPanel(ref verts, ref indices, position,
                     new Vector2(stories.X * storyDimensions.X, stories.Y * storyDimensions.Y),
                     textOrigin,
                     text2,
                     XZ,
                     colorMod);
        }
        
        /// <summary>
        /// Generic base add panel functionality
        /// </summary>
        /// <param name="verts">Vertex list</param>
        /// <param name="indices">Index list</param>
        /// <param name="position">Bottom corner</param>
        /// <param name="dimensions">Dimensions in world units</param>
        /// <param name="texture1">Bottom-left texture coordinate</param>
        /// <param name="texture2">Top-right texture coordinate</param>
        /// <param name="XZ">X-Z orientation. (1.0f, 0.0f) => panel is in the XY plane; (0.0f, 1.0f) => panel is in the ZY plane</param>
        /// <param name="colorMod">Colour modulation</param>
        public static void AddPanel(ref List<VertexPositionNormalTextureMod> verts, ref List<int> indices,
                                    Vector3 position,
                                    Vector2 dimensions,
                                    Vector2 texture1,
                                    Vector2 texture2,
                                    Vector2 XZ,
                                    Vector3 colorMod)
        {
            Vector3 oppositeCorner = position + new Vector3(dimensions.X * XZ.X, dimensions.Y, dimensions.X * XZ.Y);

            Vector3 topLeft = new Vector3(position.X, oppositeCorner.Y, position.Z);
            Vector3 bottomRight = new Vector3(oppositeCorner.X, position.Y, oppositeCorner.Z);

            Vector2 textTopLeft = new Vector2(texture1.X, texture2.Y);
            Vector2 textBottomRight = new Vector2(texture2.X, texture1.Y);

            Vector3 normal = Vector3.Cross( topLeft - position, bottomRight - position);

            int baseIndex = verts.Count();
            verts.Add(new VertexPositionNormalTextureMod(position, normal, texture1, colorMod));
            verts.Add(new VertexPositionNormalTextureMod(topLeft, normal, textTopLeft, colorMod));
            verts.Add(new VertexPositionNormalTextureMod(bottomRight, normal, textBottomRight, colorMod));
            verts.Add(new VertexPositionNormalTextureMod(oppositeCorner, normal, texture2, colorMod));
            indices.Add(baseIndex); indices.Add(baseIndex + 1); indices.Add(baseIndex + 2);
            indices.Add(baseIndex + 2); indices.Add(baseIndex + 1); indices.Add(baseIndex + 3);
        }
    }
}