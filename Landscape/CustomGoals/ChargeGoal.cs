using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KangarooSolver;
using Rhino.Geometry;

namespace CustomGoals
{
    public class ChargeGoal : GoalObject {

        public double[] targets = null;
        public int division = -1; 
        public double maxSpeed = 0.001; 

        //constructor
        public ChargeGoal(List<Point3d> chargePoints, List<Point3d> cloud, List<double> cloudTargets, double TerminalVelocity) {
            //set the max velocity 
            maxSpeed = TerminalVelocity; 

            //indicates the start index of the cloud points in the particle position array 
            division = chargePoints.Count; 

            //combine charges with cloud
            List<Point3d> allPoints = new List<Point3d>(chargePoints);
            allPoints.AddRange(cloud);

            //store all points as particles, add movements, weights and target charges for each point in the cloud
            base.PPos = allPoints.ToArray();
            base.Move = new Vector3d[allPoints.Count];
            base.Weighting = new double[allPoints.Count];
            this.targets = cloudTargets.ToArray(); 
            }
                
        public override void Calculate(List<KangarooSolver.Particle> p) {
            //get the right particle indices 
            List<int> chargePointIdx = new List<int>();
            List<int> cloudPointIdx = new List<int>();

            for (int i = 0; i < base.PPos.Length; i++) {
                if (i < division) { chargePointIdx.Add(i); }
                else { cloudPointIdx.Add(i); }
                }

            //for each point in the cloud, compute the charge and vector
           foreach (int index in cloudPointIdx) {
                //current location of the point from the point cloud
                Point3d location = p[base.PIndex[index]].Position; //current position from the solver (p list) 
                Vector3d vectorSum = Vector3d.Zero; 
                double chargeSum = 0;
                double chargeTarget = targets[index-division];

                //compute the charge by iterating each charge point 
                foreach (int chargeIdx in chargePointIdx) {
                    Point3d chargeLocation = p[base.PIndex[chargeIdx]].Position; //current position from the solver (p list) 
                    double distance = chargeLocation.DistanceTo(location);
                    chargeSum += 1 / (distance  );  //add the charge to the charge sum
                    vectorSum += (location - chargeLocation); 
                    }
 
                vectorSum.Unitize();
                vectorSum *= -(chargeTarget - chargeSum);

                if (vectorSum.Length > maxSpeed) {
                    vectorSum.Unitize(); 
                        vectorSum *= maxSpeed; 
                        ; }

                base.Move[index] = vectorSum;
                base.Weighting[index] = 1; 
                }

            }
        }
    }
