using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;
using System.Windows.Forms;

namespace Features.Rendering.NodeObject
{
    class QuerySelectionTest : Feature
    {
        public QuerySelectionTest()
        {
            Name = "Query Selection";
            Group = RenderingModule.GroupId;
            Category = RenderingModule.SceneCategoryId;
        }

        public override bool Run(FeatureContext context)
        {
            {
                TopoShape cone = GlobalInstance.BrepTools.MakeDish(100, 30, Vector3.ZERO);
                SceneNode node = context.ShowGeometry(cone);
                node.SetName("My Cone");
                context.RenderView.SceneManager.SelectNode(node);

                context.RequestDraw();
            }

            SelectedEntityQuery query = new SelectedEntityQuery();
            context.RenderView.QuerySelection(query);
            SceneNode node2 = query.GetRootNode();
            if (node2 != null)
            {
                MessageBox.Show(String.Format("Selected Node: {0}", node2.GetName()));
            }


            return true;
        }
    }
}
