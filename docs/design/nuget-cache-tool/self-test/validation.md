### Validation Unit Design

![SelfTest Structure](SelfTestView.svg)

#### Purpose

`Validation` provides a self-validation test framework that executes the tool
in-process and verifies observable outputs. It confirms that all software units
work correctly in the deployment environment.

#### Data Model

`Validation` accumulates results in a `DemaConsulting.TestResults.TestResults` container during
execution:

| Member | Type | Description |
| ------ | ---- | ----------- |
| `testResults` (local) | `DemaConsulting.TestResults.TestResults` | Accumulated pass/fail records for each test, populated by `RunValidationTest` |

`Validation` itself is a static class with no persistent instance state; the
`testResults` container is scoped to a single `Run` invocation.

#### Key Methods

##### Run(Context context)

Executes all self-validation tests and optionally writes results to the file specified by `context.ResultsFile`.

- **Preconditions**: `context` is a valid, non-disposed `Context`
- **Postconditions**: all three tests executed; pass/fail summary written via `context.WriteLine` (the `Failed` line is instead written via `context.WriteError` when the failed count is greater than zero); results file written if `context.ResultsFile` is non-null and has a supported extension
- **Algorithm**: (1) create `testResults` container; (2) call `RunVersionTest`, `RunHelpTest`, `RunCachePackageTest` unconditionally; (3) write summary; (4) if `ResultsFile` is non-null, write TRX (`.trx`) or JUnit XML (`.xml`) via the appropriate serializer; unsupported extensions call `context.WriteError`

##### RunValidationTest(Context context, TestResults testResults, string testName, string displayName, string[] additionalArgs, Func\<string, string?\> validator)

Common runner for all three self-validation tests.

- **Preconditions**: `context` and `testResults` are non-null; `testName` and `displayName` are non-null; `additionalArgs` is a valid argument array; `validator` is non-null
- **Postconditions**: a `TestResult` (pass or fail) is appended to the shared `testResults` container
- **Algorithm**: (1) create a `TemporaryDirectory` for isolation; (2) obtain log file path via
  `tempDir.GetFilePath`; (3) call `Program.Run` in-process with `--silent --log` and `additionalArgs`;
  (4) invoke `validator` with captured log content; (5) record pass or fail

#### Test Structure

Three tests are executed unconditionally:

| Test | Validates |
| ---- | --------- |
| `RunVersionTest` | `--version` flag outputs the version string |
| `RunHelpTest` | `--help` flag outputs usage information |
| `RunCachePackageTest` | Caching a known package produces a valid path |

#### RunValidationTest Pattern

`RunValidationTest` is the common test runner used by all three tests. It:

1. Creates a `TemporaryDirectory` for isolated file output
2. Obtains a log file path via `tempDir.GetFilePath`
3. Launches the tool with additional arguments and captures the log
4. Calls the caller-supplied `validator` delegate to check output
5. Records pass/fail in the shared `testResults` container

#### Results File Writing

After all tests complete, `Validation.Run` writes the results file if
`context.ResultsFile` is non-null:

- `.trx` extension → serialized using `TrxSerializer`
- `.xml` extension → serialized using `JUnitSerializer`
- any other extension → treated as an error (unsupported results file extension)

#### Dependencies

| Dependency | Usage |
| ---------- | ----- |
| `Context` | Provides `ResultsFile` path and output methods |
| `PathHelpers` (Utilities subsystem) | `SafePathCombine` constructs temp log file paths via `TemporaryDirectory.GetFilePath` |
| `TemporaryDirectory` | Used by `RunValidationTest` to create and manage isolated temporary directories for each test scenario |
| `Program` | Called in-process; `RunValidationTest` calls `Program.Run(testContext)` for each test |
| `DemaConsulting.TestResults` (OTS) | `TrxSerializer`, `JUnitSerializer` for result output |

#### Callers

`Program.Run` calls `Validation.Run(context)` when `context.Validate` is true. There are no
other callers of `Validation` in production code.

#### Error Handling

| Scenario | Behavior |
| -------- | -------- |
| A self-validation test assertion fails | `RunValidationTest` records a failed `TestResult` and continues with remaining tests |
| `Program.Run` throws an unhandled exception | `RunValidationTest` catches the exception, records a failed `TestResult`, and continues |
| `context.ResultsFile` extension is not `.trx` or `.xml` | `Run` calls `context.WriteError` with an unsupported-extension message; no file is written |
| `context.ResultsFile` is null | Results file writing is skipped entirely |

Test failures are always accumulated rather than propagated; the overall exit code
reflects the aggregate pass/fail outcome via `context.WriteError`.
