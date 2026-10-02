# GTA Center Feature Checklist

This file is the quick maintenance tracker for feature parity and follow-up work.

- Use `[x]` for shipped or working features.
- Use `[ ]` for missing work, known issues, or polish still pending.
- Keep the deeper reasoning, migration history, and implementation notes in [MIGRATION_NOTES.md](./MIGRATION_NOTES.md).

## Foundation and Shell

- [x] Writable app-data storage moved to modern Windows-safe locations.
- [x] Legacy config files migrate forward from the old app folder when present.
- [x] Read-only bundled assets still load from the detected legacy asset root.
- [x] Original VB6-era source/assets moved into `LegacySource` so the repo root is reserved for GTASA CONTROL CENTER docs and the active .NET app.
- [x] Modern GTA install detection replaces the old hardcoded registry path.
- [x] External manifests added for legacy executables.
- [x] GTASA CONTROL CENTER `.NET 8` WPF shell created.
- [x] Setup page for environment, process access, and executable support.
- [x] Executable fingerprinting for the current GTA SA v1.0-family runtime support.
- [x] Phase 1 workbench UI revamp with dedicated pages for Home, Runtime, Vehicles, Garage, Testing, Navigation, and Setup.
- [x] Release Notes page added for in-app release scope, retest order, and known limitations.
- [x] Beta 2 release notes draft added.
- [x] Root README added for GTASA CONTROL CENTER.
- [x] Release packaging script added and verified for framework-dependent Beta zip output.
- [x] Git ignore rules added for build outputs, release artifacts, IDE state, and local runtime data.
- [x] Manual Beta release checklist added.
- [x] GitHub Actions build/package workflow scaffold added.
- [x] Tag-triggered draft GitHub prerelease workflow added.
- [x] Safe in-app update checker scaffold against GitHub Releases with prompt/link-only behavior.
- [x] Configure the public GitHub Releases source before Beta publishing, or intentionally ship the update card disabled.
- [ ] Final visual polish pass for the 2026 UI direction.
- [ ] Final Beta 2 signoff pass after retesting the listed beta-retargeted lanes.

## Legacy Data and Browsing

- [x] Legacy `GTASA*.dat` parsing in GTASA CONTROL CENTER.
- [x] Legacy `GTASAConsole.ini` parsing in GTASA CONTROL CENTER.
- [x] Garage explorer built from parsed legacy data.
- [x] Navigation explorer for warps, markups, shortcuts, and cheats.
- [x] Search and filtering for vehicles, warps, markups, shortcuts, and cheats.
- [x] Vehicle browser port for the old car-selection lane.
- [x] Legacy selector readiness, thumbnail coverage, ratings, and mod-capability surfaced in the vehicle browser.
- [x] First shortcut execution workbench for warp-location shortcuts and cheat-string copy actions.
- [x] Safe trainer-command shortcut subset: vehicle dynamics, markups, sequential warps, weather/time, simple cheats, spawn, and weapons.
- [x] Editable cheat-string writeback for the Navigation cheat tree.
- [x] Editable warp-location writeback for the Navigation location tree.
- [x] Editable folder/description metadata writeback for legacy cheats and locations.
- [x] Create new custom cheat and warp entries from the modern Navigation workbench.
- [x] Guarded delete support for legacy cheat and warp entries.
- [x] Shortcut catalog metadata, active-state, and guarded-delete writeback.
- [x] Create inactive cheat/warp shortcut links from Navigation entries.
- [ ] Revisit legacy folder behavior for a future modern organization model, such as tags, favorites, smart groups, or validation-driven collections.
- [ ] Future smart tagging pass: auto-detect likely water/shoreline warp areas and tag or group them safely for vehicle-risk testing.
- [x] Full tree-edit parity for legacy Locations and Cheats management.
- [ ] Full command/shortcut execution workbench parity with the VB6 app, including injector-heavy and special-case commands still deferred.

## Runtime Readouts and Safe Testing Tools

- [x] Live player-position panel.
- [x] Live vehicle-state panel.
- [x] Vehicle warp preflight panel.
- [x] Warp recovery buffer with return actions.
- [x] Named test target preset system.
- [x] Destination-risk helper with double-click confirmation for risky vehicle warps.
- [x] Make destination-risk warnings more obvious in the UI.

## Warp and Movement Lanes

