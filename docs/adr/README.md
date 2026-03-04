# Architecture Decision Records (ADRs)

This directory contains architectural decisions for the OneDrive Sync Desktop Application MVP, aligned to the implementation plan in [docs/implementation-plan.md](../implementation-plan.md).

## ADR Process

1. **Proposed**: New decision under review before implementation.
2. **Accepted**: Decision approved; implementation may proceed.
3. **Implemented**: Decision code delivered to main branch.
4. **Superseded**: Previous decision replaced by a newer one.

Decision reviews should occur at phase boundaries aligned to the 3-month MVP timeline. See review dates in individual ADR headers.

## Decision Registry

| ID | Title | Status | Review By | Owner |
|----|-------|--------|-----------|-------|
| [001](ADR-001-serial-scheduler-and-cadence-enforcement.md) | Serial scheduler and cadence enforcement model | Proposed | Week 3 | Dev Team |
| [002](ADR-002-durable-queue-schema-and-replay-semantics.md) | Durable queue schema and replay semantics | Proposed | Week 4 | Dev Team |
| [003](ADR-003-delta-idempotency-and-duplicate-resolution.md) | Delta idempotency and duplicate resolution strategy | Proposed | Week 6 | Dev Team |
| [004](ADR-004-conflict-workflow-and-apply-to-all-boundaries.md) | Conflict workflow and apply-to-all persistence boundaries | Proposed | Week 8 | Dev Team |
| [005](ADR-005-encryption-provider-and-key-lifecycle.md) | Encryption provider abstraction and key lifecycle | Proposed | Week 5 | Dev Team |
| [006](ADR-006-telemetry-consent-and-sanitisation-policy.md) | Telemetry consent, event taxonomy, and sanitisation policy | Proposed | Week 10 | Dev Team |
| [007](ADR-007-audit-taxonomy-and-retention-execution.md) | Audit event taxonomy and retention execution design | Proposed | Week 10 | Dev Team |
| [008](ADR-008-theme-and-localisation-extension-model.md) | Theme and localisation extension model | Proposed | Week 11 | Dev Team |

## Key Decision Clusters

### Sync Correctness (ADRs 001–003)
- Serial scheduling and cadence enforcement (ADR-001)
- Durable queue persistence and recovery (ADR-002)
- Delta processing idempotency and deduplication (ADR-003)

### User Reconciliation (ADR-004)
- Explicit conflict resolution workflow and apply-to-all boundaries (ADR-004)

### Security & Privacy (ADRs 005–007)
- Encryption provider abstraction and future key rotation (ADR-005)
- GDPR-compliant telemetry consent and sanitisation (ADR-006)
- Audit taxonomy and retention policies (ADR-007)

### Product Extension (ADR-008)
- Theme and localisation infrastructure for MVP and post-MVP (ADR-008)

## Review Checklist

Before approving an ADR:
- [ ] Decision drivers align with requirements in [docs/requirements.md](../requirements.md).
- [ ] Options and trade-offs are clearly weighed.
- [ ] Risks and mitigations are realistic.
- [ ] Validation strategy is actionable (testable).
- [ ] No conflicts with other active/accepted ADRs.
- [ ] Implementation timeline realistic for assigned phase.

## Updating ADR Status

When an ADR is reviewed and accepted:
1. Change `Status: Proposed` → `Status: Accepted` in ADR header.
2. Update this table.
3. Commit with message: `adr: accept ADR-NNN (description)`.

When an ADR is superseded:
1. Change `Status: Proposed|Accepted` → `Status: Superseded By ADR-NNN`.
2. Create a new ADR for the replacement.
3. Update this table.

## Archival & History

ADRs are maintained indefinitely as record of decisions and trade-off context. Even superseded decisions remain visible for audit and learning purposes.

---

**Related Documents**
- [docs/requirements.md](../requirements.md) — Product and scope baseline.
- [docs/implementation-plan.md](../implementation-plan.md) — Delivery sequencing and phase planning.
- [.github/copilot-instructions.md](../../.github/copilot-instructions.md) — Technical foundation and conventions.
