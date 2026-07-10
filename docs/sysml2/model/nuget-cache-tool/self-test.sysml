package NuGetCacheTool {
    part def SelfTestSubsystem {
        doc /* Self-validation test execution subsystem, exercised via --validate. */

        comment testRef /* Test: test/DemaConsulting.NuGet.CacheTool.Tests/SelfTest/SelfTestTests.cs */
        comment designRef /* Design: docs/design/nuget-cache-tool/self-test.md */
        comment verificationRef /* Verification: docs/verification/nuget-cache-tool/self-test.md */
        comment reqRef /* Requirements: docs/reqstream/nuget-cache-tool/self-test.yaml */

        part validation : Validation;
    }
}
