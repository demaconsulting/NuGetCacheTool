### Context Unit Design

![Cli Structure](CliView.svg)

#### Purpose

The `Context` class provides command-line argument parsing and centralized output
management for the NuGet Cache Tool. It acts as the primary data carrier between
the entry point and all operational units.

#### Data Model

| Member | Type | Description |
| ------ | ---- | ----------- |
| `Version` | `bool` | True when `-v` or `--version` flag is present |
| `Help` | `bool` | True when `-?`, `-h`, or `--help` flag is present |
| `Silent` | `bool` | True when `--silent` flag is present; suppresses console output |
| `Validate` | `bool` | True when `--validate` flag is present |
| `ResultsFile` | `string?` | Path from `--results` flag; null if not specified |
| `Packages` | `IReadOnlyList<string>` | Package arguments in `[package]:[version]` form |
| `ExitCode` | `int` | Process exit code; set to 1 on any error |
| `_logWriter` | `StreamWriter?` | Log file writer; null if `--log` not specified |
| `_hasErrors` | `bool` | Internal flag set by `WriteError`; drives `ExitCode` |

#### Key Methods

##### Create(string[] args) — static factory

Parses command-line arguments and returns a configured `Context` instance.

- **Preconditions**: `args` is a non-null string array
- **Postconditions**: returns a `Context` with all flags and packages populated; log file open if `--log` was specified
- **Algorithm**: instantiates `ArgumentParser` and processes `args` left-to-right, recognizing flags and package arguments; multi-token arguments (`--log`, `--results`) consume the following token as their value

##### WriteLine(string message)

Writes a line to stdout (if not in silent mode) and to the log file (if open).

- **Preconditions**: context is not disposed
- **Postconditions**: line written to console and/or log file

##### WriteError(string message)

Writes a line to stderr (if not in silent mode), to the log file (always if open), and sets `ExitCode` to 1.

- **Preconditions**: context is not disposed
- **Postconditions**: line written to stderr and/or log file; `ExitCode` is 1

##### Dispose()

Flushes and closes the log file writer if open.

- **Preconditions**: context may or may not have been disposed before
- **Postconditions**: `_logWriter` is closed and set to null; safe to call multiple times

#### ArgumentParser Inner Class

`ArgumentParser` is a private inner class that implements the argument parsing
state machine. It processes `string[] args` sequentially, recognizing flags and
package arguments:

- `-v`, `--version` → sets `Version = true`
- `-?`, `-h`, `--help` → sets `Help = true`
- `--silent` → sets `Silent = true`
- `--validate` → sets `Validate = true`
- `--log <file>` → opens log file, sets `_logWriter`
- `--results <file>` → sets `ResultsFile`
- `[package]:[version]` → appends to `Packages` list
- Any other argument → throws `ArgumentException`

#### Key Algorithms

##### Argument Parsing

Arguments are consumed left-to-right. Multi-token arguments (`--log`, `--results`)
consume the following token as their value, throwing `ArgumentException` if no
value follows.

##### Output Management

`WriteLine(string)` writes to the console (unless silent) and to the log file if open.
`WriteError(string)` writes to stderr (unless silent), writes to the log file, and
sets `_hasErrors = true`, which causes `ExitCode` to return `1`.

#### Error Handling

| Scenario | Behavior |
| -------- | -------- |
| Unknown argument in `Create()` | Throws `ArgumentException` identifying the unsupported argument |
| `--log` or `--results` flag without a value | Throws `ArgumentException` |
| Log file cannot be opened | Throws `InvalidOperationException` wrapping the underlying I/O exception message |
| `WriteError()` called | Sets `_hasErrors = true` (causing `ExitCode` to return 1); writes message to stderr and log file |
| `WriteLine()` called in silent mode | Suppresses console output; still writes to log file if open |
| `Dispose()` called multiple times | Safe; the `StreamWriter` is set to null after first disposal |

Argument-parsing errors propagate to `Program.Main`, which catches both `ArgumentException` and
`InvalidOperationException` (for example, when the log file cannot be opened) and
writes the message to `Console.Error` before returning exit code 1.

#### Dependencies

`Context` depends only on the .NET BCL: `System.IO.StreamWriter` for log-file output and
`System.Console` for stdout/stderr. It does not depend on any other unit in the NuGet
Cache Tool system; it is a low-level building block consumed by `Program` and `Validation`.

#### Callers

- **`Program`**: `Program.Main` creates the context via `Context.Create` and passes it to
  `Program.Run`.
- **`Validation`** (SelfTest subsystem): `Validation.Run` receives a `Context` instance and
  uses it for output and results-path resolution.

#### Resource Management

`Context` implements `IDisposable`. The `Dispose()` method flushes and closes the
`StreamWriter` opened for the `--log` file (if any). Callers must use a `using`
statement (or `using` declaration) to ensure the log file is properly closed even
if an exception is thrown.

`_logWriter` is configured with `AutoFlush = true` so that log entries are written
to disk immediately, preventing data loss if the process terminates unexpectedly.
