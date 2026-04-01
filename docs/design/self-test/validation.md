# Validation Unit Design

## Purpose

`Validation` provides a self-validation test framework that executes the tool as a
subprocess and verifies observable outputs. It confirms that all software units
work correctly in the deployment environment.

## Test Structure

Three tests are executed unconditionally:

| Test | Validates |
| ---- | --------- |
| `RunVersionTest` | `--version` flag outputs the version string |
| `RunHelpTest` | `--help` flag outputs usage information |
| `RunCachePackageTest` | Caching a known package produces a valid path |

## RunValidationTest Pattern

`RunValidationTest` is the common test runner used by all three tests. It:

1. Creates a `TemporaryDirectory` for isolated file output
2. Constructs a log file path using `PathHelpers.SafePathCombine`
3. Launches the tool with additional arguments and captures the log
4. Calls the caller-supplied `validator` delegate to check output
5. Records pass/fail in the shared `testResults` list

## TemporaryDirectory Inner Class

`TemporaryDirectory` is a disposable inner class that creates a uniquely named
temporary directory and deletes it (with all contents) on disposal. It ensures
test isolation and clean-up even when tests fail.

## Results File Writing

After all tests complete, `Validation.Run` writes the results file if
`context.ResultsFile` is non-null:

- `.trx` extension → serialised using `TrxSerializer`
- `.xml` extension → serialised using `JUnitSerializer`
- any other extension → treated as an error (unsupported results file extension)

## Interactions

| Dependency | Usage |
| ---------- | ----- |
| `Context` | Provides `ResultsFile` path and output methods |
| `PathHelpers` | `SafePathCombine` constructs temp log file paths |
| `DemaConsulting.TestResults` | `TrxSerializer`, `JUnitSerializer` for result output |
