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

    public interface ICamera
    {
        Matrix Projection { get; }
        Matrix View { get; }
        Vector3 CameraPos { get; }
    }

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Camera : Microsoft.Xna.Framework.GameComponent, ICamera
    {
        Matrix projection;
        Matrix view;
        float angle;
        Vector3 cameraPos, right, up, lookAt;

        Vector3 position = new Vector3(0.0f, 10.0f, 1.0f);
        float hAngle = 0.0f, vAngle = -0.25f;

        public Camera(Game game)
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
            IGraphicsDeviceService graphicsDeviceService = (IGraphicsDeviceService)Game.Services.GetService(typeof(IGraphicsDeviceService));
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45),
                (float)graphicsDeviceService.GraphicsDevice.Viewport.Width / (float)graphicsDeviceService.GraphicsDevice.Viewport.Height,
                0.01f, 1000.0f);

            angle = 0.0f;

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            /*
            angle += (float)gameTime.ElapsedRealTime.Milliseconds / 4000.0f;
            cameraPos.X = (float)Math.Sin((double)angle) * 50.0f;
            cameraPos.Z = (float)Math.Cos((double)angle) * 50.0f;
            cameraPos.Y = 15.0f;

            lookAt = new Vector3(0.0f, 0.0f, 0.0f);*/
            
            KeyboardState ks = Keyboard.GetState();

            float mult = 1.0f;
            if (ks.IsKeyDown(Keys.LeftShift))
                mult *= 2.0f;
            if (ks.IsKeyDown(Keys.LeftControl))
                mult *= 2.0f;
            if (ks.IsKeyDown(Keys.Left))
                hAngle += mult * (float)gameTime.ElapsedRealTime.Milliseconds / 1000.0f;
            if (ks.IsKeyDown(Keys.Right))
                hAngle -= mult * (float)gameTime.ElapsedRealTime.Milliseconds / 1000.0f;

            if (ks.IsKeyDown(Keys.Up))
                vAngle += mult * (float)gameTime.ElapsedRealTime.Milliseconds / 1000.0f;
            if (ks.IsKeyDown(Keys.Down))
                vAngle -= mult * (float)gameTime.ElapsedRealTime.Milliseconds / 1000.0f;


            cameraPos = position;
            Vector3 lookDir = new Vector3(0.0f, 0.0f, -1.0f);

            lookDir = Vector3.Transform(lookDir, Matrix.CreateRotationX(vAngle));
            lookDir = Vector3.Transform(lookDir, Matrix.CreateRotationY(hAngle));
            right = Vector3.Cross(Vector3.UnitY, lookAt - cameraPos);
            lookAt = position + lookDir;

            if (ks.IsKeyDown(Keys.A))
                position += right * mult * ((float)gameTime.ElapsedRealTime.Milliseconds / 1000.0f);
            if (ks.IsKeyDown(Keys.D))
                position -= right * mult * ((float)gameTime.ElapsedRealTime.Milliseconds / 1000.0f);

            if (ks.IsKeyDown(Keys.W))
                position += lookDir* mult * ((float)gameTime.ElapsedRealTime.Milliseconds / 1000.0f);
            if (ks.IsKeyDown(Keys.S))
                position -= lookDir* mult * ((float)gameTime.ElapsedRealTime.Milliseconds / 1000.0f);


            up = -Vector3.Cross(right, lookAt - cameraPos);
            up.Normalize();
            view = Matrix.CreateLookAt(cameraPos, lookAt, up);

            base.Update(gameTime);
        }

        /// <summary>
        /// Implementation from ICamera - returns the view matrix
        /// </summary>
        public Matrix View 
        {
            get { return view; }
        }

        /// <summary>
        /// Implementation from ICamera - returns the projection matrix
        /// </summary>
        public Matrix Projection
        {
            get { return projection; }
        }

        /// <summary>
        /// Implementation from ICamera - returns the camera position
        /// </summary>
        public Vector3 CameraPos
        {
            get { return cameraPos; }
        }
    }
}