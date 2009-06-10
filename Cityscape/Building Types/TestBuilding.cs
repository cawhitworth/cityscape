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

            AddColumnedBox(new Vector3(17.0f, 20.0f, baseDimensions.Y), 5, 1);

        }
    }
}