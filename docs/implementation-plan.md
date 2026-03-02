# Implementation Plan — OneDrive Sync Desktop Application (MVP)

## Refer to the [Implementation Plan background](./implementation-plan-background.md) for the background / further details for this plan

## Delivery Plan (Dev Team)

### Implementation Order and Rationale
1. **Foundation Platform** (enables all slices safely).
2. **Accounts + Sync Configuration** (core invariants and user setup).
3. **Sync Orchestration + Delta Processing** (critical value path).
4. **Offline/Queue + Conflict Resolution** (correctness/resilience).
5. **Version History + Audit/Telemetry** (compliance and policy).
6. **UI Experience hardening** (visibility, themes, localisation).
7. **Performance/Reliability hardening** (large-account confidence).

### Slice Work Packages

#### 1) Foundation Platform
- **Goal**: establish project skeleton and shared primitives.
- **Key Tasks**:
  - Create solution/projects per conventions.
  - Add dependency injection composition root.
  - Add clock abstraction, result/error primitives, domain event contracts.
  - Configure EF Core context and migration bootstrap.
- **Dependencies**: none.
- **Done Criteria**:
  - App boots, DB initialises, base test projects execute.

#### 2) Accounts Slice
- **Goal**: manage account lifecycle and primary invariant.
- **Key Tasks**:
  - Add account add/remove command handlers.
  - Enforce max 5 accounts and exactly one primary.
  - Persist encrypted account metadata and token references.
- **Dependencies**: Foundation.
- **Done Criteria**:
  - Acceptance criteria for account limits/primary state pass.

#### 3) Sync Configuration Slice
- **Goal**: validated local roots and folder selection model.
- **Key Tasks**:
  - Implement root overlap validator (same + parent/child checks).
  - Implement folder discovery listing and include/exclude persistence by folder ID.
  - Disable selection changes while offline.
- **Dependencies**: Accounts, Foundation.
- **Done Criteria**:
  - Root conflict rules enforced; folder-only selection functional.

#### 4) Sync Orchestration Slice
- **Goal**: serial scheduler with cadence constraints.
- **Key Tasks**:
  - Implement scheduler and account eligibility logic.
  - Enforce primary/secondary cadence rules.
  - Enqueue startup auto-sync accounts serially.
  - Surface account sync state to UI model.
- **Dependencies**: Accounts, Sync Configuration.
- **Done Criteria**:
  - No concurrent account sync; cadence rules enforced and visible.

#### 5) Delta Processing Slice
- **Goal**: reliable discovery + incremental synchronisation.
- **Key Tasks**:
  - Implement initial discovery and token capture.
  - Implement incremental delta ingestion.
  - Add duplicate handling and deterministic last-update-wins persistence.
  - Apply DB writes in batches of 50.
  - Handle Graph 429 with retry-after or exponential backoff.
- **Dependencies**: Sync Orchestration.
- **Done Criteria**:
  - Delta token progression stable; duplicates do not corrupt state.

#### 6) Offline & Queue Slice
- **Goal**: durable offline behaviour and safe resume.
- **Key Tasks**:
  - Implement queue persistence and dequeue ordering.
  - Detect offline/online transitions.
  - Pause sync while offline; resume replay on reconnect.
- **Dependencies**: Sync Orchestration, Delta Processing.
- **Done Criteria**:
  - Offline queue survives restart/crash and replays correctly.

#### 7) Conflict Resolution Slice
- **Goal**: explicit user-driven conflict outcomes.
- **Key Tasks**:
  - Detect conflict types including casing mismatch cases.
  - Build decision workflow with apply-to-all option.
  - Persist per-account preferences and session decisions.
- **Dependencies**: Delta Processing, Offline & Queue.
- **Done Criteria**:
  - Conflicts always require explicit decision and resume cleanly.

#### 8) Version History Slice
- **Goal**: enforce retention policy accurately.
- **Key Tasks**:
  - Store version metadata.
  - Implement retention rules: 2 indefinite + up to 7 within 30 days.
  - Add scheduled pruning job.
- **Dependencies**: Delta Processing.
- **Done Criteria**:
  - Retention rules verifiably enforced in integration tests.

#### 9) Audit & Compliance Slice
- **Goal**: required audit coverage and retention controls.
- **Key Tasks**:
  - Implement event capture taxonomy.
  - Implement default retention windows and per-account override.
  - Add sanitisation policies for sensitive logs.
- **Dependencies**: Foundation + all operational slices.
- **Done Criteria**:
  - Required event classes stored and pruned per policy.

#### 10) Telemetry & Feedback Slice
- **Goal**: consent-gated launch telemetry and feedback links.
- **Key Tasks**:
  - Implement consent state machine and consent history.
  - Emit only crash/error telemetry when consented.
  - Add GitHub Issues + support email links.
