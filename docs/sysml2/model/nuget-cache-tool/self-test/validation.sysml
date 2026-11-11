package NuGetCacheTool {
    private import SoftwareItemMetadata::*;

    part def Validation {
        doc /* Self-validation test runner exercising the full application stack. */

        metadata info : SoftwareInfo {
            sourceFiles = ("src/DemaConsulting.NuGet.CacheTool/SelfTest/Validation.cs");
            testFiles = ("test/DemaConsulting.NuGet.CacheTool.Tests/SelfTest/ValidationTests.cs");
            designFiles = ("docs/design/nuget-cache-tool/self-test/validation.md");
            verificationFiles = ("docs/verification/nuget-cache-tool/self-test/validation.md");
            requirementFiles = ("docs/reqstream/nuget-cache-tool/self-test/validation.yaml");
        }
    }
}
