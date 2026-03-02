
#Snippet Template
Purpose

Snippets are short, reusable fragments included inside agents or tasks. They provide focused rules or guidelines and are not invoked directly. Use a snippet when content is reused across multiple prompts or is too detailed for copilot-instructions.md.
Naming

Use the pattern:
Code

snippet.<topic>.md

Examples:

    snippet.coding-standards.md

    snippet.testing-guidelines.md

Structure

Snippets must follow this structure:
Code

# <Title>

Short, focused content describing a single topic. Keep the text concise, declarative, and reusable. Avoid persona-like language, task instructions, or YAML front‑matter.

Writing guidelines

    Keep the content 20–120 words (hard max ~200).

    Focus on one topic only.

    Use short, direct sentences.

    Avoid duplication with other snippets.

    Do not include YAML front‑matter.

    Do not define personas or workflows.

    Do not reference editor behavior or slash commands.

    Keep the snippet reusable across multiple prompts.

Example
Code

# Logging Guidelines

- Use structured logging with clear, intention‑revealing message templates.
- Include contextual properties rather than embedding data in strings.
- Avoid logging sensitive information.
- Keep log levels consistent: Debug for diagnostics, Information for flow, Warning for recoverable issues, Error for failures.

