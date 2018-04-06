using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;

namespace Features.Modeling.Algorithm
{
    class MakeRevole : Feature
    {
        public MakeRevole()
        {
            Name = "Revole";
            Group = ModelingModule.GroupId;
            Category = ModelingModule.FeaturedCategoryId;
        }

        public override bool Run(FeatureContext context)
        {
            int size = 10;
            // Create the outline edge
            TopoShape arc = GlobalInstance.BrepTools.MakeArc3Pts(new Vector3(-size, 0, 0), new Vector3(size, 0, 0), new Vector3(0, size, 0));
            TopoShape line1 = GlobalInstance.BrepTools.MakeLine(new Vector3(-size, -size, 0), new Vector3(-size, 0, 0));
            TopoShape line2 = GlobalInstance.BrepTools.MakeLine(new Vector3(size, -size, 0), new Vector3(size, 0, 0));
            TopoShape line3 = GlobalInstance.BrepTools.MakeLine(new Vector3(-size, -size, 0), new Vector3(size, -size, 0));

            TopoShapeGroup shapeGroup = new TopoShapeGroup();
            shapeGroup.Add(line1);
            shapeGroup.Add(arc);
            shapeGroup.Add(line2);
            shapeGroup.Add(line3);

            TopoShape wire = GlobalInstance.BrepTools.MakeWire(shapeGroup);

            TopoShape revole = GlobalInstance.BrepTools.Revol(wire, new Vector3(size * 3, 0, 0), new Vector3(0, 1, 0), 145);

            context.ShowGeometry(revole);

            return true;
        }
    }
}
