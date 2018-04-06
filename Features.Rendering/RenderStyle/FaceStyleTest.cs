using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;
using System.Windows.Forms;
using AnyCAD.Exchange;

namespace Features.Rendering.Exchange
{
    class FaceStyleTest : Feature
    {
        public FaceStyleTest()
        {
            Name = "FaceStyle";
            Group = RenderingModule.GroupId;
            Category = RenderingModule.StyleCategoryId;
        }

        public override bool Run(FeatureContext context)
        {
            TopoShape box = GlobalInstance.BrepTools.MakeBox(Vector3.ZERO, Vector3.UNIT_Z, new Vector3(100, 2, 200));       
            {
                SceneNode node = context.ShowGeometry(box);
                FaceStyle fs = new FaceStyle();
                fs.SetColor(new ColorValue(0.5f, 0.5f, 1.0f, 0.5f));
                fs.SetTransparent(true);
                node.SetFaceStyle(fs);
            }
            context.RequestDraw();

            
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Texture File (*.jpg;*.png)|*.jpg;*png||";
            if (DialogResult.OK == dlg.ShowDialog())
            {
                SceneNode node2 = context.ShowGeometry(box);
                node2.SetTransform(GlobalInstance.MatrixBuilder.MakeTranslate(new Vector3(0, 50, 0)));
                FaceStyle fs2 = new FaceStyle();
                Texture tex = new Texture();
                tex.SetName(dlg.SafeFileName);
                tex.SetFilePath(new Path(dlg.FileName));

                fs2.SetTexture(0, tex);

                node2.SetFaceStyle(fs2);
            }

            return true;
        }
    }
}
