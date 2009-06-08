using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Cityscape
{
    public class SimpleBuilding : BaseBuilding
    {
        public SimpleBuilding(Vector3 center, int stories, Vector2 baseDimensions)
            : base(center, stories, baseDimensions)
        {
            // Base
            AddSimpleBox(new Vector3(baseDimensions.X, 0.1f, baseDimensions.Y));

            int baseWidth = (int)baseDimensions.X, baseHeight = (int)baseDimensions.Y;

            int xSize = (baseWidth / 2) + BuildingBuilder.rand.Next(baseWidth / 4);
            int ySize = (baseHeight / 2) + BuildingBuilder.rand.Next(baseHeight / 4);

            int xPos = BuildingBuilder.rand.Next(baseWidth - (xSize + 2)) + 1;
            int yPos = BuildingBuilder.rand.Next(baseHeight - (ySize + 2)) + 1;

            float xOffset = (float)xPos * BuildingBuilder.storyDimensions.X;
            float yOffset = (float)yPos * BuildingBuilder.storyDimensions.Z;

            // Main
            AddSimpleBox(new Vector3(xOffset, 0.0f, yOffset), new Vector3((float)xSize, (float)stories, (float)ySize));
        }
    }
}