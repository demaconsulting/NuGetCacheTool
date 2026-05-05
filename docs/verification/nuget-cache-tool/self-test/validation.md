### Validation Verification

This document describes the unit-level verification design for the `Validation` unit. It defines
the test scenarios, dependency usage, and requirement coverage for `Validation.cs`.

#### Verification Approach

`Validation` is verified with unit tests defined in `ValidationTests.cs`. The tests create
controlled `Context` instances and call `Validation.Run` directly, verifying results file
generation and error handling for unsupported formats.

#### Dependencies

| Dependency | Usage in Tests                                       |
|------------|------------------------------------------------------|
| `Context`  | Created from controlled argument arrays.             |

No mocking is required; all collaborators execute real logic.

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

#### Requirements Coverage

- **`NuGetCache-Validation-SelfValidation`**: Validation_Run_WithSilentContext_PrintsSummary
- **`NuGetCache-Validation-ResultsFile`**: Validation_Run_TrxResultsRequested_WritesTrxFile,
  Validation_Run_JUnitResultsRequested_WritesJUnitFile,
  Validation_Run_UnsupportedResultsFormat_ReportsError
