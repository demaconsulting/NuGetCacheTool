# Program Unit Design

## Purpose

`Program` is the main entry point and orchestration unit. It creates the `Context`,
dispatches execution to the appropriate handler, and returns the exit code to the
operating system.

## Version Property

`Program.Version` reads the `AssemblyInformationalVersionAttribute` from the executing
assembly. This value is set at build time by the CI/CD pipeline and reflects the
semantic version of the tool.

## Control Flow in Run()

`Program.Run(Context)` applies a strict priority ordering:

1. **Version display** (`context.Version == true`): prints version string, returns immediately
2. **Help display** (`context.Help == true`): prints banner and usage, returns immediately
3. **Self-validation** (`context.Validate == true`): prints banner, calls `Validation.Run(context)`
4. **Tool logic** (default): prints banner, calls `RunToolLogic(context)`

This ordering ensures that `--version` and `--help` always produce clean output
regardless of other flags.

## Exception Handling Strategy

`Program.Main` wraps `Program.Run` in a try/catch for `ArgumentException` and
`InvalidOperationException`. In these catch blocks, `Main` writes the exception
message directly to `Console.Error` and returns an exit code of 1; `context` is
not available in this scope, so `context.WriteError` is not used.

## RunToolLogic

`RunToolLogic(Context)` iterates `context.Packages` and calls
`NuGetCache.EnsureCachedAsync(packageId, version)` for each entry. The result
(cached package path) is written via `context.WriteLine`.

## Interactions

| Dependency | Usage |
| ---------- | ----- |
| `Context` | Created in `Main`; passed to `Run` and all helpers |
| `Validation` | Called when `context.Validate` is true |
| `NuGetCache` | Called for each package in `context.Packages` |
