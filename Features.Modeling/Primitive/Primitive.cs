using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;

namespace Features.Modeling.Primitive
{
    class PrimitiveFeature : Feature
    {
        public PrimitiveFeature()
        {
            Group = ModelingModule.GroupId;
            Category = ModelingModule.PrimitiveCategoryId;
        }
    }
}
