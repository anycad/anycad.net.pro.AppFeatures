using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;

namespace Features.Modeling.Algorithm
{
    class SurfaceSection : Feature
    {
        public SurfaceSection()
        {
            Name = "Section.Surface";
            Group = ModelingModule.GroupId;
            Category = ModelingModule.AlgorithmCategoryId;
        }

        public override bool Run(FeatureContext context)
        {
            // build two surfaces
            TopoShape arc = GlobalInstance.BrepTools.MakeArc(Vector3.ZERO, 100, 0, 135, Vector3.UNIT_Z);
            TopoShape cir = GlobalInstance.BrepTools.MakeCircle(new Vector3(-200, 0, 0), 50, Vector3.UNIT_X);
            TopoShape surf1 = GlobalInstance.BrepTools.Extrude(arc, 100, Vector3.UNIT_Z);
            TopoShape surf2 = GlobalInstance.BrepTools.Extrude(cir, 400, Vector3.UNIT_X);

            SceneNode n1 = context.ShowGeometry(surf1);
            {
                FaceStyle fs1 = new FaceStyle();
                fs1.SetColor(new ColorValue(0, 0, 0.5f, 0.5f));
                fs1.SetTransparent(true);
                n1.SetFaceStyle(fs1);
            }
            SceneNode n2 = context.ShowGeometry(surf2);
            {
                FaceStyle fs2 = new FaceStyle();
                fs2.SetColor(new ColorValue(0, 0.5f, 0.5f, 0.5f));
                fs2.SetTransparent(true);
                n2.SetFaceStyle(fs2);
            }

            // compute section wire
            TopoShape wire = GlobalInstance.BrepTools.SurfaceSection(surf1, surf2);

            SceneNode sectionNode = context.ShowGeometry(wire);
            LineStyle lineStyle = new LineStyle();
            lineStyle.SetLineWidth(4);
            lineStyle.SetColor(ColorValue.RED);
            sectionNode.SetLineStyle(lineStyle);

            return true;
        }
    }
}
