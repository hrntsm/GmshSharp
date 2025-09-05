using GmshSharp.Native;

namespace GmshSharp
{
    public static unsafe partial class Gmsh
    {
        public static partial class Model
        {
            public static class Geo
            {
                public static int AddPoint(double x, double y, double z, double meshSize = 0, int tag = -1)
                {
                    int ierr = 0;
                    int result = NativeMethods.gmshModelGeoAddPoint(x, y, z, meshSize, tag, &ierr);
                    if (ierr != 0)
                        throw new InvalidOperationException($"Failed to add point: error code {ierr}");
                    return result;
                }

                public static int AddLine(int startTag, int endTag, int tag = -1)
                {
                    int ierr = 0;
                    int result = NativeMethods.gmshModelGeoAddLine(startTag, endTag, tag, &ierr);
                    if (ierr != 0)
                        throw new InvalidOperationException($"Failed to add line: error code {ierr}");
                    return result;
                }

                public static int AddCircleArc(int startTag, int centerTag, int endTag, int tag = -1, double nx = 0, double ny = 0, double nz = 0)
                {
                    int ierr = 0;
                    int result = NativeMethods.gmshModelGeoAddCircleArc(startTag, centerTag, endTag, tag, nx, ny, nz, &ierr);
                    if (ierr != 0)
                        throw new InvalidOperationException($"Failed to add circle arc: error code {ierr}");
                    return result;
                }

                public static int AddCurveLoop(int[] curveTags, int tag = -1, bool reorient = false)
                {
                    int ierr = 0;
                    int result;
                    fixed (int* curveTagsPtr = curveTags)
                    {
                        result = NativeMethods.gmshModelGeoAddCurveLoop(curveTagsPtr, (nuint)curveTags.Length, tag, reorient ? 1 : 0, &ierr);
                    }
                    if (ierr != 0)
                        throw new InvalidOperationException($"Failed to add curve loop: error code {ierr}");
                    return result;
                }

                public static int AddPlaneSurface(int[] wireTags, int tag = -1)
                {
                    int ierr = 0;
                    int result;
                    fixed (int* wireTagsPtr = wireTags)
                    {
                        result = NativeMethods.gmshModelGeoAddPlaneSurface(wireTagsPtr, (nuint)wireTags.Length, tag, &ierr);
                    }
                    if (ierr != 0)
                        throw new InvalidOperationException($"Failed to add plane surface: error code {ierr}");
                    return result;
                }

                public static int AddSurfaceLoop(int[] surfaceTags, int tag = -1)
                {
                    int ierr = 0;
                    int result;
                    fixed (int* surfaceTagsPtr = surfaceTags)
                    {
                        result = NativeMethods.gmshModelGeoAddSurfaceLoop(surfaceTagsPtr, (nuint)surfaceTags.Length, tag, &ierr);
                    }
                    if (ierr != 0)
                        throw new InvalidOperationException($"Failed to add surface loop: error code {ierr}");
                    return result;
                }

                public static int AddVolume(int[] shellTags, int tag = -1)
                {
                    int ierr = 0;
                    int result;
                    fixed (int* shellTagsPtr = shellTags)
                    {
                        result = NativeMethods.gmshModelGeoAddVolume(shellTagsPtr, (nuint)shellTags.Length, tag, &ierr);
                    }
                    if (ierr != 0)
                        throw new InvalidOperationException($"Failed to add volume: error code {ierr}");
                    return result;
                }

                public static int AddSurfaceFilling(int[] wireTags, int tag = -1, int sphereCenterTag = -1)
                {
                    int ierr = 0;
                    int result;
                    fixed (int* wireTagsPtr = wireTags)
                    {
                        result = NativeMethods.gmshModelGeoAddSurfaceFilling(wireTagsPtr, (nuint)wireTags.Length, tag, sphereCenterTag, &ierr);
                    }
                    if (ierr != 0)
                        throw new InvalidOperationException($"Failed to add surface filling: error code {ierr}");
                    return result;
                }

                public static void Synchronize()
                {
                    int ierr = 0;
                    NativeMethods.gmshModelGeoSynchronize(&ierr);
                    if (ierr != 0)
                        throw new InvalidOperationException($"Failed to synchronize: error code {ierr}");
                }

                public static void Translate((int dim, int tag)[] entities, double dx, double dy, double dz)
                {
                    int ierr = 0;
                    int[] dimTags = new int[entities.Length * 2];
                    for (int i = 0; i < entities.Length; i++)
                    {
                        dimTags[i * 2] = entities[i].dim;
                        dimTags[i * 2 + 1] = entities[i].tag;
                    }
                    fixed (int* dimTagsPtr = dimTags)
                    {
                        NativeMethods.gmshModelGeoTranslate(dimTagsPtr, (nuint)entities.Length, dx, dy, dz, &ierr);
                    }
                    if (ierr != 0)
                        throw new InvalidOperationException($"Failed to translate: error code {ierr}");
                }

