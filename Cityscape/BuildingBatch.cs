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
    public class BuildingBatch : Microsoft.Xna.Framework.DrawableGameComponent
    {
        // Members
        Effect effect;
        Matrix model;
        VertexDeclaration vertDecl;
        static Texture2D bldTex;

        VertexPositionNormalTexture[] vertices;
        int[] indices;
        List<Building> buildings = new List<Building>();

        // Services needed
        IGraphicsDeviceService graphicsDeviceService;
        IFrameCounter frameCounter;
        ICamera camera;

        public BuildingBatch(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
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
            frameCounter = (IFrameCounter) Game.Services.GetService(typeof(IFrameCounter));

            if (bldTex == null)
                bldTex = BuildingTextureGenerator.MakeTexture(graphicsDeviceService.GraphicsDevice);
            effect = Game.Content.Load<Effect>("BuildingEffect");
            vertDecl = new VertexDeclaration(graphicsDeviceService.GraphicsDevice, VertexPositionNormalTexture.VertexElements);
            effect.CurrentTechnique = effect.Techniques["DefaultTechnique"];
            effect.Parameters["texBld"].SetValue(bldTex);
            effect.Parameters["Diffuse"].SetValue(new Vector4(0.7f, 0.7f, 0.7f, 0.0f));
            effect.Parameters["Ambient"].SetValue(new Vector4(0.3f, 0.3f, 0.3f, 0.0f));
            effect.Parameters["Light0Position"].SetValue(new Vector3(0.0f, 1.0f, 0.0f));

            model = Matrix.Identity;

            base.Initialize();
        }

        public void AddBuilding(Building b)
        {
            buildings.Add(b);
        }

        public void UpdateGeometry()
        {
            int vertCount = 0;
            int indexCount = 0;

            foreach( Building b in buildings)
            {
                vertCount += b.Vertices.Count();
                indexCount += b.Indices.Count();
            }

            vertices = new VertexPositionNormalTexture[vertCount];
            indices = new int[indexCount];

            int vertex = 0, index = 0, baseIndex ;

            foreach (Building b in buildings)
            {
                baseIndex = vertex;
                foreach(VertexPositionNormalTexture v in b.Vertices)
                    vertices[vertex++] = v;
                foreach (int i in b.Indices)
                    indices[index++] = i + baseIndex;
            }

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

        /// <summary>
        /// Draw my pretty buildings
        /// </summary>
        /// <param name="gameTime">Snapshot of timing values</param>
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

                frameCounter.AddRenderedPolys((UInt32)indices.Count() / 3);

                pass.End();
            }
            effect.End();


            base.Draw(gameTime);
        }
    }
}