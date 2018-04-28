using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandLib.Core {

    public class Sheep {

        //private fields
        private Plane position = Plane.Unset;
        private Mesh geometry = null;
        private Random rnd = null;
        public bool falling = false;
        private double scale = 1.0;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="geometry"></param>
        /// <param name="Seed"></param>
        public Sheep(Plane position, Mesh geometry, int Seed) {
            this.position = position;
            this.geometry = geometry.DuplicateMesh();
            this.geometry.Transform(Transform.PlaneToPlane(Plane.WorldXY, position));
            this.rnd = new Random(Seed);
            }

        /// <summary>
        /// Get the current Sheep geometry.
        /// </summary>
        /// <returns></returns>
        public Mesh GetGeometry() {
            Mesh dup = this.geometry.DuplicateMesh();
            dup.Transform(Transform.Scale(position.Origin, scale));
            return dup;
            }
        
        /// <summary>
        /// Updates the Sheep state.
        /// </summary>
        /// <param name="world"></param>
        public void Update(Landscape world) {
            UpdateFalling(world);

            if (falling) {
                Fall(world);
                }
            else {
                Walk(world);
                }
            }


        /// <summary>
        /// Updates if the Sheep is going to Fall.
        /// </summary>
        /// <param name="world"></param>
        private void UpdateFalling(Landscape world) {
            if (position.Origin.X < 0.5) { falling = true; }
            if (position.Origin.X > world.width - 0.5) { falling = true; }
            if (position.Origin.Y < 0.5) { falling = true; }
            if (position.Origin.Y > world.depth - 0.5) { falling = true; }
            }

        /// <summary>
        /// Actions the Sheep is performing while falling down.
        /// </summary>
        /// <param name="world"></param>
        private void Fall(Landscape world) {
            Plane oldPosition = position;
            double minz = -world.height * 10;
            position.Origin = new Point3d(position.Origin.X, position.Origin.Y, Math.Max(minz, position.Origin.Z - 1));

            //create transform
            Transform trans = Transform.PlaneToPlane(oldPosition, position);

            //transform geometry
            this.geometry.Transform(trans);

            //reduce scale
            scale = Math.Max(0.2, scale * 0.9);

            if (!(minz == position.Origin.Z)) {
                //make sound
                if (rnd.NextDouble() < 0.01) {
                    world.sheepPlayer.Play();
                    }
                }
            }

        private void Walk(Landscape world) {

            //store the old position
            Plane oldPosition = this.position;

            //move the sheep
            position.Rotate((rnd.NextDouble() - 0.5) * System.Math.PI * 0.2, position.ZAxis);
            position.Translate(position.XAxis * 0.2);
            double u, v;
            world.land.ClosestPoint(position.Origin, out u, out v);
            Point3d ptl = world.land.PointAt(u, v);
            Point3d ptm = world.water.ClosestPoint(position.Origin);

            if (ptl.Z > ptm.Z) {
                position.Origin = ptl;

                Vector3d normal = world.land.NormalAt(u, v);
                Vector3d zaxis = position.ZAxis;

                Plane anglePlane = new Plane(Point3d.Origin, Vector3d.CrossProduct(normal, zaxis));

                double angle = Vector3d.VectorAngle(zaxis, normal, anglePlane);
                position.Rotate(angle, anglePlane.ZAxis);
                }
            else {
                position.Origin = ptm;
                Vector3d normal = Vector3d.ZAxis;
                Vector3d zaxis = position.ZAxis;
                Vector3d xpr = Vector3d.CrossProduct(normal, zaxis);

                if (!(xpr.IsValid)) { xpr = Vector3d.YAxis; }
                Plane anglePlane = new Plane(Point3d.Origin, xpr);
                double angle = Vector3d.VectorAngle(zaxis, normal, anglePlane);

                if (double.IsNaN(angle)) { angle = 0; }
                if (!(anglePlane.IsValid)) { anglePlane = Plane.WorldZX; }
                position.Rotate(angle, anglePlane.ZAxis);
                }

            //create transform
            Transform trans = Transform.PlaneToPlane(oldPosition, position);

            //transform geometry
            this.geometry.Transform(trans);

            //reduce scale
            scale = Math.Max(1, scale * 0.9);

            //make sound
            if (rnd.NextDouble() < 0.001) {
                scale = 2;
                world.sheepPlayer.Play();
                }
            }
        }
    }
