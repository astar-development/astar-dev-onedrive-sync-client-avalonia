# ADR-005: Encryption provider abstraction and key lifecycle

- Status: Proposed
- Date: 2026-03-02
- Owner: Dev Team (single developer)
- Review By: End of Week 5

## Context

MVP requires column-level encryption for sensitive metadata/secrets (emails, names, tokens), while cached file content encryption and advanced key rotation are out of scope. Architecture must still support future key lifecycle improvements without schema redesign.

## Decision Drivers

- GDPR-aligned protection for sensitive fields.
- Backward-compatible extension path for post-MVP rotation.
- Low implementation complexity for MVP.

## Options Considered

1. `IEncryptionProvider` abstraction with system-managed key implementation for MVP.
2. Direct crypto calls in EF value converters with no abstraction.
3. Full key-management subsystem in MVP.

## Decision

Adopt option 1:
- Define `IEncryptionProvider` as application boundary.
- Encrypt/decrypt sensitive columns through provider-backed converters.
- Keep key management system-managed for MVP.
- Reserve key version metadata fields for future rotation.

## Consequences

### Positive
- Meets MVP security scope.
- Avoids lock-in to one cryptography mechanism.
- Enables future rotation with limited migration impact.

### Negative
- Slight upfront abstraction cost.
- Post-MVP rotation still requires operational design.

## Risks and Mitigations

- Risk: accidental plaintext logging.
  - Mitigation: central sanitisation middleware and logging tests.
- Risk: provider misuse bypassing encryption.
  - Mitigation: repository-level guardrails and integration tests on persisted ciphertext.

## Validation

- Tests asserting encrypted-at-rest storage for target columns.
- Negative tests for token leakage in logs.
- Round-trip encryption/decryption tests.

## Related

- docs/requirements.md
- docs/implementation-plan.md
