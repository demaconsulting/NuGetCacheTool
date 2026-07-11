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

using System.Runtime.InteropServices;
using DemaConsulting.NuGet.CacheTool.Cli;
using DemaConsulting.NuGet.CacheTool.Utilities;
using DemaConsulting.TestResults.IO;

namespace DemaConsulting.NuGet.CacheTool.SelfTest;

/// <summary>
///     Provides self-validation functionality for the NuGet Cache Tool.
/// </summary>
internal static class Validation
{
    /// <summary>
    ///     Runs self-validation tests and optionally writes results to a file.
    /// </summary>
    /// <param name="context">The context containing command line arguments and program state.</param>
    public static void Run(Context context)
    {
        // Validate input
        ArgumentNullException.ThrowIfNull(context);

        // Print validation header
        PrintValidationHeader(context);

        // Create test results collection
        var testResults = new DemaConsulting.TestResults.TestResults
        {
            Name = "NuGet Cache Tool Self-Validation"
        };

        // Run core functionality tests
        RunVersionTest(context, testResults);
        RunHelpTest(context, testResults);
        RunCachePackageTest(context, testResults);

        // Calculate totals
        var totalTests = testResults.Results.Count;
        var passedTests = testResults.Results.Count(t => t.Outcome == DemaConsulting.TestResults.TestOutcome.Passed);
        var failedTests = testResults.Results.Count(t => t.Outcome == DemaConsulting.TestResults.TestOutcome.Failed);

        // Print summary
        context.WriteLine("");
        context.WriteLine($"Total Tests: {totalTests}");
        context.WriteLine($"Passed: {passedTests}");
        if (failedTests > 0)
        {
            context.WriteError($"Failed: {failedTests}");
        }
        else
        {
            context.WriteLine($"Failed: {failedTests}");
        }

        // Write results file if requested
        if (context.ResultsFile != null)
        {
            WriteResultsFile(context, testResults);
        }
    }

    /// <summary>
    ///     Prints the validation header with system information.
    /// </summary>
    /// <param name="context">The context for output.</param>
    private static void PrintValidationHeader(Context context)
    {
        context.WriteLine("# DEMA Consulting NuGet Cache Tool");
        context.WriteLine("");
        context.WriteLine("| Information         | Value                                              |");
        context.WriteLine("| :------------------ | :------------------------------------------------- |");
        context.WriteLine($"| Tool Version        | {Program.Version,-50} |");
        context.WriteLine($"| Machine Name        | {Environment.MachineName,-50} |");
        context.WriteLine($"| OS Version          | {RuntimeInformation.OSDescription,-50} |");
        context.WriteLine($"| DotNet Runtime      | {RuntimeInformation.FrameworkDescription,-50} |");
        context.WriteLine($"| Time Stamp          | {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC{"",-29} |");
        context.WriteLine("");
    }

    /// <summary>
    ///     Runs a test for version display functionality.
    /// </summary>
    /// <param name="context">The context for output.</param>
    /// <param name="testResults">The test results collection.</param>
    private static void RunVersionTest(Context context, DemaConsulting.TestResults.TestResults testResults)
    {
        RunValidationTest(
            context,
            testResults,
            "NuGetCache_VersionDisplay",
            "Version Display Test",
            ["--version"],
            logContent =>
            {
                // Verify version string is in log (version matches semantic version pattern)
                if (System.Text.RegularExpressions.Regex.IsMatch(logContent.Trim(), @"\d+\.\d+\.\d+"))
                {
                    return null;
                }

                return "Version string not found in log";
            });
    }

    /// <summary>
    ///     Runs a test for help display functionality.
    /// </summary>
    /// <param name="context">The context for output.</param>
    /// <param name="testResults">The test results collection.</param>
    private static void RunHelpTest(Context context, DemaConsulting.TestResults.TestResults testResults)
    {
        RunValidationTest(
            context,
            testResults,
            "NuGetCache_HelpDisplay",
            "Help Display Test",
            ["--help"],
            logContent =>
            {
                // Verify help text is in log
                if (logContent.Contains("Usage:") && logContent.Contains("Options:"))
                {
                    return null;
                }

                return "Help text not found in log";
            });
    }

    /// <summary>
    ///     Known package/version used to prove the cache-package self-test actually resolves a
    ///     real, existing cached package directory rather than merely producing log output.
    /// </summary>
    private const string CachePackageTestId = "DemaConsulting.NuGet.Caching";

