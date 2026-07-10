## BuildMark Verification

This document provides the verification evidence for the `BuildMark` OTS software item.

### Required Functionality

DemaConsulting.BuildMark queries the GitHub API to capture workflow run details and renders them as
a markdown build-notes document included in the release artifacts. It runs as part of the same CI
pipeline that produces the TRX test results, so a successful pipeline run is evidence that BuildMark
executed without error.

### Verification Approach

BuildMark is verified by two complementary layers of evidence. First, the CI pipeline runs
`buildmark --validate --results artifacts/buildmark-self-validation.trx`, which exercises
BuildMark's built-in self-validation suite against mock data and records results for ReqStream.

Second, the pipeline invokes BuildMark with live GitHub Actions metadata to generate
`docs/build_notes/generated/build_notes.md`. Pandoc then converts this file to HTML; if the
file were absent or malformed, Pandoc would fail. WeasyPrint renders the HTML to a PDF/A
artifact, and FileAssert asserts the PDF exists, has content, and contains expected text
(`WeasyPrint_BuildNotesPdf`). A CI build failure at any step in this chain is evidence that
BuildMark did not produce the required output.

### Test Scenarios

#### BuildMark_MarkdownReportGeneration

**Scenario**: A CI pipeline run triggers BuildMark with live GitHub Actions metadata.

**Expected**: BuildMark exits without error and produces a non-empty markdown build-notes document
in the release artifacts.

**Requirement coverage**: `NuGetCache-OTS-BuildMark`.
