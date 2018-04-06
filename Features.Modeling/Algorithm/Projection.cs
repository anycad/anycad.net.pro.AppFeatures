using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;

namespace Features.Modeling.Algorithm
{
    class ProjectionPlane : Feature
    {
        public ProjectionPlane()
        {
            Name = "Projection.Plane";
            Group = ModelingModule.GroupId;
            Category = ModelingModule.AlgorithmCategoryId;
        }

        public override bool Run(FeatureContext context)
        {
            // construct a wire;
            var points = new System.Collections.Generic.List<Vector3>();
            points.Add(new Vector3(0, 0, 0));
            points.Add(new Vector3(0, 100, 0));
            points.Add(new Vector3(100, 100, 0));
            TopoShape wire = GlobalInstance.BrepTools.MakePolygon(points);
            context.ShowGeometry(wire);

            // project the wire to two planes
            Vector3 dirPlane1 = new Vector3(0, 1, 1);
            dirPlane1.Normalize();
            TopoShape newWire1 = GlobalInstance.BrepTools.ProjectOnPlane(wire, new Vector3(0, 0, 100),
                dirPlane1, new Vector3(0, 0, 1));

            Vector3 dirPlane2 = new Vector3(0, 1, -1);
            dirPlane2.Normalize();
            TopoShape newWire2 = GlobalInstance.BrepTools.ProjectOnPlane(wire, new Vector3(0, 0, 500),
                dirPlane2, new Vector3(0, 0, 1));

            // make loft
            TopoShapeGroup tsg = new TopoShapeGroup();
            tsg.Add(newWire1);
            tsg.Add(newWire2);
            TopoShape loft = GlobalInstance.BrepTools.MakeLoft(tsg, false);
            context.ShowGeometry(loft);

            return true;
        }
    }
}
