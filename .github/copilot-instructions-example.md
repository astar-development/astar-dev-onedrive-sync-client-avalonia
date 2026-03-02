
Global Copilot Instructions
Project context

This repository uses C# 14, .NET 10, and Avalonia UI with an MVVM architecture. The codebase emphasizes clarity, maintainability, operational safety, and deterministic behavior. Copilot should assume an expert developer audience and produce output that fits seamlessly into the existing structure.
Coding standards

Follow the project’s coding style and conventions. Keep code explicit, readable, and intention‑revealing. Avoid speculative abstractions unless explicitly requested. Prefer small, composable methods and guard clauses over nested conditionals.

Detailed rules are defined in:

    ./.github/prompt-snippets/snippet.coding-standards.md

Architecture

Respect the existing architecture and naming conventions. Use MVVM for Avalonia UI, keep code‑behind minimal, and avoid introducing new layers or patterns unless requested. When proposing changes, provide clear trade‑offs and keep modifications atomic.

Additional architectural guidance is in:

    ./.github/prompt-snippets/snippet.avalonia-ux.md

    ./.github/prompt-snippets/snippet.error-handling.md

Behavior

Assume the user is experienced. Avoid generic troubleshooting steps, beginner explanations, or IDE‑blaming. Communicate concisely and directly. When context is insufficient, ask one precise question. When editing existing code, provide diff‑friendly changes and avoid rewriting unrelated sections.

Copilot should:

    Produce production‑ready code.

    Keep reasoning minimal unless needed for clarity.

    Maintain consistent tone and precision.

    Avoid filler language and unnecessary verbosity.

Testing and TDD

Use xUnit for all tests. Tests must be deterministic, isolated, and intention‑revealing. Follow a strict TDD mindset: design tests that express behavior, not implementation details. When generating or modifying code, consider testability and propose minimal refactoring when needed.

Testing expectations:

    Use Arrange / Act / Assert.

    Prefer descriptive test names.

    Cover edge cases, null paths, and error conditions.

    Avoid mocking unless necessary.

Extended testing rules live in:

    ./.github/prompt-snippets/snippet.testing-guidelines.md (create if needed)

UX and Avalonia

Follow MVVM principles and keep bindings simple and explicit. Overlays must be unobtrusive, performant, and visually consistent. Avoid heavy animations and unnecessary complexity. Ensure UI changes remain responsive and predictable.

UX details are in:

    ./.github/prompt-snippets/snippet.avalonia-ux.md

Prompt library

Agents and tasks are defined in:

    ./.github/prompts/

Reusable fragments are defined in:

    ./.github/prompt-snippets/

Agents define personas.
Tasks define workflows.
Snippets define reusable rules.
Length expectations

To keep this file effective for both humans and Copilot:

    Target length: 300–800 words

    Hard maximum: ~1200 words

    Delegate detailed rules to snippets rather than expanding this file.

Summary

This file defines global behavior for Copilot across the repository. It sets expectations for coding style, architecture, testing, UX, and communication. All detailed rules live in snippets, and all personas or workflows live in prompt files. Copilot should produce concise, maintainable, production‑ready output that respects the project’s structure and conventions.

