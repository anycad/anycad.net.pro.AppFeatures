using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;

namespace Features.Rendering.NodeObject
{
    class Text3dObject : Feature
    {
        public Text3dObject()
        {
            Name = "Text.3D";
            Group = RenderingModule.GroupId;
            Category = RenderingModule.NodeCategoryId;
        }

        public override bool Run(FeatureContext context)
        {

            Text3dNode textNode = new Text3dNode();
            textNode.SetFontName("FangSong (TrueType)");
            textNode.SetText("1234565\nabcdefg\n我爱CAD");
            textNode.SetLineSpace(10);

            Coordinate3 coord = new Coordinate3();
            coord.Origion = new Vector3(100, 100, 0);
            coord.X = new Vector3(1, 1, 0);
            coord.X.Normalize();
            coord.Y = coord.Z.CrossProduct(coord.X);

            Matrix4 trf = GlobalInstance.MatrixBuilder.ToWorldMatrix(coord);
            textNode.SetTransform(trf);
            textNode.Update();

            context.ShowSceneNode(textNode);


            return true;
        }
    }
}
