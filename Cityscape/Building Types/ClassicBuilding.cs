using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Cityscape
{
    class ClassicBuilding : BaseBuilding
    {
        public ClassicBuilding(Vector3 center, int stories, Vector2 baseDimensions,
            float windowWidth, float spacerWidth,
            int initHeight, float vTierScale, float hTierScale,
            float capHeight, float capOverhang) : base(center, stories, baseDimensions)
        {
            int totalHeight = 0;
            Vector3 dimensions = new Vector3(baseDimensions.X, capHeight, baseDimensions.Y);
            Vector3 offset = new Vector3((baseDimensions.X - dimensions.X) / 2.0f,
                                          0.0f,
                                         (baseDimensions.Y - dimensions.Z) / 2.0f);
            while (totalHeight < stories)
            {
                totalHeight += initHeight;
                dimensions.Y = capHeight;
                AddBlackBox(offset * BuildingBuilder.storyDimensions, dimensions);
                offset.Y += capHeight;
                dimensions.Y = (float) initHeight;
                AddColumnedBox(offset * BuildingBuilder.storyDimensions, dimensions, 3, 1);
                offset.Y += initHeight;
                dimensions *= hTierScale; 
                dimensions.X = (float)Math.Floor(dimensions.X); dimensions.Z = (float)Math.Floor(dimensions.Z);
                offset.X = (baseDimensions.X - dimensions.X) / 2.0f;
                offset.Z = (baseDimensions.Y - dimensions.Z) / 2.0f;
                initHeight = (int)((float)initHeight * vTierScale);
            }
        }
    }
}