# LFTFC
Source code for the LANDFIRE Total Fuel Change Tool

Information and compiled ArcPro Addin available at: [LANDFIRE Resources](https://landfire.gov/resources/lftfc)

## Changelog

See the full release history and detailed updates in [UPDATES.md](UPDATES.md).

## License

See the [LICENSE](LICENSE) file for licensing information.

## Directories:
- libs -> dynamic link librarys (DDLs)
- images -> images for icons
- src -> source files

## Getting Started

### Prerequisites

Before you begin, ensure you have the following software installed:

- **ArcGIS Pro**: Ensure you have a compatible version for the ArcGIS Pro SDK used in this repository.
- **ArcGIS Pro SDK for .NET**: This should be installed in Visual Studio.
- **Visual Studio 2022**: Make sure you have the VB.NET workload installed.
- **Microsoft Access (64-bit)**: Required for Access database (`LF_TFC_Toolbar.mdb`) and to run features that manipulate the MDB directly.
- **The current Access Database can be found at**: [LANDFIRE Resources](https://landfire.gov/resources/lftfc) in the setup file.

### Build

To build the add-in, follow these steps:

1. Clone the repository.
2. Open the Visual Studio solution in Visual Studio 2022.
3. In the solution, ensure the ArcGIS Pro SDK packages and references resolve. Install the ArcGIS Pro SDK if missing.
4. Set the project Platform target to x64 when projects interact with the Access database (open __Project > Properties > Build__ and set __Platform target__ to __x64__).
5. Build the add-in by selecting __Build > Build Solution__.


### Run / Use

To run and use the add-in, follow these steps:

1. Install or start ArcGIS Pro and enable the built add-in (if required by your development workflow).
2. From the add-in toolbar, set a working project directory using the "Set Working Directory" button. The add-in will create the following subfolders in the project path if they are missing: `Input`, `MU`, `Output`, and it will copy `LF_TFC_Toolbar.mdb`.
3. Add Management Units (MU) via the UI; generated grids and outputs are written into the project's `MU` and `Output` folders.
4. Use the various tool functions to manipulate and analyze fuel data as needed.
5. Information and compiled ArcPro Addin available at: [LANDFIRE Resources](https://landfire.gov/resources/lftfc)

### Project Layout and Important Files

The project structure includes the following important files and directories:

- `LF_TFC_Toolbar.mdb` — Project Access database used by the tool (copied into new projects from the install location).
- `MU/` — Management unit rasters and generated MU-specific files.
- `Input/` — Optional input rasters referenced when creating new MUs.
- `Output/` — Exported rasters, logs, and reports produced by the add-in.

### Notes

- The implementation uses ADODB to read/write the Access MDB and relies on Access being available for certain operations. On developer machines, use the 64-bit Access components to match the add-in process.
- The install path constant in the code is `C:\Landfire\LFTFC_Pro`. Some helper routines reference this path; verify and update if needed.

LFTFC Process Image
<img width="1004" height="505" alt="image" src="https://github.com/user-attachments/assets/de12e9c2-efdd-48de-a426-20b02c63521f" />

