# ADR-004: Conflict workflow and apply-to-all persistence boundaries

- Status: Proposed
- Date: 2026-03-02
- Owner: Dev Team (single developer)
- Review By: End of Week 8

## Context

MVP requires explicit user decisions for conflicts at item level with an apply-to-all option. Conflict preferences are per account and must survive app restarts. Sync pipeline should pause impacted items until a decision is recorded.

## Decision Drivers

- User control and trust.
- Predictable conflict outcomes.
- Minimal irreversible automation.

## Options Considered

1. Persist `ConflictRecord` per item and account-scoped `ConflictDecisionPreference` with optional session apply-to-all token.
2. Prompt-only transient decisions with no persistence.
3. Global app-wide conflict defaults not scoped by account.

## Decision

Adopt option 1:
- Create explicit conflict records with lifecycle state.
- Require decision before processing impacted item.
- Support apply-to-all at session scope with account-level persistence options.
- Audit all conflict decisions.

## Consequences

### Positive
- Meets strict explicit-decision requirement.
- Supports both novice and power-user workflows.
- Enables replay-safe recovery after restart.

### Negative
- Additional UX and state complexity.
- Potential queue pause if user does not respond.

## Risks and Mitigations

- Risk: long-lived unresolved conflicts blocking progress.
  - Mitigation: clear UI surfaced queue state and manual resume controls.
- Risk: incorrect scope for apply-to-all decisions.
  - Mitigation: explicit scope labelling in UI and persistence model.

## Validation

- Integration tests for pause/resume conflict flow.
- UI tests for item-level and apply-to-all decision paths.
- Persistence tests across app restart.

## Related

- docs/requirements.md
- docs/implementation-plan.md
