using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;
using System.Windows.Forms;
using AnyCAD.Exchange;

namespace Features.Rendering.Exchange
{
    class LineStyleTest : Feature
    {
        public LineStyleTest()
        {
            Name = "LineStyle";
            Group = RenderingModule.GroupId;
            Category = RenderingModule.StyleCategoryId;
        }

        public override bool Run(FeatureContext context)
        {
            TopoShape shape = GlobalInstance.BrepTools.MakeArc(Vector3.ZERO, 100, 10, 180, Vector3.UNIT_Z);
            SceneNode node = context.ShowGeometry(shape);
            LineStyle ls = new LineStyle();
            ls.SetLineWidth(2);
            ls.SetColor(0, 255, 0);
            ls.SetPatternStyle((int)EnumLinePattern.LP_DashedLine);
            node.SetLineStyle(ls);

            return true;
        }
    }
}
