---
title: Self-validation
summary: Prove that the installed tool works, and save the evidence as test results.
aliases:
  - validate
  - validation
tags:
  - reference
order: 5
see-also:
  - command-line
  - nuget-config
examples:
  - name: run
    kind: snippet
    language: shell
    save-as: validate.sh
    description: Run the self-tests. Exits 0 when all pass.
    content: |
      {{tool}} --validate
  - name: trx
    kind: snippet
    language: shell
    save-as: validate-trx.sh
    description: Also write the results as a TRX file.
    content: |
      {{tool}} --validate --results validation.trx
  - name: junit
    kind: snippet
    language: shell
    save-as: validate-junit.sh
    description: Write the results as JUnit XML instead.
    content: |
      {{tool}} --validate --results validation.xml
  - name: missing-file
    kind: command
    language: shell
    description: --results needs a file name. Exits 1.
    content: '{{tool}} --validate --results'
    exit: 1
    expect: 'Error: --results requires a results filename argument'
---
`--validate` runs a short set of tests of the tool itself, so a regulated
project can show that the installed version works on its build machine.
The tests work in a `tmp-...` folder that they create in the current
folder and delete.

**Network.** The test `NuGetCache_CachePackage` caches the package
`DemaConsulting.NuGet.Caching` version 0.1.0 with the NuGet settings of
the current folder. It needs a feed that has that package, such as
nuget.org, unless the package is already in the global packages folder;
see [[nuget-config]]. That is why the examples here are not run offline.

**Report.** The output is Markdown. It starts with a heading and a table of
the tool version, machine name, operating system, .NET runtime, and time
stamp. Each test prints one line with its display name, such as
`✓ Version Display Test - PASSED` or
`✗ Cache Package Test - FAILED: <reason>`, and the report ends with
`Total Tests`, `Passed`, and `Failed` lines. The exit code is 0 only when
every test passes.

**Results files.** `--results <file>` also writes the outcome; the
extension chooses the format. `.trx` writes a Visual Studio TRX file and
`.xml` writes JUnit XML, both readable by CI test-report tasks. Any other
extension is an error after the tests ran. The folder of the file must
exist. Without `--validate`, `--results` is ignored.

**Evidence.** The `--results` file records each test under a stable name,
`NuGetCache_VersionDisplay`, `NuGetCache_HelpDisplay`, and
`NuGetCache_CachePackage`, so a requirements file can list them as the
tests that prove the tool works in its environment.
