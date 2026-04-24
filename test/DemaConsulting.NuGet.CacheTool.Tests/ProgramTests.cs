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

namespace DemaConsulting.NuGet.CacheTool.Tests;

/// <summary>
///     Unit tests for the Program class.
/// </summary>
[TestClass]
public class ProgramTests
{
    /// <summary>
    ///     Known package used for caching tests: a stable, small package available on NuGet.
    /// </summary>
    private const string TestPackageArg = "DemaConsulting.NuGet.Caching:0.1.0";

    /// <summary>
    ///     Test that Run with version flag displays version only.
    /// </summary>
    [TestMethod]
    public void Program_Run_WithVersionFlag_DisplaysVersionOnly()
    {
        // Arrange
        var originalOut = Console.Out;
        try
        {
            using var outWriter = new StringWriter();
            Console.SetOut(outWriter);
            using var context = Context.Create(["--version"]);

            // Act
            Program.Run(context);

            // Assert
            var output = outWriter.ToString();
            Assert.DoesNotContain("Copyright", output);
            Assert.DoesNotContain("NuGet Cache Tool version", output);
            Assert.IsFalse(string.IsNullOrWhiteSpace(output), "Version flag should output a non-empty version string");
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    /// <summary>
    ///     Test that Run with help flag displays usage information.
    /// </summary>
    [TestMethod]
    public void Program_Run_WithHelpFlag_DisplaysUsageInformation()
    {
        // Arrange
        var originalOut = Console.Out;
        try
        {
            using var outWriter = new StringWriter();
            Console.SetOut(outWriter);
            using var context = Context.Create(["--help"]);

            // Act
            Program.Run(context);

            // Assert
            var output = outWriter.ToString();
            Assert.Contains("Usage:", output);
            Assert.Contains("Options:", output);
            Assert.Contains("--version", output);
            Assert.Contains("--help", output);
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    /// <summary>
    ///     Test that Run with validate flag runs validation.
    /// </summary>
    [TestMethod]
    public void Program_Run_WithValidateFlag_RunsValidation()
    {
        // Arrange
        var originalOut = Console.Out;
        try
        {
            using var outWriter = new StringWriter();
            Console.SetOut(outWriter);
            using var context = Context.Create(["--validate"]);

            // Act
            Program.Run(context);

            // Assert
            var output = outWriter.ToString();
            Assert.Contains("Total Tests:", output);
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    /// <summary>
    ///     Test that Run with no arguments displays the banner but does nothing else.
    /// </summary>
    [TestMethod]
    public void Program_Run_NoArguments_DisplaysDefaultBehavior()
    {
        // Arrange
        var originalOut = Console.Out;
        try
        {
            using var outWriter = new StringWriter();
            Console.SetOut(outWriter);
            using var context = Context.Create([]);

            // Act
            Program.Run(context);

            // Assert - banner is printed even with no package arguments
            var output = outWriter.ToString();
            Assert.Contains("NuGet Cache Tool version", output);
            Assert.Contains("Copyright", output);
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    /// <summary>
    ///     Test that Run with a valid package argument caches the package and outputs its path.
    /// </summary>
    [TestMethod]
    public void Program_Run_WithPackageArgument_CachesPackage()
    {
        // Arrange: redirect stdout to capture program output
        var originalOut = Console.Out;
        try
        {
            using var outWriter = new StringWriter();
            Console.SetOut(outWriter);
            using var context = Context.Create([TestPackageArg]);

            // Act: run program with a valid package argument
            Program.Run(context);

            // Assert: verify output contains the cached package ID and version
            Assert.AreEqual(0, context.ExitCode);
            var output = outWriter.ToString();
            var normalizedOutput = output.ToLowerInvariant();
            Assert.Contains("demaconsulting.nuget.caching", normalizedOutput);
            Assert.Contains("0.1.0", normalizedOutput);
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    /// <summary>
    ///     Test that version property returns non-empty version string.
    /// </summary>
    [TestMethod]
    public void Program_Version_ReturnsNonEmptyString()
    {
        // Act
        var version = Program.Version;

        // Assert
        Assert.IsFalse(string.IsNullOrWhiteSpace(version));
    }

    /// <summary>
    ///     Test that Run with validate flag and unsupported results format sets error exit code.
    /// </summary>
    [TestMethod]
    public void Program_Run_WithValidateAndUnsupportedResultsFormat_SetsErrorExitCode()
    {
        // Arrange
        var originalOut = Console.Out;
        var originalError = Console.Error;
        try
        {
            using var outWriter = new StringWriter();
            using var errWriter = new StringWriter();
            Console.SetOut(outWriter);
            Console.SetError(errWriter);
            using var context = Context.Create(["--validate", "--results", "output.json"]);

            // Act
            Program.Run(context);

            // Assert - unsupported format should cause an error with a message
            Assert.AreEqual(1, context.ExitCode);
            Assert.Contains("Error", errWriter.ToString());
        }
        finally
        {
            Console.SetOut(originalOut);
            Console.SetError(originalError);
        }
    }
}
