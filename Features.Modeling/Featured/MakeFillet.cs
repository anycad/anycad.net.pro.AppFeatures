using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;

namespace Features.Modeling.Algorithm
{
    class MakeFillet : Feature
    {
        public MakeFillet()
        {
            Name = "Fillet";
            Group = ModelingModule.GroupId;
            Category = ModelingModule.FeaturedCategoryId;
        }

        public override bool Run(FeatureContext context)
        {
            // 1. Create Solid by extrude the section
            Vector3 start = new Vector3(100, 0, 0);
            Vector3 end = new Vector3(0, 100, 0);

            TopoShapeGroup group = new TopoShapeGroup();
            group.Add(GlobalInstance.BrepTools.MakeArc(start, end, Vector3.ZERO, Vector3.UNIT_Z));
            group.Add(GlobalInstance.BrepTools.MakeLine(Vector3.ZERO, start));
            group.Add(GlobalInstance.BrepTools.MakeLine(Vector3.ZERO, end));

            TopoShape section = GlobalInstance.BrepTools.MakeWire(group);
            TopoShape face = GlobalInstance.BrepTools.MakeFace(section);
            TopoShape solid = GlobalInstance.BrepTools.Extrude(face, 50, Vector3.UNIT_Z);

            // 2. Fillet the specified edges with different radius.
            int[] edges = {4, 6, 8};
            float[] radius = {5, 5, 10};

            TopoShape chamfer = GlobalInstance.BrepTools.MakeFillet(solid, edges, radius);

            context.ShowGeometry(chamfer);
            return true;
        }
    }
}
