using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cityscape
{
    class ClassicBuilding : BaseBuilding
    {
        public ClassicBuilding(Vector3 center, int stories, Vector2 baseDimensions,
            float windowWidth, float spacerWidth,
            int initHeight, float vTierScale, float hTierScale,
            float capHeight, float capOverhang) : base(center, stories, baseDimensions)
        {
            stretch = BuildingBuilder.Stretch.None;
            int totalHeight = 0;
            Vector3 dimensions = new Vector3(baseDimensions.X, capHeight, baseDimensions.Y);
            Vector3 capDimensions = dimensions;
            capDimensions.X *= capOverhang; capDimensions.Z *= capOverhang;

            Vector3 offset = new Vector3((baseDimensions.X - capDimensions.X) / 2.0f,
                                          0.0f,
                                         (baseDimensions.Y - capDimensions.Z) / 2.0f);

            AddBlackBox(offset * BuildingBuilder.storyDimensions, capDimensions);
            offset.Y += capHeight;

            while (totalHeight < stories && initHeight > 0)
            {
                totalHeight += initHeight;

                dimensions.Y = (float) initHeight;
                offset.X = (baseDimensions.X - dimensions.X) / 2.0f;
                offset.Z = (baseDimensions.Y - dimensions.Z) / 2.0f;
                AddColumnedBox(offset * BuildingBuilder.storyDimensions, dimensions, (int)windowWidth, (int)spacerWidth);

                offset.Y += initHeight;
                initHeight = (int)((float)initHeight * vTierScale);
                capDimensions = dimensions;
                capDimensions.X *= capOverhang; capDimensions.Y = capHeight; capDimensions.Z *= capOverhang;

                offset.X = (baseDimensions.X - capDimensions.X) / 2.0f;
                offset.Z = (baseDimensions.Y - capDimensions.Z) / 2.0f;
                AddBlackBox(offset * BuildingBuilder.storyDimensions, capDimensions);

                offset.Y += capHeight;
                dimensions *= hTierScale;
                dimensions.X = (float)Math.Floor(dimensions.X); dimensions.Z = (float)Math.Floor(dimensions.Z);

                if (offset.Y > lightMin)
                {
                    float lightX = 0.05f + (capDimensions.X / 2.0f);
                    float lightY = 0.05f + (capDimensions.Z / 2.0f);

                    AddLight(new Vector3(lightX, offset.Y, lightY) * BuildingBuilder.storyDimensions, lightColor);
                    AddLight(new Vector3(lightX, offset.Y, -lightY) * BuildingBuilder.storyDimensions, lightColor);
                    AddLight(new Vector3(-lightX, offset.Y, lightY) * BuildingBuilder.storyDimensions, lightColor);
                    AddLight(new Vector3(-lightX, offset.Y, -lightY) * BuildingBuilder.storyDimensions, lightColor);
                }
            }

            
            Vector3 centreTop = new Vector3(baseDimensions.X / 2.0f, offset.Y, baseDimensions.Y / 2.0f);
            centreTop *= BuildingBuilder.storyDimensions;
            AddRoofDecoration(centreTop, new Vector2(dimensions.X, dimensions.Z));
        }
    }
}