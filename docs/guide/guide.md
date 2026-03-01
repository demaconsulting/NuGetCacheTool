# Introduction

## Purpose

NuGet Cache Tool is a NuGet cache management tool for DEMA Consulting .NET developers.

## Scope

This user guide covers:

- NuGet package caching to the global packages folder
- Installation instructions
- Usage examples for common tasks
- Command-line options reference
- Practical examples for various scenarios

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

The tool outputs the path to each cached package on stdout.

## Run Self-Validation

Run self-validation tests:

```bash
nuget-cache --validate
```

Save validation results to a file:

```bash
nuget-cache --validate --results results.trx
```

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
| `--results <file>`        | Write validation results to file (TRX or JUnit format)       |
| `--log <file>`            | Write output to log file                                     |
| `[package]:[version]`     | Cache the specified NuGet package                            |

# Examples

## Example 1: Basic Usage

```bash
nuget-cache
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
