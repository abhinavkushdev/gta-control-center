# GTASA CONTROL CENTER Migration Notes

## What This Project Is Today

- Visual Basic 6 desktop application (`GTASAConsole.vbp`)
- Depends on legacy ActiveX controls: `TABCTL32.OCX` and `MSCOMCTL.OCX`
- Talks to GTA San Andreas by opening the game process and reading/writing memory directly
- Uses fixed offsets for GTA SA `v1.0` and `v1.1`

## High-Risk Areas

- The trainer logic is tightly coupled to 32-bit memory addresses and code injection.
- The UI is a large VB6 form with custom controls and fixed layout assumptions.
- The old registry lookup for `gta_sa.exe` was tied to a historical machine-specific path and was not reliable on current Windows installs.
- The original app writes config files next to the EXE, which often fails under modern protected folders such as `Program Files`.

## Changes In This Migration Pass

- Writable config/data files now resolve to `%LOCALAPPDATA%\GTASA Control Center` with fallback to `%APPDATA%` and then `App.Path`.
- Existing legacy config files are copied forward from the old app folder on first run.
- Read-only bundled assets such as `CarPics\*.bmp` still load from the legacy asset root. During repo cleanup, the full VB6-era project and bundled assets were moved into `LegacySource`.
- GTA install detection now looks for `gta_sa.exe` in modern registry locations and common install folders instead of the old hardcoded path.
- External manifests were added for `GTASA_Center.exe` and `GTASA_Console.exe` so Windows can apply modern common controls and explicit OS compatibility metadata.
- A separate feature tracker now lives in [FEATURE_CHECKLIST.md](./FEATURE_CHECKLIST.md) for quick done/pending maintenance, while this file stays focused on migration context and implementation notes.
- A Beta 2 release draft now lives in [BETA_RELEASE_NOTES.md](./BETA_RELEASE_NOTES.md), and GTASA CONTROL CENTER has a `Beta Notes` page so release scope, retest order, and known limitations are visible inside the app.
- The original VB6 project, legacy executables, DAT files, help/assets, and installer artifacts now live under [LegacySource](./LegacySource/README.md). `AppPaths` detects this folder as the legacy asset root so the active .NET app can stay isolated in `ModernShell`.
- Release hygiene is now in place: `.gitignore` excludes generated build/release output, [README.md](./README.md) describes the project and build requirements, and `scripts\package-beta.ps1` creates the Beta zip under `artifacts\releases`.
- A manual [RELEASE_CHECKLIST.md](./RELEASE_CHECKLIST.md), GitHub Actions build workflow, and tag-triggered draft release workflow are now in place. The build workflow produces framework-dependent and self-contained package artifacts, while the release workflow creates or updates a draft GitHub prerelease when a `v*` tag is pushed. The Beta Notes page now includes a prompt/link-only update checker that can query GitHub Releases once the public repository source is configured; it must not silently install updates.
- Packaged-build diagnostics now include copy/save report actions. GTA install detection now checks a saved custom executable path, a running `gta_sa.exe` process image path, sibling folders, registry app paths, and common install folders, in that order.

## Active Follow-Ups

