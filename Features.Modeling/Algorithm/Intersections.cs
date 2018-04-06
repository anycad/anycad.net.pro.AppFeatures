using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;
using System.Windows.Forms;

namespace Features.Modeling.Algorithm
{
    class CurveLineIntersection : Feature
    {
        public CurveLineIntersection()
        {
            Name = "Intersection.CL";
            Group = ModelingModule.GroupId;
            Category = ModelingModule.AlgorithmCategoryId;
        }

        public override bool Run(FeatureContext context)
        {
            context.RenderView.SetDisplayMode((int)(EnumDisplayStyle.DS_ShadeEdge | EnumDisplayStyle.DS_Vertex));
            // construct a wire;
            TopoShape TS = GlobalInstance.BrepTools.MakeEllipse(Vector3.ZERO, 100D, 50D, Vector3.UNIT_Z);
            context.ShowGeometry(TS);

            TopoShape line = GlobalInstance.BrepTools.MakeLine(new Vector3(0, -200, 0), new Vector3(200, 200, 0));
            context.ShowGeometry(line);

            IntersectionLineCurve intersector = new IntersectionLineCurve();

            TopoExplor tp = new TopoExplor();
            TopoShapeGroup tg = tp.ExplorEdges(TS);
            intersector.SetCurve(tg.GetAt(0));

            if (intersector.Perform(line))
            {

                int nCount = intersector.GetPointCount();

                List<Vector3> LV = new List<Vector3>();

                for (int ii = 0; ii < nCount; ++ii)
                {

                    Vector3 pt = intersector.GetPoint(ii+1);
                    LV.Add(pt);

                    context.ShowGeometry(GlobalInstance.BrepTools.MakePoint(pt));
                }

                         
                MessageBox.Show(String.Format("{0}", nCount));
            }

            

            return true;
        }
    }
}
