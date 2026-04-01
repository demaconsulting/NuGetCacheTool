# NuGet Cache Tool System Design

## Architecture

The NuGet Cache Tool is a flat-structured .NET global tool with four top-level units
and no subsystems. All units reside in a single assembly and collaborate directly.

### Major Components

| Unit | Responsibility |
| ---- | -------------- |
| Context | Command-line argument parsing and output management |
| Program | Main entry point and application orchestration |
| Validation | Self-validation test execution |
| PathHelpers | Safe path combination utilities |

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

- No subsystems: all units are top-level within the system
- Flat assembly structure: single project, single namespace
- Console output is managed exclusively through `Context.WriteLine` and `Context.WriteError`
- Exit codes are set only through `Context.ExitCode` and `Context.WriteError`

## Integration Patterns

The tool integrates with the NuGet ecosystem via `DemaConsulting.NuGet.Caching` and
with CI/CD test infrastructure via `DemaConsulting.TestResults`. Self-validation
tests use the tool's own binary to verify end-to-end behavior in the deployment
environment.
