# ADR-002: Durable queue schema and replay semantics

- Status: Proposed
- Date: 2026-03-02
- Owner: Dev Team (single developer)
- Review By: End of Week 4

## Context

The MVP requires durable offline queueing, crash-safe persistence, ordered replay on reconnect, and serial account execution. Queue behaviour must preserve intent ordering while remaining idempotent and restart-safe.

## Decision Drivers

- Crash/restart resilience.
- Deterministic replay behaviour.
- Minimal operational complexity.
- Testability under failure simulation.

## Options Considered

1. SQLite-backed `SyncOperation` table with explicit states and dequeue ordering.
2. In-memory queue with periodic snapshotting.
3. File-based append-only queue without relational indexing.

## Decision

Adopt option 1:
- Persist queue items in SQLite as first-class entities.
- Use explicit states: `Pending`, `InProgress`, `WaitingConflict`, `Completed`, `Failed`.
- On startup, demote stale `InProgress` records to `Pending` for safe replay.
- Dequeue order by `(AccountEmail, EnqueuedUtc, Sequence)` with fairness rules.

## Consequences

### Positive
- Strong durability guarantees.
- Clear recovery semantics.
- Efficient query/index support for large backlogs.

### Negative
- Requires careful transactional boundaries.
- Slightly more schema complexity than in-memory approaches.

## Risks and Mitigations

- Risk: duplicate execution during restart.
  - Mitigation: idempotency markers and replay checks.
- Risk: queue growth under prolonged offline periods.
  - Mitigation: bounded retry policies and monitoring metrics.

## Validation

- Integration tests for crash-recovery replay.
- Property-style tests for ordering invariants.
- Migration tests validating queue schema evolution.

## Related

- docs/requirements.md
- docs/implementation-plan.md
