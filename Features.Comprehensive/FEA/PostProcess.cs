using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;
using System.Windows.Forms;
using System.IO;

namespace Features.Comprehensive.Boolean
{
    class DeformationNode
    {
        public DeformationNode()
        {

        }

        public int Id;
        public float X;
        public float Y;
        public float Z;
        public float Data;
    }
    class PostProcess : Feature
    {
        public PostProcess()
        {
            Group = ComprehensiveModule.GroupId;
            Category = ComprehensiveModule.BooleanCategoryId;
            Name = "FEA.Postprocess";      
        }

        public override bool Run(FeatureContext context)
        {

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Deformation File (*.txt)|*.txt||";
            if (DialogResult.OK != dlg.ShowDialog())
                return true;


            String fileName = dlg.FileName;
            StreamReader sr = new StreamReader(fileName, Encoding.Default);
            String line = sr.ReadLine();

            List<DeformationNode> nodes = new List<DeformationNode>();
            float maxValue = float.NegativeInfinity;
            float minValue = float.PositiveInfinity;

            while ((line = sr.ReadLine()) != null)
            {
                String[] items = line.Split('\t');

                DeformationNode node = new DeformationNode();
                node.Id = int.Parse(items[0]);
                node.X = float.Parse(items[1]);
                node.Y = float.Parse(items[2]);
                node.Z = float.Parse(items[3]);
                node.Data = float.Parse(items[4]);

                nodes.Add(node);

                if(node.Data > maxValue)
                {
                    maxValue = node.Data;
                }
                if(node.Data < minValue)
                {
                    minValue = node.Data;
                }
            }

            float[] pointBuffer = new float[nodes.Count * 3];
            float[] colorBuffer = new float[nodes.Count * 3];
            float range = maxValue - minValue;

            List<ColorValue> colorTables = new List<ColorValue>();
            colorTables.Add(new ColorValue(0, 0, 1.0f));
            colorTables.Add(new ColorValue(0, 108.0f/255.0f, 1.0f));
            colorTables.Add(new ColorValue(0, 197.0f / 255.0f, 1.0f));
            colorTables.Add(new ColorValue(0, 243 / 255.0f, 1.0f));
            colorTables.Add(new ColorValue(0, 1.0f, 219.0f / 255.0f));
            colorTables.Add(new ColorValue(0, 1.0f, 165.0f / 255.0f));
            colorTables.Add(new ColorValue(0, 1.0f, 54.0f / 255.0f));
            colorTables.Add(new ColorValue(54.0f / 255.0f, 1.0f, 0));
            colorTables.Add(new ColorValue(219.0f / 255.0f, 1.0f, 0));
            //colorTables.Add(new ColorValue(238.0f / 255.0f, 249.0f/255.0f, 0));
            colorTables.Add(new ColorValue(238.0f / 255.0f, 249.0f / 255.0f, 0));
            colorTables.Add(new ColorValue(255.0f / 255.0f, 197.0f / 255.0f, 0));
            colorTables.Add(new ColorValue(251.0f / 255.0f, 102.0f / 255.0f, 17.0f / 255.0f));
            colorTables.Add(new ColorValue(1.0f, 0.0f, 0.0f));

            float segment = range / (colorTables.Count) ;

            int ii=-1;
            foreach(DeformationNode node in nodes)
            {
                int idx = (int)(node.Data / segment);

                if (idx >= colorTables.Count)
                    idx -= 1;

                ColorValue clr = colorTables.ElementAt(idx);

                pointBuffer[++ii] = node.X;
                colorBuffer[ii] = clr.R;
                pointBuffer[++ii] = node.Y;
                colorBuffer[ii] = clr.G;
                pointBuffer[++ii] = node.Z;
                colorBuffer[ii] = clr.B;
            }

            PointCloudNode pcn = new PointCloudNode();
            pcn.SetPoints(pointBuffer);
            pcn.SetColors(colorBuffer);
            pcn.ComputeBBox();

          

            context.ShowSceneNode(pcn);
            return true;
        }
    }
}
