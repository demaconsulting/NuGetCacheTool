## CLI Subsystem Verification

This document describes the subsystem-level verification design for the `Cli` subsystem. It
defines the overall verification strategy and test scenarios for the CLI subsystem. Requirement
traceability is tracked in the ReqStream trace matrix, not in this document.

### Verification Approach

The CLI subsystem is verified with subsystem integration tests defined in `CliTests.cs`. These
tests exercise the public API of the `Context` class directly, simulating all supported
command-line argument combinations and verifying the resulting context state and output behavior.

No mocking is required; the tests exercise the full CLI parsing and output management logic.

### Dependencies

| Dependency | Usage in Tests                                              |
|------------|-------------------------------------------------------------|
| `Context`  | Exercised directly through the `Context.Create` factory.    |

### Test Environment

CLI subsystem tests run under the standard xUnit v3 test runner within the
`DemaConsulting.NuGet.CacheTool.Tests` project. No additional configuration, external services,
or environment variables are required. Console streams are redirected within each test to capture
output and verify behavior independently of global state.

### Acceptance Criteria

The CLI subsystem test suite passes when all of the following conditions are met:

- All test scenarios defined in `CliTests.cs` pass.
- No scenario produces an unexpected context state, exit code, or console output.
- Every CLI subsystem requirement is covered by at least one passing scenario, as tracked in the
  ReqStream trace matrix.

### Test Scenarios

#### Cli_VersionFlag_SetsVersionOnContext

**Scenario**: Context is created from `["--version"]`.

**Expected**: `context.Version` is true; `context.Help` is false; exit code is 0.

#### Cli_ShortVersionFlag_SetsVersionOnContext

**Scenario**: Context is created from `["-v"]`.

**Expected**: `context.Version` is true; exit code is 0.

#### Cli_HelpFlag_SetsHelpOnContext

**Scenario**: Context is created from `["--help"]`.

**Expected**: `context.Help` is true; `context.Version` is false; exit code is 0.

#### Cli_ShortHelpFlagH_SetsHelpOnContext

**Scenario**: Context is created from `["-h"]`.

**Expected**: `context.Help` is true; exit code is 0.

#### Cli_ShortHelpFlagQuestionMark_SetsHelpOnContext

**Scenario**: Context is created from `["-?"]`.

**Expected**: `context.Help` is true; exit code is 0.

#### Cli_SilentFlag_SuppressesAllOutput

**Scenario**: Context is created from `["--silent"]`; `context.WriteLine` and
`context.WriteError` are called.

**Expected**: Neither message appears on stdout or stderr.

#### Cli_PackageArgument_AddedToPackagesList

**Scenario**: Context is created from `["Package.One:1.0.0", "Package.Two:2.3.4"]`.

**Expected**: `context.Packages` contains exactly 2 entries matching both arguments.

#### Cli_ErrorOutput_SetsNonZeroExitCode

**Scenario**: Context is created from `[]`; `context.WriteError` is called.

**Expected**: Exit code is non-zero.

#### Cli_ErrorOutput_WritesMessageToConsole

**Scenario**: Context is created from `[]`; `context.WriteError` is called with a message.

**Expected**: The error message appears in stderr.

#### Cli_UnknownArgument_ThrowsArgumentException

**Scenario**: Context is created from `["--unknown-flag"]`.

**Expected**: `ArgumentException` is thrown with a message containing "Unsupported argument".

#### Cli_LogFlag_WritesToLogFile

**Scenario**: Context is created from `["--log", logFile]`; `context.WriteLine` is called.

**Expected**: The log file is created and contains the written message.

#### Cli_ValidateFlag_SetsValidateOnContext

**Scenario**: Context is created from `["--validate"]`.

**Expected**: `context.Validate` is true; exit code is 0.

#### Cli_ResultsFlag_SetsResultsFileOnContext

**Scenario**: Context is created from `["--results", "results.trx"]`.

**Expected**: `context.ResultsFile` equals "results.trx"; exit code is 0.

#### Cli_LogFlagWithoutValue_ThrowsArgumentException

**Scenario**: Context is created from `["--log"]` (missing value).

**Expected**: `ArgumentException` is thrown.

#### Cli_ResultsFlagWithoutValue_ThrowsArgumentException

**Scenario**: Context is created from `["--results"]` (missing value).

**Expected**: `ArgumentException` is thrown.

#### Cli_SilentAndLog_WritesToLogFileOnly

**Scenario**: Context is created from `["--silent", "--log", logFile]`; `context.WriteLine` is
called.

**Expected**: Message does not appear on stdout; log file is created and contains the message.
