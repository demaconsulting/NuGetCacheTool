## SelfTest Subsystem Design

![SelfTest Structure](SelfTestView.svg)

### Overview

The SelfTest subsystem provides self-validation functionality for the NuGet Cache Tool. It
verifies that the tool operates correctly in the deployment environment.

**Responsibilities**:

- Execute self-validation tests that invoke the tool in-process and observe its outputs
- Report validation results in TRX or JUnit format for CI/CD integration

**Units**:

| Unit | Class | Description |
| ---- | ----- | ----------- |
| Validation | `Validation.cs` | Self-validation test execution and results reporting |

### Interfaces

The SelfTest subsystem exposes the following entry point:

| Member | Description |
| ------ | ----------- |
| `Validation.Run(Context context)` | Executes all self-validation tests and optionally writes results to the file specified by `context.ResultsFile`; writes the `Total Tests`/`Passed` summary lines via `context.WriteLine`, and the `Failed` summary line via `context.WriteError` when the failure count is greater than zero |

The subsystem consumes the following interfaces:

| Dependency | Direction | Description |
| ---------- | --------- | ----------- |
| `Context` (CLI subsystem) | Upstream | Provides `ResultsFile` path and output methods |
| `Program` (top-level unit) | Downstream | `RunValidationTest` calls `Program.Run` in-process to exercise the full program path for each test |
| `DemaConsulting.TestResults` (OTS) | Downstream | `TrxSerializer`, `JUnitSerializer` for result output |
| `TemporaryDirectory` (Utilities subsystem) | Downstream | `Validation` uses `TemporaryDirectory` for isolated test directories |

### Design

#### Unit Collaboration

`Validation.RunValidationTest` creates a `TemporaryDirectory` (Utilities subsystem) for
each self-validation test and obtains isolated log file paths by calling
`tempDir.GetFilePath(logFileName)`. `TemporaryDirectory.GetFilePath` internally enforces
the directory boundary via `PathHelpers.SafePathCombine` (both in the Utilities subsystem),
ensuring that log paths remain within the designated temporary directory and cannot escape
to arbitrary filesystem locations through user-influenced naming.

#### Error Handling

| Scenario | Behavior |
| -------- | -------- |
| A self-validation test fails | `Validation.Run` records the failure and continues with remaining tests |
| `--results` file extension is not `.trx` or `.xml` | `Validation.Run` records an error via `context.WriteError` |
