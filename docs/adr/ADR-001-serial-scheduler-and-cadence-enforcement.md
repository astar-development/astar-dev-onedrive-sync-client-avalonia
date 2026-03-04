# ADR-001: Serial scheduler and cadence enforcement model

- Status: Proposed
- Date: 2026-03-02
- Owner: Dev Team (single developer)
- Review By: End of Week 3

## Context

The MVP requires support for up to 5 accounts with exactly one primary account. Sync must never run concurrently across accounts. Cadence rules must be enforced:
- Primary account manual sync: not more than once every 15 minutes after previous completion.
- Secondary accounts: not more than once per hour.
- Auto-sync-enabled accounts must queue at startup and execute serially.

## Decision Drivers

- Correctness and predictable behaviour.
- Simplicity for one-developer delivery.
- Deterministic UI status updates.
- Recovery safety after restart.

## Options Considered

1. Single global scheduler loop with process-scoped serial lock and per-account eligibility checks.
2. Per-account independent schedulers with distributed lock coordination.
3. Pure cron-like timers per account without queue centralisation.

## Decision

Adopt option 1:
- Implement one scheduler loop that evaluates account eligibility and dequeues at most one account at a time.
- Persist last completion timestamps per account in UTC.
- Expose ineligible reasons (cadence window, offline, already queued) to UI state.

## Consequences

### Positive
- Strong non-concurrency guarantee with low implementation complexity.
- Easier observability and debugging.
- Natural fit for queue-first architecture.

### Negative
- Throughput is intentionally limited by serial design.
- Scheduler loop reliability becomes a critical path.

## Risks and Mitigations

- Risk: starvation of lower-priority accounts.
  - Mitigation: fair queue ordering with ageing.
- Risk: clock drift affecting cadence decisions.
  - Mitigation: central `IClock` abstraction and UTC-only persistence.

## Validation

- Unit tests for cadence eligibility rules.
- Integration tests proving no concurrent account sync execution.
- Startup auto-sync ordering tests.

## Related

- docs/requirements.md
- docs/implementation-plan.md
