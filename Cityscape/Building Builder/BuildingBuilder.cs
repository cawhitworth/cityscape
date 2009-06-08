using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Cityscape
{
    public partial class BuildingBuilder
    {
        public static Random rand = new Random();
        public static readonly Vector3 storyDimensions = new Vector3(0.1f, 0.1f, 0.1f);
        public enum Stretch { None, Horizontal, Vertical };

    }
}