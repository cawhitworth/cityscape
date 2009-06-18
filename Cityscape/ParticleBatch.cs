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
    public struct Particle
    {
        public Particle(Vector3 position, Color tint)
        {
            this.position = position; this.tint = tint;
        }

        public Vector3 position;
        public Color tint;
    }

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ParticleBatch : Microsoft.Xna.Framework.DrawableGameComponent
    {
        List<Particle> particles = new List<Particle>();
        List<VertexPositionColorTexture[]> vertices = new List<VertexPositionColorTexture[]>();

        Effect effect;
        Matrix model;
        VertexDeclaration vertDecl;
        static Texture2D lightTex;

        // Services needed
        IGraphicsDeviceService graphicsDeviceService;
        IFrameCounter frameCounter;
        ICamera camera;

        public ParticleBatch(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            model = Matrix.Identity;
        }

        public void AddParticle(Particle p)
        {
            particles.Add(p);
        }

        public void RebuildBuffers(int bufferSize)
        {
            int particle = 0, offset = 0;
            VertexPositionColorTexture[] vertArray = null;
            bufferSize -= bufferSize % 6;

            while (particle < particles.Count())
            {
                if (vertArray == null)
                {
                    int remaining = (particles.Count() - particle) * 6;
                    if (remaining > bufferSize)
                    {
                        remaining = bufferSize;
                    }
                    vertArray = new VertexPositionColorTexture[remaining];
                    offset = 0;
                }
                Particle p = particles[particle++];
                vertArray[offset++] = new VertexPositionColorTexture(p.position, p.tint, new Vector2(0.0f, 0.0f));
                vertArray[offset++] = new VertexPositionColorTexture(p.position, p.tint, new Vector2(1.0f, 0.0f));
                vertArray[offset++] = new VertexPositionColorTexture(p.position, p.tint, new Vector2(0.0f, 1.0f));

                vertArray[offset++] = new VertexPositionColorTexture(p.position, p.tint, new Vector2(1.0f, 0.0f));
                vertArray[offset++] = new VertexPositionColorTexture(p.position, p.tint, new Vector2(1.0f, 1.0f));
                vertArray[offset++] = new VertexPositionColorTexture(p.position, p.tint, new Vector2(0.0f, 1.0f));

                if (offset >= bufferSize)
                {
                    vertices.Add(vertArray);
                    vertArray = null;
                }
            }


        }

        #region Overrides from DrawableGameComponent
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
            
            if (lightTex == null)
            {
                LightTextureGenerator.deviceService = graphicsDeviceService;
                lightTex = LightTextureGenerator.texture;
            }

            effect = Game.Content.Load<Effect>("ParticleEffect");
            vertDecl = new VertexDeclaration(graphicsDeviceService.GraphicsDevice, VertexPositionColorTexture.VertexElements);

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

        /// <summary>
        /// Draw things
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            effect.Parameters["World"].SetValue(model);
            effect.Parameters["View"].SetValue(camera.View);
            effect.Parameters["Projection"].SetValue(camera.Projection);
            effect.Parameters["texParticle"].SetValue(lightTex);
            effect.Parameters["Size"].SetValue(1.0f);
            effect.Parameters["CamPos"].SetValue(camera.CameraPos);
            
            graphicsDeviceService.GraphicsDevice.VertexDeclaration = vertDecl;
            graphicsDeviceService.GraphicsDevice.RenderState.AlphaBlendEnable = true;
            graphicsDeviceService.GraphicsDevice.RenderState.AlphaBlendOperation = BlendFunction.Add;
            graphicsDeviceService.GraphicsDevice.RenderState.DepthBufferWriteEnable = false;

            effect.Begin();
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Begin();
                foreach (VertexPositionColorTexture[] v in vertices)
                {
                    graphicsDeviceService.GraphicsDevice.DrawUserPrimitives<VertexPositionColorTexture>(
                        PrimitiveType.TriangleList,
                        v, 0, v.Count() / 3);
                }
                pass.End();
            }
            effect.End();

            graphicsDeviceService.GraphicsDevice.RenderState.DepthBufferWriteEnable = true;
            base.Draw(gameTime);
        }
        #endregion

    }
}