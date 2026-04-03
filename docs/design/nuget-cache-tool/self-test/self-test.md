# SelfTest Subsystem Design

## Purpose

The SelfTest subsystem provides self-validation and safe file-system utilities for the
NuGet Cache Tool. It verifies that the tool operates correctly in the deployment environment
and provides path safety guarantees used during validation test execution.

## Responsibilities

- Execute self-validation tests that run the tool as a subprocess and observe its outputs
- Report validation results in TRX or JUnit format for CI/CD integration
- Provide safe path combination utilities that prevent path traversal attacks

## Units

| Unit | Class | Description |
| ---- | ----- | ----------- |
| Validation | `Validation.cs` | Self-validation test execution and results reporting |
| PathHelpers | `PathHelpers.cs` | Safe path combination utilities (prevents path traversal) |

## Interactions

| Dependency | Direction | Description |
| ---------- | --------- | ----------- |
| `Context` (CLI subsystem) | Upstream | Provides `ResultsFile` path and output methods |
| `DemaConsulting.TestResults` (OTS) | Downstream | `TrxSerializer`, `JUnitSerializer` for result output |
| `PathHelpers` | Internal | `Validation` uses `SafePathCombine` to construct log file paths |
