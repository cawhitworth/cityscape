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
    public class UglyModernBuilding : BaseBuilding
    {
        public UglyModernBuilding(Vector3 center, int stories, Vector2 baseDimensions)
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

            // Left lump

            int xPos2 = BuildingBuilder.rand.Next(xPos - 1);
            int yPos2 = yPos + 1 + BuildingBuilder.rand.Next(ySize / 2);
            int xSize2 = xPos - xPos2;
            int maxYSize = (ySize - 2) - (yPos2 - yPos) + 1;
            int ySize2 = maxYSize / 2 + BuildingBuilder.rand.Next(maxYSize / 2);

            xOffset = (float)xPos2 * BuildingBuilder.storyDimensions.X;
            yOffset = (float)yPos2 * BuildingBuilder.storyDimensions.Z;
            AddSimpleBox(new Vector3(xOffset, 0.0f, yOffset),
                   new Vector3((float)xSize2, (float)((stories / 4) * BuildingBuilder.rand.Next(4)), (float)ySize2));

            // Front lump

            xPos2 = xPos + 1 + BuildingBuilder.rand.Next(xSize / 2);
            yPos2 = BuildingBuilder.rand.Next(yPos - 1);
            ySize2 = yPos - yPos2;
            int maxXSize = (xSize - 2) - (xPos2 - xPos) + 1;
            xSize2 = maxXSize / 2 + BuildingBuilder.rand.Next(maxXSize / 2);

            xOffset = (float)xPos2 * BuildingBuilder.storyDimensions.X;
            yOffset = (float)yPos2 * BuildingBuilder.storyDimensions.Z;
            AddSimpleBox(new Vector3(xOffset, 0.0f, yOffset),
                   new Vector3((float)xSize2, (float)((stories / 4) * BuildingBuilder.rand.Next(4)), (float)ySize2));

            // Right lump

            xPos2 = xPos + xSize - 1;
            yPos2 = yPos + 1 + BuildingBuilder.rand.Next(ySize / 2);
            xSize2 = BuildingBuilder.rand.Next(baseWidth - (xPos2 + 1)) + 2;
            maxYSize = (ySize - 2) - (yPos2 - yPos) + 1;
            ySize2 = maxYSize / 2 + BuildingBuilder.rand.Next(maxYSize / 2);

            xOffset = (float)xPos2 * BuildingBuilder.storyDimensions.X;
            yOffset = (float)yPos2 * BuildingBuilder.storyDimensions.Z;
            AddSimpleBox(new Vector3(xOffset, 0.0f, yOffset),
                   new Vector3((float)xSize2, (float)((stories / 4) * BuildingBuilder.rand.Next(4)), (float)ySize2));

            // Back lump

            yPos2 = yPos + ySize - 1;
            xPos2 = xPos + 1 + BuildingBuilder.rand.Next(xSize / 2);
            ySize2 = BuildingBuilder.rand.Next(baseWidth - (yPos2 + 1)) + 2;
            maxXSize = (xSize - 2) - (xPos2 - xPos) + 1;
            xSize2 = maxXSize / 2 + BuildingBuilder.rand.Next(maxXSize / 2);

            xOffset = (float)xPos2 * BuildingBuilder.storyDimensions.X;
            yOffset = (float)yPos2 * BuildingBuilder.storyDimensions.Z;
            AddSimpleBox(new Vector3(xOffset, 0.0f, yOffset),
                   new Vector3((float)xSize2, (float)((stories / 4) * BuildingBuilder.rand.Next(4)), (float)ySize2));

        }
    }
}