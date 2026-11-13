# Convert oEmbed library from VB.NET to C#

Mechanical source-language conversion with a modern C# project format, while preserving the existing public API and observable behavior unless an internal implementation improvement is demonstrably non-observable.

## Specs

1. **S1** — "Please help me convert the oembed project from VB to C#."
2. **S2** — "There should otherwise be no functional changes."
3. **S3** — "Make no commits. Do not version or deploy anything."
4. **S4** — User clarified: convert every VB file, including generated `My Project/*.Designer.vb` files.
5. **S5** — User clarified: modernize the library to an SDK-style C# project while targeting .NET Framework 4.8.
6. **S6** — User clarified: preserve the existing public API exactly, including the misspelled `GetAppliationErrorObject` method, marker interfaces, provider regexes, and public type names; safe internal improvements are permitted only when they do not alter observable behavior.

## Tasks

- T1 — S1-S6 — Convert all library VB sources and generated project support files to C#, preserving namespaces, public signatures, attributes, constants, regexes, and serialization names. Commit: none (user prohibited commits).
- T2 — S1, S5 — Replace the legacy VB project with an SDK-style C# .NET Framework 4.8 project and update the solution while preserving assembly identity and references. Commit: none (user prohibited commits).
- T3 — S2, S6 — Update repository guidance and documentation to describe the C# project without changing unrelated LocalMCP policy. Commit: none (user prohibited commits).
- T4 — S2, S6 — Build Release artifacts and perform public API/metadata and behavior-preservation checks. Commit: none (user prohibited commits).

## Log

L1 — Awesome!  Next, please help me convert the oembed project from VB to C#.  
There should otherwise be no functional changes.  
Make no commits. Do not version or deploy anything.

L2 — User selected: convert everything; modernize project format; preserve API, improve internals only when behavior remains unchanged.
