package NuGetCacheTool {
    private import SoftwareItemMetadata::*;
    private import OtsDependencies::*;

    part def HelpCommand {
        doc /* Built-in help command: answers the help verb from the help cards embedded in the assembly. */

        metadata info : SoftwareInfo {
            sourceFiles = ("src/DemaConsulting.NuGet.CacheTool/Cli/HelpCommand.cs");
            testFiles = ("test/DemaConsulting.NuGet.CacheTool.Tests/Cli/HelpCommandTests.cs");
            designFiles = ("docs/design/nuget-cache-tool/cli/help-command.md");
            verificationFiles = ("docs/verification/nuget-cache-tool/cli/help-command.md");
            requirementFiles = ("docs/reqstream/nuget-cache-tool/cli/help-command.yaml");
        }
    }

    // Serves the help command from the cards embedded in the assembly
    dependency HelpCommand to HelpCards;
}
