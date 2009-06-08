using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Cityscape
{
    public class SimpleCylinderBuilding : BaseBuilding
    {
        public SimpleCylinderBuilding(Vector3 center, int stories, Vector2 baseDimensions)
            : base(center, stories, baseDimensions)
        {
            AddSimpleBox(new Vector3(baseDimensions.X, 0.1f, baseDimensions.Y));
            AddCylinder(stories, (float)BuildingBuilder.rand.Next((int)baseDimensions.X), 16);
        }
    }
}