using System;

namespace GmshSharp.Tutorials
{
    public class T1
    {
        public static void Run()
        {
            try
            {
                // Gmsh Python tutorial 1 - C# version
                // Geometry basics, elementary entities, physical groups

                Gmsh.Initialize();

                // Add a new model named "t1"
                Gmsh.Model.Add("t1");
                Console.WriteLine("Model 't1' added");

                // Define mesh size
                double lc = 1e-2;

                // Create points
                Gmsh.Model.Geo.AddPoint(0, 0, 0, lc, 1);
                Gmsh.Model.Geo.AddPoint(.1, 0, 0, lc, 2);
                Gmsh.Model.Geo.AddPoint(.1, .3, 0, lc, 3);
                int p4 = Gmsh.Model.Geo.AddPoint(0, .3, 0, lc);
                Console.WriteLine($"Points added (p4 tag: {p4})");

                // Create lines
                Gmsh.Model.Geo.AddLine(1, 2, 1);
                Gmsh.Model.Geo.AddLine(3, 2, 2);
                Gmsh.Model.Geo.AddLine(3, p4, 3);
                Gmsh.Model.Geo.AddLine(p4, 1, 4);
                Console.WriteLine("Lines added");

                // Create curve loop
                int curveLoop = Gmsh.Model.Geo.AddCurveLoop(new int[] { 4, 1, -2, 3 }, 1);
                Console.WriteLine($"Curve loop added (tag: {curveLoop})");

                // Create plane surface
                int surface = Gmsh.Model.Geo.AddPlaneSurface(new int[] { 1 }, 1);
                Console.WriteLine($"Plane surface added (tag: {surface})");

                // Synchronize the CAD entities with the Gmsh model
                Gmsh.Model.Geo.Synchronize();
                Console.WriteLine("Geometry synchronized");

                // Generate 2D mesh and save
                Gmsh.Model.Mesh.Generate(2);
                Console.WriteLine("2D mesh generated");

                Gmsh.Write("t1.msh");
                Console.WriteLine("Mesh saved to t1.msh");

                Console.WriteLine("T1 tutorial completed successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                Gmsh.Shutdown();
            }
        }
    }
}
