# NuGet Cache Tool System Design

## Architecture

The NuGet Cache Tool is a .NET global tool organized into two subsystems and one
top-level unit, all residing in a single assembly.

### Major Components

| Subsystem / Unit | Responsibility |
| ---------------- | -------------- |
| **CLI** (subsystem) | Argument parsing and output management |
| └─ Context | Command-line argument parsing and output management |
| **SelfTest** (subsystem) | Self-validation test execution and utilities |
| └─ Validation | Self-validation test execution |
| └─ PathHelpers | Safe path combination utilities |
| **Program** (top-level unit) | Main entry point and application orchestration |

## External Interfaces

| Interface | Description |
| --------- | ----------- |
| `NuGetCache.EnsureCachedAsync(packageId, version)` | Caches a NuGet package in the global packages folder |
| `DemaConsulting.TestResults` | Provides `TrxSerializer` and `JUnitSerializer` for writing test results |

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

- Two subsystems: `CLI` (argument parsing and output) and `SelfTest` (self-validation)
- `Program` is the top-level unit (entry point and orchestration), not in a subsystem
- Single assembly, with subsystem namespaces: `DemaConsulting.NuGet.CacheTool.Cli` and `DemaConsulting.NuGet.CacheTool.SelfTest`
- Console output is normally managed through `Context.WriteLine` and `Context.WriteError`;
  `Program.Main` may write directly to `Console.Error` if `Context` creation fails or
  has not yet completed
- Exit codes are normally controlled via `Context.ExitCode`; `Program.Main` may return
  a non-zero exit code directly when `Context` cannot be created

## Integration Patterns

The tool integrates with the NuGet ecosystem via `DemaConsulting.NuGet.Caching` and
with CI/CD test infrastructure via `DemaConsulting.TestResults`. Self-validation
tests use the tool's own binary to verify end-to-end behavior in the deployment
environment.
