# GTA Control Center (Modern)

A Windows-focused GTA San Andreas control center inspired by the classic GTA Control Center trainer.

This repository contains:

- a **modern .NET 8 WPF app** in `src/GtaControlCenter.App`
- **legacy compatibility/migration references** in `LegacySource`
- release/build assets for GitHub-based open source development

## Project overview

The modern app provides a clean navigation shell for gameplay control areas:

- Setup (install validation)
- Player
- Vehicles
- Weapons
- World
- Garage
- Runtime Cheats
- Configuration

Runtime-sensitive controls are intentionally **gated** and only enabled when the selected GTA SA install path and executable match supported checks.

## Safety and validation model

Before runtime features unlock, the app validates:

1. absolute installation path
2. safe path location (no Windows/system temp targets)
3. required game files/folders (`gta_sa.exe`, `data`, `models`)
4. supported executable build (version + file size allow-list)

If validation fails, runtime tabs remain locked.

## Local data handling

The app stores local state under:

`%LocalAppData%\\GtaControlCenter`

This includes:

- `settings.json` for install path persistence
- `Runtime/` for generated local runtime artifacts

No generated runtime files are written into the game install directory.

## Setup instructions

### Prerequisites

- Windows 10/11
- .NET 8 SDK (for development)

### Build locally

```powershell
dotnet restore GtaControlCenter.sln
dotnet build GtaControlCenter.sln -c Release
```

### Run (developer)

```powershell
dotnet run --project src/GtaControlCenter.App/GtaControlCenter.App.csproj
```

## Packaging releases

Framework-dependent and self-contained packages are produced by scripts in `scripts/`:

```powershell
pwsh ./scripts/build-package.ps1 -Version 0.1.0-beta.1
```

or:

```cmd
scripts\\build-package.cmd 0.1.0-beta.1
```

Artifacts are written to `artifacts/`.

## Legacy source preservation

`LegacySource/` is reserved for VB6-era project files, help files, DAT catalogs, and legacy assets. Keep legacy materials there as read-only references during migration.

## License

MIT. See [LICENSE](./LICENSE).