- **Dependencies**: Audit & Compliance, UI Experience.
- **Done Criteria**:
  - Telemetry off by default; enabled only post-consent.

#### 11) UI Experience Slice
- **Goal**: launch-ready UX constraints and visibility.
- **Key Tasks**:
  - Live account status transitions in account list.
  - Offline indicators and restricted actions.
  - Theme switching (Light/Dark/Hacker launch-ready).
  - Localise launch strings in en-GB resources.
- **Dependencies**: Orchestration, Offline, Conflict, Telemetry.
- **Done Criteria**:
  - Required status, themes, and en-GB content present.

### TDD Strategy (All Slices)
- RED: write failing unit/integration tests first.
- GREEN: minimal implementation to pass.
- REFACTOR: simplify with tests passing.
- Use naming conventions `{ClassName}Should.cs` and `{FeatureName}Should.cs`.
- Prefer integration tests for queue/scheduler/delta boundaries.

### Contract Test Recommendations
- Graph adapter contract tests for 429/backoff, duplicate items, delta token transitions.
- Repository contract tests for queue ordering, batch size of 50, retention pruning.
- UI state contract tests for status transitions and offline restrictions.

## Milestones & Sequencing (3 Months)

### Phase 1 (Weeks 1–3): Foundation + Account Setup
- Foundation Platform, Accounts, Sync Configuration.
- **Risk**: early model churn.
- **Mitigation**: freeze key aggregates and invariants by end of week 3.

### Phase 2 (Weeks 4–7): Core Sync Path
- Sync Orchestration, Delta Processing.
- **Risk**: Graph edge-case complexity.
- **Mitigation**: prioritise adapter contract tests and replay fixtures.

### Phase 3 (Weeks 8–10): Resilience + Compliance Core
- Offline & Queue, Conflict Resolution, Version History.
- **Risk**: state machine bugs under restart/offline transitions.
- **Mitigation**: crash/restart simulation tests and deterministic queue replay tests.

### Phase 4 (Weeks 11–12): Hardening + Launch Readiness
- Audit & Compliance, Telemetry & Feedback, UI Experience hardening, performance pass.
- **Risk**: finish-line scope creep.
- **Mitigation**: strict no-new-feature gate; only acceptance and reliability defects.

## Quality Gates

### Build/Test Gates
- `dotnet restore`
- `dotnet build`
- `dotnet test`
- Merge blocked unless all pass.

### Reliability/Performance Gates
- Validate queue durability across restart/crash scenarios.
- Validate serial execution across all accounts.
- Validate delta processing on large synthetic datasets.
- Validate batch DB writes at size 50.

### Security/GDPR Gates
- Verify telemetry remains disabled before consent.
- Verify sensitive fields are encrypted at rest.
- Verify audit retention windows and per-account overrides.
- Verify logs exclude tokens/file content and sensitive PII.

## Open Questions & ADR Backlog

### Remaining Open Questions
- None blocking MVP implementation from current requirements.

### ADR Backlog
1. **ADR-001**: Serial scheduler and cadence enforcement model.
2. **ADR-002**: Durable queue schema and replay semantics.
3. **ADR-003**: Delta idempotency and duplicate resolution strategy.
4. **ADR-004**: Conflict workflow and apply-to-all persistence boundaries.
5. **ADR-005**: Encryption provider abstraction and future key lifecycle.
6. **ADR-006**: Telemetry consent/event taxonomy and sanitisation rules.
7. **ADR-007**: Audit event taxonomy and retention execution design.
8. **ADR-008**: Theme/localisation extension model (post-MVP readiness).

Suggested ownership: tech lead/developer (single owner), with weekly decision cut-offs aligned to phase boundaries.

## Implementation Hand-off Checklist

- [ ] Confirm solution/project structure matches vertical slices and test projects.
- [ ] Approve core entities/value objects and natural key constraints.
- [ ] Implement and test account/primary/root invariants.
- [ ] Implement scheduler with serial guarantee and cadence checks.
- [ ] Implement delta pipeline with cursor persistence and 429 handling.
- [ ] Enforce duplicate policy and batch writes of 50.
- [ ] Implement durable offline queue and reconnect replay.
- [ ] Implement conflict decision UX + persistence + apply-to-all.
- [ ] Implement version retention policies and pruning.
- [ ] Implement audit taxonomy + retention controls.
- [ ] Implement consent-gated crash/error telemetry.
- [ ] Deliver required UI states, themes (Light/Dark/Hacker), and en-GB localisation.
- [ ] Complete end-to-end acceptance criteria verification.
- [ ] Freeze MVP scope and defer post-MVP items explicitly.
