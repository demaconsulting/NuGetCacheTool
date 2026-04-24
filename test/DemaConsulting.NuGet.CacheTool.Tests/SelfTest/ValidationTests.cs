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
///     Unit tests for the Validation class: results-file writing behavior.
/// </summary>
[TestClass]
public class ValidationTests
{
    /// <summary>
    ///     Test that Validation writes results in TRX format when --results specifies a .trx file.
    /// </summary>
    [TestMethod]
    public void Validation_Run_TrxResultsRequested_WritesTrxFile()
    {
        // Arrange: prepare a temporary TRX results file path
        var resultsFile = Path.Combine(Path.GetTempPath(), Path.ChangeExtension(Path.GetRandomFileName(), ".trx"));
        try
        {
            using var context = Context.Create(["--validate", "--silent", "--results", resultsFile]);

            // Act: run validation with TRX results output
            Validation.Run(context);

            // Assert: verify TRX results file is created with expected XML structure
            Assert.AreEqual(0, context.ExitCode);
            Assert.IsTrue(File.Exists(resultsFile), "TRX results file was not created");

            var content = File.ReadAllText(resultsFile);
            Assert.Contains("<TestRun", content);
            Assert.Contains("</TestRun>", content);
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
    ///     Test that Validation writes results in JUnit XML format when --results specifies a .xml file.
    /// </summary>
    [TestMethod]
    public void Validation_Run_JUnitResultsRequested_WritesJUnitFile()
    {
        // Arrange: prepare a temporary JUnit XML results file path
        var resultsFile = Path.Combine(Path.GetTempPath(), Path.ChangeExtension(Path.GetRandomFileName(), ".xml"));
        try
        {
            using var context = Context.Create(["--validate", "--silent", "--results", resultsFile]);

            // Act: run validation with JUnit XML results output
            Validation.Run(context);

            // Assert: verify JUnit results file is created with expected XML structure
            Assert.AreEqual(0, context.ExitCode);
            Assert.IsTrue(File.Exists(resultsFile), "JUnit results file was not created");

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

    /// <summary>
    ///     Test that Validation reports an error for an unsupported results file extension.
    /// </summary>
    [TestMethod]
    public void Validation_Run_UnsupportedResultsFormat_ReportsError()
    {
        // Arrange: create context requesting an unsupported results format
        using var context = Context.Create(["--validate", "--silent", "--results", "output.json"]);

        // Act: run validation with an unsupported results format
        Validation.Run(context);

        // Assert: verify unsupported format causes a non-zero exit code
        Assert.AreNotEqual(0, context.ExitCode);
    }
}
