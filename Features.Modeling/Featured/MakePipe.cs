using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;

namespace Features.Modeling.Algorithm
{
    class MakePipe : Feature
    {
        public MakePipe()
        {
            Name = "Sweep.Pipe";
            Group = ModelingModule.GroupId;
            Category = ModelingModule.FeaturedCategoryId;
        }

        public override bool Run(FeatureContext context)
        {
            for (int ii = 0; ii < 3; ++ii)
            {
                // Path
                Vector3 startPt = new Vector3(ii * 100, 0, 0);
                var points = new System.Collections.Generic.List<Vector3>();
                points.Add(startPt);
                points.Add(startPt + new Vector3(0, 0, 100));
                points.Add(startPt + new Vector3(50, 50, 150));
                TopoShape path = GlobalInstance.BrepTools.MakePolyline(points);

                // Profile
                TopoShape section = GlobalInstance.BrepTools.MakeCircle(startPt, 10, Vector3.UNIT_Z);

                // ii is used to define the joint style.
                TopoShape pipe = GlobalInstance.BrepTools.MakePipe(section, path, ii);

                context.ShowGeometry(pipe);
            }

            return true;
        }
    }

    class MakePipe2 : Feature
    {
        public MakePipe2()
        {
            Name = "Sweep.Solid";
            Group = ModelingModule.GroupId;
            Category = ModelingModule.FeaturedCategoryId;
        }

        public override bool Run(FeatureContext context)
        {
            List<Vector3> pts = new List<Vector3>();
            pts.Add(new Vector3(0, 0, 0));
            pts.Add(new Vector3(10, 0, 0));
            pts.Add(new Vector3(10, 100, 0));
            pts.Add(new Vector3(0, 100, 0));
            TopoShape rect = GlobalInstance.BrepTools.MakePolygon(pts);

            TopoShape arc = GlobalInstance.BrepTools.MakeArc(new Vector3(-100, 0, 100),new Vector3(0, 0, 0), new Vector3(-100, 0, 0), Vector3.UNIT_Y);
            //TopoShape arc = GlobalInstance.BrepTools.MakeArc(new Vector3(0, 0, 0), new Vector3(100, 100, 0), new Vector3(100, 0, 0), new Vector3(0, 0, -1));
            //context.ShowGeometry(arc);
            //GeomCurve curve = new GeomCurve();
            //curve.Initialize(arc);
            //var d1 = curve.D1(curve.FirstParameter());
            ////Vector3 dir = d1[1];
            ////dir = dir.CrossProduct(Vector3.UNIT_Z);
            ////float x = dir.AngleBetween(Vector3.UNIT_Y);
            //Matrix4 mm = GlobalInstance.MatrixBuilder.MakeRotation(90, Vector3.UNIT_X);
            //Matrix4 trans = GlobalInstance.MatrixBuilder.MakeTranslate(d1[0]);
            //Matrix4 trf = GlobalInstance.MatrixBuilder.Multiply(trans, mm);
            //rect = GlobalInstance.BrepTools.Transform(rect, trf);

            rect = GlobalInstance.BrepTools.MakePipe(rect, arc, 0);

            SceneNode sn = context.ShowGeometry(rect);

            return true;
        }
    }

}