- The first guarded in-vehicle warp lane now covers legacy type `car`, `bike`, `mtruck`, `boat`, `heli`, `plane`, and `train` vehicles, plus coupled trailer warps on supported road-car towing rigs.
- Coupled-rig stabilization now rotates trailer spacing and trailer-anchor placement into the new heading. Recheck hitch stability on a real turn-changing warp before widening the lane further.
- Experimental watercraft, aircraft, and rail lanes now use class-aware staging guidance in GTASA CONTROL CENTER. Standalone trailers and RC are still outside the guarded warp lane.
- A bounded recovery buffer now records successful on-foot and vehicle warp source points, so later testing can use `Return On Foot` and `Return Vehicle` instead of manually rebuilding a safe position.
- Named test target presets are now persisted in `GTASAControlCenter.TestTargets.json` under the writable data root, separate from the legacy INI markups, so safe staging spots can be pinned specifically for GTASA CONTROL CENTER testing.
- A destination-risk helper now warns when a chosen vehicle target looks mismatched for the current live lane, and requires a second identical `Warp Vehicle` click to continue risky jumps anyway.
- Destination-risk warnings are now surfaced as persistent banners on Runtime, Testing, and Navigation instead of only appearing in the shared runtime status text. A risky vehicle destination now clearly says that the first click armed the warning and the same second `Warp Vehicle` click confirms it.
- A modern vehicle catalog browser now ports the old car-selection lane into the shell: legacy selector tabs, ratings, selector readiness, thumbnail coverage, and parsed mod-id support are searchable without touching the live game yet.
- Phase 1 UI revamp is now in: GTASA CONTROL CENTER uses a fixed status rail plus dedicated workbench pages for Home, Runtime, Vehicles, Garage, Testing, Navigation, and Diagnostics, so feature testing no longer depends on one giant scrolling screen.
- Beta 2 prep UI labels are now in: Home has a readiness card, Runtime marks water/air/rail vehicle warps as experimental, Garage calls out the remaining safehouse retest matrix, Player marks proof bits as a beta retest lane, Dynamics labels deferred post-beta items, and a dedicated Beta Notes page summarizes scope/limits.
- The first live catalog action is now in: the vehicle browser can queue a selected vehicle id through the old isolated car-spawn injector on supported executables. This is a guarded live spawn lane, not a raw in-place vehicle-model rewrite.
- A first shortcut execution workbench is now in on the Navigation page. Warp-location shortcuts call the modern guarded on-foot warp lane, cheat-link shortcuts copy/show the legacy cheat string for manual entry, and built-in trainer-command shortcuts report their legacy command ordinal/data payload instead of pretending to be safely executable. Full command execution and safe cheat-string insertion remain separate parity tasks.
- True in-place current-vehicle replacement is still deferred: the legacy codebase exposes the isolated spawner, but not a safe direct model rewrite for the vehicle CJ is already sitting in. Avoid raw car-id swaps until the model/handling/pool implications are mapped.
- The first live garage-memory lane is now in: each safehouse card can write its full four-slot block into live GTA memory using the legacy garage address map and the old site parking-layout math. This is a site write, not a single-slot poke.
- Johnson's House live garage writes are confirmed working after aligning the VB6 site order, legacy parking-angle table, and original garage placement quadrant math. Keep the broader safehouse re-verification pass open until each site has been checked.
- The garage edit lane now keeps slot lock/protection flags plus parking heading, while resetting stale mod and extra payload to safe legacy defaults whenever a different vehicle is written into the slot. Same-vehicle writes keep compatible tune payload and sanitize invalid entries.
- A first live vehicle-recovery lane is now in: the shell can repair engine/burn state, zero live speed/spin, and upright supported ground vehicles without touching the broader garage or replacement lanes.
- A guarded torch lane is now in on the Runtime page: Torch Current, Torch Last, and Torch Previous write the legacy damage-tolerance value of `5` to the selected vehicle pointer. Previous vehicle targeting is session-tracked by the modern runtime sampler after switching between vehicles. Torch Current has been confirmed in-game.
- The old last/previous car utility lane is now in and confirmed in-game: while CJ is on foot, the Runtime page can take CJ to the last/previous synchronized vehicle or bring that vehicle beside CJ using the original side-of-car placement math and settle-speed write. Previous vehicle targeting depends on GTASA CONTROL CENTER observing a vehicle switch during the current session.
- A Player page is now in: live readouts plus guarded writes for health/max-health, armor, money, fat, muscle, stamina, lung capacity, gambling, driving/biking/cycling/pilot proficiencies, direct wanted-star level, and the legacy Never Wanted/Max Respect/Max Sex Appeal cheat bytes. Raw respect/sex-appeal value editing is still intentionally not exposed because that runtime chain is not cleanly mapped in the old code.
- Direct wanted-star editing is now mapped through the live player-data `CWanted` pointer instead of a static blind write. The lane reads current stars/chaos, writes the wanted chaos and wanted-level fields with 0-6 clamping, and turns off the Never Wanted byte first when applying a nonzero wanted level so stars are not immediately suppressed. User testing confirmed changing wanted stars up to 6 and back to 0 works.
- Raw respect and sex-appeal stat writes are now exposed in Player > Body / Social Stats using the v1.0 stat globals `0xB79480` for Respect and `0xB793E4` for Sex Appeal, with the existing `+0x2680` shift for v1.1. These are separate from the Max Respect / Max Sex Appeal cheat-byte toggles. User testing confirmed the other new body/social sliders but not sex appeal, so the lane now also syncs the Max Sex Appeal byte for a 100% target, disables it for partial raw-target tests, and reports raw sex-appeal readback because GTA's visible sex appeal is also derived from outfit/vehicle/condition/fat/muscle factors.
- The Player page now includes the old girlfriend-progress lane for Denise, Michelle, Helena, Katie, Barbara, and Millie. These use the simple legacy progress/stat globals recovered from the VB6 girlfriend stats page, with Sync From Live, Set All 100%, and Apply Progress actions. First user test showed Sync From Live worked but applying Helena 100% did not affect the in-game value; the fixed lane now writes both the `0xB792xx` girlfriend stat mirrors and the `0xA49Fxx` progress mirrors with readback verification, and user retesting confirmed it works in-game.
- The Player page now also includes the legacy opened-islands story unlock DWORD from the old address table: v1.0 `0xB790F4`, v1.1 `+0x2680`. The modern lane reads the current value, exposes a 0-4 target slider, writes with clamping, and verifies readback. This changes story-gate/global state only; it does not teleport CJ. User testing after starting a new game suggests all cities unlocked correctly without triggering wanted stars in another city.
- The legacy player speciality/Fix Ped lane is now in on the Player page. It reads CJ's player object `+66` specials word, exposes EP/DP/BP/FP targets, applies them once, and can keep a session-only lock active during live auto-refresh. Player damage-proof uses bit 6, while explosion-proof uses bit 7, bullet-proof bit 2, and fire-proof bit 3, matching the VB6 `chkPedSpecs` lane. User asked to leave this for later testing, so keep it pending until confirmed in-game.
- The old on-foot ped speed/freeze lane is now in on the Player page. It writes the legacy player speed vector while CJ is on foot only; `Stop Player Motion` zeros X/Y/Z like the original Freeze Player action.
- A first Weapons page is now in: it ports the old detailed weapon proficiency dialog with live readouts and guarded writes for Pistol, Silenced Pistol, Desert Eagle, Shotgun, Sawn-off Shotgun, Combat Shotgun, Machine Pistol, SMG, AK47, and M4. Slot editing, ammo editing, and a session selected-slot lock have since been layered onto the mapped live inventory block.
- The Weapons page now reads the live player weapon inventory block and can edit total/loaded ammo for an existing weapon slot.
- Weapon slot replacement is now in for valid legacy slot choices. When the target weapon id changes, the shell reuses the old spawner hook and writes the matching weapon.dat id into the per-slot initializer mailbox before writing the 16-byte live weapon slot block. The known-crashy IR Goggles option and brass-knuckles special case remain deferred.
- A session weapon lock is now in: the Weapons page can capture the selected legacy slot, weapon id, and ammo target, apply it immediately, then re-apply it during live auto-refresh. This is intentionally not persisted across app restarts yet.
- A first Live Cheats page is now in: it reads and writes the 20 simple legacy cheat bytes (`Never Wanted` through `Cars can Fly`) and ports the old `Clear Cheated Status and Count` action. Fast Cars/Cheap Cars keep the original mutual-exclusion behavior. One-Hit-Kill and Freeze Timers remain deferred because the VB6 app uses runtime code patching for those, not simple byte writes.
- Live Cheats testing confirmed several simple toggles working in-game. Follow-up UI polish: replace paired Enable/Disable buttons with switch-style on/off controls once the page gets a refinement pass.
- A first World page is now in: it reads/writes the old weather current/target/lock globals, game day/hour/minute/weekday, game-clock milliseconds, and game-speed multiplier. The weather list was recovered from the VB6 `.frx` combo data and keeps the original 46-entry order.
- A first Car Dynamics page is now in: it ports the safe direct-write subset for current vehicle doors (`open`/`locked` byte), alarm/siren timer clear, stalled-engine restart byte nudge, speed vector set/nudge, car speciality proof bits, car weight/turn-mass edits, wheel-damage flag clear/session lock, license-plate writes, Silent Mode feedback parity, and a guarded Flight Assistance lane. Specialities use the legacy object `+66` 16-bit word: explosion-proof bit 7, damage-proof bit 4, bullet-proof bit 2, and fire-proof bit 3. Weight uses the legacy object `+140` mass float and `+144` turn-mass float, enables damage-proof first like VB6 `SetCarWeight`, scales turn mass by the mass ratio, and keeps boat writes blocked for now. License plates follow the VB6 pointer chain: vehicle object `+1416` pointer, then 8 uppercase ASCII bytes at pointer `+16`, with an optional session lock during live auto-refresh. Wheel repair keeps the bike word clear at object `+1630`, and the car/heavy lane now performs a fuller tire visual restore by clearing the `CAutomobile` damage-manager wheel effect at `+1440` plus four wheel status bytes at `+1445..+1448`; this replaces the old `+1444/+1445` two-byte clear that did not visually restore already-shot tires, and user testing confirmed the new lane works on shot tires. Silent Mode maps the old feedback toggle to GTASA CONTROL CENTER status muting: routine success messages are suppressed while warnings/failures still surface. Flight Assistance is session-only and legacy-inspired: it zeroes spin, levels `ZGrad`/`ZLook`/`XWhat`/`YWhat`/`ZWhat`, normalizes horizontal speed toward the facing vector, and boosts falling Z speed by `0.03 + percent*0.02`; trailers, watercraft, and rail are blocked for this first pass. Persistent dynamics locks remain deferred.
- License plate follow-up: user testing confirmed Sync Current reads the live plate text and Apply Plate writes custom text into the same vehicle object `+1416 -> +16` 8-byte lane, but the already-spawned car still does not visually update. The first direct v1.0 engine texture rebuild attempt using `CVehicle::CustomCarPlate_TextureDestroy` / `CustomCarPlate_TextureCreate` destabilized/broke the running game, so that lane has been quarantined and the UI now labels the remaining action as Apply Memory Only. Redo this later with a safer respawn/restream-oriented strategy instead of direct texture rebuild calls.
- Live Cheats now support session-only per-cheat locks. Each simple cheat byte can be locked ON or OFF and is reapplied during live auto-refresh; Fast Cars and Cheap Cars still preserve the old mutual-exclusion behavior by dropping the conflicting ON lock.
- The Navigation > Cheats workbench now has the first safe edit lane for the old cheat tree: each listed cheat string can be copied or edited, and `Save String` writes the selected UID back into the writable `GTASACheats.dat` copy before reparsing the legacy catalog.
- Cheat-string insertion is now ported as a non-injector input lane: `Send to GTA` and cheat-link shortcut `Run` resolve the `GTA: San Andreas` window, restore/focus it, verify it is foreground, and then type the validated uppercase cheat string one key at a time like the VB6 `SendCheatCode` helper. The shell refuses to type if GTA cannot be focused so cheat text is not sent into another app. This still needs user testing across windowed/fullscreen modes.
- The Navigation > Warps workbench now has matching data-file writeback for legacy location entries. Each listed warp can save an edited `X;Y;Z;Angle` string back into the writable `GTASALocs.dat` copy, or capture CJ's current live position into that UID, then reparse the catalog so direct warps and shortcut links use the updated coordinates.
- Legacy cheat and warp entries can now also save folder/description metadata from the modern Navigation workbench. Folder edits intentionally move the entry after the catalog reparse instead of trying to maintain a separate in-memory tree state.
- Navigation can now create new custom cheat rows and new custom warp rows with fresh UIDs in the writable `GTASACheats.dat` and `GTASALocs.dat` copies. New warp drafts can be seeded from CJ's current live position before saving.
- Navigation now has guarded delete support for legacy cheat and warp rows. Deletes require a confirmation dialog and warn that shortcut links to the removed UID will become unresolved until relinked or recreated.
- Shortcut rows in `GTASAConfig.dat` now have the same safe data-management lane for metadata and active state: folder/description writeback, enable/disable, and guarded delete are available from Navigation > Shortcuts. This intentionally does not imply that every legacy trainer command is executable yet.
- Cheat and warp entries can now create linked shortcut rows in `GTASAConfig.dat` directly from their Navigation cards. These links are intentionally created inactive and unassigned by default so they appear in the Shortcuts workbench without silently installing a real hotkey; key-capture/binding UX remains a later polish pass.
- Future release note: the VB6-era folder tree is now preserved for compatibility, but it may be worth migrating user-facing organization to a more modern model later, such as tags, favorites, smart groups, recent/safe test targets, or validation-driven collections while keeping legacy DAT export compatibility underneath.
- Future smart-data note: revisit automatic detection of likely water/shoreline warp entries so GTASA CONTROL CENTER can tag or group them for safer vehicle-risk testing instead of relying only on folder names and keyword hints.
- A first live paint lane is now in: the vehicle browser can apply a catalog vehicle's legacy default colors onto the current live vehicle, and mirrors the same colors into an attached trailer when one is present.
- The Runtime page now ports the old `Set Car Direction / Kickstart` lane plus the `Turn My Car in Clock/Counterclock Direction by ...` lane. Direction/kickstart use the original VB6 eight-way placement and kickstart tables; degree-turn writes rotate the live heading basis without moving the vehicle and keep readable coupled road-car trailers aligned. Direction/turn behavior has been confirmed in-game on the current supported executable.
- Occasional upside-down vehicle landings were observed during modern in-car teleports. Investigate transform-basis and landing normalization before widening support to more vehicle classes.

