using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;

namespace Features.Rendering.NodeObject
{
    class PointMarker : Feature
    {
        public PointMarker()
        {
            Name = "Point Marker";
            Group = RenderingModule.GroupId;
            Category = RenderingModule.NodeCategoryId;
        }

        public override bool Run(FeatureContext context)
        {

            const float len = 100.0f;
            const int nDim = 50;
            float[] pointBuffer = new float[nDim * nDim * nDim * 3];
            float[] colorBuffer = new float[nDim * nDim * nDim * 3];
            int idx = -1;
            for (int ii = 0; ii < nDim; ++ii)
                for (int jj = 0; jj < nDim; ++jj)
                    for (int kk = 0; kk < nDim; ++kk)
                    {
                        ++idx;
                        pointBuffer[idx * 3] = ii * len;
                        pointBuffer[idx * 3 + 1] = jj * len;
                        pointBuffer[idx * 3 + 2] = kk * len;

                        colorBuffer[idx * 3] = ((float)ii) / ((float)nDim);
                        colorBuffer[idx * 3 + 1] = ((float)jj) / ((float)nDim);
                        colorBuffer[idx * 3 + 2] = ((float)kk) / ((float)nDim);
                    }

            PointStyle pointStyle = new PointStyle();
            //pointStyle.SetPointSize(4.0f);
            pointStyle.SetMarker("cross");

            PointCloudNode pcn = new PointCloudNode();
            pcn.SetPointStyle(pointStyle);
            pcn.SetPoints(pointBuffer);
            pcn.SetColors(colorBuffer);
            pcn.ComputeBBox();
            AABox bbox = pcn.GetBBox();
            Vector3 pt = bbox.MinPt;

            context.ShowSceneNode(pcn);


            return true;
        }
    }
}
