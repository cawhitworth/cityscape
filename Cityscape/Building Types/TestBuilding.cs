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
    class TestBuilding : BaseBuilding
    {
        public TestBuilding(Vector3 center, int stories, Vector2 baseDimensions)
            : base(center, stories, baseDimensions)
        {
            AddBox(new Vector3(baseDimensions.X, 0.1f, baseDimensions.Y));

            BuildingBuilder.AddPanelBox(ref vertices, ref indices,
                                        new Vector3(0.0f, 0.0f, 0.0f),
                                        BuildingBuilder.storyDimensions,
                                        new Vector3(10.0f, 10.0f, 10.0f),
                                        new Vector3(1.0f, 1.0f, 1.0f),
                                        BuildingBuilder.Stretch.None);
        }
    }
}