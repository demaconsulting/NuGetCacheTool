package NuGetCacheTool {
    private import SoftwareItemMetadata::*;

    part def UtilitiesSubsystem {
        doc /* Shared utility helpers: safe path combination and temporary directory lifecycle. */

        metadata info : SoftwareInfo {
            description = "Shared utility helpers: safe path combination and temporary directory lifecycle.";
            testFiles = ("test/DemaConsulting.NuGet.CacheTool.Tests/Utilities/UtilitiesTests.cs");
            designFiles = ("docs/design/nuget-cache-tool/utilities.md");
            verificationFiles = ("docs/verification/nuget-cache-tool/utilities.md");
            requirementFiles = ("docs/reqstream/nuget-cache-tool/utilities.yaml");
        }

        part pathHelpers : PathHelpers;
        part temporaryDirectory : TemporaryDirectory;
    }
}
