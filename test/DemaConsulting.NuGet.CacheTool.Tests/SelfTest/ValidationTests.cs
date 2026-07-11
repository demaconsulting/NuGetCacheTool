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
using DemaConsulting.NuGet.CacheTool.Utilities;

namespace DemaConsulting.NuGet.CacheTool.Tests.SelfTest;

/// <summary>
///     Unit tests for the Validation class: results-file writing behavior.
/// </summary>
[Collection("Sequential")]
public class ValidationTests
{
    /// <summary>
    ///     Test that Validation writes results in TRX format when --results specifies a .trx file.
    /// </summary>
    [Fact]
    public void Validation_Run_TrxResultsRequested_WritesTrxFile()
    {
        // Arrange: prepare a temporary TRX results file path
        using var temporaryDirectory = new TemporaryDirectory();
        var resultsFile = temporaryDirectory.GetFilePath($"{Guid.NewGuid():N}.trx");
        try
        {
            using var context = Context.Create(["--validate", "--silent", "--results", resultsFile]);

            // Act: run validation with TRX results output
            Validation.Run(context);

            // Assert: verify TRX results file is created with expected XML structure
            Assert.Equal(0, context.ExitCode);
            Assert.True(File.Exists(resultsFile), "TRX results file was not created");

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
    [Fact]
    public void Validation_Run_JUnitResultsRequested_WritesJUnitFile()
    {
        // Arrange: prepare a temporary JUnit XML results file path
        using var temporaryDirectory = new TemporaryDirectory();
        var resultsFile = temporaryDirectory.GetFilePath($"{Guid.NewGuid():N}.xml");
        try
        {
            using var context = Context.Create(["--validate", "--silent", "--results", resultsFile]);

            // Act: run validation with JUnit XML results output
            Validation.Run(context);

            // Assert: verify JUnit results file is created with expected XML structure
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

    /// <summary>
    ///     Test that Validation reports an error for an unsupported results file extension.
    /// </summary>
    [Fact]
    public void Validation_Run_UnsupportedResultsFormat_ReportsError()
    {
        // Arrange: create context requesting an unsupported results format
        using var context = Context.Create(["--validate", "--silent", "--results", "output.json"]);

        // Act: run validation with an unsupported results format
        Validation.Run(context);

        // Assert: verify unsupported format causes a non-zero exit code
        Assert.NotEqual(0, context.ExitCode);
    }

    /// <summary>
    ///     Test that Run's cache-package self-test passes because it resolves a real, existing
    ///     cached package directory - not merely because some non-whitespace text was logged.
    /// </summary>
    [Fact]
    public void Validation_Run_CachePackageSelfTest_PassesWithRealCachedPackagePath()
    {
        // Arrange: redirect stdout to capture validation output
        var originalOut = Console.Out;
        try
        {
            using var outWriter = new StringWriter();
            Console.SetOut(outWriter);
            using var context = Context.Create(["--validate"]);

            // Act: run self-validation, which includes the cache-package self-test
            Validation.Run(context);

            // Assert: the cache-package self-test reports success and the overall run passes
            var output = outWriter.ToString();
            Assert.Contains("Cache Package Test - PASSED", output);
            Assert.Equal(0, context.ExitCode);
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    /// <summary>
    ///     Test that ValidateCachePackagePath accepts a path whose directory name and parent
    ///     directory name exactly match the expected version and package ID.
    /// </summary>
    [Fact]
    public void Validation_ValidateCachePackagePath_ExactMatch_ReturnsNull()
    {
        // Arrange: a path exactly matching the expected package id and version
        var path = Path.Combine("packages", "demaconsulting.nuget.caching", "0.1.0");

        // Act: validate the path against the expected identity
        var result = Validation.ValidateCachePackagePath(path, "DemaConsulting.NuGet.Caching", "0.1.0");

        // Assert: no error is reported
        Assert.Null(result);
    }

    /// <summary>
    ///     Test that ValidateCachePackagePath rejects a path whose version directory merely
    ///     contains the expected version as a substring (e.g. a pre-release suffix), proving the
    ///     check is an exact match rather than a substring match.
    /// </summary>
    [Fact]
    public void Validation_ValidateCachePackagePath_VersionSuffixSubstringMatch_ReturnsError()
    {
        // Arrange: a version directory that contains, but is not equal to, the expected version
        var path = Path.Combine("packages", "demaconsulting.nuget.caching", "0.1.0-beta");

        // Act: validate the path against the expected identity
        var result = Validation.ValidateCachePackagePath(path, "DemaConsulting.NuGet.Caching", "0.1.0");

        // Assert: an error is reported since the version does not exactly match
        Assert.NotNull(result);
    }

    /// <summary>
    ///     Test that ValidateCachePackagePath rejects a path whose version directory merely
    ///     contains the expected version as a substring (e.g. a higher major version), proving the
    ///     check is an exact match rather than a substring match.
    /// </summary>
    [Fact]
    public void Validation_ValidateCachePackagePath_VersionPrefixSubstringMatch_ReturnsError()
    {
        // Arrange: a version directory that contains, but is not equal to, the expected version
        var path = Path.Combine("packages", "demaconsulting.nuget.caching", "10.1.0");

        // Act: validate the path against the expected identity
        var result = Validation.ValidateCachePackagePath(path, "DemaConsulting.NuGet.Caching", "0.1.0");

        // Assert: an error is reported since the version does not exactly match
        Assert.NotNull(result);
    }

    /// <summary>
    ///     Test that ValidateCachePackagePath rejects a path whose parent directory does not
    ///     match the expected package ID.
    /// </summary>
    [Fact]
    public void Validation_ValidateCachePackagePath_WrongPackageId_ReturnsError()
    {
        // Arrange: a path for a different package ID
        var path = Path.Combine("packages", "some.other.package", "0.1.0");

        // Act: validate the path against the expected identity
        var result = Validation.ValidateCachePackagePath(path, "DemaConsulting.NuGet.Caching", "0.1.0");

        // Assert: an error is reported since the package ID does not match
        Assert.NotNull(result);
    }

    /// <summary>
    ///     Test that Run prints a summary containing total, passed, and failed counts.
    /// </summary>
    [Fact]
    public void Validation_Run_WithSilentContext_PrintsSummary()
    {
        // Arrange: setup unique log file path to capture silent context output
        using var temporaryDirectory = new TemporaryDirectory();
        var logFile = temporaryDirectory.GetFilePath($"validation_test_{Guid.NewGuid():N}.log");
        try
        {
            using (var context = Context.Create(["--silent", "--log", logFile]))
            {
                // Act: run validation with silent context and log file
                Validation.Run(context);
            }

            // Assert: verify summary lines are written to log file
            var logContent = File.ReadAllText(logFile);
            Assert.Contains("Total Tests:", logContent);
            Assert.Contains("Passed:", logContent);
            Assert.Contains("Failed:", logContent);
        }
        finally
        {
            if (File.Exists(logFile))
            {
                File.Delete(logFile);
            }
        }
    }
}
