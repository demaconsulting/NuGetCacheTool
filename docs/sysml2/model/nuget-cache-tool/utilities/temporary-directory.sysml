package NuGetCacheTool {
    private import SoftwareItemMetadata::*;

    part def TemporaryDirectory {
        doc /* Disposable temporary directory utility used by self-test and test infrastructure. */

        metadata info : SoftwareInfo {
            description = "Disposable temporary directory utility used by self-test and test infrastructure.";
            sourceFiles = ("src/DemaConsulting.NuGet.CacheTool/Utilities/TemporaryDirectory.cs");
            testFiles = ("test/DemaConsulting.NuGet.CacheTool.Tests/Utilities/TemporaryDirectoryTests.cs");
            designFiles = ("docs/design/nuget-cache-tool/utilities/temporary-directory.md");
            verificationFiles = ("docs/verification/nuget-cache-tool/utilities/temporary-directory.md");
            requirementFiles = ("docs/reqstream/nuget-cache-tool/utilities/temporary-directory.yaml");
        }
    }
}
