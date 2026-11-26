---
title: Getting started
summary: Install the tool, cache a first package, and find the other topics.
aliases:
  - start
  - quickstart
  - examples
  - install
tags:
  - tutorial
order: 1
see-also:
  - caching
  - nuget-config
  - command-line
examples:
  - name: install-local
    kind: snippet
    language: shell
    save-as: install-local.sh
    description: Add the tool to the tool manifest of a repository.
    content: |
      dotnet new tool-manifest
      dotnet tool install --local DemaConsulting.NuGet.CacheTool
  - name: run-in-ci
    kind: snippet
    language: shell
    save-as: run-in-ci.sh
    description: Restore the manifest tools in CI, then run the tool.
    content: |
      dotnet tool restore
      dotnet {{tool}} --version
  - name: version
    kind: command
    language: shell
    description: Print the version and exit.
    content: '{{tool}} --version'
  - name: validate
    kind: snippet
    language: shell
    save-as: validate.sh
    description: Run the built-in self-tests; needs a NuGet feed.
    content: |
      {{tool}} --validate
  - name: first-cache
    kind: snippet
    language: shell
    save-as: first-cache.sh
    description: Cache one package from nuget.org and print its folder.
    content: |
      {{tool}} Newtonsoft.Json:13.0.3
  - name: topic
    kind: command
    language: shell
    description: Print a help topic, here the list of options.
    content: '{{tool}} help command-line'
    expect: '--results'
---
{{tool}} {{version}} is a .NET tool.
It writes packages into the global packages folder and only the files that
options name, apart from a `tmp-...` folder that `--validate` creates in the
current folder and deletes.

1. **Install it.** The tool needs the .NET 10 SDK or later. In CI, set it
   up first when the runner has an older .NET, for example with
   `actions/setup-dotnet` and `dotnet-version: 10.x`.
   For a repository or CI, install it as a local tool. Run the commands
   that [[#install-local]] prints once, without
   `dotnet new tool-manifest` when `.config/dotnet-tools.json` exists, and
   commit that file. In CI, run `dotnet tool restore` first, then run the
   tool as `dotnet {{tool}}`, as in the commands that
   [[#run-in-ci]] prints. For one machine, install it globally and
   run it as `{{tool}}`:

   ```bash
   dotnet tool install -g DemaConsulting.NuGet.CacheTool
   ```

2. **Check it.** The command that [[#version]] prints shows the version.
   The command from [[#validate]] runs the built-in self-tests and exits 0
   when all pass; it downloads a small package, so it needs nuget.org or
   another feed that has it. See [[self-validation]].
3. **Cache a package.** Give each package as `<id>:<version>`. The
   command from [[#first-cache]] downloads one package from nuget.org,
   unless it is already cached, and prints its folder; see [[caching]].
   The feeds, credentials, and cache folder come from your NuGet
   configuration; see [[nuget-config]], which also has an example that
   works offline with a local feed folder.

Print any help topic with `{{tool}} help <topic>`, as in the command that
[[#topic]] prints. Add `--search <words>` to find a topic, and `--json` to
get any help answer as JSON.
