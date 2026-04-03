# CLI Subsystem Design

## Purpose

The CLI subsystem provides command-line interface functionality for the NuGet Cache Tool.
It handles argument parsing and output management, translating raw command-line arguments
into structured options and commands that are executed by the top-level `Program` unit.

## Responsibilities

- Parse and validate all command-line arguments
- Manage output channels (console, log file, silent mode)
- Track error state and determine the process exit code
- Provide structured command/context information for top-level operations such as version
  display, help display, package caching, and self-validation

## Units

| Unit | Class | Description |
| ---- | ----- | ----------- |
| Context | `Context.cs` | Command-line argument parsing and output management |

## Interactions

| Dependency | Direction | Description |
| ---------- | --------- | ----------- |
| `Program` | Top-level unit (entry point) | Creates `Context` and dispatches to CLI subsystem |
| `SelfTest` subsystem | Downstream | CLI passes `Context` to `Validation.Run` |
