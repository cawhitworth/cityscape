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
using System.Collections.Generic;

namespace Cityscape
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Building : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private VertexPositionNormalTexture[] vertices;
        private Int16[] indices;
        private Effect effect;
        private Matrix model;
        private VertexDeclaration vertDecl;
        private Texture2D bldTex;

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
            List<VertexPositionNormalTexture> listVert = new List<VertexPositionNormalTexture>();
            List<Int16> listIndex = new List<Int16>();

            AddBox(ref listVert, ref listIndex, new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f));

            updateGeometry(listVert, listIndex);

            bldTex = Game.Content.Load<Texture2D>("bldtex");
            effect = Game.Content.Load<Effect>("BuildingEffect");
            vertDecl = new VertexDeclaration(Game.GraphicsDevice, VertexPositionNormalTexture.VertexElements);
            effect.CurrentTechnique = effect.Techniques["DefaultTechnique"];
            effect.Parameters["texBld"].SetValue(bldTex);
            effect.Parameters["Diffuse"].SetValue(new Vector4(0.7f, 0.7f, 0.7f, 0.0f));
            effect.Parameters["Ambient"].SetValue(new Vector4(0.0f, 0.0f, 0.0f, 0.0f));
            effect.Parameters["Light0Position"].SetValue(new Vector3(0.0f, 1.0f, 0.0f));

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
            model = Matrix.Identity;

            effect.Parameters["World"].SetValue(model);
            effect.Parameters["View"].SetValue(((Game1)Game).view);
            effect.Parameters["Projection"].SetValue(((Game1)Game).projection);
            effect.Parameters["Light0Position"].SetValue(((Game1)Game).cameraPos);
            Game.GraphicsDevice.VertexDeclaration = vertDecl;

            effect.Begin();
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Begin();

                GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(
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
            Vector3 back    = new Vector3(0.0f, 0.0f, -1.0f);
            Vector3 left    = new Vector3(-1.0f, 0.0f, 0.0f);
            Vector3 right   = new Vector3(1.0f, 0.0f, 0.0f);
            Vector3 up      = new Vector3(0.0f, 1.0f, 0.0f);
            Vector3 down    = new Vector3(0.0f, -1.0f, 0.0f);

            Vector2 texBottomLeft = new Vector2(0.0f, 0.0f);
            Vector2 texBottomRight = new Vector2(1.0f, 0.0f);
            Vector2 texTopLeft = new Vector2(0.0f, 1.0f);
            Vector2 texTopRight = new Vector2(1.0f, 1.0f);

            Int16 index = (Int16)verts.Count();
            // Front face
            verts.Add(new VertexPositionNormalTexture( frontBottomLeft, forward, texBottomLeft));
            verts.Add(new VertexPositionNormalTexture( frontTopLeft, forward, texTopLeft));
            verts.Add(new VertexPositionNormalTexture( frontBottomRight, forward, texBottomRight));
            verts.Add(new VertexPositionNormalTexture( frontTopRight, forward, texTopRight));

            indices.Add( index );
            indices.Add( (Int16)(index + 1) );
            indices.Add( (Int16)(index + 2) );

            indices.Add( (Int16)(index + 1) );
            indices.Add( (Int16)(index + 3) );
            indices.Add( (Int16)(index + 2) );

            // Left face
            index = (Int16)verts.Count();

            verts.Add(new VertexPositionNormalTexture( backBottomLeft, left, texBottomLeft));
            verts.Add(new VertexPositionNormalTexture( backTopLeft, left, texTopLeft));
            verts.Add(new VertexPositionNormalTexture( frontBottomLeft, left, texBottomRight));
            verts.Add(new VertexPositionNormalTexture( frontTopLeft, left, texTopRight));

            indices.Add( index );
            indices.Add( (Int16)(index + 1) );
            indices.Add( (Int16)(index + 2) );

            indices.Add( (Int16)(index + 1) );
            indices.Add( (Int16)(index + 3) );
            indices.Add( (Int16)(index + 2) );

            // Right face

            index = (Int16)verts.Count();

            verts.Add(new VertexPositionNormalTexture( frontBottomRight, right, texBottomLeft));
            verts.Add(new VertexPositionNormalTexture( frontTopRight, right, texTopLeft));
            verts.Add(new VertexPositionNormalTexture( backBottomRight, right, texBottomRight));
            verts.Add(new VertexPositionNormalTexture( backTopRight, right, texTopRight));

            indices.Add( index );
            indices.Add( (Int16)(index + 1) );
            indices.Add( (Int16)(index + 2) );

            indices.Add( (Int16)(index + 1) );
            indices.Add( (Int16)(index + 3) );
            indices.Add( (Int16)(index + 2) );

            // Back face
            index = (Int16)verts.Count();

            verts.Add(new VertexPositionNormalTexture( backBottomRight, back, texBottomLeft));
            verts.Add(new VertexPositionNormalTexture( backTopRight, back, texTopLeft));
            verts.Add(new VertexPositionNormalTexture( backBottomLeft, back, texBottomRight));
            verts.Add(new VertexPositionNormalTexture( backTopLeft, back, texTopRight));

            indices.Add( index );
            indices.Add( (Int16)(index + 1) );
            indices.Add( (Int16)(index + 2) );

            indices.Add( (Int16)(index + 1) );
            indices.Add( (Int16)(index + 3) );
            indices.Add( (Int16)(index + 2) );

            // Top face

            index = (Int16)verts.Count();

            verts.Add(new VertexPositionNormalTexture( frontTopLeft, up, texBottomLeft));
            verts.Add(new VertexPositionNormalTexture( backTopLeft, up, texTopLeft));
            verts.Add(new VertexPositionNormalTexture( frontTopRight, up, texBottomRight));
            verts.Add(new VertexPositionNormalTexture( backTopRight, up, texTopRight));

            indices.Add( index );
            indices.Add( (Int16)(index + 1) );
            indices.Add( (Int16)(index + 2) );

            indices.Add( (Int16)(index + 1) );
            indices.Add( (Int16)(index + 3) );
            indices.Add( (Int16)(index + 2) );

            // Bottom face

            index = (Int16)verts.Count();

            verts.Add(new VertexPositionNormalTexture( backBottomLeft, down, texBottomLeft));
            verts.Add(new VertexPositionNormalTexture( frontBottomLeft, down, texTopLeft));
            verts.Add(new VertexPositionNormalTexture( backBottomRight, down, texBottomRight));
            verts.Add(new VertexPositionNormalTexture( frontBottomRight, down, texTopRight));

            indices.Add( index );
            indices.Add( (Int16)(index + 1) );
            indices.Add( (Int16)(index + 2) );

            indices.Add( (Int16)(index + 1) );
            indices.Add( (Int16)(index + 3) );
            indices.Add( (Int16)(index + 2) );

        }
    }
}