## CLI Subsystem Design

![Cli Structure](CliView.svg)

### Overview

The CLI subsystem provides command-line interface functionality for the NuGet Cache Tool.
It handles argument parsing and output management, translating raw command-line arguments
into structured options and commands that are executed by the top-level `Program` unit.

**Responsibilities**:

- Parse and validate all command-line arguments
- Manage output channels (console, log file, silent mode)
- Track error state and determine the process exit code
- Provide structured command/context information for top-level operations such as version
  display, help display, package caching, and self-validation
- Write output to a log file when --log is specified, even in silent mode

**Units**:

| Unit | Class | Description |
| ---- | ----- | ----------- |
| Context | `Context.cs` | Command-line argument parsing and output management |

### Interfaces

The `Context` unit exposes the following public API:

| Member | Type | Description |
| ------ | ---- | ----------- |
| `Create(string[] args)` | static factory | Parses arguments and returns a configured `Context` instance |
| `Version` | `bool` property | True when `--version` or `-v` was supplied |
| `Help` | `bool` property | True when `--help`, `-h`, or `-?` was supplied |
| `Silent` | `bool` property | True when `--silent` was supplied |
| `Validate` | `bool` property | True when `--validate` was supplied |
| `Packages` | `IReadOnlyList<string>` property | List of `package:version` strings to cache |
| `ResultsFile` | `string?` property | Path to results file supplied with `--results`, or null |
| `ExitCode` | `int` property | Current exit code; 0 = success, 1 = failure |
| `WriteLine(string)` | method | Writes a line to stdout (and log file if open); suppressed in silent mode |
| `WriteError(string)` | method | Writes a line to stderr (suppressed in silent mode), and to the log file (always); sets ExitCode to 1 |
| `Dispose()` | method | Flushes and closes the log file writer if open |

### Design

#### Interactions

| Dependency | Direction | Description |
| ---------- | --------- | ----------- |
| `Program` | Top-level unit (entry point) | Creates `Context` and dispatches to CLI subsystem |
| `SelfTest` subsystem | Upstream | `Program` passes the `Context` it created (via CLI) to `Validation.Run`; `Validation` consumes `Context` for output and results-path resolution |

#### Error Handling

| Scenario | Behavior |
| -------- | -------- |
| Unknown or malformed argument | `Create()` throws `ArgumentException` with a message |
| | identifying the unsupported argument |
| `--log` flag without a value | `Create()` throws `ArgumentException` |
| `--results` flag without a value | `Create()` throws `ArgumentException` |
| Log file cannot be opened | `Create()` throws `InvalidOperationException` with the underlying I/O error message |
| `WriteError()` called | Sets `ExitCode` to 1; writes message to stderr (and log file if open) |

Exit code semantics: `ExitCode` starts at 0 and is set to 1 on the first call to `WriteError()`.
`Program` reads `context.ExitCode` as its return value after `Run()` completes.
