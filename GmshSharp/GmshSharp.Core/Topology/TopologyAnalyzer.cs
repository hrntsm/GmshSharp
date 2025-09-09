using GmshSharp.Core.Mesh;

namespace GmshSharp.Core.Topology
{
    /// <summary>
    /// Provides mesh topology analysis capabilities
    /// </summary>
    public static class TopologyAnalyzer
    {
        /// <summary>
        /// Computes the Euler characteristic of the mesh (V - E + F)
        /// </summary>
        public static int ComputeEulerCharacteristic(HalfEdgeMesh mesh)
        {
            int vertices = mesh.Vertices.Count;
            int faces = mesh.Faces.Count;
            int edges = mesh.HalfEdges.Count / 2; // Each edge has two half-edges

            return vertices - edges + faces;
        }

        /// <summary>
        /// Finds all boundary loops in the mesh
        /// </summary>
        public static List<List<Vertex>> FindBoundaryLoops(HalfEdgeMesh mesh)
        {
            var boundaryLoops = new List<List<Vertex>>();
            var visited = new HashSet<HalfEdge>();

            foreach (HalfEdge boundaryEdge in mesh.BoundaryHalfEdges())
            {
                if (visited.Contains(boundaryEdge))
                    continue;

                var loop = new List<Vertex>();
                HalfEdge? current = boundaryEdge;

                do
                {
                    visited.Add(current);
                    loop.Add(current.Origin);
                    current = GetNextBoundaryEdge(current);
                } while (current != null && current != boundaryEdge && !visited.Contains(current));

                if (loop.Count > 0)
                    boundaryLoops.Add(loop);
            }

            return boundaryLoops;
        }

        /// <summary>
        /// Checks if the mesh is a closed surface (no boundary)
        /// </summary>
        public static bool IsClosedSurface(HalfEdgeMesh mesh)
        {
            return !mesh.BoundaryHalfEdges().Any();
        }

        /// <summary>
        /// Computes mesh quality metrics
        /// </summary>
        public static MeshQualityMetrics ComputeQualityMetrics(HalfEdgeMesh mesh)
        {
            var edgeLengths = mesh.HalfEdges.Select(he => he.Length()).ToList();
            var faceAreas = mesh.Faces.Select(f => f.Area()).ToList();

            return new MeshQualityMetrics
            {
                MinEdgeLength = edgeLengths.Count > 0 ? edgeLengths.Min() : 0,
                MaxEdgeLength = edgeLengths.Count > 0 ? edgeLengths.Max() : 0,
                AverageEdgeLength = edgeLengths.Count > 0 ? edgeLengths.Average() : 0,
                MinFaceArea = faceAreas.Count > 0 ? faceAreas.Min() : 0,
                MaxFaceArea = faceAreas.Count > 0 ? faceAreas.Max() : 0,
                AverageFaceArea = faceAreas.Count > 0 ? faceAreas.Average() : 0,
                TotalSurfaceArea = faceAreas.Sum()
            };
        }

        private static HalfEdge? GetNextBoundaryEdge(HalfEdge currentEdge)
        {
            Vertex vertex = currentEdge.Target;
            return vertex.OutgoingHalfEdges().FirstOrDefault(he => he.IsBoundary);
        }
    }

    /// <summary>
    /// Contains mesh quality metrics
    /// </summary>
    public class MeshQualityMetrics
    {
        public double MinEdgeLength { get; set; }
        public double MaxEdgeLength { get; set; }
        public double AverageEdgeLength { get; set; }
        public double MinFaceArea { get; set; }
        public double MaxFaceArea { get; set; }
        public double AverageFaceArea { get; set; }
        public double TotalSurfaceArea { get; set; }
    }
}
