# NuGet Cache Tool

[![GitHub forks][badge-forks]][link-forks]
[![GitHub stars][badge-stars]][link-stars]
[![GitHub contributors][badge-contributors]][link-contributors]
[![License][badge-license]][link-license]
[![Build][badge-build]][link-build]
[![Quality Gate][badge-quality]][link-quality]
[![Security][badge-security]][link-security]
[![NuGet][badge-nuget]][link-nuget]

DEMA Consulting NuGet cache management tool for .NET developers.

## Features

This tool provides:

- **Standardized Command-Line Interface**: Context class handling common arguments
  (`--version`, `--help`, `--silent`, `--validate`, `--results`, `--log`)
- **Self-Validation**: Built-in validation tests with TRX/JUnit output
- **Multi-Platform Support**: Builds and runs on Windows and Linux
- **Multi-Runtime Support**: Targets .NET 8, 9, and 10
- **Comprehensive CI/CD**: GitHub Actions workflows with quality checks, builds, and
  integration tests
- **Documentation Generation**: Automated build notes, user guide, code quality reports,
  requirements, justifications, and trace matrix

## Installation

Install the tool globally using the .NET CLI:

```bash
dotnet tool install -g DemaConsulting.NuGet.CacheTool
```

## Usage

```bash
# Display version
nuget-cache --version

# Display help
nuget-cache --help

# Run self-validation
nuget-cache --validate

# Save validation results
nuget-cache --validate --results results.trx

# Silent mode with logging
nuget-cache --silent --log output.log
```

## Command-Line Options

| Option               | Description                                                  |
| -------------------- | ------------------------------------------------------------ |
| `-v`, `--version`    | Display version information                                  |
| `-?`, `-h`, `--help` | Display help message                                         |
| `--silent`           | Suppress console output                                      |
| `--validate`         | Run self-validation                                          |
| `--results <file>`   | Write validation results to file (TRX or JUnit format)       |
| `--log <file>`       | Write output to log file                                     |

## Documentation

Generated documentation includes:

- **Build Notes**: Release information and changes
- **User Guide**: Comprehensive usage documentation
- **Code Quality Report**: CodeQL and SonarCloud analysis results
- **Requirements**: Functional and non-functional requirements
- **Requirements Justifications**: Detailed requirement rationale
- **Trace Matrix**: Requirements to test traceability

## License

Copyright (c) DEMA Consulting. Licensed under the MIT License. See [LICENSE][link-license] for details.

By contributing to this project, you agree that your contributions will be licensed under the MIT License.

<!-- Badge References -->
[badge-forks]: https://img.shields.io/github/forks/demaconsulting/NuGetCacheTool?style=plastic
[badge-stars]: https://img.shields.io/github/stars/demaconsulting/NuGetCacheTool?style=plastic
[badge-contributors]: https://img.shields.io/github/contributors/demaconsulting/NuGetCacheTool?style=plastic
[badge-license]: https://img.shields.io/github/license/demaconsulting/NuGetCacheTool?style=plastic
[badge-build]: https://img.shields.io/github/actions/workflow/status/demaconsulting/NuGetCacheTool/build_on_push.yaml?style=plastic
[badge-quality]: https://sonarcloud.io/api/project_badges/measure?project=demaconsulting_NuGetCacheTool&metric=alert_status
[badge-security]: https://sonarcloud.io/api/project_badges/measure?project=demaconsulting_NuGetCacheTool&metric=security_rating
[badge-nuget]: https://img.shields.io/nuget/v/DemaConsulting.NuGet.CacheTool?style=plastic

<!-- Link References -->
[link-forks]: https://github.com/demaconsulting/NuGetCacheTool/network/members
[link-stars]: https://github.com/demaconsulting/NuGetCacheTool/stargazers
[link-contributors]: https://github.com/demaconsulting/NuGetCacheTool/graphs/contributors
[link-license]: https://github.com/demaconsulting/NuGetCacheTool/blob/main/LICENSE
[link-build]: https://github.com/demaconsulting/NuGetCacheTool/actions/workflows/build_on_push.yaml
[link-quality]: https://sonarcloud.io/dashboard?id=demaconsulting_NuGetCacheTool
[link-security]: https://sonarcloud.io/dashboard?id=demaconsulting_NuGetCacheTool
[link-nuget]: https://www.nuget.org/packages/DemaConsulting.NuGet.CacheTool
