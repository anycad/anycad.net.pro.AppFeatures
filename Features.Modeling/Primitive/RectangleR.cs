using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;
namespace Features.Modeling.Primitive
{
    class RectangleR : PrimitiveFeature
    {
        public RectangleR()
        {
            Name = "Rectangle";     
        }

        public override bool Run(FeatureContext context)
        {
            TopoShape rect = GlobalInstance.BrepTools.MakeRectangle(100, 50, 10, Coordinate3.UNIT_XYZ);
            rect = GlobalInstance.BrepTools.MakeFace(rect);

            context.ShowGeometry(rect);
            return true;
        }
    }
}
