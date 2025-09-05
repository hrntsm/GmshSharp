use csbindgen::Builder;

fn main() {
    let result = Builder::default()
        .input_bindgen_file("src/bindings.rs")
        .csharp_dll_name("gmsh")
        .csharp_namespace("GmshSharp.Native")
        .csharp_class_name("NativeMethods")
        .generate_csharp_file("./NativeMethods.cs");

    match result {
        Ok(_) => println!("Generated C# bindings at: ./NativeMethods.cs"),
        Err(e) => println!("Error generating C# bindings: {}", e),
    }
}
