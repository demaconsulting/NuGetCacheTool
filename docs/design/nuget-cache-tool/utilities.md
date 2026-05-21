## NuGet Cache Tool Utilities Subsystem Design

### Overview

The Utilities subsystem provides shared helper functionality used by production code and
test infrastructure. It contains two units: `TemporaryDirectory`, which manages the
lifecycle of a temporary directory for use during self-test execution and unit testing,
and `PathHelpers`, which provides safe path combination utilities that prevent
path-traversal attacks.

### Interfaces

**Exposed to the rest of the system:**

- `PathHelpers.SafePathCombine(string basePath, string relativePath)` — combines two paths
  and rejects any result that escapes the base directory; throws `ArgumentNullException` for
  null arguments and `ArgumentException` for traversal or absolute-path overrides.
- `TemporaryDirectory()` constructor — creates a uniquely-named subdirectory under
  `Path.GetFullPath(Path.GetTempPath())`. Throws `InvalidOperationException` if the directory
  cannot be created.
- `TemporaryDirectory.DirectoryPath` — the full path to the created directory.
- `TemporaryDirectory.GetFilePath(string relativePath)` — returns the absolute path to a
  file within the temporary directory, creating any intermediate subdirectories.
  Throws `ArgumentNullException` when `relativePath` is null; throws `ArgumentException`
  when `relativePath` escapes the directory boundary.
- `TemporaryDirectory.Dispose()` — deletes the temporary directory and all its contents;
  `IOException` and `UnauthorizedAccessException` are suppressed.

**Consumed from other items:**

N/A - the Utilities subsystem has no external dependencies; `PathHelpers` depends only on
`System.IO.Path` from the .NET BCL, and `TemporaryDirectory` uses `PathHelpers` internally
within the same subsystem.

### Design

`TemporaryDirectory` uses `Path.GetFullPath(Path.GetTempPath())` as the base for the
temporary directory. This allows temporary-directory creation to succeed when the
current working directory is read-only.

#### Unit Collaboration

`TemporaryDirectory` uses `PathHelpers.SafePathCombine` internally (both units are within
the Utilities subsystem) to enforce the path boundary in `GetFilePath`. Any path-traversal
attempt surfaced by `GetFilePath` is detected and rejected by `SafePathCombine`.

#### Error Handling

| Scenario | Behavior |
| -------- | -------- |
| Directory cannot be created (I/O error, access denied) | Constructor throws `InvalidOperationException` wrapping the original exception |
| `relativePath` is null | `GetFilePath` throws `ArgumentNullException` (from `PathHelpers`) |
| `relativePath` escapes the boundary (e.g. `../escape`) | `GetFilePath` throws `ArgumentException` (from `PathHelpers`) |
| Directory deletion fails during disposal | `Dispose` suppresses `IOException` and `UnauthorizedAccessException` |
