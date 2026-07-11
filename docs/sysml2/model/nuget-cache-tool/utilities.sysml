package NuGetCacheTool {
    part def UtilitiesSubsystem {
        doc /* Shared utility helpers: safe path combination and temporary directory lifecycle. */

        comment testRef /* Test: test/DemaConsulting.NuGet.CacheTool.Tests/Utilities/UtilitiesTests.cs */
        comment designRef /* Design: docs/design/nuget-cache-tool/utilities.md */
        comment verificationRef /* Verification: docs/verification/nuget-cache-tool/utilities.md */
        comment reqRef /* Requirements: docs/reqstream/nuget-cache-tool/utilities.yaml */

        part pathHelpers : PathHelpers;
        part temporaryDirectory : TemporaryDirectory;
    }
}
