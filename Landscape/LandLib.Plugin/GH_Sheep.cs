using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing; 

namespace IntroGH.LandLib.Plugin {
    public class GH_Sheep : GH_GeometricGoo<IntroGH.LandLib.Core.Sheep>, Grasshopper.Kernel.IGH_PreviewData{

        //Display Material for DrawViewportWires
        static private Rhino.Display.DisplayMaterial displayMat = new Rhino.Display.DisplayMaterial(System.Drawing.Color.White);

        public GH_Sheep() { }

        public GH_Sheep(Core.Sheep Sheep) {
            this.Value = Sheep; 
            }

        public override BoundingBox Boundingbox => GetBoundingBox(Rhino.Geometry.Transform.Identity);

        public override string TypeName => "Sheep";

        public override string TypeDescription => "Sheep";

        public BoundingBox ClippingBox => this.Boundingbox;

        public void DrawViewportMeshes(GH_PreviewMeshArgs args) {
            args.Pipeline.DrawMeshShaded(this.Value.GetGeometry(), displayMat);
            }

        public void DrawViewportWires(GH_PreviewWireArgs args) {
            return;
            }

        public override IGH_GeometricGoo DuplicateGeometry() {
            return new GH_Sheep(this.Value.Duplicate()); 
            }

        public override BoundingBox GetBoundingBox(Transform xform) {
            return this.Value.GetGeometry().GetBoundingBox(xform); 
            }

        public override IGH_GeometricGoo Morph(SpaceMorph xmorph) {
            return this; 
            }

        public override string ToString() {
            return "Sheep"; 
            }

        public override IGH_GeometricGoo Transform(Transform xform) {
            this.Value.Transform(xform);
            return this; 
            }
        }
    }
