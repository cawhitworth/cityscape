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

    public interface IParticleService
    {
        void AddStaticParticle(Particle p);
        void AddStaticParticleRange(IEnumerable<Particle> p);
        void Reset();
        void RebuildStaticParticles(int bufferSize);
    }

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ParticleBatch : Microsoft.Xna.Framework.DrawableGameComponent, IParticleService
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
            Reset();
        }

        /// <summary>
        /// Add a static particle: These never go away, unless the entire object is re-initialised
        /// </summary>
        /// <param name="p">The particle to add</param>
        public void AddStaticParticle(Particle p)
        {
            particles.Add(p);
        }

        /// <summary>
        /// Add several static particles.
        /// </summary>
        /// <param name="p"></param>
        public void AddStaticParticleRange(IEnumerable<Particle> p)
        {
            particles.AddRange(p);
        }

        public void Reset()
        {
            particles.Clear();
            vertices.Clear();
        }

        /// <summary>
        /// Call this after adding new static particles, to re-initialise the vertex buffers
        /// </summary>
        /// <param name="bufferSize">The batch size in vertices - about 5,000 seems a good value</param>
        public void RebuildStaticParticles(int bufferSize)
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

            if (vertArray != null)
            {
                vertices.Add(vertArray);
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
            effect.Parameters["Size"].SetValue(0.1f);
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
                    frameCounter.AddRenderedPolys((uint)v.Count() / 3);
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