
Snippet Index
Purpose

This index lists all reusable prompt fragments available in this repository. Snippets provide focused rules or guidelines that can be included inside agents or tasks. They are not invoked directly and do not contain YAML front‑matter.
Available snippets
Coding and architecture

    snippet.coding-standards.md — Core coding conventions for C# 12 / .NET 8, including clarity, explicitness, and method structure.

    snippet.architecture-basics.md — High‑level architectural principles, layering rules, and MVVM expectations.

    snippet.error-handling.md — Consistent error‑handling practices, guard clauses, and exception rules.

Testing and TDD

    snippet.testing-guidelines.md — Deterministic xUnit testing, TDD expectations, naming conventions, and isolation rules.

UX and Avalonia

    snippet.avalonia-ux.md — MVVM‑focused Avalonia UI guidance, binding rules, overlays, and performance considerations.

Communication and workflow

    snippet.commit-message.md — Commit message format, types, and style rules.

    snippet.personality.md — Shared communication tone: direct, precise, expert‑level, and concise.

How to use snippets

Snippets are designed to be referenced from agents and tasks to avoid duplication and keep prompts concise. They should be included when a prompt needs consistent rules but should not be expanded inline.

Examples of appropriate use:

    Referencing coding standards from an agent persona.

    Linking to testing guidelines from a unit‑test task prompt.

    Including UX rules in an Avalonia‑related agent or task.

Adding new snippets

Create a new snippet when:

    A rule or guideline is reused across multiple prompts.

    Content is too detailed for copilot-instructions.md.

    The topic is narrow and self‑contained.

Naming pattern:
Code

snippet.<topic>.md

Snippets should be short (20–120 words), focused, and written as standalone fragments.

