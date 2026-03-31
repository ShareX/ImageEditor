---
name: update-changelog
description: "Update CHANGELOG.md with entries for the upcoming release before bumping the version."
---

# Update Changelog

## Steps

1. Open `CHANGELOG.md` in the repository root (create it if it does not exist).
2. Add a new `## [Unreleased]` section at the top (or rename an existing one to the new version).
3. Under the section, summarise all changes since the last release grouped by type:
   - `### Added` — new features
   - `### Changed` — changes to existing behaviour
   - `### Fixed` — bug fixes
   - `### Removed` — removed features
4. Stage the file: `git add CHANGELOG.md`
5. Do **not** commit yet — the release bump script will commit all staged changes together.

If there is no `CHANGELOG.md` and the user confirms it is not needed, skip this step and pass `--assume-changelog-done` to `run-release-sequence.sh`.
