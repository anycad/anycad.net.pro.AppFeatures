using AnyCAD.Platform;
using Features.Core;


namespace Features.Modeling.Primitive
{
    class RectTube : PrimitiveFeature
    {
        public RectTube()
        {
            Name = "Rectangel Tube";
        }
        public override bool Run(FeatureContext context)
        {
            TopoShape rect = GlobalInstance.BrepTools.MakeRectangle(100, 50, 10, Coordinate3.UNIT_XYZ);
            rect = GlobalInstance.BrepTools.MakeFace(rect);

            TopoShape rect2 = GlobalInstance.BrepTools.MakeRectangle(90, 40, 9, Coordinate3.UNIT_XYZ);
            rect2 = GlobalInstance.BrepTools.MakeFace(rect2);

            rect2 = GlobalInstance.BrepTools.Translate(rect2, new Vector3(5, 5, 0));

            var cut = GlobalInstance.BrepTools.BooleanCut(rect, rect2);

            var body = GlobalInstance.BrepTools.Extrude(cut, 100, Vector3.UNIT_Z);

            context.ShowGeometry(body);
            return true;
        }
    }
}
