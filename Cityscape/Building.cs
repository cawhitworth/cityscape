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
    public interface IBuilding
    {
        IList<VertexPositionNormalTextureMod> Vertices
        {
            get;
        }
        IList<int> Indices
        {
            get;
        }
    }

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
            Vector3 backBottomLeft   = new Vector3( origin.X - dimensions.X / 2.0f, origin.Y - dimensions.Y / 2.0f, origin.Z - dimensions.Z / 2.0f);
            Vector3 backBottomRight  = new Vector3( origin.X + dimensions.X / 2.0f, origin.Y - dimensions.Y / 2.0f, origin.Z - dimensions.Z / 2.0f);
            Vector3 backTopLeft      = new Vector3( origin.X - dimensions.X / 2.0f, origin.Y + dimensions.Y / 2.0f, origin.Z - dimensions.Z / 2.0f);
            Vector3 backTopRight     = new Vector3( origin.X + dimensions.X / 2.0f, origin.Y + dimensions.Y / 2.0f, origin.Z - dimensions.Z / 2.0f);

            Vector3 frontBottomLeft  = new Vector3( origin.X - dimensions.X / 2.0f, origin.Y - dimensions.Y / 2.0f, origin.Z + dimensions.Z / 2.0f);
            Vector3 frontBottomRight = new Vector3( origin.X + dimensions.X / 2.0f, origin.Y - dimensions.Y / 2.0f, origin.Z + dimensions.Z / 2.0f);
            Vector3 frontTopLeft     = new Vector3( origin.X - dimensions.X / 2.0f, origin.Y + dimensions.Y / 2.0f, origin.Z + dimensions.Z / 2.0f);
            Vector3 frontTopRight    = new Vector3( origin.X + dimensions.X / 2.0f, origin.Y + dimensions.Y / 2.0f, origin.Z + dimensions.Z / 2.0f);

            // These should really go in a utility function later
            Vector3 forward = new Vector3(0.0f, 0.0f, 1.0f);
            Vector3 back = new Vector3(0.0f, 0.0f, -1.0f);
            Vector3 left    = new Vector3(-1.0f, 0.0f, 0.0f);
            Vector3 right   = new Vector3(1.0f, 0.0f, 0.0f);
            Vector3 up      = new Vector3(0.0f, 1.0f, 0.0f);
            Vector3 down    = new Vector3(0.0f, -1.0f, 0.0f);

            float texWidth = stories.X * BuildingTextureGenerator.StoryXMultiplier;
            float texDepth = stories.Z * BuildingTextureGenerator.StoryXMultiplier;
            float texHeight = stories.Y * BuildingTextureGenerator.StoryYMultiplier;

            Vector2 texOrigin = new Vector2(rand.Next(64) * BuildingTextureGenerator.StoryXMultiplier, 
                                            rand.Next(64) * BuildingTextureGenerator.StoryYMultiplier);

            Vector2 texBottomLeft = texOrigin + new Vector2(0.0f, 0.0f);
            Vector2 texBottomRight = texOrigin + new Vector2(texWidth, 0.0f);
            Vector2 texTopLeft = texOrigin + new Vector2(0.0f, texHeight);
            Vector2 texTopRight = texOrigin + new Vector2(texWidth, texHeight);

            Vector2 texBottomLeftBlack = new Vector2(0.0f, 0.0f);
            Vector2 texBottomRightBlack = new Vector2(0.0f, 0.0f);
            Vector2 texTopLeftBlack = new Vector2(0.0f, 0.0f);
            Vector2 texTopRightBlack = new Vector2(0.0f, 0.0f);

            Vector2 faceTexScale = new Vector2(1.0f, 1.0f);

            switch (stretch)
            {
                case Stretch.Horizontal:
                    faceTexScale.Y = 0.5f;
                    break;
                case Stretch.Vertical:
                    faceTexScale.X = 0.5f;
                    break;
            }

            // Front face
            int index =  verts.Count();
