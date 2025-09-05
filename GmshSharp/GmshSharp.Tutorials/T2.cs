using System;

namespace GmshSharp.Tutorials
{
    public class T2
    {
        public static void Run()
        {
            try
            {
                // Gmsh Python tutorial 2 - C# version
                // Transformations, extruded geometries, volumes

                // Clear existing model and add new one named "t2"
                Gmsh.Clear();
                Gmsh.Model.Add("t2");
                Console.WriteLine("Model 't2' added");

                // Define mesh size (copied from t1.py)
                double lc = 1e-2;

                // Create initial rectangle (copied from t1.py)
                Gmsh.Model.Geo.AddPoint(0, 0, 0, lc, 1);
                Gmsh.Model.Geo.AddPoint(.1, 0, 0, lc, 2);
                Gmsh.Model.Geo.AddPoint(.1, .3, 0, lc, 3);
                Gmsh.Model.Geo.AddPoint(0, .3, 0, lc, 4);
                Gmsh.Model.Geo.AddLine(1, 2, 1);
                Gmsh.Model.Geo.AddLine(3, 2, 2);
                Gmsh.Model.Geo.AddLine(3, 4, 3);
                Gmsh.Model.Geo.AddLine(4, 1, 4);
                Gmsh.Model.Geo.AddCurveLoop(new int[] { 4, 1, -2, 3 }, 1);
                Gmsh.Model.Geo.AddPlaneSurface(new int[] { 1 }, 1);
                Gmsh.Model.Geo.Synchronize();
                Console.WriteLine("Initial rectangle geometry created");

                // Add new points and curves
                Gmsh.Model.Geo.AddPoint(0, .4, 0, lc, 5);
                Gmsh.Model.Geo.AddLine(4, 5, 5);

                // Transform point 5: translate by -0.02 in x direction
                Gmsh.Model.Geo.Translate(new (int, int)[] { (0, 5) }, -0.02, 0, 0);

                // Rotate point 5 by -π/4 around (0, 0.3, 0) along z axis
                Gmsh.Model.Geo.Rotate(new (int, int)[] { (0, 5) }, 0, 0.3, 0, 0, 0, 1, -Math.PI / 4);
                Console.WriteLine("Point 5 transformed (translated and rotated)");

                // Copy point 3 and translate the copy
                (int dim, int tag)[] ov = Gmsh.Model.Geo.Copy(new (int, int)[] { (0, 3) });
                Console.WriteLine($"Copy returned {ov.Length} entities");
                if (ov.Length > 0)
                {
                    Gmsh.Model.Geo.Translate(ov, 0, 0.05, 0);
                    Console.WriteLine($"Point 3 copied, new point tag: {ov[0].tag}");
                }
                else
                {
                    Console.WriteLine("Warning: Copy returned no entities, creating point 6 manually");
                    // Manually create point 6 as alternative
                    Gmsh.Model.Geo.AddPoint(.1, .35, 0, lc, 6);
                }

                // Create new lines using the copied or created point
                int newPointTag = ov.Length > 0 ? ov[0].tag : 6;
                Gmsh.Model.Geo.AddLine(3, newPointTag, 7);
                Gmsh.Model.Geo.AddLine(newPointTag, 5, 8);
                Gmsh.Model.Geo.AddCurveLoop(new int[] { 5, -8, -7, 3 }, 10);
                Gmsh.Model.Geo.AddPlaneSurface(new int[] { 10 }, 11);
                Console.WriteLine("Additional surface created");

                // Copy the two surfaces and translate them to the right
                (int dim, int tag)[] ov_surfaces = Gmsh.Model.Geo.Copy(new (int, int)[] { (2, 1), (2, 11) });
                Gmsh.Model.Geo.Translate(ov_surfaces, 0.12, 0, 0);
                Console.WriteLine($"New surfaces {ov_surfaces[0].tag} and {ov_surfaces[1].tag}");

                // Create points for the volume
                Gmsh.Model.Geo.AddPoint(0.0, 0.3, 0.12, lc, 100);
                Gmsh.Model.Geo.AddPoint(0.1, 0.3, 0.12, lc, 101);
                Gmsh.Model.Geo.AddPoint(0.1, 0.35, 0.12, lc, 102);

                // Get coordinates of point 5 to create point 103
                Gmsh.Model.Geo.Synchronize();
                double[] xyz = Gmsh.Model.GetValue(0, 5);
                Gmsh.Model.Geo.AddPoint(xyz[0], xyz[1], 0.12, lc, 103);
                Console.WriteLine($"Point 103 created at coordinates ({xyz[0]}, {xyz[1]}, 0.12)");

                // Create lines for the volume
                Gmsh.Model.Geo.AddLine(4, 100, 110);
                Gmsh.Model.Geo.AddLine(3, 101, 111);
                Gmsh.Model.Geo.AddLine(newPointTag, 102, 112);
                Gmsh.Model.Geo.AddLine(5, 103, 113);
                Gmsh.Model.Geo.AddLine(103, 100, 114);
                Gmsh.Model.Geo.AddLine(100, 101, 115);
                Gmsh.Model.Geo.AddLine(101, 102, 116);
                Gmsh.Model.Geo.AddLine(102, 103, 117);

                // Create surfaces for the volume
                Gmsh.Model.Geo.AddCurveLoop(new int[] { 115, -111, 3, 110 }, 118);
                Gmsh.Model.Geo.AddPlaneSurface(new int[] { 118 }, 119);
                Gmsh.Model.Geo.AddCurveLoop(new int[] { 111, 116, -112, -7 }, 120);
                Gmsh.Model.Geo.AddPlaneSurface(new int[] { 120 }, 121);
                Gmsh.Model.Geo.AddCurveLoop(new int[] { 112, 117, -113, -8 }, 122);
                Gmsh.Model.Geo.AddPlaneSurface(new int[] { 122 }, 123);
                Gmsh.Model.Geo.AddCurveLoop(new int[] { 114, -110, 5, 113 }, 124);
                Gmsh.Model.Geo.AddPlaneSurface(new int[] { 124 }, 125);
                Gmsh.Model.Geo.AddCurveLoop(new int[] { 115, 116, 117, 114 }, 126);
                Gmsh.Model.Geo.AddPlaneSurface(new int[] { 126 }, 127);

                // Create surface loop and volume
                Gmsh.Model.Geo.AddSurfaceLoop(new int[] { 127, 119, 121, 123, 125, 11 }, 128);
                Gmsh.Model.Geo.AddVolume(new int[] { 128 }, 129);
                Console.WriteLine("Manual volume created");

                // Extrude surface 11 to create another volume automatically
                (int dim, int tag)[] ov2 = Gmsh.Model.Geo.Extrude(new (int, int)[] { ov_surfaces[1] }, 0, 0, 0.12);
                Console.WriteLine($"Surface extruded, new volume tag: {ov2[ov2.Length - 1].tag}");

                // Set mesh sizes for specific points (simplified to avoid non-existent tags)
                Gmsh.Model.Geo.Mesh.SetSize(new (int, int)[] {
                    (0, 103), (0, 102), (0, newPointTag), (0, 5)
                }, lc * 3);
                Console.WriteLine("Mesh sizes set for specific points");

                // Synchronize the data from the built-in CAD kernel
                Gmsh.Model.Geo.Synchronize();
                Console.WriteLine("Geometry synchronized");

                // Generate 3D mesh and save
                Gmsh.Model.Mesh.Generate(3);
                Console.WriteLine("3D mesh generated");

                Gmsh.Write("t2.msh");
                Console.WriteLine("Mesh saved to t2.msh");

                Console.WriteLine("T2 tutorial completed successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }
    }
}
