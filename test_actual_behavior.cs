using DemaConsulting.NuGet.CacheTool.SelfTest;
using System;

try {
    var result = PathHelpers.SafePathCombine("/home/user", "file..txt");
    Console.WriteLine($"SUCCESS: {result}");
} catch (Exception e) {
    Console.WriteLine($"REJECTED: {e.Message}");
}
