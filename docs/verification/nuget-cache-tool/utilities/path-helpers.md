### PathHelpers Verification

This document describes the unit-level verification design for the `PathHelpers` unit. It defines
the test scenarios, dependency usage, and requirement coverage for `PathHelpers.cs`.

#### Verification Approach

`PathHelpers` is verified with unit tests defined in `PathHelpersTests.cs`. The tests exercise
all supported path combinations, boundary conditions, and error paths. No external dependencies
are involved; the tests call `PathHelpers.SafePathCombine` directly.

#### Dependencies

No dependencies. `PathHelpers.SafePathCombine` is a pure static method with no external
collaborators.

#### Test Environment

PathHelpers unit tests run under the standard xUnit v3 test runner within the
`DemaConsulting.NuGet.CacheTool.Tests` project. `PathHelpers.SafePathCombine` is a pure static
method with no I/O or external dependencies, so no special environment setup is required.

#### Acceptance Criteria

The PathHelpers unit test suite passes when all of the following conditions are met:

- All test scenarios defined in `PathHelpersTests.cs` pass.
- All boundary and error-path scenarios produce the expected exception type with the expected
  message content.
- Every PathHelpers unit requirement listed in the Requirements Coverage section is covered by
  at least one passing scenario.

#### Test Scenarios

##### PathHelpers_SafePathCombine_ValidPaths_CombinesCorrectly

**Scenario**: `SafePathCombine` is called with a valid base and relative path.

**Expected**: Returns the path equal to `Path.Combine(basePath, relativePath)`.

**Requirement coverage**: `NuGetCache-PathHelpers-SafePathCombine`.

##### PathHelpers_SafePathCombine_PathTraversalWithDoubleDots_ThrowsArgumentException

**Scenario**: `SafePathCombine` is called with `"../etc/passwd"` as the relative path.

**Expected**: `ArgumentException` is thrown with "Invalid path component" in the message.

**Requirement coverage**: `NuGetCache-PathHelpers-SafePathCombine`.

##### PathHelpers_SafePathCombine_DoubleDotsInMiddle_ThrowsArgumentException

**Scenario**: `SafePathCombine` is called with `"subfolder/../../../etc/passwd"`.

**Expected**: `ArgumentException` is thrown with "Invalid path component" in the message.

**Requirement coverage**: `NuGetCache-PathHelpers-SafePathCombine`.

##### PathHelpers_SafePathCombine_UnixAbsolutePath_ThrowsArgumentException

**Scenario**: `SafePathCombine` is called with `"/etc/passwd"` as the relative path.

**Expected**: `ArgumentException` is thrown.

**Requirement coverage**: `NuGetCache-PathHelpers-SafePathCombine`.

##### PathHelpers_SafePathCombine_WindowsAbsolutePath_ThrowsArgumentException

**Scenario**: `SafePathCombine` is called with `@"C:\Windows\System32"` as the relative path
(Windows only; test skips on non-Windows).

**Expected**: `ArgumentException` is thrown.

**Requirement coverage**: `NuGetCache-PathHelpers-SafePathCombine`.

##### PathHelpers_SafePathCombine_RootedPathInsideBase_ThrowsArgumentException

**Scenario**: `SafePathCombine` is called with a rooted `relativePath` that already resolves
inside `basePath` (for example, `basePath` combined with itself to form the rooted input).

**Expected**: `ArgumentException` is thrown with "Invalid path component" in the message, proving
the rooted-path rejection happens upfront rather than only after resolving whether the result
escapes `basePath`.

**Requirement coverage**: `NuGetCache-PathHelpers-SafePathCombine`.

##### PathHelpers_SafePathCombine_CurrentDirectoryReference_CombinesCorrectly

**Scenario**: `SafePathCombine` is called with `"./subfolder/file.txt"`.

**Expected**: Returns the path equal to `Path.Combine(basePath, relativePath)`.

**Requirement coverage**: `NuGetCache-PathHelpers-SafePathCombine`.

##### PathHelpers_SafePathCombine_NestedPaths_CombinesCorrectly

**Scenario**: `SafePathCombine` is called with a deeply nested relative path.

**Expected**: Returns the path equal to `Path.Combine(basePath, relativePath)`.

**Requirement coverage**: `NuGetCache-PathHelpers-SafePathCombine`.

##### PathHelpers_SafePathCombine_EmptyRelativePath_ReturnsBasePath

**Scenario**: `SafePathCombine` is called with an empty string as the relative path.

**Expected**: Returns the path equal to `Path.Combine(basePath, "")`.

**Requirement coverage**: `NuGetCache-PathHelpers-SafePathCombine`.

##### PathHelpers_SafePathCombine_DoubleDotPrefix_CombinesCorrectly

**Scenario**: `SafePathCombine` is called with `"..data/file.txt"` (a filename starting with
`..` but not a traversal component).

**Expected**: Returns the path equal to `Path.Combine(basePath, relativePath)`.

**Requirement coverage**: `NuGetCache-PathHelpers-SafePathCombine`.

##### PathHelpers_SafePathCombine_NullBase_ThrowsArgumentNullException

**Scenario**: `SafePathCombine` is called with `null` as the base path.

**Expected**: `ArgumentNullException` is thrown.

**Requirement coverage**: `NuGetCache-PathHelpers-NullArguments`.

##### PathHelpers_SafePathCombine_NullRelative_ThrowsArgumentNullException

**Scenario**: `SafePathCombine` is called with `null` as the relative path.

**Expected**: `ArgumentNullException` is thrown.

**Requirement coverage**: `NuGetCache-PathHelpers-NullArguments`.
