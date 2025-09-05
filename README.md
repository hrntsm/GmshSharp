# GmshSharp

A comprehensive C# wrapper for the Gmsh mesh generation library, providing high-level APIs for creating finite element meshes programmatically.

## Overview

GmshSharp is a .NET binding for [Gmsh](https://gmsh.info/), a three-dimensional finite element mesh generator with a built-in CAD engine and post-processor. This library provides a native C# interface to Gmsh's powerful mesh generation capabilities, making it easy to create complex geometries and meshes directly from your .NET applications.

## Features

- **Complete Gmsh API Coverage**: Full access to Gmsh's geometry, meshing, and post-processing capabilities
- **Multi-Target Support**: Compatible with .NET 8.0 and .NET Framework 4.8
- **Type Safety**: Strongly-typed C# API with nullable reference types support (.NET 8.0)
- **Memory Safe**: Proper resource management and memory handling
- **Cross-Platform**: Works on Windows, macOS, and Linux
- **Tutorial Examples**: Complete C# ports of Gmsh's official Python tutorials (T1-T5)

## Project Structure

```
GmshSharp/
├── GmshSharp.Native/     # Low-level P/Invoke bindings to native Gmsh library
├── GmshSharp.Core/       # High-level C# API and abstractions
└── GmshSharp.Tutorials/  # Example applications and tutorials
```

## Quick Start

### Prerequisites

- .NET 8.0 SDK or .NET Framework 4.8
- Native Gmsh libraries (included in `native-libs/` directory)

**Important:**

This project targets Gmsh version **4.14**. You must place the appropriate native library files for Gmsh 4.14 (`.dll` for Windows, `.dylib` for macOS, `.so` for Linux) into the `native-libs/gmsh_lib/` directory. Only the required library for your platform is necessary. The repository does not include these files by default; please obtain them from the [official Gmsh 4.14 releases](https://gmsh.info/) or your system package manager. These files are included in the Gmsh SDK distribution for each platform.

The `native-libs/gmsh_lib/` directory is git-ignored except for a `.gitkeep` file, so you need to add the native library manually after cloning the repository.

### Basic Usage

```csharp
using GmshSharp.Tutorials;

// Initialize Gmsh
Gmsh.Initialize();

// Create a new model
Gmsh.Model.Add("example");

// Define mesh size
double lc = 1e-2;

// Create geometry
var p1 = Gmsh.Model.Geo.AddPoint(0, 0, 0, lc);
var p2 = Gmsh.Model.Geo.AddPoint(1, 0, 0, lc);
var p3 = Gmsh.Model.Geo.AddPoint(1, 1, 0, lc);
var p4 = Gmsh.Model.Geo.AddPoint(0, 1, 0, lc);

// Create lines
var l1 = Gmsh.Model.Geo.AddLine(p1, p2);
var l2 = Gmsh.Model.Geo.AddLine(p2, p3);
var l3 = Gmsh.Model.Geo.AddLine(p3, p4);
var l4 = Gmsh.Model.Geo.AddLine(p4, p1);

// Create surface
var curveLoop = Gmsh.Model.Geo.AddCurveLoop(new[] { l1, l2, l3, l4 });
var surface = Gmsh.Model.Geo.AddPlaneSurface(new[] { curveLoop });

// Synchronize and generate mesh
Gmsh.Model.Geo.Synchronize();
Gmsh.Model.Mesh.Generate(2);

// Save mesh
Gmsh.Write("output.msh");

// Cleanup
Gmsh.Shutdown();
```

### Running Tutorials

The project includes complete C# implementations of Gmsh's official tutorials:

```csharp
// Run individual tutorials
T1.Run(); // Basic geometry and meshing
T2.Run(); // Transformations, extrusions, volumes
T3.Run(); // Extruded meshes, ONELAB parameters
T4.Run(); // Built-in CAD kernel features
T5.Run(); // Mesh sizes, fields
```

Or run all tutorials:

```bash
cd GmshSharp/GmshSharp.Tutorials
dotnet run
```

## Building from Source

1. Clone the repository:

   ```bash
   git clone https://github.com/yourusername/GmshSharp.git
   cd GmshSharp
   ```

2. Build the solution:

   ```bash
   dotnet build GmshSharp/GmshSharp.sln
   ```

3. Run the tutorials:
   ```bash
   dotnet run --project GmshSharp/GmshSharp.Tutorials
   ```

## API Documentation

### Core Classes

- **`Gmsh`**: Main entry point for Gmsh operations
- **`Gmsh.Model`**: Model management and operations
- **`Gmsh.Model.Geo`**: Built-in CAD kernel for geometry creation
- **`Gmsh.Model.Mesh`**: Mesh generation and manipulation
- **`Gmsh.View`**: Post-processing and visualization

### Key Methods

- `Gmsh.Initialize()`: Initialize the Gmsh library
- `Gmsh.Model.Add(name)`: Create a new model
- `Gmsh.Model.Geo.AddPoint(x, y, z, meshSize)`: Add a point
- `Gmsh.Model.Geo.AddLine(startPoint, endPoint)`: Add a line
- `Gmsh.Model.Geo.Synchronize()`: Synchronize CAD model
- `Gmsh.Model.Mesh.Generate(dim)`: Generate mesh (1D, 2D, or 3D)
- `Gmsh.Write(filename)`: Export mesh to file
- `Gmsh.Shutdown()`: Clean up resources

## Supported Platforms

- **Windows**: x64, x86
- **macOS**: x64, ARM64
- **Linux**: x64, ARM64

Native libraries for all platforms are included in the `native-libs/` directory.

## Examples and Tutorials

The `GmshSharp.Tutorials` project contains complete implementations of Gmsh's official tutorials:

- **T1**: Basic geometry creation, elementary entities, physical groups
- **T2**: Transformations, extrusions, volumes
- **T3**: Extruded meshes, ONELAB parameters
- **T4**: Built-in CAD kernel features, boolean operations
- **T5**: Mesh sizes, characteristic lengths, background fields

## Contributing

Contributions are welcome! Please feel free to submit issues, feature requests, or pull requests.

## Acknowledgments

- [Gmsh development team](https://gmsh.info/) for creating the excellent mesh generation library
- Built with inspiration from Gmsh's official Python API
