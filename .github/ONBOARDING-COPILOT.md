Using Copilot in This Repository

This project includes a structured set of Copilot prompts to help you work consistently and efficiently. This guide explains how to use them inside your editor and how they fit into the development workflow.
Where the prompts live

The .github/ directory contains all Copilot configuration:

    copilot-instructions.md — global behavior for Copilot

    prompts/ — agent and task prompts

    prompt-snippets/ — reusable fragments used by prompts

These files shape how Copilot behaves when you write code, generate tests, refactor, or perform other tasks.
How to use prompts in your editor
VS Code

Copilot supports slash commands and prompt selection:

    Type / in the editor to see available task prompts (e.g., /unit-tester, /generate-readme).

    Use the Copilot sidebar to browse all prompts in .github/prompts/.

    Select an agent prompt to switch Copilot into a specific persona (e.g., Software Engineer Agent).

    Select a task prompt to run a one‑off workflow.

JetBrains IDEs (Rider, IntelliJ, etc.)

    Open the Copilot tool window.

    Use the “Prompts” tab to browse all agent and task prompts.

    Click a prompt to activate it.

    Agents persist until you switch to another prompt or reset Copilot.

    Tasks run once and then return Copilot to its default behavior.

How agents work

Agents define who Copilot should be. They set tone, behavior, constraints, and expectations.

Examples:

    Software Engineer Agent

    Data Engineer Agent

    Architect Agent

When you activate an agent:

    Copilot adopts that persona for the current session.

    It follows the rules defined in the agent file.

    It maintains consistent behavior across multiple interactions.

Use agents when you want Copilot to act like a teammate with a specific role.
How tasks work

Tasks define what Copilot should do. They are single‑action workflows.

Examples:

    /unit-tester

    /refactor

    /generate-readme

When you run a task:

    Copilot performs the action once.

    It does not adopt a persona.

    It returns to normal behavior afterward.

Use tasks when you want a repeatable, predictable action.
How snippets work

Snippets are reusable fragments included inside agents or tasks.

Examples:

    Coding standards

    Commit message rules

    Avalonia UX guidelines

Snippets:

    Are not slash commands

    Do not appear in the Copilot UI

    Are included by reference inside prompts

They help keep prompts consistent and avoid duplication.
When to use which prompt

    Use an agent when you want Copilot to behave like a specific expert.

    Use a task when you want Copilot to perform a single workflow.

    Use a snippet when writing or updating prompts.

    Rely on global instructions for repo‑wide behavior.

Tips for working effectively with Copilot in this repo

    Keep prompts concise; shorter prompts produce more reliable behavior.

    Use task prompts for repeatable workflows like generating tests or refactoring.

    Switch agents when you need a different perspective or role.

    When Copilot’s output drifts, re‑select the agent or re‑run the task.

    Follow the naming conventions so new prompts remain discoverable.
