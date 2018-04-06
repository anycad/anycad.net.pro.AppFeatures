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
    class HexNode
    {
        public HexNode()
        {
            Value = new Vector3();
        }

        public uint Id;
        public Vector3 Value;
        public float Data;
    }

    class Hex20
    {
        public Hex20()
        {
            NodeIds = new uint[20];
        }

        public uint Id;
        public uint[] NodeIds;
    }

    class PostProcessHex20 : Feature
    {
        public PostProcessHex20()
        {
            Group = ComprehensiveModule.GroupId;
            Category = ComprehensiveModule.BooleanCategoryId;
            Name = "FEA.Hex20";      
        }

        public override bool Run(FeatureContext context)
        {

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Deformation File (*.xls)|*.xls||";
            if (DialogResult.OK != dlg.ShowDialog())
                return true;


            String fileName = dlg.FileName;
            StreamReader sr = new StreamReader(fileName, Encoding.Default);
            String line = sr.ReadLine();

            Dictionary<uint, HexNode> nodes = new Dictionary<uint, HexNode>();
            float maxValue = float.NegativeInfinity;
            float minValue = float.PositiveInfinity;

            while ((line = sr.ReadLine()) != null)
            {
                String[] items = line.Split('\t');

                HexNode node = new HexNode();
                node.Id = uint.Parse(items[0]) - 1;
                node.Value.X = float.Parse(items[1]);
                node.Value.Y = float.Parse(items[2]);
                node.Value.Z = float.Parse(items[3]);
                node.Data = float.Parse(items[4]);

                nodes[node.Id] = node;

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

            //int ii=-1;
            //foreach(HexNode node in nodes)
            //{
            //    int idx = (int)(node.Data / segment);

            //    if (idx >= colorTables.Count)
            //        idx -= 1;

            //    ColorValue clr = colorTables.ElementAt(idx);

            //    pointBuffer[++ii] = node.X;
            //    colorBuffer[ii] = clr.R;
            //    pointBuffer[++ii] = node.Y;
            //    colorBuffer[ii] = clr.G;
            //    pointBuffer[++ii] = node.Z;
            //    colorBuffer[ii] = clr.B;
            //}
            dlg = new OpenFileDialog();
            dlg.Filter = "Hex20 File (*.xls)|*.xls||";
            if (DialogResult.OK != dlg.ShowDialog())
                return true;


            fileName = dlg.FileName;
            sr = new StreamReader(fileName, Encoding.Default);
            line = sr.ReadLine();

            List<Hex20> hex20s = new List<Hex20>();
            while ((line = sr.ReadLine()) != null)
            {
                String[] items = line.Split('\t');

                Hex20 hex = new Hex20();
                hex.Id = uint.Parse(items[0]) - 1;

                for(int ii=0; ii<20; ++ii)
                {
                    hex.NodeIds[ii] = uint.Parse(items[ii + 2]) - 1;
                }
                
                hex20s.Add(hex);
            }

            //Hex20 hexItem = hex20s[0];
            //for(int ii=0; ii<20; ++ii)
            //{

            //    HexNode n = nodes[hexItem.NodeIds[ii]];
            //    PointNode node = new PointNode();
            //    node.SetName(n.Id.ToString());
            //    node.SetShowText(true);
            //    node.SetPoint(new Vector3(n.X, n.Y, n.Z));

            //    context.ShowSceneNode(node);
            //}
            //GlobalInstance.TopoShapeConvert.
            List<uint> ib = new List<uint>();
            for (int ii = 0; ii < hex20s.Count; ++ii )
            {
                Hex20 hexItem = hex20s[ii];


                // TOP
                ib.Add(hexItem.NodeIds[0]);
                ib.Add(hexItem.NodeIds[8]);
                ib.Add(hexItem.NodeIds[11]);

                ib.Add(hexItem.NodeIds[8]);
                ib.Add(hexItem.NodeIds[1]);
                ib.Add(hexItem.NodeIds[9]);

                ib.Add(hexItem.NodeIds[9]);
                ib.Add(hexItem.NodeIds[2]);
                ib.Add(hexItem.NodeIds[10]);

                ib.Add(hexItem.NodeIds[10]);
                ib.Add(hexItem.NodeIds[3]);
                ib.Add(hexItem.NodeIds[11]);

                ib.Add(hexItem.NodeIds[8]);
                ib.Add(hexItem.NodeIds[10]);
                ib.Add(hexItem.NodeIds[11]);

                ib.Add(hexItem.NodeIds[8]);
                ib.Add(hexItem.NodeIds[9]);
                ib.Add(hexItem.NodeIds[10]);


                // BOTTOM
                ib.Add(hexItem.NodeIds[4]);
                ib.Add(hexItem.NodeIds[12]);
                ib.Add(hexItem.NodeIds[15]);

                ib.Add(hexItem.NodeIds[12]);
                ib.Add(hexItem.NodeIds[5]);
                ib.Add(hexItem.NodeIds[13]);

                ib.Add(hexItem.NodeIds[13]);
                ib.Add(hexItem.NodeIds[6]);
                ib.Add(hexItem.NodeIds[14]);

                ib.Add(hexItem.NodeIds[14]);
                ib.Add(hexItem.NodeIds[7]);
                ib.Add(hexItem.NodeIds[15]);

                ib.Add(hexItem.NodeIds[12]);
                ib.Add(hexItem.NodeIds[14]);
                ib.Add(hexItem.NodeIds[15]);

                ib.Add(hexItem.NodeIds[12]);
                ib.Add(hexItem.NodeIds[13]);
                ib.Add(hexItem.NodeIds[14]);

                // FRONT
                ib.Add(hexItem.NodeIds[1]);
                ib.Add(hexItem.NodeIds[9]);
                ib.Add(hexItem.NodeIds[17]);

                ib.Add(hexItem.NodeIds[9]);
                ib.Add(hexItem.NodeIds[2]);
                ib.Add(hexItem.NodeIds[18]);

                ib.Add(hexItem.NodeIds[18]);
                ib.Add(hexItem.NodeIds[6]);
                ib.Add(hexItem.NodeIds[13]);

                ib.Add(hexItem.NodeIds[13]);
                ib.Add(hexItem.NodeIds[5]);
                ib.Add(hexItem.NodeIds[17]);

                ib.Add(hexItem.NodeIds[9]);
                ib.Add(hexItem.NodeIds[18]);
                ib.Add(hexItem.NodeIds[17]);

                ib.Add(hexItem.NodeIds[18]);
                ib.Add(hexItem.NodeIds[13]);
                ib.Add(hexItem.NodeIds[17]);

                // BACK
                ib.Add(hexItem.NodeIds[0]);
                ib.Add(hexItem.NodeIds[11]);
                ib.Add(hexItem.NodeIds[16]);

                ib.Add(hexItem.NodeIds[11]);
                ib.Add(hexItem.NodeIds[3]);
                ib.Add(hexItem.NodeIds[19]);

                ib.Add(hexItem.NodeIds[19]);
                ib.Add(hexItem.NodeIds[7]);
                ib.Add(hexItem.NodeIds[15]);

                ib.Add(hexItem.NodeIds[15]);
                ib.Add(hexItem.NodeIds[4]);
                ib.Add(hexItem.NodeIds[16]);

                ib.Add(hexItem.NodeIds[11]);
                ib.Add(hexItem.NodeIds[19]);
                ib.Add(hexItem.NodeIds[16]);

                ib.Add(hexItem.NodeIds[19]);
                ib.Add(hexItem.NodeIds[15]);
                ib.Add(hexItem.NodeIds[16]);

                // LEFT
                ib.Add(hexItem.NodeIds[1]);
                ib.Add(hexItem.NodeIds[17]);
                ib.Add(hexItem.NodeIds[8]);

                ib.Add(hexItem.NodeIds[17]);
                ib.Add(hexItem.NodeIds[5]);
                ib.Add(hexItem.NodeIds[12]);

                ib.Add(hexItem.NodeIds[12]);
                ib.Add(hexItem.NodeIds[4]);
                ib.Add(hexItem.NodeIds[16]);

                ib.Add(hexItem.NodeIds[16]);
                ib.Add(hexItem.NodeIds[0]);
                ib.Add(hexItem.NodeIds[8]);

                ib.Add(hexItem.NodeIds[8]);
                ib.Add(hexItem.NodeIds[17]);
                ib.Add(hexItem.NodeIds[12]);

                ib.Add(hexItem.NodeIds[12]);
                ib.Add(hexItem.NodeIds[16]);
                ib.Add(hexItem.NodeIds[8]);

                // RIGHT
                ib.Add(hexItem.NodeIds[2]);
                ib.Add(hexItem.NodeIds[10]);
                ib.Add(hexItem.NodeIds[18]);

                ib.Add(hexItem.NodeIds[10]);
                ib.Add(hexItem.NodeIds[3]);
                ib.Add(hexItem.NodeIds[19]);

                ib.Add(hexItem.NodeIds[19]);
                ib.Add(hexItem.NodeIds[7]);
                ib.Add(hexItem.NodeIds[14]);

                ib.Add(hexItem.NodeIds[14]);
                ib.Add(hexItem.NodeIds[6]);
                ib.Add(hexItem.NodeIds[18]);

                ib.Add(hexItem.NodeIds[10]);
                ib.Add(hexItem.NodeIds[14]);
                ib.Add(hexItem.NodeIds[18]);

                ib.Add(hexItem.NodeIds[10]);
                ib.Add(hexItem.NodeIds[19]);
                ib.Add(hexItem.NodeIds[14]);
            }

            Vector3[] normals = new Vector3[nodes.Count];
            for(int ii=0; ii<ib.Count/3; ++ii)
            {
                uint a = ib[ii*3];
                uint b = ib[ii*3+1];
                uint c = ib[ii*3+2];

                Vector3 p1 = nodes[a].Value;
                Vector3 p2 = nodes[b].Value ;
                Vector3 p3 = nodes[c].Value ;

                Vector3 normal = (p2 - p1).CrossProduct(p3-p1);

                if( normals[a] == null )
                {
                    normals[a] = Vector3.ZERO;
                }
                if( normals[b] == null )
                {
                    normals[b] = Vector3.ZERO;
                }
                if( normals[c] == null )
                {
                    normals[c] = Vector3.ZERO;
                }

                normals[a] += normal;
                normals[b] += normal;
                normals[c] += normal;
            }

            float[] vb = new float[nodes.Count * 3];
            float[] nb = new float[nodes.Count * 3];
            float[] cb = new float[nodes.Count * 3];

            AABox bbox = new AABox();
            for (uint ii = 0; ii < nodes.Count; ++ii )
            {
                var nd = nodes[ii];

                bbox.Merge(new AABox(nd.Value, nd.Value));

                vb[ii * 3] = (float)nd.Value.X;
                vb[ii * 3 + 1] = (float)nd.Value.Y;
                vb[ii * 3 + 2] = (float)nd.Value.Z;

                var normal = normals[ii];
                normal.Normalize();

                nb[ii * 3] = (float)normal.X;
                nb[ii * 3 + 1] = (float)normal.Y;
                nb[ii * 3 + 2] = (float)normal.Z;


                int idx = (int)((nd.Data - minValue) / segment);

                if (idx >= colorTables.Count)
                    idx = colorTables.Count - 1;

                ColorValue clr = colorTables.ElementAt(idx);

                cb[ii * 3] = clr.R;
                cb[ii * 3 + 1] = clr.G;
                cb[ii * 3 + 2] = clr.B;
            }

            var pEntity = GlobalInstance.TopoShapeConvert.CreateColoredFaceEntity(vb, ib.ToArray(), nb, cb, bbox);
            var nEntityNode = new EntitySceneNode();
            nEntityNode.SetEntity(pEntity);
            context.ShowSceneNode(nEntityNode);

                return true;
        }
    }
}
