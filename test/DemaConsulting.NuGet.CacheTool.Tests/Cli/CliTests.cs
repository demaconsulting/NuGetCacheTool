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
        // Act: create context with --version flag
        using var context = Context.Create(["--version"]);

        // Assert: verify version flag is set and other flags are unset
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
        // Act: create context with -v flag
        using var context = Context.Create(["-v"]);

        // Assert: verify version flag is set on context
        Assert.IsTrue(context.Version);
        Assert.AreEqual(0, context.ExitCode);
    }

    /// <summary>
    ///     Test that the CLI accepts the --help flag and configures the context for help display.
    /// </summary>
    [TestMethod]
    public void Cli_HelpFlag_SetsHelpOnContext()
    {
        // Act: create context with --help flag
        using var context = Context.Create(["--help"]);

        // Assert: verify help flag is set and other flags are unset
        Assert.IsTrue(context.Help);
        Assert.IsFalse(context.Version);
        Assert.AreEqual(0, context.ExitCode);
    }

    /// <summary>
    ///     Test that the CLI accepts the short -h flag and configures the context for help display.
    /// </summary>
    [TestMethod]
    public void Cli_ShortHelpFlagH_SetsHelpOnContext()
    {
        // Act: create context with -h flag
        using var context = Context.Create(["-h"]);

        // Assert: verify help flag is set on context
        Assert.IsTrue(context.Help);
        Assert.AreEqual(0, context.ExitCode);
    }

    /// <summary>
    ///     Test that the CLI accepts the short -? flag and configures the context for help display.
    /// </summary>
    [TestMethod]
    public void Cli_ShortHelpFlagQuestionMark_SetsHelpOnContext()
    {
        // Act: create context with -? flag
        using var context = Context.Create(["-?"]);

        // Assert: verify help flag is set on context
        Assert.IsTrue(context.Help);
        Assert.AreEqual(0, context.ExitCode);
    }

    /// <summary>
    ///     Test that the CLI --silent flag suppresses both stdout and stderr output channels.
    /// </summary>
    [TestMethod]
    public void Cli_SilentFlag_SuppressesAllOutput()
    {
        // Arrange: redirect stdout and stderr to capture output
        var originalOut = Console.Out;
        var originalError = Console.Error;
        try
        {
            using var outWriter = new StringWriter();
            using var errWriter = new StringWriter();
            Console.SetOut(outWriter);
            Console.SetError(errWriter);
            using var context = Context.Create(["--silent"]);

            // Act: write to both output channels
            context.WriteLine("Standard output message");
            context.WriteError("Error output message");

            // Assert: verify both channels are suppressed
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
        // Act: create context with two package arguments
        using var context = Context.Create(["Package.One:1.0.0", "Package.Two:2.3.4"]);

        // Assert: verify both packages are added to the packages list
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
        // Arrange: redirect stderr to suppress output during test
        var originalError = Console.Error;
        try
        {
            Console.SetError(new StringWriter());
            using var context = Context.Create([]);

            // Act: trigger an error via WriteError
            context.WriteError("Something went wrong");

            // Assert: verify exit code is non-zero
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
        // Arrange: redirect stderr to capture error output
        var originalError = Console.Error;
        try
        {
            using var errWriter = new StringWriter();
            Console.SetError(errWriter);
            using var context = Context.Create([]);

            // Act: trigger an error via WriteError
            context.WriteError("Error details here");

            // Assert: verify error message appears in stderr
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
        // Act: verify that an unknown argument throws
        var exception = Assert.ThrowsExactly<ArgumentException>(() => Context.Create(["--unknown-flag"]));

        // Assert: the exception message mentions the unsupported argument
        Assert.Contains("Unsupported argument", exception.Message);
    }

    /// <summary>
    ///     Test that the CLI --log flag writes output to the specified log file.
    /// </summary>
    [TestMethod]
    public void Cli_LogFlag_WritesToLogFile()
    {
        // Arrange
        var logFile = Path.GetTempFileName();
        try
        {
            // Act: create context with --log flag and write a message
            using (var context = Context.Create(["--log", logFile]))
            {
                context.WriteLine("Log test message");
            }

            // Assert: verify the log file was written with the expected message
            Assert.IsTrue(File.Exists(logFile));
            var content = File.ReadAllText(logFile);
            Assert.Contains("Log test message", content);
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
    ///     Test that the CLI --validate flag configures the context to trigger self-validation.
    /// </summary>
    [TestMethod]
    public void Cli_ValidateFlag_SetsValidateOnContext()
    {
        // Act: create context with --validate flag
        using var context = Context.Create(["--validate"]);

        // Assert: verify validate flag is set and exit code is zero
        Assert.IsTrue(context.Validate);
        Assert.AreEqual(0, context.ExitCode);
    }

    /// <summary>
    ///     Test that the CLI --results flag sets the results file path on the context.
    /// </summary>
    [TestMethod]
    public void Cli_ResultsFlag_SetsResultsFileOnContext()
    {
        // Act: create context with --results flag
        using var context = Context.Create(["--results", "results.trx"]);

        // Assert: verify results file path is set and exit code is zero
        Assert.AreEqual("results.trx", context.ResultsFile);
        Assert.AreEqual(0, context.ExitCode);
    }

    /// <summary>
    ///     Test that the CLI rejects --log without a following value.
    /// </summary>
    [TestMethod]
    public void Cli_LogFlagWithoutValue_ThrowsArgumentException()
    {
        // Act & Assert: verify that --log without a value throws
        Assert.ThrowsExactly<ArgumentException>(() => Context.Create(["--log"]));
    }

    /// <summary>
    ///     Test that the CLI rejects --results without a following value.
    /// </summary>
    [TestMethod]
    public void Cli_ResultsFlagWithoutValue_ThrowsArgumentException()
    {
        // Act & Assert: verify that --results without a value throws
        Assert.ThrowsExactly<ArgumentException>(() => Context.Create(["--results"]));
    }

    /// <summary>
    ///     Test that the CLI --log flag writes output to the log file even when --silent is active.
    /// </summary>
    [TestMethod]
    public void Cli_SilentAndLog_WritesToLogFileOnly()
    {
        // Arrange: redirect stdout and stderr to verify they are suppressed
        var logFile = Path.GetTempFileName();
        var originalOut = Console.Out;
        var originalError = Console.Error;
        try
        {
            using var outWriter = new StringWriter();
            using var errWriter = new StringWriter();
            Console.SetOut(outWriter);
            Console.SetError(errWriter);

            // Act: create context with both --silent and --log flags and write a message
            using (var context = Context.Create(["--silent", "--log", logFile]))
            {
                context.WriteLine("Silent log test message");
            }

            // Assert: verify stdout is suppressed but log file received the message
            Assert.DoesNotContain("Silent log test message", outWriter.ToString());
            Assert.IsTrue(File.Exists(logFile));
            var content = File.ReadAllText(logFile);
            Assert.Contains("Silent log test message", content);
        }
        finally
        {
            Console.SetOut(originalOut);
            Console.SetError(originalError);
            if (File.Exists(logFile))
            {
                File.Delete(logFile);
            }
        }
    }
}
