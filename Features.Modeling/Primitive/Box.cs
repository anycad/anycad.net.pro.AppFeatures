using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;
namespace Features.Modeling.Primitive
{
    class Box : PrimitiveFeature
    {
        public Box()
        {
            Name = "Box";     
        }

        public override bool Run(FeatureContext context)
        {
            TopoShape body = GlobalInstance.BrepTools.MakeBox(Vector3.ZERO, Vector3.UNIT_Z, new Vector3(10,20,30));

            context.ShowGeometry(body);
            return true;
        }
    }
}
