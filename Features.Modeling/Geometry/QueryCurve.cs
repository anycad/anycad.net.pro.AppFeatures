using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;

namespace Features.Modeling.Geometry
{
    class QuertyCurve : Feature
    {
        public QuertyCurve()
        {
            Name = "Curve";
            Group = ModelingModule.GroupId;
            Category = ModelingModule.GeometryCategoryId;
        }

        public override bool Run(FeatureContext context)
        {
            LineStyle lineStyle = new LineStyle();
            lineStyle.SetLineWidth(0.5f);
            lineStyle.SetColor(ColorValue.BLUE);
            lineStyle.SetLineWidth(1.5f);
            LineStyle lineStyle2 = new LineStyle();
            lineStyle2.SetLineWidth(0.5f);
            lineStyle2.SetColor(ColorValue.GREEN);
            lineStyle2.SetLineWidth(2);
            TopoShape arc = GlobalInstance.BrepTools.MakeEllipseArc(Vector3.ZERO, 100, 50, 45, 270, Vector3.UNIT_Z);
            context.ShowGeometry(arc);

            {
                GeomCurve curve = new GeomCurve();
                curve.Initialize(arc);

                double paramStart = curve.FirstParameter();
                double paramEnd = curve.LastParameter();

                double step = (paramEnd - paramStart) * 0.1;

                for (double uu = paramStart; uu <= paramEnd; uu += step)
                {
                    Vector3 dir = curve.DN(uu, 1);
                    Vector3 pos = curve.Value(uu);

                    // 切线
                    {
                        TopoShape line = GlobalInstance.BrepTools.MakeLine(pos, pos + dir);
                        SceneNode node = context.ShowGeometry(line);
                        node.SetLineStyle(lineStyle);
                    }
                    // 法线
                    {
                        Vector3 dirN = dir.CrossProduct(Vector3.UNIT_Z);
                        TopoShape line = GlobalInstance.BrepTools.MakeLine(pos, pos + dirN);
                        SceneNode node = context.ShowGeometry(line);
                        node.SetLineStyle(lineStyle2);
                    }

                }

            }

            return true;
        }
    }
}
