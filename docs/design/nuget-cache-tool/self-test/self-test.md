# SelfTest Subsystem Design

## Purpose

The SelfTest subsystem provides self-validation and safe file-system utilities for the
NuGet Cache Tool. It verifies that the tool operates correctly in the deployment environment
and provides path safety guarantees used during validation test execution.

## Responsibilities

- Execute self-validation tests that invoke the tool in-process and observe its outputs
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

## Error Handling

| Scenario | Behavior |
| -------- | -------- |
| `relative` argument is null | `SafePathCombine` throws `ArgumentNullException` |
| `relative` argument is empty | `SafePathCombine` returns the base path |
| `relative` argument is an absolute path | `SafePathCombine` throws `ArgumentException` |
| `relative` argument contains `..` path traversal | `SafePathCombine` throws `ArgumentException` |
| A self-validation test fails | `Validation.Run` records the failure via `context.WriteError` |
| | and continues running remaining tests |
| `--results` file extension is not `.trx` or `.xml` | `Validation.Run` records an error via `context.WriteError` |
