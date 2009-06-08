using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Cityscape
{
    public class ManchesterHilton : BaseBuilding
    {
        public ManchesterHilton(Vector3 center, int stories, Vector2 baseDimensions) : base(center, stories, baseDimensions)
        {
            // Base
            AddSimpleBox(new Vector3(baseDimensions.X, 0.1f, baseDimensions.Y));
            int baseWidth = (int)baseDimensions.X, baseHeight = (int) baseDimensions.Y;

            int longSide = (baseWidth / 2) + BuildingBuilder.rand.Next(baseWidth / 4);
            int shortSide = (baseWidth / 8) + BuildingBuilder.rand.Next(baseWidth / 8);

            int xSize, ySize, orientation = BuildingBuilder.rand.Next(2);
            if (orientation == 0)
            {
                xSize = longSide; ySize = shortSide;
            }
            else
            {
                ySize = longSide; xSize = shortSide;
            }

            int xPos = BuildingBuilder.rand.Next(baseWidth - (xSize + 2)) + 1;
            int yPos = BuildingBuilder.rand.Next(baseHeight - (ySize + 2)) + 1;

            float xOffset = (float) xPos * BuildingBuilder.storyDimensions.X;
            float yOffset = (float) yPos * BuildingBuilder.storyDimensions.Z;

            AddSimpleBox( new Vector3(xOffset, 0.0f, yOffset), new Vector3((float)xSize, (float) stories / 2, (float) ySize) );

            if (orientation == 0)
            {
                ySize += 2;
            }
            else
            {
                xSize += 2;
            }

            AddSimpleBox( new Vector3(xOffset, (float)stories * 0.5f * BuildingBuilder.storyDimensions.Y, yOffset), 
                new Vector3((float)xSize, (float) stories / 2, (float) ySize) );
        }
    }
}