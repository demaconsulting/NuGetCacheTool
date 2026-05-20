## NuGet Cache Tool Utilities Subsystem Verification

This document describes subsystem-level verification for the Utilities subsystem.

### Verification Strategy

The Utilities subsystem is verified entirely through unit tests for its two units,
`TemporaryDirectory` and `PathHelpers`, defined in `TemporaryDirectoryTests.cs` and
`PathHelpersTests.cs` respectively. There are no subsystem-level integration tests;
all requirements are addressed at the unit level.

### Dependencies

- **`TemporaryDirectory`**: Invoked directly; tests create real directories under
  `Environment.CurrentDirectory` and verify creation, path resolution, traversal
  rejection, and disposal.
- **`PathHelpers`**: Invoked directly; tests call `SafePathCombine` with valid and
  invalid path combinations and verify the expected return values or exceptions.

### Test Environment

Tests run under `[Collection("Sequential")]` because they create and delete real
directories under `Environment.CurrentDirectory`. Sequential execution prevents
interference between concurrent test runs sharing the same working directory.

### Acceptance Criteria

All unit tests in `TemporaryDirectoryTests.cs` and `PathHelpersTests.cs` pass; all
requirements listed in the Requirements Coverage section have at least one passing test
scenario; no tests may be skipped or marked as expected failures.

### Requirements Coverage

- **`NuGetCache-Utilities-TempDirectory`**: Covered by the `TemporaryDirectory` unit
  tests — see `docs/verification/nuget-cache-tool/utilities/temporary-directory.md`.
- **`NuGetCache-Utilities-PathSafety`**: Covered by the `PathHelpers` unit
  tests — see `docs/verification/nuget-cache-tool/utilities/path-helpers.md`.