//            Vector2 faceTexScale = new Vector2(dimensions.X, dimensions.Y);

            verts.Add(new VertexPositionNormalTextureMod( frontBottomLeft, forward, texBottomLeft * faceTexScale, colorMod));
            verts.Add(new VertexPositionNormalTextureMod( frontTopLeft, forward, texTopLeft * faceTexScale, colorMod));
            verts.Add(new VertexPositionNormalTextureMod( frontBottomRight, forward, texBottomRight * faceTexScale, colorMod));
            verts.Add(new VertexPositionNormalTextureMod( frontTopRight, forward, texTopRight * faceTexScale, colorMod));

            indices.Add( index ); indices.Add( index + 1 ); indices.Add( index + 2 );
            indices.Add( index + 1 ); indices.Add( index + 3 ); indices.Add( index + 2 );

            
            // Right face

            texBottomLeft.X += texWidth; texTopLeft.X += texWidth;
            texBottomRight.X += texDepth; texTopRight.X += texDepth;

            index = verts.Count();
//            faceTexScale.X = dimensions.Z; faceTexScale.Y = dimensions.Y;

            verts.Add(new VertexPositionNormalTextureMod( frontBottomRight, right, texBottomLeft * faceTexScale, colorMod));
            verts.Add(new VertexPositionNormalTextureMod( frontTopRight, right, texTopLeft * faceTexScale, colorMod));
            verts.Add(new VertexPositionNormalTextureMod( backBottomRight, right, texBottomRight * faceTexScale, colorMod));
            verts.Add(new VertexPositionNormalTextureMod( backTopRight, right, texTopRight * faceTexScale, colorMod));

            indices.Add( index ); indices.Add( index + 1 ); indices.Add( index + 2 );
            indices.Add( index + 1 ); indices.Add( index + 3 ); indices.Add( index + 2 );

            // Back face
            texBottomLeft.X += texDepth; texTopLeft.X += texDepth;
            texBottomRight.X += texWidth; texTopRight.X += texWidth;
            index = verts.Count();
//            faceTexScale.X = dimensions.X; faceTexScale.Y = dimensions.Y;

            verts.Add(new VertexPositionNormalTextureMod( backBottomRight, back, texBottomLeft * faceTexScale, colorMod));
            verts.Add(new VertexPositionNormalTextureMod( backTopRight, back, texTopLeft * faceTexScale, colorMod));
            verts.Add(new VertexPositionNormalTextureMod( backBottomLeft, back, texBottomRight * faceTexScale, colorMod));
            verts.Add(new VertexPositionNormalTextureMod( backTopLeft, back, texTopRight * faceTexScale, colorMod));

            indices.Add( index ); indices.Add( index + 1 ); indices.Add( index + 2 );
            indices.Add( index + 1 ); indices.Add( index + 3 ); indices.Add( index + 2 );

            // Left face
            texBottomLeft.X += texWidth; texTopLeft.X += texWidth;
            texBottomRight.X += texDepth; texTopRight.X += texDepth;

            index = verts.Count();
//            faceTexScale.X = dimensions.Z; faceTexScale.Y = dimensions.Y;

            verts.Add(new VertexPositionNormalTextureMod( backBottomLeft, left, texBottomLeft * faceTexScale, colorMod));
            verts.Add(new VertexPositionNormalTextureMod( backTopLeft, left, texTopLeft * faceTexScale, colorMod));
            verts.Add(new VertexPositionNormalTextureMod( frontBottomLeft, left, texBottomRight * faceTexScale, colorMod));
            verts.Add(new VertexPositionNormalTextureMod( frontTopLeft, left, texTopRight * faceTexScale, colorMod));

            indices.Add( index ); indices.Add( index + 1 ); indices.Add( index + 2 );
            indices.Add( index + 1 ); indices.Add( index + 3 ); indices.Add( index + 2 );

            // Top face

            index = verts.Count();
//            faceTexScale.X = dimensions.X; faceTexScale.Y = dimensions.Z;

            verts.Add(new VertexPositionNormalTextureMod( frontTopLeft, up, texBottomLeftBlack * faceTexScale, colorMod));
            verts.Add(new VertexPositionNormalTextureMod( backTopLeft, up, texTopLeftBlack * faceTexScale, colorMod));
            verts.Add(new VertexPositionNormalTextureMod( frontTopRight, up, texBottomRightBlack * faceTexScale, colorMod));
            verts.Add(new VertexPositionNormalTextureMod( backTopRight, up, texTopRightBlack * faceTexScale, colorMod));

            indices.Add( index ); indices.Add( index + 1 ); indices.Add( index + 2 );
            indices.Add( index + 1 ); indices.Add( index + 3 ); indices.Add( index + 2 );

            // Bottom face

            index = verts.Count();
