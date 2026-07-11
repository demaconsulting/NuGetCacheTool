### Validation Verification

This document describes the unit-level verification design for the `Validation` unit. It defines
the test scenarios, dependency usage, and requirement coverage for `Validation.cs`.

#### Verification Approach

`Validation` is verified with unit tests defined in `ValidationTests.cs`. Most tests create
controlled `Context` instances and call `Validation.Run` directly, verifying results file
generation and error handling for unsupported formats. The `ValidateCachePackagePath` helper is
additionally verified with direct unit tests exercising known-good and known-bad paths, proving
the exact-match identity check would catch a regression to substring matching.

#### Dependencies

| Dependency | Usage in Tests                                       |
|------------|------------------------------------------------------|
| `Context`  | Created from controlled argument arrays.             |

No mocking is required; all collaborators execute real logic.

#### Test Environment

Validation unit tests run under the standard xUnit v3 test runner within the
`DemaConsulting.NuGet.CacheTool.Tests` project. Tests that generate results files create
temporary files and clean them up on completion. Most scenarios require no external services or
network connectivity; the `Validation_Run_CachePackageSelfTest_PassesWithRealCachedPackagePath`
scenario invokes the tool's cache-package self-test against a real NuGet package identity, which
requires network access if that package is not already present in the local NuGet global
packages folder.

#### Acceptance Criteria

The Validation unit test suite passes when all of the following conditions are met:

- All test scenarios defined in `ValidationTests.cs` pass.
- Results files are created at the expected path with syntactically valid TRX or JUnit XML
  content.
- Every Validation unit requirement is covered by at least one passing scenario, per the
  ReqStream trace matrix.

#### Test Scenarios

##### Validation_Run_TrxResultsRequested_WritesTrxFile

**Scenario**: `Validation.Run` is called with `["--validate", "--silent", "--results", trxFile]`.

**Expected**: Exit code 0; TRX results file is created containing `<TestRun` and `</TestRun>`.

**Requirement coverage**: `NuGetCache-Validation-ResultsFile`.

##### Validation_Run_JUnitResultsRequested_WritesJUnitFile

**Scenario**: `Validation.Run` is called with `["--validate", "--silent", "--results", xmlFile]`.

**Expected**: Exit code 0; JUnit XML results file is created containing `<testsuites`,
`<testsuite`, and `<testcase`.

**Requirement coverage**: `NuGetCache-Validation-ResultsFile`.

##### Validation_Run_UnsupportedResultsFormat_ReportsError

**Scenario**: `Validation.Run` is called with `["--validate", "--silent", "--results", "output.json"]`.

**Expected**: Exit code non-zero.

**Requirement coverage**: `NuGetCache-Validation-ResultsFile`.

##### Validation_Run_WithSilentContext_PrintsSummary

**Scenario**: `Validation.Run` is called with `["--silent", "--log", logFile]`; output goes to
the log file only.

**Expected**: Log file contains "Total Tests:", "Passed:", and "Failed:".

**Requirement coverage**: `NuGetCache-Validation-SelfValidation`.

##### Validation_Run_CachePackageSelfTest_PassesWithRealCachedPackagePath

**Scenario**: `Validation.Run` is called with `["--validate"]` against a real, populated NuGet
global packages folder; standard output is captured.

**Expected**: Captured output contains "Cache Package Test - PASSED"; exit code 0.

**Requirement coverage**: `NuGetCache-Validation-SelfValidation`.

##### Validation_ValidateCachePackagePath_ExactMatch_ReturnsNull

**Scenario**: `Validation.ValidateCachePackagePath` is called with a path whose directory name
and parent directory name exactly match the expected version and package ID.

**Expected**: Returns `null` (no error).

**Requirement coverage**: `NuGetCache-Validation-SelfValidation`.

##### Validation_ValidateCachePackagePath_VersionSuffixSubstringMatch_ReturnsError

**Scenario**: `Validation.ValidateCachePackagePath` is called with a version directory name that
contains the expected version as a prefix followed by a suffix (e.g. `0.1.0-beta` against
expected `0.1.0`).

**Expected**: Returns a non-null error message, proving the check is an exact match rather than a
substring match.

**Requirement coverage**: `NuGetCache-Validation-SelfValidation`.

##### Validation_ValidateCachePackagePath_VersionPrefixSubstringMatch_ReturnsError

**Scenario**: `Validation.ValidateCachePackagePath` is called with a version directory name that
contains the expected version as a substring preceded by extra digits (e.g. `10.1.0` against
expected `0.1.0`).

**Expected**: Returns a non-null error message, proving the check is an exact match rather than a
substring match.

**Requirement coverage**: `NuGetCache-Validation-SelfValidation`.

##### Validation_ValidateCachePackagePath_WrongPackageId_ReturnsError

**Scenario**: `Validation.ValidateCachePackagePath` is called with a path whose parent directory
name does not match the expected package ID.

**Expected**: Returns a non-null error message.

**Requirement coverage**: `NuGetCache-Validation-SelfValidation`.

##### Validation_ValidateCachePackagePath_NoParentDirectory_ReturnsError

**Scenario**: `Validation.ValidateCachePackagePath` is called with a root path (`"/"`), for which
`Path.GetDirectoryName` returns `null` rather than a parent directory name.

**Expected**: Returns a non-null error message, proving the null-parent-directory case is handled
as a non-match rather than throwing.

**Requirement coverage**: `NuGetCache-Validation-SelfValidation`.

##### Validation_ValidateCachePackagePath_BareRelativeName_ReturnsError

**Scenario**: `Validation.ValidateCachePackagePath` is called with a bare relative directory name
with no path separator (`"0.1.0"`), for which `Path.GetDirectoryName` returns an empty string
rather than `null`.

**Expected**: Returns a non-null error message, proving the empty-parent-directory-name case is
handled as a non-match.

**Requirement coverage**: `NuGetCache-Validation-SelfValidation`.
