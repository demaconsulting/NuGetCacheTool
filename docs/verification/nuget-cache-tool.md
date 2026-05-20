# System Verification

This document describes the system-level verification design for the NuGet Cache Tool. It
defines the overall verification strategy, test environments, interface simulation approach, and
end-to-end integration test scenarios that together demonstrate the system meets its requirements.

## Verification Strategy

System-level verification uses end-to-end integration tests that invoke the tool as a real process
via the `Runner.Run` helper in `IntegrationTests.cs`. Each test exercises the full stack — argument
parsing, dispatch, execution, and output — and validates both exit code and console output.

This approach ensures that system requirements are verified at the system boundary without assuming
any internal implementation detail. The tests treat the tool as a black box and assert on
observable outputs only.

**Note**: `Runner.Run` merges stdout and stderr into a single combined output string. Per-stream
assertions (e.g., "standard error is empty") are therefore not possible at the integration test
level; all assertions are made against the combined output.

## Test Environments

Integration tests are executed across the following environments to satisfy multi-runtime and
multi-platform requirements:

| Runtime    | Platform |
|------------|----------|
| .NET 8.0   | Windows  |
| .NET 8.0   | Linux    |
| .NET 8.0   | macOS    |
| .NET 9.0   | Windows  |
| .NET 9.0   | Linux    |
| .NET 9.0   | macOS    |
| .NET 10.0  | Windows  |
| .NET 10.0  | Linux    |
| .NET 10.0  | macOS    |

All integration test scenarios are expected to produce identical results on all supported runtime
and platform combinations.

## Acceptance Criteria

The system-level integration test suite passes when all of the following conditions are met:

- All integration test scenarios defined in `IntegrationTests.cs` pass on every supported runtime
  and platform combination (.NET 8, 9, and 10 on Windows, Linux, and macOS).
- No test scenario produces an unexpected exit code or output pattern.
- Every system-level requirement listed in the Requirements Coverage section is covered by at least
  one passing scenario.

## External Interface Simulation

At the system level, no interfaces are mocked. All external interfaces are exercised with real
implementations:

- **Standard output / standard error** — Captured by `Runner.Run` and returned as a combined
  string for assertion. Per-stream assertions are not available.
- **File system** — Temporary files and directories are created and cleaned up within each test.
  The `--results` and `--log` flags are exercised with real file paths under a temporary folder.
- **Process exit code** — Returned by `Runner.Run` and asserted directly.
- **NuGet package cache** — Real NuGet packages are downloaded and cached during tests that
  exercise the cache command.
- **Path construction** — The `IntegrationTests` constructor uses `PathHelpers.SafePathCombine`
  (Utilities subsystem) to locate the tool DLL at a path derived from `AppContext.BaseDirectory`.

## Integration Test Scenarios

The following integration test scenarios are defined in `IntegrationTests.cs`.

### NuGetCacheTool_VersionDisplay_VersionFlagProvided_OutputsVersion

**Scenario**: The `--version` flag is passed as the sole argument.

**Expected**: Exit code 0; combined output is non-empty and does not contain "Error" or
"Copyright".

### NuGetCacheTool_HelpDisplay_HelpFlagProvided_OutputsUsageInformation

**Scenario**: The `--help` flag is passed as the sole argument.

**Expected**: Exit code 0; combined output contains "Usage:", "Options:", and "--version".

### NuGetCacheTool_SelfValidation_ValidateFlagProvided_RunsValidation

**Scenario**: The `--validate` flag is passed as the sole argument.

**Expected**: Exit code 0; combined output contains "Total Tests:" and "Passed:".

### NuGetCacheTool_ResultsFile_ValidateWithTrxExtension_GeneratesTrxFile

**Scenario**: The `--validate` flag is combined with `--results <path>.trx` pointing to a
temporary file.

**Expected**: Exit code 0; a TRX file is created at the specified path containing `<TestRun`
and `</TestRun>` XML elements.

### NuGetCacheTool_ResultsFile_ValidateWithXmlExtension_GeneratesJUnitFile

**Scenario**: The `--validate` flag is combined with `--results <path>.xml` pointing to a
temporary file.

**Expected**: Exit code 0; an XML file is created at the specified path containing `<testsuites`,
`<testsuite`, and `<testcase` XML elements.

### NuGetCacheTool_SilentMode_SilentFlagProvided_SuppressesOutput

**Scenario**: The `--silent` flag is passed as the sole argument.

**Expected**: Exit code 0; combined output is empty or whitespace-only.

### NuGetCacheTool_LogFile_LogFlagProvided_WritesOutputToFile

**Scenario**: The `--log <path>` flag is passed pointing to a temporary file.

**Expected**: Exit code 0; the specified log file is created and contains "NuGet Cache Tool
version".

### NuGetCacheTool_ErrorHandling_UnknownArgumentProvided_ReturnsError

**Scenario**: An unrecognized argument (e.g., `--unknown`) is passed.

