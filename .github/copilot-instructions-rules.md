# Purpose of the instructions file

The instructions file defines global, repo‑wide behavior for Copilot. It sets expectations for coding style, architecture, testing, UX, and tone. It should not duplicate content that belongs in agents, tasks, or snippets. Instead, it should act as the “north star” for how Copilot behaves in this project.
Recommended structure for copilot‑instructions.md

A clean structure keeps the file readable and predictable:
1. Project context

A short paragraph describing the project’s domain, stack, and architectural style.
2. Coding standards

High‑level rules only. Link to snippets for details.
3. Architectural expectations

Guidance on layering, naming, patterns, and constraints.
4. Behavioral expectations

How Copilot should communicate, what to avoid, and how to handle ambiguity.
5. Testing conventions

Framework, style, and expectations.
6. UX / UI rules (if applicable)

High‑level principles; details live in snippets.
7. File references

Links to snippets and prompt folders.

This structure keeps the file focused on global rules while delegating specifics to reusable fragments.
What to include (and what to avoid)
Include

    High‑level coding style (e.g., “use C# 14 idioms”)

    Architectural constraints (e.g., “MVVM only, no code‑behind except trivial glue”)

    Behavioral rules (e.g., “assume expert user, avoid generic troubleshooting”)

    Testing expectations (e.g., “use xUnit, deterministic tests only”)

    UX principles (e.g., “overlays must be unobtrusive and performant”)

    Pointers to snippets for details

Avoid

    Long lists of rules (move to snippets)

    Persona‑like behavior (belongs in agents)

    Task‑specific instructions (belongs in task prompts)

    Repetition of content already in snippets

    Anything that changes frequently (keep instructions stable)

Soft and hard length limits

These limits keep the file readable and ensure Copilot can parse it effectively.
Soft limit: 300–800 words

This is the ideal range for clarity and maintainability.
Hard limit: ~1200 words

Beyond this, Copilot’s behavior becomes less predictable and humans stop reading it.
Rule of thumb

If the file scrolls more than two screens in VS Code, it’s too long.
How to link to sub‑documents effectively

Copilot benefits from explicit references to snippets and prompt folders. Humans benefit from clarity. The best pattern is:
1. Reference snippets by name

Example:

    Follow the coding standards in snippet.coding-standards.md.

2. Reference prompt folders by purpose

Example:

    Agents and tasks are defined in ./.github/prompts/.

3. Reference UX or architecture snippets

Example:

    Avalonia UX rules are defined in snippet.avalonia-ux.md.

4. Avoid inline duplication

Never paste snippet content into the instructions file. Copilot will read both files.
5. Keep references stable

Do not rename snippet files casually; Copilot relies on consistent filenames.
Recommended template for copilot‑instructions.md

This is the structure you can use directly:
Code

# Global Copilot Instructions

## Project Context
Short description of the project, tech stack, and architectural style.

## Coding Standards
High-level rules.

See [`./.github/prompt-snippets/snippet.coding-standards.md`](./.github/prompt-snippets/snippet.coding-standards.md) for details.

## Architecture
High-level architectural constraints.
Reference architecture snippets if needed.

## Behavior
How Copilot should communicate.
Assume expert user.
Avoid generic troubleshooting.
Keep changes atomic and diff-friendly.

## Testing
Use xUnit.
Tests must be deterministic and isolated.
See `snippet.testing-guidelines.md` if present.

## UX / UI
High-level UX principles.
See `snippet.avalonia-ux.md` for details.

## Prompt Library
Agents and tasks live in `./.github/prompts/`.
Snippets live in `./.github/prompt-snippets/`.

This keeps the file clean, modular, and easy to maintain.
Practical tips for maximizing effectiveness

    Keep the file stable; move volatile details into snippets.

    Use short, declarative sentences; Copilot responds better to them.

    Avoid contradictions across snippets and instructions.

    Keep the instructions file focused on global rules only.

    Treat snippets as the “extended manual” and instructions as the “executive summary.”

