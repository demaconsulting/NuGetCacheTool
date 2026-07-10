# NuGet Cache Tool Design Introduction

## Purpose

This document provides the design overview for the NuGet Cache Tool, a .NET global
tool that ensures NuGet packages are cached in the global packages folder. It serves
as the entry point for design documentation, guiding reviewers and maintainers through
the software structure and folder layout.

## Scope

This design documentation covers the following software items:

Local items:

- **NuGetCacheTool**: system, subsystem, and unit design for all local components.

OTS items:

- **DemaConsulting.NuGet.Caching**: integration and usage design.
- **DemaConsulting.TestResults**: integration and usage design.
- **SysML2Tools**: integration and usage design.

`DemaConsulting.NuGet.Caching` and `DemaConsulting.TestResults` are runtime dependencies that
this project's own source code integrates with directly, so each has an integration/usage design
document describing that source-level integration. `SysML2Tools` is build-time tooling — it is
not called by this project's source code at runtime — but it still has its own design document
here because it generates the SysML2 software-structure model and diagrams referenced below, so
its usage is design-relevant even though it is not a source dependency. The SysML2
software-structure model itself represents runtime software composition only; build-time-only
tooling (including SysML2Tools, along with BuildMark, FileAssert, Pandoc, ReqStream, ReviewMark,
SarifMark, SonarMark, VersionMark, WeasyPrint, and xUnit) is deliberately excluded from that
structural model — see the clarifying comment in `docs/sysml2/model/ots.sysml` for the rationale.
Requirements and verification evidence for the full set of build-pipeline OTS tooling is tracked
separately; see *OTS Verification* in the verification documentation.

It applies to the current release and all subsequent releases until superseded.

The following items are explicitly excluded from this design documentation:
`DemaConsulting.NuGet.CacheTool.Tests` and CI/CD pipeline configuration.

## Software Structure

The software structure is modeled in SysML2 under `docs/sysml2/` and rendered to the
diagram below by SysML2Tools as part of the build pipeline. AI agents should query the
SysML2 model directly (see the `sysml2tools-query` skill) rather than parsing this
diagram or the prose below.

![Software Structure](SoftwareStructureView.svg)

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

- Requirements: `docs/reqstream/ots/nuget-caching.yaml`, `docs/reqstream/ots/test-results.yaml`,
  `docs/reqstream/ots/sysml2tools.yaml`
- Design: `docs/design/ots/nuget-caching.md`, `docs/design/ots/test-results.md`,
  `docs/design/ots/sysml2tools.md`
- Verification: `docs/verification/ots/nuget-caching.md`, `docs/verification/ots/test-results.md`,
  `docs/verification/ots/sysml2tools.md`

Review-sets: defined in `.reviewmark.yaml`

## References

- [REF-1] NuGet Cache Tool Releases, available at the demaconsulting/NuGetCacheTool GitHub repository releases page.
