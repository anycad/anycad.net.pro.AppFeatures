using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AnyCAD.Presentation;
using Features.Core;
using AnyCAD.Platform;


namespace AnyFeatures
{
    public partial class MainForm : Form
    {
        private RenderWindow3d m_RenderView = null;
        private Feature m_CurrentFeature = null;

        public AnyCAD.Presentation.RenderWindow3d RenderView
        {
            get { return m_RenderView; }
            set { m_RenderView = value; }
        }

        public MainForm()
        {
            InitializeComponent();

            // Add 3D RenderView to container.
            var container = this.splitContainer1.Panel2;

            m_RenderView = new RenderWindow3d();

            m_RenderView.Size = container.ClientSize;
            m_RenderView.Dock = System.Windows.Forms.DockStyle.Fill;
            container.Controls.Add(m_RenderView);

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Features.Modeling.ModelingModule.Initialize();
            Features.Rendering.RenderingModule.Initialize();
            Features.Comprehensive.ComprehensiveModule.Initialize();
            

            Dictionary<String, TreeNode> groupNodes = new Dictionary<String, TreeNode>();
            Dictionary<String, TreeNode> categoryNodes = new Dictionary<String, TreeNode>();

            foreach (Feature fe in FeatureManager.Instance().FeatrueList)
            {
                TreeNode group = null;
                if (!groupNodes.TryGetValue(fe.Group, out group))
                {
                    group = this.treeView1.Nodes.Add(fe.Group);
                    groupNodes.Add(fe.Group, group);
                }

                TreeNode category = null;
                if (!categoryNodes.TryGetValue(fe.Category, out category))
                {
                    category = group.Nodes.Add(fe.Category);
                    categoryNodes.Add(fe.Category, category);
                }

                TreeNode feNode = category.Nodes.Add(fe.Name);
                feNode.Tag = fe;
            }
            this.treeView1.ExpandAll();
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            TreeNode node = this.treeView1.SelectedNode;
            if (node == null || node.Tag == null)
                return;

            FeatureContext context = new FeatureContext(RenderView);
            if (m_CurrentFeature != null)
                m_CurrentFeature.OnExit(context);
            m_RenderView.ClearScene();
            m_CurrentFeature = node.Tag as Feature;
            if(m_CurrentFeature.Run(context))
                m_RenderView.FitAll();
            m_RenderView.RequestDraw();

            this.toolStripStatusLabel1.Text = m_CurrentFeature.Name;
        }

        private void queryMultiSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TopoShapeGroup group = new TopoShapeGroup();

            MultiShapeQuery query = new MultiShapeQuery();
            m_RenderView.QuerySelection(query);
            int nCount = query.GetCount();
            for (int ii = 0; ii < nCount; ++ii)
            {
                SelectedShapeQuery shapeQuery = query.GetSubContext(ii);
                TopoShape subShape = shapeQuery.GetSubGeometry();
                if (subShape != null)
                {
                    group.Add(subShape);
                }
            }

            // clear the scene and only keep the selected shapes
            if (group.Size() > 0)
            {
                m_RenderView.ClearScene();
                for (int ii = 0; ii < group.Size(); ++ii)
                {
                    m_RenderView.ShowGeometry(group.GetAt(ii), 100+ii); ;
                }
            }
        }

        private void pickByRectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_RenderView.ExecuteCommand("RectPick", "");
        }

        private void pickByClickToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_RenderView.ExecuteCommand("Pick", "");
        }
    }
}