    /// <summary>
    ///     Known package version used alongside <see cref="CachePackageTestId"/> for the
    ///     cache-package self-test.
    /// </summary>
    private const string CachePackageTestVersion = "0.1.0";

    /// <summary>
    ///     Runs a test for NuGet package caching functionality.
    /// </summary>
    /// <param name="context">The context for output.</param>
    /// <param name="testResults">The test results collection.</param>
    private static void RunCachePackageTest(Context context, DemaConsulting.TestResults.TestResults testResults)
    {
        RunValidationTest(
            context,
            testResults,
            "NuGetCache_CachePackage",
            "Cache Package Test",
            [$"{CachePackageTestId}:{CachePackageTestVersion}"],
            logContent =>
            {
                // The log contains the banner followed by the cached package path as its final
                // line (see Program.RunToolLogic), so the path must be extracted from the last
                // non-blank line rather than treating the whole log as the path. Verify the log
                // contains a cached package path rather than accepting any non-empty output: the
                // path must actually exist on disk, and must be named for the requested package id
                // and version, so this test genuinely proves that caching produced a valid, usable
                // package location.
                var packagePath = logContent
                    .Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
                    .LastOrDefault()
                    ?.Trim();

                if (string.IsNullOrWhiteSpace(packagePath))
                {
                    return "Package path not found in log";
                }

                if (!Directory.Exists(packagePath))
                {
                    return $"Cached package path '{packagePath}' does not exist on disk";
                }

                return ValidateCachePackagePath(packagePath, CachePackageTestId, CachePackageTestVersion);
            });
    }

    /// <summary>
    ///     Verifies that <paramref name="packagePath"/> is named for the exact requested package
    ///     identity by checking the directory name (version) and its parent directory name
    ///     (package ID) explicitly, rather than a substring match against the full path. A
    ///     substring match could produce false positives — for example, version "0.1.0" would
    ///     also match a path for "0.1.0-beta" or "10.1.0" — which would silently defeat this
    ///     regression check. Extracted as its own method (rather than an inline check) so it can
    ///     be exercised directly by unit tests against known-good and known-bad paths, proving
    ///     the check itself would catch a regression rather than only proving the overall
    ///     self-test happens to pass today.
    /// </summary>
    /// <param name="packagePath">The resolved cached package directory path.</param>
    /// <param name="expectedPackageId">The expected package ID.</param>
    /// <param name="expectedVersion">The expected package version.</param>
    /// <returns>An error message if the path does not match, or null if it does.</returns>
    internal static string? ValidateCachePackagePath(
        string packagePath,
        string expectedPackageId,
        string expectedVersion)
    {
        var trimmedPath = packagePath.TrimEnd('/', '\\');
        var versionDirectoryName = Path.GetFileName(trimmedPath);
        var packageIdDirectoryName = Path.GetFileName(Path.GetDirectoryName(trimmedPath));

        if (!string.Equals(versionDirectoryName, expectedVersion, StringComparison.OrdinalIgnoreCase) ||
            !string.Equals(packageIdDirectoryName, expectedPackageId, StringComparison.OrdinalIgnoreCase))
        {
            return $"Cached package path '{packagePath}' does not reference the expected " +
                $"package '{expectedPackageId}' version '{expectedVersion}'";
        }

        return null;
    }

