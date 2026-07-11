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

using DemaConsulting.NuGet.CacheTool.Utilities;

namespace DemaConsulting.NuGet.CacheTool.Tests;

/// <summary>
///     Integration tests that run the NuGet Cache Tool application through dotnet.
/// </summary>
[Collection("Sequential")]
public class IntegrationTests
{
    /// <summary>Full path to the NuGet Cache Tool DLL under test.</summary>
    private readonly string _dllPath;

    /// <summary>
    ///     Initialize test by locating the NuGet Cache Tool DLL.
    /// </summary>
    public IntegrationTests()
    {
        // The DLL should be in the same directory as the test assembly
        // because the test project references the main project
        var baseDir = AppContext.BaseDirectory;
        _dllPath = PathHelpers.SafePathCombine(baseDir, "DemaConsulting.NuGet.CacheTool.dll");

        Assert.True(File.Exists(_dllPath), $"Could not find NuGet Cache Tool DLL at {_dllPath}");
    }

    /// <summary>
    ///     Test that version flag outputs version information.
    /// </summary>
    [Fact]
    public void NuGetCacheTool_VersionDisplay_VersionFlagProvided_OutputsVersion()
    {
        // Arrange: no setup required
        // Act
        var exitCode = Runner.Run(
            out var output,
            "dotnet",
            _dllPath,
            "--version");

        // Assert
        Assert.Equal(0, exitCode);
        Assert.False(string.IsNullOrWhiteSpace(output));
        Assert.DoesNotContain("Error", output);
        Assert.DoesNotContain("Copyright", output);
    }

    /// <summary>
    ///     Test that help flag outputs usage information.
    /// </summary>
    [Fact]
    public void NuGetCacheTool_HelpDisplay_HelpFlagProvided_OutputsUsageInformation()
    {
        // Arrange: no setup required
        // Act
        var exitCode = Runner.Run(
            out var output,
            "dotnet",
            _dllPath,
            "--help");

        // Assert
        Assert.Equal(0, exitCode);
        Assert.Contains("Usage:", output);
        Assert.Contains("Options:", output);
        Assert.Contains("--version", output);
    }

    /// <summary>
    ///     Test that validate flag runs self-validation.
    /// </summary>
    [Fact]
    public void NuGetCacheTool_SelfValidation_ValidateFlagProvided_RunsValidation()
    {
        // Arrange: no setup required
        // Act
        var exitCode = Runner.Run(
            out var output,
            "dotnet",
            _dllPath,
            "--validate");

        // Assert
        Assert.Equal(0, exitCode);
        Assert.Contains("Total Tests:", output);
        Assert.Contains("Passed:", output);
    }

    /// <summary>
    ///     Test that validate with results flag generates TRX file.
    /// </summary>
    [Fact]
    public void NuGetCacheTool_ResultsFile_ValidateWithTrxExtension_GeneratesTrxFile()
    {
        // Arrange
        var originalTempFile = Path.GetTempFileName();
        var resultsFile = Path.ChangeExtension(originalTempFile, ".trx");

        try
        {
            // Act
            var exitCode = Runner.Run(
                out var _,
                "dotnet",
                _dllPath,
                "--validate",
                "--results",
                resultsFile);

            // Assert
            Assert.Equal(0, exitCode);
            Assert.True(File.Exists(resultsFile), "Results file was not created");

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

            if (File.Exists(originalTempFile))
            {
                File.Delete(originalTempFile);
            }
        }
    }

    /// <summary>
    ///     Test that validate with results flag generates JUnit XML file.
    /// </summary>
    [Fact]
    public void NuGetCacheTool_ResultsFile_ValidateWithXmlExtension_GeneratesJUnitFile()
    {
        // Arrange
        var originalTempFile = Path.GetTempFileName();
        var resultsFile = Path.ChangeExtension(originalTempFile, ".xml");

        try
        {
            // Act
            var exitCode = Runner.Run(
                out var _,
                "dotnet",
                _dllPath,
                "--validate",
                "--results",
                resultsFile);

            // Assert
            Assert.Equal(0, exitCode);
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

            if (File.Exists(originalTempFile))
            {
                File.Delete(originalTempFile);
            }
        }
    }

