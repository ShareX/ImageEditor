# Core Guardrails

## Scope

This refactor establishes `src/ShareX.ImageEditor/Core/**` as behavior-critical editor logic. Changes in this area are considered high risk for regressions in:

- Annotation rendering and hit-testing
- Undo/redo stack behavior
- Image effect annotation processing
- Editor state restoration and serialization

## Review Policy

- Treat `Core/**` changes as protected changes.
- Require at least one maintainer review with explicit verification of behavior parity.
- If CODEOWNERS is added later, map `Core/**` to required maintainers (for example: `@Jaex` and one additional maintainer).

## Local Validation

Run these commands from the `ImageEditor` repository root:

```powershell
dotnet build ShareX.ImageEditor.sln -v minimal
dotnet test ShareX.ImageEditor.sln -v minimal
```

Expected result:

- Build succeeds with zero errors.
- Test suite passes, including `tests/ShareX.ImageEditor.Tests/EditorCoreHistoryTests.cs`.

## What Counts As Behavior Change

Any of the following is a behavior change and must be called out explicitly in PR descriptions:

- Different undo/redo results for the same sequence of actions.
- Different annotation bounds, rendering order, or hit-test outcomes.
- Different effect annotation output or effect update timing.
- Different serialization/deserialization output for the same annotations.

## Refactor Safety Rule

- Structural moves (file/folder/namespace organization) are allowed.
- Logic changes require dedicated tests that demonstrate intended behavior changes.
