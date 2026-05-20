## SelfTest Subsystem Design

### Overview

The SelfTest subsystem provides self-validation and safe file-system utilities for the
NuGet Cache Tool. It verifies that the tool operates correctly in the deployment environment
and provides path safety guarantees used during validation test execution.

**Responsibilities**:

- Execute self-validation tests that invoke the tool in-process and observe its outputs
- Report validation results in TRX or JUnit format for CI/CD integration
- Provide safe path combination utilities that prevent path traversal attacks

**Units**:

| Unit | Class | Description |
| ---- | ----- | ----------- |
| Validation | `Validation.cs` | Self-validation test execution and results reporting |
| PathHelpers | `PathHelpers.cs` | Safe path combination utilities (prevents path traversal) |

### Interfaces

The SelfTest subsystem exposes the following entry point:

| Member | Description |
| ------ | ----------- |
| `Validation.Run(Context context)` | Executes all self-validation tests and optionally writes results to the file specified by `context.ResultsFile`; writes pass/fail summary via `context.WriteLine` |

The subsystem consumes the following interfaces:

| Dependency | Direction | Description |
| ---------- | --------- | ----------- |
| `Context` (CLI subsystem) | Upstream | Provides `ResultsFile` path and output methods |
| `DemaConsulting.TestResults` (OTS) | Downstream | `TrxSerializer`, `JUnitSerializer` for result output |
| `PathHelpers` | Internal | `Validation` uses `SafePathCombine` to construct log file paths |

### Design

#### Unit Collaboration

`Validation.RunValidationTest` uses `PathHelpers.SafePathCombine` to construct isolated
temporary log file paths within a `TemporaryDirectory`. For each self-validation test,
a unique log file name is combined with the temporary directory base path via
`SafePathCombine`, ensuring that log paths remain within the designated temporary
directory and cannot escape to arbitrary filesystem locations through user-influenced
naming.

#### Error Handling

| Scenario | Behavior |
| -------- | -------- |
| `relative` argument is null | `SafePathCombine` throws `ArgumentNullException` |
| `relative` argument is empty | `SafePathCombine` returns the base path |
| `relative` argument is an absolute path | `SafePathCombine` throws `ArgumentException` |
| `relative` argument contains `..` path traversal | `SafePathCombine` throws `ArgumentException` |
| A self-validation test fails | `Validation.Run` records the failure and continues with remaining tests |
| `--results` file extension is not `.trx` or `.xml` | `Validation.Run` records an error via `context.WriteError` |
