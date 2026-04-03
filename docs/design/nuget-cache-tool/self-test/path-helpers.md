# PathHelpers Unit Design

## Purpose

`PathHelpers` provides safe path combination utilities that prevent path traversal
attacks when user-controlled path components are combined with a trusted base path.

## SafePathCombine Algorithm

`SafePathCombine(basePath, relativePath)` applies the following steps:

1. **Reject null inputs**: throws `ArgumentNullException` via `ArgumentNullException.ThrowIfNull` if either argument is `null`
2. **Combine paths**: calls `Path.Combine(basePath, relativePath)` to produce the candidate path (preserving the caller's relative/absolute style)
3. **Resolve to absolute form**: calls `Path.GetFullPath` on both `basePath` and the combined path
4. **GetRelativePath containment check**: calls `Path.GetRelativePath(absoluteBase, absoluteCombined)` and rejects the input if the result is exactly `".."`, starts with `".."` followed by `Path.DirectorySeparatorChar` or `Path.AltDirectorySeparatorChar`, or is itself rooted (absolute), which would indicate the combined path escapes the base directory

## Security Properties

| Property | Guarantee |
| -------- | --------- |
| No parent traversal | Post-combine `GetRelativePath` check detects any traversal that escapes the base directory |
| No absolute override | Rooted `checkRelative` result is detected and rejected |
| Canonicalisation check | `GetFullPath` normalizes paths; `GetRelativePath` confirms path stays within `basePath` |
| Valid names with `..` prefix | Names like `..data` stay within the base and are correctly accepted |

## Design Decisions

- **`Path.GetRelativePath` for containment check**: Using `GetRelativePath` to verify
  containment handles root paths (e.g. `/`, `C:\`), platform case-sensitivity, and
  directory-separator normalization natively. The containment test treats `..` as an
  escaping segment only when it is the entire relative result or is followed by a directory
  separator, avoiding false positives for valid in-base names such as `..data`.
- **Post-combine canonical-path check**: Resolving paths after combining handles all traversal
  patterns — `../`, embedded `/../`, absolute-path overrides, and platform edge cases —
  without fragile pre-combine string inspection of `relativePath`.
- **ArgumentException on invalid input**: Callers receive a specific `ArgumentException`
  identifying `relativePath` as the problematic parameter, making debugging straightforward.
- **No logging or error accumulation**: `SafePathCombine` is a pure utility method that throws
  on invalid input; it does not interact with the `Context` or any output mechanism.

## Interactions

- **Called by `Validation`**: constructs log file paths inside temporary directories
- **Indirectly tested**: `PathHelpersTests` covers all valid and invalid input cases
