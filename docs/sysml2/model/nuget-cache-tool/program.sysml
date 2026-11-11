package NuGetCacheTool {
    private import SoftwareItemMetadata::*;

    part def Program {
        doc /* Entry point and application orchestrator. */

        metadata info : SoftwareInfo {
            sourceFiles = ("src/DemaConsulting.NuGet.CacheTool/Program.cs");
            testFiles = ("test/DemaConsulting.NuGet.CacheTool.Tests/ProgramTests.cs");
            designFiles = ("docs/design/nuget-cache-tool/program.md");
            verificationFiles = ("docs/verification/nuget-cache-tool/program.md");
            requirementFiles = ("docs/reqstream/nuget-cache-tool/program.yaml");
        }
    }
}
