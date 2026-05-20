# OTS Integration Design

## Selection Criteria

OTS items are selected for the NuGet Cache Tool based on the following criteria:

- **Functionality alignment**: the OTS item must provide the exact capability required, avoiding any
  need to re-implement the feature within the NuGet Cache Tool codebase
- **License compatibility**: the OTS item must be published under a license compatible with the MIT
  License used by the NuGet Cache Tool
- **Organizational provenance**: preference is given to OTS items published by DEMA Consulting, which
  provides full visibility into the implementation, release process, and compliance evidence
- **Minimal API surface**: the OTS item should expose a small, well-defined API that is directly
  callable without wrapper or adapter layers

Both `DemaConsulting.NuGet.Caching` and `DemaConsulting.TestResults` satisfy all four criteria:
they are purpose-built libraries published by DEMA Consulting under the MIT License, each exposing
a minimal, stable API that meets the NuGet Cache Tool's exact needs.

## Version Management Policy

OTS package versions are declared as exact NuGet package references in the project file
(`src/DemaConsulting.NuGet.CacheTool/DemaConsulting.NuGet.CacheTool.csproj`). Version information
is managed in SBOMs outside of design documentation. Dependabot monitors published OTS packages and
raises pull requests for version upgrades. Each upgrade pull request is evaluated against the full
CI/CD test suite before merging; major-version upgrades additionally require a design review to
confirm that the API surface used by the NuGet Cache Tool remains compatible.

## General Integration Approach

Both OTS items are consumed as NuGet packages declared in the project file. No wrapper classes or
abstraction interfaces are introduced; OTS APIs are called directly from the units that require
them. This keeps the integration surface minimal and directly traceable from source to design:

- `DemaConsulting.NuGet.Caching` is called directly from `Program.RunToolLogic`
- `DemaConsulting.TestResults` serializers are called directly from `Validation.Run`

Error handling follows the conventions of the calling unit: exceptions thrown by OTS APIs are caught
at the call site and reported via `context.WriteError`, which sets the process exit code to 1.

## Qualification Strategy

OTS items are qualified through integration-test evidence produced by the CI/CD pipeline. The
NuGet Cache Tool's self-validation tests (`--validate`) exercise both OTS items end-to-end in
the deployment environment:

- `DemaConsulting.NuGet.Caching` is exercised by `NuGetCache_CachePackage`, which calls
  `NuGetCache.EnsureCachedAsync` against the live NuGet feed
- `DemaConsulting.TestResults` is exercised by `--results` output from `Validation.Run`, which
  invokes `TrxSerializer` or `JUnitSerializer` and asserts the structure of the produced file

Self-validation results are emitted as machine-readable test result files (TRX or JUnit XML)
consumed by the ReqStream traceability tool, providing traceable OTS qualification evidence.
See each OTS item's verification document for the specific test scenarios and pass/fail criteria.

## Integration Strategy

The NuGet Cache Tool integrates two OTS (off-the-shelf) software items:

| OTS Item | Package | Purpose |
| -------- | ------- | ------- |
| DemaConsulting.NuGet.Caching | `DemaConsulting.NuGet.Caching` | NuGet package caching to the global packages folder |
| DemaConsulting.TestResults | `DemaConsulting.TestResults` | Serialization of self-validation results to TRX and JUnit XML formats |

Both OTS items are consumed as NuGet packages declared in the main project file
(`src/DemaConsulting.NuGet.CacheTool/DemaConsulting.NuGet.CacheTool.csproj`).
No wrapper layers or abstraction interfaces are introduced; the OTS APIs are called
directly from the units that need them.

OTS items are verified by integration-test evidence from the CI/CD pipeline. Each
OTS item's verification document lists the integration test scenarios that demonstrate
the item functions correctly in the deployment environment — see
*DemaConsulting.NuGet.Caching Integration Design* and
*DemaConsulting.TestResults Integration Design*.
