using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;

namespace Features.Modeling.Algorithm
{
    class MakeSplit : Feature
    {
        public MakeSplit()
        {
            Name = "Split";
            Group = ModelingModule.GroupId;
            Category = ModelingModule.AlgorithmCategoryId;
        }

        public override bool Run(FeatureContext context)
        {
            // Split box with two spheres.
            TopoShape box = GlobalInstance.BrepTools.MakeBox(Vector3.ZERO, Vector3.UNIT_Z, new Vector3(150f, 150f, 150f));
            TopoShape sphere = GlobalInstance.BrepTools.MakeSphere(Vector3.ZERO, 100f);
            TopoShape sphere2 = GlobalInstance.BrepTools.MakeSphere(Vector3.ZERO, 50f);

            TopoShapeGroup tools = new TopoShapeGroup();
            tools.Add(sphere);
            tools.Add(sphere2);
            TopoShape split = GlobalInstance.BrepTools.MakeSplit(box, tools);

            // Display the results with customized face styles.
            TopoExplor expo = new TopoExplor();
            TopoShapeGroup bodies = expo.ExplorSolids(split);


            Random random = new Random();
            for (int ii = 0; ii < bodies.Size(); ++ii)
            {
                SceneNode node = context.ShowGeometry(bodies.GetTopoShape(ii));
                FaceStyle fs = new FaceStyle();
                fs.SetColor(new ColorValue((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(), 0.5f));
                fs.SetTransparent(true);
                node.SetFaceStyle(fs);
            }

            return true;
        }
    }
}
