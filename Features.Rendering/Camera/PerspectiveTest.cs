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

    class PerspectiveTest : Feature
    {
        public PerspectiveTest()
        {
            Name = "Perspective";
            Group = RenderingModule.GroupId;
            Category = RenderingModule.CameraCategoryId;
        }

        public override bool Run(FeatureContext context)
        {

            context.RenderView.ExecuteCommand("ProjectionMode");

            return true;
        }
    }
}
