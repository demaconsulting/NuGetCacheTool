# NuGet Cache Tool System Design

## Architecture

The NuGet Cache Tool is a .NET global tool organized into two subsystems and one
top-level unit, all residing in a single assembly.

### Major Components

| Subsystem / Unit | Responsibility |
| ---------------- | -------------- |
| **CLI** (subsystem) | Argument parsing and output management |
| └─ Context | Command-line argument parsing and output management |
| **SelfTest** (subsystem) | Self-validation test execution |
| └─ Validation | Self-validation test execution |
| **Utilities** (subsystem) | Shared utilities: temporary directory and path safety |
| └─ TemporaryDirectory | Disposable temporary directory for self-test and test use |
| └─ PathHelpers | Safe path combination utilities |
| **Program** (top-level unit) | Main entry point and application orchestration |

## External Interfaces

| Interface | Description |
| --------- | ----------- |
| CLI entry point (`nuget-cache [options] [package:version ...]`) | Main command-line invocation pattern accepted by the tool |
| stdout | Package paths are written one per line on success; banner and help text on non-version invocations |
| stderr (`Error: {message}`) | Error messages are written to stderr when an error occurs and silent mode is not active |
| Exit code | 0 = success; non-zero = failure |

## Dependencies

- **DemaConsulting.NuGet.Caching**: provides the `NuGetCache.EnsureCachedAsync` API used to
  cache packages in the global NuGet packages folder — see
  *DemaConsulting.NuGet.Caching Integration Design*
- **DemaConsulting.TestResults**: provides `TrxSerializer` and `JUnitSerializer` used by the
  SelfTest subsystem to emit validation results in TRX and JUnit XML formats — see
  *DemaConsulting.TestResults Integration Design*

## Risk Control Measures

The NuGet Cache Tool has one security-relevant requirement: `NuGetCache-Sys-PathSafety`
requires that user-supplied path components are validated before being combined with trusted
base paths to prevent path-traversal attacks (e.g., `../../etc/passwd`).

This risk is mitigated by the `PathHelpers.SafePathCombine` utility in the Utilities subsystem,
which rejects absolute paths and path components containing `..` traversal sequences. All
callers that combine user-supplied relative paths with trusted base paths must use
`SafePathCombine` rather than `Path.Combine` directly.

No patient-safety or functional-safety requirements apply. No software item segregation
is required beyond the security constraint described above.

## Data Flow

```text
args
 └─► Context.Create(args)
      ├── Version=true  ──► Program.Run ──► display version, exit 0
      ├── Help=true     ──► Program.Run ──► display banner + help, exit 0
      ├── Validate=true ──► Program.Run ──► display banner + Validation.Run(), exit code
      └── (default)     ──► Program.Run ──► display banner + RunToolLogic(), exit code
                                                   └─► NuGetCache.EnsureCachedAsync per package
```

## Design Constraints

### Platform Constraints

- The tool targets .NET 8, .NET 9, and .NET 10 on Windows, Linux, and macOS (see
  [Platform and Runtime Targeting](#platform-and-runtime-targeting) below)
- All platform-specific behavior is delegated to the .NET SDK and the
  `DemaConsulting.NuGet.Caching` library; the tool itself contains no platform-conditional code

### Structural Constraints

- Three subsystems: `CLI` (argument parsing and output), `SelfTest` (self-validation), and
  `Utilities` (shared helpers)
- `Program` is the top-level unit (entry point and orchestration), not in a subsystem
- Single assembly, with subsystem namespaces: `DemaConsulting.NuGet.CacheTool.Cli`,
  `DemaConsulting.NuGet.CacheTool.SelfTest`, and `DemaConsulting.NuGet.CacheTool.Utilities`
- Console output is normally managed through `Context.WriteLine` and `Context.WriteError`;
  `Program.Main` may write directly to `Console.Error` if `Context` creation fails or
  has not yet completed
- Exit codes are normally controlled via `Context.ExitCode`; `Program.Main` may return
  a non-zero exit code directly when `Context` cannot be created

### Security Constraints

- All code paths that combine user-supplied path components with trusted base paths MUST
  use `PathHelpers.SafePathCombine` rather than `Path.Combine` directly, to prevent
  path-traversal attacks (see `NuGetCache-Sys-PathSafety` and Risk Control Measures)

## Platform and Runtime Targeting

The NuGet Cache Tool targets multiple platforms and .NET runtime versions to support
the broadest possible developer and CI/CD environment coverage.

| Dimension | Supported Values |
| --------- | ---------------- |
| Operating Systems | Windows, Linux, macOS |
| .NET Runtimes | .NET 8, .NET 9, .NET 10 |

Multi-targeting is achieved through the `TargetFrameworks` MSBuild property in the
project file. All platform-specific behavior is delegated to the .NET SDK and the
`DemaConsulting.NuGet.Caching` library; the tool itself contains no platform-conditional
code.

Platform and runtime compatibility is verified by the CI/CD pipeline, which executes
the integration tests and self-validation tests on each target platform and runtime
combination.

## Self-Validation Tests

When invoked with `--validate`, the tool executes three built-in self-validation
tests that exercise the full application stack within the deployment environment.
These tests use the same `Program.Run` path as normal usage, capturing output via
`--silent --log` and asserting on the log contents.

| Test Name | Command | Validates |
| --------- | ------- | --------- |
| `NuGetCache_VersionDisplay` | `--version` | Version string is present and contains dots |
| `NuGetCache_HelpDisplay` | `--help` | Output contains `Usage:` and `Options:` |
| `NuGetCache_CachePackage` | `DemaConsulting.NuGet.Caching:0.1.0` | A non-empty package path is returned |

When `--results` is supplied, these tests are emitted as test results (TRX (`.trx`)
or JUnit XML (`.xml`), depending on the `--results` file extension) and serve as
system-level evidence that all units work correctly together in the target environment.

## Integration Patterns

The tool integrates with the NuGet ecosystem via `DemaConsulting.NuGet.Caching` and
with CI/CD test infrastructure via `DemaConsulting.TestResults`. Self-validation
tests use the tool's own binary to verify end-to-end behavior in the deployment
environment.
