# Requirements — OneDrive Sync Desktop Application

## Executive Summary

This product is a cross-platform desktop OneDrive Sync client for individual users who need reliable multi-account sync without combining accounts in the cloud. The launch market is the UK (`en-GB`), with GDPR compliance required. The core differentiators are multi-account support, fine-grained folder selection, and free permanent availability under MIT.

The MVP target is 3 months, delivered by one developer with near-zero budget. Architecture must therefore prioritise local-first simplicity, strong sync correctness, and strict scope control.

## 1) User Base & Market

### Business Requirements
- Primary audience: individual users managing multiple OneDrive accounts.
- No user segment is explicitly excluded.
- Initial personas:
  - **Jay** (power/admin user): multiple daily sync needs, image-heavy workloads, multi-account separation requirement.
  - **Lucia** (basic user): low technical confidence, expects automation, large account size exceeding local disk capacity.
- Primary geography: UK.
- Initial localisation: `en-GB`.
- Compliance baseline: GDPR.
- Time handling: UTC throughout; convert API-provided times to UTC where required (NodaTime acceptable).

### Architectural Implications
- Multi-account flows are a first-class UX and domain concern.
- Time abstractions should be explicit and centralised.
- Localisation infrastructure must exist from launch despite single-language MVP.

## 2) Feature Prioritisation & MVP

### Must-Have Features (priority order)
1. Multi-account support.
2. Selective folder sync using initial delta discovery of available folders/files.
3. Version history policy.
4. In-app notifications/status visibility.

### Value Proposition
- Multi-account superiority.
- Fine-grained folder-level sync control.
- Free and open-source.
- Distinctive UX via themes/layouts.

### Fourth Layout
- Still TBD.
- Preferred direction (Jay): diagnostics/troubleshooting view with debug info, error logs, and metrics.

### Architectural Implications
- Folder selection model and delta sync are central to MVP.
- Diagnostics data pipeline should be designed now even if the fourth layout ships later.

## Scope Boundary (Strict)

### MVP In Scope (Launch in 3 Months)
- Multi-account support (max 5 accounts), with exactly one primary account.
- Folder-only selective sync (no file-level selection).
- Initial delta discovery + incremental sync with duplicate handling.
- Serial sync execution (no concurrent cross-account sync).
- Sync cadence policy enforcement (primary 15-minute manual window; secondary hourly maximum).
- Per-account auto-sync on startup with queue-based processing.
- Offline state handling with durable local queue and reconnect resume.
- Conflict handling via explicit user choice (item-level + apply-to-all).
- Version history policy (2 indefinite + up to 7 versions within 30 days).
- In-app notifications/account-list sync state visibility.
- Required audit logging + per-account retention controls.
- GDPR consent-gated crash/error telemetry.
- Launch themes: Light, Dark, Hacker.
- Localisation: en-GB only.

### Post-MVP / Explicitly Out of Scope for Launch
- Full implementation of remaining themes (Auto, Colourful, High Contrast, Professional) beyond placeholders.
- Additional languages beyond en-GB.
- Fourth layout implementation (diagnostics view remains candidate, not launch-committed).
- OS-level integrations (tray, shell extensions, native notifications).
- Shared-permissions enforcement/monitoring workflows.
- Cached file-content encryption.
- Advanced key management/rotation.
- Expanded telemetry beyond crash/error events.
- Folder recommendation/smart-sync suggestion engine.

### Scope Control Rules
- Any requirement not listed in “MVP In Scope” is treated as post-MVP by default.
- No new launch features without explicit trade-off (remove one MVP item of similar effort).
- Architecture may prepare extension points for post-MVP items, but implementation is deferred.

## 3) Sync Behaviour & Data Handling

### Business Requirements
- Selection granularity: **folders only** (no individual file selection).
- No file-type filtering.
- No hard file-size limit; apply sensible bandwidth constraints.
- No automatic recommendation of folders to sync.
- Local renaming must be supported and reflected in inclusion/exclusion behaviour.
- Conflict handling: always user choice at item level with an “apply to all” option.
- Conflict preferences stored per account.
- Offline behaviour:
  - Pause sync while offline.
  - Queue local changes since last complete sync.
  - Inform users of offline/online status changes.
  - Allow local file/folder access while offline.
  - Disallow sync-selection changes while offline.
- Version history policy:
  - Keep 2 versions indefinitely.
  - Keep up to 7 versions if they are within last 30 days.

### Architectural Implications
- Folder identity should be ID-based, not name-based.
- Sync engine requires explicit state machine and durable operation queue.
- Conflict UI and decision persistence are mandatory.
- Background retention jobs required for version policy enforcement.

## 4) Multi-Account & Permissions

### Business Requirements
- Maximum accounts per user: 5.
- Exactly one account may be designated as primary.
- Local sync roots must be isolated per account:
  - No same root across accounts.
  - No parent/child overlap across account roots.
