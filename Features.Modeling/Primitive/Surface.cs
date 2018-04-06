using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;
namespace Features.Modeling.Primitive
{
    class SurfaceFromPoints : PrimitiveFeature
    {
        public SurfaceFromPoints()
        {
            Name = "Surface.Points";     
        }

        public override bool Run(FeatureContext context)
        {
            var points = new List<Vector3>();
            points.Add(new Vector3(0, 0, 0));
            points.Add(new Vector3(50, 0, 0));
            points.Add(new Vector3(100, 0, 0));

            points.Add(new Vector3(0, 50, 0));
            points.Add(new Vector3(50, 50, 5));
            points.Add(new Vector3(100, 50, -5));

            points.Add(new Vector3(0, 150, 5));
            points.Add(new Vector3(50, 150, -5));
            points.Add(new Vector3(100, 150, 0));

            TopoShape face = GlobalInstance.BrepTools.MakeSurfaceFromPoints(points, 3, 3);

            context.ShowGeometry(face);
            return true;
        }
    }
}
