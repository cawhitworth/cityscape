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
    public partial class BuildingBuilder
    {
        public static Random rand = new Random();
        public static readonly Vector3 storyDimensions = new Vector3(0.1f, 0.1f, 0.1f);
        public enum Stretch { None, Horizontal, Vertical };

    }
}