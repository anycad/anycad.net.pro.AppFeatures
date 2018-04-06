using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;
using System.Windows.Forms;

namespace Features.Rendering.NodeObject
{


    class ArrowNode : Feature
    {
        public ArrowNode()
        {
            Name = "Arrow.FixedSize";
            Group = RenderingModule.GroupId;
            Category = RenderingModule.NodeCategoryId;
        }

        public override bool Run(FeatureContext context)
        {

            ArrowWidget arrow = new ArrowWidget();
            arrow.SetFixedSize(true);

            context.ShowSceneNode(arrow);


            return true;
        }
    }
}
