## NuGet Cache Tool Utilities Subsystem Verification

This document describes the subsystem-level verification design for the `Utilities` subsystem. It
defines the overall verification strategy and requirement coverage for the Utilities subsystem.

### Verification Approach

The Utilities subsystem is verified with subsystem integration tests defined in `UtilitiesTests.cs`.
These tests exercise `TemporaryDirectory` and `PathHelpers` collaborating together, verifying that
path-boundary enforcement flows end-to-end from `TemporaryDirectory.GetFilePath` through
`PathHelpers.SafePathCombine`. Unit-level tests in `TemporaryDirectoryTests.cs` and
`PathHelpersTests.cs` verify each unit in isolation and address child requirements.

### Dependencies

| Dependency           | Usage in Tests                                                                         |
|----------------------|----------------------------------------------------------------------------------------|
| `TemporaryDirectory` | Created directly; exercises directory creation, file-path resolution, and disposal.    |
| `PathHelpers`        | Called directly via `SafePathCombine` in the path-safety scenario; also exercised      |
|                      | indirectly through `TemporaryDirectory.GetFilePath` in all other scenarios.            |
| Real filesystem      | All tests create and delete real directories under `Environment.CurrentDirectory`.     |

### Test Environment

Utilities subsystem tests run under the standard xUnit v3 test runner within the
`DemaConsulting.NuGet.CacheTool.Tests` project. Tests use the `[Collection("Sequential")]`
attribute because they create and delete real directories under `Environment.CurrentDirectory`;
sequential execution prevents interference between concurrent test runs sharing the same
working directory. No external services or network connectivity are required.

### Acceptance Criteria

The Utilities subsystem test suite passes when all of the following conditions are met:

- All test scenarios defined in `UtilitiesTests.cs` pass.
- Every Utilities subsystem requirement listed in the Requirements Coverage section is covered by
  at least one passing scenario.
- No tests may be skipped or marked as expected failures.

### Test Scenarios

#### Utilities_PathResolution_ValidRelativePath_ReturnsPathWithinDirectory

**Scenario**: A `TemporaryDirectory` is created and `GetFilePath("output.txt")` is called with a
simple file name.

**Expected**: The returned path is fully qualified and starts with `DirectoryPath`, confirming
`GetFilePath` delegates to `SafePathCombine` and returns a path within the boundary.

**Requirement coverage**: `NuGetCache-Utilities-TempDirectory`.

#### Utilities_PathResolution_NestedRelativePath_CreatesIntermediateDirectories

**Scenario**: A `TemporaryDirectory` is created and `GetFilePath("sub/dir/output.txt")` is called
with a path that requires two levels of intermediate directories.

**Expected**: The intermediate directory `sub/dir/` exists on the filesystem after the call,
and the returned path is within `DirectoryPath`.

**Requirement coverage**: `NuGetCache-Utilities-TempDirectory`.

#### Utilities_PathTraversal_TraversalAttempt_ThrowsArgumentException

**Scenario**: A `TemporaryDirectory` is created and `GetFilePath("../escape.txt")` is called with
a traversal component.

**Expected**: `ArgumentException` is thrown before any filesystem access, confirming that
`PathHelpers.SafePathCombine` rejects the traversal inside `GetFilePath`.

**Requirement coverage**: `NuGetCache-Utilities-PathSafety`, `NuGetCache-Utilities-TempDirectory`.

#### Utilities_DirectoryLifecycle_CreateAndDispose_DirectoryCreatedThenDeleted

**Scenario**: A `TemporaryDirectory` is created, a file is written via `GetFilePath`, and the
instance is disposed.

**Expected**: The directory and its contents are removed from the filesystem after disposal, and
the directory and file are accessible while the instance is live.

**Requirement coverage**: `NuGetCache-Utilities-TempDirectory`.

#### Utilities_PathSafety_SafePathCombine_StaysWithinBase

**Scenario**: `PathHelpers.SafePathCombine` is called directly with a real temporary directory
as the base path and `"safe/child/file.txt"` as the relative component.

**Expected**: The combined path resolves to a location within the base directory; the relative
path between the absolute base and the absolute combined path contains no leading `..` components
and is not rooted.

**Requirement coverage**: `NuGetCache-Utilities-PathSafety`.

### Requirements Coverage

- **`NuGetCache-Utilities-PathSafety`**: Utilities_PathTraversal_TraversalAttempt_ThrowsArgumentException,
  Utilities_PathSafety_SafePathCombine_StaysWithinBase
- **`NuGetCache-Utilities-TempDirectory`**: Utilities_PathResolution_ValidRelativePath_ReturnsPathWithinDirectory,
  Utilities_PathResolution_NestedRelativePath_CreatesIntermediateDirectories,
  Utilities_PathTraversal_TraversalAttempt_ThrowsArgumentException,
  Utilities_DirectoryLifecycle_CreateAndDispose_DirectoryCreatedThenDeleted
