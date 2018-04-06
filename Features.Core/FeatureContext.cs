using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnyCAD.Presentation;
using AnyCAD.Platform;

namespace Features.Core
{
    public class FeatureContext
    {
        public RenderWindow3d RenderView { get; set; }

        public ElementId CurrentId { get; set; }

        public FeatureContext(RenderWindow3d renderView)
        {
            RenderView = renderView;
            CurrentId = new ElementId(100);
        }
        public SceneNode ShowGeometry(TopoShape shape)
        {
            ++CurrentId;
            return RenderView.ShowGeometry(shape, CurrentId);
        }
        public void ShowSceneNode(SceneNode node)
        {
            ++CurrentId;
            node.SetId(CurrentId);
            RenderView.ShowSceneNode(node);
        }

        public SceneNode ShowGeometry(TopoShape shape, ElementId id)
        {
            CurrentId = id;
            return RenderView.ShowGeometry(shape, id);
        }

        public void RequestDraw()
        {
            RenderView.RequestDraw();
        }
    }
}
