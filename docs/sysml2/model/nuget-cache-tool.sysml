package NuGetCacheTool {
    import OtsDependencies::*;

    doc
    /*
     * NuGetCacheTool is a .NET global tool that ensures NuGet packages are cached
     * in the global packages folder.
     */

    part def NuGetCacheToolSystem {
        doc /* The NuGetCacheTool system as a whole. */

        comment testRef /* Test: test/DemaConsulting.NuGet.CacheTool.Tests/IntegrationTests.cs */
        comment designRef /* Design: docs/design/nuget-cache-tool.md */
        comment verificationRef /* Verification: docs/verification/nuget-cache-tool.md */
        comment reqRef /* Requirements: docs/reqstream/nuget-cache-tool.yaml */

        part program : Program;
        part cli : CliSubsystem;
        part selfTest : SelfTestSubsystem;
        part utilities : UtilitiesSubsystem;

        part nuGetCaching : NuGetCaching;
        part testResults : TestResults;
    }
}
