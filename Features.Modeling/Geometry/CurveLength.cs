using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;
using System.Windows.Forms;

namespace Features.Modeling.Geometry
{
    class CurveLength : Feature
    {
        public CurveLength()
        {
            Name = "Curve.Length";
            Group = ModelingModule.GroupId;
            Category = ModelingModule.GeometryCategoryId;
        }

        public override bool Run(FeatureContext context)
        {
            TopoShape arc = GlobalInstance.BrepTools.MakeEllipseArc(Vector3.ZERO, 100, 50, 45, 270, Vector3.UNIT_Z);
            context.ShowGeometry(arc);


            TopoShapeProperty property = new TopoShapeProperty();
            property.SetShape(arc);

            double length = property.EdgeLength();

            MessageBox.Show(String.Format("Length: {0}",length));

            return true;
        }
    }
}
