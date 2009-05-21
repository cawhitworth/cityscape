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
        List<VertexPositionNormalTexture> vertices = new List<VertexPositionNormalTexture>();
        List<int> indices = new List<int>();
        Vector3 origin;
        float height;

        static Random rand = new Random();

        static readonly Vector3 storyDimensions = new Vector3(0.1f, 0.1f, 0.1f);

        public IList<VertexPositionNormalTexture> Vertices
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
            this.origin = center - new Vector3(baseDimensions.X * storyDimensions.X, 0.0f, baseDimensions.Y * storyDimensions.Z);;

            this.height = (float)stories * storyDimensions.Y; ;

            // Base
            AddBox(ref vertices, ref indices,
                origin,
                storyDimensions,
                new Vector3(baseDimensions.X, 0.1f, baseDimensions.Y)
                );

            int xSize, ySize, xPos, yPos;
            GetCentreSpanningRect((int)baseDimensions.X, (int)baseDimensions.Y, true,
                            out xSize, out ySize, out xPos, out yPos);
            float xOffset = (float) xPos * storyDimensions.X;
            float yOffset = (float) yPos * storyDimensions.Z;
            
            // Main
            AddBox(ref vertices, ref indices,
                origin + new Vector3(xOffset, 0.0f, yOffset), 
                storyDimensions,
                new Vector3((float)xSize, (float) stories, (float) ySize)
                );

            int subBoxes = rand.Next(4);
            for(int subBox = 0; subBox < subBoxes; subBox++)
            {
                stories = (stories * 3) / 4;

                GetCentreSpanningRect((int)baseDimensions.X, (int)baseDimensions.Y, false,
                              out xSize, out ySize, out xPos, out yPos);
                xOffset = (float)xPos * storyDimensions.X;
                yOffset = (float)yPos * storyDimensions.Z;

                AddBox(ref vertices, ref indices,
                  origin + new Vector3(xOffset, 0.0f, yOffset),
                  storyDimensions,
                  new Vector3((float)xSize, (float)stories, (float)ySize)
                  );
            }
        }


        private static void AddBox(ref List<VertexPositionNormalTexture> verts, ref List<int> indices,
                                   Vector3 position, 
                                   Vector3 storyDimensions,
                                   Vector3 stories)
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

            verts.Add(new VertexPositionNormalTexture( frontBottomLeft, forward, texBottomLeft * faceTexScale));
            verts.Add(new VertexPositionNormalTexture( frontTopLeft, forward, texTopLeft * faceTexScale));
            verts.Add(new VertexPositionNormalTexture( frontBottomRight, forward, texBottomRight * faceTexScale));
            verts.Add(new VertexPositionNormalTexture( frontTopRight, forward, texTopRight * faceTexScale));

            indices.Add( index ); indices.Add( index + 1 ); indices.Add( index + 2 );
            indices.Add( index + 1 ); indices.Add( index + 3 ); indices.Add( index + 2 );

            
            // Right face

            texBottomLeft.X += texWidth; texTopLeft.X += texWidth;
            texBottomRight.X += texDepth; texTopRight.X += texDepth;

            index = verts.Count();
//            faceTexScale.X = dimensions.Z; faceTexScale.Y = dimensions.Y;

            verts.Add(new VertexPositionNormalTexture( frontBottomRight, right, texBottomLeft * faceTexScale));
            verts.Add(new VertexPositionNormalTexture( frontTopRight, right, texTopLeft * faceTexScale));
            verts.Add(new VertexPositionNormalTexture( backBottomRight, right, texBottomRight * faceTexScale));
            verts.Add(new VertexPositionNormalTexture( backTopRight, right, texTopRight * faceTexScale));

            indices.Add( index ); indices.Add( index + 1 ); indices.Add( index + 2 );
            indices.Add( index + 1 ); indices.Add( index + 3 ); indices.Add( index + 2 );

            // Back face
            texBottomLeft.X += texDepth; texTopLeft.X += texDepth;
            texBottomRight.X += texWidth; texTopRight.X += texWidth;
            index = verts.Count();
//            faceTexScale.X = dimensions.X; faceTexScale.Y = dimensions.Y;

            verts.Add(new VertexPositionNormalTexture( backBottomRight, back, texBottomLeft * faceTexScale));
            verts.Add(new VertexPositionNormalTexture( backTopRight, back, texTopLeft * faceTexScale));
            verts.Add(new VertexPositionNormalTexture( backBottomLeft, back, texBottomRight * faceTexScale));
            verts.Add(new VertexPositionNormalTexture( backTopLeft, back, texTopRight * faceTexScale));

            indices.Add( index ); indices.Add( index + 1 ); indices.Add( index + 2 );
            indices.Add( index + 1 ); indices.Add( index + 3 ); indices.Add( index + 2 );

            // Left face
            texBottomLeft.X += texWidth; texTopLeft.X += texWidth;
            texBottomRight.X += texDepth; texTopRight.X += texDepth;

            index = verts.Count();
//            faceTexScale.X = dimensions.Z; faceTexScale.Y = dimensions.Y;

            verts.Add(new VertexPositionNormalTexture( backBottomLeft, left, texBottomLeft * faceTexScale));
            verts.Add(new VertexPositionNormalTexture( backTopLeft, left, texTopLeft * faceTexScale));
            verts.Add(new VertexPositionNormalTexture( frontBottomLeft, left, texBottomRight * faceTexScale));
            verts.Add(new VertexPositionNormalTexture( frontTopLeft, left, texTopRight * faceTexScale));

            indices.Add( index ); indices.Add( index + 1 ); indices.Add( index + 2 );
            indices.Add( index + 1 ); indices.Add( index + 3 ); indices.Add( index + 2 );

            // Top face

            index = verts.Count();
//            faceTexScale.X = dimensions.X; faceTexScale.Y = dimensions.Z;

            verts.Add(new VertexPositionNormalTexture( frontTopLeft, up, texBottomLeftBlack * faceTexScale));
            verts.Add(new VertexPositionNormalTexture( backTopLeft, up, texTopLeftBlack * faceTexScale));
            verts.Add(new VertexPositionNormalTexture( frontTopRight, up, texBottomRightBlack * faceTexScale));
            verts.Add(new VertexPositionNormalTexture( backTopRight, up, texTopRightBlack * faceTexScale));

            indices.Add( index ); indices.Add( index + 1 ); indices.Add( index + 2 );
            indices.Add( index + 1 ); indices.Add( index + 3 ); indices.Add( index + 2 );

            // Bottom face

            index = verts.Count();
//            faceTexScale.X = dimensions.X; faceTexScale.Y = dimensions.Z;

            verts.Add(new VertexPositionNormalTexture( backBottomLeft, down, texBottomLeftBlack * faceTexScale));
            verts.Add(new VertexPositionNormalTexture( frontBottomLeft, down, texTopLeftBlack * faceTexScale));
            verts.Add(new VertexPositionNormalTexture( backBottomRight, down, texBottomRightBlack * faceTexScale));
            verts.Add(new VertexPositionNormalTexture( frontBottomRight, down, texTopRightBlack * faceTexScale));

            indices.Add( index ); indices.Add( index + 1 ); indices.Add( index + 2 );
            indices.Add( index + 1 ); indices.Add( index + 3 ); indices.Add( index + 2 );
        }
    }
}