    /// <summary>
    ///     Test that silent flag suppresses output.
    /// </summary>
    [Fact]
    public void NuGetCacheTool_SilentMode_SilentFlagProvided_SuppressesOutput()
    {
        // Arrange: no setup required
        // Act
        var exitCode = Runner.Run(
            out var output,
            "dotnet",
            _dllPath,
            "--silent");

        // Assert
        Assert.Equal(0, exitCode);
        Assert.True(string.IsNullOrWhiteSpace(output), "Silent mode should suppress console output");
    }

    /// <summary>
    ///     Test that log flag writes output to file.
    /// </summary>
    [Fact]
    public void NuGetCacheTool_LogFile_LogFlagProvided_WritesOutputToFile()
    {
        // Arrange
        var logFile = Path.GetTempFileName();

        try
        {
            // Act
            var exitCode = Runner.Run(
                out var _,
                "dotnet",
                _dllPath,
                "--log",
                logFile);

            // Assert
            Assert.Equal(0, exitCode);
            Assert.True(File.Exists(logFile), "Log file was not created");

            var logContent = File.ReadAllText(logFile);
            Assert.Contains("NuGet Cache Tool version", logContent);
        }
        finally
        {
            if (File.Exists(logFile))
            {
                File.Delete(logFile);
            }
        }
    }

    /// <summary>
    ///     Test that unknown argument returns error.
    /// </summary>
    [Fact]
    public void NuGetCacheTool_ErrorHandling_UnknownArgumentProvided_ReturnsError()
    {
        // Arrange: no setup required
        // Act
        var exitCode = Runner.Run(
            out var output,
            "dotnet",
            _dllPath,
            "--unknown");

        // Assert
        Assert.NotEqual(0, exitCode);
        Assert.Contains("Error", output);
    }

    /// <summary>
    ///     Test that a package argument caches the package and outputs the path.
    /// </summary>
    [Fact]
    public void NuGetCacheTool_PackageCaching_ValidPackageProvided_OutputsPath()
    {
        // Arrange: no setup required
        // Act
        var exitCode = Runner.Run(
            out var output,
            "dotnet",
            _dllPath,
            "DemaConsulting.NuGet.Caching:0.1.0");

        // Assert
        Assert.Equal(0, exitCode);
        Assert.False(string.IsNullOrWhiteSpace(output));
        Assert.DoesNotContain("Error", output);
    }

    /// <summary>
    ///     Test that attempting to cache a nonexistent package returns an error.
    /// </summary>
    [Fact]
    public void NuGetCacheTool_PackageCaching_NonexistentPackageProvided_ReturnsError()
    {
        // Arrange: no setup required
        // Act
        var exitCode = Runner.Run(
            out var output,
            "dotnet",
            _dllPath,
            "DemaConsulting.NonExistent.Package.XYZ:99.99.99");

        // Assert
        Assert.NotEqual(0, exitCode);
        Assert.Contains("Error", output);
    }

    /// <summary>
    ///     Test that specifying an invalid log file path returns an error.
    /// </summary>
    [Fact]
    public void NuGetCacheTool_LogFile_InvalidFilenameProvided_ReturnsError()
    {
        // Arrange - use a path into a nonexistent directory under a managed temporary root
        using var temporaryDirectory = new TemporaryDirectory();
        var invalidLogRelativePath = Path.Combine("nonexistent_subdir_xyz_abc", "invalid.log");
        var invalidLogPath = PathHelpers.SafePathCombine(temporaryDirectory.DirectoryPath, invalidLogRelativePath);

        // Act
        var exitCode = Runner.Run(
            out var output,
            "dotnet",
            _dllPath,
            "--log",
            invalidLogPath);

        // Assert
        Assert.NotEqual(0, exitCode);
        Assert.Contains("Error", output);
    }
}
