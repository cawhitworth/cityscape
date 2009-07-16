using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cityscape
{

    public class Road
    {
        public enum Orientation { Horizontal, Vertical };
        public enum Type { DualCarriageway, Major, Minor, Path };
        public int x, y;
        public Orientation orientation;
        public Type type;
        public int width, length;
    }

    public class Lot
    {
        private static Random rand = new Random();
        
        public Lot() { }

        public Lot(int x, int y, int w, int h)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
        }

        public int x { get; set; }
        public int y { get; set; }
        public int w { get; set; }
        public int h { get; set; }

        public Tuple<Lot, Road> Split(int splitInto, int roadWidth)
        {
            if (rand.NextDouble() < (double) w / (double)(w+h))
                return SplitH(splitInto, roadWidth);
            else
                return SplitV(splitInto, roadWidth);
        }

        public Tuple<Lot, Road> SplitH(int splitInto, int roadWidth)
        {
            if (w < roadWidth * 3) return new Tuple<Lot,Road>(new Lot(), new Road());

            Lot lot = new Lot();
            Road road = new Road();
            int splitPoint;
            if (w == roadWidth * 3)
                splitPoint = roadWidth;
            else
                splitPoint = rand.Next(w - (roadWidth * 3)) + roadWidth;
            lot.x = x + splitPoint + roadWidth;
            lot.y = y;
            lot.w = w - (splitPoint + roadWidth);
            lot.h = h;
            road.orientation = Road.Orientation.Vertical;
            road.x = x + splitPoint;
            road.y = y;
            road.width = roadWidth;
            road.length = h;
            w = splitPoint;
            return new Tuple<Lot,Road>(lot, road);
        }

        public Tuple<Lot, Road> SplitV(int splitInto, int roadWidth)
        {
            if (h < roadWidth * 3) return new Tuple<Lot,Road>(new Lot(), new Road());
            Lot lot = new Lot();
            Road road = new Road();
            int splitPoint;
            if (h == roadWidth * 3)
                splitPoint = roadWidth;
            else
                splitPoint = rand.Next(h - (roadWidth * 3)) + roadWidth;
            lot.y = y + splitPoint + roadWidth;
            lot.x = x;
            lot.h = h - (splitPoint + roadWidth);
            lot.w = w;
            road.orientation = Road.Orientation.Horizontal;
            road.y = y + splitPoint;
            road.x = x;
            road.width = roadWidth;
            road.length = w;
            h = splitPoint;
            return new Tuple<Lot, Road>(lot,road);
        }

    }


    public class CityPlanner
    {
        static int height = 1024;
        static int width = 1024;

        static Random rand = new Random();
        
        static Color lot = Color.Blue;
        static Dictionary<Road.Type, Color> roadColors = null;
        static int maxSize = 60;
        static int maxRatio = 5;
        static Texture2D city = null;

        public static Texture2D streetMap
        {
            get { return city; }
        }

        static Color Get(ref Color[] data, int x, int y)
        {
            return data[x + y * width];
        }

        static void Plot(ref Color[] data, int x, int y, Color pixel)
        {
            data[x + y * width] = pixel;
        }

        static void AddLot(ref Color[] data, Lot l)
        {
            AddRect(ref data, l.x, l.y, l.w, l.h, lot);
        }

        static void AddRect(ref Color[] data, int x, int y, int w, int h, Color color)
        {
            int px, py;
            for (px = x; px < x + w; px++)
                for (py = y; py < y + h; py++)
                    Plot(ref data, px, py, color);
        }

        static void BuildRoadColors()
        {
            roadColors = new Dictionary<Road.Type, Color>();
            roadColors[Road.Type.DualCarriageway] = Color.DarkRed;
            roadColors[Road.Type.Major] = Color.Yellow;
            roadColors[Road.Type.Minor] = Color.Gray;
            roadColors[Road.Type.Path] = Color.DarkBlue;
        }

        static void AddRoad(ref Color[] data, Road r)
        {
            if (roadColors == null)
                BuildRoadColors();
            if (r.orientation == Road.Orientation.Horizontal)
                AddRect(ref data, r.x, r.y, r.length, r.width, roadColors[r.type]);
            else
                AddRect(ref data, r.x, r.y, r.width, r.length, roadColors[r.type]);
        }

        public static void BuildCity(Game1 game)
        {
            IGraphicsDeviceService deviceService = (IGraphicsDeviceService) game.Services.GetService(typeof(IGraphicsDeviceService));

            if (city == null)
                city = new Texture2D(deviceService.GraphicsDevice, width, height);
            Color[] textureData = new Color[width * height];
            city.GetData<Color>(textureData);
            for(int i=0; i<width*height; i++) textureData[i] = Color.Black;

            List<Lot> lots = new List<Lot>();
            List<Road> roads = new List<Road>();

            lots.Add(new Lot(0, 0, width, height));

            int count = 0, notSplit = 0, splitFail = 0;
            int roadWidth = 16;
            Road.Type roadType = Road.Type.DualCarriageway;
            for(int splits = 0; splits < 50; splits++)
            {
                List<Lot> newLots = new List<Lot>();
                List<Road> newRoads = new List<Road>();

                switch(splits)
                {
                    case 5:
                        roadWidth = 8;
                        roadType = Road.Type.Major;
                        break;
                    case 10:
                        roadWidth = 4;
                        roadType = Road.Type.Minor;
                        break;
                    case 15:
                        roadWidth = 2;
                        break;
                    case 35:
                        roadType = Road.Type.Path;
                        break;
                }

                foreach (Lot l in lots)
                {
                    int midX = (l.x + l.w / 2);
                    int midY = (l.y + l.h / 2);
                    int d = (midX - width / 2) * (midX - width / 2) +
                            (midY - height / 2) * (midY - height / 2);
                    double distFactor = Math.Sqrt((double)d) / Math.Sqrt(width * width + height* height);
                    double areaFactor = (double)(l.w * l.h) / (double)(width * height);

                    if (rand.NextDouble() < distFactor + areaFactor * 2 || splits < 2)
                    {
                        count++;
                        Tuple<Lot, Road> t = l.Split(2, roadWidth);
                        t.Second.type = roadType;
                        if (t.First.w != 0)
                        {
                            newLots.Add(t.First);
                            newRoads.Add(t.Second);
                        }
                        else splitFail++;
                    }
                    else notSplit++;
                }
                lots.AddRange(newLots);
                roads.AddRange(newRoads);
            }

            bool done = false;
            do
            {
                done = true;
                List<Lot> newLots = new List<Lot>();
                List<Road> newRoads = new List<Road>();
                foreach (Lot l in lots)
                {
                    if (l.w > maxSize || (l.w / l.h) > maxRatio)
                    {
                        Tuple<Lot, Road> t = l.SplitH(2, 2);
                        t.Second.type = Road.Type.Path;
                        if (t.First.w != 0)
                        {
                            newLots.Add(t.First);
                            newRoads.Add(t.Second);
                        }
                        done = false;
                    }
                    if (l.h > maxSize || (l.h / l.w) > maxRatio)
                    {
                        Tuple<Lot, Road> t = l.SplitV(2, 2);
                        t.Second.type = Road.Type.Path;
                        if (t.First.w != 0)
                        {
                            newLots.Add(t.First);
                            newRoads.Add(t.Second);
                        }
                        done = false;
                    }
                }
                lots.AddRange(newLots);
                roads.AddRange(newRoads);
            } while (!done);

            
            foreach(Lot l in lots)
                AddLot(ref textureData, l);
            foreach (Road r in roads)
                AddRoad(ref textureData, r);

            city.SetData<Color>(textureData);
            city.Save("city.png", ImageFileFormat.Png);

            IBuildingBatch b = (IBuildingBatch)game.Services.GetService(typeof(IBuildingBatch));
            
            foreach(Lot l in lots)
            {
                Vector3 centre = new Vector3( (float)(l.x) + (float)(l.w) / 2.0f,
                                              0.0f,
                                              (float)(l.y) + (float)(l.h) / 2.0f);
                centre *= BuildingBuilder.storyDimensions;
                int stories = Math.Max(l.h, l.w);
                Vector2 baseDimensions = new Vector2((float)l.w, (float)l.h);
                IBuilding bldg = null;
                if (Math.Min(l.h, l.w) < 8)
                {
                    bldg = new SimpleBuilding(centre, stories, baseDimensions);
                }
                else
                {
                    switch(rand.Next(5))
                    {
                        case 0:
                        case 1: 
                            bldg = new ClassicBuilding(centre, stories, baseDimensions, 3, 1, stories / 2,
                                (float)(0.5 * rand.NextDouble() + 0.5), (float)(0.25 * rand.NextDouble() + 0.75),
                                2.0f, 1.0f + (float)(rand.NextDouble() * 0.2));
                            break;
                        case 2:
                            bldg = new UglyModernBuilding(centre, stories, baseDimensions);
                            break;
                        case 3:
                            if (Math.Min(l.h, l.w) < 16)
                            {
                                bldg = new UglyModernBuilding(centre, stories, baseDimensions);
                            }
                            else
                            {
                                bldg = new SimpleCylinderBuilding(centre, stories, baseDimensions);
                            }
                            break;
                        case 4:
                            bldg = new SimpleBuilding(centre, stories, baseDimensions);
                            break;
                    }
                }

                b.AddBuilding(bldg);
            }

        }
    }
}
