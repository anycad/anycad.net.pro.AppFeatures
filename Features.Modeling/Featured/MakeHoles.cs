using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;

namespace Features.Modeling.Algorithm
{
    class MakeHoles : Feature
    {
        public MakeHoles()
        {
            Name = "Holes.Loops";
            Group = ModelingModule.GroupId;
            Category = ModelingModule.FeaturedCategoryId;
        }

        public override bool Run(FeatureContext context)
        {
            var ptlist = new System.Collections.Generic.List<Vector3>();
            ptlist.Add(Vector3.ZERO);
            ptlist.Add(new Vector3(100, 0, 0));
            ptlist.Add(new Vector3(100, 100, 0));
            ptlist.Add(new Vector3(0, 100, 0));
            TopoShape polygon = GlobalInstance.BrepTools.MakePolygon(ptlist);

            TopoShape cir1 = GlobalInstance.BrepTools.MakeCircle(new Vector3(20, 20, 0), 15, Vector3.UNIT_Z);
            TopoShape cir2 = GlobalInstance.BrepTools.MakeCircle(new Vector3(80, 20, 0), 15, Vector3.UNIT_Z);
            TopoShape cir3 = GlobalInstance.BrepTools.MakeCircle(new Vector3(80, 80, 0), 15, Vector3.UNIT_Z);
            TopoShape cir4 = GlobalInstance.BrepTools.MakeCircle(new Vector3(20, 80, 0), 15, Vector3.UNIT_Z);

            TopoShapeGroup group = new TopoShapeGroup();
            group.Add(polygon);
            group.Add(cir1);
            group.Add(cir2);
            group.Add(cir3);
            group.Add(cir4);

            // build faces with holes
            LoopsBuilder lb = new LoopsBuilder();
            lb.Initialize(group);
            TopoShapeGroup faces = lb.BuildFacesWithHoles();
            for (int ii = 0; ii < faces.Size(); ++ii)
            {
                context.ShowGeometry(faces.GetTopoShape(ii));
            }

            return true;
        }
    }
}
