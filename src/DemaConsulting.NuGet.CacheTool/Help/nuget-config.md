---
title: Feeds, credentials, and the cache folder
summary: Choose the feeds, credentials, and cache folder with nuget.config.
aliases:
  - feeds
  - sources
  - credentials
  - config
  - cache-folder
  - offline
tags:
  - reference
order: 4
see-also:
  - caching
  - troubleshooting
examples:
  - name: offline
    kind: source
    language: xml
    save-as: nuget.config
    description: Use only a local feed folder, and cache into a local folder.
    content: |
      <?xml version="1.0" encoding="utf-8"?>
      <configuration>
        <packageSources>
          <clear />
          <add key="local-feed" value="feed" />
        </packageSources>
        <config>
          <add key="globalPackagesFolder" value="cache" />
        </config>
      </configuration>
  - name: from-feed
    kind: command
    language: shell
    description: Cache a package from the local feed folder.
    content: '{{tool}} Demo.Package:1.0.0'
    expect: 'demo.package'
  - name: missing
    kind: command
    language: shell
    description: A version that no feed has. Exits 1.
    content: '{{tool}} Demo.Package:2.0.0'
    exit: 1
    expect: 'was not found in any configured NuGet source'
  - name: credentials
    kind: snippet
    language: xml
    save-as: private-feed.config
    description: A private feed whose user and token come from the environment.
    content: |
      <?xml version="1.0" encoding="utf-8"?>
      <configuration>
        <packageSources>
          <add key="private" value="https://nuget.example.com/v3/index.json" />
        </packageSources>
        <packageSourceCredentials>
          <private>
            <add key="Username" value="%FEED_USER%" />
            <add key="ClearTextPassword" value="%FEED_TOKEN%" />
          </private>
        </packageSourceCredentials>
      </configuration>
---
{{tool}} reads the NuGet settings like `dotnet restore` does: the
`nuget.config` files in the current folder and its parent folders, then
the user and machine configuration. It has no options of its own for
feeds or folders.

**Feeds.** Every enabled package source is searched; disabled sources are
skipped. `<clear />` drops the sources that the other configuration files
define, and `packageSourceMapping` limits which sources serve a package,
as in a restore. A source can be a URL or a local folder of `.nupkg` files; a
relative path is relative to the folder of the `nuget.config`.

**Offline example.** With the `nuget.config` that [[#offline]] shows, the
tool uses only the folder `feed` next to it and caches into `cache`.
Put `demo.package.1.0.0.nupkg` in `feed`, and the command that
[[#from-feed]] prints caches it and prints its folder, `1.0.0` in
`demo.package` in `cache`. [[#missing]] asks for a version the feed lacks.

**Cache folder.** Packages go to the global packages folder: by default
`.nuget/packages` in your home folder. `globalPackagesFolder` in
`nuget.config` changes it, and the `NUGET_PACKAGES` environment variable
overrides both.

**Credentials.** A private feed needs credentials in
`packageSourceCredentials`, under the key of the source, as in
[[#credentials]] (use it as your `nuget.config`). Keep secrets out of the
file: `%FEED_USER%` and `%FEED_TOKEN%` are read from environment
variables, which CI sets from its secrets. An installed NuGet credential
provider, such as the one for Azure Artifacts, is also used, without
prompts. Rejected or missing credentials give `HTTP 401 (Unauthorized)`
or `HTTP 403 (Forbidden)`; see [[troubleshooting]].
