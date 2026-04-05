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

namespace DemaConsulting.NuGet.CacheTool.Tests.Cli;

/// <summary>
///     Subsystem integration tests for the CLI subsystem (argument parsing and output management).
/// </summary>
[TestClass]
public class CliTests
{
    /// <summary>
    ///     Test that the CLI accepts the --version flag and configures the context for version display.
    /// </summary>
    [TestMethod]
    public void Cli_VersionFlag_SetsVersionOnContext()
    {
        // Act
        using var context = Context.Create(["--version"]);

        // Assert
        Assert.IsTrue(context.Version);
        Assert.IsFalse(context.Help);
        Assert.IsFalse(context.Silent);
        Assert.AreEqual(0, context.ExitCode);
    }

    /// <summary>
    ///     Test that the CLI accepts the short -v flag and configures the context for version display.
    /// </summary>
    [TestMethod]
    public void Cli_ShortVersionFlag_SetsVersionOnContext()
    {
        // Act
        using var context = Context.Create(["-v"]);

        // Assert
        Assert.IsTrue(context.Version);
        Assert.AreEqual(0, context.ExitCode);
    }

    /// <summary>
    ///     Test that the CLI accepts the --help flag and configures the context for help display.
    /// </summary>
    [TestMethod]
    public void Cli_HelpFlag_SetsHelpOnContext()
    {
        // Act
        using var context = Context.Create(["--help"]);

        // Assert
        Assert.IsTrue(context.Help);
        Assert.IsFalse(context.Version);
        Assert.AreEqual(0, context.ExitCode);
    }

    /// <summary>
    ///     Test that the CLI accepts the short -h and -? flags and configures the context for help display.
    /// </summary>
    [TestMethod]
    public void Cli_ShortHelpFlag_SetsHelpOnContext()
    {
        // Act - test -h form
        using var contextH = Context.Create(["-h"]);

        // Assert
        Assert.IsTrue(contextH.Help);
        Assert.AreEqual(0, contextH.ExitCode);

        // Act - test -? form
        using var contextQ = Context.Create(["-?"]);

        // Assert
        Assert.IsTrue(contextQ.Help);
        Assert.AreEqual(0, contextQ.ExitCode);
    }

    /// <summary>
    ///     Test that the CLI --silent flag suppresses both stdout and stderr output channels.
    /// </summary>
    [TestMethod]
    public void Cli_SilentFlag_SuppressesAllOutput()
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
            using var context = Context.Create(["--silent"]);

            // Act - write to both output channels
            context.WriteLine("Standard output message");
            context.WriteError("Error output message");

            // Assert - both channels must be suppressed
            Assert.DoesNotContain("Standard output message", outWriter.ToString());
            Assert.DoesNotContain("Error output message", errWriter.ToString());
        }
        finally
        {
            Console.SetOut(originalOut);
            Console.SetError(originalError);
        }
    }

    /// <summary>
    ///     Test that the CLI accepts [package-name]:[version] arguments and adds them to the packages list.
    /// </summary>
    [TestMethod]
    public void Cli_PackageArgument_AddedToPackagesList()
    {
        // Act
        using var context = Context.Create(["Package.One:1.0.0", "Package.Two:2.3.4"]);

        // Assert
        Assert.HasCount(2, context.Packages);
        Assert.AreEqual("Package.One:1.0.0", context.Packages[0]);
        Assert.AreEqual("Package.Two:2.3.4", context.Packages[1]);
    }

    /// <summary>
    ///     Test that the CLI WriteError sets a non-zero exit code on failure.
    /// </summary>
    [TestMethod]
    public void Cli_ErrorOutput_SetsNonZeroExitCode()
    {
        // Arrange
        var originalError = Console.Error;
        try
        {
            Console.SetError(new StringWriter());
            using var context = Context.Create([]);

            // Act
            context.WriteError("Something went wrong");

            // Assert
            Assert.AreNotEqual(0, context.ExitCode);
        }
        finally
        {
            Console.SetError(originalError);
        }
    }

    /// <summary>
    ///     Test that the CLI WriteError writes the error message to the console when not in silent mode.
    /// </summary>
    [TestMethod]
    public void Cli_ErrorOutput_WritesMessageToConsole()
    {
        // Arrange
        var originalError = Console.Error;
        try
        {
            using var errWriter = new StringWriter();
            Console.SetError(errWriter);
            using var context = Context.Create([]);

            // Act
            context.WriteError("Error details here");

            // Assert
            Assert.Contains("Error details here", errWriter.ToString());
        }
        finally
        {
            Console.SetError(originalError);
        }
    }

    /// <summary>
    ///     Test that the CLI rejects an unknown argument with a descriptive ArgumentException.
    /// </summary>
    [TestMethod]
    public void Cli_UnknownArgument_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Context.Create(["--unknown-flag"]));
        Assert.Contains("Unsupported argument", exception.Message);
    }

    /// <summary>
    ///     Test that the CLI rejects --log without a following value.
    /// </summary>
    [TestMethod]
    public void Cli_LogFlagWithoutValue_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Context.Create(["--log"]));
    }

    /// <summary>
    ///     Test that the CLI rejects --results without a following value.
    /// </summary>
    [TestMethod]
    public void Cli_ResultsFlagWithoutValue_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Context.Create(["--results"]));
    }
}
