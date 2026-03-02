
Overview

This directory contains all project‑wide configuration for GitHub Copilot. It defines how Copilot behaves across the repository, how prompts are organized, and how developers should create or extend prompt files.

The goal is to keep Copilot predictable, consistent, and aligned with the project’s engineering standards.
Copilot configuration files
Global instructions

copilot-instructions.md  
Defines repository‑wide behavior for Copilot, including coding style, architectural expectations, testing conventions, and general rules. This file influences all Copilot interactions within the workspace.
Prompt folders
Prompts

prompts/  
Contains all agent and task prompt files. These become available as slash commands in supported editors.

Two types of prompts live here:

    Agents — personas with identity, behavior, and constraints

        Naming: agent.<role>.prompt.md

        Purpose: define who Copilot should be

        Examples:

            agent.software-engineer.prompt.md

            agent.data-engineer.prompt.md

    Tasks — one‑off actions or workflows

        Naming: <task>.prompt.md

        Purpose: define what Copilot should do

        Examples:

            unit-tester.prompt.md

            generate-readme.prompt.md

Each prompt file includes YAML front‑matter, a purpose section, rules, output expectations, and start behavior.
Snippets

prompt-snippets/  
Contains reusable fragments that can be included inside agents or tasks. Snippets are not slash commands and are not invoked directly.

    Naming: snippet.<topic>.md

    Examples:

        snippet.coding-standards.md

        snippet.commit-message.md

Snippets should be short, focused, and composable.
Size guidelines

Keeping prompts concise improves reliability and reduces noise.

    Agents: 200–500 words (max ~800)

    Tasks: 120–300 words (max ~500)

    Snippets: 20–120 words (max ~200)

    Global instructions: 300–800 words (max ~1200)

Folder expectations

    Only .prompt.md files belong in prompts/.

    Only snippet files belong in prompt-snippets/.

    Global instructions remain at the root of ./.github/.

    Naming conventions must be followed to keep the library consistent and discoverable.

