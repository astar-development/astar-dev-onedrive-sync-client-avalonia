
```mermaid
sequenceDiagram
    autonumber

    participant Dev as Developer
    participant Editor as Editor (VS Code / JetBrains)
    participant Copilot as Copilot Engine
    participant GI as Global Instructions<br/>copilot-instructions.md
    participant Agent as Agent Prompt<br/>./.github/prompts/
    participant Task as Task Prompt<br/>./.github/prompts/
    participant Snip as Snippets<br/>./.github/prompt-snippets/

    Dev->>Editor: Selects agent or triggers task (e.g., /unit-tester)
    Editor->>Copilot: Sends request with selected prompt context

    Copilot->>GI: Loads global rules (coding, architecture, TDD, UX)
    GI-->>Copilot: Provides repo-wide constraints

    alt Agent selected
        Copilot->>Agent: Loads persona, behavior, constraints
        Agent-->>Copilot: Provides role-specific rules
    else Task selected
        Copilot->>Task: Loads workflow instructions
        Task-->>Copilot: Provides action-specific rules
    end

    Copilot->>Snip: Pulls referenced snippets (coding, testing, UX, etc.)
    Snip-->>Copilot: Provides reusable guidelines

    Copilot->>Copilot: Synthesizes instructions + prompt + snippets
    Copilot-->>Editor: Returns output (code, tests, refactor, docs)
    Editor-->>Dev: Displays final result

```
How to read this diagram
Developer

Triggers the workflow by selecting an agent or running a task.
Editor

Passes the selected prompt context to Copilot.
Copilot Engine

The orchestrator. It merges:

    global instructions

    agent or task rules

    referenced snippets

into a single behavioural context.
Global Instructions

Always loaded first. They define:

    coding style

    architecture

    TDD/testing expectations

    UX rules

    communication tone

Agent or Task

Only one is active per interaction:

    Agent → persona with long‑running behavior

    Task → one‑off workflow

Snippets

Reusable fragments pulled in as needed:

    coding standards

    testing guidelines

    Avalonia UX rules

    error handling

    commit messages

Output

Copilot synthesizes everything and returns deterministic, project‑aligned output.
Why this lifecycle matters

    It shows contributors where each file type fits in the decision chain.

    It clarifies why global instructions must stay concise and stable.

    It demonstrates how snippets prevent duplication and drift.

    It helps developers understand why agents behave consistently and tasks remain atomic.

    It reinforces the mental model that Copilot is not “guessing” — it’s following a layered instruction stack.
