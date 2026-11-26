---
title: Caching packages
summary: Cache packages by id and version, and read the folder that each run prints.
aliases:
  - cache
  - packages
  - package
tags:
  - reference
order: 3
see-also:
  - nuget-config
  - troubleshooting
examples:
  - name: one
    kind: snippet
    language: shell
    save-as: cache-one.sh
    description: Cache one package from nuget.org.
    content: |
      {{tool}} Newtonsoft.Json:13.0.3
  - name: many
    kind: snippet
    language: shell
    save-as: cache-many.sh
    description: Cache two packages in one run.
    content: |
      {{tool}} Newtonsoft.Json:13.0.3 NuGet.Common:6.12.1
  - name: no-version
    kind: command
    language: shell
    description: A package without a version is an unknown argument. Exits 1.
    content: '{{tool}} Newtonsoft.Json'
    exit: 1
    expect: "Error: Unsupported argument 'Newtonsoft.Json'"
  - name: latest
    kind: command
    language: shell
    description: Only exact versions are accepted. Exits 1.
    content: '{{tool}} Newtonsoft.Json:latest'
    exit: 1
    expect: 'is not a valid version string'
---
Give each package as one `<id>:<version>` argument. The argument is split
at the first `:`, and both parts must be present. The command that
[[#one]] prints and the command that
[[#many]] prints need nuget.org, or the feeds of your
`nuget.config`; see [[nuget-config]].

**Dependencies.** Only the packages named on the command line are
cached. Their dependencies are not resolved and not downloaded. List
every package that you need, each with its exact version, as in
[[#many]].

**Versions.** The version must be exact, such as `13.0.3` or
`2.0.0-beta.1`. Ranges, wildcards such as `13.*`, and `latest` are not
accepted ([[#latest]]). A short version is normalized: `1.0` is cached
as `1.0.0`.

**Output.** After the banner, the tool prints one line per package: the
folder of the package in the global packages folder, as
`<cache>/<lower-case id>/<version>`. A build script can read the files
of the package from there, for example a tool in its `tools` folder.

**Already cached.** A package that is already in the global packages
folder is not downloaded again, and no feed is contacted for it. Running
the same command twice is safe and fast.

**Failures.** A package that cannot be downloaded prints
`Error caching <id>:<version>: ...`; the tool goes on with the other
packages and exits 1. An invalid version stops the run at once, and like
an argument error it is printed even with `--silent` and is not logged.
A package argument without a version is an unknown argument
([[#no-version]]). See [[troubleshooting]] for the messages.
