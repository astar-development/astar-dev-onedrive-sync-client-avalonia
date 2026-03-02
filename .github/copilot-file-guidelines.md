📏 Recommended Maximum Size Guidelines

These keep prompts fast, readable, and easy for Copilot to parse.
Agents

    Target: 200–500 words

    Hard max: ~800 words

    Why: Agents define behavior; too long → diluted, too short → ineffective.

Task Prompts

    Target: 120–300 words

    Hard max: ~500 words

    Why: Tasks should be crisp, focused, and easy to trigger.

Snippets

    Target: 20–120 words

    Hard max: ~200 words

    Why: Snippets are meant to be composable fragments.

Global Instructions

    Target: 300–800 words

    Hard max: ~1200 words

    Why: This file shapes repo-wide behavior; clarity > volume.

General Rule

    If a prompt scrolls more than one screen in VS Code, it’s probably too long.

🧭 Why the Two Naming Styles Exist
1) agent.<role>.prompt.md

This naming pattern is used for persistent personas — long‑lived agents with identity, behavior, constraints, and capabilities.

These files define:

    a role

    a personality

    rules

    output expectations

    long‑running behavior

They are meant to be invoked as “agents,” not one‑off tasks.

Examples:

    agent.software-engineer.prompt.md

    agent.data-engineer.prompt.md

    agent.architect.prompt.md

These behave like “Copilot personalities” or “AI teammates.”
2) unit-tester.prompt.md

This pattern is used for task prompts, which are short-lived, action‑oriented commands.

These files define:

    a single workflow

    a repeatable action

    no persona

    no long-term behavior

They are meant to be invoked as slash commands like /unit-tester.

Examples:

    unit-tester.prompt.md

    generate-readme.prompt.md

    refactor.prompt.md

These behave like “tools” or “macros,” not agents.
🧩 The Naming Convention in One Sentence

    Use agent.<role>.prompt.md for personas.

    Use <task>.prompt.md for actions.

This keeps your prompt library clean, predictable, and scalable.
🧱 Recommended Naming Rules (simple and practical)

    Agents:  
    agent.<role>.prompt.md  
    Because they define a persona with rules and behavior.

    Tasks:  
    <task>.prompt.md  
    Because they define a single action or workflow.

    Snippets:  
    snippet.<topic>.md  
    Because they are fragments, not commands.

    Global instructions:  
    copilot-instructions.md  
    Because GitHub expects this exact filename.

🧭 Why This Matters

Copilot (and humans) benefit from:

    predictable discovery

    clear separation of concerns

    easy mental model of “who is an agent” vs “what is a task”

    scalable naming as your library grows

It also prevents confusion when you have dozens of prompts.

Naming conventions for Copilot prompt files
🧩 Agents (personas)

Use this pattern:
Code

agent.<role>.prompt.md

Agents represent persistent personas with identity, behavior, constraints, and long‑running context. They are not one‑off commands.

Examples:

    agent.software-engineer.prompt.md

    agent.data-engineer.prompt.md

    agent.architect.prompt.md

    agent.task-manager.prompt.md

Why this prefix matters:

    Makes agents visually distinct from tasks.

    Makes it obvious these files define who Copilot should be, not what it should do.

    Scales cleanly as you add more personas.

🛠 Task prompts (actions)

Use this pattern:
Code

<task>.prompt.md

Tasks represent single actions or workflows. They are short-lived and do not define a persona.

Examples:

    unit-tester.prompt.md

    generate-readme.prompt.md

    refactor.prompt.md

    api-docs.prompt.md

Why no prefix:

    Tasks are verbs; the filename should read like a command.

    Keeps slash commands clean (/unit-tester instead of /task.unit-tester).

    Avoids clutter when you have many tasks.

🧱 Snippets (reusable fragments)

Use this pattern:
Code

snippet.<topic>.md

Snippets are not commands and not personas. They are reusable building blocks.

Examples:

    snippet.coding-standards.md

    snippet.commit-message.md

    snippet.avalonia-ux.md

Why this prefix:

    Makes it clear these files are not meant to be invoked directly.

    Keeps them grouped together in the folder.

📘 Global instructions

This file has a fixed name:
Code

copilot-instructions.md

GitHub expects this exact filename for repo‑wide behavior.
Summary table
File type	Purpose	Naming pattern	Example
Agent	Persona with rules & behavior	agent..prompt.md	agent.software-engineer.prompt.md
Task	Action or workflow	.prompt.md	unit-tester.prompt.md
Snippet	Reusable fragment	snippet..md	snippet.coding-standards.md
Global instructions	Repo-wide behavior	copilot-instructions.md	copilot-instructions.md
Why this convention works for teams

    Makes the folder self-explanatory.

    Prevents naming drift as more prompts are added.

    Helps new developers understand the difference between agents and tasks instantly.

    Keeps slash commands clean and readable.

    Scales to dozens of prompts without confusion.


