<img width="1712" height="488" alt="image" src="https://github.com/user-attachments/assets/7e85b3c4-6016-424b-881d-04c1fc777f5a" />

# LFTFC
Source code for the LANDFIRE Total Fuel Change ArcGISPro Addin toolbar

Information and compiled ArcPro Addin available at: [LANDFIRE Resources](https://landfire.gov/resources/lftfc)

## Changelog

See the full release history and detailed updates in [UPDATES.md](UPDATES.md).

## License

See the [LICENSE](LICENSE) file for licensing information.

## Directories:
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
   > A) Start a new instance of Visual Studio 2022
   > B) Click "Clone a repository" <img width="374" height="127" alt="image" src="https://github.com/user-attachments/assets/1f8c28f6-3b86-44df-ba4c-272bdf3cbdd3" />
   C) Set Repository location to: https://github.com/firelab/LFTFC and Set Path to a new folder location <img width="633" height="255" alt="image" src="https://github.com/user-attachments/assets/85ef9f78-d02f-462a-a6ec-a5c2df831af8" />
   D) Click "Clone" <img width="266" height="108" alt="image" src="https://github.com/user-attachments/assets/fe1e4582-187d-401b-855d-d037d47e72af" />
2. Open the Visual Studio solution in Visual Studio 2022.
3. In the solution, ensure the ArcGIS Pro SDK packages and references resolve. Install the ArcGIS Pro SDK if missing.
4. Add manifest file. (Right-click "My Project" folder > Add > New Item > Select "Application Manifest File" > Click "Add" Button. 
5. Build the add-in by Right-clicking Solution > Build Solution.
6. To Run ArcGISPro in Debug.
   > A) Select the dropdown > B) LFTFC_Pro Debug Properties <img width="257" height="156" alt="image" src="https://github.com/user-attachments/assets/7b859952-e729-46ea-ba6e-d1cbb6169f40" />
   B) Create a new profile <img width="166" height="99" alt="image" src="https://github.com/user-attachments/assets/4e2380ad-339c-4a4b-93a2-04b2daaf022f" />
   > C) Executable <img width="144" height="115" alt="image" src="https://github.com/user-attachments/assets/7c232d6b-d794-410e-9bed-b4ce318ec458" />
   D) Rename the new profile if you want. <img width="243" height="175" alt="image" src="https://github.com/user-attachments/assets/6dc01173-2efb-4c50-984c-0166cbd6a274" />
   E) Browse to set ArcGISPro.exe as the executable and close <img width="419" height="89" alt="image" src="https://github.com/user-attachments/assets/49934dfd-343e-4c56-9ec3-2f7d4cc5da5e" />
   F) Select the new profile <img width="246" height="161" alt="image" src="https://github.com/user-attachments/assets/d2ef461f-b66f-4661-8b0c-01c0dab10821" />
   G) Click the profile to start and run ArcGISPro and debugger

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

