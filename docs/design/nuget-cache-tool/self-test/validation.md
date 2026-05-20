### Validation Unit Design

#### Purpose

`Validation` provides a self-validation test framework that executes the tool
in-process and verifies observable outputs. It confirms that all software units
work correctly in the deployment environment.

#### Data Model

`Validation` accumulates results in a list during execution:

| Member | Type | Description |
| ------ | ---- | ----------- |
| `testResults` (local) | `List<TestResult>` | Accumulated pass/fail records for each test, populated by `RunValidationTest` |

`Validation` itself is a static class with no persistent instance state; the
`testResults` list is scoped to a single `Run` invocation.

#### Key Methods

##### Run(Context context)

Executes all self-validation tests and optionally writes results to the file specified by `context.ResultsFile`.

- **Preconditions**: `context` is a valid, non-disposed `Context`
- **Postconditions**: all three tests executed; pass/fail summary written via `context.WriteLine`; results file written if `context.ResultsFile` is non-null and has a supported extension
- **Algorithm**: (1) create `testResults` list; (2) call `RunVersionTest`, `RunHelpTest`, `RunCachePackageTest` unconditionally; (3) write summary; (4) if `ResultsFile` is non-null, write TRX (`.trx`) or JUnit XML (`.xml`) via the appropriate serializer; unsupported extensions call `context.WriteError`

##### RunValidationTest(string testName, string[] args, Action\<string\> validator)

Common runner for all three self-validation tests.

- **Preconditions**: `testName` is non-null; `args` is a valid argument array; `validator` is non-null
- **Postconditions**: a `TestResult` (pass or fail) is appended to the shared `testResults` list
- **Algorithm**: (1) create a `TemporaryDirectory` for isolation; (2) construct log file path using `PathHelpers.SafePathCombine`; (3) call `Program.Run` in-process with `--silent --log` and `args`; (4) invoke `validator` with captured log content; (5) record pass or fail

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
2. Constructs a log file path using `PathHelpers.SafePathCombine`
3. Launches the tool with additional arguments and captures the log
4. Calls the caller-supplied `validator` delegate to check output
5. Records pass/fail in the shared `testResults` list

#### TemporaryDirectory Inner Class

`TemporaryDirectory` is a disposable inner class that creates a uniquely named
temporary directory and deletes it (with all contents) on disposal. It ensures
test isolation and clean-up even when tests fail.

#### Results File Writing

After all tests complete, `Validation.Run` writes the results file if
`context.ResultsFile` is non-null:

- `.trx` extension → serialized using `TrxSerializer`
- `.xml` extension → serialized using `JUnitSerializer`
- any other extension → treated as an error (unsupported results file extension)

#### Interactions

| Dependency | Usage |
| ---------- | ----- |
| `Context` | Provides `ResultsFile` path and output methods |
| `PathHelpers` | `SafePathCombine` constructs temp log file paths |
| `Program` | Called in-process; `RunValidationTest` calls `Program.Run(testContext)` for each test |
| `DemaConsulting.TestResults` | `TrxSerializer`, `JUnitSerializer` for result output |

#### Error Handling

| Scenario | Behavior |
| -------- | -------- |
| A self-validation test assertion fails | `RunValidationTest` records a failed `TestResult` and continues with remaining tests |
| `Program.Run` throws an unhandled exception | `RunValidationTest` catches the exception, records a failed `TestResult`, and continues |
| `context.ResultsFile` extension is not `.trx` or `.xml` | `Run` calls `context.WriteError` with an unsupported-extension message; no file is written |
| `context.ResultsFile` is null | Results file writing is skipped entirely |

Test failures are always accumulated rather than propagated; the overall exit code
reflects the aggregate pass/fail outcome via `context.WriteError`.
