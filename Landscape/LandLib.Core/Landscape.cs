using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroGH.LandLib.Core {

    public class Landscape{

        //fields
        public double width, height, depth, waterLevel, sheepPerSQM;
        public Mesh treegeometry = null;
        public Mesh sheepgeometry = null;
        public Random rnd = null;
        public Surface land = null;
        public List<Mesh> trees = new List<Mesh>();
        public List<Curve> contour = new List<Curve>();
        public Mesh water = null;
        public List<Sheep> sheeps = new List<Sheep>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width">Width of the landscape</param>
        /// <param name="depth">Depth of the landscape</param>
        /// <param name="height">Height of the landscape</param>
        /// <param name="tree">Tree geometry</param>
        /// <param name="sheepGeometry">Sheep Geometry</param>
        /// <param name="sheepSound">Sheep Sound</param>
        /// <param name="sheepPerSQM">Sheep per sq meter</param>
        /// <param name="seed">Randomness seed</param>
        public Landscape(double width, double depth, double height, Mesh tree, Mesh sheepGeometry, double sheepPerSQM, int seed) {
            // set the fields
            this.width = width;
            this.depth = depth;
            this.height = height;
            this.treegeometry = tree;
            this.rnd = new Random(seed);
            this.waterLevel = rnd.NextDouble() * 0.6;
            this.sheepgeometry = sheepGeometry;
            this.sheepPerSQM = sheepPerSQM;
                        }

        //method to generate the landscape
        public void Generate() {
            //reset fields
            land = null;
            trees.Clear();
            contour.Clear();
            sheeps.Clear();
            water = null;

            //generate the land surface using the GenerateLand method
            //generate contours
            //get the land and populate with trees using the GenerateTrees method
            GenerateLand();
            GenerateContours();
            GenerateTrees();
            GenerateWater();
            GenerateSheeps();
            }

        private void GenerateSheeps() {
            double sqm = this.width * this.depth;
            int sheepCount = (int)(sqm * this.sheepPerSQM);

            for (int i = 0; i < sheepCount; i++) {
                double u = rnd.NextDouble();
                double v = rnd.NextDouble();

                Point3d origin = this.land.PointAt(u, v);
                Vector3d normal = this.land.NormalAt(u, v);

                Plane start = new Plane(origin, normal);
                start.Rotate(rnd.NextDouble() * System.Math.PI * 2, start.ZAxis);

                sheeps.Add(new Sheep(start, this.sheepgeometry, rnd.Next()));
                }
            }

        public void WalkSheeps() {
            foreach (Sheep sheep in sheeps) {
                sheep.Update(this);
                }
            }

        public Mesh GetSheeps() {
            Mesh allSheeps = new Mesh();
            foreach (Sheep sheep in sheeps) {
                allSheeps.Append(sheep.GetGeometry());
                }
            return allSheeps;
            }

        private void GenerateWater() {
            this.water = Mesh.CreateFromPlane(Plane.WorldXY, new Interval(0, this.width), new Interval(0, this.depth), 1, 1);
            this.water.Translate(0, 0, this.waterLevel * this.height);
            }

        //generate contours
        private void GenerateContours() {
            BoundingBox bb = this.land.GetBoundingBox(false);
            contour.AddRange(Brep.CreateContourCurves(this.land.ToBrep(), bb.GetCorners()[0], bb.GetCorners()[4], 0.5));
            }

        private void GenerateLand() {
            //for 3x
            //  create random points
            //  create curve from points
            //  move the curve in y direction
            //get 3 curves and create loft surface

            List<Curve> curves = new List<Curve>();

            for (int i = 0; i < 8; i++) {
                //create random points
                List<Point3d> pts = new List<Point3d>();
                double dx = (this.width / 7);

                for (int j = 0; j < 8; j++) {
                    pts.Add(new Point3d(dx * j, 0, rnd.NextDouble() * this.height));
                    }

                //create the curve
                Curve c = Curve.CreateControlPointCurve(pts, 3);

                //move the curve
                double dy = (this.depth / 7);
                c.Translate(0, dy * i, 0);
                c.Reverse();
                curves.Add(c);
                }

            //loft the curves
            Brep[] br = Brep.CreateFromLoft(curves, Point3d.Unset, Point3d.Unset, LoftType.Normal, false);
            BrepFace bf = br[0].Faces[0];
            this.land = bf.DuplicateSurface();

            //reparametrize the surface
            this.land.SetDomain(0, new Interval(0, 1));
            this.land.SetDomain(1, new Interval(0, 1));
            }

        private void GenerateTrees() {
            int TreesCount = rnd.Next(10, 40);

            for (int i = 0; i < TreesCount; i++) {
                Point3d location = ChooseLocation(this.waterLevel * this.height);
                if (location == Point3d.Unset) { continue; }
                Mesh tree = this.treegeometry.DuplicateMesh();
                tree.Translate(location.X, location.Y, location.Z);
                tree.Rotate(rnd.NextDouble() * Math.PI * 2, Vector3d.ZAxis, location);
                this.trees.Add(tree);
                }
            }

        private Point3d ChooseLocation(double MinimalHeight) {
            int trials = 100;

            for (int i = 0; i < trials; i++) {
                Point3d thispt = this.land.PointAt(rnd.NextDouble(), rnd.NextDouble());
                if (thispt.Z >= MinimalHeight) { return thispt; }
                }


            return Point3d.Unset;
            }

        }
    }

