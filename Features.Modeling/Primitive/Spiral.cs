using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;
namespace Features.Modeling.Primitive
{
    class Spiral : PrimitiveFeature
    {
        public Spiral()
        {
            Name = "Spiral";     
        }

        public override bool Run(FeatureContext context)
        {
            TopoShape spiralCurve = GlobalInstance.BrepTools.MakeSpiralCurve(100, 10, 10, Coordinate3.UNIT_XYZ);

            context.ShowGeometry(spiralCurve);
            return true;
        }
    }
}
