---
name: publish-release
description: "Orchestrate ShareX.ImageEditor release flow in strict order: run-maintenance first, update-changelog second (optional if no CHANGELOG), verify build, bump/commit/push/tag, monitor GitHub Actions every 2 minutes, ensure standard release notes, then set pre-release by default (use explicit opt-out for stable). On failures, inspect logs, fix root cause, and retry with the next patch release."
---

# ShareX.ImageEditor Release Bump Tag

## Overview

Use this skill to run release steps in strict order:
- Step 1: Execute maintenance chores first (`git pull --recurse-submodules` and `git submodule update --init --recursive`)
- Step 2: Run `.ai/skills/update-changelog/SKILL.md` second (optional if no `CHANGELOG.md` exists)
- Step 3: Verify build, then execute bump/commit/push/tag automation
- Step 4: Monitor the tag-triggered release workflow every 2 minutes
- Step 5: If failure occurs, inspect logs, fix issues, and retry with the next patch version
- Step 6: Ensure standard release notes block is present on the GitHub release
- Step 7: Set the successful release as pre-release by default (opt out only when intentionally publishing stable)

Step 3 performs:
- Pre-check: Run `dotnet build ShareX.ImageEditor.sln`; do not proceed if build fails.
- Prompts for `x/y/z` bump type (major/minor/patch) unless specified.
- Updates every tracked `Directory.Build.props` file that defines `<Version>`.
- Stages all current repo changes.
- Commits with version-prefixed message.
- Pushes current branch and creates/pushes annotated tag `vX.Y.Z`.

Step 4-5 performs:
- Find tag run for `Release Build`.
- Poll run status every 120 seconds until completion.
- On failure, inspect failing job logs and identify first blocking error.
- Fix root cause in code/workflow/scripts.
- Re-run local pre-check build.
- Retry release using next patch bump, then monitor again.
- Repeat until workflow succeeds.

Step 6 performs:
- Ensures release notes always include a link to the releases page.
- After the release is published, attach any build artifacts if the workflow produces them.

## Primary Command

From repository root:

```bash
./.ai/skills/publish-release/scripts/run-release-sequence.sh
```

Automated monitor + default pre-release (recommended):

```bash
./.ai/skills/publish-release/scripts/run-release-sequence.sh --assume-changelog-done --monitor --set-prerelease --bump z --yes
```

Stable release opt-out example:

```bash
./.ai/skills/publish-release/scripts/run-release-sequence.sh --assume-changelog-done --monitor --no-prerelease --bump z --yes
```

## Non-Interactive Examples

Patch bump, no prompts:

```bash
./.ai/skills/publish-release/scripts/run-release-sequence.sh --assume-changelog-done --bump z --yes
```

Patch bump with built-in 2-minute monitoring:

```bash
./.ai/skills/publish-release/scripts/run-release-sequence.sh --assume-changelog-done --monitor --monitor-interval 120 --bump z --yes
```

Minor bump with custom commit token/summary:

```bash
./.ai/skills/publish-release/scripts/run-release-sequence.sh --assume-changelog-done --bump y --type CI --summary "Prepare release artifacts" --yes
```

Preview only:

```bash
./.ai/skills/publish-release/scripts/run-release-sequence.sh --assume-changelog-done --bump z --dry-run --yes
```

## When bash is unavailable (e.g. Windows PowerShell)

On environments where `bash` is not in PATH, execute the sequence manually:

1. Step 1 - Maintenance
   - `git pull --recurse-submodules`
   - `git submodule update --init --recursive`

2. Step 2 - Changelog
   - Run `.ai/skills/update-changelog/SKILL.md`.
   - Skip if no `CHANGELOG.md` or user confirms skip.

3. Step 3 - Bump, commit, push, tag
   - Run `dotnet build ShareX.ImageEditor.sln`; abort if it fails.
   - Read current version from root `Directory.Build.props`.
   - Compute next version: patch `Z+1`, minor `Y+1.0`, major `X+1.0.0`.
   - Ensure tag `v<new-version>` does not exist locally or on `origin`.
   - Update all tracked `Directory.Build.props` files containing `<Version>`.
   - `git add -A` -> `git commit -m "[v<new-version>] [CI] Release v<new-version>"` -> `git push origin <current-branch>` -> `git tag -a v<new-version> -m "v<new-version>"` -> `git push origin v<new-version>`.

