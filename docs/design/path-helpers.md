# PathHelpers Unit Design

## Purpose

`PathHelpers` provides safe path combination utilities that prevent path traversal
attacks when user-controlled path components are combined with a trusted base path.

## SafePathCombine Algorithm

`SafePathCombine(basePath, relativePath)` applies layered defences:

1. **Reject `..` segments**: throws `ArgumentException` if `relativePath` contains `..`
2. **Reject absolute paths**: throws `ArgumentException` if `Path.IsPathRooted(relativePath)` is true
3. **Combine paths**: calls `Path.Combine(basePath, relativePath)`
4. **GetFullPath check**: calls `Path.GetFullPath` on the result and verifies the
   combined path starts with the fully-resolved `basePath`, providing defence-in-depth
   against OS-specific traversal sequences

## Security Properties

| Property | Guarantee |
| -------- | --------- |
| No parent traversal | `..` in relative path is rejected before combination |
| No absolute override | Absolute relative paths cannot escape the base directory |
| Canonicalisation check | `GetFullPath` resolves symlinks and OS quirks as a final guard |

## Interactions

- **Called by `Validation`**: constructs log file paths inside temporary directories
- **Indirectly tested**: `PathHelpersTests` covers all valid and invalid input cases
