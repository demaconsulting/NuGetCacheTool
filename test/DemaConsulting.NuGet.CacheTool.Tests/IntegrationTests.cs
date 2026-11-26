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

using System.IO.Compression;
using DemaConsulting.HelpCards.Catalog;
using DemaConsulting.HelpCards.Testing;
using DemaConsulting.NuGet.CacheTool.Cli;
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

    /// <summary>
    ///     Test that the help verb lists the help topics.
    /// </summary>
    [Fact]
    public void NuGetCacheTool_HelpTopics_HelpVerb_ListsTopics()
    {
        // Arrange: (none — constructor initializes _dllPath)

        // Act: run the tool with the help verb
        var exitCode = Runner.Run(
            out var output,
            "dotnet",
            _dllPath,
            "help");

        // Assert: every topic is listed with its command, without the banner
        Assert.Equal(0, exitCode);
        foreach (var topic in HelpCommand.RootCardIds)
        {
            Assert.Contains($"{HelpCommand.ToolName} help {topic}", output);
        }

        Assert.DoesNotContain("Copyright", output);
    }

    /// <summary>
    ///     Test that an unknown help topic returns the usage-error exit code.
    /// </summary>
    [Fact]
    public void NuGetCacheTool_HelpTopics_UnknownTopic_ReturnsUsageError()
    {
        // Arrange: (none — constructor initializes _dllPath)

        // Act: ask for a topic that does not exist
        var exitCode = Runner.Run(
            out var output,
            "dotnet",
            _dllPath,
            "help",
            "no-such-topic");

        // Assert: exit code 2 and a message that names the topic
        Assert.Equal(2, exitCode);
        Assert.Contains("no-such-topic", output);
    }

    /// <summary>
    ///     Test that every command example in the help cards gives the result that its card states.
    /// </summary>
    [Fact]
    public void NuGetCacheTool_HelpExamples_AllCommandExamples_GiveStatedResults()
    {
        // Arrange: the catalog the tool ships, and the manifest of the demo package that the examples cache
        var catalog = HelpCatalog.Create(HelpCommand.CreateOptions().GetCatalogConfiguration());
        var nuspecPath = PathHelpers.SafePathCombine(
            PathHelpers.SafePathCombine(
                PathHelpers.SafePathCombine(AppContext.BaseDirectory, "TestData"),
                "HelpExamples"),
            "Demo.Package.nuspec");

        // Act: run every command example against the built tool in the runner's scratch folder; the runner
        // compares the exit codes and the 'expect' fragments. Before every command the demo package is packed
        // into the 'feed' folder, the local feed of the nuget.config that a card saves there.
        var report = HelpExampleRunner.Run(catalog, invocation =>
        {
            var feedPath = PathHelpers.SafePathCombine(invocation.WorkingDirectory, "feed");
            var packagePath = PathHelpers.SafePathCombine(feedPath, "demo.package.1.0.0.nupkg");
            if (!File.Exists(packagePath))
            {
                Directory.CreateDirectory(feedPath);
                using var package = ZipFile.Open(packagePath, ZipArchiveMode.Create);
                package.CreateEntryFromFile(nuspecPath, "Demo.Package.nuspec");
            }

            // Remove NUGET_PACKAGES so the globalPackagesFolder of the saved nuget.config applies and the demo
            // package never reaches a real global packages folder
            var exitCode = Runner.RunInDirectory(
                out var output,
                invocation.WorkingDirectory,
                new Dictionary<string, string?> { ["NUGET_PACKAGES"] = null },
                "dotnet",
                [_dllPath, .. invocation.Arguments]);
            return new HelpExampleOutcome(exitCode, output);
        });

        // Assert: the cards hold command examples, and every one gave the result its card states
        Assert.True(report.CommandsRun > 0, "The help cards contain no command examples.");
        report.AssertSucceeded();
    }
}
