<p align="center">
  <img src="assets/branding/GTASAControlCenter.ico" alt="GTASA CONTROL CENTER icon" width="96" height="96">
</p>

<h1 align="center">GTASA CONTROL CENTER</h1>

<p align="center">
  A modern Windows control center for GTA San Andreas, rebuilt from the classic VB6-era trainer source.
</p>

<p align="center">
  <img src="assets/banner/GTA_SA_CA_BANNER.png" alt="GTASA CONTROL CENTER banner" width="100%">
</p>

GTASA CONTROL CENTER is a modern Windows-focused rewrite of the classic GTA San Andreas Control Center trainer/workbench. The active app is a .NET 8 WPF project in `ModernShell`, while the original VB6-era source, DAT files, help files, and images are preserved under `LegacySource`.

## Beta Status

This repository is currently preparing the first public beta. Beta 2 is the polished release candidate for that guarded beta: the supported lanes are intentionally narrow, diagnostics-heavy, and labeled when experimental or deferred.

See [BETA_RELEASE_NOTES.md](./BETA_RELEASE_NOTES.md) for the current release scope, retest checklist, and known limitations.

## Requirements

- Windows 10/11.
- GTA San Andreas with a supported v1.0-family executable profile.
- .NET 8 SDK for development builds.
- For framework-dependent release builds, users need the .NET 8 Desktop Runtime.

For custom GTA SA installs, open `Setup > GTA Install Path` and save either the `gta_sa.exe` path or the game folder. GTASA CONTROL CENTER also tries to resolve the executable from a running `gta_sa.exe` process before falling back to registry/common-folder detection.

## Repository Layout

- `ModernShell/`: active .NET 8 WPF application.
- `LegacySource/`: preserved original VB6 source, legacy DAT/INI files, images, help files, and migration reference material.
- `scripts/`: release/build helper scripts.
- `BETA_RELEASE_NOTES.md`: Beta 2 scope and known limitations.
- `FEATURE_CHECKLIST.md`: feature parity and beta readiness tracker.
- `MIGRATION_NOTES.md`: detailed migration history and implementation notes.

## Build

```powershell
dotnet build .\ModernShell\GTACenter.Modern.csproj
```

## Package A Beta Build

Framework-dependent package:

```powershell
.\scripts\package-beta.ps1
```

Single-file self-contained Windows x64 package:

```powershell
.\scripts\package-beta.ps1 -SelfContained
```

The framework-dependent package is smaller but requires the .NET 8 Desktop Runtime. The self-contained package is larger, but it publishes the app as a cleaner single-file executable beside `LegacySource` and does not require users to install .NET separately. It may need NuGet access the first time Windows runtime packs are restored.

Packages are written under `artifacts\releases`.

Release packages include the tracked `LegacySource` DAT/help/image assets required by Setup checks and catalog parsing, while ignored legacy executables/installers/backups stay local-only.

## Release Checklist And CI

Use [RELEASE_CHECKLIST.md](./RELEASE_CHECKLIST.md) before publishing a GitHub pre-release.

GitHub Actions builds framework-dependent and single-file self-contained Windows x64 packages on pushes, pull requests, manual workflow runs, and `v*` tags. The build workflow uploads zip files as build artifacts.

When a `v*` tag is pushed, the release workflow also packages both build styles and creates or updates a draft GitHub prerelease with the generated zip files attached. Keep the release as a draft until the Beta checklist has been manually signed off.

## Future Update Checks

The Release Notes page includes a safe update check. It compares the current app version to the latest non-draft GitHub Release and shows a release-page link only; it does not silently download or install trainer binaries.

The built-in update source is `yyppsk/GTA-SA-Control-Center`. For local validation against a fork or private test repository, override it with this environment variable:

```powershell
$env:GTASA_CONTROL_CENTER_GITHUB_REPOSITORY = "owner/repository"
```

## Safety Notes

- Live runtime features read/write GTA SA process memory and are guarded by executable fingerprint checks.
- Custom install paths are stored locally under `%LOCALAPPDATA%\GTASA Control Center\GTAExecutablePath.txt` and are not committed to the repository.
- Experimental watercraft, aircraft, rail, and other risky lanes should be tested carefully and one at a time.
- Deferred lanes such as visible license plate texture refresh, One-Hit-Kill, Freeze Timers, standalone trailer support, RC runtime support, and true in-place current-vehicle replacement are intentionally outside Beta 2.

## Legacy Source

The old application has not been thrown away. `LegacySource` remains the source of truth for original DAT files, VB6 behavior, help documentation, and migration reference material. GTASA CONTROL CENTER detects this folder as the legacy asset root during development.

## License

See [LICENSE](./LICENSE). The modern rewrite is MIT licensed. `LegacySource` is preserved under its original open-source notices and excluded compiled/archive artifacts are kept out of the public source repository.
