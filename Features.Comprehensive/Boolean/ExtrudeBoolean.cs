using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;

namespace Features.Comprehensive.Boolean
{
    class ExtrudeBoolean : Feature
    {
        public ExtrudeBoolean()
        {
            Group = ComprehensiveModule.GroupId;
            Category = ComprehensiveModule.BooleanCategoryId;
            Name = "Extrude.Boolean";      
        }

        public override bool Run(FeatureContext context)
        {

            // Create sketch on XZ plane
            float width = 20;
            float height = 20;
            float thickness = 5;
            float length = 100;
            List<Vector3> points = new List<Vector3>();
            points.Add(Vector3.ZERO);
            points.Add(new Vector3(width, 0, 0));
            points.Add(new Vector3(width, 0, thickness));
            points.Add(new Vector3(thickness, 0, thickness));
            points.Add(new Vector3(thickness, 0, height));
            points.Add(new Vector3(0, 0, height));

            TopoShape polygon = GlobalInstance.BrepTools.MakePolygon(points);
            TopoShape face = GlobalInstance.BrepTools.MakeFace(polygon);

            // Extrude along Y direction.
            TopoShape extrude = GlobalInstance.BrepTools.Extrude(face, length, Vector3.UNIT_Y);

            // Cylinders
            float radius = 2;
            TopoShapeGroup groups = new TopoShapeGroup();
            groups.Add(GlobalInstance.BrepTools.MakeCylinder(new Vector3(0, 10, 10), Vector3.UNIT_X, radius, thickness, 0));
            groups.Add(GlobalInstance.BrepTools.MakeCylinder(new Vector3(0, 50, 10), Vector3.UNIT_X, radius, thickness, 0));
            groups.Add(GlobalInstance.BrepTools.MakeCylinder(new Vector3(10, 50, 0), Vector3.UNIT_Z, radius, thickness, 0));

            TopoShape compound = GlobalInstance.BrepTools.MakeCompound(groups);

            // Cut
            TopoShape cut = GlobalInstance.BrepTools.BooleanCut(extrude, compound);

            // Dispaly 
            context.ShowGeometry(cut);
            return true;
        }
    }
}
