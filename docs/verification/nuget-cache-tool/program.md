## Program Verification

This document describes the unit-level verification design for the `Program` unit. It defines the
test scenarios, dependency usage, and requirement coverage for `Program.cs`.

### Verification Approach

`Program` is verified with unit tests defined in `ProgramTests.cs`. Because `Program` directly
instantiates `Context` from real arguments and calls `Validation.Run` when needed, no mocking is
required. The tests pass controlled argument arrays and assert on captured console output and exit
codes.

### Dependencies

| Dependency   | Usage in Tests                                                           |
|--------------|--------------------------------------------------------------------------|
| `Context`    | Used directly (not mocked) — created from the argument array under test. |
| `Validation` | Used directly (not mocked) — called when the validate flag is set.       |

No test doubles are introduced at the `Program` level; all collaborators execute their real logic.

### Test Environment

Program unit tests run under the standard xUnit v3 test runner within the
`DemaConsulting.NuGet.CacheTool.Tests` project. Tests capture console output by redirecting
standard streams within each test method. Tests that require NuGet package caching use the real
NuGet client against a live feed; network connectivity is required for those scenarios.

### Acceptance Criteria

The Program unit test suite passes when all of the following conditions are met:

- All test scenarios defined in `ProgramTests.cs` pass.
- Console output assertions match the expected content for every invocation pattern.
- Every Program unit requirement listed in the Requirements Coverage section is covered by at
  least one passing scenario.

### Test Scenarios

#### Program_Run_WithVersionFlag_DisplaysVersionOnly

**Scenario**: `Program.Run` is called with a context created from `["--version"]`.

**Expected**: Standard output does not contain "Copyright" or "NuGet Cache Tool version"; output
is non-empty; exit code is 0.

**Requirement coverage**: `NuGetCache-Program-VersionDisplay`.

#### Program_Run_WithHelpFlag_DisplaysUsageInformation

**Scenario**: `Program.Run` is called with a context created from `["--help"]`.

**Expected**: Standard output contains "Usage:", "Options:", "--version", and "--help"; exit code
is 0.

**Requirement coverage**: `NuGetCache-Program-HelpDisplay`.

#### Program_Run_WithValidateFlag_RunsValidation

**Scenario**: `Program.Run` is called with a context created from `["--validate"]`.

**Expected**: Standard output contains "Total Tests:"; exit code is 0.

**Requirement coverage**: `NuGetCache-Program-SelfValidation`.

#### Program_Run_NoArguments_DisplaysDefaultBehavior

**Scenario**: `Program.Run` is called with a context created from an empty argument array.

**Expected**: Standard output contains "NuGet Cache Tool version" and "Copyright"; exit code is 0.

**Requirement coverage**: `NuGetCache-Program-Banner`.

#### Program_Run_WithPackageArgument_CachesPackage

**Scenario**: `Program.Run` is called with a context created from
`["DemaConsulting.NuGet.Caching:0.1.0"]`.

**Expected**: Exit code 0; output contains the package name and version (case-insensitive).

**Requirement coverage**: `NuGetCache-Program-CachePackages`.

#### Program_Version_ReturnsNonEmptyString

**Scenario**: The `Program.Version` static property is read.

**Expected**: The returned string is non-empty and non-null.

**Requirement coverage**: `NuGetCache-Program-VersionDisplay`.

#### Program_Run_WithValidateAndUnsupportedResultsFormat_SetsErrorExitCode

**Scenario**: `Program.Run` is called with a context created from
`["--validate", "--results", "output.json"]`.

**Expected**: Exit code 1; error output contains "Error".

**Requirement coverage**: `NuGetCache-Program-ErrorOutput`.

### Requirements Coverage

- **`NuGetCache-Program-VersionDisplay`**: Program_Run_WithVersionFlag_DisplaysVersionOnly,
  Program_Version_ReturnsNonEmptyString
- **`NuGetCache-Program-HelpDisplay`**: Program_Run_WithHelpFlag_DisplaysUsageInformation
- **`NuGetCache-Program-CachePackages`**: Program_Run_WithPackageArgument_CachesPackage
- **`NuGetCache-Program-Banner`**: Program_Run_NoArguments_DisplaysDefaultBehavior
- **`NuGetCache-Program-ErrorOutput`**: Program_Run_WithValidateAndUnsupportedResultsFormat_SetsErrorExitCode
- **`NuGetCache-Program-SelfValidation`**: Program_Run_WithValidateFlag_RunsValidation
