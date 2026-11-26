---
title: Errors and exit codes
summary: What the exit codes mean, and how to fix the common error messages.
aliases:
  - errors
  - exit-codes
tags:
  - troubleshooting
order: 6
see-also:
  - command-line
  - caching
  - nuget-config
examples:
  - name: unknown-option
    kind: command
    language: shell
    description: An unknown argument. Exits 1.
    content: '{{tool}} --unknown'
    exit: 1
    expect: "Error: Unsupported argument '--unknown'"
  - name: silent-error
    kind: command
    language: shell
    description: An invalid version is printed even with --silent. Exits 1.
    content: '{{tool}} --silent Newtonsoft.Json:latest'
    exit: 1
    expect: 'is not a valid version string'
  - name: no-log-file
    kind: command
    language: shell
    description: --log without a file name. Exits 1.
    content: '{{tool}} --log'
    exit: 1
    expect: 'Error: --log requires a filename argument'
  - name: unknown-topic
    kind: command
    language: shell
    description: A help topic that does not exist. Exits 2.
    content: '{{tool}} help no-such-topic'
    exit: 2
    expect: "Unknown help card 'no-such-topic'"
---
**Exit codes.**

| Code | Meaning |
| --- | --- |
| 0 | Success. |
| 1 | An error, a package not cached, or a failed self-test. |
| 2 | A wrong `help` argument or an unknown help topic. |

Errors are printed to standard error and set the exit code. Errors found
during a run, such as a package that is not found, are hidden by
`--silent` and kept in the `--log` file. Errors in the arguments, an
invalid version, and a log file that cannot be opened stop the tool at
once: they are always printed and never reach the log file, as the
command that [[#silent-error]] prints shows.

**Common messages.**

- `Unsupported argument`: check the spelling against [[command-line]].
  A package needs `:<version>`, and `help` must come first.
  The command that [[#unknown-option]] prints shows the error.
- `requires a filename argument` and similar: the option before it needs
  a value. The command that [[#no-log-file]] prints shows the error.
- `Failed to open log file`: the folder of `--log` does not exist or the
  file is locked.
- `is not a valid version string`: use an exact version; see [[caching]].
- `was not found in any configured NuGet source`: no enabled feed has
  that id and version. Check the spelling and the feeds; see
  [[nuget-config]]. Lines that start with `-` name feeds that failed.
- `HTTP 401 (Unauthorized)` or `HTTP 403 (Forbidden)`: the feed needs
  credentials, or rejected them. Check `packageSourceCredentials` or the
  credential provider; see [[nuget-config]].
- `Unsupported results file format`: `--results` takes `.trx` or `.xml`.
- `Failed to write results file`: the folder of `--results` does not
  exist or the file is locked.
- `Unknown help card`: the topic does not exist. The message lists the
  topics. The command that [[#unknown-topic]] prints shows the error.