**Expected**: Exit code non-zero; combined output contains "Error".

### NuGetCacheTool_PackageCaching_ValidPackageProvided_OutputsPath

**Scenario**: A valid package argument `DemaConsulting.NuGet.Caching:0.1.0` is passed.

**Expected**: Exit code 0; combined output is non-empty and does not contain "Error".

### NuGetCacheTool_PackageCaching_NonexistentPackageProvided_ReturnsError

**Scenario**: An invalid package argument `DemaConsulting.NonExistent.Package.XYZ:99.99.99` is
passed.

**Expected**: Exit code non-zero; combined output contains "Error".

### NuGetCacheTool_LogFile_InvalidFilenameProvided_ReturnsError

**Scenario**: The `--log` flag is passed with an invalid file path
`/nonexistent_dir_xyz_abc/invalid.log`.

**Expected**: Exit code non-zero; combined output contains "Error".

## Requirements Coverage

**Note on `NuGetCache-Sys-PathSafety`**: This security requirement is verified at unit level
(see `docs/verification/nuget-cache-tool/utilities/path-helpers.md`). At the system level,
`NuGetCacheTool_LogFile_InvalidFilenameProvided_ReturnsError` exercises error handling for
invalid paths, but dedicated path-traversal prevention is verified in the PathHelpers unit tests.

The following list maps each system-level requirement to the integration test scenarios that
verify it.

- **`NuGetCache-Sys-Integration`**: NuGetCacheTool_VersionDisplay_VersionFlagProvided_OutputsVersion,
  NuGetCacheTool_HelpDisplay_HelpFlagProvided_OutputsUsageInformation, NuGetCacheTool_PackageCaching_ValidPackageProvided_OutputsPath,
  NuGetCacheTool_SelfValidation_ValidateFlagProvided_RunsValidation, NuGetCacheTool_SilentMode_SilentFlagProvided_SuppressesOutput,
  NuGetCacheTool_LogFile_LogFlagProvided_WritesOutputToFile, NuGetCacheTool_ErrorHandling_UnknownArgumentProvided_ReturnsError,
  NuGetCacheTool_PackageCaching_NonexistentPackageProvided_ReturnsError
- **`NuGetCache-Sys-ValidateResults`**: NuGetCacheTool_ResultsFile_ValidateWithTrxExtension_GeneratesTrxFile,
  NuGetCacheTool_ResultsFile_ValidateWithXmlExtension_GeneratesJUnitFile
- **`NuGetCache-Sys-SilentMode`**: NuGetCacheTool_SilentMode_SilentFlagProvided_SuppressesOutput
- **`NuGetCache-Sys-LogFile`**: NuGetCacheTool_LogFile_LogFlagProvided_WritesOutputToFile,
  NuGetCacheTool_LogFile_InvalidFilenameProvided_ReturnsError
- **`NuGetCache-Sys-Banner`**: NuGetCacheTool_SelfValidation_ValidateFlagProvided_RunsValidation,
  NuGetCacheTool_LogFile_LogFlagProvided_WritesOutputToFile
- **`NuGetCache-Sys-InvalidArguments`**: NuGetCacheTool_ErrorHandling_UnknownArgumentProvided_ReturnsError
- **`NuGetCache-Sys-ExitCode`**: NuGetCacheTool_PackageCaching_NonexistentPackageProvided_ReturnsError,
  NuGetCacheTool_ErrorHandling_UnknownArgumentProvided_ReturnsError
- **`NuGetCache-Sys-StderrOutput`**: NuGetCacheTool_PackageCaching_NonexistentPackageProvided_ReturnsError,
  NuGetCacheTool_ErrorHandling_UnknownArgumentProvided_ReturnsError.
  **Accepted limitation**: `Runner.Run` merges stdout and stderr into a single combined
  output string, so these tests can only confirm that "Error" appears somewhere in the
  combined output rather than confirming the message was sent specifically to stderr.
  Stderr routing is verified at the unit level by `Context_WriteError_NotSilent_WritesToConsole`
  in `docs/verification/nuget-cache-tool/cli/context.md`.
- **`NuGetCache-Sys-SilentLogInteraction`**: NuGetCacheTool_LogFile_LogFlagProvided_WritesOutputToFile

- **`NuGetCache-Sys-PathSafety`**: Verified at unit level in
  `docs/verification/nuget-cache-tool/utilities/path-helpers.md`.
  The `PathHelpers.SafePathCombine` unit tests (PathHelpers_SafePathCombine_PathTraversalWithDoubleDots_ThrowsArgumentException,
  PathHelpers_SafePathCombine_WindowsAbsolutePath_ThrowsArgumentException,
  PathHelpers_SafePathCombine_UnixAbsolutePath_ThrowsArgumentException) directly exercise
  the path-traversal prevention mechanism. No integration-level scenario is required
  because path safety is a pure unit-level concern that does not depend on external interfaces.
