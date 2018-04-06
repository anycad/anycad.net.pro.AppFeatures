using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;


namespace Features.Comprehensive.Algorithm
{
    class RemoveExtraEdges : Feature
    {
        public RemoveExtraEdges()
        {
            Name = "Repair.RemoveExtraEdges";
            Group = ComprehensiveModule.GroupId;
            Category = ComprehensiveModule.AlgorithmCategoryId;
        }

        public override bool Run(FeatureContext context)
        {
            float y = 10;
            TopoShape line1 = GlobalInstance.BrepTools.MakeLine(new Vector3(0, 0, 0 + y), new Vector3(4, 0, 0 + y));
            TopoShape line2 = GlobalInstance.BrepTools.MakeLine(new Vector3(4, 0, 0 + y), new Vector3(4, 0, 0.5f + y));
            TopoShape line3 = GlobalInstance.BrepTools.MakeLine(new Vector3(4, 0, 0.5f + y), new Vector3(1, 0, 0.5f + y));
            TopoShape line4 = GlobalInstance.BrepTools.MakeLine(new Vector3(1, 0, 0.5f + y), new Vector3(1, 0, 4 + y));
            TopoShape line5 = GlobalInstance.BrepTools.MakeLine(new Vector3(1, 0, 4 + y), new Vector3(0.5f, 0, 4 + y));
            TopoShape line6 = GlobalInstance.BrepTools.MakeLine(new Vector3(0.5f, 0, 4 + y), new Vector3(0.5f, 0, 0.5f + y));
            TopoShape line7 = GlobalInstance.BrepTools.MakeLine(new Vector3(0.5f, 0, 0.5f + y), new Vector3(0, 0, 0.5f + y));
            TopoShape line8 = GlobalInstance.BrepTools.MakeLine(new Vector3(0, 0, 0.5f + y), new Vector3(0, 0, 0 + y));
            TopoShapeGroup shapeGroup = new TopoShapeGroup();

            shapeGroup.Add(line2);
            shapeGroup.Add(line3);
            shapeGroup.Add(line4);
            shapeGroup.Add(line5);
            shapeGroup.Add(line6);
            shapeGroup.Add(line7);
            shapeGroup.Add(line8);
            shapeGroup.Add(line1);
            TopoShape profile = GlobalInstance.BrepTools.MakeWire(shapeGroup);

            TopoShape line9 = GlobalInstance.BrepTools.MakeLine(new Vector3(0, 0, 0 + y), new Vector3(0, -20, 0 + y));
            TopoShapeGroup lineGroup = new TopoShapeGroup();
            lineGroup.Add(line9);
            TopoShape wire = GlobalInstance.BrepTools.MakeWire(lineGroup);
            TopoShape sweep = GlobalInstance.BrepTools.Sweep(profile, wire, true);


            TopoShape line10 = GlobalInstance.BrepTools.MakeLine(new Vector3(1, 0, 0.5f + y), new Vector3(1, 0, 3 + y));
            TopoShape line11 = GlobalInstance.BrepTools.MakeLine(new Vector3(1, 0, 3 + y), new Vector3(2, 0, 3 + y));
            TopoShape line12 = GlobalInstance.BrepTools.MakeLine(new Vector3(2, 0, 3 + y), new Vector3(2, 0, 0.5f + y));
            TopoShape line13 = GlobalInstance.BrepTools.MakeLine(new Vector3(2, 0, 0.5f + y), new Vector3(1, 0, 0.5f + y));
            TopoShapeGroup shapeGroup1 = new TopoShapeGroup();
            shapeGroup1.Add(line10);
            shapeGroup1.Add(line11);
            shapeGroup1.Add(line12);
            shapeGroup1.Add(line13);
            TopoShape profile1 = GlobalInstance.BrepTools.MakeWire(shapeGroup1);
            TopoShape line14 = GlobalInstance.BrepTools.MakeLine(new Vector3(1, 0, 0.5f + y), new Vector3(1, -0.5f, 0.5f + y));
            TopoShapeGroup lineGroup1 = new TopoShapeGroup();
            lineGroup1.Add(line14);
            TopoShape wire1 = GlobalInstance.BrepTools.MakeWire(lineGroup1);

            TopoShape sweep1 = GlobalInstance.BrepTools.Sweep(profile1, wire1, true);

            TopoShape comp = GlobalInstance.BrepTools.BooleanAdd(sweep, sweep1);

            RepairTools rt = new RepairTools();
            comp = rt.RemoveExtraEdges(comp);

            context.ShowGeometry(comp);

            return true;
        }
    }
}
