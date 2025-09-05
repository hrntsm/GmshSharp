using System;

namespace GmshSharp.Tutorials
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("GmshSharp Tutorials");
            Console.WriteLine("===================");

            if (args.Length == 0)
            {
                ShowUsage();
                return;
            }

            string tutorial = args[0].ToLower();

            try
            {
                switch (tutorial)
                {
                    case "t1":
                        Console.WriteLine("Running Tutorial 1: Geometry basics, elementary entities, physical groups");
                        T1.Run();
                        break;
                    case "t2":
                        Console.WriteLine("Running Tutorial 2: Transformations, extruded geometries, volumes");
                        T2.Run();
                        break;
                    case "t3":
                        Console.WriteLine("Running Tutorial 3: Extruded meshes, ONELAB parameters, options");
                        T3.Run();
                        break;
                    case "t4":
                        Console.WriteLine("Running Tutorial 4: Holes in surfaces, annotations, entity colors");
                        T4.Run();
                        break;
                    case "t5":
                        Console.WriteLine("Running Tutorial 5: Mesh sizes, holes in volumes");
                        T5.Run();
                        break;
                    case "all":
                        Console.WriteLine("Running all tutorials...");
                        RunAllTutorials();
                        break;
                    default:
                        Console.WriteLine($"Unknown tutorial: {args[0]}");
                        ShowUsage();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error running tutorial {tutorial}: {ex.Message}");
                if (args.Length > 1 && args[1] == "--verbose")
                {
                    Console.WriteLine($"Stack trace: {ex.StackTrace}");
                }
            }
        }

        private static void ShowUsage()
        {
            Console.WriteLine();
            Console.WriteLine("Usage: dotnet run [tutorial] [options]");
            Console.WriteLine();
            Console.WriteLine("Available tutorials:");
            Console.WriteLine("  t1      - Geometry basics, elementary entities, physical groups");
            Console.WriteLine("  t2      - Transformations, extruded geometries, volumes");
            Console.WriteLine("  t3      - Extruded meshes, ONELAB parameters, options");
            Console.WriteLine("  t4      - Holes in surfaces, annotations, entity colors");
            Console.WriteLine("  t5      - Mesh sizes, holes in volumes");
            Console.WriteLine("  all     - Run all tutorials");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  --verbose   Show detailed error information");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine("  dotnet run t1");
            Console.WriteLine("  dotnet run t4 --verbose");
            Console.WriteLine("  dotnet run all");
        }

        private static void RunAllTutorials()
        {
            (string, Action)[] tutorials = new[]
            {
                ("T1", (Action)T1.Run),
                ("T2", (Action)T2.Run),
                ("T3", (Action)T3.Run),
                ("T4", (Action)T4.Run),
                ("T5", (Action)T5.Run)
            };

            foreach ((string name, Action action) in tutorials)
            {
                Console.WriteLine($"\n--- Running {name} ---");
                try
                {
                    action();
                    Console.WriteLine($"✓ {name} completed successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"✗ {name} failed: {ex.Message}");
                }
            }

            Console.WriteLine("\n--- All tutorials completed ---");
        }
    }
}