                public static void Rotate((int dim, int tag)[] entities, double x, double y, double z, double ax, double ay, double az, double angle)
                {
                    int ierr = 0;
                    int[] dimTags = new int[entities.Length * 2];
                    for (int i = 0; i < entities.Length; i++)
                    {
                        dimTags[i * 2] = entities[i].dim;
                        dimTags[i * 2 + 1] = entities[i].tag;
                    }
                    fixed (int* dimTagsPtr = dimTags)
                    {
                        NativeMethods.gmshModelGeoRotate(dimTagsPtr, (nuint)entities.Length, x, y, z, ax, ay, az, angle, &ierr);
                    }
                    if (ierr != 0)
                        throw new InvalidOperationException($"Failed to rotate: error code {ierr}");
                }

                public static (int dim, int tag)[] Copy((int dim, int tag)[] entities)
                {
                    int ierr = 0;
                    int[] dimTags = new int[entities.Length * 2];
                    for (int i = 0; i < entities.Length; i++)
                    {
                        dimTags[i * 2] = entities[i].dim;
                        dimTags[i * 2 + 1] = entities[i].tag;
                    }
                    int* outDimTagsPtr;
                    nuint outDimTags_n;
                    fixed (int* dimTagsPtr = dimTags)
                    {
                        NativeMethods.gmshModelGeoCopy(dimTagsPtr, (nuint)entities.Length, &outDimTagsPtr, &outDimTags_n, &ierr);
                    }
                    if (ierr != 0)
                        throw new InvalidOperationException($"Failed to copy: error code {ierr}");

                    var result = new (int dim, int tag)[outDimTags_n];
                    for (int i = 0; i < (int)outDimTags_n; i++)
                    {
                        result[i] = (outDimTagsPtr[i * 2], outDimTagsPtr[i * 2 + 1]);
                    }
                    NativeMethods.gmshFree(outDimTagsPtr);
                    return result;
                }

                public static (int dim, int tag)[] Extrude((int dim, int tag)[] entities, double dx, double dy, double dz)
                {
                    int ierr = 0;
                    int[] dimTags = new int[entities.Length * 2];
                    for (int i = 0; i < entities.Length; i++)
                    {
                        dimTags[i * 2] = entities[i].dim;
                        dimTags[i * 2 + 1] = entities[i].tag;
                    }
                    int* outDimTagsPtr;
                    nuint outDimTags_n;
                    fixed (int* dimTagsPtr = dimTags)
                    {
                        NativeMethods.gmshModelGeoExtrude(dimTagsPtr, (nuint)entities.Length, dx, dy, dz, &outDimTagsPtr, &outDimTags_n, null, 0, null, 0, 0, &ierr);
                    }
                    if (ierr != 0)
                        throw new InvalidOperationException($"Failed to extrude: error code {ierr}");

                    var result = new (int dim, int tag)[outDimTags_n / 2];
                    for (int i = 0; i < (int)outDimTags_n / 2; i++)
                    {
                        result[i] = (outDimTagsPtr[i * 2], outDimTagsPtr[i * 2 + 1]);
                    }
                    NativeMethods.gmshFree(outDimTagsPtr);
                    return result;
                }

                public static (int dim, int tag)[] Extrude((int dim, int tag)[] entities, double dx, double dy, double dz, int[] numElements, double[] heights)
                {
                    int ierr = 0;
                    int[] dimTags = new int[entities.Length * 2];
                    for (int i = 0; i < entities.Length; i++)
                    {
                        dimTags[i * 2] = entities[i].dim;
                        dimTags[i * 2 + 1] = entities[i].tag;
                    }
                    int* outDimTagsPtr;
                    nuint outDimTags_n;
                    fixed (int* dimTagsPtr = dimTags)
                    fixed (int* numElementsPtr = numElements)
                    fixed (double* heightsPtr = heights)
                    {
                        NativeMethods.gmshModelGeoExtrude(dimTagsPtr, (nuint)entities.Length, dx, dy, dz, &outDimTagsPtr, &outDimTags_n, numElementsPtr, (nuint)numElements.Length, heightsPtr, (nuint)heights.Length, 0, &ierr);
                    }
                    if (ierr != 0)
                        throw new InvalidOperationException($"Failed to extrude with layers: error code {ierr}");

                    var result = new (int dim, int tag)[outDimTags_n / 2];
                    for (int i = 0; i < (int)outDimTags_n / 2; i++)
                    {
                        result[i] = (outDimTagsPtr[i * 2], outDimTagsPtr[i * 2 + 1]);
                    }
                    NativeMethods.gmshFree(outDimTagsPtr);
                    return result;
                }

