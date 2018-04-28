using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroGH.LandLib.Plugin {
    class GH_Sheep : GH_GeometricGoo<IntroGH.LandLib.Core.Sheep> {
        public override BoundingBox Boundingbox => throw new NotImplementedException();

        public override string TypeName => throw new NotImplementedException();

        public override string TypeDescription => throw new NotImplementedException();

        public override IGH_GeometricGoo DuplicateGeometry() {
            throw new NotImplementedException();
            }

        public override BoundingBox GetBoundingBox(Transform xform) {
            throw new NotImplementedException();
            }

        public override IGH_GeometricGoo Morph(SpaceMorph xmorph) {
            throw new NotImplementedException();
            }

        public override string ToString() {
            throw new NotImplementedException();
            }

        public override IGH_GeometricGoo Transform(Transform xform) {
            throw new NotImplementedException();
            }
        }
    }
