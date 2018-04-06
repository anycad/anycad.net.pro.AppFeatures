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
    class ImportDxf : Feature
    {
        public ImportDxf()
        {
            Name = "DXF";
            Group = RenderingModule.GroupId;
            Category = RenderingModule.ImportCategoryId;
        }

        public override bool Run(FeatureContext context)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "DXF File (*.dxf)|*.dxf||";
            if(DialogResult.OK != dlg.ShowDialog())
                return true;

            ShowShapeReaderContext renderContext = new ShowShapeReaderContext(context.RenderView.SceneManager);
            DxfReader reader = new DxfReader();
            reader.Read(dlg.FileName, renderContext, false);

            return true;
        }
    }
}
