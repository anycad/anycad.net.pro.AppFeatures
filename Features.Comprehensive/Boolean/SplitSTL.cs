using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;
using System.Windows.Forms;

namespace Features.Comprehensive.Boolean
{
    class SplitSTL : Feature
    {
        public SplitSTL()
        {
            Group = ComprehensiveModule.GroupId;
            Category = ComprehensiveModule.BooleanCategoryId;
            Name = "Splt.STL";      
        }

        public override bool Run(FeatureContext context)
        {

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "STL (*.stl)|*.stl|IGES (*.igs;*.iges)|*.igs;*.iges|STEP (*.stp;*.step)|*.stp;*.step|BREP (*.brep)|*.brep|All Files(*.*)|*.*";

            if (DialogResult.OK != dlg.ShowDialog())
                return false;

            context.RenderView.RenderTimer.Enabled = false;
             TopoShape shape = GlobalInstance.BrepTools.LoadFile(new AnyCAD.Platform.Path(dlg.FileName));
             context.RenderView.RenderTimer.Enabled = true;
             MessageBox.Show("loaded");
            if (shape != null)
            {
                //context.ShowGeometry(shape, new ElementId(100));
                //GlobalInstance.BrepTools.SaveFile(shape, new Path(dlg.FileName + ".brep"));

                AABox bbox = shape.GetBBox();
                Vector3 size = bbox.Size();
                Vector3 start = new Vector3(bbox.MinPt.X - 10, bbox.MinPt.Y + size.Y * 0.5f, bbox.MinPt.Z - 10);
                TopoShape boxShape = GlobalInstance.BrepTools.MakeBox(start, Vector3.UNIT_Z, new Vector3(size.X + 20, size.Y * 0.25f, size.Z + 20));

                shape = GlobalInstance.BrepTools.BooleanCut(shape, boxShape);

                MessageBox.Show("cut!");
                var groups = GlobalInstance.TopoExplor.ExplorSubShapes(shape);
                for (int ii = 0, len = groups.Size(); ii < len; ++ii)
                {
                    shape = groups.GetAt(ii);
                    var node = context.ShowGeometry(shape, new ElementId(100));
                    var fs = new FaceStyle();
                    fs.SetColor(ii * 100, 0, ii + 200);
                    node.SetFaceStyle(fs);
                }
            }
            return true;
        }
    }
}
