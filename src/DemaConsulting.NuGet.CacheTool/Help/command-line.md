---
title: Command-line options
summary: Every option of the tool, and the order in which a run handles them.
aliases:
  - options
  - flags
  - usage
tags:
  - reference
order: 2
see-also:
  - caching
  - self-validation
  - troubleshooting
examples:
  - name: usage
    kind: command
    language: shell
    description: Print the short usage text. It ends with a pointer to this help.
    content: '{{tool}} --help'
    expect: "Run '{{tool}} help' for topics and examples."
  - name: silent-log
    kind: command
    language: shell
    description: Hide the console output and keep it in a log file.
    content: '{{tool}} --silent --log output.log --version'
  - name: unknown-option
    kind: command
    language: shell
    description: An unknown argument is an error, printed even with --silent. Exits 1.
    content: '{{tool}} --silent --unknown'
    exit: 1
    expect: "Error: Unsupported argument '--unknown'"
---
Options start with `--` (or a single letter after `-`) and may be given in
any order, mixed with the packages to cache.

| Option | Meaning |
| --- | --- |
| `-v`, `--version` | Print only the version. |
| `-?`, `-h`, `--help` | Print the short usage text. |
| `--silent` | Hide console output. Errors still set exit code 1. |
| `--log <file>` | Also write the output to a file. |
| `--validate` | Run the self-tests; see [[self-validation]]. |
| `--results <file>` | Write the self-test results as `.trx` or `.xml`. |
| `<id>:<version>` | Cache a package; repeat for more. See [[caching]]. |

**Order.** `--version`, when all arguments are valid, prints only the
version and caches nothing. Without it, the tool prints a banner, then
handles the first of `--help`, `--validate`, or caching the packages.
With no packages, it prints only the banner and exits 0.
The command that [[#silent-log]] prints writes the version to the log
file only.

**Errors.** An unknown argument (anything that is not an option and has
no `:<version>`), an option without its value, or a log file that cannot
be opened prints `Error: ...` and exits 1 ([[#unknown-option]]). These
errors are found before `--silent` and `--log` take effect, so they are
always printed and never logged.

**Help.** `help` is a command, not an option, and must be the first
argument. `--silent` and `--log` do not apply to it, and an unknown topic
exits 2.
