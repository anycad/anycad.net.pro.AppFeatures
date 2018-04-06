using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;

namespace Features.Modeling.Algorithm
{
    class MakeCut : Feature
    {
        public MakeCut()
        {
            Name = "Cut";
            Group = ModelingModule.GroupId;
            Category = ModelingModule.BooleanCategoryId;
        }

        public override bool Run(FeatureContext context)
        {
            TopoShape tube = GlobalInstance.BrepTools.MakeTube(Vector3.ZERO, Vector3.UNIT_Z, 8, 2, 100);

            TopoShape cyl = GlobalInstance.BrepTools.MakeCylinder(new Vector3(0, 0, 50), Vector3.UNIT_X, 5, 10, 0);
            {
                SceneNode node = context.ShowGeometry(cyl);
                FaceStyle fs = new FaceStyle();
                fs.SetColor(new ColorValue(0.5f, 0.5f, 0, 0.5f));
                fs.SetTransparent(true);
                node.SetFaceStyle(fs);
            }

            TopoShape cut = GlobalInstance.BrepTools.BooleanCut(tube, cyl);
            context.ShowGeometry(cut);
            
            return true;
        }
    }
}
