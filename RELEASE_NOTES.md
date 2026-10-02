# GTA SA Control Center - Beta Release Notes

## Scope in this beta

- Modern .NET 8 WPF desktop shell with structured sections:
  - Setup
  - Player
  - Vehicles
  - Weapons
  - World
  - Garage
  - Runtime Cheats
  - Configuration
- Installation safety checks for GTA San Andreas:
  - absolute path requirement
  - system/temp path blocking
  - required `gta_sa.exe`, `data`, and `models` validation
  - executable build/version gate for runtime feature enablement
- Local app data persistence under `%LocalAppData%\\GtaControlCenter`
- Legacy source preservation under `LegacySource/`
- Packaging scripts for framework-dependent and self-contained Windows releases

## Known limitations

- Runtime tabs are currently scaffolded and gated; gameplay memory editing operations are not yet implemented.
- Only curated GTA SA executable builds are currently marked as runtime-compatible.
- Linux/macOS are not targeted; this project is Windows focused.

## Packaging steps

Use one of the following from repository root:

```powershell
pwsh ./scripts/build-package.ps1 -Version 0.1.0-beta.1
```

or on Windows CMD:

```cmd
scripts\\build-package.cmd 0.1.0-beta.1
```

Artifacts are produced in `artifacts/`:

- `framework-dependent/`
- `self-contained-win-x64/`
- zipped archives for both outputs
