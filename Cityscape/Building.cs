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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Building
    {
        List<VertexPositionNormalTextureMod> vertices = new List<VertexPositionNormalTextureMod>();
        List<int> indices = new List<int>();
        Vector3 origin;
        float height;

        static Random rand = new Random();

        static readonly Vector3 storyDimensions = new Vector3(0.1f, 0.1f, 0.1f);

        public IList<VertexPositionNormalTextureMod> Vertices
        {
            get { return vertices.AsReadOnly(); }
        }

        public IList<int> Indices
        {
            get { return indices.AsReadOnly(); }
        }

        private void GetCentreSpanningRect(int baseWidth, int baseHeight, bool isBase,
            out int xSize, out int ySize, out int xPos, out int yPos)
        {
            int centreX = baseWidth / 2;
            int centreY = baseHeight / 2;
            do {
                if (isBase)
                {
                    xSize = (baseWidth / 2) + rand.Next(baseWidth / 4);
                    ySize = xSize;
                }
                else
                {
                    xSize = (baseWidth / 2) + rand.Next(baseWidth / 4);
                    ySize = (baseWidth / 2) + rand.Next(baseHeight / 4);
                }
                xPos = rand.Next(baseWidth - xSize);
                yPos = rand.Next(baseHeight - ySize);

            } while ( ! ( (xPos <= centreX) && (xPos + xSize > centreX) && 
                          (yPos <= centreY) && (yPos + ySize > centreY)));

        }

        public Building(Vector3 center, int stories, Vector2 baseDimensions)
        {
            this.origin = center - new Vector3(baseDimensions.X * (storyDimensions.X / 2.0f), 0.0f, baseDimensions.Y * (storyDimensions.Z / 2.0f));;

            this.height = (float)stories * storyDimensions.Y; ;

            Vector3 colorMod;
            
            switch( rand.Next(3) )
            {

                case 1: // Yellow tint
                    colorMod = new Vector3(1.0f, 1.0f, 0.8f);
                    break;

                case 2: // Blue tint
                    colorMod = new Vector3(0.8f, 1.0f, 1.0f);
                    break;

                case 0: // White
                default:
                    colorMod = new Vector3(1.0f, 1.0f, 1.0f);
                    break;
            }

            // Base
            AddBox(ref vertices, ref indices,
                origin,
                storyDimensions,
                new Vector3(baseDimensions.X, 0.1f, baseDimensions.Y),
                colorMod
                );

            int baseWidth = (int)baseDimensions.X, baseHeight = (int) baseDimensions.Y;

            int xSize = (baseWidth / 2) + rand.Next(baseWidth / 4);
            int ySize = (baseHeight / 2) + rand.Next(baseHeight / 4);

            int xPos = rand.Next(baseWidth - (xSize + 2)) + 1;
            int yPos = rand.Next(baseHeight - (ySize + 2)) + 1;

            System.Console.WriteLine("{0} {1} {2} {3}", xPos, yPos, xSize, ySize);

            float xOffset = (float) xPos * storyDimensions.X;
            float yOffset = (float) yPos * storyDimensions.Z;
            
            // Main
            AddBox(ref vertices, ref indices,
                origin + new Vector3(xOffset, 0.0f, yOffset), 
                storyDimensions,
                new Vector3((float)xSize, (float) stories, (float) ySize),
                colorMod
                );

            // Left lump

            int xPos2 = rand.Next(xPos-1);
            int yPos2 = yPos + 1 + rand.Next(ySize / 2);
            int xSize2 = xPos - xPos2;
            int maxYSize = (ySize - 2) - (yPos2 - yPos) + 1;
            int ySize2 = maxYSize / 2 + rand.Next( maxYSize / 2 );
            System.Console.WriteLine("{0} {1} {2} {3} m{4}", xPos2, yPos2, xSize2, ySize2, maxYSize);

            xOffset = (float) xPos2 * storyDimensions.X;
            yOffset = (float) yPos2 * storyDimensions.Z;
            AddBox(ref vertices, ref indices,
                origin + new Vector3(xOffset, 0.0f, yOffset), 
                storyDimensions,
                new Vector3((float)xSize2, (float) ((stories / 4) * rand.Next(4)), (float) ySize2),
                colorMod
                );

            // Front lump

            xPos2 = xPos + 1 + rand.Next(xSize / 2);
            yPos2 = rand.Next(yPos - 1);
            ySize2 = yPos - yPos2;
            int maxXSize = (xSize - 2) - (xPos2 - xPos) + 1;
            xSize2 = maxXSize / 2 + rand.Next( maxXSize / 2 );
            System.Console.WriteLine("{0} {1} {2} {3} m{4}", xPos2, yPos2, xSize2, ySize2, maxYSize);

            xOffset = (float) xPos2 * storyDimensions.X;
            yOffset = (float) yPos2 * storyDimensions.Z;
            AddBox(ref vertices, ref indices,
                origin + new Vector3(xOffset, 0.0f, yOffset), 
                storyDimensions,
                new Vector3((float)xSize2, (float) ((stories / 4) * rand.Next(4)), (float) ySize2),
                colorMod
                );

            // Right lump

            xPos2 = xPos + xSize - 1;
            yPos2 = yPos + 1 + rand.Next(ySize / 2);
            xSize2 = rand.Next(baseWidth - (xPos2 + 1)) + 2;
            maxYSize = (ySize - 2) - (yPos2 - yPos) + 1;
            ySize2 = maxYSize / 2 + rand.Next( maxYSize / 2 );
            System.Console.WriteLine("{0} {1} {2} {3} m{4}", xPos2, yPos2, xSize2, ySize2, maxYSize);

            xOffset = (float) xPos2 * storyDimensions.X;
            yOffset = (float) yPos2 * storyDimensions.Z;
            AddBox(ref vertices, ref indices,
                origin + new Vector3(xOffset, 0.0f, yOffset), 
                storyDimensions,
                new Vector3((float)xSize2, (float) ((stories / 4) * rand.Next(4)), (float) ySize2),
                colorMod
                );

            // Back lump

            yPos2 = yPos + ySize - 1;
            xPos2 = xPos + 1 + rand.Next(xSize / 2);
            ySize2 = rand.Next(baseWidth - (yPos2 + 1)) + 2;
            maxXSize = (xSize - 2) - (xPos2 - xPos) + 1;
            xSize2 = maxXSize / 2 + rand.Next( maxXSize / 2 );
            System.Console.WriteLine("{0} {1} {2} {3} m{4}", xPos2, yPos2, xSize2, ySize2, maxYSize);

            xOffset = (float) xPos2 * storyDimensions.X;
            yOffset = (float) yPos2 * storyDimensions.Z;
            AddBox(ref vertices, ref indices,
                origin + new Vector3(xOffset, 0.0f, yOffset), 
                storyDimensions,
                new Vector3((float)xSize2, (float) ((stories / 4) * rand.Next(4)), (float) ySize2),
                colorMod
                );

        }


        private static void AddBox(ref List<VertexPositionNormalTextureMod> verts, ref List<int> indices,
                                   Vector3 position, 
                                   Vector3 storyDimensions,
                                   Vector3 stories,
                                   Vector3 colorMod)
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
    }
}