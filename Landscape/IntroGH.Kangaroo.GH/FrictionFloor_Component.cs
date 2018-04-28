using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry; 

namespace IntroGH.Kangaroo.GH {
   public class FrictionFloor : GH_Component { //components are always public !!!

        public FrictionFloor() : base("Friction Floor", "FFloor", "Friction floor", "Extra", "Goals") {; }

        public override Guid ComponentGuid => new Guid("{83B1FF6F-DA2B-4C8D-9E4E-A5B22C48D152}");

        protected override void RegisterInputParams(GH_InputParamManager pManager) {
            pManager.AddPlaneParameter("Plane", "P", "Floor plane", GH_ParamAccess.item, Rhino.Geometry.Plane.WorldXY);
            pManager.AddNumberParameter("Friction", "F", "Friction", GH_ParamAccess.item, 0.5);
            pManager.AddPointParameter("Particles", "P", "Particles", GH_ParamAccess.list);  
            }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager) {
            pManager.AddGenericParameter("Goal", "G", "Goal", GH_ParamAccess.item);
            }

        protected override void SolveInstance(IGH_DataAccess DA) {
            Plane fp = Plane.WorldXY;
            double el = 0.5;
            List<Point3d> pts = new List<Point3d>();

            if (!(DA.GetData(0, ref fp))) { return; }
            if (!(DA.GetData(1, ref el))) { return; }
            if (!(DA.GetDataList(2, pts))) { return; }

            DA.SetData(0, new CustomGoals.FrictionFloor(fp, el, pts)); 
            }
        }
    }
