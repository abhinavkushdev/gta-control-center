# GTASA CONTROL CENTER - Beta Release Checklist

Use this checklist for `0.1.0-beta.2` and future GitHub beta releases.

## Preflight

- [x] Confirm `FEATURE_CHECKLIST.md` has no accidental regressions or unlabelled risky lanes.
- [x] Confirm `BETA_RELEASE_NOTES.md` matches the build version and current known limitations.
- [x] Confirm `README.md` still has accurate requirements and packaging commands.
- [x] Confirm `LegacySource` still contains the legacy DAT/INI/assets needed for development-time parsing.
- [x] Confirm the working tree does not include generated `bin`, `obj`, `artifacts`, or local runtime data.
- [x] Confirm old compiled legacy binaries/installers/backups are ignored or untracked before pushing publicly.
- [x] Confirm the in-app update source points at `yyppsk/GTA-SA-Control-Center`, or intentionally override it only for local testing.

## Local Build

- [x] Run `dotnet build .\ModernShell\GTACenter.Modern.csproj`.
- [x] Run `.\scripts\package-beta.ps1`.
- [x] Optional: run `.\scripts\package-beta.ps1 -SelfContained` for the larger single-file no-runtime-required package; this may need NuGet access the first time Windows runtime packs are restored.
- [x] Confirm `artifacts\releases\GTASAControlCenter-0.1.0-beta.2-win-x64-framework-dependent.zip` exists.
- [x] Confirm `artifacts\releases\GTASAControlCenter-0.1.0-beta.2-win-x64-self-contained.zip` exists and contains one app executable beside docs and `LegacySource`.

## Smoke Test

- [x] Download the GitHub Actions single-file self-contained artifact and confirm it works in a smoke test.
- [x] Extract either the framework-dependent zip or the single-file self-contained zip to a clean temporary folder.
- [x] Launch `GTASAControlCenter.exe`.
- [x] Confirm the extracted package contains `LegacySource\GTASAData.dat`, `LegacySource\GTASACheats.dat`, `LegacySource\GTASALocs.dat`, `LegacySource\GTASAConfig.dat`, `LegacySource\CarPics`, `LegacySource\Help`, and `LegacySource\Images`.
- [x] Open Setup and confirm the app finds the legacy asset root and writable data root.
- [x] If GTA is installed in a custom folder, use Setup > GTA Install Path to save the path and confirm compatibility unlocks.
- [x] With GTA running, confirm Setup reports the expected executable support posture.
- [x] Confirm Runtime page refresh works without crashing.
- [x] Confirm Release Notes are visible and known limitations are clear.
- [ ] Confirm Release Notes > Update Channel checks GitHub Releases after the repository/release is publicly visible.
- [x] Use Setup > Copy Report if any packaged-build check differs from your local test build.

## Beta Runtime Retest

- [x] Re-verify all safehouse/site live garage writes beyond Johnson's House.
- [x] Retest player proof/speciality bits and session lock behavior.
- [x] Retest direct respect/sex appeal behavior, especially visible sex appeal versus raw readback.
- [x] Test cheat-string typing in the display mode used by beta testers.
- [x] Tune or document watercraft, helicopter, fixed-wing, and rail warp behavior.
- [x] Watch for rare upside-down vehicle landing cases after warps.

## GitHub Release

- [ ] Create tag `v0.1.0-beta.2`.
- [ ] Push the tag to GitHub.
- [ ] Confirm the `Draft Beta Release` workflow succeeds.
- [ ] Confirm the draft release has the framework-dependent zip attached.
- [ ] Confirm the draft release has the single-file self-contained zip attached.
- [ ] Confirm the draft release notes match `BETA_RELEASE_NOTES.md`.
- [ ] Keep the release marked as pre-release.
- [ ] Publish the draft only after the smoke test and Beta retest checklist are complete.

## Future Automation Notes

- GitHub Actions now builds and uploads package artifacts for pushes, pull requests, and version tags.
- Pushing a `v*` tag now creates or updates a draft GitHub prerelease with generated zip assets attached.
- The in-app update checker is intentionally prompt/link-only. It should never silently download or install trainer binaries.
