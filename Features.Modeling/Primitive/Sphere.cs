using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;

namespace Features.Modeling.Primitive
{
    class Sphere : PrimitiveFeature
    {
        public Sphere()
        {
            Name = "Sphere";      
        }

        public override bool Run(FeatureContext context)
        {
            TopoShape body = GlobalInstance.BrepTools.MakeSphere(Vector3.ZERO, 10);

            context.ShowGeometry(body);
            return true;
        }
    }
}