    /// <summary>
    ///     Runs a validation test with common test execution logic.
    /// </summary>
    /// <param name="context">The context for output.</param>
    /// <param name="testResults">The test results collection.</param>
    /// <param name="testName">The name of the test.</param>
    /// <param name="displayName">The display name for console output.</param>
    /// <param name="additionalArgs">Additional command-line arguments for the test.</param>
    /// <param name="validator">
    ///     Function to validate test results. Receives log content and returns null on
    ///     success or an error message on failure.
    /// </param>
    private static void RunValidationTest(
        Context context,
        DemaConsulting.TestResults.TestResults testResults,
        string testName,
        string displayName,
        string[] additionalArgs,
        Func<string, string?> validator)
    {
        var startTime = DateTime.UtcNow;
        var test = CreateTestResult(testName);

        try
        {
            using var tempDir = new TemporaryDirectory();
            var logFile = tempDir.GetFilePath($"{testName}.log");

            // Build command line arguments: always use --silent and --log for consistent capture
            var args = new List<string> { "--silent", "--log", logFile };
            args.AddRange(additionalArgs);

            // Run the program
            int exitCode;
            using (var testContext = Context.Create([.. args]))
            {
                Program.Run(testContext);
                exitCode = testContext.ExitCode;
            }

            // Check if execution succeeded
            if (exitCode == 0)
            {
                // Read log content and invoke the validator
                var logContent = File.ReadAllText(logFile);
                var errorMessage = validator(logContent);

                if (errorMessage == null)
                {
                    test.Outcome = DemaConsulting.TestResults.TestOutcome.Passed;
                    context.WriteLine($"✓ {displayName} - PASSED");
                }
                else
                {
                    test.Outcome = DemaConsulting.TestResults.TestOutcome.Failed;
                    test.ErrorMessage = errorMessage;
                    context.WriteError($"✗ {displayName} - FAILED: {errorMessage}");
                }
            }
            else
            {
                test.Outcome = DemaConsulting.TestResults.TestOutcome.Failed;
                test.ErrorMessage = $"Program exited with code {exitCode}";
                context.WriteError($"✗ {displayName} - FAILED: Exit code {exitCode}");
            }
        }
        // Generic catch is justified here as this is a test framework - any exception should be
        // recorded as a test failure to ensure robust test execution and reporting.
        catch (Exception ex)
        {
            HandleTestException(test, context, displayName, ex);
        }

        FinalizeTestResult(test, startTime, testResults);
    }

    /// <summary>
    ///     Writes test results to a file in TRX or JUnit format.
    /// </summary>
    /// <param name="context">The context for output.</param>
    /// <param name="testResults">The test results to write.</param>
    private static void WriteResultsFile(Context context, DemaConsulting.TestResults.TestResults testResults)
    {
        if (context.ResultsFile == null)
        {
            return;
        }

        try
        {
            var extension = Path.GetExtension(context.ResultsFile).ToLowerInvariant();
            string content;

            if (extension == ".trx")
            {
                content = TrxSerializer.Serialize(testResults);
            }
            else if (extension == ".xml")
            {
                // Assume JUnit format for .xml extension
                content = JUnitSerializer.Serialize(testResults);
            }
            else
            {
                context.WriteError($"Error: Unsupported results file format '{extension}'. Use .trx or .xml extension.");
                return;
            }

            File.WriteAllText(context.ResultsFile, content);
            context.WriteLine($"Results written to {context.ResultsFile}");
        }
        // Generic catch is justified here as a top-level handler to log file write errors
        catch (Exception ex)
        {
            context.WriteError($"Error: Failed to write results file: {ex.Message}");
        }
    }

    /// <summary>
    ///     Creates a new test result object with common properties.
    /// </summary>
    /// <param name="testName">The name of the test.</param>
    /// <returns>A new test result object.</returns>
    private static DemaConsulting.TestResults.TestResult CreateTestResult(string testName)
    {
        return new DemaConsulting.TestResults.TestResult
        {
            Name = testName,
            ClassName = "Validation",
            CodeBase = "NuGetCacheTool"
        };
    }

    /// <summary>
    ///     Finalizes a test result by setting its duration and adding it to the collection.
    /// </summary>
    /// <param name="test">The test result to finalize.</param>
    /// <param name="startTime">The start time of the test.</param>
    /// <param name="testResults">The test results collection to add to.</param>
    private static void FinalizeTestResult(
        DemaConsulting.TestResults.TestResult test,
        DateTime startTime,
        DemaConsulting.TestResults.TestResults testResults)
    {
        test.Duration = DateTime.UtcNow - startTime;
        testResults.Results.Add(test);
    }

    /// <summary>
    ///     Handles test exceptions by setting failure information and logging the error.
    /// </summary>
    /// <param name="test">The test result to update.</param>
    /// <param name="context">The context for output.</param>
    /// <param name="testName">The name of the test for error messages.</param>
    /// <param name="ex">The exception that occurred.</param>
    private static void HandleTestException(
        DemaConsulting.TestResults.TestResult test,
        Context context,
        string testName,
        Exception ex)
    {
        test.Outcome = DemaConsulting.TestResults.TestOutcome.Failed;
        test.ErrorMessage = $"Exception: {ex.Message}";
        context.WriteError($"✗ {testName} - FAILED: {ex.Message}");
    }
}
