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
    public class SimpleCylinderBuilding : BaseBuilding
    {
        public SimpleCylinderBuilding(Vector3 center, int stories, Vector2 baseDimensions)
            : base(center, stories, baseDimensions)
        {
            AddBox(new Vector3(baseDimensions.X, 0.1f, baseDimensions.Y));
            AddCylinder(stories, (float)BuildingBuilder.rand.Next((int)baseDimensions.X), 16);
        }
    }
}