//            faceTexScale.X = dimensions.X; faceTexScale.Y = dimensions.Z;

            verts.Add(new VertexPositionNormalTextureMod( backBottomLeft, down, texBottomLeftBlack * faceTexScale, colorMod));
            verts.Add(new VertexPositionNormalTextureMod( frontBottomLeft, down, texTopLeftBlack * faceTexScale, colorMod));
            verts.Add(new VertexPositionNormalTextureMod( backBottomRight, down, texBottomRightBlack * faceTexScale, colorMod));
            verts.Add(new VertexPositionNormalTextureMod( frontBottomRight, down, texTopRightBlack * faceTexScale, colorMod));

            indices.Add( index ); indices.Add( index + 1 ); indices.Add( index + 2 );
            indices.Add( index + 1 ); indices.Add( index + 3 ); indices.Add( index + 2 );
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
            float circumference = (float)Math.PI * diameter;
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
            
            // Vertices
            for(int segment = 0; segment < segments + 1; segment++)
            {
                normal.X = (float)Math.Sin((double)angle);
                normal.Y = 0.0f;
                normal.Z = (float)Math.Cos((double)angle);
                position = normal;
                position *= (diameter * storyDimensions.X) / 2.0f;
                position += origin;

                verts.Add(new VertexPositionNormalTextureMod(position, normal, texBottomLeft, colorMod));
                position += new Vector3(0.0f, height, 0.0f);
                verts.Add(new VertexPositionNormalTextureMod(position, normal, texTopLeft, colorMod));

                texBottomLeft += new Vector2(texWidth, 0.0f);
                texTopLeft += new Vector2(texWidth, 0.0f);
                angle += (2.0f * (float)(Math.PI)) / (float)segments;
            }

            int thisSegment, nextSegment;
            // Indices
            for(int segment = 0; segment < segments; segment++)
            {
                thisSegment = baseIndex + (segment * 2);
                nextSegment = thisSegment + 2;
//                if (nextSegment >= baseIndex + (segments * 2))
//                    nextSegment = baseIndex ;

                indices.Add(thisSegment); indices.Add(thisSegment + 1); indices.Add(nextSegment);
                indices.Add(nextSegment); indices.Add(thisSegment + 1); indices.Add(nextSegment+1);
            }
            
            // Cap

            baseIndex = verts.Count();
            // Vertices
            normal = new Vector3(0.0f, 1.0f, 0.0f);
            texBottomLeft = new Vector2(0.0f, 0.0f);
            angle = 0;
            position.Y = 0.0f;
            for(int segment = 0; segment < segments + 1; segment++)
            {
                position.X = (float)Math.Sin((double)angle);
                position.Z = (float)Math.Cos((double)angle);
                position *= (diameter * storyDimensions.X) / 2.0f;

                position.Y = height;
                position += origin;

                verts.Add(new VertexPositionNormalTextureMod(position, normal, texBottomLeft, colorMod));
                angle += (2.0f * (float)(Math.PI)) / (float)segments;
            }

            int centerIndex = verts.Count();
            verts.Add(new VertexPositionNormalTextureMod(origin + new Vector3(0.0f, height, 0.0f), normal, texBottomLeft, colorMod));

            // indices
            for(int segment = 0; segment < segments; segment++)
            {
                indices.Add(baseIndex + segment); indices.Add(centerIndex);
                if (segment < segments)
                    indices.Add(baseIndex + segment + 1);
                else
                    indices.Add(baseIndex);
            }
        }
    }

    public class BaseBuilding : IBuilding
    {
        protected List<VertexPositionNormalTextureMod> vertices = new List<VertexPositionNormalTextureMod>();
        protected List<int> indices = new List<int>();
        protected Vector3 origin;
        protected Vector3 center;
        protected Vector3 colorMod;
        protected BuildingBuilder.Stretch stretch = BuildingBuilder.Stretch.None;
        protected float height;

        public BaseBuilding(Vector3 center, int stories, Vector2 baseDimensions)
        {
            this.center = center;
            origin = center - new Vector3(baseDimensions.X * (BuildingBuilder.storyDimensions.X / 2.0f), 0.0f, baseDimensions.Y * (BuildingBuilder.storyDimensions.Z / 2.0f)); ;
            height = (float)stories;

            float tint = 1.0f - (0.2f * (float)BuildingBuilder.rand.NextDouble());
            switch (BuildingBuilder.rand.Next(3))
            {

                case 1: // Yellow tint
                    colorMod = new Vector3(1.0f, 1.0f, tint);
                    break;

                case 2: // Blue tint
                    colorMod = new Vector3(tint, tint, 1.0f);
                    break;

                case 0: // White
                default:
                    colorMod = new Vector3(1.0f, 1.0f, 1.0f);
                    break;
            }

            switch(BuildingBuilder.rand.Next(3))
            {
                case 0: stretch = BuildingBuilder.Stretch.None; break;
                case 1: stretch = BuildingBuilder.Stretch.Horizontal; break;
                case 2: if (stories % 2 == 0) { stretch = BuildingBuilder.Stretch.Vertical; } break;
            }
        }

        protected void AddBox(Vector3 dimensions)
        {
            BuildingBuilder.AddBox(
                ref vertices, ref indices,
                origin, BuildingBuilder.storyDimensions, dimensions,
                colorMod, stretch);
        }
 
        protected void AddBox(Vector3 offset, Vector3 dimensions)
        {
            BuildingBuilder.AddBox(
                ref vertices, ref indices,
                origin + offset, BuildingBuilder.storyDimensions, dimensions,
                colorMod, stretch);
        }

        protected void AddCylinder(int stories, float diameter, int segments)
        {
            BuildingBuilder.AddCylinder(
                ref vertices, ref indices,
                center, BuildingBuilder.storyDimensions,
                (float) stories, diameter,
                colorMod, stretch, segments);
        }

        public IList<VertexPositionNormalTextureMod> Vertices
        {
            get { return vertices.AsReadOnly(); }
        }

        public IList<int> Indices
        {
            get { return indices.AsReadOnly(); }
        }
    }

    public class SimpleBuilding : BaseBuilding
    {
        public SimpleBuilding(Vector3 center, int stories, Vector2 baseDimensions) : base(center, stories, baseDimensions)
        {
            // Base
            AddBox(new Vector3(baseDimensions.X, 0.1f, baseDimensions.Y));

            int baseWidth = (int)baseDimensions.X, baseHeight = (int) baseDimensions.Y;

            int xSize = (baseWidth / 2) + BuildingBuilder.rand.Next(baseWidth / 4);
            int ySize = (baseHeight / 2) + BuildingBuilder.rand.Next(baseHeight / 4);

            int xPos = BuildingBuilder.rand.Next(baseWidth - (xSize + 2)) + 1;
            int yPos = BuildingBuilder.rand.Next(baseHeight - (ySize + 2)) + 1;

            float xOffset = (float) xPos * BuildingBuilder.storyDimensions.X;
            float yOffset = (float) yPos * BuildingBuilder.storyDimensions.Z;
            
            // Main
            AddBox( new Vector3(xOffset, 0.0f, yOffset), new Vector3((float)xSize, (float) stories, (float) ySize) );
        }
    }

    public class SimpleCylinderBuilding : BaseBuilding
    {
        public SimpleCylinderBuilding(Vector3 center, int stories, Vector2 baseDimensions) : base(center, stories, baseDimensions)
        {
            AddBox(new Vector3(baseDimensions.X, 0.1f, baseDimensions.Y));
            AddCylinder(stories, (float)BuildingBuilder.rand.Next((int)baseDimensions.X), 16);
        }
    }

    public class UglyModernBuilding : BaseBuilding
    {
        public UglyModernBuilding(Vector3 center, int stories, Vector2 baseDimensions): base(center, stories, baseDimensions)
        {
            // Base
            AddBox( new Vector3(baseDimensions.X, 0.1f, baseDimensions.Y) );

            int baseWidth = (int)baseDimensions.X, baseHeight = (int) baseDimensions.Y;

            int xSize = (baseWidth / 2) + BuildingBuilder.rand.Next(baseWidth / 4);
            int ySize = (baseHeight / 2) + BuildingBuilder.rand.Next(baseHeight / 4);

            int xPos = BuildingBuilder.rand.Next(baseWidth - (xSize + 2)) + 1;
            int yPos = BuildingBuilder.rand.Next(baseHeight - (ySize + 2)) + 1;

            float xOffset = (float) xPos * BuildingBuilder.storyDimensions.X;
            float yOffset = (float) yPos * BuildingBuilder.storyDimensions.Z;
            
            // Main
            AddBox( new Vector3(xOffset, 0.0f, yOffset), new Vector3((float)xSize, (float) stories, (float) ySize) );

            // Left lump

            int xPos2 = BuildingBuilder.rand.Next(xPos-1);
            int yPos2 = yPos + 1 + BuildingBuilder.rand.Next(ySize / 2);
            int xSize2 = xPos - xPos2;
            int maxYSize = (ySize - 2) - (yPos2 - yPos) + 1;
            int ySize2 = maxYSize / 2 + BuildingBuilder.rand.Next( maxYSize / 2 );

            xOffset = (float) xPos2 * BuildingBuilder.storyDimensions.X;
            yOffset = (float) yPos2 * BuildingBuilder.storyDimensions.Z;
            AddBox(new Vector3(xOffset, 0.0f, yOffset),
                   new Vector3((float)xSize2, (float) ((stories / 4) * BuildingBuilder.rand.Next(4)), (float) ySize2) );

            // Front lump

            xPos2 = xPos + 1 + BuildingBuilder.rand.Next(xSize / 2);
            yPos2 = BuildingBuilder.rand.Next(yPos - 1);
            ySize2 = yPos - yPos2;
            int maxXSize = (xSize - 2) - (xPos2 - xPos) + 1;
            xSize2 = maxXSize / 2 + BuildingBuilder.rand.Next( maxXSize / 2 );

            xOffset = (float) xPos2 * BuildingBuilder.storyDimensions.X;
            yOffset = (float) yPos2 * BuildingBuilder.storyDimensions.Z;
            AddBox(new Vector3(xOffset, 0.0f, yOffset), 
                   new Vector3((float)xSize2, (float) ((stories / 4) * BuildingBuilder.rand.Next(4)), (float) ySize2) );

            // Right lump

            xPos2 = xPos + xSize - 1;
            yPos2 = yPos + 1 + BuildingBuilder.rand.Next(ySize / 2);
            xSize2 = BuildingBuilder.rand.Next(baseWidth - (xPos2 + 1)) + 2;
            maxYSize = (ySize - 2) - (yPos2 - yPos) + 1;
            ySize2 = maxYSize / 2 + BuildingBuilder.rand.Next( maxYSize / 2 );

            xOffset = (float) xPos2 * BuildingBuilder.storyDimensions.X;
            yOffset = (float) yPos2 * BuildingBuilder.storyDimensions.Z;
            AddBox(new Vector3(xOffset, 0.0f, yOffset), 
                   new Vector3((float)xSize2, (float) ((stories / 4) * BuildingBuilder.rand.Next(4)), (float) ySize2));

            // Back lump

            yPos2 = yPos + ySize - 1;
            xPos2 = xPos + 1 + BuildingBuilder.rand.Next(xSize / 2);
            ySize2 = BuildingBuilder.rand.Next(baseWidth - (yPos2 + 1)) + 2;
            maxXSize = (xSize - 2) - (xPos2 - xPos) + 1;
            xSize2 = maxXSize / 2 + BuildingBuilder.rand.Next( maxXSize / 2 );

            xOffset = (float) xPos2 * BuildingBuilder.storyDimensions.X;
            yOffset = (float) yPos2 * BuildingBuilder.storyDimensions.Z;
            AddBox(new Vector3(xOffset, 0.0f, yOffset), 
                   new Vector3((float)xSize2, (float) ((stories / 4) * BuildingBuilder.rand.Next(4)), (float) ySize2));

        }


    }
}