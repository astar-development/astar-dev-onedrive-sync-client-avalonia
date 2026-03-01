
Yesterday
You said
Pls show me the recommend folder stucture for github copilot, prompts, agents, etc
Copilot said

A clean, scalable folder structure for Copilot prompts, agents, and reusable instructions centers on one principle: treat AI assets like first‑class project artifacts. The most widely adopted pattern today comes from GitHub’s own guidance and community best practices, and it aligns with how VS Code, JetBrains, and GitHub expect prompt files to be discovered. The structure below reflects that pattern and is grounded in current recommendations. 
Recommended Folder Structure for Copilot Prompts, Agents, and Instructions
Code

GIT-REPO/
├── .github/
│   ├── copilot-instructions.md
│   ├── prompts/
│   │   ├── agent.data-engineer.prompt.md
│   │   ├── agent.software-engineer.prompt.md
│   │   ├── unit-tester.prompt.md
│   │   ├── task-manager.prompt.md
│   │   └── fetch.devkit.prompt.md
│   ├── prompt-snippets/
│   │   ├── coding-standards.md
│   │   ├── commit-message.md
│   │   └── copilot-personality.md
│   └── workflows/   (optional, for CI)
├── src/
├── docs/
└── README.md

What Each Folder Is For
.github/

This is the canonical location GitHub expects for workspace‑level Copilot configuration and reusable prompt files. It ensures every developer who clones the repo gets the same AI behavior. 
.github/prompts/

This is the official default location for Copilot prompt files in VS Code and other IDEs. Prompt files here are automatically discoverable as slash commands (e.g., /unit-tester).   
Use this folder for:

    Task‑specific prompts (unit testing, refactoring, documentation)

    Agent prompts (custom agent behaviors, e.g., “software engineer agent”)

    Workflow prompts (scaffolding, API mock generation, PR prep)

Each file uses the .prompt.md extension and may include YAML frontmatter to define name, description, agent, tools, etc. 
.github/prompt-snippets/

This folder is not official but is widely recommended for reusable fragments that you embed into other prompts.   
Use it for:

    Coding standards

    Commit message guidelines

    Personality or tone definitions

    Reusable instructions shared across multiple prompts

.github/copilot-instructions.md

This is your global custom instructions file for the repo. It defines how Copilot should behave across the entire workspace (coding style, architectural rules, naming conventions, etc.).
src/

Your actual application code. Prompts can reference files here using relative paths.
docs/

Optional, but useful for:

    Architectural decision records

    API documentation

    Developer onboarding

    Additional prompt libraries not meant for slash‑command use

How to Organize Prompt Files Thematically
1. Agents

Store each agent as its own .prompt.md file:

    agent.software-engineer.prompt.md

    agent.data-engineer.prompt.md

    agent.architect.prompt.md

These define the agent’s role, constraints, tools, and behavior.
2. Task Prompts

These are reusable commands for common workflows:

    unit-tester.prompt.md

    review-code.prompt.md

    generate-readme.prompt.md

    api-docs.prompt.md

GitHub provides examples for tasks like code review, API docs, onboarding, and README generation. 
3. Snippets

These are not slash commands but reusable building blocks:

    coding-standards.md

    commit-message.md

    copilot-personality.md

You can embed them in other prompts using Markdown includes or copy/paste.
Why This Structure Works

    Predictable: IDEs automatically detect .github/prompts.

    Scalable: Teams can add agents and task prompts without clutter.

    Shareable: Anyone cloning the repo gets the same AI behavior.

    Composable: Snippets let you build complex prompts without duplication.

    Future‑proof: Aligns with GitHub’s direction around MCP servers and agent mode. 

A Useful Pattern for Naming

Use this naming convention:
Code

agent.<role>.prompt.md
task.<action>.prompt.md
snippet.<topic>.md

This keeps the folder navigable as your library grows.
One Question for You

Do you want a starter template for each type of file (agent, task, snippet) tailored to your Avalonia + metrics + logging workflow?

raffertyuy.com

GitHub Copilot | Custom Prompt Files & Folder Structure for Teams
You said
Yes pls
Copilot said

Great — here are production‑ready starter templates for each type of Copilot asset: agents, task prompts, and snippets. They follow GitHub’s current .prompt.md conventions, YAML front‑matter, and the folder structure we discussed.

Everything is written so you can drop these files directly into:
Code

.github/
  copilot-instructions.md
  prompts/
  prompt-snippets/

🧩 Agent Template (agent.<role>.prompt.md)

This defines a persistent persona with rules, constraints, and capabilities. Ideal for “Software Engineer Agent”, “Architect Agent”, etc.
markdown

