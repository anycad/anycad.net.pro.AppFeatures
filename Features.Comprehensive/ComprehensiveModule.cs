using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using Features.Comprehensive.Boolean;
using Features.Comprehensive.Algorithm;

namespace Features.Comprehensive
{
    public class ComprehensiveModule
    {
        public static String GroupId = "5. Comprehensive";
        static public String BooleanCategoryId = "Boolean";
        static public String AlgorithmCategoryId = "Algorithm";

        public static void Initialize()
        {
            FeatureManager.Instance().Add(new SplitSTL());
            FeatureManager.Instance().Add(new ExtrudeBoolean());
            FeatureManager.Instance().Add(new SurfaceSection());
            FeatureManager.Instance().Add(new RemoveExtraEdges());
            FeatureManager.Instance().Add(new FindInnerWiresFromShape());
            FeatureManager.Instance().Add(new PostProcess());
            FeatureManager.Instance().Add(new PostProcessHex20());
        }
    }
}
