## DemaConsulting.NuGet.Caching Integration Design

### Why Chosen

`DemaConsulting.NuGet.Caching` is the purpose-built library for caching NuGet packages
to the global packages folder. It provides a single async API — `NuGetCache.EnsureCachedAsync`
— that abstracts all NuGet feed querying, download, and extraction logic. Using this library
avoids re-implementing NuGet client protocol handling within the NuGet Cache Tool itself.

### APIs Used

| API | Signature | Usage |
| --- | --------- | ----- |
| `NuGetCache.EnsureCachedAsync` | `Task<string> EnsureCachedAsync(string packageId, string version)` | Called by `Program.RunToolLogic` for each package argument; returns the path to the cached package in the global packages folder |

### Integration Pattern

`Program.RunToolLogic` calls `NuGetCache.EnsureCachedAsync(packageId, version)` synchronously
using `.GetAwaiter().GetResult()`. This is safe because the NuGet Cache Tool is a console
application with no synchronization context that could cause a deadlock. The returned path
is written to output via `context.WriteLine`. Any `InvalidOperationException` thrown by the
library is caught and reported via `context.WriteError`.

### Version Constraints

The specific version of `DemaConsulting.NuGet.Caching` is declared in the project file.
Compatibility is verified by the integration test suite, which exercises the
`NuGetCache.EnsureCachedAsync` API end-to-end against the live NuGet feed.

See *OTS Integration Design* for the overall OTS integration strategy.