4. Step 4 - Monitor every 2 minutes
   - Find run: `gh run list --limit 10 --json databaseId,workflowName,headBranch,status,conclusion,url`
   - Poll: `sleep 120`; then `gh run view <run-id> --json status,conclusion,jobs,url`

5. Step 5 - On failure, fix and retry
   - Fetch failed job logs: `gh run view <run-id> --job <job-id> --log`
   - Fix root cause in repository.
   - Re-run `dotnet build ShareX.ImageEditor.sln`.
   - Repeat Step 3 with next patch version.

6. Step 6 - Ensure standard release notes content
   - Read current body: `gh release view v<new-version> --json body`
   - Append the standard releases link if missing.
   - Write body: `gh release edit v<new-version> --notes-file <file>`

7. Step 7 - Set pre-release (default behavior)
   - `gh release edit v<new-version> --prerelease`
   - Verify: `gh release view v<new-version> --json isPrerelease,url,assets`
   - Stable opt-out: skip this step only when intentionally publishing stable.

Default bump when unspecified: patch (`z`). Default commit type token: `CI`.

## Behavior

1. Require completion of `run-maintenance` first.
   - Script behavior: executes maintenance commands automatically unless explicitly bypassed with `--skip-maintenance` (or legacy alias `--assume-maintenance-done`).
2. Require completion of `update-changelog` second (skip if no `CHANGELOG.md` or user confirms).
3. Before bump, run `dotnet build ShareX.ImageEditor.sln`; abort on failure.
4. Run `scripts/bump-version-commit-tag.sh` (or PowerShell/manual equivalent when bash unavailable).
5. After tag push, monitor the release workflow every 120 seconds until complete.
6. If failed, inspect logs, fix root cause, and retry with next patch version.
7. Continue retry loop until release workflow is successful.
8. Ensure standard release notes content is present on the successful release.
9. Mark successful release as pre-release by default; only skip when explicitly publishing stable.

## Guardrails

- Do not skip sequence unless user explicitly requests bypass.
- Do not skip maintenance unless user explicitly requests bypass (`--skip-maintenance`).
- Do not commit/push during maintenance/changelog steps.
- Always verify build before bump/tag.
- Always monitor workflow after tag push; do not stop at tag creation.
- Always inspect logs on failure and fix root cause before retry.
- Always ensure the standard release notes block exists on the successful release.
- Always use a new patch version for retries requiring new commits/tags.
- Abort on detached HEAD.
- Abort if version format is not `X.Y.Z`.
- Abort if matching tag already exists locally or remotely.
- Support `--no-push` and `--no-tag` when partial flow is needed.

## Agent usage (Cursor / Codex)

When executing this skill:
1. Run sequence: maintenance -> changelog -> build verify -> bump/commit/push/tag.
2. Use bash scripts if bash exists; otherwise use PowerShell/manual flow.
3. Default bump is patch (`z`) when unspecified.
4. Monitor tag workflow every 120 seconds until completion.
5. On failure, inspect logs, fix issue, and retry with next patch version.
6. Ensure release notes include the releases page link.
7. If requested, set the final successful release to pre-release.
8. Report final version, commit hash, branch push status, tag push status, run URL, and pre-release status.

Default pre-release policy: unless explicitly instructed otherwise, keep `--set-prerelease` enabled. Use `--no-prerelease` only for intentional stable publishes.

## Notes (lessons learnt)

- Build before bump: avoid tagging broken trees.
- Changelog optional: do not block if `CHANGELOG.md` does not exist unless user requires it.
- Version sync: update every tracked `Directory.Build.props` with `<Version>`.
- Release reliability loop: tag push is not the end; monitor, fix, and retry until green.
- **Workflow name**: the GitHub Actions workflow is `Release Build` — update this in `run-release-sequence.sh` if the workflow file name changes.