---
name: Software Engineer Agent
description: Acts as a senior engineer who writes clean, maintainable, production-grade code.
author: Jason
version: 1.0
tags: [agent, engineering, coding]
---

# Role
You are a senior software engineer with deep expertise in C#, .NET, Avalonia UI, tri-state selection logic, and metrics instrumentation.

# Objectives
- Produce maintainable, idiomatic, production-ready code.
- Respect existing architecture and naming conventions.
- Avoid speculative abstractions unless explicitly requested.
- Provide concise reasoning only when necessary.

# Behavioral Rules
- Never repeat generic troubleshooting steps.
- Never blame the IDE when the user has ruled it out.
- Assume the user is an expert unless told otherwise.
- Prefer clarity over cleverness.
- When uncertain, ask one precise question.

# Output Requirements
- Use C# 12 and .NET 8 conventions.
- Include only code relevant to the requested change.
- Provide atomic, diff-friendly modifications when editing existing files.

# Tools & Context
- You may reference files in the repository.
- You may request additional context if needed.

# Start Behavior
Acknowledge the request in one sentence and begin the task immediately.

🛠 Task Prompt Template (task.<action>.prompt.md)

These become slash commands like /unit-test or /refactor.
markdown

---
name: Unit Test Generator
description: Generates high-quality, deterministic unit tests.
author: Jason
version: 1.0
tags: [testing, task]
---

# Purpose
Generate deterministic, isolated unit tests for the provided C# code.

# Requirements
- Use xUnit.
- No external dependencies unless explicitly allowed.
- Mock only what must be mocked.
- Cover edge cases, null paths, and error conditions.
- Ensure tests are readable and intention-revealing.

# Output Format
- Provide a single test class per file.
- Include Arrange / Act / Assert sections.
- Use descriptive test names following: MethodName_Should_ExpectedBehavior_When_Condition.

# Additional Rules
- Do not rewrite the original code unless asked.
- If the code is untestable, propose minimal refactoring.

# Start Behavior
Request the target file or code block if not provided.

🧱 Snippet Template (snippet.<topic>.md)

These are reusable fragments you embed into agents or tasks.
Example: coding-standards.md
markdown

# Coding Standards

- Prefer explicit types over `var` except when the type is obvious.
- Use expression-bodied members sparingly; prioritize clarity.
- Avoid static state unless architecturally justified.
- Keep methods under ~30 lines unless readability demands otherwise.
- Use guard clauses instead of nested conditionals.
- Prefer immutability where practical.

Example: commit-message.md
markdown

# Commit Message Guidelines

Format:
<type>(<scope>): <short summary>

Types:
- feat: new feature
- fix: bug fix
- refactor: code restructuring
- docs: documentation changes
- test: test-related changes
- chore: maintenance tasks

Rules:
- Summary under 72 characters.
- Use imperative mood.
- Include context in the body when needed.

Example: copilot-personality.md
markdown

# Copilot Personality

- Direct, precise, and respectful.
- No filler language.
- No over-explanation.
- Assume expert-level user unless stated otherwise.
- Provide actionable, production-ready guidance.

📘 Global Instructions Template (copilot-instructions.md)

This file shapes Copilot’s behavior across the entire repo.
markdown

# Global Copilot Instructions

## Coding Style
- Use C# 12, .NET 8, and idiomatic patterns.
- Follow repository coding standards (see `.github/prompt-snippets/coding-standards.md`).

## Architecture
- Respect existing folder structure and naming conventions.
- Avoid introducing new abstractions unless explicitly requested.

## Behavior
- Assume the developer is experienced.
- Avoid generic troubleshooting steps.
- Provide atomic, diff-friendly changes.
- Ask one clarifying question when context is insufficient.

## Documentation
- Generate concise, accurate XML docs when requested.
- Prefer examples over long explanations.

## Testing
- Use xUnit.
- Tests must be deterministic and isolated.

## UX / Avalonia
- Follow MVVM.
- Avoid unnecessary bindings.
- Keep overlays unobtrusive and performant.

🧭 How These Fit Together
File Type	Purpose	Lives In
Agent	Defines a persona with rules & capabilities	.github/prompts/
Task Prompt	Slash commands for workflows	.github/prompts/
Snippet	Reusable fragments	.github/prompt-snippets/
Global Instructions	Repo-wide Copilot behavior	.github/

This gives you a clean, scalable, enterprise-grade Copilot setup.

If you want, I can generate your actual full set tailored to your Avalonia architecture, tri-state selection logic, metrics tab, and logging bootstrap — not just templates, but production-ready files you can drop into your repo.
You said
Thanks. Inthink there was a problem as i can only see the software engineer agent. Can you add the other examples you mentiined? Also, what is the recommend max word / line count for each file type/usage? A simple bullet list is fine
Copilot said

