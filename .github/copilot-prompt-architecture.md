Copilot Prompt Architecture (Mermaid)

```mermaid
flowchart TD

    %% Top-level global instructions
    A[Global Instructions<br/>copilot-instructions.md]:::core

    %% Prompt folders
    B[Agents<br/>.github/prompts/]:::agent
    C[Tasks<br/>.github/prompts/]:::task
    D[Snippets<br/>.github/prompt-snippets/]:::snippet

    %% Relationships
    A --> B
    A --> C
    A --> D

    %% Agents and tasks consume snippets
    D --> B
    D --> C

    %% Editors
    E[Developer Editor<br/>(VS Code / JetBrains)]:::editor
    B --> E
    C --> E

    %% Styles
    classDef core fill:#4c8bf5,stroke:#1a4fb3,stroke-width:2px,color:#fff;
    classDef agent fill:#6aa84f,stroke:#274e13,stroke-width:2px,color:#fff;
    classDef task fill:#f6b26b,stroke:#b45f06,stroke-width:2px,color:#fff;
    classDef snippet fill:#8e7cc3,stroke:#351c75,stroke-width:2px,color:#fff;
    classDef editor fill:#999,stroke:#333,stroke-width:2px,color:#fff;
```