## Validation Checklists

### Road-Car Stability Check

- Use a normal road car with no trailer attached.
- Bring the vehicle to a full stop before warping.
- Warp once to a flat street location.
- Confirm the car lands upright, with no immediate roll/spin, and can drive away normally.
- Confirm the `Vehicle Warp Preflight` card stays green before the warp.

### Bike Warp Check

- Use a normal bike with no trailer or special mission state active.
- Bring the bike to a full stop before warping.
- Warp once to a flat road or parking-lot location.
- Confirm the bike lands upright and CJ stays mounted after the warp.
- Confirm the `Vehicle Warp Preflight` card turns green before the warp.

### Trailer Warp Check

- Use a supported towing road car with one attached trailer.
- Bring the full rig to a complete stop on a straight, flat road.
- Confirm the `Vehicle Warp Preflight` card turns green before warping.
- Warp once to a flat road destination with a noticeably different heading from the current rig direction.
- Confirm the truck and trailer both move together, stay coupled, and settle without immediate spin or jackknife.

### Heavy-Ground Warp Check

- Use a legacy `mtruck` vehicle such as a Monster, Duneride, or Dumper.
- Bring it to a complete stop on flat ground.
- Confirm the `Vehicle Warp Preflight` card turns green before warping.
- Warp once to a flat destination.
- Confirm the vehicle lands upright, clears the ground cleanly, and can drive away without sudden bounce or roll.

