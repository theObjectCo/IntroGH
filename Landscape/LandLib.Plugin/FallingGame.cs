using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using IntroGH.LandLib.Core;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LandLib.Plugin {
    public class FallingGame : GH_Component {

        public FallingGame() : base("Falling Game", "Falling Game", "The last one standing", "Landscape", "Games") {
            }

        public override Guid ComponentGuid => new Guid("{1CF6B874-01B7-496F-B3B9-348C5219AB86}");

        protected override void RegisterInputParams(GH_InputParamManager pManager) {
            pManager.AddIntegerParameter("Seed", "S", "Randomness seed", GH_ParamAccess.item);
            pManager.AddMeshParameter("Tree", "T", "Tree geometry", GH_ParamAccess.item);
            pManager.AddMeshParameter("Sheep", "S", "Sheep geometry", GH_ParamAccess.item);
            }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager) {
            pManager.AddSurfaceParameter("Landscape", "L", "Landscape surface", GH_ParamAccess.item);
            pManager.AddCurveParameter("Contours", "C", "Landscape contours", GH_ParamAccess.list);
            pManager.AddMeshParameter("Water", "W", "Water", GH_ParamAccess.item);
            pManager.AddMeshParameter("Trees", "T", "Trees", GH_ParamAccess.list);
            pManager.AddMeshParameter("Sheeps", "S", "Sheeps", GH_ParamAccess.item);
            }

        private int mySeed = -1;
        private Landscape myWorld = null;

        protected override void SolveInstance(IGH_DataAccess DA) {

            //fields
            int thisSeed = 0;
            Mesh tree = null;
            Mesh sheep = null;

            //getting data 
            if (!(DA.GetData(0, ref thisSeed))) {
                myWorld = null;
                }

            if (!(DA.GetData(1, ref tree))) {
                myWorld = null;
                }

            if (!(DA.GetData(2, ref sheep))) {
                myWorld = null;
                }

            //if the seed is updated 
            if (thisSeed != mySeed) {
                //restart everything
                //update the seed 
                mySeed = thisSeed;

                myWorld = null;

                //variables for the new world
                Random rnd = new Random(mySeed);
                double width = ((rnd.NextDouble() * 0.5) + 1) * 100;
                double depth = ((rnd.NextDouble() * 0.5) + 1) * 100;
                double height = ((rnd.NextDouble() * 0.5) + 1) * 40;

                double sheepPerSQM = (rnd.NextDouble() * 0.002) + 0.002;

                //create a new world
                myWorld = new Landscape(width, depth, height, tree, sheep, sheepPerSQM, mySeed);
                myWorld.Generate(); //bug 1
                }

            //run the world 
            if (myWorld != null) {

                //see if there is a winner 
                int walkingSheeps = 0;
                foreach (Sheep thisSheep in myWorld.sheeps) {
                    if (!(thisSheep.falling)) { walkingSheeps += 1; } //bug 2
                    }

                if (walkingSheeps == 0) {
                    //there is no winner, we stop the game 
                    System.Media.SystemSounds.Exclamation.Play();
                    }
                if (walkingSheeps == 1) {
                    //there is a winner
                    System.Media.SystemSounds.Asterisk.Play();
                    }
                if (walkingSheeps > 1) { //bug 3
                    //just run the game 
                    myWorld.WalkSheeps();
                    }

                //pManager.AddSurfaceParameter("Landscape", "L", "Landscape surface", GH_ParamAccess.item);
                //pManager.AddCurveParameter("Contours", "C", "Landscape contours", GH_ParamAccess.list);
                //pManager.AddMeshParameter("Water", "W", "Water", GH_ParamAccess.item);
                //pManager.AddMeshParameter("Trees", "T", "Trees", GH_ParamAccess.list);
                //pManager.AddMeshParameter("Sheeps", "S", "Sheeps", GH_ParamAccess.item);

                DA.SetData(0, myWorld.land);
                DA.SetDataList(1, myWorld.contour);
                DA.SetData(2, myWorld.water);
                DA.SetDataList(3, myWorld.trees);
                DA.SetData(4, myWorld.GetSheeps()); //change the param access to item in the registerinputparams method.

                }

            }
        }
    }


