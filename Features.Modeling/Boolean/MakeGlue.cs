using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;

namespace Features.Modeling.Algorithm
{
    class MakeGlue : Feature
    {
        public MakeGlue()
        {
            Name = "Glue";
            Group = ModelingModule.GroupId;
            Category = ModelingModule.BooleanCategoryId;
        }

        public override bool Run(FeatureContext context)
        {
            TopoShape box1 = GlobalInstance.BrepTools.MakeBox(Vector3.ZERO, Vector3.UNIT_Z, new Vector3(100, 100, 100));
            TopoShape box2 = GlobalInstance.BrepTools.MakeBox(new Vector3(0, 0, -100), Vector3.UNIT_Z, new Vector3(100, 100, 100));

            TopoShapeGroup group = new TopoShapeGroup();
            group.Add(box1);
            group.Add(box2);

            TopoShape compound = GlobalInstance.BrepTools.MakeCompound(group);
   
            RepairTools repairTool = new RepairTools();
            TopoShape glue = repairTool.GlueFaces(compound, 0.00001f, true);
            //TopoShape newBody = repairTool.RemoveExtraEdges(glue);
            context.ShowGeometry(glue);

            return true;
        }
    }
}
