package NuGetCacheTool {
    part def CliSubsystem {
        doc /* Command-line argument parsing and output-management subsystem. */

        comment testRef /* Test: test/DemaConsulting.NuGet.CacheTool.Tests/Cli/CliTests.cs */
        comment designRef /* Design: docs/design/nuget-cache-tool/cli.md */
        comment verificationRef /* Verification: docs/verification/nuget-cache-tool/cli.md */
        comment reqRef /* Requirements: docs/reqstream/nuget-cache-tool/cli.yaml */

        part context : Context;
    }
}
