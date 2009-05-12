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
    public class Building : Microsoft.Xna.Framework.DrawableGameComponent
    {
        VertexPositionNormalTexture[] vertices;
        Int16[] indices;
        Effect effect;
        Matrix model;
        VertexDeclaration vertDecl;
        Texture2D bldTex;
        IGraphicsDeviceService graphicsDeviceService;
        ICamera camera;

        public Building(Game game)
            : base(game)
        {
        }

        public void updateGeometry( List<VertexPositionNormalTexture> listVertices, List<Int16> listIndices )
        {
            vertices = new VertexPositionNormalTexture[listVertices.Count()];
            indices = new Int16[listIndices.Count()];
            for (int index = 0; index < listVertices.Count(); index++)
                vertices[index] = listVertices[index];
            for (int index = 0; index < listIndices.Count(); index++)
                indices[index] = listIndices[index];
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            graphicsDeviceService = (IGraphicsDeviceService)Game.Services.GetService(typeof(IGraphicsDeviceService));
            camera = (ICamera) Game.Services.GetService(typeof(ICamera));

            List<VertexPositionNormalTexture> listVert = new List<VertexPositionNormalTexture>();
            List<Int16> listIndex = new List<Int16>();

            AddBox(ref listVert, ref listIndex, new Vector3(0.0f, -0.5f, 0.0f), new Vector3(1.0f, 0.1f, 1.0f));

            AddBox(ref listVert, ref listIndex, new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.5f, 1.0f, 0.5f));

            updateGeometry(listVert, listIndex);

            bldTex = Game.Content.Load<Texture2D>("bldtex");
            effect = Game.Content.Load<Effect>("BuildingEffect");
            vertDecl = new VertexDeclaration(graphicsDeviceService.GraphicsDevice, VertexPositionNormalTexture.VertexElements);
            effect.CurrentTechnique = effect.Techniques["DefaultTechnique"];
            effect.Parameters["texBld"].SetValue(bldTex);
            effect.Parameters["Diffuse"].SetValue(new Vector4(0.7f, 0.7f, 0.7f, 0.0f));
            effect.Parameters["Ambient"].SetValue(new Vector4(0.0f, 0.0f, 0.0f, 0.0f));
            effect.Parameters["Light0Position"].SetValue(new Vector3(0.0f, 1.0f, 0.0f));

            model = Matrix.CreateScale(2.0f);

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            effect.Parameters["World"].SetValue(model);
            effect.Parameters["View"].SetValue(camera.View);
            effect.Parameters["Projection"].SetValue(camera.Projection);
            effect.Parameters["Light0Position"].SetValue(camera.CameraPos);
            graphicsDeviceService.GraphicsDevice.VertexDeclaration = vertDecl;

            effect.Begin();
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Begin();

                graphicsDeviceService.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(
                  PrimitiveType.TriangleList,
                  vertices,
                  0,
                  vertices.Count(),
                  indices,
                  0,
                  indices.Count() / 3);

                pass.End();
            }
            effect.End();



            base.Draw(gameTime);
        }

        private void AddBox(ref List<VertexPositionNormalTexture> verts, ref List<Int16> indices, Vector3 origin, Vector3 dimensions)
        {
            // Corners
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

            Vector2 texBottomLeft = new Vector2(0.0f, 0.0f);
            Vector2 texBottomRight = new Vector2(1.0f, 0.0f);
            Vector2 texTopLeft = new Vector2(0.0f, 1.0f);
            Vector2 texTopRight = new Vector2(1.0f, 1.0f);

            // Front face
            Int16 index = (Int16)verts.Count();
            Vector2 faceTexScale = new Vector2(dimensions.X, dimensions.Y);

            verts.Add(new VertexPositionNormalTexture( frontBottomLeft, forward, texBottomLeft * faceTexScale));
            verts.Add(new VertexPositionNormalTexture( frontTopLeft, forward, texTopLeft * faceTexScale));
            verts.Add(new VertexPositionNormalTexture( frontBottomRight, forward, texBottomRight * faceTexScale));
            verts.Add(new VertexPositionNormalTexture( frontTopRight, forward, texTopRight * faceTexScale));

            indices.Add( index );
            indices.Add( (Int16)(index + 1) );
            indices.Add( (Int16)(index + 2) );

            indices.Add( (Int16)(index + 1) );
            indices.Add( (Int16)(index + 3) );
            indices.Add( (Int16)(index + 2) );

            // Left face
            index = (Int16)verts.Count();
            faceTexScale.X = -dimensions.Z; faceTexScale.Y = dimensions.Y;

            verts.Add(new VertexPositionNormalTexture( backBottomLeft, left, texBottomLeft * faceTexScale));
            verts.Add(new VertexPositionNormalTexture( backTopLeft, left, texTopLeft * faceTexScale));
            verts.Add(new VertexPositionNormalTexture( frontBottomLeft, left, texBottomRight * faceTexScale));
            verts.Add(new VertexPositionNormalTexture( frontTopLeft, left, texTopRight * faceTexScale));

            indices.Add( index );
            indices.Add( (Int16)(index + 1) );
            indices.Add( (Int16)(index + 2) );

            indices.Add( (Int16)(index + 1) );
            indices.Add( (Int16)(index + 3) );
            indices.Add( (Int16)(index + 2) );

            // Right face

            index = (Int16)verts.Count();
            faceTexScale.X = dimensions.Z; faceTexScale.Y = dimensions.Y;

            verts.Add(new VertexPositionNormalTexture( frontBottomRight, right, texBottomLeft * faceTexScale));
            verts.Add(new VertexPositionNormalTexture( frontTopRight, right, texTopLeft * faceTexScale));
            verts.Add(new VertexPositionNormalTexture( backBottomRight, right, texBottomRight * faceTexScale));
            verts.Add(new VertexPositionNormalTexture( backTopRight, right, texTopRight * faceTexScale));

            indices.Add( index );
            indices.Add( (Int16)(index + 1) );
            indices.Add( (Int16)(index + 2) );

            indices.Add( (Int16)(index + 1) );
            indices.Add( (Int16)(index + 3) );
            indices.Add( (Int16)(index + 2) );

            // Back face
            index = (Int16)verts.Count();
            faceTexScale.X = -dimensions.X; faceTexScale.Y = dimensions.Y;

            verts.Add(new VertexPositionNormalTexture( backBottomRight, back, texBottomLeft * faceTexScale));
            verts.Add(new VertexPositionNormalTexture( backTopRight, back, texTopLeft * faceTexScale));
            verts.Add(new VertexPositionNormalTexture( backBottomLeft, back, texBottomRight * faceTexScale));
            verts.Add(new VertexPositionNormalTexture( backTopLeft, back, texTopRight * faceTexScale));

            indices.Add( index );
            indices.Add( (Int16)(index + 1) );
            indices.Add( (Int16)(index + 2) );

            indices.Add( (Int16)(index + 1) );
            indices.Add( (Int16)(index + 3) );
            indices.Add( (Int16)(index + 2) );

            // Top face

            index = (Int16)verts.Count();
            faceTexScale.X = dimensions.X; faceTexScale.Y = dimensions.Z;

            verts.Add(new VertexPositionNormalTexture( frontTopLeft, up, texBottomLeft * faceTexScale));
            verts.Add(new VertexPositionNormalTexture( backTopLeft, up, texTopLeft * faceTexScale));
            verts.Add(new VertexPositionNormalTexture( frontTopRight, up, texBottomRight * faceTexScale));
            verts.Add(new VertexPositionNormalTexture( backTopRight, up, texTopRight * faceTexScale));

            indices.Add( index );
            indices.Add( (Int16)(index + 1) );
            indices.Add( (Int16)(index + 2) );

            indices.Add( (Int16)(index + 1) );
            indices.Add( (Int16)(index + 3) );
            indices.Add( (Int16)(index + 2) );

            // Bottom face

            index = (Int16)verts.Count();
            faceTexScale.X = -dimensions.X; faceTexScale.Y = -dimensions.Z;

            verts.Add(new VertexPositionNormalTexture( backBottomLeft, down, texBottomLeft * faceTexScale));
            verts.Add(new VertexPositionNormalTexture( frontBottomLeft, down, texTopLeft * faceTexScale));
            verts.Add(new VertexPositionNormalTexture( backBottomRight, down, texBottomRight * faceTexScale));
            verts.Add(new VertexPositionNormalTexture( frontBottomRight, down, texTopRight * faceTexScale));

            indices.Add( index );
            indices.Add( (Int16)(index + 1) );
            indices.Add( (Int16)(index + 2) );

            indices.Add( (Int16)(index + 1) );
            indices.Add( (Int16)(index + 3) );
            indices.Add( (Int16)(index + 2) );

        }
    }
}