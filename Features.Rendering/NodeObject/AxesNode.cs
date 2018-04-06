using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;

namespace Features.Rendering.NodeObject
{
    class AxesNodeObject : Feature
    {
        public AxesNodeObject()
        {
            Name = "Axes";
            Group = RenderingModule.GroupId;
            Category = RenderingModule.NodeCategoryId;
        }

        public override bool Run(FeatureContext context)
        {

            AxesWidget node = new AxesWidget();
            node.SetArrowText((int)EnumAxesDirection.Axes_X, "");
            node.SetArrowText((int)EnumAxesDirection.Axes_Y, "");
            node.SetArrowText((int)EnumAxesDirection.Axes_Z, "");

            context.ShowSceneNode(node);


            return true;
        }
    }
}
