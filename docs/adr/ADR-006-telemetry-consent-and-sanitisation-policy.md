# ADR-006: Telemetry consent, event taxonomy, and sanitisation policy

- Status: Proposed
- Date: 2026-03-02
- Owner: Dev Team (single developer)
- Review By: End of Week 10

## Context

Telemetry must be GDPR-compliant and consent-gated, off by default, and limited at launch to errors and crashes. PII and secret leakage must be prevented.

## Decision Drivers

- GDPR compliance from day one.
- Minimal launch complexity.
- Future extensibility for phased telemetry.

## Options Considered

1. Consent-state model with strict event allowlist (`Error`, `Crash`) and sanitisation pipeline.
2. Broad telemetry collection with opt-out.
3. No telemetry pipeline in MVP.

## Decision

Adopt option 1:
- Persist explicit consent state and change history.
- Block all telemetry emission unless consent is active.
- Permit only `Error` and `Crash` events in MVP.
- Apply mandatory sanitisation before sink submission.

## Consequences

### Positive
- Clear compliance posture.
- Reduced privacy and scope risk.
- Straightforward to verify.

### Negative
- Less operational insight at launch.
- Requires explicit event governance for future expansion.

## Risks and Mitigations

- Risk: accidental non-allowlisted event emission.
  - Mitigation: compile-time event registry and tests.
- Risk: PII in exception payloads.
  - Mitigation: sanitisation redaction rules and rejection logging.

## Validation

- Tests proving zero telemetry when consent is absent.
- Tests proving allowlist enforcement.
- Sanitisation tests for token/email/path redaction.

## Related

- docs/requirements.md
- docs/implementation-plan.md
