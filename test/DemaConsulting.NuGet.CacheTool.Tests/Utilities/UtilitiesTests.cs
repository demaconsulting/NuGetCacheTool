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
///     Subsystem integration tests for the Utilities subsystem, exercising
///     <see cref="TemporaryDirectory"/> and <see cref="PathHelpers"/> collaborating together.
/// </summary>
/// <remarks>
///     These tests operate on the real filesystem to verify that path-boundary enforcement
///     flows end-to-end from <see cref="TemporaryDirectory.GetFilePath"/> through
///     <see cref="PathHelpers.SafePathCombine"/>. The <c>[Collection("Sequential")]</c>
///     attribute prevents concurrent directory creation and deletion from interfering with
///     shared working-directory state.
/// </remarks>
[Collection("Sequential")]
public class UtilitiesTests
{
    /// <summary>
    ///     Validates that <see cref="TemporaryDirectory.GetFilePath"/> with a valid relative
    ///     file name returns a path that lives inside <see cref="TemporaryDirectory.DirectoryPath"/>.
    /// </summary>
    [Fact]
    public void Utilities_PathResolution_ValidRelativePath_ReturnsPathWithinDirectory()
    {
        // Arrange: create a temporary directory on the real filesystem
        using var tmpDir = new TemporaryDirectory();

        // Act: resolve a simple relative file name within the temporary directory
        var filePath = tmpDir.GetFilePath("output.txt");

        // Assert: the returned path must be rooted and contained within the temporary directory
        Assert.True(
            filePath.StartsWith(tmpDir.DirectoryPath, StringComparison.Ordinal),
            $"Expected path to start with '{tmpDir.DirectoryPath}' but was '{filePath}'");
        Assert.True(Path.IsPathFullyQualified(filePath), "GetFilePath should return a fully-qualified path");
    }

    /// <summary>
    ///     Validates that <see cref="TemporaryDirectory.GetFilePath"/> with a nested relative
    ///     path creates the required intermediate directories on the filesystem.
    /// </summary>
    [Fact]
    public void Utilities_PathResolution_NestedRelativePath_CreatesIntermediateDirectories()
    {
        // Arrange: create a temporary directory on the real filesystem
        using var tmpDir = new TemporaryDirectory();

        // Act: resolve a nested path that requires two intermediate directories to be created
        var filePath = tmpDir.GetFilePath("sub/dir/output.txt");

        // Assert: the intermediate directory must exist so the caller can write the file immediately
        var intermediateDir = Path.GetDirectoryName(filePath)!;
        Assert.True(
            Directory.Exists(intermediateDir),
            $"Expected intermediate directory '{intermediateDir}' to be created by GetFilePath");
        Assert.True(
            filePath.StartsWith(tmpDir.DirectoryPath, StringComparison.Ordinal),
            "Nested path must remain within the temporary directory");
    }

    /// <summary>
    ///     Validates that <see cref="TemporaryDirectory.GetFilePath"/> throws
    ///     <see cref="ArgumentException"/> when the relative path contains a traversal
    ///     component (<c>..</c>) that would escape the temporary directory.
    /// </summary>
    [Fact]
    public void Utilities_PathTraversal_TraversalAttempt_ThrowsArgumentException()
    {
        // Arrange: create a temporary directory on the real filesystem
        using var tmpDir = new TemporaryDirectory();

        // Act / Assert: a traversal path must be rejected before any filesystem access
        Assert.Throws<ArgumentException>(() => tmpDir.GetFilePath("../escape.txt"));
    }

    /// <summary>
    ///     Validates that the full lifecycle of a <see cref="TemporaryDirectory"/> — creation,
    ///     file-path resolution, and disposal — leaves no directory artifacts on the filesystem.
    /// </summary>
    [Fact]
    public void Utilities_DirectoryLifecycle_CreateAndDispose_DirectoryCreatedThenDeleted()
    {
        // Arrange: create the temporary directory and capture its path before disposal
        string capturedPath;
        using (var tmpDir = new TemporaryDirectory())
        {
            capturedPath = tmpDir.DirectoryPath;

            // Act: write a file into the directory via GetFilePath to prove the directory is usable
            var filePath = tmpDir.GetFilePath("sentinel.txt");
            File.WriteAllText(filePath, "lifecycle test");

            Assert.True(Directory.Exists(capturedPath), "Directory must exist before disposal");
            Assert.True(File.Exists(filePath), "File written via GetFilePath must be accessible");
        }

        // Assert: the directory (and its contents) must be gone after the using block closes
        Assert.False(
            Directory.Exists(capturedPath),
            $"Directory '{capturedPath}' should have been deleted by Dispose");
    }

    /// <summary>
    ///     Validates that <see cref="PathHelpers.SafePathCombine"/> called directly with a real
    ///     base path and a safe relative component returns a path that remains within the base.
    /// </summary>
    [Fact]
    public void Utilities_PathSafety_SafePathCombine_StaysWithinBase()
    {
        // Arrange: use a real temporary directory as the base so the path is fully resolved
        using var tmpDir = new TemporaryDirectory();
        var basePath = tmpDir.DirectoryPath;
        const string relative = "safe/child/file.txt";

        // Act: combine the base with the safe relative path
        var combined = PathHelpers.SafePathCombine(basePath, relative);

        // Assert: the combined path must be within the base directory
        var absoluteBase = Path.GetFullPath(basePath);
        var absoluteCombined = Path.GetFullPath(combined);
        var check = Path.GetRelativePath(absoluteBase, absoluteCombined);

        Assert.False(
            Path.IsPathRooted(check) ||
            check.Equals("..", StringComparison.Ordinal) ||
            check.StartsWith(".." + Path.DirectorySeparatorChar, StringComparison.Ordinal) ||
            check.StartsWith(".." + Path.AltDirectorySeparatorChar, StringComparison.Ordinal),
            $"Combined path '{combined}' must stay within base '{basePath}'");
    }
}
