package NuGetCacheTool {
    private import SoftwareItemMetadata::*;

    part def PathHelpers {
        doc /* Safe path-combination utility that prevents path-traversal attacks. */

        metadata info : SoftwareInfo {
            description = "Safe path-combination utility that prevents path-traversal attacks.";
            sourceFiles = ("src/DemaConsulting.NuGet.CacheTool/Utilities/PathHelpers.cs");
            testFiles = ("test/DemaConsulting.NuGet.CacheTool.Tests/Utilities/PathHelpersTests.cs");
            designFiles = ("docs/design/nuget-cache-tool/utilities/path-helpers.md");
            verificationFiles = ("docs/verification/nuget-cache-tool/utilities/path-helpers.md");
            requirementFiles = ("docs/reqstream/nuget-cache-tool/utilities/path-helpers.yaml");
        }
    }
}
