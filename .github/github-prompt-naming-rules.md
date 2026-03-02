
# Prompt Naming Rules

## Agent prompts

Agent prompts define personas with identity, behavior, constraints, and long‑running context. They describe who Copilot should be.

    Naming pattern: agent.<role>.prompt.md

    Examples:

        agent.software-engineer.prompt.md

        agent.data-engineer.prompt.md

        agent.architect.prompt.md

Agents should be used when the prompt defines a role that persists across multiple interactions.
Task prompts

Task prompts define actions or workflows. They describe what Copilot should do.

    Naming pattern: <task>.prompt.md

    Examples:

        unit-tester.prompt.md

        generate-readme.prompt.md

        refactor.prompt.md

Tasks should be used for one‑off commands that do not require a persona.
Snippets

Snippets are reusable fragments included inside agents or tasks. They are not invoked directly.

    Naming pattern: snippet.<topic>.md

    Examples:

        snippet.coding-standards.md

        snippet.commit-message.md

        snippet.avalonia-ux.md

Snippets should be short, focused, and composable.
Global instructions

The repository‑wide Copilot behavior file uses a fixed name.

    Filename: copilot-instructions.md

This file defines coding style, architectural rules, and general expectations for Copilot across the entire project.
Summary

    Agents: agent.<role>.prompt.md — personas

    Tasks: <task>.prompt.md — actions

    Snippets: snippet.<topic>.md — reusable fragments

    Global: copilot-instructions.md — repo‑wide rules
