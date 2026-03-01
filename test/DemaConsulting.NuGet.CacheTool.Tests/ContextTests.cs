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

namespace DemaConsulting.NuGet.CacheTool.Tests;

/// <summary>
///     Unit tests for the Context class.
/// </summary>
[TestClass]
public class ContextTests
{
    /// <summary>
    ///     Test creating a context with no arguments.
    /// </summary>
    [TestMethod]
    public void Context_Create_NoArguments_ReturnsDefaultContext()
    {
        // Act
        using var context = Context.Create([]);

        // Assert
        Assert.IsFalse(context.Version);
        Assert.IsFalse(context.Help);
        Assert.IsFalse(context.Silent);
        Assert.IsFalse(context.Validate);
        Assert.AreEqual(0, context.ExitCode);
    }

    /// <summary>
    ///     Test creating a context with the version flag.
    /// </summary>
    [TestMethod]
    public void Context_Create_VersionFlag_SetsVersionTrue()
    {
        // Act
        using var context = Context.Create(["--version"]);

        // Assert
        Assert.IsTrue(context.Version);
        Assert.IsFalse(context.Help);
        Assert.AreEqual(0, context.ExitCode);
    }

    /// <summary>
    ///     Test creating a context with the short version flag.
    /// </summary>
    [TestMethod]
    public void Context_Create_ShortVersionFlag_SetsVersionTrue()
    {
        // Act
        using var context = Context.Create(["-v"]);

        // Assert
        Assert.IsTrue(context.Version);
        Assert.IsFalse(context.Help);
        Assert.AreEqual(0, context.ExitCode);
    }

    /// <summary>
    ///     Test creating a context with the help flag.
    /// </summary>
    [TestMethod]
    public void Context_Create_HelpFlag_SetsHelpTrue()
    {
        // Act
        using var context = Context.Create(["--help"]);

        // Assert
        Assert.IsFalse(context.Version);
        Assert.IsTrue(context.Help);
        Assert.AreEqual(0, context.ExitCode);
    }

    /// <summary>
    ///     Test creating a context with the short help flag -h.
    /// </summary>
    [TestMethod]
    public void Context_Create_ShortHelpFlag_H_SetsHelpTrue()
    {
        // Act
        using var context = Context.Create(["-h"]);

        // Assert
        Assert.IsFalse(context.Version);
        Assert.IsTrue(context.Help);
        Assert.AreEqual(0, context.ExitCode);
    }

    /// <summary>
    ///     Test creating a context with the short help flag -?.
    /// </summary>
    [TestMethod]
    public void Context_Create_ShortHelpFlag_Question_SetsHelpTrue()
    {
        // Act
        using var context = Context.Create(["-?"]);

        // Assert
        Assert.IsFalse(context.Version);
        Assert.IsTrue(context.Help);
        Assert.AreEqual(0, context.ExitCode);
    }

    /// <summary>
    ///     Test creating a context with the silent flag.
    /// </summary>
    [TestMethod]
    public void Context_Create_SilentFlag_SetsSilentTrue()
    {
        // Act
        using var context = Context.Create(["--silent"]);

        // Assert
        Assert.IsTrue(context.Silent);
        Assert.AreEqual(0, context.ExitCode);
    }

    /// <summary>
    ///     Test creating a context with the validate flag.
    /// </summary>
    [TestMethod]
    public void Context_Create_ValidateFlag_SetsValidateTrue()
    {
        // Act
        using var context = Context.Create(["--validate"]);

        // Assert
        Assert.IsTrue(context.Validate);
        Assert.AreEqual(0, context.ExitCode);
    }

    /// <summary>
    ///     Test creating a context with the results flag.
    /// </summary>
    [TestMethod]
    public void Context_Create_ResultsFlag_SetsResultsFile()
    {
        // Act
        using var context = Context.Create(["--results", "test.trx"]);

        // Assert
        Assert.AreEqual("test.trx", context.ResultsFile);
        Assert.AreEqual(0, context.ExitCode);
    }

    /// <summary>
    ///     Test creating a context with the log flag.
    /// </summary>
    [TestMethod]
    public void Context_Create_LogFlag_OpensLogFile()
    {
        // Arrange
        var logFile = Path.GetTempFileName();
        try
        {
            // Act
            using (var context = Context.Create(["--log", logFile]))
            {
                context.WriteLine("Test message");
                Assert.AreEqual(0, context.ExitCode);
            }

            // Assert
            // Verify log file was written
            Assert.IsTrue(File.Exists(logFile));
            var logContent = File.ReadAllText(logFile);
            Assert.Contains("Test message", logContent);
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
    ///     Test creating a context with an unknown argument throws exception.
    /// </summary>
    [TestMethod]
    public void Context_Create_UnknownArgument_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Context.Create(["--unknown"]));
        Assert.Contains("Unsupported argument", exception.Message);
    }

    /// <summary>
    ///     Test creating a context with a package argument adds it to the Packages list.
    /// </summary>
    [TestMethod]
    public void Context_Create_PackageArgument_AddsToPackagesList()
    {
        // Arrange & Act
        using var context = Context.Create(["DemaConsulting.NuGet.Caching:0.1.0"]);

        // Assert
        Assert.AreEqual(1, context.Packages.Count);
        Assert.AreEqual("DemaConsulting.NuGet.Caching:0.1.0", context.Packages[0]);
    }

    /// <summary>
    ///     Test WriteLine writes to console output when not silent.
    /// </summary>
    [TestMethod]
    public void Context_WriteLine_NotSilent_WritesToConsole()
    {
        // Arrange
        var originalOut = Console.Out;
        try
        {
            using var outWriter = new StringWriter();
            Console.SetOut(outWriter);
            using var context = Context.Create([]);

            // Act
            context.WriteLine("Test message");

            // Assert
            var output = outWriter.ToString();
            Assert.Contains("Test message", output);
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    /// <summary>
    ///     Test WriteLine does not write to console when silent.
    /// </summary>
    [TestMethod]
    public void Context_WriteLine_Silent_DoesNotWriteToConsole()
    {
        // Arrange
        var originalOut = Console.Out;
        try
        {
            using var outWriter = new StringWriter();
            Console.SetOut(outWriter);
            using var context = Context.Create(["--silent"]);

            // Act
            context.WriteLine("Test message");

            // Assert
            var output = outWriter.ToString();
            Assert.DoesNotContain("Test message", output);
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }
}
