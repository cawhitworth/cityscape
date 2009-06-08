﻿using System;
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

        public static void AddBlankPanel(ref List<VertexPositionNormalTextureMod> verts, ref List<int> indices,
                                    Vector3 position,
                                    Vector2 stories,
                                    Vector2 XZ)
        {
            AddPanel(ref verts, ref indices, position, stories, new Vector2(0.0f, 0.0f), XZ, false, new Vector3(0.0f, 0.0f, 0.0f), Stretch.None);
        }

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

        public static void AddPanelBox(ref List<VertexPositionNormalTextureMod> verts, ref List<int> indices,
                                   Vector3 position,
                                   Vector3 storyDimensions,
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
            
            // Front
            AddPanel(ref verts, ref indices, position,
                     new Vector2(stories.X, stories.Y), texOrigin, new Vector2(1.0f, 0.0f), true, colorMod , stretch);
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

        public static void AddBox(ref List<VertexPositionNormalTextureMod> verts, ref List<int> indices,
                                   Vector3 position,
                                   Vector3 storyDimensions,
                                   Vector3 stories,
                                   Vector3 colorMod,
                                   Stretch stretch)
        {
            AddPanelBox(ref verts, ref indices, position, storyDimensions, stories, colorMod, stretch);
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