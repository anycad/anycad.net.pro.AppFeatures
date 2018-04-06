using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;

namespace Features.Modeling.Algorithm
{
    class MakeSweep : Feature
    {
        public MakeSweep()
        {
            Name = "Sweep.Evolution";
            Group = ModelingModule.GroupId;
            Category = ModelingModule.FeaturedCategoryId;
        }

        public override bool Run(FeatureContext context)
        {
            AdvFeatureTools advTool = new AdvFeatureTools();

            // Create the evolution spline
            Primitive2dTools tool2d = new Primitive2dTools();
            float radius = 50;
            TopoShapeGroup group = new TopoShapeGroup();
            group.Add(tool2d.MakeArc(new Vector2(0, radius), radius, 0, 45));
            group.Add(tool2d.MakeLine(new Vector2(radius, radius), new Vector2(radius * 2, radius)));
            TopoShape spline = tool2d.ToBSplineCurve(group);

            // Create the profile section
            TopoShape profile = GlobalInstance.BrepTools.MakeCircle(new Vector3(100, 100, 0), 1, Vector3.UNIT_Z);
            
            // Create the path
            List<Vector3> pts = new List<Vector3>();
            pts.Add(new Vector3(100, 100, 0));
            pts.Add(new Vector3(100, 100, 100));
            pts.Add(new Vector3(100, 200, 400));
            TopoShape path = GlobalInstance.BrepTools.MakeSpline(pts);

            // Make sweep
            TopoShape sweepBody = advTool.MakeSweep(profile, path, spline, true);

            context.ShowGeometry(sweepBody);

            return true;
        }
    }
}
