# LFTFC — Changelog

All notable changes to LFTFC_Pro. Entries are listed newest first. Dates use ISO format (YYYY-MM-DD).
## v4.04 — 2026-04-15
- Fix: Resolved unexpecteded error in 'DistGraph' when viewing the distribution line graphs.
- Includes earlier changes.
- 
## v4.03 — 2025-12-23
- Fix: Resolve error in `SetRasterValues` when the ArcGIS Pro container has no standalone tables to remove (ArcGIS Pro 3.5).
- Includes earlier changes.

## v4.02 — 2024-08-13
- Fix: Resolve an error that appears after `AddJoin` during fuel raster creation (ArcGIS Pro 3.3).
- 2024-07-15: Clip topographic rasters to the extent of required grids even if the "Limit the extent of the MU to the map view" setting is not checked; helps match extents across rasters.
- Includes earlier changes.

## v4.01 — 2024-04-02
- Fix: FCCS outputs exported as 32-bit signed raster (previously 16-bit).
- 2024-03-14: Clear Table of Contents before creating fuel rasters to avoid using the wrong MU for calculations.
- 2024-03-05: Added custom fuel model selection/creation in the "Compare Fuel Model" tab.
- 2024-01-09: Fix to `gs_validProject` method (corrected earlier change that set it to true accidentally).
- 2024-01-05: Fix for valid project path handling.
- 2023-11-27: Migrated LFTFC from ArcMap 10+ to ArcGIS Pro 3.1+.

## v4.00 — baseline
- Baseline for 4.x releases.
