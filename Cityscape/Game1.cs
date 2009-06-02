using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

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
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        KeyboardState ks;
        UInt32 frame;
        IFrameCounter frameCounterService;
        SpriteBatch spriteBatch;
        SpriteFont font;
        Vector2 textPos;
        BuildingBatch buildings;
        Color backgroundCol = new Color(0.0f, 0.0f, 0.1f);
        static Random rand = new Random();

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
            FrameCounter fc = new FrameCounter(this);
            Components.Add(fc);
            Services.AddService(typeof(IFrameCounter), fc);
            frameCounterService = (IFrameCounter) Services.GetService(typeof(IFrameCounter));

            buildings = new BuildingBatch(this);
            Components.Add(buildings);

            Stopwatch s;
            s = Stopwatch.StartNew();
            IBuilding bldg;
            int stories;
            for(int x=-20; x<21; x++)
                for (int y=-20; y <21; y++)
                {
/*                    switch (rand.Next(5))
                    {
                        case 0:
                            stories = 30 + rand.Next(30);
                            bldg = new UglyModernBuilding(new Vector3(x*2, 0.0f, y*2), stories, new Vector2(20.0f, 20.0f));
                            break;
                        case 1:
                            stories = 10 + rand.Next(15);
                            bldg = new UglyModernBuilding(new Vector3(x*2, 0.0f, y*2), stories, new Vector2(20.0f, 20.0f));
                            break;
                        case 2:
                        case 3:
                        case 4:
                            stories = 5 + rand.Next(10);
                            bldg = new SimpleBuilding(new Vector3(x*2, 0.0f, y*2), stories, new Vector2(20.0f, 20.0f));
                            break;
                        default:
                            bldg = null;
                            break;
                    }*/
                    bldg = new SimpleCylinderBuilding(new Vector3(x * 2, 0.0f, y * 2), 10 + rand.Next(15), new Vector2(20.0f, 20.0f));

                    buildings.AddBuilding(bldg);
                }


//            bldg = new SimpleBuilding(new Vector3(0.0f, 0.0f, 0.0f), 1, new Vector2(2000.0f, 2000.0f));
//            buildings.AddBuilding(bldg);

            s.Stop();
            System.Console.WriteLine(s.ElapsedMilliseconds);

            s.Reset(); s.Start();
            buildings.UpdateGeometry(5000);
            s.Stop();

            System.Console.WriteLine(s.ElapsedMilliseconds);

            Camera camera = new Camera(this);
            Components.Add(camera);
            Services.AddService(typeof(ICamera), camera);
            ks = Keyboard.GetState();
            frame = 0;
            IsFixedTimeStep = false; // If this is true, then Update() and Draw() can block on things like window movements
                                     // Besides, who uses fixed step updates anyway? What is this, 1993?
            textPos = new Vector2(graphics.GraphicsDevice.Viewport.Width / 8,
                                  graphics.GraphicsDevice.Viewport.Height / 8);

//            graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.DisplayMode.Height;
//            graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.DisplayMode.Width;

//            graphics.ToggleFullScreen();
            base.Initialize();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("DebugFont"); 
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
            KeyboardState newState = Keyboard.GetState();
            if (newState.IsKeyUp(Keys.Escape) && ks.IsKeyDown(Keys.Escape))
                this.Exit();

            ks = newState;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, backgroundCol, 1.0f, 0);

            graphics.GraphicsDevice.RenderState.CullMode = CullMode.None;
            graphics.GraphicsDevice.RenderState.DepthBufferEnable = true;
            graphics.GraphicsDevice.RenderState.AlphaBlendEnable = false;
            graphics.GraphicsDevice.RenderState.AlphaTestEnable = false;
            graphics.GraphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
            graphics.GraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Wrap;
            graphics.GraphicsDevice.SamplerStates[0].MagFilter = TextureFilter.GaussianQuad;
            graphics.GraphicsDevice.SamplerStates[0].MinFilter = TextureFilter.GaussianQuad;
            graphics.GraphicsDevice.SamplerStates[0].MipFilter = TextureFilter.GaussianQuad;

            base.Draw(gameTime);

            spriteBatch.Begin();
            string fps = "Framerate: Current " + (int)frameCounterService.CurrentFPS + "fps, overall " + (int)frameCounterService.OverallFPS + ", polys/frame " + frameCounterService.CurrentPolysPerFrame;
            spriteBatch.DrawString(font, fps, textPos, Color.Yellow);
            spriteBatch.End();
        }
    }
}
