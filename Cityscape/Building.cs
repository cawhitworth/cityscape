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
    public interface IBuilding
    {
        IList<VertexPositionNormalTextureMod> Vertices
        {
            get;
        }
        IList<int> Indices
        {
            get;
        }
    }


    public class BaseBuilding : IBuilding
    {
        protected List<VertexPositionNormalTextureMod> vertices = new List<VertexPositionNormalTextureMod>();
        protected List<int> indices = new List<int>();
        protected Vector3 origin;
        protected Vector3 center;
        protected Vector3 colorMod;
        protected BuildingBuilder.Stretch stretch = BuildingBuilder.Stretch.None;

        public BaseBuilding(Vector3 center, int stories, Vector2 baseDimensions)
        {
            this.center = center;
            origin = center - new Vector3(baseDimensions.X * (BuildingBuilder.storyDimensions.X / 2.0f), 0.0f, baseDimensions.Y * (BuildingBuilder.storyDimensions.Z / 2.0f)); ;

            float tint = 1.0f - (0.2f * (float)BuildingBuilder.rand.NextDouble());
            switch (BuildingBuilder.rand.Next(3))
            {

                case 1: // Yellow tint
                    colorMod = new Vector3(1.0f, 1.0f, tint);
                    break;

                case 2: // Blue tint
                    colorMod = new Vector3(tint, tint, 1.0f);
                    break;

                case 0: // White
                default:
                    colorMod = new Vector3(1.0f, 1.0f, 1.0f);
                    break;
            }

            switch(BuildingBuilder.rand.Next(3))
            {
                case 0: stretch = BuildingBuilder.Stretch.None; break;
                case 1: stretch = BuildingBuilder.Stretch.Horizontal; break;
                case 2: if (stories % 2 == 0) { stretch = BuildingBuilder.Stretch.Vertical; } break;
            }
        }

        protected void AddSimpleBox(Vector3 storyDimensions)
        {
            BuildingBuilder.AddSimpleBox(
                ref vertices, ref indices,
                origin, storyDimensions,
                colorMod, stretch);
        }
 
        protected void AddSimpleBox(Vector3 offset, Vector3 storyDimensions)
        {
            BuildingBuilder.AddSimpleBox(
                ref vertices, ref indices,
                origin + offset, storyDimensions,
                colorMod, stretch);
        }

        protected void AddBlackBox(Vector3 offset, Vector3 storyDimensions)
        {
            BuildingBuilder.AddSimpleBox(
                ref vertices, ref indices,
                origin + offset, storyDimensions,
                new Vector3(0.0f, 0.0f, 0.0f),
                stretch);
        }

        protected void AddColumnedBox(Vector3 storyDimensions, int windowWidth, int spacerWidth)
        {
            BuildingBuilder.AddColumnedBox(ref vertices, ref indices,
                origin, storyDimensions,
                windowWidth, spacerWidth,
                colorMod, stretch);
        }

        protected void AddColumnedBox(Vector3 offset, Vector3 storyDimensions, int windowWidth, int spacerWidth)
        {
            BuildingBuilder.AddColumnedBox(ref vertices, ref indices,
                origin + offset, storyDimensions,
                windowWidth, spacerWidth,
                colorMod, stretch);
        }

        protected void AddColumnedBox(Vector3 offset, float height, float windowWidth, float spacerWidth,
                                      int nPanelsWide, int nPanelsDeep)
        {
            BuildingBuilder.AddColumnedBoxSizedByColumns(ref vertices, ref indices,
                origin+offset, height, windowWidth, spacerWidth, nPanelsWide, nPanelsDeep,
                colorMod, stretch);
        }

        protected void AddCylinder(int stories, float diameter, int segments)
        {
            BuildingBuilder.AddCylinder(
                ref vertices, ref indices,
                center, BuildingBuilder.storyDimensions,
                (float) stories, diameter,
                colorMod, stretch, segments);
        }

        public IList<VertexPositionNormalTextureMod> Vertices
        {
            get { return vertices.AsReadOnly(); }
        }

        public IList<int> Indices
        {
            get { return indices.AsReadOnly(); }
        }
    }

}