package NuGetCacheTool {
    private import SoftwareItemMetadata::*;

    part def CliSubsystem {
        doc /* Command-line argument parsing and output-management subsystem. */

        metadata info : SoftwareInfo {
            testFiles = ("test/DemaConsulting.NuGet.CacheTool.Tests/Cli/CliTests.cs");
            designFiles = ("docs/design/nuget-cache-tool/cli.md");
            verificationFiles = ("docs/verification/nuget-cache-tool/cli.md");
            requirementFiles = ("docs/reqstream/nuget-cache-tool/cli.yaml");
        }

        part context : Context;
    }
}
