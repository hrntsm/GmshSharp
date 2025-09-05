using System.IO;
using System.Runtime.InteropServices;

namespace GmshSharp.Native
{
    public static class GmshWrapper
    {
        private const string DllName = "gmsh";

        static GmshWrapper()
        {
            string dllPath = GetDllPath();
            if (!string.IsNullOrEmpty(dllPath))
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    SetDllDirectory(dllPath);
                }
            }
        }

        private static string GetDllPath()
        {
            string baseDir = Path.GetDirectoryName(typeof(GmshWrapper).Assembly.Location) ?? "";

            string[] searchPaths = {
                Path.Combine(baseDir, "native"),
                Path.Combine(baseDir, "runtimes", GetRuntimeIdentifier(), "native"),
                baseDir
            };

            string dllFileName = GetDllFileName();

            foreach (string path in searchPaths)
            {
                string fullPath = Path.Combine(path, dllFileName);
                if (File.Exists(fullPath))
                {
                    return path;
                }
            }

            return "";
        }

        private static string GetDllFileName()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return "gmsh.dll";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return "libgmsh.dylib";
            else
                return "libgmsh.so";
        }

        private static string GetRuntimeIdentifier()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return RuntimeInformation.ProcessArchitecture == Architecture.X64 ? "win-x64" : "win-x86";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return RuntimeInformation.ProcessArchitecture == Architecture.Arm64 ? "osx-arm64" : "osx-x64";
            }
            else
            {
                return RuntimeInformation.ProcessArchitecture == Architecture.X64 ? "linux-x64" : "linux-arm64";
            }
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool SetDllDirectory(string lpPathName);
    }
}
