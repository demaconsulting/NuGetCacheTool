# SelfTest Subsystem Verification

This document describes the subsystem-level verification design for the `SelfTest` subsystem. It
defines the overall verification strategy and requirement coverage for the SelfTest subsystem.

## Verification Approach

The SelfTest subsystem is verified with subsystem integration tests defined in `SelfTestTests.cs`.
These tests exercise the public API of `Validation` and `PathHelpers` directly, verifying
self-validation execution, results file generation, and path combination safety.

## Dependencies

| Dependency     | Usage in Tests                                            |
|----------------|-----------------------------------------------------------|
| `Validation`   | Exercised directly; validates self-validation behavior.   |
| `PathHelpers`  | Exercised directly; validates path combination safety.    |
| `Context`      | Created from controlled argument arrays for each test.    |

## Test Scenarios

### SelfTest_Validation_ExecutesSelfValidationTests

**Scenario**: `Validation.Run` is called with `["--validate"]` context; stdout is captured.

**Expected**: Output contains "Total Tests:" and "Passed:".

**Requirement coverage**: `NuGetCache-SelfTest-Validation`.

### SelfTest_Validation_ReportsPassFail

**Scenario**: `Validation.Run` is called with `["--validate"]` context.

**Expected**: Exit code is 0 when all self-validation tests pass.

**Requirement coverage**: `NuGetCache-SelfTest-Validation`.

### SelfTest_ResultsFile_GeneratesTrxFile

**Scenario**: `Validation.Run` is called with `["--validate", "--silent", "--results", trxFile]`.

**Expected**: Exit code 0; TRX file is created containing `<TestRun` and `</TestRun>`.

**Requirement coverage**: `NuGetCache-SelfTest-ResultsFile`.

### SelfTest_ResultsFile_GeneratesJUnitFile

**Scenario**: `Validation.Run` is called with `["--validate", "--silent", "--results", xmlFile]`.

**Expected**: Exit code 0; XML file is created containing `<testsuites`, `<testsuite`, and
`<testcase`.

**Requirement coverage**: `NuGetCache-SelfTest-ResultsFile`.

### SelfTest_SafePathCombine_AcceptsValidPaths

**Scenario**: `PathHelpers.SafePathCombine` is called with a valid base and relative path.

**Expected**: Returns the combined path equal to `Path.Combine(basePath, relativePath)`.

**Requirement coverage**: `NuGetCache-SelfTest-SafePathCombine`.

### SelfTest_SafePathCombine_RejectsPathTraversal

**Scenario**: `PathHelpers.SafePathCombine` is called with a traversal attempt `"../traversal"`.

**Expected**: `ArgumentException` is thrown.

**Requirement coverage**: `NuGetCache-SelfTest-SafePathCombine`.

### SelfTest_SafePathCombine_RejectsAbsolutePath

**Scenario**: `PathHelpers.SafePathCombine` is called with an absolute path as the relative argument.

**Expected**: `ArgumentException` is thrown.

**Requirement coverage**: `NuGetCache-SelfTest-SafePathCombine`.

## Requirements Coverage

- **`NuGetCache-SelfTest-Validation`**: SelfTest_Validation_ExecutesSelfValidationTests,
  SelfTest_Validation_ReportsPassFail
- **`NuGetCache-SelfTest-ResultsFile`**: SelfTest_ResultsFile_GeneratesTrxFile,
  SelfTest_ResultsFile_GeneratesJUnitFile
- **`NuGetCache-SelfTest-SafePathCombine`**: SelfTest_SafePathCombine_AcceptsValidPaths,
  SelfTest_SafePathCombine_RejectsPathTraversal, SelfTest_SafePathCombine_RejectsAbsolutePath