                public static (int dim, int tag)[] Revolve((int dim, int tag)[] entities, double x, double y, double z, double ax, double ay, double az, double angle, int[]? numElements = null)
                {
                    int ierr = 0;
                    int[] dimTags = new int[entities.Length * 2];
                    for (int i = 0; i < entities.Length; i++)
                    {
                        dimTags[i * 2] = entities[i].dim;
                        dimTags[i * 2 + 1] = entities[i].tag;
                    }
                    int* outDimTagsPtr;
                    nuint outDimTags_n;
                    fixed (int* dimTagsPtr = dimTags)
                    {
                        if (numElements != null)
                        {
                            fixed (int* numElementsPtr = numElements)
                            {
                                NativeMethods.gmshModelGeoRevolve(dimTagsPtr, (nuint)entities.Length, x, y, z, ax, ay, az, angle, &outDimTagsPtr, &outDimTags_n, numElementsPtr, (nuint)numElements.Length, null, 0, 0, &ierr);
                            }
                        }
                        else
                        {
                            NativeMethods.gmshModelGeoRevolve(dimTagsPtr, (nuint)entities.Length, x, y, z, ax, ay, az, angle, &outDimTagsPtr, &outDimTags_n, null, 0, null, 0, 0, &ierr);
                        }
                    }
                    if (ierr != 0)
                        throw new InvalidOperationException($"Failed to revolve: error code {ierr}");

                    var result = new (int dim, int tag)[outDimTags_n / 2];
                    for (int i = 0; i < (int)outDimTags_n / 2; i++)
                    {
                        result[i] = (outDimTagsPtr[i * 2], outDimTagsPtr[i * 2 + 1]);
                    }
                    NativeMethods.gmshFree(outDimTagsPtr);
                    return result;
                }

                public static (int dim, int tag)[] Twist((int dim, int tag)[] entities, double x, double y, double z, double dx, double dy, double dz, double ax, double ay, double az, double angle, int[] numElements, double[]? heights = null, bool recombine = false)
                {
                    int ierr = 0;
                    int[] dimTags = new int[entities.Length * 2];
                    for (int i = 0; i < entities.Length; i++)
                    {
                        dimTags[i * 2] = entities[i].dim;
                        dimTags[i * 2 + 1] = entities[i].tag;
                    }
                    int* outDimTagsPtr;
                    nuint outDimTags_n;
                    fixed (int* dimTagsPtr = dimTags)
                    fixed (int* numElementsPtr = numElements)
                    {
                        if (heights != null)
                        {
                            fixed (double* heightsPtr = heights)
                            {
                                NativeMethods.gmshModelGeoTwist(dimTagsPtr, (nuint)entities.Length, x, y, z, dx, dy, dz, ax, ay, az, angle, &outDimTagsPtr, &outDimTags_n, numElementsPtr, (nuint)numElements.Length, heightsPtr, (nuint)heights.Length, recombine ? 1 : 0, &ierr);
                            }
                        }
                        else
                        {
                            NativeMethods.gmshModelGeoTwist(dimTagsPtr, (nuint)entities.Length, x, y, z, dx, dy, dz, ax, ay, az, angle, &outDimTagsPtr, &outDimTags_n, numElementsPtr, (nuint)numElements.Length, null, 0, recombine ? 1 : 0, &ierr);
                        }
                    }
                    if (ierr != 0)
                        throw new InvalidOperationException($"Failed to twist: error code {ierr}");

                    var result = new (int dim, int tag)[outDimTags_n / 2];
                    for (int i = 0; i < (int)outDimTags_n / 2; i++)
                    {
                        result[i] = (outDimTagsPtr[i * 2], outDimTagsPtr[i * 2 + 1]);
                    }
                    NativeMethods.gmshFree(outDimTagsPtr);
                    return result;
                }

                public static class Mesh
                {
                    public static void SetSize((int dim, int tag)[] entities, double size)
                    {
                        int ierr = 0;
                        int[] dimTags = new int[entities.Length * 2];
                        for (int i = 0; i < entities.Length; i++)
                        {
                            dimTags[i * 2] = entities[i].dim;
                            dimTags[i * 2 + 1] = entities[i].tag;
                        }
                        fixed (int* dimTagsPtr = dimTags)
                        {
                            NativeMethods.gmshModelGeoMeshSetSize(dimTagsPtr, (nuint)entities.Length, size, &ierr);
                        }
                        if (ierr != 0)
                            throw new InvalidOperationException($"Failed to set mesh size: error code {ierr}");
                    }
                }
            }
        }
    }
}
