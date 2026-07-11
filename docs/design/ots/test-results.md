## DemaConsulting.TestResults Integration Design

### Purpose

`DemaConsulting.TestResults` provides standard test result serialization in TRX (MSTest)
and JUnit XML formats. Using this library allows the NuGet Cache Tool's self-validation
results to be consumed by CI/CD systems and requirements traceability tools (such as
ReqStream) that expect standard test result file formats.

### Features Used

| API | Signature | Usage |
| --- | --------- | ----- |
| `TrxSerializer` | `string Serialize(TestResults results)` | Called by `Validation.Run` when `context.ResultsFile` has a `.trx` extension; the returned XML content is then written to disk via `File.WriteAllText` |
| `JUnitSerializer` | `string Serialize(TestResults results)` | Called by `Validation.Run` when `context.ResultsFile` has a `.xml` extension; the returned XML content is then written to disk via `File.WriteAllText` |

### Integration Pattern

After all self-validation tests complete, `Validation.Run` checks `context.ResultsFile`.
If non-null, the file extension determines which serializer is used:

- `.trx` → `TrxSerializer.Serialize` returns MSTest-compatible TRX XML content, written to disk via `File.WriteAllText`
- `.xml` → `JUnitSerializer.Serialize` returns JUnit-compatible XML content, written to disk via `File.WriteAllText`
- any other extension → `context.WriteError` is called; no file is written

The serialized results are self-contained test result files that can be consumed
directly by CI/CD pipelines and by the ReqStream traceability tool.

The specific version of `DemaConsulting.TestResults` is declared in the project file.
Compatibility is verified by the integration test suite, which exercises both serializers
end-to-end and asserts the structure of the produced files.

See *OTS Integration Design* for the overall OTS integration strategy.
