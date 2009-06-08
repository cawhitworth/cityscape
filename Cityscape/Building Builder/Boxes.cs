using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Cityscape
{
    public partial class BuildingBuilder
    {
        /// <summary>
        /// Add a box made of panels
        /// </summary>
        /// <param name="verts">Vertex list</param>
        /// <param name="indices">Index list</param>
        /// <param name="position">Bottom-left-back corner of box</param>
        /// <param name="stories">Dimensions in stories</param>
        /// <param name="colorMod">Colour modulation</param>
        /// <param name="stretch">Stretch</param>
        public static void AddSimplePanelBox(ref List<VertexPositionNormalTextureMod> verts, ref List<int> indices,
                                   Vector3 position,
                                   Vector3 stories,
                                   Vector3 colorMod,
                                   Stretch stretch)
        {
            Vector3 dimensions = new Vector3(stories.X * storyDimensions.X, stories.Y * storyDimensions.Y, stories.Z * storyDimensions.Z);
            Vector2 texOrigin;
            float texWidth = stories.X * BuildingTextureGenerator.StoryXMultiplier;
            float texDepth = stories.Z * BuildingTextureGenerator.StoryXMultiplier;

            switch (stretch)
            {
                case Stretch.Horizontal:
                    texOrigin = new Vector2(rand.Next(32) * 2 * BuildingTextureGenerator.StoryXMultiplier,
                                            rand.Next(64) * BuildingTextureGenerator.StoryYMultiplier);
                    break;
                case Stretch.Vertical:
                    texOrigin = new Vector2(rand.Next(64) * BuildingTextureGenerator.StoryXMultiplier,
                                            rand.Next(32) * 2 * BuildingTextureGenerator.StoryYMultiplier);
                    break;
                default:
                    texOrigin = new Vector2(rand.Next(64) * BuildingTextureGenerator.StoryXMultiplier,
                                            rand.Next(64) * BuildingTextureGenerator.StoryYMultiplier);
                    break;
            }

            // These are badly misnamed, because of the handedness issue. By "Front", I probably mean "Back"

            // Front
            AddPanel(ref verts, ref indices, position,
                     new Vector2(stories.X, stories.Y), texOrigin, new Vector2(1.0f, 0.0f), true, colorMod, stretch);
            // Right
            texOrigin.X += texWidth;
            AddPanel(ref verts, ref indices, position + new Vector3(dimensions.X, 0.0f, 0.0f),
                     new Vector2(stories.Z, stories.Y), texOrigin, new Vector2(0.0f, 1.0f), true, colorMod, stretch);
            // Back
            texOrigin.X += texDepth;
            AddPanel(ref verts, ref indices, position + new Vector3(dimensions.X, 0.0f, dimensions.Z),
                     new Vector2(stories.X, stories.Y), texOrigin, new Vector2(-1.0f, 0.0f), true, colorMod, stretch);
            // Left
            texOrigin.X += texWidth;
            AddPanel(ref verts, ref indices, position + new Vector3(0.0f, 0.0f, dimensions.Z),
                     new Vector2(stories.Z, stories.Y), texOrigin, new Vector2(0.0f, -1.0f), true, colorMod, stretch);

            // Top
            AddPlane(ref verts, ref indices, position + new Vector3(0.0f, stories.Y * BuildingBuilder.storyDimensions.Y, 0.0f),
                new Vector2(stories.X * storyDimensions.X, stories.Z * storyDimensions.Z));

        }

        /// <summary>
        /// Wrapper for the AddSimplePanelBox function
        /// </summary>
        /// <param name="verts">Vertex list</param>
        /// <param name="indices">Index list</param>
        /// <param name="position">Bottom corner</param>
        /// <param name="stories">Dimensions in stories</param>
        /// <param name="colorMod">Color modulation</param>
        /// <param name="stretch">Stretch</param>
        public static void AddSimpleBox(ref List<VertexPositionNormalTextureMod> verts, ref List<int> indices,
                                   Vector3 position,
                                   Vector3 stories,
                                   Vector3 colorMod,
                                   Stretch stretch)
        {
            AddSimplePanelBox(ref verts, ref indices, position, stories, colorMod, stretch);
        }

        public static void AddPanelledBox(ref List<VertexPositionNormalTextureMod> verts, ref List<int> indices,
                                    Vector3 position,
                                    float height, float panelWidth, float spacerWidth, int nPanelsWide, int nPanelsDeep,
                                    Vector3 colorMod, Stretch stretch)
        {
            Vector3 dimensions = new Vector3(( (panelWidth * nPanelsWide) + (spacerWidth * (nPanelsWide-1))) * storyDimensions.X,
                                              height * storyDimensions.Y,
                                             ( (panelWidth * nPanelsDeep) + (spacerWidth * (nPanelsDeep-1))) * storyDimensions.Z);
            Vector2 texOrigin;
            float texWidth = (panelWidth * nPanelsWide) * BuildingTextureGenerator.StoryXMultiplier;
            float texDepth = (panelWidth * nPanelsDeep) * BuildingTextureGenerator.StoryXMultiplier;

            switch (stretch)
            {
                case Stretch.Horizontal:
                    texOrigin = new Vector2(rand.Next(32) * 2 * BuildingTextureGenerator.StoryXMultiplier,
                                            rand.Next(64) * BuildingTextureGenerator.StoryYMultiplier);
                    break;
                case Stretch.Vertical:
                    texOrigin = new Vector2(rand.Next(64) * BuildingTextureGenerator.StoryXMultiplier,
                                            rand.Next(32) * 2 * BuildingTextureGenerator.StoryYMultiplier);
                    break;
                default:
                    texOrigin = new Vector2(rand.Next(64) * BuildingTextureGenerator.StoryXMultiplier,
                                            rand.Next(64) * BuildingTextureGenerator.StoryYMultiplier);
                    break;
            }

            // These are badly misnamed, because of the handedness issue. By "Front", I probably mean "Back"

            // Front
            AddColumnedPanel(ref verts, ref indices, position,
                     height, panelWidth, spacerWidth, nPanelsWide, texOrigin, new Vector2(1.0f, 0.0f), colorMod, stretch);
            // Right
            texOrigin.X += texWidth;
            AddColumnedPanel(ref verts, ref indices, position + new Vector3(dimensions.X, 0.0f, 0.0f),
                     height, panelWidth, spacerWidth, nPanelsDeep, texOrigin, new Vector2(0.0f, 1.0f), colorMod, stretch);
            // Back
            texOrigin.X += texDepth;
            AddColumnedPanel(ref verts, ref indices, position + new Vector3(dimensions.X, 0.0f, dimensions.Z),
                     height, panelWidth, spacerWidth, nPanelsWide, texOrigin, new Vector2(-1.0f, 0.0f), colorMod, stretch);
            // Left
            texOrigin.X += texWidth;
            AddColumnedPanel(ref verts, ref indices, position + new Vector3(0.0f, 0.0f, dimensions.Z),
                     height, panelWidth, spacerWidth, nPanelsDeep, texOrigin, new Vector2(0.0f, -1.0f), colorMod, stretch);

            // Top
            AddPlane(ref verts, ref indices, position + new Vector3(0.0f, height * BuildingBuilder.storyDimensions.Y, 0.0f),
                new Vector2(dimensions.X, dimensions.Z));
        }
    }
}