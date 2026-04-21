# Context Unit Design

## Purpose

The `Context` class provides command-line argument parsing and centralized output
management for the NuGet Cache Tool. It acts as the primary data carrier between
the entry point and all operational units.

## Data Model

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

## ArgumentParser Inner Class

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

## Key Algorithms

### Argument Parsing

Arguments are consumed left-to-right. Multi-token arguments (`--log`, `--results`)
consume the following token as their value, throwing `ArgumentException` if no
value follows.

### Output Management

`WriteLine(string)` writes to the console (unless silent) and to the log file if open.
`WriteError(string)` writes to stderr (unless silent), writes to the log file, and
sets `_hasErrors = true`, which causes `ExitCode` to return `1`.

## Interactions

- **Consumed by `Program`**: `Program.Main` creates the context and passes it to `Program.Run`
- **Consumed by `Validation`**: `Validation.Run` uses `Context` for output and results path
- **Consumed by `PathHelpers`**: indirectly via `Validation.Run` calling `SafePathCombine`
