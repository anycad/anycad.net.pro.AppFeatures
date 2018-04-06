using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;

namespace Features.Modeling.Algorithm
{
    class MakeLoft : Feature
    {
        public MakeLoft()
        {
            Name = "Loft";
            Group = ModelingModule.GroupId;
            Category = ModelingModule.FeaturedCategoryId;
        }

        public override bool Run(FeatureContext context)
        {
            TopoShape circle = GlobalInstance.BrepTools.MakeCircle(new Vector3(0, 0, 50), 10, Vector3.UNIT_Z);
            TopoShape rect = GlobalInstance.BrepTools.MakeRectangle(40, 40, 5, new Coordinate3(new Vector3(-20, -20, 0), Vector3.UNIT_X, Vector3.UNIT_Y, Vector3.UNIT_Z));

            TopoShape loft = GlobalInstance.BrepTools.MakeLoft(rect, circle, true);

            context.ShowGeometry(loft);

            return true;
        }
    }
}
