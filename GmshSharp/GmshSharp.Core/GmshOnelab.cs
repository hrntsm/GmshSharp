using System.Text;

using GmshSharp.Native;

namespace GmshSharp
{
    public static unsafe partial class Gmsh
    {
        public static class Onelab
        {
            public static void Set(string data, string format = "json")
            {
                int ierr = 0;
                fixed (byte* dataPtr = Encoding.UTF8.GetBytes(data + '\0'))
                fixed (byte* formatPtr = Encoding.UTF8.GetBytes(format + '\0'))
                {
                    NativeMethods.gmshOnelabSet(dataPtr, formatPtr, &ierr);
                }
                if (ierr != 0)
                    throw new InvalidOperationException($"Failed to set ONELAB data: error code {ierr}");
            }

            public static void SetNumber(string name, double[] values)
            {
                int ierr = 0;
                fixed (byte* namePtr = Encoding.UTF8.GetBytes(name + '\0'))
                fixed (double* valuesPtr = values)
                {
                    NativeMethods.gmshOnelabSetNumber(namePtr, valuesPtr, (nuint)values.Length, &ierr);
                }
                if (ierr != 0)
                    throw new InvalidOperationException($"Failed to set ONELAB number {name}: error code {ierr}");
            }

            public static double[] GetNumber(string name)
            {
                int ierr = 0;
                double* valuesPtr;
                nuint values_n;
                fixed (byte* namePtr = Encoding.UTF8.GetBytes(name + '\0'))
                {
                    NativeMethods.gmshOnelabGetNumber(namePtr, &valuesPtr, &values_n, &ierr);
                }
                if (ierr != 0)
                    throw new InvalidOperationException($"Failed to get ONELAB number {name}: error code {ierr}");
                /* Unmerged change from project 'GmshSharp.Core(net48)'
                Before:
                                var result = new double[values_n];
                After:
                                double[] result = new double[values_n];
                */


                double[] result = new double[values_n];
                for (int i = 0; i < (int)values_n; i++)
                {
                    result[i] = valuesPtr[i];
                }
                NativeMethods.gmshFree(valuesPtr);
                return result;
            }
        }
    }
}