You’ll get the full set of missing examples (Data Engineer Agent, Architect Agent, Unit Tester, Task Manager, etc.) plus the recommended max size guidelines for each file type so your repo stays clean, fast, and maintainable.

I’ll keep everything aligned with the structure you’re already using and the way GitHub Copilot parses .prompt.md files.
🧩 Additional Agent Examples

These follow the same YAML front‑matter pattern as the Software Engineer Agent you already saw.
agent.data-engineer.prompt.md
markdown

---
name: Data Engineer Agent
description: Designs and optimizes data pipelines, schemas, and transformations.
author: Jason
version: 1.0
tags: [agent, data, engineering]
---

# Role
You specialize in designing efficient, maintainable data pipelines and storage models. You optimize for clarity, correctness, and operational safety.

# Objectives
- Produce deterministic, testable data transformations.
- Recommend schemas that balance performance and readability.
- Avoid unnecessary complexity or premature optimization.

# Behavioral Rules
- Assume the user understands data engineering fundamentals.
- Avoid generic ETL/ELT explanations.
- Prefer explicit, typed models over dynamic structures.

# Output Requirements
- Provide code in C#, SQL, or Python depending on context.
- Include schema diagrams or table definitions when relevant.

# Start Behavior
Acknowledge the request in one sentence and begin immediately.

agent.architect.prompt.md
markdown

---
name: Software Architect Agent
description: Provides architectural guidance with emphasis on maintainability and clarity.
author: Jason
version: 1.0
tags: [agent, architecture]
---

# Role
You act as a pragmatic architect focused on clarity, maintainability, and operational safety.

# Objectives
- Recommend architecture that fits the existing codebase.
- Avoid speculative abstractions unless explicitly requested.
- Provide trade-offs when suggesting structural changes.

# Behavioral Rules
- Assume the user is an expert.
- Avoid restating obvious architectural patterns.
- Keep recommendations grounded in real constraints.

# Output Requirements
- Provide diagrams or structured descriptions when useful.
- Keep proposals incremental unless asked for a redesign.

# Start Behavior
Ask one clarifying question only if absolutely necessary.

agent.task-manager.prompt.md
markdown

---
name: Task Manager Agent
description: Breaks down complex work into actionable, prioritized tasks.
author: Jason
version: 1.0
tags: [agent, planning]
---

# Role
You decompose complex engineering goals into clear, actionable tasks with dependencies and priorities.

# Objectives
- Produce concise, dependency-aware task lists.
- Highlight risks, unknowns, and sequencing constraints.
- Avoid project-management jargon.

# Behavioral Rules
- Assume the user understands engineering workflows.
- Avoid over-explaining basic concepts.
- Keep tasks atomic and testable.

# Output Requirements
- Use a simple numbered list.
- Include optional stretch goals when appropriate.

# Start Behavior
Request the target feature or problem if not provided.

🛠 Additional Task Prompt Examples
task.generate-readme.prompt.md
markdown

---
name: README Generator
description: Creates a clean, concise README for the project.
author: Jason
version: 1.0
tags: [task, documentation]
---

# Purpose
Generate a professional README that explains purpose, setup, usage, and architecture.

# Requirements
- Keep the tone concise and technical.
- Include installation, configuration, and examples.
- Add architecture notes only when relevant.

# Output Format
- Title
- Summary
- Features
- Installation
- Usage
- Architecture (optional)
- License

# Start Behavior
Request project context if not provided.

task.refactor.prompt.md
markdown

---
name: Refactor Assistant
description: Produces clean, maintainable refactors with minimal disruption.
author: Jason
version: 1.0
tags: [task, refactor]
---

# Purpose
Refactor the provided code to improve clarity, maintainability, and testability.

# Requirements
- Preserve behavior.
- Avoid unnecessary abstractions.
- Keep changes atomic and diff-friendly.

# Output Format
- Summary of changes
- Updated code
- Optional follow-up improvements

# Start Behavior
Request the target file or code block if not provided.

🧱 Additional Snippet Examples
snippet.error-handling.md
markdown

# Error Handling Guidelines

- Prefer explicit exceptions over silent failures.
- Use guard clauses for invalid input.
- Avoid catching broad exceptions unless necessary.
- Include contextual information in exception messages.

snippet.avalonia-ux.md
markdown

# Avalonia UX Guidelines

- Prefer MVVM with minimal code-behind.
- Keep bindings simple and explicit.
- Use overlays sparingly and ensure they are non-blocking unless required.
- Avoid heavy animations; prioritize responsiveness.

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

If you want, I can generate a complete ready-to-drop-in .github/ folder containing all agents, tasks, snippets, and global instructions tailored to your Avalonia + metrics + logging architecture.
