
Copilot Quick Reference (Project Guide)
How Copilot is configured in this repo

    Global behavior is defined in ./.github/copilot-instructions.md.

    Agents and tasks live in ./.github/prompts/.

    Reusable fragments live in ./.github/prompt-snippets/.

    All prompts follow strict naming conventions so they’re easy to find and use.

Agents — personas you can switch Copilot into

Agents define who Copilot should be (e.g., Software Engineer, Architect, Data Engineer).

    Naming: agent.<role>.prompt.md

    Use when you want consistent behavior across multiple interactions.

    Agents persist until you switch to another prompt or reset Copilot.

How to use:  
Open the Copilot prompt list → select an agent → Copilot adopts that persona.
Tasks — one‑off actions you can trigger

Tasks define what Copilot should do (e.g., generate tests, refactor code, create a README).

    Naming: <task>.prompt.md

    Use when you want a single workflow executed once.

    Tasks do not define a persona and do not persist.

How to use:  
Type / in your editor → choose a task (e.g., /unit-tester).
Snippets — reusable fragments for prompt authors

Snippets are small, composable pieces of instruction used inside agents or tasks.

    Naming: snippet.<topic>.md

    Not slash commands.

    Not selectable in the UI.

    Used to avoid duplication across prompts.

When to use what

    Need a persona? Use an agent.

    Need a workflow? Use a task.

    Need reusable rules? Use a snippet.

    Need repo‑wide defaults? Edit global instructions.

Size guidelines

    Agents: 200–500 words

    Tasks: 120–300 words

    Snippets: 20–120 words

    Global instructions: 300–800 words

Shorter prompts produce more reliable behavior.
Tips for effective use

    Switch agents when you need a different perspective or role.

    Use tasks for repeatable workflows like generating tests or refactoring.

    Re-run a task if Copilot drifts off-topic.

    Keep prompts concise and consistent with naming rules.

    When adding new prompts, follow the contributor guide in ./.github/CONTRIBUTING-PROMPTS.md.

