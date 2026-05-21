# NuGet Cache Tool Design Introduction

## Purpose

This document provides the design overview for the NuGet Cache Tool, a .NET global
tool that ensures NuGet packages are cached in the global packages folder. It serves
as the entry point for design documentation, guiding reviewers and maintainers through
the software structure and folder layout.

## Scope

This design documentation covers all software items of the NuGet Cache Tool system.
It applies to the current release and all subsequent releases until superseded.

The following items are explicitly excluded from this design documentation:
`DemaConsulting.NuGet.CacheTool.Tests` and CI/CD pipeline configuration.

## Software Structure

```text
NuGetCacheTool (System)
├── CLI (Subsystem)
│   └── Context (Unit)
├── SelfTest (Subsystem)
│   └── Validation (Unit)
├── Utilities (Subsystem)
│   ├── TemporaryDirectory (Unit)
│   └── PathHelpers (Unit)
└── Program (Unit)

OTS Items
├── DemaConsulting.NuGet.Caching
└── DemaConsulting.TestResults
```

## Folder Layout

```text
src/DemaConsulting.NuGet.CacheTool/
├── Cli/
│   └── Context.cs               — CLI subsystem: argument parsing and output management
├── SelfTest/
│   └── Validation.cs            — SelfTest subsystem: self-validation test execution
├── Utilities/
│   ├── PathHelpers.cs           — Utilities subsystem: safe path combination utilities
│   └── TemporaryDirectory.cs    — Utilities subsystem: disposable temporary directory
└── Program.cs                   — top-level entry point and application orchestration
```

## Companion Artifact Structure

Each local software item has corresponding artifacts in parallel directory trees:

- Requirements: `docs/reqstream/nuget-cache-tool.yaml`, `docs/reqstream/nuget-cache-tool/.../{item}.yaml`
- Design: `docs/design/nuget-cache-tool.md`, `docs/design/nuget-cache-tool/.../{item}.md`
- Verification: `docs/verification/nuget-cache-tool.md`, `docs/verification/nuget-cache-tool/.../{item}.md`
- Source: `src/DemaConsulting.NuGet.CacheTool/.../{Item}.cs`
- Tests: `test/DemaConsulting.NuGet.CacheTool.Tests/.../{Item}Tests.cs`

OTS items have integration/usage design documentation parallel to system folders:

- Requirements: `docs/reqstream/ots/nuget-caching.yaml`, `docs/reqstream/ots/test-results.yaml`
- Design: `docs/design/ots/nuget-caching.md`, `docs/design/ots/test-results.md`
- Verification: `docs/verification/ots/nuget-caching.md`, `docs/verification/ots/test-results.md`

Review-sets: defined in `.reviewmark.yaml`

## References

- [REF-1] NuGet Cache Tool Releases, available at the demaconsulting/NuGetCacheTool GitHub repository releases page.
