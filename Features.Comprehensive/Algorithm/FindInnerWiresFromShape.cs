using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;
using System.Windows.Forms;
using AnyCAD.Exchange;

namespace Features.Comprehensive.Algorithm
{
    class FindWireContext : AnyCAD.Platform.TopoShapeReaderContext
    {
        private LineStyle _LineStyle = new LineStyle();
        private FaceStyle _FaceStyle = new FaceStyle();

        private FeatureContext _Context;
        public FindWireContext(FeatureContext context)
        {
            _Context = context;
            _LineStyle.SetLineWidth(2);
            _LineStyle.SetColor(255, 0, 0);

            _FaceStyle.SetColor(new ColorValue(0.5f, 0.5f, 0.5f, 0.5f));
            _FaceStyle.SetTransparent(true);
        }

        public override void OnFace(TopoShape shape)
        {
            WireClassifier wc = new WireClassifier();
            if (!wc.Initialize(shape))
                return;

            TopoShapeGroup innerWires =  wc.GetInnerWires();
            int nCount = innerWires.Size();
            for (int ii = 0; ii < nCount; ++ii)
            {
               SceneNode node  = _Context.ShowGeometry(innerWires.GetAt(ii));
               node.SetLineStyle(_LineStyle);
            }

            SceneNode faceNode = _Context.ShowGeometry(shape);
            faceNode.SetFaceStyle(_FaceStyle);
        }

        public override void OnBeginGroup(string name)
        {

        }
        public override void OnEndGroup()
        {

        }
    }

    class FindInnerWiresFromShape : Feature
    {
        public FindInnerWiresFromShape()
        {
            Name = "Find InnerWires";
            Group = ComprehensiveModule.GroupId;
            Category = ComprehensiveModule.AlgorithmCategoryId;
        }

        public override bool Run(FeatureContext context)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Shape File (*.igs;*.iges)|*.iges;*.igs||";
            if (DialogResult.OK != dlg.ShowDialog())
                return true;

            FindWireContext renderContext = new FindWireContext(context);
            IgesReader reader = new IgesReader();
            reader.Read(dlg.FileName, renderContext);

            return true;
        }
    }
}
