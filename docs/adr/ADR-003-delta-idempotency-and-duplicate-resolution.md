# ADR-003: Delta idempotency and duplicate resolution strategy

- Status: Proposed
- Date: 2026-03-02
- Owner: Dev Team (single developer)
- Review By: End of Week 6

## Context

Microsoft Graph delta responses may contain duplicate items. MVP requires deterministic handling using last-update-wins persistence, with robust 429 retry/backoff and no silent failure.

## Decision Drivers

- Deterministic data correctness.
- Replay safety across retries/restarts.
- Compatibility with large datasets.

## Options Considered

1. Idempotency key by `(AccountEmail, DriveItemId, EffectiveVersionMarker)` and deterministic dedupe before persistence.
2. Last record in batch wins without identity keying.
3. Strict upsert on item ID only, ignoring version marker.

## Decision

Adopt option 1:
- Deduplicate in-memory per batch using account + item identity + effective version marker.
- For duplicates in same batch, keep highest `LastModifiedUtc`; tie-break with stable lexical item ID.
- Persist with deterministic last-update-wins policy.
- Advance delta cursor atomically with successful batch commit.

## Consequences

### Positive
- Robust against duplicate and replayed events.
- Explicit and testable idempotency contract.

### Negative
- Requires additional metadata and comparison logic.
- Cursor and write ordering must remain strongly coordinated.

## Risks and Mitigations

- Risk: cursor advances despite partial write failure.
  - Mitigation: transactional write + cursor update unit.
- Risk: timestamp anomalies from upstream.
  - Mitigation: deterministic secondary tie-break strategy.

## Validation

- Contract tests with duplicate delta fixtures.
- Integration tests for retry/replay without corruption.
- Tests for 429 handling using `Retry-After` and exponential backoff fallback.

## Related

- docs/requirements.md
- docs/implementation-plan.md
