using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;

namespace Features.Modeling.Algorithm
{
    class MakeEvolved : Feature
    {
        public MakeEvolved()
        {
            Name = "Evolved";
            Group = ModelingModule.GroupId;
            Category = ModelingModule.FeaturedCategoryId;
        }

        public override bool Run(FeatureContext context)
        {

            //1. Create the path
            float radius = 100;
            TopoShape arc = GlobalInstance.BrepTools.MakeArc(Vector3.ZERO, new Vector3(-radius, -radius, 0), new Vector3(0, -radius, 0), Vector3.UNIT_Z);
            TopoShape line = GlobalInstance.BrepTools.MakeLine(new Vector3(-radius, -radius, 0), new Vector3(-radius, -radius * 2, 0));

            TopoShapeGroup edges = new TopoShapeGroup();
            edges.Add(arc);
            edges.Add(line);
            TopoShape wire = GlobalInstance.BrepTools.MakeWire(edges);


            Vector3 dirZ = new Vector3(1, -1, 0);
            dirZ.Normalize();
            Vector3 dirX = dirZ.CrossProduct(Vector3.UNIT_Z);
            Coordinate3 coord = new Coordinate3(Vector3.ZERO, dirX, Vector3.UNIT_Z, dirZ);


            TopoShape path = GlobalInstance.BrepTools.Transform(wire, coord);
            context.ShowGeometry(path);

            //2. Create the profile
            List<Vector3> points = new List<Vector3>();
            points.Add(new Vector3());
            points.Add(new Vector3(200, 0, 0));
            points.Add(new Vector3(200, 200, 0));
            points.Add(new Vector3(0, 200, 0));

            TopoShape polygon = GlobalInstance.BrepTools.MakePolygon(points);

            // 3. Make body
            AdvFeatureTools advFT = new AdvFeatureTools();
            TopoShape shape = advFT.MakeEvolved(polygon, path, 0, true);

            context.ShowGeometry(shape);

            return true;
        }
    }
}
