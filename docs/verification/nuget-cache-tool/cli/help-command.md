### HelpCommand Verification

This document describes the unit-level verification design for the `HelpCommand` unit. It defines
the test scenarios, dependency usage, and requirement coverage for `HelpCommand.cs`.

#### Verification Approach

`HelpCommand` is verified with unit tests defined in `HelpCommandTests.cs`. Each test creates a
help command with `HelpCommand.Create`, which writes to in-memory writers instead of the console.

#### Dependencies

No mocking is applied: the tests use the real help cards embedded in the tool assembly and the
real HelpCards library, a documented dependency of the unit.

#### Test Environment

HelpCommand unit tests run under the standard xUnit v3 test runner within the
`DemaConsulting.NuGet.CacheTool.Tests` project. No external services are required.

#### Acceptance Criteria

The HelpCommand unit test suite passes when all of the following conditions are met:

- All test scenarios defined in `HelpCommandTests.cs` pass.
- The help verb prints the topic index.
- The shipped catalog passes validation with warnings treated as errors.
- Every card prints in lines of at most 120 characters.

#### Test Scenarios

##### HelpCommand_TryRun_HelpVerb_PrintsTopicIndexAndReturnsZero

**Scenario**: `TryRun` is called with `["help"]`.

**Expected**: The command is answered with exit code 0, and the output lists the command of every
root topic.

**Requirement coverage**: `NuGetCache-HelpCommand-HelpVerb`.

##### HelpCommand_CreateOptions_ShippedCatalog_PassesValidation

**Scenario**: The catalog configuration from `CreateOptions` is validated with warnings treated as
errors.

**Expected**: No defect is reported, which proves that the root cards are exactly `RootCardIds`,
that every card carries one purpose tag, and that every command example parses as a command line
of the tool.

**Requirement coverage**: `NuGetCache-HelpCommand-ValidCards`.

##### HelpCommand_TryRun_EveryCard_FitsMaximumLineWidth

**Scenario**: Every card of the shipped catalog is printed with its links and tokens expanded and
a fixed, long prerelease version in place of the build's version.

**Expected**: Every card is answered with exit code 0 and has no line longer than 120 characters.

**Requirement coverage**: `NuGetCache-HelpCommand-LineWidth`.
