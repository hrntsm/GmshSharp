using System.Text;

using GmshSharp.Native;

namespace GmshSharp
{
    public static unsafe partial class Gmsh
    {
        public static partial class Model
        {
            public static void Add(string name)
            {
                int ierr = 0;
                fixed (byte* ptr = Encoding.UTF8.GetBytes(name + '\0'))
                {
                    NativeMethods.gmshModelAdd(ptr, &ierr);
                }
                if (ierr != 0)
                    throw new InvalidOperationException($"Failed to add model {name}: error code {ierr}");
            }

            public static int AddPhysicalGroup(int dimension, int[] tags, int tag = -1, string? name = null)
            {
                int ierr = 0;
                int result;
                fixed (int* tagsPtr = tags)
                {
                    if (name != null)
                    {
                        fixed (byte* namePtr = Encoding.UTF8.GetBytes(name + '\0'))
                        {
                            result = NativeMethods.gmshModelAddPhysicalGroup(dimension, tagsPtr, (nuint)tags.Length, tag, namePtr, &ierr);
                        }
                    }
                    else
                    {
                        result = NativeMethods.gmshModelAddPhysicalGroup(dimension, tagsPtr, (nuint)tags.Length, tag, (byte*)0, &ierr);
                    }
                }
                if (ierr != 0)
                    throw new InvalidOperationException($"Failed to add physical group: error code {ierr}");
                return result;
            }

            public static double[] GetValue(int dimension, int tag, double[]? parametricCoord = null)
            {
                int ierr = 0;
                double* coordPtr;
                nuint coord_n;
                if (parametricCoord == null)
                {
                    NativeMethods.gmshModelGetValue(dimension, tag, null, 0, &coordPtr, &coord_n, &ierr);
                }
                else
                {
                    fixed (double* paramPtr = parametricCoord)
                    {
                        NativeMethods.gmshModelGetValue(dimension, tag, paramPtr, (nuint)parametricCoord.Length, &coordPtr, &coord_n, &ierr);
                    }
                }
                if (ierr != 0)
                    throw new InvalidOperationException($"Failed to get value: error code {ierr}");

                double[] result = new double[coord_n];
                for (int i = 0; i < (int)coord_n; i++)
                {
                    result[i] = coordPtr[i];
                }
                NativeMethods.gmshFree(coordPtr);
                return result;
            }
        }
    }
}