### Watercraft Warp Check Later

- Use a normal boat with no mission scripting active.
- Prefer a water destination or wide shoreline destination first.
- Confirm the hull settles cleanly and the craft remains controllable after the warp.

### Helicopter Warp Check Later

- Use a normal helicopter in a quiet open area.
- Prefer a wide helipad, runway, or open field destination first.
- Confirm the helicopter spawns clear of obstacles and does not immediately clip the ground.

### Fixed-Wing Warp Check Later

- Use a fixed-wing aircraft on a quiet runway or open airstrip.
- Prefer long open destinations first.
- Confirm the aircraft stages high enough to avoid an instant strike and can recover cleanly.

### Rail Warp Check Later

- Use a single rail vehicle first before trying longer consists.
- Prefer a rail-aligned destination first.
- Confirm the vehicle remains stable and does not immediately twist or launch on arrival.

## Recommended Migration Strategy

1. Stabilize the legacy app on modern Windows.
2. Port config parsing, diagnostics, and install detection into a new .NET desktop shell.
3. Move trainer operations behind a smaller service layer so GTA offsets and memory writes are isolated.
4. Rebuild the UI section by section instead of trying to port the full VB6 form in one step.

## Best Modern Target

- `WPF` on `.NET 8` is the safest choice for a modern Windows-only rewrite.
- `WinUI 3` is possible, but it adds more packaging and deployment complexity than this project needs for a first migration.
- `twinBASIC` is also worth evaluating if the goal is to preserve more of the VB6 code while gradually modernizing.

