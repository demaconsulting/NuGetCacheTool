// Copyright (c) DEMA Consulting
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using DemaConsulting.NuGet.CacheTool.Cli;
using DemaConsulting.NuGet.CacheTool.SelfTest;

namespace DemaConsulting.NuGet.CacheTool.Tests.SelfTest;

/// <summary>
///     Subsystem integration tests for the SelfTest subsystem (self-validation and path utilities).
/// </summary>
[Collection("Sequential")]
public class SelfTestTests
{
    /// <summary>
    ///     Test that the SelfTest subsystem executes self-validation tests and reports a summary.
    /// </summary>
    [Fact]
    public void SelfTest_Validation_ExecutesSelfValidationTests()
    {
        // Arrange: redirect stdout to capture validation output
        var originalOut = Console.Out;
        try
        {
            using var outWriter = new StringWriter();
            Console.SetOut(outWriter);
            using var context = Context.Create(["--validate"]);

            // Act: run self-validation
            Validation.Run(context);

            // Assert: verify validation summary appears in output
            var output = outWriter.ToString();
            Assert.Contains("Total Tests:", output);
            Assert.Contains("Passed:", output);
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    /// <summary>
    ///     Test that the SelfTest subsystem reports a zero exit code when all validation tests pass.
    /// </summary>
    [Fact]
    public void SelfTest_Validation_ReportsPassFail()
    {
        // Arrange: redirect stdout to suppress validation output
        var originalOut = Console.Out;
        try
        {
            using var outWriter = new StringWriter();
            Console.SetOut(outWriter);
            using var context = Context.Create(["--validate"]);

            // Act: run self-validation
            Validation.Run(context);

            // Assert: all self-validation tests must pass with zero exit code
            Assert.Equal(0, context.ExitCode);
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    /// <summary>
    ///     Test that the SelfTest subsystem writes validation results in TRX format when --results .trx is specified.
    /// </summary>
    [Fact]
    public void SelfTest_ResultsFile_GeneratesTrxFile()
    {
        // Arrange: prepare a temporary TRX results file path
        var resultsFile = Path.Combine(Path.GetTempPath(), Path.ChangeExtension(Path.GetRandomFileName(), ".trx"));
        try
        {
            using var context = Context.Create(["--validate", "--silent", "--results", resultsFile]);

            // Act: run self-validation with TRX results output
            Validation.Run(context);

            // Assert: verify TRX results file is created with expected content
            Assert.Equal(0, context.ExitCode);
            Assert.True(File.Exists(resultsFile), "TRX results file was not created");

            var trxContent = File.ReadAllText(resultsFile);
            Assert.Contains("<TestRun", trxContent);
            Assert.Contains("</TestRun>", trxContent);
        }
        finally
        {
            if (File.Exists(resultsFile))
            {
                File.Delete(resultsFile);
            }
        }
    }

    /// <summary>
    ///     Test that the SelfTest subsystem writes validation results in JUnit XML format when --results .xml is specified.
    /// </summary>
    [Fact]
    public void SelfTest_ResultsFile_GeneratesJUnitFile()
    {
        // Arrange: prepare a temporary JUnit XML results file path
        var resultsFile = Path.Combine(Path.GetTempPath(), Path.ChangeExtension(Path.GetRandomFileName(), ".xml"));
        try
        {
            using var context = Context.Create(["--validate", "--silent", "--results", resultsFile]);

            // Act: run self-validation with JUnit XML results output
            Validation.Run(context);

            // Assert: verify JUnit results file is created with expected content
            Assert.Equal(0, context.ExitCode);
            Assert.True(File.Exists(resultsFile), "JUnit results file was not created");

            var content = File.ReadAllText(resultsFile);
            Assert.Contains("<testsuites", content);
            Assert.Contains("<testsuite", content);
            Assert.Contains("<testcase", content);
        }
        finally
        {
            if (File.Exists(resultsFile))
            {
                File.Delete(resultsFile);
            }
        }
    }

}
