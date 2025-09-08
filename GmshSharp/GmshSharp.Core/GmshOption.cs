using System;
using System.Text;

using GmshSharp.Native;

namespace GmshSharp
{
    public static unsafe partial class Gmsh
    {
        public static class Option
        {
            public static void SetNumber(string name, double value)
            {
                int ierr = 0;
                fixed (byte* ptr = Encoding.UTF8.GetBytes(name + '\0'))
                {
                    NativeMethods.gmshOptionSetNumber(ptr, value, &ierr);
                }
                if (ierr != 0)
                    throw new InvalidOperationException($"Failed to set option {name}: error code {ierr}");
            }

            public static double GetNumber(string name)
            {
                int ierr = 0;
                double value = 0;
                fixed (byte* ptr = Encoding.UTF8.GetBytes(name + '\0'))
                {
                    NativeMethods.gmshOptionGetNumber(ptr, &value, &ierr);
                }
                if (ierr != 0)
                    throw new InvalidOperationException($"Failed to get option {name}: error code {ierr}");
                return value;
            }

            public static void SetString(string name, string value)
            {
                int ierr = 0;
                fixed (byte* namePtr = Encoding.UTF8.GetBytes(name + '\0'))
                fixed (byte* valuePtr = Encoding.UTF8.GetBytes(value + '\0'))
                {
                    NativeMethods.gmshOptionSetString(namePtr, valuePtr, &ierr);
                }
                if (ierr != 0)
                    throw new InvalidOperationException($"Failed to set option {name}: error code {ierr}");
            }

            public static void SetColor(string name, int r, int g, int b, int a = 255)
            {
                int ierr = 0;
                fixed (byte* ptr = Encoding.UTF8.GetBytes(name + '\0'))
                {
                    NativeMethods.gmshOptionSetColor(ptr, r, g, b, a, &ierr);
                }
                if (ierr != 0)
                    throw new InvalidOperationException($"Failed to set color option {name}: error code {ierr}");
            }

            public static (int r, int g, int b, int a) GetColor(string name)
            {
                int ierr = 0;
                int r = 0, g = 0, b = 0, a = 0;
                fixed (byte* ptr = Encoding.UTF8.GetBytes(name + '\0'))
                {
                    NativeMethods.gmshOptionGetColor(ptr, &r, &g, &b, &a, &ierr);
                }
                if (ierr != 0)
                    throw new InvalidOperationException($"Failed to get color option {name}: error code {ierr}");
                return (r, g, b, a);
            }
        }
    }
}