- [x] Guarded on-foot warp lane.
- [x] Save current position into legacy markup slots.
- [x] `Warp Vehicle` for road cars.
- [x] `Warp Vehicle` for bikes.
- [x] `Warp Vehicle` for heavy ground vehicles (`mtruck`).
- [x] Coupled trailer warp lane.
- [x] Coupled-rig stabilization for heading changes.
- [x] Experimental watercraft warp lane.
- [x] Experimental helicopter warp lane.
- [x] Experimental fixed-wing aircraft warp lane.
- [x] Experimental rail-vehicle warp lane.
- [x] `Set Car Direction` runtime lane.
- [x] `Kickstart` runtime lane.
- [ ] Stabilize the occasional upside-down landing edge case.
- [ ] Validate and tune watercraft warp behavior.
- [ ] Validate and tune helicopter warp behavior.
- [ ] Validate and tune fixed-wing warp behavior.
- [ ] Validate and tune rail-vehicle warp behavior.
- [ ] Standalone trailer support.
- [ ] RC vehicle runtime support.
- [x] Arbitrary degree-turn lane (`Turn My Car in Clock/Counterclock Direction by ...`).

## Vehicle Runtime Actions

- [x] Live vehicle repair.
- [x] Live stop-motion / freeze-current-motion lane.
- [x] Live set-upright lane for supported ground vehicles.
- [x] Live paint from selected catalog vehicle defaults.
- [x] Live catalog spawn (`Spawn Live`) through the guarded legacy injector lane.
- [ ] True current-vehicle replacement in place of the vehicle CJ is already using. Deferred until model/handling/pool safety is mapped; the legacy source exposes spawn, not a safe direct model rewrite.
- [x] Torch current / last / previous vehicle actions.

## Garage Work

- [x] Stage a vehicle from the modern browser for garage editing.
- [x] Write staged vehicle data into legacy garage slots in config.
- [x] Garage payload sanitization when vehicle ids change.
- [x] Live garage site write lane.
- [x] Align live garage writer with the legacy site order, parking angle table, and quadrant math.
- [x] Fix Johnson's House live garage write behavior.
- [ ] Re-verify every safehouse/site after the live garage writer fix.

## Missing High-Value Legacy Pages

- [ ] Player stats page parity.
  - [x] Health
  - [x] Armor
  - [x] Money
  - [x] Fat / muscle
  - [x] Never Wanted legacy toggle
  - [x] Stamina / lung capacity / gambling
  - [x] Driving / biking / cycling / pilot proficiencies
  - [x] Max Respect / Max Sex Appeal legacy toggles
  - [x] Direct wanted-star level editing
  - [x] Girlfriend progress stat editing
  - [x] Opened-islands story unlock editing
  - [ ] Player proof/speciality bits with session lock (implemented; needs user test)
  - [ ] Direct respect / sex appeal value editing and other player stats (respect works; sex appeal patched with Max Sex Appeal bridge/readback and needs retest)
  - [x] On-foot freeze / ped speed controls
- [ ] Weapon page parity.
  - [x] Weapon slot editing for valid legacy slot choices
  - [x] Ammo editing for existing live slots
  - [x] Weapon lock behavior (session selected-slot reapply)
  - [x] Weapon proficiency stats
- [ ] Live cheat toggle page parity.
  - [x] First guarded simple-byte cheat matrix
  - [x] Clear cheated status/count
  - [x] Per-cheat lock/reapply behavior
  - [x] Replace paired Enable/Disable buttons with switch-style toggle controls
  - [ ] One-Hit-Kill / Freeze Timers injector toggles
  - [ ] Safe cheat-string insertion into the running game (implemented with foreground-window guard; needs user test)
- [x] Weather and clock/time controls.
  - [x] Weather set/lock/release
  - [x] Game time set and hour shift
  - [x] Normal / real-time / frozen game clock speed
  - [x] Game speed percentage
- [ ] Car dynamics page parity.
  - [x] Door states
  - [x] Specialities
  - [x] Weight
  - [x] Speed set/increase
  - [x] Wheel damage repair/session lock
  - [x] Full tire visual restore after tires are already shot
  - [x] Flight assistance
  - [x] License plate memory sync/write/readback/session lock confirmed
  - [ ] Visible license-plate texture refresh after custom plate write (quarantined; first direct engine rebuild attempt broke the game and needs a safer redo)
  - [x] Silent mode
  - [x] Alarm-related controls
  - [x] Stalled-engine restart nudge
- [x] Last / previous car utilities.
  - [x] Take me to last car
  - [x] Bring last car to me
  - [x] Take me to previous car
  - [x] Bring previous car to me

## Suggested Work Order

- [x] Fix known broken or risky lanes first.
- [x] Port the highest-value missing legacy pages next.
- [ ] Beta readiness sweep: retest remaining beta lanes, keep deferred/risky items labeled, and run release build verification.
- [ ] Leave final cosmetic polish until after Beta 2 signoff.
