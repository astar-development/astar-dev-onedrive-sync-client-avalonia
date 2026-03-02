
Purpose of this folder

This folder contains all Copilot prompt files that define agents (personas) and tasks (actions). These files become available as slash commands inside supported editors and help standardize how Copilot behaves across the project.
File types
🧩 Agent prompts

Agent prompts define personas with identity, behavior, constraints, and long‑running context. They describe who Copilot should be.

    Naming pattern: agent.<role>.prompt.md

    Examples:

        agent.software-engineer.prompt.md

        agent.data-engineer.prompt.md

        agent.architect.prompt.md

Use an agent when the prompt defines a role that persists across multiple interactions.
🛠 Task prompts

Task prompts define actions or workflows. They describe what Copilot should do.

    Naming pattern: <task>.prompt.md

    Examples:

        unit-tester.prompt.md

        generate-readme.prompt.md

        refactor.prompt.md

Use a task prompt for one‑off commands that do not require a persona.
File structure

Each .prompt.md file should include:

    YAML front‑matter (name, description, author, version, tags)

    A clear purpose section

    Rules or constraints

    Output expectations

    Start behavior (how Copilot should begin)

Size guidelines

    Agents: 200–500 words (max ~800)

    Tasks: 120–300 words (max ~500)

Keeping prompts concise improves reliability and reduces noise.
Folder expectations

    Only .prompt.md files live here.

    Snippets belong in ../prompt-snippets/.

    Global instructions belong in ../copilot-instructions.md.
