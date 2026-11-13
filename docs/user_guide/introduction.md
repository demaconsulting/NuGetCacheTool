# Introduction

This guide describes how to install, configure, and use NuGet Cache Tool.

## Purpose

NuGet Cache Tool is a general-purpose .NET global tool for CI/CD pipelines that ensures
NuGet packages are pre-cached in the global packages folder before build steps that depend
on them.

The workflow below shows how NuGet Cache Tool is used: install the tool and choose the `package:version`
arguments to cache, run the `nuget-cache` command on a build machine or in a CI job, and use the cached
packages, their printed cache paths, and the exit code in later build steps, offline builds, and CI gates.

![NuGet Cache Tool workflow: install the tool and choose package:version arguments and NuGet feeds, run nuget-cache
with one or more packages and the optional log and silent options, get the packages cached in the global packages
folder, one cache path per package, errors on stderr, and a pass/fail exit code, then use them in later build steps,
offline builds, scripts and pipelines, or as a CI gate](workflow.svg)

NuGet Cache Tool requires .NET 10 or 11 and runs on Windows, Linux, and macOS.

## Scope

This user guide covers:

- NuGet package caching to the global packages folder
- Installation instructions
- Usage examples for common tasks
- Command-line options reference
- Practical examples for various scenarios

## References

- [REF-1] NuGet Cache Tool Releases, available at the demaconsulting/NuGetCacheTool GitHub repository releases page.

# Installation

Install the tool globally using the .NET CLI:

```bash
dotnet tool install -g DemaConsulting.NuGet.CacheTool
```

# Usage

## Display Version

Display the tool version:

```bash
nuget-cache --version
```

## Display Help

Display usage information:

```bash
nuget-cache --help
```

## Cache Packages

Cache a NuGet package to the global packages folder:

```bash
nuget-cache DemaConsulting.NuGet.Caching:0.1.0
```

Cache multiple packages in a single command:

```bash
nuget-cache Newtonsoft.Json:13.0.3 NuGet.Common:6.12.1
```

The tool outputs the path to each cached package to stdout. Error messages are written to stderr.

## Run Self-Validation

Run self-validation tests:

```bash
nuget-cache --validate
```

Save validation results to a file:

```bash
nuget-cache --validate --results results.trx
```

The results file format is determined by the file extension: `.trx` for TRX (MSTest) format,
or `.xml` for JUnit format.

## Silent Mode

Suppress console output:

```bash
nuget-cache --silent
```

## Logging

Write output to a log file:

```bash
nuget-cache --log output.log
```

# Command-Line Options

The following command-line options are supported:

| Option                    | Description                                                  |
| ------------------------- | ------------------------------------------------------------ |
| `-v`, `--version`         | Display version information                                  |
| `-?`, `-h`, `--help`      | Display help message                                         |
| `--silent`                | Suppress console output                                      |
| `--validate`              | Run self-validation                                          |
| `--results <file>`        | Write validation results to file (.trx or .xml)              |
| `--log <file>`            | Write output to log file                                     |
| `[package]:[version]`     | Cache the specified NuGet package                            |

# Examples

## Example 1: Basic Usage

```bash
nuget-cache DemaConsulting.NuGet.Caching:0.1.0
```

## Example 2: Self-Validation with Results

```bash
nuget-cache --validate --results validation-results.trx
```

## Example 3: Silent Mode with Logging

```bash
nuget-cache --silent --log tool-output.log
```

## Example 4: Cache Multiple Packages

Cache several packages in one command and capture their paths:

```bash
nuget-cache Newtonsoft.Json:13.0.3 NuGet.Common:6.12.1 NuGet.Protocol:6.12.1
```
