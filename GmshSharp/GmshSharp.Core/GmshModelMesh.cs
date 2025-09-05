using GmshSharp.Native;

namespace GmshSharp
{
    public static unsafe partial class Gmsh
    {
        public static partial class Model
        {
            public static class Mesh
            {
                public static void Generate(int dimension)
                {
                    int ierr = 0;
                    NativeMethods.gmshModelMeshGenerate(dimension, &ierr);
                    if (ierr != 0)
                        throw new InvalidOperationException($"Failed to generate mesh: error code {ierr}");
                }

                public static void SetAlgorithm(int dimension, int tag, int algorithm)
                {
                    int ierr = 0;
                    NativeMethods.gmshModelMeshSetAlgorithm(dimension, tag, algorithm, &ierr);
                    if (ierr != 0)
                        throw new InvalidOperationException($"Failed to set mesh algorithm: error code {ierr}");
                }
            }
        }
    }
}
