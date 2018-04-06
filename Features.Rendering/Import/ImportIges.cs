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
    class ImportIges : Feature
    {
        public ImportIges()
        {
            Name = "IGES";
            Group = RenderingModule.GroupId;
            Category = RenderingModule.ImportCategoryId;
        }

        public override bool Run(FeatureContext context)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "IGES File (*.igs;*.iges)|*.igs;*.iges||";
            if(DialogResult.OK != dlg.ShowDialog())
                return true;

            ShowShapeReaderContext renderContext = new ShowShapeReaderContext(context.RenderView.SceneManager);
            IgesReader reader = new IgesReader();
            reader.Read(dlg.FileName, renderContext);

            return true;
        }
    }
}
