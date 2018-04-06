using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;

namespace Features.Modeling.Geometry
{
    class QuertySurface : Feature
    {
        public QuertySurface()
        {
            Name = "Surface";
            Group = ModelingModule.GroupId;
            Category = ModelingModule.GeometryCategoryId;
        }

        public override bool Run(FeatureContext context)
        {
            LineStyle lineStyle = new LineStyle();
            lineStyle.SetLineWidth(0.5f);
            lineStyle.SetColor(ColorValue.RED);

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

            GeomSurface surface = new GeomSurface();
            surface.Initialize(face);
            double ufirst = surface.FirstUParameter();
            double uLarst = surface.LastUParameter();
            double vfirst = surface.FirstVParameter();
            double vLast = surface.LastVParameter();

            double ustep = (uLarst - ufirst) * 0.1;
            double vstep = (vLast - vfirst) * 0.1;
            for (double ii = ufirst; ii <= uLarst; ii += ustep)
                for (double jj = vfirst; jj <= vLast; jj += vstep)
                {
                    var data = surface.D1(ii, jj);

                    Vector3 pos = data[0];
                    Vector3 dirU = data[1];
                    Vector3 dirV = data[2];
                    Vector3 dir = dirV.CrossProduct(dirU);
                    //dir.Normalize();
                    {
                        TopoShape line = GlobalInstance.BrepTools.MakeLine(pos, pos + dir * 0.01f);
                        SceneNode node = context.ShowGeometry(line);

                        node.SetLineStyle(lineStyle);
                    }
                }

            return true;
        }
    }
}
