using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;
using System.Windows.Forms;

namespace Features.Rendering.NodeObject
{
    class RemoveNode : Feature
    {
        public RemoveNode()
        {
            Name = "Remove Node";
            Group = RenderingModule.GroupId;
            Category = RenderingModule.SceneCategoryId;
        }

        public override bool Run(FeatureContext context)
        {
            {
                ArrowWidget arrow = new ArrowWidget();
                context.ShowSceneNode(arrow);
                context.RenderView.FitAll();
                context.RequestDraw();
            }


            ElementId id = context.CurrentId;
            MessageBox.Show("Remove Node");

            SceneManager sceneMgr = context.RenderView.SceneManager;
            SceneNode node = sceneMgr.FindNode(id);
            if (node != null)
            {
                sceneMgr.RemoveNode(node);
            }

            return true;
        }
    }
}
