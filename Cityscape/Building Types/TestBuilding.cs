using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Cityscape
{
    class TestBuilding : BaseBuilding
    {
        public TestBuilding(Vector3 center, int stories, Vector2 baseDimensions)
            : base(center, stories, baseDimensions)
        {
            AddSimpleBox(new Vector3(baseDimensions.X, 0.1f, baseDimensions.Y));

            BuildingBuilder.AddSimplePanelBox(ref vertices, ref indices,
                                        new Vector3(0.0f, 0.0f, 0.0f),
                                        new Vector3(10.0f, 10.0f, 10.0f),
                                        new Vector3(1.0f, 1.0f, 1.0f),
                                        BuildingBuilder.Stretch.None);
        }
    }
}