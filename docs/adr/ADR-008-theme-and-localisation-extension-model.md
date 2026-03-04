# ADR-008: Theme and localisation extension model

- Status: Proposed
- Date: 2026-03-02
- Owner: Dev Team (single developer)
- Review By: End of Week 11

## Context

Launch requires Light, Dark, and Hacker themes plus en-GB localisation. Architecture must structurally support all seven themes and future languages in 3–6 months without major refactoring.

## Decision Drivers

- Launch readiness with minimal scope.
- Low-friction post-MVP extension.
- Consistent UX and localisation discipline.

## Options Considered

1. Theme registry + resource-driven localisation with launch-complete subset and placeholder support.
2. Hard-coded theme/localisation logic in views.
3. Full implementation of all themes/languages at MVP.

## Decision

Adopt option 1:
- Implement theme registry for all seven variants.
- Mark non-launch themes as placeholder-ready.
- Route all user-facing text through resource localisation pipeline.
- Keep en-GB as launch locale and define locale-loading extension point.

## Consequences

### Positive
- Meets launch scope while preserving expansion path.
- Reduces post-MVP rework.
- Encourages consistent localisation from start.

### Negative
- Requires discipline to avoid hard-coded strings in MVP.
- Placeholder themes may have limited polish initially.

## Risks and Mitigations

- Risk: untranslated strings slipping into UI.
  - Mitigation: localisation lint/checklist and UI test coverage.
- Risk: theme regressions during switching.
  - Mitigation: theme-switch smoke tests and shared token usage.

## Validation

- UI tests for runtime theme switching across launch themes.
- Checks ensuring all visible strings are resource-backed.
- Sanity tests for placeholder theme loading.

## Related

- docs/requirements.md
- docs/implementation-plan.md
