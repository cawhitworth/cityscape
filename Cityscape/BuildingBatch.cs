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
        int count = 0;

        List<VertexPositionNormalTextureMod[]> vertices = new List<VertexPositionNormalTextureMod[]>();
        List<short[]> indices = new List<short[]>();
        List<IBuilding> buildings = new List<IBuilding>();

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
            {
                BuildingTextureGenerator.deviceService = graphicsDeviceService;
                bldTex = BuildingTextureGenerator.texture;
            }

            effect = Game.Content.Load<Effect>("BuildingEffect");
            vertDecl = new VertexDeclaration(graphicsDeviceService.GraphicsDevice, VertexPositionNormalTextureMod.VertexElements);

            model = Matrix.Identity;

            base.Initialize();
        }

        public void AddBuilding(IBuilding b)
        {
            buildings.Add(b);
        }

        public void UpdateGeometry(int batchSize)
        {
            bool newBatch = true;
            List<VertexPositionNormalTextureMod> vertBatch = null;
            VertexPositionNormalTextureMod[] vertArray = null;
            List<int> indexBatch = null;
            short[] indexArray = null;

            foreach(IBuilding b in buildings)
            {
                if (newBatch)
                {
                    if (vertBatch != null)
                    {
                        int index = 0;
                        vertArray = new VertexPositionNormalTextureMod[vertBatch.Count()];
                        foreach(VertexPositionNormalTextureMod v in vertBatch)
                            vertArray[index++] = v;
                        vertices.Add(vertArray);
                        index = 0;
                        indexArray = new short[indexBatch.Count()];
                        foreach(int i in indexBatch)
                            indexArray[index++] = (short)i;
                        indices.Add(indexArray);
                    }
                    vertBatch = new List<VertexPositionNormalTextureMod>();
                    indexBatch = new List<int>();
                }

                int baseIndex = vertBatch.Count();
                vertBatch.AddRange(b.Vertices);
                foreach (int i in b.Indices)
                    indexBatch.Add( i + baseIndex );

                newBatch = vertBatch.Count() > batchSize;
            }
            if (vertBatch != null)
            {
                int index = 0;
                vertArray = new VertexPositionNormalTextureMod[vertBatch.Count()];
                foreach (VertexPositionNormalTextureMod v in vertBatch)
                    vertArray[index++] = v;
                vertices.Add(vertArray);
                index = 0;
                indexArray = new short[indexBatch.Count()];
                foreach (int i in indexBatch)
                    indexArray[index++] = (short)i;
                indices.Add(indexArray);
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
            effect.Parameters["texBld"].SetValue(bldTex);
            effect.Parameters["Diffuse"].SetValue(new Vector4(0.9f, 0.9f, 0.9f, 0.0f));
            effect.Parameters["Ambient"].SetValue(new Vector4(0.1f, 0.1f, 0.1f, 0.0f));
            effect.Parameters["LightDistance"].SetValue(0.1f);

            graphicsDeviceService.GraphicsDevice.VertexDeclaration = vertDecl;

            effect.Begin();
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Begin();

                int index = 0;
                for (index = 0; index < vertices.Count(); index++)
                {
                    graphicsDeviceService.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTextureMod>(
                      PrimitiveType.TriangleList,
                      vertices[index],
                      0,
                      vertices[index].Count(),
                      indices[index],
                      0,
                      indices[index].Count() / 3);
                    frameCounter.AddRenderedPolys((UInt32)indices[index].Count() / 3);
                }

                pass.End();
            }
            effect.End();


            base.Draw(gameTime);
        }
    }
}