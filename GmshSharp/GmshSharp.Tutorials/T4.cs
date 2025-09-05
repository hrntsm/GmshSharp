using System;

namespace GmshSharp.Tutorials
{
    public class T4
    {
        public static void Run()
        {
            try
            {
                // Gmsh Python tutorial 4 - C# version
                // Holes in surfaces, annotations, entity colors

                Gmsh.Initialize();

                Gmsh.Model.Add("t4");

                double cm = 1e-02;
                double e1 = 4.5 * cm;
                double e2 = 6 * cm / 2;
                double e3 = 5 * cm / 2;
                double h1 = 5 * cm;
                double h2 = 10 * cm;
                double h3 = 5 * cm;
                double h4 = 2 * cm;
                double h5 = 4.5 * cm;
                double R1 = 1 * cm;
                double R2 = 1.5 * cm;
                double r = 1 * cm;
                double Lc1 = 0.01;
                double Lc2 = 0.003;

                static double hypot(double a, double b)
                {
                    return Math.Sqrt(a * a + b * b);
                }

                double ccos = (-h5 * R1 + e2 * hypot(h5, hypot(e2, R1))) / (h5 * h5 + e2 * e2);
                double ssin = Math.Sqrt(1 - ccos * ccos);

                // Add points directly using static methods
                Gmsh.Model.Geo.AddPoint(-e1 - e2, 0, 0, Lc1, 1);
                Gmsh.Model.Geo.AddPoint(-e1 - e2, h1, 0, Lc1, 2);
                Gmsh.Model.Geo.AddPoint(-e3 - r, h1, 0, Lc2, 3);
                Gmsh.Model.Geo.AddPoint(-e3 - r, h1 + r, 0, Lc2, 4);
                Gmsh.Model.Geo.AddPoint(-e3, h1 + r, 0, Lc2, 5);
                Gmsh.Model.Geo.AddPoint(-e3, h1 + h2, 0, Lc1, 6);
                Gmsh.Model.Geo.AddPoint(e3, h1 + h2, 0, Lc1, 7);
                Gmsh.Model.Geo.AddPoint(e3, h1 + r, 0, Lc2, 8);
                Gmsh.Model.Geo.AddPoint(e3 + r, h1 + r, 0, Lc2, 9);
                Gmsh.Model.Geo.AddPoint(e3 + r, h1, 0, Lc2, 10);
                Gmsh.Model.Geo.AddPoint(e1 + e2, h1, 0, Lc1, 11);
                Gmsh.Model.Geo.AddPoint(e1 + e2, 0, 0, Lc1, 12);
                Gmsh.Model.Geo.AddPoint(e2, 0, 0, Lc1, 13);

                Gmsh.Model.Geo.AddPoint(R1 / ssin, h5 + R1 * ccos, 0, Lc2, 14);
                Gmsh.Model.Geo.AddPoint(0, h5, 0, Lc2, 15);
                Gmsh.Model.Geo.AddPoint(-R1 / ssin, h5 + R1 * ccos, 0, Lc2, 16);
                Gmsh.Model.Geo.AddPoint(-e2, 0.0, 0, Lc1, 17);

                Gmsh.Model.Geo.AddPoint(-R2, h1 + h3, 0, Lc2, 18);
                Gmsh.Model.Geo.AddPoint(-R2, h1 + h3 + h4, 0, Lc2, 19);
                Gmsh.Model.Geo.AddPoint(0, h1 + h3 + h4, 0, Lc2, 20);
                Gmsh.Model.Geo.AddPoint(R2, h1 + h3 + h4, 0, Lc2, 21);
                Gmsh.Model.Geo.AddPoint(R2, h1 + h3, 0, Lc2, 22);
                Gmsh.Model.Geo.AddPoint(0, h1 + h3, 0, Lc2, 23);

                Gmsh.Model.Geo.AddPoint(0, h1 + h3 + h4 + R2, 0, Lc2, 24);
                Gmsh.Model.Geo.AddPoint(0, h1 + h3 - R2, 0, Lc2, 25);

                Gmsh.Model.Geo.AddLine(1, 17, 1);
                Gmsh.Model.Geo.AddLine(17, 16, 2);

                // Define a new circle arc, starting at point 14 and ending at point 16,
                // with the circle's center being point 15
                Gmsh.Model.Geo.AddCircleArc(14, 15, 16, 3);

                // Additional lines and circles
                Gmsh.Model.Geo.AddLine(14, 13, 4);
                Gmsh.Model.Geo.AddLine(13, 12, 5);
                Gmsh.Model.Geo.AddLine(12, 11, 6);
                Gmsh.Model.Geo.AddLine(11, 10, 7);
                Gmsh.Model.Geo.AddCircleArc(8, 9, 10, 8);
                Gmsh.Model.Geo.AddLine(8, 7, 9);
                Gmsh.Model.Geo.AddLine(7, 6, 10);
                Gmsh.Model.Geo.AddLine(6, 5, 11);
                Gmsh.Model.Geo.AddCircleArc(3, 4, 5, 12);
                Gmsh.Model.Geo.AddLine(3, 2, 13);
                Gmsh.Model.Geo.AddLine(2, 1, 14);
                Gmsh.Model.Geo.AddLine(18, 19, 15);
                Gmsh.Model.Geo.AddCircleArc(21, 20, 24, 16);
                Gmsh.Model.Geo.AddCircleArc(24, 20, 19, 17);
                Gmsh.Model.Geo.AddCircleArc(18, 23, 25, 18);
                Gmsh.Model.Geo.AddCircleArc(25, 23, 22, 19);
                Gmsh.Model.Geo.AddLine(21, 22, 20);

                Gmsh.Model.Geo.AddCurveLoop(new int[] { 17, -15, 18, 19, -20, 16 }, 21);
                Gmsh.Model.Geo.AddPlaneSurface(new int[] { 21 }, 22);

                // Define the exterior surface with a hole
                // Since this surface has a hole, its definition requires two curve loops
                Gmsh.Model.Geo.AddCurveLoop(new int[] { 11, -12, 13, 14, 1, 2, -3, 4, 5, 6, 7, -8, 9, 10 }, 23);
                Gmsh.Model.Geo.AddPlaneSurface(new int[] { 23, 21 }, 24);

                Gmsh.Model.Geo.Synchronize();

                // Note: View and color setting APIs are not available in this version
                // The geometry and mesh generation will still work correctly

                Gmsh.Model.Mesh.Generate(2);

                Gmsh.Write("t4.msh");

                Console.WriteLine("T4 tutorial completed successfully");
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
    }
}
