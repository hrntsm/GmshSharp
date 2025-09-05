using System;
using System.Text;

namespace GmshSharp.Native
{
    public static unsafe class Gmsh
    {
        static Gmsh()
        {
            // Initialize wrapper to setup DLL paths
            _ = typeof(GmshWrapper);
        }

        public static void Initialize()
        {
            int ierr = 0;
            NativeMethods.gmshInitialize(0, null, 0, 0, &ierr);
            if (ierr != 0)
                throw new InvalidOperationException($"Failed to initialize Gmsh: error code {ierr}");
        }

        public static void Shutdown()
        {
            int ierr = 0;
            NativeMethods.gmshFinalize(&ierr);
        }

        public static bool IsInitialized()
        {
            int ierr = 0;
            int result = NativeMethods.gmshIsInitialized(&ierr);
            return result != 0;
        }

        public static void Clear()
        {
            int ierr = 0;
            NativeMethods.gmshClear(&ierr);
            if (ierr != 0)
                throw new InvalidOperationException($"Failed to clear Gmsh: error code {ierr}");
        }

        public static void Open(string fileName)
        {
            int ierr = 0;
            fixed (byte* ptr = Encoding.UTF8.GetBytes(fileName + '\0'))
            {
                NativeMethods.gmshOpen(ptr, &ierr);
            }
            if (ierr != 0)
                throw new InvalidOperationException($"Failed to open file {fileName}: error code {ierr}");
        }

        public static void Write(string fileName)
        {
            int ierr = 0;
            fixed (byte* ptr = Encoding.UTF8.GetBytes(fileName + '\0'))
            {
                NativeMethods.gmshWrite(ptr, &ierr);
            }
            if (ierr != 0)
                throw new InvalidOperationException($"Failed to write file {fileName}: error code {ierr}");
        }

        public static void SetOption(string name, double value)
        {
            int ierr = 0;
            fixed (byte* ptr = Encoding.UTF8.GetBytes(name + '\0'))
            {
                NativeMethods.gmshOptionSetNumber(ptr, value, &ierr);
            }
            if (ierr != 0)
                throw new InvalidOperationException($"Failed to set option {name}: error code {ierr}");
        }
    }
}