- No concurrent sync across accounts.
- Shared-permissions handling is out of scope for MVP.

### Architectural Implications
- Strong path validation rules at configuration time.
- Serial sync scheduler with per-account queueing.
- Permission revocation handling can remain API-error-based for MVP.

## 5) Scale & Performance Requirements

### Business Requirements
- No business-imposed hard cap on file count or sync size.
- Real-world expected large accounts: ~400k files, ~500GB.
- Users must not be functionally blocked from full sync if disk space permits.
- Sync cadence constraints:
  - Primary account: manual sync no more than every 15 minutes, after prior sync completes.
  - Secondary accounts: no more than hourly.
- Auto-sync is configurable per account.
- On app startup, accounts with auto-sync enabled should start immediately (queued serially, not concurrently).
- UI must show live account sync state (“syncing”) and update as queue advances.
- Platform priority: Linux-first only if it improves delivery speed; otherwise no strict platform priority.
- No OS-level integrations at launch.

### Architectural Implications
- Efficient paging/delta processing and resumability are required.
- Database indexing and batching are critical for large datasets.
- Scheduler policy must enforce account-tier cadence and non-concurrency.

## 6) Security & Compliance Strategy

### Business Requirements
- No additional launch auth requirements beyond OAuth 2.0 via Microsoft Graph.
- No additional data residency constraints beyond GDPR baseline.
- Encryption scope for MVP:
  - Encrypt metadata/secrets (emails, names, tokens).
  - Do not encrypt cached file content.
- Key approach: system-managed key; advanced key management/rotation deferred.

### Architectural Implications
- Encryption abstraction should allow future key rotation without schema redesign.
- Security boundaries and logging sanitisation are still mandatory at MVP.

## 7) Theming & Globalisation

### Business Requirements
- Launch theme minimum: Light, Dark, Hacker.
- Remaining themes may be placeholders initially.
- Accessibility target: best effort for MVP.
- Launch localisation: `en-GB`.
- Additional languages planned in 3–6 months.
- In-app help/documentation should be localised when new languages are added.

### Architectural Implications
- Theme system should support all 7 variants structurally from launch.
- Localisation should cover both UI strings and help content model.

## 8) Telemetry & User Feedback

### Business Requirements
- Telemetry must be GDPR-compliant and consent-gated.
- Non-negotiable launch telemetry: errors and crashes.
- Other usage/performance telemetry may be phased in later.
- Feedback channels for MVP:
  - GitHub Issues link.
  - Email support link.

### Architectural Implications
- Minimal telemetry pipeline now, extensible event model later.
- Consent gating and PII sanitisation are required from day one.

## 9) Go-to-Market & Resource

### Business Requirements
- MVP launch target: 3 months.
- Team: 1 developer.
- Budget: near-zero (Copilot Pro + development time).
- Licensing model: open source, MIT, permanently free.

### Architectural Implications
- Avoid paid dependencies and backend-heavy architecture for MVP.
- Ruthless scope control required to hit timeline.

## 10) Constraints & Risk Assessment

### Known Technical Constraints
- Microsoft Graph rate limiting must handle `429` responses cleanly.
- Use exponential backoff when Graph does not provide retry delay.
- Graph may return duplicate items during sync.
- Duplicate item handling policy: last-update-wins in persistence.
- Batch DB updates (50 updates per batch).
- Batch UI updates to preserve “real-time feel” without UI overload.
- Honour OneDrive/local casing differences; treat relevant mismatches as conflicts.

### Top Business Risks (priority)
1. Delivery speed.
2. User adoption.
3. Scope creep.

## Data Model Considerations

- **Natural keys first** (per project guideline), with GUID shadow properties where natural keys are absent.
- Core entities/value objects likely required:
  - `SyncAccount` (natural key: account email; flags: `IsPrimary`, cadence policy, auto-sync setting).
  - `SyncRoot` (validated per-account local root, non-overlap constraints).
  - `FolderSelection` (account-scoped include/exclude by folder identity).
  - `SyncOperation` queue (durable, ordered, account-scoped).
  - `FileVersion` metadata (retention policy: 2 indefinite + up to 7 within 30 days).
  - `ConflictRecord` + `ConflictDecision` (per-account preferences + apply-to-all sessions).
  - `AuditEvent` (typed retention policies, account-scoped).
  - `TelemetryConsent` and `ConsentChange` records.
- Required indexing focus:
  - Account + folder identity.
  - Delta token progression per account.
  - Pending queue order and state.
  - Audit retention pruning queries.

## Vertical Slice Breakdown

### MVP Slices
- **Accounts Slice**
  - Add/remove accounts, designate primary account, enforce max=5.
- **Sync Configuration Slice**
  - Local root assignment, non-overlap validation, folder selection.
- **Sync Orchestration Slice**
  - Serial scheduler, per-account cadence policy, startup auto-sync queue.
