package NuGetCacheTool {
    private import SoftwareItemMetadata::*;
    import OtsDependencies::*;

    doc
    /*
     * NuGetCacheTool is a .NET global tool that ensures NuGet packages are cached
     * in the global packages folder.
     */

    part def NuGetCacheToolSystem {
        doc /* The NuGetCacheTool system as a whole. */

        metadata info : SoftwareInfo {
            description = "The NuGetCacheTool system as a whole.";
            testFiles = ("test/DemaConsulting.NuGet.CacheTool.Tests/IntegrationTests.cs");
            designFiles = ("docs/design/nuget-cache-tool.md");
            verificationFiles = ("docs/verification/nuget-cache-tool.md");
            requirementFiles = ("docs/reqstream/nuget-cache-tool.yaml");
        }

        part program : Program;
        part cli : CliSubsystem;
        part selfTest : SelfTestSubsystem;
        part utilities : UtilitiesSubsystem;

        part nuGetCaching : NuGetCaching;
        part testResults : TestResults;
    }
}
