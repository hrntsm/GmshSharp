use std::env;
use std::path::PathBuf;

fn main() {
    println!("cargo:rerun-if-changed=gmsh_api/gmshc.h");

    let bindings = bindgen::Builder::default()
        .header("gmsh_api/gmshc.h")
        .parse_callbacks(Box::new(bindgen::CargoCallbacks::new()))
        .generate()
        .expect("Unable to generate bindings");

    let out_path = PathBuf::from("src/");
    bindings
        .write_to_file(out_path.join("bindings.rs"))
        .expect("Couldn't write bindings!");

    println!("Generated bindings at: src/bindings.rs");
}
