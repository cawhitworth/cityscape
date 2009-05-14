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
                1.0f, 1000.0f);

            angle = 0.0f;
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            angle += (float)gameTime.ElapsedRealTime.Milliseconds / 4000.0f;
            cameraPos.X = (float)Math.Sin((double)angle) * 20.0f;
            cameraPos.Z = (float)Math.Cos((double)angle) * 20.0f;
            cameraPos.Y = 5.0f;

            lookAt = new Vector3(0.0f, 0.0f, 0.0f);
            right = Vector3.Cross(Vector3.UnitY, lookAt - cameraPos);
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