using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KangarooSolver;
using Rhino.Geometry; 

namespace CustomGoals {
    public class FrictionFloor : GoalObject {

        Plane Floor = Plane.WorldXY;
        double FrictionFactor = 0.5; 

        public FrictionFloor(Plane FloorPlane, double Friction, List<Point3d> Particles) {
            base.PPos = Particles.ToArray();
            base.Move = new Vector3d[base.PPos.Length];
            base.Weighting = new double[base.PPos.Length]; 
            FrictionFactor = Friction;
            Floor = FloorPlane; 
            }

        public override void Calculate(List<KangarooSolver.Particle> p) {

            foreach (int index in base.PIndex) {
                Point3d location = p[index].Position;
                double distanceToPlane = Floor.DistanceTo(location);

                if (distanceToPlane <= Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance*10) {
                    //affect the vector 
                    Vector3d velocity = p[index].Velocity;
                    velocity *= FrictionFactor;
                    base.Move[index] = velocity;
                    p[index].Velocity = velocity;
                    }
                }
            }
        }
    }
