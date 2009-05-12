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
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        List<Building> buildings = new List<Building>();

        public Matrix view;
        public Matrix projection;
        public Vector3 cameraPos;
        Vector3 lookAt;
        float angle;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Building bldg = new Building(this);
            buildings.Add(bldg);
            Components.Add(bldg);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45),
                (float)graphics.GraphicsDevice.Viewport.Width / (float)graphics.GraphicsDevice.Viewport.Height,
                1.0f, 1000.0f);

            angle = 0;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.CornflowerBlue, 1.0f, 0);

            graphics.GraphicsDevice.RenderState.CullMode = CullMode.None;
            graphics.GraphicsDevice.RenderState.DepthBufferEnable = true;
            graphics.GraphicsDevice.RenderState.AlphaBlendEnable = false;
            graphics.GraphicsDevice.RenderState.AlphaTestEnable = false;
            graphics.GraphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
            graphics.GraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Wrap;
            graphics.GraphicsDevice.SamplerStates[0].MagFilter = TextureFilter.GaussianQuad;
            graphics.GraphicsDevice.SamplerStates[0].MinFilter = TextureFilter.GaussianQuad;
            graphics.GraphicsDevice.SamplerStates[0].MipFilter = TextureFilter.GaussianQuad;
            // TODO: Add your drawing code here

            angle += (float)gameTime.ElapsedRealTime.Milliseconds / 1000.0f;

            cameraPos.X = (float)Math.Sin((double)angle) * 4.0f;
            cameraPos.Z = (float)Math.Cos((double)angle) * 4.0f;
            cameraPos.Y = 1.0f;

            lookAt = new Vector3(0.0f, 0.0f, 0.0f);
            Vector3 right = Vector3.Cross(Vector3.UnitY, lookAt - cameraPos);
            Vector3 Up = -Vector3.Cross(right, lookAt - cameraPos);
            Up.Normalize();
            view = Matrix.CreateLookAt(cameraPos, lookAt, Up);
            base.Draw(gameTime);
        }
    }
}
