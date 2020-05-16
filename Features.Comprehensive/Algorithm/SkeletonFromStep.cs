using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Features.Core;
using AnyCAD.Platform;
using System.Windows.Forms;

namespace Features.Comprehensive.Algorithm
{

    class VertexInfo
    {
        public int FaceIndex;
        public int EdgeIndex;
        public int Index;
        public Vector3 Point;

        public VertexInfo(Vector3 pt, int vi, int ei, int fi)
        {
            Point = pt;
            Index = vi;
            EdgeIndex = ei;
            FaceIndex = fi;
        }
    }
    class EdgeInfo
    {
        public GeomCurve Curve;
        public int Index;
        public int FaceIndex;
        public double Length;
        public Vector3 Direction;

        public List<VertexInfo> Vertices = new List<VertexInfo>();
        public TopoShape GetShape() { return Curve.GetShape(); }
        public EdgeInfo(TopoShape edge, int idx, int faceIdx, double length)
        {
            Curve = new GeomCurve();
            Curve.Initialize(edge);
            Index = idx;
            FaceIndex = faceIdx;
            var start = Curve.Value(Curve.FirstParameter());
            var end = Curve.Value(Curve.LastParameter());
            Direction = end - start;
            Direction.Normalize();
            Length = length;

            var pts = GlobalInstance.TopoExplor.ExplorVertices(edge);
            for(int ii=0; ii<pts.Size(); ++ii)
            {
                GeomPoint gp = new GeomPoint();
                gp.Initialize(pts.GetAt(ii));
                
                Vertices.Add(new VertexInfo(gp.GetPoint(), ii, idx, faceIdx));
            }
        }
    }

    class FaceInfo
    {
        public GeomSurface Surface;
        public double Area;
        public int Index;
        public Dictionary<int, EdgeInfo> Edges = new Dictionary<int, EdgeInfo>();
        public Vector3 Direction;
       
        public FaceInfo(TopoShape face, int idx, double area)
        {
            Surface = new GeomSurface();
            Surface.Initialize(face);
            Index = idx;
            Area = area;

            Direction = Surface.GetNormal(Surface.FirstUParameter(), Surface.FirstVParameter());

            TopoShapeProperty prop = new TopoShapeProperty();
            var edges = GlobalInstance.TopoExplor.ExplorEdges(face);
            for (int jj = 0; jj < edges.Size(); ++jj)
            {
                var edge = edges.GetAt(jj);
                prop.SetShape(edge);
                var edgeInfo = new EdgeInfo(edge, jj, idx, prop.EdgeLength());
                // 只加直线？
                Edges.Add(jj, edgeInfo);
            }
        }

        public TopoShape GetShape() { return Surface.GetShape(); }

        public EdgeInfo GetEdgeInfo(int idx) { return Edges[idx]; }
    }

    class LongFaceEdge
    {
        public int FaceIdx;
        public int EdgeIdx;

        public LongFaceEdge(int faceIdx, int edgeIdx)
        {
            FaceIdx = faceIdx;
            EdgeIdx = edgeIdx;
        }
    }

    class GroupByDirection
    {
        public Vector3 Key;

        Vector3 Abs(Vector3 dir)
        {
            return new Vector3(Math.Abs(dir.X), Math.Abs(dir.Y), Math.Abs(dir.Z));
        }


        public GroupByDirection(Vector3 key)
        {
            Key = Abs(key);
        }


        public bool IsEqual(double a, double b)
        {
            return Math.Abs(a - b) < 0.001;
        }

