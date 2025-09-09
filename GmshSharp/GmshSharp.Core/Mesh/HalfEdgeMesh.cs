namespace GmshSharp.Core.Mesh
{
    /// <summary>
    /// Represents a vertex in a half-edge mesh structure
    /// </summary>
    public class Vertex
    {
        public int Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public HalfEdge? OutgoingHalfEdge { get; set; }

        public Vertex(int id, double x, double y, double z)
        {
            Id = id;
            X = x;
            Y = y;
            Z = z;
        }

        public IEnumerable<HalfEdge> OutgoingHalfEdges()
        {
            if (OutgoingHalfEdge == null) yield break;
            HalfEdge? current = OutgoingHalfEdge;
            do
            {
                yield return current;
                current = current.Twin?.Next;
            } while (current != null && current != OutgoingHalfEdge);
        }

        public IEnumerable<Face> AdjacentFaces()
        {
            return OutgoingHalfEdges()
                .Where(he => he.Face != null)
                .Select(he => he.Face!);
        }

        public IEnumerable<Vertex> AdjacentVertices()
        {
            return OutgoingHalfEdges()
                .Select(he => he.Target);
        }
    }

    /// <summary>
    /// Represents a face in a half-edge mesh structure
    /// </summary>
    public class Face
    {
        public int Id { get; set; }
        public HalfEdge? HalfEdge { get; set; }

        public Face(int id)
        {
            Id = id;
        }

        public IEnumerable<HalfEdge> HalfEdges()
        {
            if (HalfEdge == null) yield break;
            HalfEdge? current = HalfEdge;
            do
            {
                yield return current;
                current = current.Next;
            } while (current != null && current != HalfEdge);
        }

        public IEnumerable<Vertex> Vertices()
        {
            return HalfEdges().Select(he => he.Origin);
        }

        public double Area()
        {
            Vertex[] vertices = Vertices().ToArray();
            if (vertices.Length < 3) return 0;

            double area = 0;
            for (int i = 1; i < vertices.Length - 1; i++)
            {
                Vertex v0 = vertices[0];
                Vertex v1 = vertices[i];
                Vertex v2 = vertices[i + 1];

                (double x, double y, double z) cross = CrossProduct(
                    v1.X - v0.X, v1.Y - v0.Y, v1.Z - v0.Z,
                    v2.X - v0.X, v2.Y - v0.Y, v2.Z - v0.Z
                );
                area += 0.5 * Math.Sqrt(cross.x * cross.x + cross.y * cross.y + cross.z * cross.z);
            }
            return area;
        }

        private static (double x, double y, double z) CrossProduct(double ax, double ay, double az, double bx, double by, double bz)
        {
            return (ay * bz - az * by, az * bx - ax * bz, ax * by - ay * bx);
        }
    }

    /// <summary>
    /// Represents a half-edge in a half-edge mesh structure
    /// </summary>
    public class HalfEdge
    {
        public int Id { get; set; }
        public Vertex Origin { get; set; }
        public Vertex Target { get; set; }
        public HalfEdge? Twin { get; set; }
        public HalfEdge? Next { get; set; }
        public HalfEdge? Previous { get; set; }
        public Face? Face { get; set; }

        public HalfEdge(int id, Vertex origin, Vertex target)
        {
            Id = id;
            Origin = origin;
            Target = target;
        }

        public bool IsBoundary => Twin == null;

        public double Length()
        {
            double dx = Target.X - Origin.X;
            double dy = Target.Y - Origin.Y;
            double dz = Target.Z - Origin.Z;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }
    }

    /// <summary>
    /// A half-edge mesh data structure for efficient mesh topology operations
    /// </summary>
    public class HalfEdgeMesh
    {
        private readonly Dictionary<int, Vertex> _vertices = new();
        private readonly Dictionary<int, Face> _faces = new();
        private readonly Dictionary<int, HalfEdge> _halfEdges = new();
        private int _nextVertexId = 1;
        private int _nextFaceId = 1;
        private int _nextHalfEdgeId = 1;

        public IReadOnlyCollection<Vertex> Vertices => _vertices.Values;
        public IReadOnlyCollection<Face> Faces => _faces.Values;
        public IReadOnlyCollection<HalfEdge> HalfEdges => _halfEdges.Values;

        public Vertex AddVertex(double x, double y, double z)
        {
            var vertex = new Vertex(_nextVertexId++, x, y, z);
            _vertices[vertex.Id] = vertex;
            return vertex;
        }

        public Face AddTriangleFace(Vertex v1, Vertex v2, Vertex v3)
        {
            var face = new Face(_nextFaceId++);
            _faces[face.Id] = face;

            HalfEdge he1 = GetOrCreateHalfEdge(v1, v2);
            HalfEdge he2 = GetOrCreateHalfEdge(v2, v3);
            HalfEdge he3 = GetOrCreateHalfEdge(v3, v1);

            he1.Next = he2;
            he1.Previous = he3;
            he1.Face = face;

            he2.Next = he3;
            he2.Previous = he1;
            he2.Face = face;

            he3.Next = he1;
            he3.Previous = he2;
            he3.Face = face;

            face.HalfEdge = he1;

            if (v1.OutgoingHalfEdge == null) v1.OutgoingHalfEdge = he1;
            if (v2.OutgoingHalfEdge == null) v2.OutgoingHalfEdge = he2;
            if (v3.OutgoingHalfEdge == null) v3.OutgoingHalfEdge = he3;

            return face;
        }

        public Face AddQuadFace(Vertex v1, Vertex v2, Vertex v3, Vertex v4)
        {
            var face = new Face(_nextFaceId++);
            _faces[face.Id] = face;

            HalfEdge he1 = GetOrCreateHalfEdge(v1, v2);
            HalfEdge he2 = GetOrCreateHalfEdge(v2, v3);
            HalfEdge he3 = GetOrCreateHalfEdge(v3, v4);
            HalfEdge he4 = GetOrCreateHalfEdge(v4, v1);

            he1.Next = he2;
            he1.Previous = he4;
            he1.Face = face;

            he2.Next = he3;
            he2.Previous = he1;
            he2.Face = face;

            he3.Next = he4;
            he3.Previous = he2;
            he3.Face = face;

            he4.Next = he1;
            he4.Previous = he3;
            he4.Face = face;

            face.HalfEdge = he1;

            if (v1.OutgoingHalfEdge == null) v1.OutgoingHalfEdge = he1;
            if (v2.OutgoingHalfEdge == null) v2.OutgoingHalfEdge = he2;
            if (v3.OutgoingHalfEdge == null) v3.OutgoingHalfEdge = he3;
            if (v4.OutgoingHalfEdge == null) v4.OutgoingHalfEdge = he4;

            return face;
        }

        private HalfEdge GetOrCreateHalfEdge(Vertex origin, Vertex target)
        {
            HalfEdge? existingTwin = _halfEdges.Values
                .FirstOrDefault(he => he.Origin == target && he.Target == origin);

            if (existingTwin != null)
            {
                var newHalfEdge = new HalfEdge(_nextHalfEdgeId++, origin, target);
                _halfEdges[newHalfEdge.Id] = newHalfEdge;

                newHalfEdge.Twin = existingTwin;
                existingTwin.Twin = newHalfEdge;

                return newHalfEdge;
            }

            var halfEdge = new HalfEdge(_nextHalfEdgeId++, origin, target);
            _halfEdges[halfEdge.Id] = halfEdge;
            return halfEdge;
        }

        public IEnumerable<HalfEdge> BoundaryHalfEdges()
        {
            return _halfEdges.Values.Where(he => he.IsBoundary);
        }

        public bool IsManifold()
        {
            foreach (Vertex vertex in _vertices.Values)
            {
                var outgoingEdges = vertex.OutgoingHalfEdges().ToList();
                var boundaryEdges = outgoingEdges.Where(he => he.IsBoundary).ToList();

                if (boundaryEdges.Count > 2)
                    return false;
            }

            foreach (HalfEdge? halfEdge in _halfEdges.Values.Where(he => !he.IsBoundary))
            {
                if (halfEdge.Twin == null)
                    return false;
            }

            return true;
        }

        public double TotalSurfaceArea()
        {
            return _faces.Values.Sum(f => f.Area());
        }

        public (double minX, double minY, double minZ, double maxX, double maxY, double maxZ) GetBoundingBox()
        {
            if (!_vertices.Any())
                return (0, 0, 0, 0, 0, 0);

            double minX = _vertices.Values.Min(v => v.X);
            double minY = _vertices.Values.Min(v => v.Y);
            double minZ = _vertices.Values.Min(v => v.Z);
            double maxX = _vertices.Values.Max(v => v.X);
            double maxY = _vertices.Values.Max(v => v.Y);
            double maxZ = _vertices.Values.Max(v => v.Z);

            return (minX, minY, minZ, maxX, maxY, maxZ);
        }

        public void Clear()
        {
            _vertices.Clear();
            _faces.Clear();
            _halfEdges.Clear();
            _nextVertexId = 1;
            _nextFaceId = 1;
            _nextHalfEdgeId = 1;
        }
    }
}
