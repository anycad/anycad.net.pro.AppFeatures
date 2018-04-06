using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;
using System.Windows.Forms;

namespace Features.Modeling.Geometry
{
    class SolidVolumn : Feature
    {
        public SolidVolumn()
        {
            Name = "Solid.Volumn";
            Group = ModelingModule.GroupId;
            Category = ModelingModule.GeometryCategoryId;
        }

        public override bool Run(FeatureContext context)
        {
            TopoShape box = GlobalInstance.BrepTools.MakeBox(Vector3.ZERO, Vector3.UNIT_Z, new Vector3(100, 100, 100));
            context.ShowGeometry(box);


            TopoShapeProperty property = new TopoShapeProperty();
            property.SetShape(box);

            MessageBox.Show(String.Format("Area: {0}", property.SolidVolume()));

            return true;
        }
    }
}
