# ADR-007: Audit event taxonomy and retention execution design

- Status: Proposed
- Date: 2026-03-02
- Owner: Dev Team (single developer)
- Review By: End of Week 10

## Context

MVP requires required audit events plus per-account retention controls. Baseline retention targets are 30 days for errors and consent events, and 7 days for other audit events.

## Decision Drivers

- Compliance evidence quality.
- Retention correctness and low maintenance cost.
- Queryability at account scope.

## Options Considered

1. Typed `AuditEvent` model with retention class metadata and scheduled pruning job.
2. Unstructured log files with external parsing for retention.
3. Keep all events indefinitely in MVP.

## Decision

Adopt option 1:
- Persist structured audit events with event class and retention class.
- Implement default retention windows by class.
- Support per-account retention override constraints.
- Execute daily pruning job plus startup catch-up pruning.

## Consequences

### Positive
- Deterministic retention behaviour.
- Better compliance reporting and debugging.
- Clear extension path for new event classes.

### Negative
- Requires taxonomy governance.
- Potential migration effort for taxonomy changes.

## Risks and Mitigations

- Risk: misclassified events causing over/under retention.
  - Mitigation: central enum mapping and review checklist.
- Risk: pruning job lag under large datasets.
  - Mitigation: indexed pruning queries and batched deletes.

## Validation

- Tests for event classification and retention mapping.
- Integration tests for pruning windows and per-account overrides.
- Performance checks on prune queries with large fixtures.

## Related

- docs/requirements.md
- docs/implementation-plan.md
