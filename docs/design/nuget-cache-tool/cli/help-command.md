### HelpCommand Unit Design

![Cli Structure](CliView.svg)

#### Purpose

`HelpCommand` answers the `nuget-cache help` command from the help cards that are embedded in the
tool assembly. It is the only unit that references the HelpCards library.

#### Data Model

**ToolName**: `string` constant — `nuget-cache`, the name of the tool as the user types it. The
usage lines, the index preamble, and the `{{tool}}` token of the cards are derived from it.

**RootCardIds**: `string[]` — the ids of the root cards, which are the topics that
`nuget-cache help` lists. It is the one list of topics; a missing or extra root card fails the
catalog.

**PurposeTags**: `string[]` (private) — `tutorial`, `reference`, and `troubleshooting`; the closed
tag vocabulary. Every card carries one of these tags and no other tag.

**Cards**: `HelpSource` (private, static) — the cards embedded in the tool assembly. Creating the
source reads nothing.

**Default**: `HelpCommand` (static) — the instance used by the tool; it writes to the console.

**_cli**: `HelpCli` — the command-line helper of HelpCards, created from `CreateOptions`. It is
immutable and keeps no state between calls.

The help cards are the Markdown files under `src/DemaConsulting.NuGet.CacheTool/Help/`, embedded by the
HelpCards build targets. The rules for writing them are in the *Help Cards* section of `CONTRIBUTING.md`.

#### Key Methods

##### TryRun(string[] args, out int exitCode)

Runs the help command when the arguments start with the `help` verb.

- **Preconditions**: None
- **Postconditions**: returns `true` when the command was answered, with the exit code set; returns `false`,
  having written nothing, when the tool should continue with its own option parser
- **Algorithm**: delegates to `HelpCli.TryRun`, which builds the catalog only after the verb was recognized

##### Footer

Gets the one-line pointer to the help command that ends the usage text, without building the catalog.

##### CreateOptions(TextWriter? output, TextWriter? error)

Creates the options of the command-line helper: the tool name, the version from `Program.Version`, the catalog
configuration (the embedded cards, `PurposeTags`, and `RootCardIds`), the usage lines, the index preamble, and the
writers (`null` for the console).

- **Preconditions**: None
- **Postconditions**: `GetCatalogConfiguration` of the options gives the tests the composition that the tool ships

##### Create(TextWriter output, TextWriter error)

Creates a help command that writes to the given writers; used by tests.

#### Error Handling

`TryRun` does not throw for user input; the library writes errors to the error writer and sets the exit code (see
*Help command* in the system design). The validation test reports every card defect at build time, so a defective
catalog is not shipped.

The command is answered before the options are parsed, so no `Context` exists and the output and error providers of
HelpCards are not used: routing the answers through a `Context` would mean parsing the options a second time just
for help.

#### Dependencies

| Dependency | Usage |
| --- | --- |
| HelpCards (OTS) | `HelpCli`, `HelpCliOptions`, `HelpSource`, and `HelpCatalogBuilder` |
| `Program` | `Program.Version` supplies the version shown in the index and the `{{version}}` token |

#### Callers

`Program.Main` calls `HelpCommand.Default.TryRun` before `Context.Create`, and `Program.PrintHelp` ends the usage
text with `HelpCommand.Default.Footer`.
