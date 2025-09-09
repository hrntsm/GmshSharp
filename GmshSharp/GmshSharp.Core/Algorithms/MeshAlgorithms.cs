using GmshSharp.Core.Mesh;

namespace GmshSharp.Core.Algorithms
{
    /// <summary>
    /// Common mesh processing algorithms
    /// </summary>
    public static class MeshAlgorithms
    {
        /// <summary>
        /// Computes vertex normals using area-weighted face normals
        /// </summary>
        public static Dictionary<Vertex, (double x, double y, double z)> ComputeVertexNormals(HalfEdgeMesh mesh)
        {
            var vertexNormals = new Dictionary<Vertex, (double x, double y, double z)>();

            foreach (Vertex vertex in mesh.Vertices)
            {
                double nx = 0, ny = 0, nz = 0;

                foreach (Face face in vertex.AdjacentFaces())
                {
                    (double x, double y, double z) normal = ComputeFaceNormal(face);
                    double area = face.Area();

                    nx += normal.x * area;
                    ny += normal.y * area;
                    nz += normal.z * area;
                }

                double length = Math.Sqrt(nx * nx + ny * ny + nz * nz);
                if (length > 0)
                {
                    nx /= length;
                    ny /= length;
                    nz /= length;
                }

                vertexNormals[vertex] = (nx, ny, nz);
            }

            return vertexNormals;
        }

        /// <summary>
        /// Computes face normal using cross product
        /// </summary>
        public static (double x, double y, double z) ComputeFaceNormal(Face face)
        {
            Vertex[] vertices = face.Vertices().ToArray();
            if (vertices.Length < 3)
                return (0, 0, 1);

            Vertex v0 = vertices[0];
            Vertex v1 = vertices[1];
            Vertex v2 = vertices[2];

            (double, double, double) edge1 = (v1.X - v0.X, v1.Y - v0.Y, v1.Z - v0.Z);
            (double, double, double) edge2 = (v2.X - v0.X, v2.Y - v0.Y, v2.Z - v0.Z);

            (double, double, double) normal = (
                edge1.Item2 * edge2.Item3 - edge1.Item3 * edge2.Item2,
                edge1.Item3 * edge2.Item1 - edge1.Item1 * edge2.Item3,
                edge1.Item1 * edge2.Item2 - edge1.Item2 * edge2.Item1
            );

            double length = Math.Sqrt(normal.Item1 * normal.Item1 + normal.Item2 * normal.Item2 + normal.Item3 * normal.Item3);
            if (length > 0)
            {
                normal = (normal.Item1 / length, normal.Item2 / length, normal.Item3 / length);
            }

            return normal;
        }

        /// <summary>
        /// Finds vertices within a given radius from a center point
        /// </summary>
        public static List<Vertex> FindVerticesInRadius(HalfEdgeMesh mesh, double centerX, double centerY, double centerZ, double radius)
        {
            var result = new List<Vertex>();
            double radiusSquared = radius * radius;

            foreach (Vertex vertex in mesh.Vertices)
            {
                double dx = vertex.X - centerX;
                double dy = vertex.Y - centerY;
                double dz = vertex.Z - centerZ;
                double distanceSquared = dx * dx + dy * dy + dz * dz;

                if (distanceSquared <= radiusSquared)
                {
                    result.Add(vertex);
                }
            }

            return result;
        }

        /// <summary>
        /// Smooths the mesh using Laplacian smoothing
        /// </summary>
        public static void LaplacianSmooth(HalfEdgeMesh mesh, int iterations = 1, double factor = 0.5)
        {
            for (int iter = 0; iter < iterations; iter++)
            {
                var newPositions = new Dictionary<Vertex, (double x, double y, double z)>();

                foreach (Vertex vertex in mesh.Vertices)
                {
                    var neighbors = vertex.AdjacentVertices().ToList();
                    if (neighbors.Count == 0)
                    {
                        newPositions[vertex] = (vertex.X, vertex.Y, vertex.Z);
                        continue;
                    }

                    double avgX = neighbors.Average(v => v.X);
                    double avgY = neighbors.Average(v => v.Y);
                    double avgZ = neighbors.Average(v => v.Z);

                    double newX = vertex.X + factor * (avgX - vertex.X);
                    double newY = vertex.Y + factor * (avgY - vertex.Y);
                    double newZ = vertex.Z + factor * (avgZ - vertex.Z);

                    newPositions[vertex] = (newX, newY, newZ);
                }

                foreach (KeyValuePair<Vertex, (double x, double y, double z)> kvp in newPositions)
                {
                    kvp.Key.X = kvp.Value.x;
                    kvp.Key.Y = kvp.Value.y;
                    kvp.Key.Z = kvp.Value.z;
                }
            }
        }

        /// <summary>
        /// Computes the distance between two vertices
        /// </summary>
        public static double Distance(Vertex v1, Vertex v2)
        {
            double dx = v2.X - v1.X;
            double dy = v2.Y - v1.Y;
            double dz = v2.Z - v1.Z;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        /// <summary>
        /// Finds the closest vertex to a given point
        /// </summary>
        public static Vertex? FindClosestVertex(HalfEdgeMesh mesh, double x, double y, double z)
        {
            Vertex? closest = null;
            double minDistance = double.MaxValue;

            foreach (Vertex vertex in mesh.Vertices)
            {
                double dx = vertex.X - x;
                double dy = vertex.Y - y;
                double dz = vertex.Z - z;
                double distance = Math.Sqrt(dx * dx + dy * dy + dz * dz);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = vertex;
                }
            }

            return closest;
        }
    }
}
