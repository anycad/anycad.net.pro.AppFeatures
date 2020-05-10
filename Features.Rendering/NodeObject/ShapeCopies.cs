using AnyCAD.Platform;
using Features.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Features.Rendering.NodeObject
{
    class ShapeCopies : Feature
    {
        public ShapeCopies()
        {
            Name = "Copy.Shapes";
            Group = RenderingModule.GroupId;
            Category = RenderingModule.NodeCategoryId;
        }

        public override bool Run(FeatureContext context)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "IGES File (*.igs;*.iges)|*.igs;*.iges||";
            if (DialogResult.OK != dlg.ShowDialog())
                return true;

            var shape = GlobalInstance.BrepTools.LoadFile(new Path(dlg.FileName));

            var size = shape.GetBBox().Size();

            var entity = GlobalInstance.TopoShapeConvert.ToEntity(shape, 1);

            for(int ii=0; ii<7; ++ii)
            {
                for(int jj=0; jj<7; ++jj)
                {
                    var node = new EntitySceneNode();
                    node.SetEntity(entity);

                    var trf = GlobalInstance.MatrixBuilder.MakeTranslate(new Vector3(size.X* ii, size.Y * jj, 0));
                    node.SetTransform(trf);

                    context.ShowSceneNode(node);
                }

            }

            return true;
        }
    }
}
