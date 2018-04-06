using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;

namespace Features.Rendering.NodeObject
{
    class ColoredFaceObject : Feature
    {
        public ColoredFaceObject()
        {
            Name = "Colorful Face";
            Group = RenderingModule.GroupId;
            Category = RenderingModule.NodeCategoryId;
        }

        public override bool Run(FeatureContext context)
        {

            TopoShape circle = GlobalInstance.BrepTools.MakeCircle(Vector3.ZERO, 100, Vector3.UNIT_Z);
            TopoShape face = GlobalInstance.BrepTools.MakeFace(circle);

            FaceTriangulation ft = new FaceTriangulation();
            ft.SetTolerance(5);
            ft.Perform(face);
            float[] points = ft.GetVertexBuffer();
            int pointCount = points.Length / 3;
            uint[] indexBuffer = ft.GetIndexBuffer();
            int faceCount = indexBuffer.Length / 3;
            float[] normals = ft.GetNormalBuffer();


            float[] colorBuffer = new float[pointCount * 4];

            Random num = new Random();
            for (int ii = 0; ii < pointCount; ++ii)
            {
                int idx = ii * 4;
                colorBuffer[idx] = num.Next(0, 256) / 256.0f;
                colorBuffer[idx + 1] = num.Next(0, 256) / 256.0f;
                colorBuffer[idx + 2] = num.Next(0, 256) / 256.0f;
                colorBuffer[idx + 3] = 1;
            }

            RenderableEntity entity = GlobalInstance.TopoShapeConvert.CreateColoredFaceEntity(points, indexBuffer, normals, colorBuffer, face.GetBBox());

            EntitySceneNode node = new EntitySceneNode();
            node.SetEntity(entity);

            context.ShowSceneNode(node);

            //////////////////////////////////////////////////////////////////////////
            // Code to get the mesh
            /*
            for (int ii = 0; ii < faceCount; ++ii)
            {
                int p0 = (int)indexBuffer[ii * 3];
                int p1 = (int)indexBuffer[ii * 3 + 1];
                int p2 = (int)indexBuffer[ii * 3 + 2];

                Vector3 pt0 = new Vector3(points[p0 * 3], points[p0 * 3 + 1], points[p0 * 3 + 2]);
                Vector3 pt1 = new Vector3(points[p1 * 3], points[p1 * 3 + 1], points[p1 * 3 + 2]);
                Vector3 pt2 = new Vector3(points[p2 * 3], points[p2 * 3 + 1], points[p2 * 3 + 2]);

                // ....
                // use the same way to get the normal data for each point.
            }
             * */


            return true;
        }
    }
}
