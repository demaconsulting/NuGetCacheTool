---
name: nuget-cache
schema: 2
title: NuGet Cache Tool
summary: Built-in help of NuGet Cache Tool, which caches NuGet packages for builds.
---
{{tool}} makes sure that NuGet packages are in the global packages folder. It downloads the missing ones from the
feeds of your NuGet configuration and prints the folder of each package, so a build can use files from it.

Every operation is an option or an `<id>:<version>` argument. The only command is `{{tool}} help`, which must be the
first argument and takes topic names, such as `{{tool}} help caching`.