## Immediate Next Steps

1. Create a new .NET desktop shell with sections for `Dashboard`, `Vehicle`, `Player`, `Garage`, `Teleport`, `Cheats`, and `Diagnostics`.
2. Port the config/data parsers first because they are low-risk and easy to verify.
3. Add a runtime diagnostics page that shows:
   - detected GTA executable
   - selected GTA version
   - whether process access succeeded
   - whether code injection is enabled
4. Only after that, start porting memory operations feature by feature.

## Shortcut Workbench Notes

- The first `Run` button pass only handled direct warp links and cheat-string copy actions; most imported shortcuts are actually legacy trainer command ordinals (`Category 0`), so they appeared to do nothing useful in game.
- The modern workbench now maps the safe ordinal subset into guarded runtime lanes: car doors/specialities/weight/paint/speed/spin/forward speed/flight assist/turn/upright/stop/torch, markups, sequential locations, last/previous car utilities, game time/clock/weather, simple cheat toggles, catalog spawn, and weapon give/remove/clear actions.
- Deferred shortcut ordinals remain intentionally read-only with explanatory status text. High-risk or not-yet-modeled examples include girlfriend date-prep, ped turn/flight assist, one-hit-kill/freeze-timer injectors, previous-car door writes, and relative clock/game-speed slider nudges.

## Live Cheat UI Notes

- The Live Cheats page now uses a single switch-style state button per simple cheat byte instead of separate Enable/Disable buttons. The explicit Lock ON / Lock OFF buttons remain separate because session locks choose a reapply target rather than just flipping the current byte once.
