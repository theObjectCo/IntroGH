using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace IntroGH.LandLib.Plugin {
   public class Param_Sheep : Grasshopper.Kernel.GH_PersistentGeometryParam<GH_Sheep>, Grasshopper.Kernel.IGH_PreviewObject {

        public Param_Sheep() : base(new GH_InstanceDescription("Sheep", "S", "Sheep", "Landscape", "Params")) {}

        public override Guid ComponentGuid => new Guid("{9D3C0595-3DE1-4B6F-8BC2-4E8590126C98}");

        private bool hid = false; 
        public bool Hidden { get => hid; set => hid = value; }

        public bool IsPreviewCapable => true;

        public BoundingBox ClippingBox => Preview_ComputeClippingBox();

        public void DrawViewportMeshes(IGH_PreviewArgs args) {
            Preview_DrawMeshes(args);
            }

        public void DrawViewportWires(IGH_PreviewArgs args) {
            return;
            }

        protected override GH_GetterResult Prompt_Plural(ref List<GH_Sheep> values) {
            return GH_GetterResult.cancel; 
            }

        protected override GH_GetterResult Prompt_Singular(ref GH_Sheep value) {
            return GH_GetterResult.cancel;
            }
        }
    }
