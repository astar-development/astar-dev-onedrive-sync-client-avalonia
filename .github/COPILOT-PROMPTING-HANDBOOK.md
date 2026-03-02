

Copilot Prompting Handbook
Purpose of this handbook

This handbook defines how Copilot is configured, how prompts are organized, and how contributors should create, maintain, and use prompt files in this repository. It ensures consistency, predictability, and high‑quality output across the entire engineering workflow.
How Copilot is structured in this repository
Global instructions

/./.github/copilot-instructions.md  
Defines repo‑wide behavior: coding style, architecture, testing, UX, tone, and expectations.
Prompt library

/./.github/prompts/  
Contains all agents (personas) and tasks (actions). These appear as slash commands or selectable prompts in supported editors.
Snippets

/./.github/prompt-snippets/  
Reusable fragments included inside agents or tasks. They keep prompts concise and consistent.
Documentation

    README.md — project overview

    ./.github/README.md — Copilot folder overview

    ./.github/ONBOARDING-COPILOT.md — how to use prompts in editors

    ./.github/CONTRIBUTING-PROMPTS.md — how to create new prompts

    ./.github/prompt-snippets/INDEX.md — snippet catalogue

Prompt types and when to use them
Agents — personas

Agents define who Copilot should be. They include identity, behavior, constraints, and long‑running context.

Use an agent when:

    You want Copilot to act like a specific expert (e.g., Software Engineer, Architect).

    You need consistent behavior across multiple interactions.

Naming:
Code

agent.<role>.prompt.md

Tasks — workflows

Tasks define what Copilot should do. They are single‑action commands.

Use a task when:

    You want a repeatable workflow (e.g., generate tests, refactor code).

    No persona or long‑term behavior is required.

Naming:
Code

<task>.prompt.md

Snippets — reusable fragments

Snippets are short, focused rules included inside agents or tasks.

Use a snippet when:

    Content is reused across prompts.

    Details would clutter the global instructions or prompt files.

Naming:
Code

snippet.<topic>.md

Folder structure
Code

./.github/
│
├── copilot-instructions.md
│
├── prompts/
│   ├── agent.software-engineer.prompt.md
│   ├── agent.architect.prompt.md
│   ├── unit-tester.prompt.md
│   ├── refactor.prompt.md
│   └── TEMPLATE.agent.prompt.md
│
└── prompt-snippets/
    ├── snippet.coding-standards.md
    ├── snippet.testing-guidelines.md
    ├── snippet.avalonia-ux.md
    ├── snippet.error-handling.md
    ├── INDEX.md
    └── TEMPLATE.md

This structure keeps prompts discoverable, maintainable, and scalable.
Length guidelines
Agents

    Target: 200–500 words

    Hard max: ~800 words

Tasks

    Target: 120–300 words

    Hard max: ~500 words

Snippets

    Target: 20–120 words

    Hard max: ~200 words

Global instructions

    Target: 300–800 words

    Hard max: ~1200 words

Shorter prompts produce more reliable behavior.
Required structure for each prompt type
Agent structure

    YAML front‑matter

    Role

    Objectives

    Behavioral rules

    Output requirements

    Start behavior

Task structure

    YAML front‑matter

    Purpose

    Requirements

    Output format

    Start behavior

Snippet structure

    Title

    Short, focused content

    No YAML front‑matter

How to create new prompts
When to create an agent

    You need a persona with consistent behavior.

    You want Copilot to act like a specific expert.

    The rules apply across multiple interactions.

When to create a task

    You need a single workflow.

    You want a slash command.

    No persona is required.

When to create a snippet

    Content is reused across prompts.

    Details would clutter the global instructions.

    The topic is narrow and self‑contained.

Naming rules

    Agents: agent.<role>.prompt.md

    Tasks: <task>.prompt.md

    Snippets: snippet.<topic>.md

Templates

Use the templates in ./.github/prompts/ and ./.github/prompt-snippets/ to ensure consistency.
How to use prompts in your editor
VS Code

    Type / to see task prompts.

    Use the Copilot sidebar to select agents.

    Agents persist until changed; tasks run once.

JetBrains (Rider, IntelliJ, etc.)

    Use the Copilot tool window.

    Select agents or tasks from the prompt list.

    Agents persist; tasks are one‑off.

TDD and testing expectations

This repository follows strict TDD principles:

    Write tests that express behavior before implementing solutions.

    Use xUnit.

    Tests must be deterministic and isolated.

    Prefer descriptive test names.

    Avoid mocking unless necessary.

Detailed rules live in:

    snippet.testing-guidelines.md

UX and Avalonia expectations

    Follow MVVM.

    Keep bindings simple and explicit.

    Overlays must be unobtrusive and performant.

    Avoid heavy animations.

Detailed rules live in:

    snippet.avalonia-ux.md

Contributor responsibilities

    Follow naming conventions.

    Use templates for new prompts.

    Keep prompts concise.

    Avoid duplicating snippet content.

    Keep changes atomic and diff‑friendly.

    Update version numbers when behavior changes.

    Ensure new prompts fit the defined structure.
