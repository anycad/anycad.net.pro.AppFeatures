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
    class ImportStep : Feature
    {
        public ImportStep()
        {
            Name = "STEP";
            Group = RenderingModule.GroupId;
            Category = RenderingModule.ImportCategoryId;
        }

        public override bool Run(FeatureContext context)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Step File (*.stp;*.step)|*.stp;*.step||";
            if(DialogResult.OK != dlg.ShowDialog())
                return true;

            ShowShapeReaderContext renderContext = new ShowShapeReaderContext(context.RenderView.SceneManager);
            StepReader reader = new StepReader();
            reader.Read(dlg.FileName, renderContext);

            return true;
        }
    }
}
