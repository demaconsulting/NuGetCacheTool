package NuGetCacheTool {
    private import SoftwareItemMetadata::*;

    part def Context {
        doc /* Command-line argument parser and centralized output-management owner. */

        metadata info : SoftwareInfo {
            description = "Command-line argument parser and centralized output-management owner.";
            sourceFiles = ("src/DemaConsulting.NuGet.CacheTool/Cli/Context.cs");
            testFiles = ("test/DemaConsulting.NuGet.CacheTool.Tests/Cli/ContextTests.cs");
            designFiles = ("docs/design/nuget-cache-tool/cli/context.md");
            verificationFiles = ("docs/verification/nuget-cache-tool/cli/context.md");
            requirementFiles = ("docs/reqstream/nuget-cache-tool/cli/context.yaml");
        }
    }
}