- **Delta Processing Slice**
  - Initial discovery, incremental delta sync, duplicate handling (last-update-wins).
- **Conflict Resolution Slice**
  - Item-level decisions, apply-to-all workflow, per-account preference persistence.
- **Offline & Queue Slice**
  - Offline detection, queued local changes, reconnect resume.
- **Version History Slice**
  - Version tracking and retention enforcement.
- **Audit & Compliance Slice**
  - Required event capture and retention policy execution.
- **Telemetry & Feedback Slice**
  - Consent-gated crash/error telemetry, GitHub/email feedback entry points.
- **UI Experience Slice**
  - Layout navigation, theme switching, live sync status in account list.

### Post-MVP Slices
- **Diagnostics Slice (TBD layout candidate)**
  - Debug data, error logs, sync metrics visualisation.

## Non-Functional Requirements

- **Performance**
  - Handle very large accounts (400k files / 500GB class) without functional block.
  - Keep UI responsive via event batching and background processing.
- **Scalability (local)**
  - Support up to 5 accounts with serial scheduling and durable queues.
- **Reliability**
  - Robust retry/backoff for API throttling.
  - Crash-safe queue and sync state persistence.
- **Security & Privacy**
  - Column-level encryption for sensitive metadata/secrets.
  - No file-content encryption in MVP (explicitly accepted).
  - GDPR consent gating for telemetry.
- **Compliance**
  - Audit logging with event-specific retention and per-account configuration.
- **Usability**
  - Simple defaults for basic users; advanced visibility for power users.
- **Portability**
  - Cross-platform delivery; no platform-specific launch dependencies.

## Acceptance Criteria

- Users can connect up to 5 accounts and set exactly one as primary.
- System blocks overlapping local roots (same path or parent/child overlap) across accounts.
- Users can select folders (not files) for sync; all available folders are listed with no auto-selection.
- Primary account cannot be manually re-synced before 15 minutes since prior completion.
- Secondary accounts cannot be manually/automatically synced more often than hourly.
- Auto-sync-enabled accounts start syncing at app launch and execute serially.
- UI shows active account status transitions as queue advances.
- Offline state is clearly indicated; selection changes are disabled while offline.
- Local changes queue durably while offline and resume on reconnect.
- Conflict flow always requires user decision, supports item-level handling and “apply to all”.
- Version history retention enforces: 2 versions indefinite + up to 7 within 30 days.
- Graph `429` is handled with retry/backoff and no silent failure.
- Duplicate Graph items do not corrupt state; last-update-wins is applied deterministically.
- DB write batching uses chunks of 50 updates.
- Audit events capture all required event classes with retention:
  - Errors + consent: 30 days.
  - Others: 7 days.
  - Retention configurable per account.
- Telemetry is off by default until consent is provided; launch telemetry includes errors/crashes.
- Launch includes Light, Dark, Hacker themes with functional switching.
- Product ships with `en-GB` user-facing content.

## Dependencies & Constraints

- Microsoft Graph API via Kiota V5.
- SQLite + EF Core with `IEntityTypeConfiguration<T>` patterns.
- EF migrations applied at app startup.
- Functional programming conventions via `AStar.Dev.Functional.Extensions`.
- TDD process with RED/GREEN/REFACTOR and review checkpoints.
- Single-developer bandwidth and 3-month launch window.

## Risks & Mitigations

- **Risk 1: Delivery speed (single dev, broad scope)**
  - Mitigation: strict MVP cut line, vertical slices, test-first increments, defer non-core telemetry and advanced integrations.
- **Risk 2: Adoption**
  - Mitigation: optimise first-run setup, prioritise reliability/status clarity, provide clear value in multi-account + selective sync workflows.
- **Risk 3: Scope creep**
  - Mitigation: freeze launch scope, gate new features behind post-MVP roadmap, enforce PR/TDD phase discipline.

## Next Steps for Architect

- Produce architecture options for serial sync orchestration and queue durability.
- Define data schema for folder identity mapping, conflict records, version metadata, and account-scoped retention policies.
- Finalise retry/idempotency strategy for Graph delta processing and duplicate events.
- Specify UI state model for account list sync statuses and offline restrictions.
- Decide implementation approach for diagnostics layout candidate (fourth layout).
- Define localisation expansion pattern for UI + help content in 3–6 month roadmap.
- Create ADRs for:
  - Sync scheduling policy enforcement.
  - Conflict resolution workflow.
  - Encryption provider/key lifecycle (MVP vs post-MVP).
  - Logging/telemetry consent and data minimisation.

## Appendix — Technical Foundation Reference

This requirements document is grounded in the project technical guidance in .github/copilot-instructions.md, including:
- C# 14 + Avalonia.
- Vertical slice architecture.
- Functional programming patterns (`Option`, `Result`, `Either`).
- SQLite/EF Core conventions and natural-key-first modelling.
- Mandatory TDD with review/commit checkpoints.
