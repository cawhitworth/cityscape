using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Cityscape
{
    class ClassicBuilding : BaseBuilding
    {
        public ClassicBuilding(Vector3 center, int stories, Vector2 baseDimensions,
            float windowWidth, float spacerWidth,
            int initHeight, float tierScale,
            float capHeight, float capOverhang) : base(center, stories, baseDimensions)
        {
            int totalHeight = initHeight;
            while (totalHeight < stories)
            {

                float xPanels = 1.0f + (float)Math.Floor((baseDimensions.X - windowWidth) / (windowWidth + spacerWidth));
                float zPanels = 1.0f + (float)Math.Floor((baseDimensions.Y - windowWidth) / (windowWidth + spacerWidth));
                Vector3 dimensions = new Vector3(xPanels * windowWidth + (xPanels - 1) * spacerWidth,
                                                 capHeight,
                                                 zPanels * windowWidth + (zPanels - 1) * spacerWidth);
                Vector3 offset = new Vector3((baseDimensions.X - dimensions.X) / 2.0f,
                                              0.0f,
                                             (baseDimensions.Y - dimensions.Y) / 2.0f);

                AddBlackBox(offset * BuildingBuilder.storyDimensions, dimensions);
                offset.Y += capHeight;
                AddColumnedBox(offset * BuildingBuilder.storyDimensions, initHeight, windowWidth, spacerWidth, (int)xPanels, (int)zPanels);
            }
        }
    }
}