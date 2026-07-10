# OTS Verification

This section documents the verification evidence for Off-The-Shelf (OTS) software items used by
the NuGet Cache Tool. Each OTS item has a dedicated verification document in `docs/verification/ots/`
that identifies the required functionality, verification approach, and references to published
compliance evidence.

## Verification Strategy

OTS software items used by the NuGet Cache Tool are verified through a combination of
vendor-supplied self-validation evidence and local integration tests. Where an OTS item provides
a built-in `--validate` command, the CI pipeline runs that command and records results in a TRX
file consumed by ReqStream. Where self-validation is not available, the OTS item is exercised
through its integration with the build pipeline; downstream FileAssert assertions and a passing
ReqStream enforcement step constitute evidence that the item is functioning correctly.

## Qualification Evidence

Qualification evidence for each OTS item is collected in two forms:

1. **Self-validation TRX files** — tools that support `--validate --results <file>` produce TRX
   results ingested by ReqStream and linked to the corresponding OTS requirement.
2. **Pipeline integration evidence** — tools that lack a self-validation mode are exercised by the
   full CI pipeline; a passing build with FileAssert assertions and ReqStream enforcement
   constitutes transitive evidence that the tool performed its required function.

Per-item evidence details are in the individual OTS verification documents in
`docs/verification/ots/`.

## Regression Approach

When an OTS item is upgraded to a new version:

1. Re-run the full CI pipeline and confirm all self-validation TRX files continue to pass.
2. Confirm all FileAssert assertions and ReqStream enforcement still pass.
3. Review the OTS item's release notes for breaking changes that could affect integration points
   documented in `docs/design/ots/`.
4. If any integration point changes, update the corresponding OTS verification document and
   re-run the pipeline to collect fresh evidence.

## OTS Items

| OTS Item | Verification Document |
| -------- | --------------------- |
| BuildMark | ots/buildmark.md |
| FileAssert | ots/fileassert.md |
| DemaConsulting.NuGet.Caching | ots/nuget-caching.md |
| Pandoc | ots/pandoc.md |
| ReqStream | ots/reqstream.md |
| ReviewMark | ots/reviewmark.md |
| SarifMark | ots/sarifmark.md |
| SonarMark | ots/sonarmark.md |
| SysML2Tools | ots/sysml2tools.md |
| DemaConsulting.TestResults | ots/test-results.md |
| VersionMark | ots/versionmark.md |
| WeasyPrint | ots/weasyprint.md |
| xUnit | ots/xunit.md |
