# Implementation Plan — OneDrive Sync Desktop Application (MVP)

## Executive Summary

This plan delivers the MVP in 3 months using a local-first, single-process architecture optimised for sync correctness, resilience, and strict scope control. The implementation follows vertical slices, test-first delivery (RED → GREEN → REFACTOR), and minimal operational complexity suitable for one developer and near-zero budget.

The architecture centres on:
- serial cross-account orchestration,
- durable queue-backed sync operations,
- deterministic delta/idempotency behaviour,
- explicit conflict decision flows,
- GDPR-compliant consent-gated telemetry and audit retention.

## Scope Baseline

### MVP In Scope (Authoritative)
- Up to 5 accounts, exactly one primary account.
- Folder-only selective sync with initial folder discovery.
- Initial delta sync + incremental sync.
- Duplicate Graph item handling with deterministic last-update-wins persistence.
- Serial sync execution only (no concurrent cross-account sync).
- Cadence rules: primary manual >=15 minutes since completion; secondary <=1 per hour.
- Auto-sync on startup (queued serially).
- Offline pause + durable local queue + reconnect resume.
- Conflict handling with explicit user choice and apply-to-all.
- Version policy: keep 2 indefinitely + up to 7 in last 30 days.
- In-app sync status visibility in account list.
- Required audit events + retention controls per account.
- Telemetry consent-gated, launch telemetry limited to errors/crashes.
- Launch themes: Light, Dark, Hacker.
- Launch localisation: en-GB.

### Explicitly Out of Scope (Post-MVP)
- Fourth layout implementation (diagnostics remains candidate only).
- Full implementation of remaining themes beyond placeholders.
- Additional launch languages beyond en-GB.
- OS-level integrations (tray/shell/native notifications).
- Shared-permissions enforcement workflows.
- Cached file-content encryption.
- Advanced key rotation/management.
- Expanded telemetry beyond crashes/errors.
- Smart-sync/recommendation features.

### Assumptions and Planning Decisions
- Single local SQLite database per app installation.
- Single process owns queue scheduling to preserve serial guarantees.
- Local filesystem watcher events are treated as queued intents, not immediate sync actions.
- All business timestamps are normalised to UTC at boundaries.
- Diagnostics instrumentation is added now as internal events, UI exposure deferred.

## Architecture Overview

### Context-Level Components
- **Avalonia Client (UI Layer)**
  - Dashboard, Explorer, Terminal, placeholder for fourth layout.
  - Account list state, conflict prompts, offline indicators, theme/localisation controls.
- **Application Slices (Feature-Oriented)**
  - Commands/queries/models/views/tests grouped by business capability.
- **Sync Core Services**
  - Scheduler, operation queue, delta processor, conflict resolver, retention executor.
- **Infrastructure**
  - SQLite + EF Core repositories and configurations.
  - Graph API integration through Kiota V5.
  - Encryption provider abstraction for sensitive columns.
  - Telemetry and audit sinks (consent-gated telemetry).
- **External**
  - Microsoft Graph API (OneDrive delta/content metadata).

### Vertical Slice Mapping
- **Accounts Slice**: account lifecycle, primary-account invariant, max=5 enforcement.
- **Sync Configuration Slice**: local root policy, folder identity selection rules.
- **Sync Orchestration Slice**: serial scheduler, cadence gates, startup queueing.
- **Delta Processing Slice**: discovery, incremental tokens, idempotent persistence.
- **Conflict Resolution Slice**: conflict detection, user decisions, apply-to-all state.
- **Offline & Queue Slice**: network state transitions, durable intents, resume orchestration.
- **Version History Slice**: metadata retention rules and pruning jobs.
- **Audit & Compliance Slice**: event capture, per-account retention policy execution.
- **Telemetry & Feedback Slice**: consent model, error/crash pipeline, feedback links.
- **UI Experience Slice**: account status updates, themes, localisation plumbing.

### Core Architectural Patterns
- Functional flow composition with `Result<T, TError>`, `Option<T>`, and explicit mapping/binding.
- Domain invariants in application/domain services (not UI).
- Mediated boundaries between slices via command/query interfaces and domain events.
- Outbox-like durable queue table for sync intents and resumability.
- Idempotent processors keyed by account + remote item identity + version marker.

## Domain & Data Design

### Core Model Set
- **Entity: SyncAccount** (class)
  - Natural key: `Email`.
  - Fields: `IsPrimary`, cadence policy, `AutoSyncEnabled`, encrypted display metadata.
  - Shadow `Guid Id` only for technical joins where natural key is impractical.
- **Entity: SyncRoot** (class)
  - Natural key candidate: `(AccountEmail, NormalisedRootPath)`.
  - Enforces no duplicate and no parent/child overlap across accounts.
- **Entity: FolderSelection** (class)
  - Natural key: `(AccountEmail, DriveItemId)`.
  - Stores include/exclude and effective state.
- **Entity: SyncOperation** (class)
  - Durable ordered queue item, account-scoped.
  - Suggested fields: `OperationType`, `State`, `EnqueuedUtc`, `AttemptCount`, `PayloadJson`.
- **Entity: DeltaCursor** (class)
  - Natural key: `(AccountEmail, ScopeKey)`.
  - Tracks discovery/delta progression and resumable cursors.
- **Entity: FileVersionMetadata** (class)
  - Natural key: `(AccountEmail, DriveItemId, VersionId)`.
- **Entity: ConflictRecord** (class)
  - Natural key candidate: `(AccountEmail, ConflictKey)`.
