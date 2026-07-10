## Program Unit Design

![NuGetCacheTool Structure](NuGetCacheToolView.svg)

### Purpose

`Program` is the main entry point and orchestration unit. It creates the `Context`,
dispatches execution to the appropriate handler, and returns the exit code to the
operating system.

### Data Model

`Program` is a static class with no instance state. Its only data member is:

| Member | Type | Description |
| ------ | ---- | ----------- |
| `Version` (static property) | `string` | Informational version string read from `AssemblyInformationalVersionAttribute` at runtime |

### Key Methods

#### Version (static property)

Returns the informational version string read from `AssemblyInformationalVersionAttribute`
at runtime. This value is set at build time by the CI/CD pipeline and reflects the
semantic version of the tool.

- **Preconditions**: executing assembly contains `AssemblyInformationalVersionAttribute` (set by CI build)
- **Postconditions**: returns a non-null, non-empty string
- **Algorithm**: reads `AssemblyInformationalVersionAttribute` from the executing assembly;
  falls back to `AssemblyVersion`, then to `"0.0.0"` if neither is available

#### Main(string[] args)

Application entry point. Creates `Context`, delegates to `Run`, returns exit code.

- **Preconditions**: invoked by .NET runtime with command-line arguments
- **Postconditions**: returns 0 on success, non-zero on failure
- **Algorithm**: wraps `Context.Create(args)` and `Run(context)` in try/catch;
  catches `ArgumentException` and `InvalidOperationException`, writes `"Error: {message}"`
  to `Console.Error`, returns 1; catches any other `Exception`, writes
  `"Unexpected error: {message}"` to `Console.Error`, re-throws

#### Run(Context context)

Dispatches execution to the appropriate handler based on context flags.

- **Preconditions**: `context` is a valid, non-disposed `Context`
- **Postconditions**: appropriate action is taken; `context.ExitCode` reflects success/failure
- **Algorithm**: applies four-level priority ordering — version → banner+help → banner+validate → banner+tool logic;
  `--version` produces clean output without a banner; all other paths print the banner first

#### PrintBanner(Context context)

Writes the tool name, version, and copyright notice to output.

- **Preconditions**: `context` is valid
- **Postconditions**: banner lines written via `context.WriteLine`

#### PrintHelp(Context context)

Writes usage information and the options list to output.

- **Preconditions**: `context` is valid
- **Postconditions**: usage text written via `context.WriteLine`

#### RunToolLogic(Context context)

Iterates `context.Packages` and caches each package via `NuGetCache.EnsureCachedAsync`.

- **Preconditions**: `context` is valid; package strings are in `[package]:[version]` form
- **Postconditions**: cached paths written via `context.WriteLine`; caching errors written via
  `context.WriteError`; invalid-format packages written as errors and skipped

### Error Handling

`Program.Main` wraps `Program.Run` in a try/catch for `ArgumentException` and
`InvalidOperationException`. In these catch blocks, `Main` writes `"Error: {message}"`
(with the "Error: " prefix) directly to `Console.Error` and returns an exit code of 1;
`context` is not available in this scope, so `context.WriteError` is not used.

A third handler catches any other `Exception`. It writes `"Unexpected error: {message}"` to
`Console.Error` and re-throws the exception. Re-throwing allows the runtime and operating system
to record the unhandled exception in event logs and generate a crash dump, providing diagnostics
for unexpected failures without suppressing the error.

### Dependencies

| Dependency | Usage |
| ---------- | ----- |
| `Context` | Created in `Main`; passed to `Run` and all helpers |
| `Validation` | Called when `context.Validate` is true |
| `NuGetCache` | Called for each package in `context.Packages` |

### Callers

`Program.Main` is the application entry point, invoked directly by the .NET runtime. There
are no other callers of `Main` in production code.

`Program.Run(Context)` is called from `Main` in normal operation and also called
in-process by `Validation.RunValidationTest` during self-validation, allowing the full
execution path to be exercised without spawning a child process.
