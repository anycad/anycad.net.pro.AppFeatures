using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;
using System.Windows.Forms;

namespace Features.Rendering.NodeObject
{
    class ListNodes : Feature
    {
        public ListNodes()
        {
            Name = "List Nodes";
            Group = RenderingModule.GroupId;
            Category = RenderingModule.SceneCategoryId;
        }

        public override bool Run(FeatureContext context)
        {
            TopoShape box = GlobalInstance.BrepTools.MakeBox(Vector3.ZERO, Vector3.UNIT_Z, new Vector3(10, 10, 10));
            RenderableEntity entity = GlobalInstance.TopoShapeConvert.ToEntity(box, 0);
            for (int ii = 0; ii < 10; ++ii)
            {
                EntitySceneNode node = new EntitySceneNode();
                node.SetEntity(entity);
                Matrix4 trf = GlobalInstance.MatrixBuilder.MakeTranslate(new Vector3(11 * ii, 0, 0));
                node.SetTransform(trf);

                context.ShowSceneNode(node);
            }
            context.RequestDraw();

            SceneNodeIterator itr = context.RenderView.SceneManager.NewSceneNodeIterator();
            String msg = "Node Ids: ";
            while (itr.More())
            {
                SceneNode node = itr.Next();
                msg += String.Format(" {0}", node.GetId().AsInt());
            }

            MessageBox.Show(msg);

            return true;
        }
    }
}
