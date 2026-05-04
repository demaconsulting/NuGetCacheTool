# CLI Subsystem Verification

This document describes the subsystem-level verification design for the `Cli` subsystem. It
defines the overall verification strategy and requirement coverage for the CLI subsystem.

## Verification Approach

The CLI subsystem is verified with subsystem integration tests defined in `CliTests.cs`. These
tests exercise the public API of the `Context` class directly, simulating all supported
command-line argument combinations and verifying the resulting context state and output behavior.

No mocking is required; the tests exercise the full CLI parsing and output management logic.

## Dependencies

| Dependency | Usage in Tests                                              |
|------------|-------------------------------------------------------------|
| `Context`  | Exercised directly through the `Context.Create` factory.   |

## Test Scenarios

### Cli_VersionFlag_SetsVersionOnContext

**Scenario**: Context is created from `["--version"]`.

**Expected**: `context.Version` is true; `context.Help` is false; exit code is 0.

**Requirement coverage**: `NuGetCache-Cli-VersionFlag`.

### Cli_ShortVersionFlag_SetsVersionOnContext

**Scenario**: Context is created from `["-v"]`.

**Expected**: `context.Version` is true; exit code is 0.

**Requirement coverage**: `NuGetCache-Cli-VersionFlag`.

### Cli_HelpFlag_SetsHelpOnContext

**Scenario**: Context is created from `["--help"]`.

**Expected**: `context.Help` is true; `context.Version` is false; exit code is 0.

**Requirement coverage**: `NuGetCache-Cli-HelpFlag`.

### Cli_ShortHelpFlagH_SetsHelpOnContext

**Scenario**: Context is created from `["-h"]`.

**Expected**: `context.Help` is true; exit code is 0.

**Requirement coverage**: `NuGetCache-Cli-HelpFlag`.

### Cli_ShortHelpFlagQuestionMark_SetsHelpOnContext

**Scenario**: Context is created from `["-?"]`.

**Expected**: `context.Help` is true; exit code is 0.

**Requirement coverage**: `NuGetCache-Cli-HelpFlag`.

### Cli_SilentFlag_SuppressesAllOutput

**Scenario**: Context is created from `["--silent"]`; `context.WriteLine` and
`context.WriteError` are called.

**Expected**: Neither message appears on stdout or stderr.

**Requirement coverage**: `NuGetCache-Cli-SilentFlag`.

### Cli_PackageArgument_AddedToPackagesList

**Scenario**: Context is created from `["Package.One:1.0.0", "Package.Two:2.3.4"]`.

**Expected**: `context.Packages` contains exactly 2 entries matching both arguments.

**Requirement coverage**: `NuGetCache-Cli-CachePackages`.

### Cli_ErrorOutput_SetsNonZeroExitCode

**Scenario**: Context is created from `[]`; `context.WriteError` is called.

**Expected**: Exit code is non-zero.

**Requirement coverage**: `NuGetCache-Cli-ErrorOutput`.

### Cli_ErrorOutput_WritesMessageToConsole

**Scenario**: Context is created from `[]`; `context.WriteError` is called with a message.

**Expected**: The error message appears in stderr.

**Requirement coverage**: `NuGetCache-Cli-ErrorOutput`.

### Cli_UnknownArgument_ThrowsArgumentException

**Scenario**: Context is created from `["--unknown-flag"]`.

**Expected**: `ArgumentException` is thrown with a message containing "Unsupported argument".

**Requirement coverage**: `NuGetCache-Cli-InvalidArguments`.

### Cli_LogFlag_WritesToLogFile

**Scenario**: Context is created from `["--log", logFile]`; `context.WriteLine` is called.

**Expected**: The log file is created and contains the written message.

**Requirement coverage**: `NuGetCache-Cli-LogFlag`.

### Cli_ValidateFlag_SetsValidateOnContext

**Scenario**: Context is created from `["--validate"]`.

**Expected**: `context.Validate` is true; exit code is 0.

**Requirement coverage**: `NuGetCache-Cli-ValidateFlag`.

### Cli_ResultsFlag_SetsResultsFileOnContext

**Scenario**: Context is created from `["--results", "results.trx"]`.

**Expected**: `context.ResultsFile` equals "results.trx"; exit code is 0.

**Requirement coverage**: `NuGetCache-Cli-ResultsFlag`.

### Cli_LogFlagWithoutValue_ThrowsArgumentException

**Scenario**: Context is created from `["--log"]` (missing value).

**Expected**: `ArgumentException` is thrown.

**Requirement coverage**: `NuGetCache-Cli-InvalidArguments`.

### Cli_ResultsFlagWithoutValue_ThrowsArgumentException

**Scenario**: Context is created from `["--results"]` (missing value).

**Expected**: `ArgumentException` is thrown.

**Requirement coverage**: `NuGetCache-Cli-InvalidArguments`.

### Cli_SilentAndLog_WritesToLogFileOnly

**Scenario**: Context is created from `["--silent", "--log", logFile]`; `context.WriteLine` is
called.

**Expected**: Message does not appear on stdout; log file is created and contains the message.

**Requirement coverage**: `NuGetCache-Cli-SilentLogInteraction`.

## Requirements Coverage

- **`NuGetCache-Cli-VersionFlag`**: Cli_VersionFlag_SetsVersionOnContext,
  Cli_ShortVersionFlag_SetsVersionOnContext
- **`NuGetCache-Cli-HelpFlag`**: Cli_HelpFlag_SetsHelpOnContext, Cli_ShortHelpFlagH_SetsHelpOnContext,
  Cli_ShortHelpFlagQuestionMark_SetsHelpOnContext
- **`NuGetCache-Cli-SilentFlag`**: Cli_SilentFlag_SuppressesAllOutput
- **`NuGetCache-Cli-CachePackages`**: Cli_PackageArgument_AddedToPackagesList
- **`NuGetCache-Cli-ErrorOutput`**: Cli_ErrorOutput_SetsNonZeroExitCode,
  Cli_ErrorOutput_WritesMessageToConsole
- **`NuGetCache-Cli-LogFlag`**: Cli_LogFlag_WritesToLogFile
- **`NuGetCache-Cli-ValidateFlag`**: Cli_ValidateFlag_SetsValidateOnContext
- **`NuGetCache-Cli-ResultsFlag`**: Cli_ResultsFlag_SetsResultsFileOnContext
- **`NuGetCache-Cli-InvalidArguments`**: Cli_UnknownArgument_ThrowsArgumentException,
  Cli_LogFlagWithoutValue_ThrowsArgumentException, Cli_ResultsFlagWithoutValue_ThrowsArgumentException
- **`NuGetCache-Cli-SilentLogInteraction`**: Cli_SilentAndLog_WritesToLogFileOnly
