# GTASA CONTROL CENTER - Beta 2 Release Notes Draft

Date prepared: 2026-04-25

## Release Goal

Beta 2 is the polished first public beta candidate for GTASA CONTROL CENTER, the new .NET 8 WPF version of the classic trainer/control-center app. Its purpose is to validate the new app structure, Windows-safe data handling, executable fingerprint guardrails, legacy data parsers, and the live runtime lanes that have already been tested during migration.

This is not final VB6 feature parity yet. Risky injector-heavy lanes and memory behaviors that destabilized the game are intentionally deferred.

## Supported Beta Runtime

- GTA San Andreas on the currently mapped v1.0-family executable profile.
- Modern Windows desktop systems with the .NET 8 desktop runtime/SDK available.
- Live memory features should only unlock when Setup reports the supported executable/fingerprint/runtime support.

## Included In Beta 2

- GTASA CONTROL CENTER app UI with dedicated Home, Runtime, Dynamics, Player, Weapons, Live Cheats, World, Vehicles, Garage, Testing, Navigation, Release Notes, and Setup pages.
- Setup checks for install detection, data roots, process access, executable support, and legacy data parsing.
- Setup install-path override for custom GTA SA folders, including Browse, Use Running GTA, Save Path, Clear Path, and copy/save report actions.
- Writable Windows-safe app-data migration while preserving bundled legacy assets.
- Original VB6-era project files and bundled assets are archived under `LegacySource`, while the active app remains in `ModernShell`.
- Parsed legacy vehicles, locations, markups, cheats, shortcuts, garage data, and console settings.
- Guarded on-foot warps, vehicle warps, risk warnings, recovery buffer, and named test targets.
- Confirmed vehicle runtime lanes: repair, stop motion, set upright, direction, kickstart, degree turns, torch actions, paint live, spawn live, car dynamics, wheel visual repair, and last/previous vehicle utilities.
- Garage staging, legacy slot writeback, live garage site writes, Johnson's House live write fix, and garage payload sanitization.
- Player stats, wanted level, girlfriend progress, opened-islands unlock, on-foot motion controls, and implemented proof-bit lane.
- Weapon proficiency, ammo, slot replacement, and session weapon lock lanes.
- Simple live cheat toggles, per-cheat session locks, clear cheated status/count, editable cheat strings, and guarded cheat-string typing.
- Weather, game time, clock speed, and game-speed controls.
- Navigation editing for custom warps, cheats, shortcut metadata, active state, guarded delete, and inactive cheat/warp shortcut creation.

## Beta Retest Checklist

- Re-verify all safehouse/site live garage writes beyond Johnson's House.
- Retest player proof/speciality bits and session lock behavior.
- Retest direct respect/sex appeal behavior, especially visible sex appeal versus raw readback.
- Test cheat-string typing in the display mode used by the beta build.
- Tune or document watercraft, helicopter, fixed-wing, and rail warp behavior.
- Watch for rare upside-down vehicle landing cases after warps.
- Validate packaged-build custom install detection by saving a non-standard `gta_sa.exe` path from Setup.

## Known Limitations

- Visible license-plate texture refresh after memory write is deferred. Memory sync/write/readback works, but direct texture rebuild is quarantined because the first attempt destabilized the game.
- One-Hit-Kill and Freeze Timers are deferred because the VB6 app implemented them through runtime code patching, not simple byte writes.
- True in-place current-vehicle replacement is deferred. `Spawn Live` is the supported beta catalog action.
- Standalone trailer and RC vehicle runtime support are not included in Beta 2.
- Full command/shortcut execution parity is not complete for injector-heavy or special-case legacy trainer commands.
- Watercraft, aircraft, and rail warp lanes are available for testing but should be labeled experimental until tuned.
- Legacy folder organization is preserved for compatibility; future releases may add tags, favorites, smart groups, or validation-driven collections.

## Release Signoff Rule

Before tagging Beta 2, run a clean build, verify Setup on the target machine, run the Beta retest checklist, and keep any unverified or risky lane labeled as experimental or deferred in both the UI and checklist.

## Packaging

Use `scripts\package-beta.ps1` from the repository root to create a framework-dependent Beta zip under `artifacts\releases`. Pass `-SelfContained` to create the larger single-file no-runtime-required package.

The package includes the tracked `LegacySource` data/help/image assets required for Setup checks, catalogs, and thumbnails. Ignored legacy binaries, installers, backups, and binary dumps are not included.

GitHub Actions also builds both package styles and uploads them as workflow artifacts for pushes, pull requests, manual workflow runs, and `v*` tags. The self-contained artifact is published as a single-file app executable beside docs and `LegacySource`. Pushing a `v*` tag creates or updates a draft GitHub prerelease with generated package zips attached, so the final publish step remains a deliberate manual signoff.

## Future Update Checks

GTASA CONTROL CENTER includes a safe update check on the Release Notes page. It checks GitHub Releases when the release source is configured, compares against the current app version, and shows an explicit release-page link. It must remain prompt/link-only; avoid silent auto-install behavior for trainer binaries.
