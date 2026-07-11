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

namespace DemaConsulting.NuGet.CacheTool.Tests.Utilities;

/// <summary>
///     Tests for the PathHelpers class.
/// </summary>
[Collection("Sequential")]
public class PathHelpersTests
{
    /// <summary>
    ///     Test that SafePathCombine correctly combines valid paths.
    /// </summary>
    [Fact]
    public void PathHelpers_SafePathCombine_ValidPaths_CombinesCorrectly()
    {
        // Arrange
        var basePath = "/home/user/project";
        var relativePath = "subfolder/file.txt";

        // Act
        var result = PathHelpers.SafePathCombine(basePath, relativePath);

        // Assert
        Assert.Equal(Path.Combine(basePath, relativePath), result);
    }

    /// <summary>
    ///     Test that SafePathCombine throws ArgumentException for path traversal with double dots.
    /// </summary>
    [Fact]
    public void PathHelpers_SafePathCombine_PathTraversalWithDoubleDots_ThrowsArgumentException()
    {
        // Arrange
        var basePath = "/home/user/project";
        var relativePath = "../etc/passwd";

        // Act
        var act = () => PathHelpers.SafePathCombine(basePath, relativePath);

        // Assert
        var exception = Assert.Throws<ArgumentException>(act);
        Assert.Contains("Invalid path component", exception.Message);
    }

    /// <summary>
    ///     Test that SafePathCombine throws ArgumentException for path with double dots in middle.
    /// </summary>
    [Fact]
    public void PathHelpers_SafePathCombine_DoubleDotsInMiddle_ThrowsArgumentException()
    {
        // Arrange
        var basePath = "/home/user/project";
        var relativePath = "subfolder/../../../etc/passwd";

        // Act
        var act = () => PathHelpers.SafePathCombine(basePath, relativePath);

        // Assert
        var exception = Assert.Throws<ArgumentException>(act);
        Assert.Contains("Invalid path component", exception.Message);
    }

    /// <summary>
    ///     Test that SafePathCombine throws ArgumentException for a Unix absolute path.
    /// </summary>
    [Fact]
    public void PathHelpers_SafePathCombine_UnixAbsolutePath_ThrowsArgumentException()
    {
        // Arrange
        const string basePath = "/tmp/base";
        const string relativePath = "/etc/passwd";

        // Act
        var act = () => PathHelpers.SafePathCombine(basePath, relativePath);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    /// <summary>
    ///     Test that SafePathCombine throws ArgumentException for a Windows absolute path.
    /// </summary>
    [Fact]
    public void PathHelpers_SafePathCombine_WindowsAbsolutePath_ThrowsArgumentException()
    {
        // This test only applies on Windows where drive-letter paths are rooted
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        // Arrange
        const string basePath = "/tmp/base";
        const string relativePath = @"C:\Windows\System32";

        // Act
        var act = () => PathHelpers.SafePathCombine(basePath, relativePath);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    /// <summary>
    ///     Test that SafePathCombine throws ArgumentException for a rooted relative path that
    ///     would still resolve inside the base directory, proving rooted paths are rejected
    ///     upfront regardless of where they would resolve to.
    /// </summary>
    [Fact]
    public void PathHelpers_SafePathCombine_RootedPathInsideBase_ThrowsArgumentException()
    {
        // Arrange: a rooted relative path that already resolves inside the base directory
        var basePath = Path.GetTempPath();
        var relativePath = Path.Combine(basePath, "child.txt");

        // Act
        var act = () => PathHelpers.SafePathCombine(basePath, relativePath);

        // Assert
        var exception = Assert.Throws<ArgumentException>(act);
        Assert.Contains("Invalid path component", exception.Message);
    }

    /// <summary>
    ///     Test that SafePathCombine correctly handles current directory reference.
    /// </summary>
    [Fact]
    public void PathHelpers_SafePathCombine_CurrentDirectoryReference_CombinesCorrectly()
    {
        // Arrange
        var basePath = "/home/user/project";
        var relativePath = "./subfolder/file.txt";

        // Act
        var result = PathHelpers.SafePathCombine(basePath, relativePath);

        // Assert
        Assert.Equal(Path.Combine(basePath, relativePath), result);
    }

    /// <summary>
    ///     Test that SafePathCombine correctly handles nested paths.
    /// </summary>
    [Fact]
    public void PathHelpers_SafePathCombine_NestedPaths_CombinesCorrectly()
    {
        // Arrange
        var basePath = "/home/user/project";
        var relativePath = "level1/level2/level3/file.txt";

        // Act
        var result = PathHelpers.SafePathCombine(basePath, relativePath);

        // Assert
        Assert.Equal(Path.Combine(basePath, relativePath), result);
    }

    /// <summary>
    ///     Test that SafePathCombine correctly handles empty relative path.
    /// </summary>
    [Fact]
    public void PathHelpers_SafePathCombine_EmptyRelativePath_ReturnsBasePath()
    {
        // Arrange
        var basePath = "/home/user/project";
        var relativePath = "";

        // Act
        var result = PathHelpers.SafePathCombine(basePath, relativePath);

        // Assert
        Assert.Equal(Path.Combine(basePath, relativePath), result);
    }

    /// <summary>
    ///     Test that SafePathCombine correctly handles filenames that start with double dots but stay within the base directory.
    /// </summary>
    [Fact]
    public void PathHelpers_SafePathCombine_DoubleDotPrefix_CombinesCorrectly()
    {
        // Arrange
        var basePath = "/home/user/project";
        var relativePath = "..data/file.txt";

        // Act
        var result = PathHelpers.SafePathCombine(basePath, relativePath);

        // Assert
        Assert.Equal(Path.Combine(basePath, relativePath), result);
    }

    /// <summary>
    ///     Test that SafePathCombine throws ArgumentNullException when base path is null.
    /// </summary>
    [Fact]
    public void PathHelpers_SafePathCombine_NullBase_ThrowsArgumentNullException()
    {
        // Arrange
        const string relativePath = "relative/path";

        // Act
        var act = () => PathHelpers.SafePathCombine(null!, relativePath);

        // Assert
        Assert.Throws<ArgumentNullException>(act);
    }

    /// <summary>
    ///     Test that SafePathCombine throws ArgumentNullException when relative path is null.
    /// </summary>
    [Fact]
    public void PathHelpers_SafePathCombine_NullRelative_ThrowsArgumentNullException()
    {
        // Arrange
        const string basePath = "/base/path";

        // Act
        var act = () => PathHelpers.SafePathCombine(basePath, null!);

        // Assert
        Assert.Throws<ArgumentNullException>(act);
    }
}
