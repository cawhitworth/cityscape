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

        public static void AddColumnedPanel(ref List<VertexPositionNormalTextureMod> verts, ref List<int> indices,
                                    Vector3 basePosition,
                                    Vector2 dimensions,
                                    int desiredWidth, int sepWidth,
                                    Vector2 textureOrigin,
                                    Vector2 XZ, Vector3 colorMod, Stretch stretch)
        {
            Vector3 position = basePosition;
            // WWW-W-WWW is the minimum we want to do
            // 
            if (dimensions.X <= (float)(desiredWidth * 2 + sepWidth + 2))
            {
                AddPanel(ref verts, ref indices, position, dimensions, textureOrigin, XZ, true, colorMod, stretch);
                return;
            }
            Vector2 panelDimensions = new Vector2((float)desiredWidth, dimensions.Y);
            Vector2 spacerDimensions = new Vector2((float) sepWidth, dimensions.Y);

            float offset = 0.0f;
            Vector3 offsetPosition = new Vector3(0.0f);

            AddPanel(ref verts, ref indices, position, panelDimensions, textureOrigin, XZ, true, colorMod, stretch);
            offset += panelDimensions.X;
            offsetPosition.X = offset * storyDimensions.X * XZ.X;
            offsetPosition.Z = offset * storyDimensions.Z * XZ.Y;
            AddPanel(ref verts, ref indices, position + offsetPosition, spacerDimensions, textureOrigin, XZ, false, colorMod, stretch);
            offset += spacerDimensions.X;
            offsetPosition.X = offset * storyDimensions.X * XZ.X;
            offsetPosition.Z = offset * storyDimensions.Z * XZ.Y;
            textureOrigin.X += panelDimensions.X * BuildingTextureGenerator.StoryXMultiplier;

            bool done = false;
            float midPoint = dimensions.X / 2.0f;
            while (!done)
            {
                if (offset + panelDimensions.X + spacerDimensions.X > midPoint)
                {
                    float tempWidth = 2.0f * (midPoint - offset);
                    AddPanel(ref verts, ref indices, position + offsetPosition, new Vector2(tempWidth, panelDimensions.Y),
                        textureOrigin, XZ, true, colorMod, stretch);
                    offset += tempWidth;

                    offsetPosition.X = offset * storyDimensions.X * XZ.X;
                    offsetPosition.Z = offset * storyDimensions.Z * XZ.Y;
                    textureOrigin.X += tempWidth * BuildingTextureGenerator.StoryXMultiplier;
                    done = true;
                }
                else if (position.X == midPoint)
                {
                    done = true;
                }
                else
                {
                    AddPanel(ref verts, ref indices, position + offsetPosition, panelDimensions, textureOrigin, XZ, true, colorMod, stretch);
                    offset += panelDimensions.X;
                    offsetPosition.X = offset * storyDimensions.X * XZ.X;
                    offsetPosition.Z = offset * storyDimensions.Z * XZ.Y;
                    AddPanel(ref verts, ref indices, position + offsetPosition, spacerDimensions, textureOrigin, XZ, false, colorMod, stretch);
                    offset += spacerDimensions.X;
                    offsetPosition.X = offset * storyDimensions.X * XZ.X;
                    offsetPosition.Z = offset * storyDimensions.Z * XZ.Y;

                    textureOrigin.X += panelDimensions.X * BuildingTextureGenerator.StoryXMultiplier;
                }
            }

            while (offset < dimensions.X)
            {
                AddPanel(ref verts, ref indices, position + offsetPosition, spacerDimensions, textureOrigin, XZ, false, colorMod, stretch);
                offset += spacerDimensions.X;
                offsetPosition.X = offset * storyDimensions.X * XZ.X;
                offsetPosition.Z = offset * storyDimensions.Z * XZ.Y;

                AddPanel(ref verts, ref indices, position + offsetPosition, panelDimensions, textureOrigin, XZ, true, colorMod, stretch);
                offset += panelDimensions.X;
                offsetPosition.X = offset * storyDimensions.X * XZ.X;
                offsetPosition.Z = offset * storyDimensions.Z * XZ.Y;
                textureOrigin.X += panelDimensions.X * BuildingTextureGenerator.StoryXMultiplier;
            }

        }

        public static void AddColumnedPanel(ref List<VertexPositionNormalTextureMod> verts, ref List<int> indices,
                                    Vector3 position,
                                    float height, float panelWidth, float spacerWidth, int nPanels,
                                    Vector2 textureOrigin,
                                    Vector2 XZ,
                                    Vector3 colorMod, Stretch stretch)
        {
            Vector2 panelDimensions = new Vector2(panelWidth, height);
            Vector2 spacerDimensions = new Vector2(spacerWidth, height);
            AddPanel(ref verts, ref indices, position, panelDimensions, textureOrigin, XZ, true, colorMod, stretch);
            for(int panel = 1; panel < nPanels; panel++)
            {
                position.X += panelWidth * (storyDimensions.X * XZ.X);
                position.Z += panelWidth * (storyDimensions.Z * XZ.Y);
                AddPanel(ref verts, ref indices, position, spacerDimensions, textureOrigin, XZ, false, colorMod, stretch);

                position.X += spacerWidth * (storyDimensions.X * XZ.X);
                position.Z += spacerWidth * (storyDimensions.Z * XZ.Y);
                textureOrigin.X += panelWidth * BuildingTextureGenerator.StoryXMultiplier;
                AddPanel(ref verts, ref indices, position, panelDimensions, textureOrigin, XZ, true, colorMod, stretch);
            }
        }
    }
}