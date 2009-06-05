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
    public class BuildingBuilder
    {
        public static Random rand = new Random();
        public static readonly Vector3 storyDimensions = new Vector3(0.1f, 0.1f, 0.1f);
        public enum Stretch { None, Horizontal, Vertical };

        public static void AddBox(ref List<VertexPositionNormalTextureMod> verts, ref List<int> indices,
                                   Vector3 position,
                                   Vector3 storyDimensions,
                                   Vector3 stories,
                                   Vector3 colorMod,
                                   Stretch stretch)
        {
            Vector3 dimensions = new Vector3(stories.X * storyDimensions.X, stories.Y * storyDimensions.Y, stories.Z * storyDimensions.Z);
            Vector3 origin = position + (dimensions / 2.0f);

            // Corners - the maths is getting increasingly redundant here...
            Vector3 backBottomLeft = new Vector3(origin.X - dimensions.X / 2.0f, origin.Y - dimensions.Y / 2.0f, origin.Z - dimensions.Z / 2.0f);
            Vector3 backBottomRight = new Vector3(origin.X + dimensions.X / 2.0f, origin.Y - dimensions.Y / 2.0f, origin.Z - dimensions.Z / 2.0f);
            Vector3 backTopLeft = new Vector3(origin.X - dimensions.X / 2.0f, origin.Y + dimensions.Y / 2.0f, origin.Z - dimensions.Z / 2.0f);
            Vector3 backTopRight = new Vector3(origin.X + dimensions.X / 2.0f, origin.Y + dimensions.Y / 2.0f, origin.Z - dimensions.Z / 2.0f);

            Vector3 frontBottomLeft = new Vector3(origin.X - dimensions.X / 2.0f, origin.Y - dimensions.Y / 2.0f, origin.Z + dimensions.Z / 2.0f);
            Vector3 frontBottomRight = new Vector3(origin.X + dimensions.X / 2.0f, origin.Y - dimensions.Y / 2.0f, origin.Z + dimensions.Z / 2.0f);
            Vector3 frontTopLeft = new Vector3(origin.X - dimensions.X / 2.0f, origin.Y + dimensions.Y / 2.0f, origin.Z + dimensions.Z / 2.0f);
            Vector3 frontTopRight = new Vector3(origin.X + dimensions.X / 2.0f, origin.Y + dimensions.Y / 2.0f, origin.Z + dimensions.Z / 2.0f);

            // These should really go in a utility function later
            Vector3 forward = new Vector3(0.0f, 0.0f, 1.0f);
            Vector3 back = new Vector3(0.0f, 0.0f, -1.0f);
            Vector3 left = new Vector3(-1.0f, 0.0f, 0.0f);
            Vector3 right = new Vector3(1.0f, 0.0f, 0.0f);
            Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);
            Vector3 down = new Vector3(0.0f, -1.0f, 0.0f);

            float texWidth = stories.X * BuildingTextureGenerator.StoryXMultiplier;
            float texDepth = stories.Z * BuildingTextureGenerator.StoryXMultiplier;
            float texHeight = stories.Y * BuildingTextureGenerator.StoryYMultiplier;

            Vector2 faceTexScale = new Vector2(1.0f, 1.0f);
            Vector2 texOrigin;
            switch (stretch)
            {
                case Stretch.Horizontal:
                    faceTexScale.Y = 0.5f;
                    texOrigin = new Vector2(rand.Next(32) * 2 * BuildingTextureGenerator.StoryXMultiplier,
                                            rand.Next(64) * BuildingTextureGenerator.StoryYMultiplier);
                    break;
                case Stretch.Vertical:
                    faceTexScale.X = 0.5f;
                    texOrigin = new Vector2(rand.Next(64) * BuildingTextureGenerator.StoryXMultiplier,
                                            rand.Next(32) * 2 * BuildingTextureGenerator.StoryYMultiplier);
                    break;
                default:
                    texOrigin = new Vector2(rand.Next(64) * BuildingTextureGenerator.StoryXMultiplier,
                                            rand.Next(64) * BuildingTextureGenerator.StoryYMultiplier);
                    break;
            }

            Vector2 texBottomLeft = texOrigin + new Vector2(0.0f, 0.0f);
            Vector2 texBottomRight = texOrigin + new Vector2(texWidth, 0.0f);
            Vector2 texTopLeft = texOrigin + new Vector2(0.0f, texHeight);
            Vector2 texTopRight = texOrigin + new Vector2(texWidth, texHeight);

            Vector2 texBottomLeftBlack = new Vector2(0.0f, 0.0f);
            Vector2 texBottomRightBlack = new Vector2(0.0f, 0.0f);
            Vector2 texTopLeftBlack = new Vector2(0.0f, 0.0f);
            Vector2 texTopRightBlack = new Vector2(0.0f, 0.0f);

            // Front face
            int index = verts.Count();

            verts.Add(new VertexPositionNormalTextureMod(frontBottomLeft, forward, texBottomLeft * faceTexScale, colorMod));
            verts.Add(new VertexPositionNormalTextureMod(frontTopLeft, forward, texTopLeft * faceTexScale, colorMod));
            verts.Add(new VertexPositionNormalTextureMod(frontBottomRight, forward, texBottomRight * faceTexScale, colorMod));
            verts.Add(new VertexPositionNormalTextureMod(frontTopRight, forward, texTopRight * faceTexScale, colorMod));

            indices.Add(index); indices.Add(index + 1); indices.Add(index + 2);
            indices.Add(index + 1); indices.Add(index + 3); indices.Add(index + 2);


            // Right face

            texBottomLeft.X += texWidth; texTopLeft.X += texWidth;
            texBottomRight.X += texDepth; texTopRight.X += texDepth;

            index = verts.Count();

            verts.Add(new VertexPositionNormalTextureMod(frontBottomRight, right, texBottomLeft * faceTexScale, colorMod));
            verts.Add(new VertexPositionNormalTextureMod(frontTopRight, right, texTopLeft * faceTexScale, colorMod));
            verts.Add(new VertexPositionNormalTextureMod(backBottomRight, right, texBottomRight * faceTexScale, colorMod));
            verts.Add(new VertexPositionNormalTextureMod(backTopRight, right, texTopRight * faceTexScale, colorMod));

            indices.Add(index); indices.Add(index + 1); indices.Add(index + 2);
            indices.Add(index + 1); indices.Add(index + 3); indices.Add(index + 2);

            // Back face
            texBottomLeft.X += texDepth; texTopLeft.X += texDepth;
            texBottomRight.X += texWidth; texTopRight.X += texWidth;
            index = verts.Count();

            verts.Add(new VertexPositionNormalTextureMod(backBottomRight, back, texBottomLeft * faceTexScale, colorMod));
            verts.Add(new VertexPositionNormalTextureMod(backTopRight, back, texTopLeft * faceTexScale, colorMod));
            verts.Add(new VertexPositionNormalTextureMod(backBottomLeft, back, texBottomRight * faceTexScale, colorMod));
            verts.Add(new VertexPositionNormalTextureMod(backTopLeft, back, texTopRight * faceTexScale, colorMod));

            indices.Add(index); indices.Add(index + 1); indices.Add(index + 2);
            indices.Add(index + 1); indices.Add(index + 3); indices.Add(index + 2);

            // Left face
            texBottomLeft.X += texWidth; texTopLeft.X += texWidth;
            texBottomRight.X += texDepth; texTopRight.X += texDepth;

            index = verts.Count();

            verts.Add(new VertexPositionNormalTextureMod(backBottomLeft, left, texBottomLeft * faceTexScale, colorMod));
            verts.Add(new VertexPositionNormalTextureMod(backTopLeft, left, texTopLeft * faceTexScale, colorMod));
            verts.Add(new VertexPositionNormalTextureMod(frontBottomLeft, left, texBottomRight * faceTexScale, colorMod));
            verts.Add(new VertexPositionNormalTextureMod(frontTopLeft, left, texTopRight * faceTexScale, colorMod));

            indices.Add(index); indices.Add(index + 1); indices.Add(index + 2);
            indices.Add(index + 1); indices.Add(index + 3); indices.Add(index + 2);

            // Top face

            index = verts.Count();

            verts.Add(new VertexPositionNormalTextureMod(frontTopLeft, up, texBottomLeftBlack * faceTexScale, colorMod));
            verts.Add(new VertexPositionNormalTextureMod(backTopLeft, up, texTopLeftBlack * faceTexScale, colorMod));
            verts.Add(new VertexPositionNormalTextureMod(frontTopRight, up, texBottomRightBlack * faceTexScale, colorMod));
            verts.Add(new VertexPositionNormalTextureMod(backTopRight, up, texTopRightBlack * faceTexScale, colorMod));

            indices.Add(index); indices.Add(index + 1); indices.Add(index + 2);
            indices.Add(index + 1); indices.Add(index + 3); indices.Add(index + 2);

            // Bottom face

            index = verts.Count();

            verts.Add(new VertexPositionNormalTextureMod(backBottomLeft, down, texBottomLeftBlack * faceTexScale, colorMod));
            verts.Add(new VertexPositionNormalTextureMod(frontBottomLeft, down, texTopLeftBlack * faceTexScale, colorMod));
            verts.Add(new VertexPositionNormalTextureMod(backBottomRight, down, texBottomRightBlack * faceTexScale, colorMod));
            verts.Add(new VertexPositionNormalTextureMod(frontBottomRight, down, texTopRightBlack * faceTexScale, colorMod));

            indices.Add(index); indices.Add(index + 1); indices.Add(index + 2);
            indices.Add(index + 1); indices.Add(index + 3); indices.Add(index + 2);
        }

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

            }

            int centerIndex = verts.Count();
            verts.Add(new VertexPositionNormalTextureMod(origin + new Vector3(0.0f, height, 0.0f), normal, texBottomLeft, colorMod));

            angle = 0.0f;
            // indices
            for (int segment = 0; segment < segments; segment++)
            {
                indices.Add(baseIndex + segment); indices.Add(centerIndex);
                if (segment < segments)
                    indices.Add(baseIndex + segment + 1);
                else
                    indices.Add(baseIndex);
            }
        }
    }
}