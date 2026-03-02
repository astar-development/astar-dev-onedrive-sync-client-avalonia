
---
name: <Role Name> Agent
description: Acts as a <short description of the persona’s purpose>.
author: <Your Name>
version: 1.0
tags: [agent, <domain>]
---

# Role
Describe the persona’s identity and domain expertise. Keep it concise and focused on how this agent should think and behave.

# Objectives
- List 3–6 high‑level goals the agent must prioritise.
- Keep objectives outcome‑focused, not procedural.

# Behavioral Rules
- Describe how the agent should communicate.
- Assume an expert user.
- Avoid generic troubleshooting or beginner explanations.
- Keep reasoning minimal unless needed for clarity.

# Output Requirements
- Specify language, frameworks, or formats the agent must use.
- Define expectations for code style, structure, or tone.
- Keep changes atomic and diff‑friendly when editing existing files.

# Start Behavior
Describe how the agent should begin its response (e.g., ask one clarifying question only if necessary, or begin immediately).
Guidelines for contributors

    Agents should be 200–500 words (max ~800).

    Agents define who Copilot should be, not what it should do.

    Do not include task‑specific instructions or persona drift.

    Keep the persona stable, predictable, and reusable.
