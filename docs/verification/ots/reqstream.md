## ReqStream Verification

This document provides the verification evidence for the `ReqStream` OTS software item.

### Required Functionality

DemaConsulting.ReqStream processes requirements.yaml and the TRX test-result files to produce a
requirements report, justifications document, and traceability matrix. When run with `--enforce`, it
exits with a non-zero code if any requirement lacks test evidence, making unproven requirements a
build-breaking condition. A successful pipeline run with `--enforce` proves all requirements are
covered and that ReqStream is functioning.

### Verification Approach

ReqStream is verified by two complementary layers of evidence. First, the CI pipeline runs
`reqstream --validate --results artifacts/reqstream-self-validation.trx`, which exercises
ReqStream's built-in self-validation suite against internal test requirements and records
results.

Second, the pipeline invokes `reqstream --enforce` with `requirements.yaml` and all TRX
test-evidence files accumulated during the build. ReqStream generates `requirements.md`,
`justifications.md`, and `trace_matrix.md`. If ReqStream failed or produced no output, the
subsequent Pandoc step would fail, breaking the CI build. Additionally, `--enforce` exits
non-zero if any requirement lacks test evidence, which would also fail the build. A passing
CI build proves ReqStream correctly processed the project's real requirements and found
complete test coverage.

### Test Scenarios

#### ReqStream_EnforcementMode

**Scenario**: ReqStream self-validation exercises enforcement mode where all requirements have test
coverage.

**Expected**: Exits 0; would exit non-zero if any requirement lacked coverage.

**Requirement coverage**: `NuGetCache-OTS-ReqStream`.

### Requirements Coverage

- **`NuGetCache-OTS-ReqStream`**: ReqStream_EnforcementMode
