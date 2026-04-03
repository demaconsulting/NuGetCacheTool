# PathHelpers Security Review - Attack Vector Analysis

## Known Path Traversal Attack Patterns

1. **Basic parent directory traversal**: `../../../etc/passwd`
2. **Absolute path override**: `/etc/passwd` or `C:\Windows\System32`
3. **Current directory with traversal**: `./../../etc/passwd`
4. **URL-encoded traversal**: `%2e%2e%2f` (not applicable in file paths)
5. **Backslash on Unix**: `..\..\..\etc\passwd`
6. **Mixed separators**: `..\../etc/passwd`
7. **Embedded traversal**: `subfolder/../../etc/passwd`
8. **Legitimate ".." prefix**: `..data`, `..gitignore`
9. **Empty string**: ``
10. **Dot only**: `.`
11. **Double-dot only**: `..`

## PathHelpers.SafePathCombine Defense Analysis

### Defense Mechanism
```csharp
1. Path.Combine(basePath, relativePath)          // Combine paths
2. Path.GetFullPath(basePath)                    // Resolve base to absolute
3. Path.GetFullPath(combinedPath)                // Resolve combined to absolute
4. Path.GetRelativePath(absoluteBase, absoluteCombined)  // Get relative path
5. Check if result is:
   - Exactly ".."
   - Starts with "../" or "..\"
   - IsPathRooted (absolute)
```

### Attack Pattern Coverage

| Pattern | Example | Combined | FullPath | GetRelativePath | Detected? |
|---------|---------|----------|----------|-----------------|-----------|
| Parent traversal | `../etc` | `/home/user/../etc` | `/home/etc` | `../../etc` | ✓ Starts with `..` |
| Multi-level | `../../etc` | `/home/user/proj/../../etc` | `/home/etc` | `../../../etc` | ✓ Starts with `..` |
| Absolute Unix | `/etc/passwd` | `/etc/passwd` | `/etc/passwd` | `/etc/passwd` (rooted) | ✓ IsPathRooted |
| Absolute Windows | `C:\Windows` | `C:\Windows` | `C:\Windows` | `C:\Windows` (rooted) | ✓ IsPathRooted |
| Embedded traversal | `sub/../../etc` | `/home/user/proj/sub/../../etc` | `/home/etc` | `../../etc` | ✓ Starts with `..` |
| Legitimate ..prefix | `..data` | `/home/user/proj/..data` | `/home/user/proj/..data` | `..data` | ✗ Valid |
| Empty | `` | `/home/user/proj` | `/home/user/proj` | `.` | ✗ Valid |
| Dot | `.` | `/home/user/proj/.` | `/home/user/proj` | `.` | ✗ Valid |
| Double-dot | `..` | `/home/user/proj/..` | `/home/user` | `..` | ✓ Equals `..` |

### Edge Cases to Verify

1. **Windows alternative separator**: Does `..` + `Path.AltDirectorySeparatorChar` catch `../` on Windows?
2. **Cross-drive traversal**: Does Windows `C:\temp` + `D:\etc` get caught?
3. **Case sensitivity**: Does Unix handle `/Home/user` vs `/home/user`?
4. **Symlinks**: Are symlinks resolved by GetFullPath?
5. **UNC paths**: `\\server\share\..` patterns
