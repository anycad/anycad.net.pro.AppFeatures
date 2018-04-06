using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;


namespace Features.Comprehensive.Algorithm
{
    class SurfaceSection : Feature
    {
        public SurfaceSection()
        {
            Name = "Section.Surface";
            Group = ComprehensiveModule.GroupId;
            Category = ComprehensiveModule.AlgorithmCategoryId;
        }

        public override bool Run(FeatureContext context)
        {
            TopoShape oCircle1 = GlobalInstance.BrepTools.MakeCircle(Vector3.ZERO, 20, Vector3.UNIT_Z);
            TopoShape Pipe01_Surf = GlobalInstance.BrepTools.Extrude(oCircle1, 100, Vector3.UNIT_Z);
            context.ShowGeometry(Pipe01_Surf);

            TopoShape oCircle2 = GlobalInstance.BrepTools.MakeCircle(new Vector3(0.0f, 0.0f, 50.0f), 10, Vector3.UNIT_Y);
            TopoShape Pipe02_Surf = GlobalInstance.BrepTools.Extrude(oCircle2, 80, Vector3.UNIT_Y);
            {
                SceneNode node2 = context.ShowGeometry(Pipe02_Surf);
                FaceStyle fs = new FaceStyle();
                fs.SetColor(new ColorValue(0.5f, 0.5f, 0.5f, 0.5f));
                fs.SetTransparent(true);
                node2.SetFaceStyle(fs);
            }

            // Compute the intersection curve
            TopoShape Inters1 = GlobalInstance.BrepTools.SurfaceSection(Pipe01_Surf, Pipe02_Surf);
          
            if (Inters1 != null)
            {
                SceneNode node = context.ShowGeometry(Inters1);
                LineStyle ls = new LineStyle();
                ls.SetLineWidth(3);
                ls.SetColor(ColorValue.RED);
                node.SetLineStyle(ls);

                // Get the curve parameters
                GeomCurve curve = new GeomCurve();
                if (curve.Initialize(Inters1))
                {
                    LineStyle ls2 = new LineStyle();
                    ls2.SetColor(ColorValue.GREEN);

                    double start = curve.FirstParameter();
                    double end = curve.LastParameter();
                    for (double ii = start; ii <= end; ii += 0.1)
                    {
                        List<Vector3> rt = curve.D1(ii);
                        LineNode ln = new LineNode();
                        ln.SetLineStyle(ls2);
                        ln.Set(rt[0], rt[0] + rt[1]);
                        context.ShowSceneNode(ln);
                    }
                }
            }

            return true;
        }
    }
}
