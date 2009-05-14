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
    public interface IFrameCounter
    {
        float OverallFPS { get; }
        float CurrentFPS { get; }
        UInt32 CurrentPolysPerFrame { get; }
        void AddRenderedPolys(UInt32 count);
    }

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class FrameCounter : Microsoft.Xna.Framework.DrawableGameComponent, IFrameCounter
    {
        UInt32 frames;
        UInt32 framesSinceLastReset;
        UInt32 totalTime;
        UInt32 timeSinceLastReset;
        float totalFPS;
        float currentFPS;
        UInt32 polysRendered;
        UInt32 polysPerFrame;

        public float OverallFPS
        {
            get { return totalFPS; }
        }

        public float CurrentFPS
        {
            get { return currentFPS; }
        }

        public UInt32 CurrentPolysPerFrame
        {
            get { return polysPerFrame;}
        }

        public void AddRenderedPolys(UInt32 count)
        {
            polysRendered += count;
        }

        public FrameCounter(Game game)
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

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            frames++;
            framesSinceLastReset++;
            timeSinceLastReset += (UInt32) gameTime.ElapsedRealTime.Milliseconds;
            totalTime += (UInt32) gameTime.ElapsedRealTime.Milliseconds;

            float totalTimeInSeconds = (float)totalTime / 1000.0f;
            totalFPS = (float)frames / totalTimeInSeconds;

            float timeSinceLastResetInSeconds = (float)timeSinceLastReset / 1000.0f;

            if (timeSinceLastReset > 1000)
            {
                polysPerFrame = polysRendered / framesSinceLastReset;
                currentFPS = (float)framesSinceLastReset / timeSinceLastResetInSeconds;
                timeSinceLastReset = 0;
                framesSinceLastReset = 0;
                polysRendered = 0;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws things
        /// </summary>
        /// <param name="gameTime">Snapshot of timing values</param>
        public override void Draw(GameTime gameTime)
        {


            base.Draw(gameTime);
        }
    }
}