### PathHelpers Unit Design

#### Purpose

`PathHelpers` provides safe path combination utilities that prevent path traversal
attacks when user-controlled path components are combined with a trusted base path.

#### Data Model

`PathHelpers` is a static class with no instance state and no persistent data. All
behavior is encapsulated in the pure static method `SafePathCombine`.

#### Key Methods

##### SafePathCombine(string basePath, string relativePath)

Combines `basePath` and `relativePath`, rejecting any result that escapes the base directory.

- **Preconditions**: both arguments are non-null
- **Postconditions**: returned path is within `basePath`; throws on traversal or absolute override
- **Algorithm**: (1) reject null inputs via `ArgumentNullException.ThrowIfNull`; (2) combine paths with `Path.Combine`; (3) resolve both `basePath` and combined path to absolute form using `Path.GetFullPath`; (4) call `Path.GetRelativePath(absoluteBase, absoluteCombined)` and reject if result is `".."`, starts with `".."`+separator, or is itself rooted

#### SafePathCombine Algorithm

`SafePathCombine(basePath, relativePath)` applies the following steps:

1. **Reject null inputs**: throws `ArgumentNullException` via
   `ArgumentNullException.ThrowIfNull` if either argument is `null`
2. **Combine paths**: calls `Path.Combine(basePath, relativePath)` to produce the
   candidate path (preserving the caller's relative/absolute style)
3. **Resolve to absolute form**: calls `Path.GetFullPath` on both `basePath` and the
   combined path
4. **GetRelativePath containment check**: calls
   `Path.GetRelativePath(absoluteBase, absoluteCombined)` and rejects the input if the
   result is exactly `".."`, starts with `".."` followed by `Path.DirectorySeparatorChar`
   or `Path.AltDirectorySeparatorChar`, or is itself rooted (absolute), which would
   indicate the combined path escapes the base directory

#### Security Properties

| Property | Guarantee |
| -------- | --------- |
| No parent traversal | Post-combine `GetRelativePath` check detects any traversal that escapes the base directory |
| No absolute override | Escape detected via `GetRelativePath` `..` prefix (same-root) or rooted result (cross-root) |
| Canonicalization check | `GetFullPath` normalizes paths; `GetRelativePath` checks `..`, `..`+sep, or rooted |
| Valid names with `..` prefix | Names like `..data` stay within the base and are correctly accepted |

#### Design Decisions

- **`Path.GetRelativePath` for containment check**: Using `GetRelativePath` to verify
  containment handles root paths (e.g. `/`, `C:\`), platform case-sensitivity, and
  directory-separator normalization natively. The containment test treats `..` as an
  escaping segment only when it is the entire relative result or is followed by a directory
  separator, avoiding false positives for valid in-base names such as `..data`.
- **Post-combine canonical-path check**: Resolving paths after combining handles all traversal
  patterns — `../`, embedded `/../`, absolute-path overrides, and platform edge cases —
  without fragile pre-combine string inspection of `relativePath`.
- **Specific exception for containment-check failure**: When the post-combine
  `GetRelativePath` containment check determines the combined path escapes the base
  directory, `SafePathCombine` throws a specific `ArgumentException` identifying
  `relativePath` as the problematic parameter, making debugging straightforward.
- **No logging or error accumulation**: `SafePathCombine` is a pure utility method that throws
  on invalid input; it does not interact with the `Context` or any output mechanism.

#### Interactions

- **Called by `Validation`**: constructs log file paths inside temporary directories

#### Error Handling

| Scenario | Exception thrown |
| -------- | ---------------- |
| `basePath` is null | `ArgumentNullException` (parameter: `basePath`) |
| `relativePath` is null | `ArgumentNullException` (parameter: `relativePath`) |
| `relativePath` is empty | Returns `basePath` unchanged (no exception) |
| Combined path escapes the base directory | `ArgumentException` (parameter: `relativePath`) |
| Combined path is absolute (cross-root override) | `ArgumentException` (parameter: `relativePath`) |

All errors are reported by throwing immediately; no partial state is produced and
no error accumulation occurs.

#### Dependencies

`PathHelpers` depends exclusively on `System.IO.Path` from the .NET BCL. It has no
dependency on other units, subsystems, or third-party packages.