        public bool CompareWithKey(Vector3 dir)
        {
            var newDir = Abs(dir);
            if (IsEqual(newDir.X, Key.X) && IsEqual(newDir.Y, Key.Y) && IsEqual(newDir.Z, Key.Z))
            {
                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// Group the edges by the direction of the edge.
    /// </summary>
    class EdgeGroup : GroupByDirection
    {
        public double EdgeLength;

        public List<LongFaceEdge> LongEdges = new List<LongFaceEdge>();
        
        public EdgeGroup(EdgeInfo edge)
                :base(edge.Direction)
        {
            EdgeLength = edge.Length;
        }

        public bool Add(FaceInfo face)
        {
            foreach(var item in face.Edges)
            {
                var edge = item.Value;
                if (Math.Abs(edge.Length - EdgeLength) > 0.001)
                    continue;

                if (CompareWithKey(edge.Direction))
                {
                    LongEdges.Add(new LongFaceEdge(face.Index, edge.Index));
                    return true;
                }
            }

            return false;
        }
    }

    /// <summary>
    /// Group side faces by Boundingbox and direction.
    /// </summary>
    class FaceGroup : GroupByDirection
    {
        public List<FaceInfo> Faces = new List<FaceInfo>();
        AABox BoundingBox;
        double MaxDistance;

        public FaceGroup(FaceInfo face, double maxDistance)
            :base(face.Direction)
        {
            MaxDistance = maxDistance;
            BoundingBox = face.GetShape().GetBBox();
            Faces.Add(face);
        }

        public bool AddFace(FaceInfo face)
        {
            var box = face.GetShape().GetBBox();
            if (MaxDistance < box.GetCenter().Distance(BoundingBox.GetCenter()))
                return false;

            if (!CompareWithKey(face.Direction))
                return false;

            BoundingBox.Merge(box);
            Faces.Add(face);
            return true;
        }

        public void ComputeMiddelLines()
        {
            
            
        }
    }

    class Solid
    {
        public Dictionary<int, FaceInfo> Faces = new Dictionary<int, FaceInfo>();

        public List<EdgeGroup> EdgeGroups = new List<EdgeGroup>();
        public List<FaceGroup> SideFaceGroup = new List<FaceGroup>();
        public FaceInfo GetFaceInfo(int idx) { return Faces[idx]; }
        public AABox BoundingBox;
        public Solid(TopoShape solid)
        {
            BoundingBox = solid.GetBBox();

            var faces = GlobalInstance.TopoExplor.ExplorFaces(solid);
            TopoShapeProperty prop = new TopoShapeProperty();
            List<FaceInfo> dictFaceByArea = new List<FaceInfo>();
            for (int ii = 0; ii < faces.Size(); ++ii)
            {
                var face = faces.GetAt(ii);
                prop.SetShape(face);
                double area = prop.SurfaceArea();

                var faceInfo = new FaceInfo(face, ii, area);
                dictFaceByArea.Add(faceInfo);

                Faces.Add(ii, faceInfo);
            }
            dictFaceByArea.Sort((a, b) =>
            {
                return (int)((b.Area - a.Area) * 1000);
            });

            var baseFace = dictFaceByArea[0];
            foreach(var item in baseFace.Edges)
            {
                EdgeGroup eg = new EdgeGroup(item.Value);
                EdgeGroups.Add(eg);
            }

            for(int ii=2; ii<dictFaceByArea.Count; ++ii)
            {
                var faceInfo = dictFaceByArea[ii];
                if (AddLongFace(faceInfo))
                    continue;

                AddSideFace(faceInfo);
            }
        }

       void AddSideFace(FaceInfo faceInfo)
        {
            bool added = false;
            foreach(var face in SideFaceGroup)
            {
                if(face.AddFace(faceInfo))
                {
                    added = true;
                    break;
                }
            }

            if(!added)
            {
                SideFaceGroup.Add(new FaceGroup(faceInfo, BoundingBox.Size().Length()/3));
            }
        }

        bool AddLongFace(FaceInfo faceInfo)
        {
            for (int jj = 0; jj < EdgeGroups.Count; ++jj)
            {
                if (EdgeGroups[jj].Add(faceInfo))
                    return true;
            }

            return false;
        }
    }

    class SkeletonFromStep : Feature
    {
        public SkeletonFromStep()
        {
            Name = "Section.SkeletonFromStep";
            Group = ComprehensiveModule.GroupId;
            Category = ComprehensiveModule.AlgorithmCategoryId;
        }

        public override bool Run(FeatureContext context)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "STEP (*.stp;*.step)|*.stp;*.step";
            if (dlg.ShowDialog() != DialogResult.OK)
                return false;

            var shape = GlobalInstance.BrepTools.LoadFile(new Path(dlg.FileName));
            if (shape == null)
                return false;

            // 1. Adjust position and direction by box
            var box = shape.GetBBox();
            var sz = box.Size();
            var center = box.GetCenter();            
            Matrix4 trf = GlobalInstance.MatrixBuilder.MakeTranslate(-center);
            
            
            if (sz.X < sz.Y && sz.X < sz.Z)
            {
                trf = GlobalInstance.MatrixBuilder.MakeRotation(90, Vector3.UNIT_Y) * trf;
            }        
            else if (sz.Y < sz.X && sz.Y < sz.Z)
            {
                trf = GlobalInstance.MatrixBuilder.MakeRotation(90, Vector3.UNIT_X) * trf;
            }

            shape = GlobalInstance.BrepTools.Transform(shape, trf);

            // 2. Find base face
            Solid solid = new Solid(shape);

            foreach (var fg in solid.SideFaceGroup)
            {
                foreach (var face in fg.Faces)
                {
                    context.ShowGeometry(face.GetShape());
                }
                break;
            }

            return true;
        }
    }
}
