using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using Features.Modeling.Primitive;
using Features.Modeling.Algorithm;
using Features.Modeling.Geometry;

namespace Features.Modeling
{
    public class ModelingModule
    {
        public static String GroupId = "1. Modeling";
        static public String PrimitiveCategoryId = "1.1 Primitive";
        static public String BooleanCategoryId = "1.2 Boolean";
        static public String FeaturedCategoryId = "1.3 Feature";
        static public String AlgorithmCategoryId = "1.4 Algorithm";
        static public String GeometryCategoryId = "1.6 Geometry";

        public static void Initialize()
        {
            FeatureManager.Instance().Add(new Spiral());
            FeatureManager.Instance().Add(new RectangleR());
            FeatureManager.Instance().Add(new Sphere());
            FeatureManager.Instance().Add(new Box());
            FeatureManager.Instance().Add(new SurfaceFromPoints());
            FeatureManager.Instance().Add(new RectTube());

            FeatureManager.Instance().Add(new MakeCut());
            //FeatureManager.Instance().Add(new MakeGlue());
            FeatureManager.Instance().Add(new MakeSection());
            FeatureManager.Instance().Add(new MakeSplitCurve());

            FeatureManager.Instance().Add(new MakeHoles());
            FeatureManager.Instance().Add(new MakeFillet());
            FeatureManager.Instance().Add(new MakeRevole());
            FeatureManager.Instance().Add(new MakeSweep());
            FeatureManager.Instance().Add(new MakePipe());
            FeatureManager.Instance().Add(new MakePipe2());
            FeatureManager.Instance().Add(new MakeLoft());
            FeatureManager.Instance().Add(new MakeEvolved());

            FeatureManager.Instance().Add(new MakeSplit());
            FeatureManager.Instance().Add(new SurfaceSection());
            FeatureManager.Instance().Add(new ProjectionPlane());
            FeatureManager.Instance().Add(new CurveLineIntersection());

            FeatureManager.Instance().Add(new QuertyCurve());
            FeatureManager.Instance().Add(new QuertySurface());
            FeatureManager.Instance().Add(new CurveLength());
            FeatureManager.Instance().Add(new SurfaceArea());
            FeatureManager.Instance().Add(new SolidVolumn());
        }
    }
}