- **Entity: ConflictDecisionPreference** (class)
  - Per-account defaults and active apply-to-all session marker.
- **Entity: AuditEvent** (class)
  - Account-scoped event type, payload summary, retention class.
- **Entity: TelemetryConsent / ConsentChange** (class)
  - Consent timeline with source and timestamp.

### Value Objects (record/record struct)
- `AccountEmail`, `AccountTier`, `SyncCadencePolicy`, `NormalisedPath`, `DriveItemIdentity`, `UtcTimestamp`, `RetentionPolicy`, `ConflictChoice`, `QueuePosition`.

### EF Core Strategy
- Separate `IEntityTypeConfiguration<T>` per entity.
- Explicit unique constraints for natural keys.
- Shadow GUIDs only where required.
- Add concurrency tokens (`RowVersion`/equivalent) for queue and conflict records.
- Migrations applied at startup.

### Indexing Strategy
- `SyncOperation`: `(AccountEmail, State, EnqueuedUtc)` for dequeue order.
- `DeltaCursor`: `(AccountEmail, ScopeKey)` unique.
- `FolderSelection`: `(AccountEmail, DriveItemId)` unique + `(AccountEmail, IsIncluded)`.
- `ConflictRecord`: `(AccountEmail, State, DetectedUtc)`.
- `AuditEvent`: `(AccountEmail, EventType, CreatedUtc)` for pruning.
- `FileVersionMetadata`: `(AccountEmail, DriveItemId, CreatedUtc)` for retention jobs.

### Encryption Boundaries
- Encrypt at column level: email aliases/display names/tokens/secrets and other sensitive metadata.
- Do not encrypt cached file content for MVP.
- Introduce `IEncryptionProvider` abstraction now to allow future key rotation without schema redesign.

## Sync Engine Design

### Serial Scheduler Model
- Single scheduler loop picks next runnable account based on cadence + queue state.
- Global lock (process-scoped) guarantees one active account sync at any time.
- Per-account operation queues support startup enqueue and offline replay.

### Cadence Enforcement
- Primary account manual sync command rejected if `< 15 minutes` since previous completion.
- Secondary sync attempts blocked if `< 1 hour` since completion.
- Scheduler computes next eligible timestamp per account and surfaces reason in UI.

### Delta Processing Pipeline
1. Resolve account auth/session.
2. Load folder selection set by ID.
3. If first run: discovery pass and baseline token capture.
4. Incremental delta query using stored cursor.
5. Filter to selected-folder scope.
6. Detect duplicates and resolve deterministically.
7. Persist batched updates (size 50).
8. Emit state updates/events and advance cursor atomically.

### Duplicate & Idempotency Strategy
- Identity key: `(AccountEmail, DriveItemId, EffectiveVersionMarker)`.
- If duplicate detected in batch, retain newest by `LastModifiedUtc` (tie-breaker: stable lexical item ID).
- Persistence policy remains last-update-wins, deterministic.
- Operations are replay-safe by checking applied marker before write.

### Offline Queue and Resume
- Network monitor raises offline/online domain events.
- Offline: pause remote operations, continue local observation queueing.
- Restrict sync-selection commands while offline.
- Reconnect: replay queue in original order with stale-operation validation.
- Queue writes are transactionally committed before acknowledging UI action.

### Conflict Workflow
- Detect conflict categories: content divergence, casing mismatch, rename/move ambiguity.
- Create `ConflictRecord` and pause impacted item pipeline.
- UI presents item-level choice with optional apply-to-all.
- Persist decisions per account and optional session scope.
- Resume pipeline after decision commit; audit decision event.

### State Machine (Account Sync)
- `Idle` → `Eligible` → `Queued` → `Syncing` → (`WaitingForConflictDecision` | `Completed` | `Failed`) → `Idle`.
- `Offline` acts as an overlay state pausing transition into `Syncing`.

## Cross-Cutting Concerns

### Telemetry & Consent
- Default telemetry state: disabled.
- Collect only crash/error telemetry after explicit consent.
- Maintain consent history (`ConsentChange`) with timestamp and source.
- Sanitise PII and token-like content before emission.

### Audit Logging & Retention
- Required event classes: auth/session, sync start/end/failure, conflict decisions, consent changes, export/delete actions.
- Default retention execution:
  - Errors + consent events: 30 days.
  - Other events: 7 days.
- Retention policy overridable per account configuration.
- Scheduled pruning job runs daily (plus startup catch-up).

### Localisation & Theming
- All user-facing strings through localisable resources (en-GB at launch).
- Theme system structurally supports all 7 themes; launch quality for Light/Dark/Hacker.
- Placeholder theme assets allowed for non-launch themes.

### Time Handling
- Abstract time via `IClock` interface.
- Convert all external times to UTC at ingress.
- Persist UTC only; format in UI using en-GB culture.

## Delivery Plan (Dev Team)

### Implementation Order and Rationale
1. **Foundation Platform** (enables all slices safely).
2. **Accounts + Sync Configuration** (core invariants and user setup).
3. **Sync Orchestration + Delta Processing** (critical value path).
4. **Offline/Queue + Conflict Resolution** (correctness/resilience).
5. **Version History + Audit/Telemetry** (compliance and policy).
6. **UI Experience hardening** (visibility, themes, localisation).
7. **Performance/Reliability hardening** (large-account confidence).

## Refer to the [Implementation Plan](./implementation-plan.md) for the phase details