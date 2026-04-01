# CLI Subsystem Design

## Purpose

The CLI subsystem provides command-line interface functionality for the NuGet Cache Tool.
It handles argument parsing, output management, and the main entry point, translating
raw command-line arguments into structured requests for execution by the core subsystems.

## Responsibilities

- Parse and validate all command-line arguments
- Manage output channels (console, log file, silent mode)
- Track error state and determine the process exit code
- Orchestrate the top-level execution flow: version display, help display, package caching,
  and self-validation dispatch

## Units

| Unit | Class | Description |
| ---- | ----- | ----------- |
| Context | `Context.cs` | Command-line argument parsing and output management |

## Interactions

| Dependency | Direction | Description |
| ---------- | --------- | ----------- |
| `Program` | Top-level unit (entry point) | Creates `Context` and dispatches to CLI subsystem |
| `SelfTest` subsystem | Downstream | CLI passes `Context` to `Validation.Run` |
