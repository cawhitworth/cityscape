using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Cityscape
{
    public partial class BuildingBuilder
    {
        public static void AddCylinder(ref List<VertexPositionNormalTextureMod> verts, ref List<int> indices,
                                        Vector3 origin,
                                        Vector3 storyDimensions,
                                        float stories,
                                        float diameter,
                                        Vector3 colorMod,
                                        Stretch stretch,
                                        int segments)
        {
            AddCylinder(ref verts, ref indices, origin, storyDimensions, stories, diameter, diameter, colorMod, stretch, segments);
        }

        public static void AddCylinder(ref List<VertexPositionNormalTextureMod> verts, ref List<int> indices,
                                        Vector3 origin,
                                        Vector3 storyDimensions,
                                        float stories,
                                        float diameter1,
                                        float diameter2,
                                        Vector3 colorMod,
                                        Stretch stretch,
                                        int segments)
        {
            float circumference = (float)Math.PI * ((3.0f * (diameter1 + diameter2)) - (float)Math.Sqrt(((3.0f * diameter1) + diameter2) * (diameter1 + (3.0f * diameter2))));
            float windowsAroundCircumference = (float)Math.Floor(circumference);

            float windowsPerSegment = windowsAroundCircumference / (float)segments;

            float texWidth = windowsPerSegment * BuildingTextureGenerator.StoryXMultiplier;
            float texHeight = stories * BuildingTextureGenerator.StoryYMultiplier;

            Vector2 texOrigin = new Vector2(rand.Next(64) * BuildingTextureGenerator.StoryXMultiplier,
                                            rand.Next(64) * BuildingTextureGenerator.StoryYMultiplier);


            switch (stretch)
            {
                case Stretch.Horizontal:
                    texWidth /= 2.0f;
                    break;
                case Stretch.Vertical:
                    texHeight /= 2.0f;
                    break;
            }

            Vector2 texBottomLeft = texOrigin + new Vector2(0.0f, 0.0f);
            Vector2 texTopLeft = texOrigin + new Vector2(0.0f, texHeight);

            float height = stories * storyDimensions.Y;
            float angle = 0;
            Vector3 normal = new Vector3();
            Vector3 position;
            int baseIndex = verts.Count();
            float angleStep = (2.0f * (float)(Math.PI)) / (float)segments;
            // Vertices
            for (int segment = 0; segment < segments + 1; segment++)
            {
                normal.X = (float)Math.Sin((double)angle);
                normal.Y = 0.0f;
                normal.Z = (float)Math.Cos((double)angle);
                position = normal;
                position.X *= (diameter1 * storyDimensions.X) / 2.0f;
                position.Z *= (diameter2 * storyDimensions.X) / 2.0f;
                position += origin;

                verts.Add(new VertexPositionNormalTextureMod(position, normal, texBottomLeft, colorMod));
                position += new Vector3(0.0f, height, 0.0f);
                verts.Add(new VertexPositionNormalTextureMod(position, normal, texTopLeft, colorMod));

                texBottomLeft += new Vector2(texWidth, 0.0f);
                texTopLeft += new Vector2(texWidth, 0.0f);
                angle += angleStep;
            }

            int thisSegment, nextSegment;
            angle = 0;
            // Indices
            for (int segment = 0; segment < segments; segment++)
            {
                thisSegment = baseIndex + (segment * 2);
                nextSegment = thisSegment + 2;
                indices.Add(thisSegment); indices.Add(thisSegment + 1); indices.Add(nextSegment);
                indices.Add(nextSegment); indices.Add(thisSegment + 1); indices.Add(nextSegment + 1);
            }
            // Cap

            angle = 0;
            baseIndex = verts.Count();
            // Vertices
            normal = new Vector3(0.0f, 1.0f, 0.0f);
            texBottomLeft = new Vector2(0.0f, 0.0f);
            position.Y = 0.0f;
            for (int segment = 0; segment < segments + 1; segment++)
            {
                position.X = (float)Math.Sin((double)angle);
                position.Z = (float)Math.Cos((double)angle);
                position.X *= (diameter1 * storyDimensions.X) / 2.0f;
                position.Z *= (diameter2 * storyDimensions.X) / 2.0f;

                position.Y = height;
                position += origin;

                verts.Add(new VertexPositionNormalTextureMod(position, normal, texBottomLeft, colorMod));
                angle += angleStep;
            }

            int centerIndex = verts.Count();
            verts.Add(new VertexPositionNormalTextureMod(origin + new Vector3(0.0f, height, 0.0f), normal, texBottomLeft, colorMod));

            angle = 0.0f;
            // indices
            for (int segment = 0; segment < segments; segment++)
            {
                indices.Add(baseIndex + segment);
                indices.Add(centerIndex);
                if (segment < segments)
                    indices.Add(baseIndex + segment + 1);
                else
                    indices.Add(baseIndex);
            }
        }
    }
}
