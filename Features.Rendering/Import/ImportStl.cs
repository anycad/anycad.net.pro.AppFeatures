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
    class ImportStl : Feature
    {
        public ImportStl()
        {
            Name = "STL";
            Group = RenderingModule.GroupId;
            Category = RenderingModule.ImportCategoryId;
        }

        public override bool Run(FeatureContext context)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "STL File (*.stl)|*.stl||";
            if(DialogResult.OK != dlg.ShowDialog())
                return true;

            ModelReader reader = new ModelReader();
            GroupSceneNode node = reader.LoadFile(new Path(dlg.FileName));
            context.ShowSceneNode(node);

            return true;
        }
    }
}
