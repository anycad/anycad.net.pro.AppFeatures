using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;

namespace Features.Modeling.Algorithm
{
    class MakeSection : Feature
    {
        public MakeSection()
        {
            Name = "Section";
            Group = ModelingModule.GroupId;
            Category = ModelingModule.BooleanCategoryId;
        }

        public override bool Run(FeatureContext context)
        {
            TopoShape cylinder = GlobalInstance.BrepTools.MakeCylinder(Vector3.ZERO, Vector3.UNIT_Z, 100, 200, 270);
            TopoShape section = GlobalInstance.BrepTools.BodySection(cylinder, new Vector3(0, 0, 50), Vector3.UNIT_Z);

            context.ShowGeometry(cylinder);

            SceneNode node = context.ShowGeometry(section);
            LineStyle ls = new LineStyle();
            ls.SetLineWidth(3.0f);
            ls.SetColor(255, 0, 0);
            node.SetLineStyle(ls);

            return true;
        }
    }
}
