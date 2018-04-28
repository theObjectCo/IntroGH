using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandLib.Plugin {
    public class AddComp : GH_Component {
         
        public AddComp() : base("Addition", "+", "Addition component", "Landscape", "Math") { }

        public override Guid ComponentGuid => new Guid("{B8E4F1AD-D765-4DF3-8AF0-8C7A7ED06B6D}");

        protected override void RegisterInputParams(GH_InputParamManager pManager) {
            pManager.AddNumberParameter("A", "A", "Number A", GH_ParamAccess.item);
            pManager.AddNumberParameter("B", "B", "Number B", GH_ParamAccess.item);
            }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager) {
            pManager.AddNumberParameter("C", "C", "Number C", GH_ParamAccess.item);
            }

        protected override void SolveInstance(IGH_DataAccess DA) {
            //declare variables
            double na = 0;
            double nb = 0;

            //get the data from the inputs
            if (!(DA.GetData(0, ref na))) { return; }
            if (!(DA.GetData(1, ref nb))) { return; }

            //process the data
            double nc = na + nb;

            //output the data
            DA.SetData(0, nc);

            }
        }
    }
