using System;

namespace GmshSharp.Tutorials
{
    public class T5
    {
        public static void Run()
        {
            try
            {
                // Gmsh Python tutorial 5 - C# version - Simplified
                // Mesh sizes, basic cube with proper 3D meshing

                Gmsh.Initialize();
                Gmsh.Model.Add("t5");

                double lcar1 = .1;

                // Add 8 points for a simple cube
                Gmsh.Model.Geo.AddPoint(0, 0, 0, lcar1, 1);
                Gmsh.Model.Geo.AddPoint(1, 0, 0, lcar1, 2);
                Gmsh.Model.Geo.AddPoint(1, 1, 0, lcar1, 3);
                Gmsh.Model.Geo.AddPoint(0, 1, 0, lcar1, 4);
                Gmsh.Model.Geo.AddPoint(0, 0, 1, lcar1, 5);
                Gmsh.Model.Geo.AddPoint(1, 0, 1, lcar1, 6);
                Gmsh.Model.Geo.AddPoint(1, 1, 1, lcar1, 7);
                Gmsh.Model.Geo.AddPoint(0, 1, 1, lcar1, 8);

                // Add lines for the cube edges
                Gmsh.Model.Geo.AddLine(1, 2, 1);
                Gmsh.Model.Geo.AddLine(2, 3, 2);
                Gmsh.Model.Geo.AddLine(3, 4, 3);
                Gmsh.Model.Geo.AddLine(4, 1, 4);

                Gmsh.Model.Geo.AddLine(5, 6, 5);
                Gmsh.Model.Geo.AddLine(6, 7, 6);
                Gmsh.Model.Geo.AddLine(7, 8, 7);
                Gmsh.Model.Geo.AddLine(8, 5, 8);

                Gmsh.Model.Geo.AddLine(1, 5, 9);
                Gmsh.Model.Geo.AddLine(2, 6, 10);
                Gmsh.Model.Geo.AddLine(3, 7, 11);
                Gmsh.Model.Geo.AddLine(4, 8, 12);

                // Create surfaces for each face of the cube
                // Bottom face
                Gmsh.Model.Geo.AddCurveLoop(new int[] { 1, 2, 3, 4 }, 1);
                Gmsh.Model.Geo.AddPlaneSurface(new int[] { 1 }, 1);

                // Top face
                Gmsh.Model.Geo.AddCurveLoop(new int[] { 5, 6, 7, 8 }, 2);
                Gmsh.Model.Geo.AddPlaneSurface(new int[] { 2 }, 2);

                // Front face
                Gmsh.Model.Geo.AddCurveLoop(new int[] { 1, 10, -5, -9 }, 3);
                Gmsh.Model.Geo.AddPlaneSurface(new int[] { 3 }, 3);

                // Right face
                Gmsh.Model.Geo.AddCurveLoop(new int[] { 2, 11, -6, -10 }, 4);
                Gmsh.Model.Geo.AddPlaneSurface(new int[] { 4 }, 4);

                // Back face
                Gmsh.Model.Geo.AddCurveLoop(new int[] { 3, 12, -7, -11 }, 5);
                Gmsh.Model.Geo.AddPlaneSurface(new int[] { 5 }, 5);

                // Left face
                Gmsh.Model.Geo.AddCurveLoop(new int[] { 4, 9, -8, -12 }, 6);
                Gmsh.Model.Geo.AddPlaneSurface(new int[] { 6 }, 6);

                // Create surface loop and volume
                Gmsh.Model.Geo.AddSurfaceLoop(new int[] { 1, 2, 3, 4, 5, 6 }, 1);
                Gmsh.Model.Geo.AddVolume(new int[] { 1 }, 1);

                Gmsh.Model.Geo.Synchronize();

                // Define a physical volume for the elements discretizing the cube
                Gmsh.Model.AddPhysicalGroup(3, new int[] { 1 }, 1);

                // Meshing algorithms can be changed globally using options
                Gmsh.Option.SetNumber("Mesh.Algorithm", 6); // Frontal-Delaunay for 2D meshes

                // They can also be set for individual surfaces
                Gmsh.Model.Mesh.SetAlgorithm(2, 1, 1);

                Gmsh.Model.Mesh.Generate(3);
                Gmsh.Write("t5.msh");

                Console.WriteLine("T5 tutorial completed successfully");
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
