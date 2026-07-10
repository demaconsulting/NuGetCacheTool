### Context Verification

This document describes the unit-level verification design for the `Context` unit. It defines the
test scenarios, dependency usage, and requirement coverage for `Context.cs`.

#### Verification Approach

`Context` is verified with unit tests defined in `ContextTests.cs`. The tests exercise all
supported command-line flags, output methods, and error conditions. Console streams are redirected
within each test to capture output and verify behavior independently of global state.

#### Dependencies

No external dependencies are mocked. `Context` is tested by direct instantiation via
`Context.Create`.

#### Test Environment

Context unit tests run under the standard xUnit v3 test runner within the
`DemaConsulting.NuGet.CacheTool.Tests` project. Console streams are redirected within each test
to capture output. Tests that open log files create temporary files and clean them up on
completion. No external services are required.

#### Acceptance Criteria

The Context unit test suite passes when all of the following conditions are met:

- All test scenarios defined in `ContextTests.cs` pass.
- Console and log-file output assertions match expected values for every flag combination.
- Every Context unit requirement listed in the Requirements Coverage section is covered by at
  least one passing scenario.

#### Test Scenarios

##### Context_Create_NoArguments_ReturnsDefaultContext

**Scenario**: `Context.Create` is called with an empty argument array.

**Expected**: All flags are false; `ExitCode` is 0.

**Requirement coverage**: `NuGetCache-Context-ArgumentParsing`.

##### Context_Create_VersionFlag_SetsVersionTrue

**Scenario**: `Context.Create` is called with `["--version"]`.

**Expected**: `context.Version` is true; `context.Help` is false; `ExitCode` is 0.

**Requirement coverage**: `NuGetCache-Context-ArgumentParsing`.

##### Context_Create_ShortVersionFlag_SetsVersionTrue

**Scenario**: `Context.Create` is called with `["-v"]`.

**Expected**: `context.Version` is true; `context.Help` is false; `ExitCode` is 0.

**Requirement coverage**: `NuGetCache-Context-ArgumentParsing`.

##### Context_Create_HelpFlag_SetsHelpTrue

**Scenario**: `Context.Create` is called with `["--help"]`.

**Expected**: `context.Version` is false; `context.Help` is true; `ExitCode` is 0.

**Requirement coverage**: `NuGetCache-Context-ArgumentParsing`.

##### Context_Create_ShortHelpFlag_H_SetsHelpTrue

**Scenario**: `Context.Create` is called with `["-h"]`.

**Expected**: `context.Help` is true; `ExitCode` is 0.

**Requirement coverage**: `NuGetCache-Context-ArgumentParsing`.

##### Context_Create_ShortHelpFlag_Question_SetsHelpTrue

**Scenario**: `Context.Create` is called with `["-?"]`.

**Expected**: `context.Help` is true; `ExitCode` is 0.

**Requirement coverage**: `NuGetCache-Context-ArgumentParsing`.

##### Context_Create_SilentFlag_SetsSilentTrue

**Scenario**: `Context.Create` is called with `["--silent"]`.

**Expected**: `context.Silent` is true; `ExitCode` is 0.

**Requirement coverage**: `NuGetCache-Context-ArgumentParsing`, `NuGetCache-Context-SilentOutput`.

##### Context_Create_ValidateFlag_SetsValidateTrue

**Scenario**: `Context.Create` is called with `["--validate"]`.

**Expected**: `context.Validate` is true; `ExitCode` is 0.

**Requirement coverage**: `NuGetCache-Context-ArgumentParsing`.

##### Context_Create_ResultsFlag_SetsResultsFile

**Scenario**: `Context.Create` is called with `["--results", "test.trx"]`.

**Expected**: `context.ResultsFile` equals "test.trx"; `ExitCode` is 0.

**Requirement coverage**: `NuGetCache-Context-ArgumentParsing`.

##### Context_Create_LogFlag_OpensLogFile

**Scenario**: `Context.Create` is called with `["--log", logFile]`; a message is written.

**Expected**: `ExitCode` is 0; log file is created and contains the written message.

**Requirement coverage**: `NuGetCache-Context-ArgumentParsing`, `NuGetCache-Context-LogFile`.

##### Context_Create_PackageArgument_AddsToPackagesList

**Scenario**: `Context.Create` is called with `["DemaConsulting.NuGet.Caching:0.1.0"]`.

**Expected**: `context.Packages` contains exactly 1 entry equal to the argument.

**Requirement coverage**: `NuGetCache-Context-ArgumentParsing`.

##### Context_WriteLine_NotSilent_WritesToConsole

**Scenario**: `context.WriteLine` is called on a non-silent context.

**Expected**: The message appears in stdout.

**Requirement coverage**: `NuGetCache-Context-SilentOutput`.

##### Context_WriteLine_Silent_DoesNotWriteToConsole

**Scenario**: `context.WriteLine` is called on a silent context.

**Expected**: The message does not appear in stdout.

**Requirement coverage**: `NuGetCache-Context-SilentOutput`.

##### Context_WriteError_SetsErrorExitCode

**Scenario**: `context.WriteError` is called on a context with `ExitCode` 0.

**Expected**: `ExitCode` is 1 after the call.

**Requirement coverage**: `NuGetCache-Context-ErrorTracking`.

##### Context_WriteError_NotSilent_WritesToConsole

**Scenario**: `context.WriteError` is called on a non-silent context.

**Expected**: The message appears in stderr.

**Requirement coverage**: `NuGetCache-Context-ErrorTracking`.

##### Context_WriteError_WritesToLogFile

**Scenario**: `context.WriteError` is called on a silent context with a log file.

**Expected**: The message appears in the log file; `ExitCode` is 1.

**Requirement coverage**: `NuGetCache-Context-LogFile`.

##### Context_Create_UnknownArgument_ThrowsArgumentException

**Scenario**: `Context.Create` is called with `["--unknown"]`.

**Expected**: `ArgumentException` is thrown with "Unsupported argument" in the message.

**Requirement coverage**: `NuGetCache-Context-InvalidArguments-UnknownFlag`.

##### Context_Create_LogFlag_WithoutValue_ThrowsArgumentException

**Scenario**: `Context.Create` is called with `["--log"]` (no value following).

**Expected**: `ArgumentException` is thrown with "--log" in the message.

**Requirement coverage**: `NuGetCache-Context-InvalidArguments-MissingLogValue`.

##### Context_Create_ResultsFlag_WithoutValue_ThrowsArgumentException

**Scenario**: `Context.Create` is called with `["--results"]` (no value following).

**Expected**: `ArgumentException` is thrown with "--results" in the message.

**Requirement coverage**: `NuGetCache-Context-InvalidArguments-MissingResultsValue`.

##### Context_Create_WithoutColonInPackage_ThrowsArgumentException

**Scenario**: `Context.Create` is called with `["notapackage"]` (no colon separator).

**Expected**: `ArgumentException` is thrown.

**Requirement coverage**: `NuGetCache-Context-InvalidArguments-MalformedPackage`.

##### Context_WriteError_Silent_DoesNotWriteToConsole

**Scenario**: `context.WriteError` is called on a silent context.

**Expected**: The message does not appear in stderr.

**Requirement coverage**: `NuGetCache-Context-SilentOutput`.
