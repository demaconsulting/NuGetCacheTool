package OtsDependencies {
    doc
    /*
     * Off-the-shelf (OTS) dependencies used by NuGetCacheTool.
     *
     * This package models only OTS items that are runtime dependencies of the shipped
     * NuGetCacheTool binary. Build-time/pipeline-only OTS tooling (e.g. BuildMark, FileAssert,
     * Pandoc, ReqStream, ReviewMark, SarifMark, SonarMark, SysML2Tools, VersionMark, WeasyPrint,
     * xUnit) is intentionally NOT modeled here as a structural part, because this model
     * represents the shipped system's runtime composition, not its build tooling. Those items
     * remain fully documented via their own docs/reqstream, docs/design, and
     * docs/verification OTS artifacts and OTS-{Name} review-sets in .reviewmark.yaml.
     */

    part def NuGetCaching {
        doc /* OTS: DemaConsulting.NuGet.Caching. */

        comment designRef /* Design: docs/design/ots/nuget-caching.md */
        comment verificationRef /* Verification: docs/verification/ots/nuget-caching.md */
        comment reqRef /* Requirements: docs/reqstream/ots/nuget-caching.yaml */
    }

    part def TestResults {
        doc /* OTS: DemaConsulting.TestResults. */

        comment designRef /* Design: docs/design/ots/test-results.md */
        comment verificationRef /* Verification: docs/verification/ots/test-results.md */
        comment reqRef /* Requirements: docs/reqstream/ots/test-results.yaml */
    }
}
