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

            BuildingBuilder.AddPanelledBox(ref vertices, ref indices, new Vector3(0.0f, 0.0f, 0.0f),
                10.0f, 3.0f, 1.0f, 5, 5, new Vector3(1.0f, 1.0f, 1.0f), BuildingBuilder.Stretch.None);
            BuildingBuilder.AddSimpleBox(ref vertices, ref indices, new Vector3(0.0f, 1.0f, 0.0f),
                new Vector3(19.0f, 2.0f, 19.0f), new Vector3(0.0f), BuildingBuilder.Stretch.None);
            BuildingBuilder.AddPanelledBox(ref vertices, ref indices, new Vector3(0.0f, 1.2f, 0.0f),
                10.0f, 3.0f, 1.0f, 5, 5, new Vector3(1.0f, 1.0f, 1.0f), BuildingBuilder.Stretch.None);
        }
    }
}