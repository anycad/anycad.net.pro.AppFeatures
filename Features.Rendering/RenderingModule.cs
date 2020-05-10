using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using Features.Rendering.NodeObject;
using Features.Rendering.Animation;
using Features.Rendering.Exchange;

namespace Features.Rendering
{
    public class RenderingModule
    {
        public static String GroupId = "2. Rendering";
        static public String NodeCategoryId = "2.1 Node";
        static public String SceneCategoryId = "2.2 Scene";
        static public String AnimationCategoryId = "2.3 Animation";
        static public String StyleCategoryId = "2.4 Style";
        static public String CameraCategoryId = "2.5 Camera";
        static public String ImportCategoryId = "2.6 Import";

        public static void Initialize()
        {
            FeatureManager.Instance().Add(new ArrowNode());
            FeatureManager.Instance().Add(new AxesNodeObject());
            FeatureManager.Instance().Add(new ColoredFaceObject());
            FeatureManager.Instance().Add(new Text3dObject());
            FeatureManager.Instance().Add(new PointCloudObject());
            FeatureManager.Instance().Add(new PointMarker());
            FeatureManager.Instance().Add(new ShapeCopies());

            FeatureManager.Instance().Add(new RemoveNode());
            FeatureManager.Instance().Add(new ListNodes());
            FeatureManager.Instance().Add(new QuerySelectionTest());

            FeatureManager.Instance().Add(new DancingBall());

            FeatureManager.Instance().Add(new FaceStyleTest());
            FeatureManager.Instance().Add(new LineStyleTest());

            FeatureManager.Instance().Add(new StdTopCameraTest());
            FeatureManager.Instance().Add(new PerspectiveTest());

            FeatureManager.Instance().Add(new ImportStep());
            FeatureManager.Instance().Add(new ImportDxf());
            FeatureManager.Instance().Add(new ImportIges());
            FeatureManager.Instance().Add(new ImportStl());

        }
    }
}
