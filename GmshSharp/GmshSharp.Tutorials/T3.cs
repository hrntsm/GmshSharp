using System;

namespace GmshSharp.Tutorials
{
    public class T3
    {
        public static void Run()
        {
            try
            {
                // Gmsh Python tutorial 3 - C# version
                // Extruded meshes, ONELAB parameters, options

                Gmsh.Initialize();

                // Set options to make point tags visible and redefine colors
                Gmsh.Option.SetNumber("Geometry.PointNumbers", 1);
                Gmsh.Option.SetColor("Geometry.Color.Points", 255, 165, 0);
                Gmsh.Option.SetColor("General.Color.Text", 255, 255, 255);
                Gmsh.Option.SetColor("Mesh.Color.Points", 255, 0, 0);

                // Get color and apply it to surfaces
                (int r, int g, int b, int a) = Gmsh.Option.GetColor("Geometry.Points");
                Gmsh.Option.SetColor("Geometry.Surfaces", r, g, b, a);

                // Create a ONELAB parameter to define the angle of the twist
                Gmsh.Onelab.Set(@"[
                  {
                    ""type"":""number"",
                    ""name"":""Parameters/Twisting angle"",
                    ""values"":[90],
                    ""min"":0,
                    ""max"":120,
                    ""step"":1
                  }
                ]");

                // Create the geometry and mesh it
                CreateGeometryAndMesh();

                Console.WriteLine("T3 tutorial completed successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
            finally
            {
                Gmsh.Shutdown();
            }
        }

        private static void CreateGeometryAndMesh()
        {
            Console.WriteLine("Creating T3 geometry...");

            // Clear all models and create a new one
            Gmsh.Clear();
            Gmsh.Model.Add("t3");
            Console.WriteLine("Model created");

            // Create basic geometry (copied from t1)
            double lc = 1e-2;
            Gmsh.Model.Geo.AddPoint(0, 0, 0, lc, 1);
            Gmsh.Model.Geo.AddPoint(.1, 0, 0, lc, 2);
            Gmsh.Model.Geo.AddPoint(.1, .3, 0, lc, 3);
            Gmsh.Model.Geo.AddPoint(0, .3, 0, lc, 4);
            Console.WriteLine("Points created");

            Gmsh.Model.Geo.AddLine(1, 2, 1);
            Gmsh.Model.Geo.AddLine(3, 2, 2);
            Gmsh.Model.Geo.AddLine(3, 4, 3);
            Gmsh.Model.Geo.AddLine(4, 1, 4);
            Console.WriteLine("Lines created");

            Gmsh.Model.Geo.AddCurveLoop(new int[] { 4, 1, -2, 3 }, 1);
            Gmsh.Model.Geo.AddPlaneSurface(new int[] { 1 }, 1);
            Console.WriteLine("Surface created");

            Gmsh.Model.Geo.Synchronize();
            Gmsh.Model.AddPhysicalGroup(1, new int[] { 1, 2, 4 }, 5);
            Gmsh.Model.AddPhysicalGroup(2, new int[] { 1 }, name: "My surface");
            Console.WriteLine("Physical groups added");

            // Simple extrude without layers (for now)
            double h = 0.1;
            Console.WriteLine("Starting extrude...");
            /* Unmerged change from project 'GmshSharp.Tutorials(net48)'
            Before:
                        var ov = Gmsh.Model.Geo.Extrude(new (int, int)[] { (2, 1) }, 0, 0, h);
            After:
                        (int dim, int tag)[] ov = Gmsh.Model.Geo.Extrude(new (int, int)[] { (2, 1) }, 0, 0, h);
            */

            (int dim, int tag)[] ov = Gmsh.Model.Geo.Extrude(new (int, int)[] { (2, 1) }, 0, 0, h);
            Console.WriteLine($"Extrude completed, returned {ov.Length} entities");

            Gmsh.Model.Geo.Synchronize();
            Console.WriteLine("Synchronized");

            // Define physical volume
            if (ov.Length > 0)
            {
                Console.WriteLine($"Creating physical group with tag {ov[ov.Length - 1].tag}");
                Gmsh.Model.AddPhysicalGroup(3, new int[] { ov[ov.Length - 1].tag }, 101);
            }

            Console.WriteLine("Generating 3D mesh...");
            Gmsh.Model.Mesh.Generate(3);
            Console.WriteLine("Writing mesh file...");
            Gmsh.Write("t3.msh");
            Console.WriteLine("T3 mesh generated successfully");
        }
    }
}
