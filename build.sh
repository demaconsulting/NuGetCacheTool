#!/usr/bin/env bash
# Build and test NuGet Cache Tool

set -e  # Exit on error

echo "🔧 Building NuGet Cache Tool..."
dotnet build --configuration Release

echo "🧪 Running unit tests..."
dotnet test --configuration Release

echo "✅ Running self-validation..."
dotnet run --project src/DemaConsulting.NuGet.CacheTool --configuration Release --framework net10.0 --no-build -- --validate

echo "✨ Build, tests, and validation completed successfully!"
