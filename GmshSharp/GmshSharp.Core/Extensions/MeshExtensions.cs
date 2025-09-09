using GmshSharp.Core.Mesh;

namespace GmshSharp.Core.Extensions
{
    /// <summary>
    /// Extension methods for converting between Gmsh and HalfEdgeMesh
    /// </summary>
    public static class MeshExtensions
    {
        /// <summary>
        /// Creates a HalfEdgeMesh from Gmsh mesh data
        /// </summary>
        public static HalfEdgeMesh ToHalfEdgeMesh(this GmshMeshData meshData)
        {
            var mesh = new HalfEdgeMesh();
            var vertexMap = new Dictionary<int, Vertex>();

            // Add vertices
            foreach ((int tag, (double x, double y, double z) coords) in meshData.Nodes)
            {
                Vertex vertex = mesh.AddVertex(coords.x, coords.y, coords.z);
                vertexMap[tag] = vertex;
            }

            // Add faces based on element type
            foreach ((int elementTag, int elementType, int[] nodeTags) in meshData.Elements)
            {
                switch (elementType)
                {
                    case 2: // Triangle
                        if (nodeTags.Length >= 3)
                        {
                            mesh.AddTriangleFace(
                                vertexMap[nodeTags[0]],
                                vertexMap[nodeTags[1]],
                                vertexMap[nodeTags[2]]
                            );
                        }
                        break;
                    case 3: // Quadrangle
                        if (nodeTags.Length >= 4)
                        {
                            mesh.AddQuadFace(
                                vertexMap[nodeTags[0]],
                                vertexMap[nodeTags[1]],
                                vertexMap[nodeTags[2]],
                                vertexMap[nodeTags[3]]
                            );
                        }
                        break;
                }
            }

            return mesh;
        }
    }

    /// <summary>
    /// Data structure to hold Gmsh mesh information
    /// </summary>
    public class GmshMeshData
    {
        public Dictionary<int, (double x, double y, double z)> Nodes { get; set; } = new();
        public List<(int elementTag, int elementType, int[] nodeTags)> Elements { get; set; } = new();
    }
}
