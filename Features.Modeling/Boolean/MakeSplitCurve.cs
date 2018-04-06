using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;

namespace Features.Modeling.Algorithm
{
    class MakeSplitCurve : Feature
    {
        public MakeSplitCurve()
        {
            Name = "Split Curve";
            Group = ModelingModule.GroupId;
            Category = ModelingModule.BooleanCategoryId;
        }

        public override bool Run(FeatureContext context)
        {
            TopoShape arc = GlobalInstance.BrepTools.MakeArc(new Vector3(-100, 0, 0), new Vector3(100, 0, 0), Vector3.ZERO, Vector3.UNIT_Z);
            TopoShape line = GlobalInstance.BrepTools.MakeLine(new Vector3(100, 0, 0), new Vector3(100, 200, 0));

            TopoShapeGroup group = new TopoShapeGroup();
            group.Add(arc);
            group.Add(line);

            TopoShape wire = GlobalInstance.BrepTools.MakeSpline(group);

            TopoShape splitter1 = GlobalInstance.BrepTools.MakePoint(new Vector3(0, -100, 0));
            TopoShape splitter2 = GlobalInstance.BrepTools.MakePoint(new Vector3(100, 100, 0));

            TopoShapeGroup spliterGroup = new TopoShapeGroup();
            spliterGroup.Add(splitter1);
            spliterGroup.Add(splitter2);
            TopoShape result = GlobalInstance.BrepTools.MakeSplit(wire, spliterGroup);

            TopoExplor exp = new TopoExplor();
            TopoShapeGroup itmes = exp.ExplorSubShapes(result);
            for (int ii = 0; ii < itmes.Size(); ++ii)
            {
                SceneNode node = context.ShowGeometry(itmes.GetAt(ii));
                LineStyle ls = new LineStyle();
                ls.SetLineWidth(3.0f);
                ls.SetColor((ii + 1) * 50, ii * 20, ii * 10);
                node.SetLineStyle(ls);

            }


            return true;
        }
    }
}
