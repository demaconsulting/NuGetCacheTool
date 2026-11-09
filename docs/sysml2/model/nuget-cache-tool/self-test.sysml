package NuGetCacheTool {
    private import SoftwareItemMetadata::*;

    part def SelfTestSubsystem {
        doc /* Self-validation test execution subsystem, exercised via --validate. */

        metadata info : SoftwareInfo {
            description = "Self-validation test execution subsystem, exercised via --validate.";
            testFiles = ("test/DemaConsulting.NuGet.CacheTool.Tests/SelfTest/SelfTestTests.cs");
            designFiles = ("docs/design/nuget-cache-tool/self-test.md");
            verificationFiles = ("docs/verification/nuget-cache-tool/self-test.md");
            requirementFiles = ("docs/reqstream/nuget-cache-tool/self-test.yaml");
        }

        part validation : Validation;
    }
}
