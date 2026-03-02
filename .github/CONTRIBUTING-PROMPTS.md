
Creating New Copilot Prompts

This guide explains when to create an agent, a task, or a snippet, how to structure each file, and the naming conventions used across the repository. It ensures all prompts remain consistent, predictable, and easy to maintain.
Choosing the right prompt type
🧩 Agents — use when defining a persona

Agents describe who Copilot should be. They define identity, behavior, constraints, and long‑running context.

Create an agent when:

    You need a persistent role (e.g., software engineer, architect, data engineer).

    The prompt defines rules, tone, or decision‑making style.

    The behavior should remain consistent across multiple interactions.

Agents should not be used for single actions.
🛠 Tasks — use when defining an action

Tasks describe what Copilot should do. They are short‑lived, action‑oriented commands.

Create a task when:

    You want a slash command (e.g., /unit-tester).

    The prompt performs a single workflow.

    No persona or long-term behavior is required.

Tasks should not define identity or personality.
🧱 Snippets — use for reusable fragments

Snippets are small, composable pieces of instruction included inside agents or tasks.

Create a snippet when:

    You need to reuse rules across multiple prompts.

    The content is short and focused (e.g., coding standards, commit message rules).

    The text is not meant to be invoked directly.

Snippets should not contain YAML front‑matter.
Naming conventions
Agents
Code

agent.<role>.prompt.md

Tasks
Code

<task>.prompt.md

Snippets
Code

snippet.<topic>.md

Global instructions
Code

copilot-instructions.md

These conventions keep the library predictable and easy to navigate.
Required structure for each file type
Agent structure

Agents must include:

    YAML front‑matter (name, description, author, version, tags)

    Role definition

    Objectives

    Behavioral rules

    Output requirements

    Start behavior

Agents should be 200–500 words (max ~800).
Task structure

Tasks must include:

    YAML front‑matter

    Purpose

    Requirements

    Output format

    Start behavior

Tasks should be 120–300 words (max ~500).
Snippet structure

Snippets must include:

    A single header describing the topic

    Short, focused content (20–120 words, max ~200)

    No YAML front‑matter

Snippets should be readable and composable.
Folder placement

    Agents and tasks go in:
    ./.github/prompts/

    Snippets go in:
    ./.github/prompt-snippets/

    Global instructions stay in:
    ./.github/copilot-instructions.md

No other files should be placed in these folders.
Quality expectations

All prompts must:

    Be concise and free of filler.

    Assume an experienced developer audience.

    Avoid generic troubleshooting steps.

    Use clear, direct language.

    Follow the project’s coding and architectural standards.

    Produce deterministic, predictable behavior.

Prompts should not:

    Duplicate content already covered by snippets.

    Introduce new naming conventions.

    Define overlapping roles or tasks.

When updating an existing prompt

Before modifying a prompt:

    Check whether the change belongs in a snippet instead.

    Ensure the update does not break naming or structure rules.

    Keep changes atomic and diff‑friendly.

    Update the version number in the YAML front‑matter if behavior changes.

