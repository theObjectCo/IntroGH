using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace IntroGH.Kangaroo.GH {
    public class ChargeGoal_Component : GH_Component {

        //constructor 
        public ChargeGoal_Component() : base("Charge Goal", "CGoal","Charge goal", "Extra", "Goals") { }
        
        //component id number 
        public override Guid ComponentGuid => new Guid("{A059716F-3EEF-46D5-9483-E1B85621DB83}");
        
        //component inputs 
        protected override void RegisterInputParams(GH_InputParamManager pManager) {
            pManager.AddPointParameter("Charges", "C", "Charge locations", GH_ParamAccess.list);
            pManager.AddPointParameter("Points", "P", "Point locations", GH_ParamAccess.list);
            pManager.AddNumberParameter("Targets", "T", "Points' target charges", GH_ParamAccess.list);
            pManager.AddNumberParameter("Velocity", "V", "Terminal velocity", GH_ParamAccess.item); //max Speed for the particles
            }

        //component outputs 
        protected override void RegisterOutputParams(GH_OutputParamManager pManager) {
            pManager.AddGenericParameter("Goal", "G", "Goal", GH_ParamAccess.item); 
            }

        protected override void SolveInstance(IGH_DataAccess DA) {
            List<Point3d> charges = new List<Point3d>();
            List<Point3d> points = new List<Point3d>();
            List<double> targets = new List<double>();
            double maxSpeed = 1; 
            
            if (!(DA.GetDataList(0, charges))) { return; }
            if (!(DA.GetDataList(1, points))) { return; }
            if (!(DA.GetDataList(2, targets))) { return; }
            if (!(DA.GetData(3,ref maxSpeed))) { return; }

            //extend the targets list if there is not enough targets (longest list beehavior) 
            for (int i = targets.Count-1 ; i < points.Count-1; i++) {
                targets.Add(targets.Last()); 
                }

            CustomGoals.ChargeGoal goal = new CustomGoals.ChargeGoal(charges, points, targets, maxSpeed);

            DA.SetData(0, goal); 
            }
        }
    }
