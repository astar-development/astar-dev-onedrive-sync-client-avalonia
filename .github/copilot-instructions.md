# Project Guidelines

## Overview

Cross-platform OneDrive Sync desktop application supporting multiple OneDrive accounts, built with Avalonia UI 11.3.+ and C# 14. This project follows vertical slice architecture and functional programming paradigms with strict TDD requirements.

## Code Style

### Language and Framework
- **C# 14** - Use all modern C# features (file-scoped namespaces, primary constructors, collection expressions, etc.)
- **Avalonia** - Cross-platform UI framework for all views
- **Functional Programming** - Use `AStar.Dev.Functional.Extensions` nuget package; prefer functional patterns over OOP
- **Immutability** - Use `record` and `record struct` for DTOs, value objects, and immutable data structures
- **Entities** - Database entities MUST be `class` types (not records)

### Type Conventions
```csharp
// Immutable data transfer objects
public record UserSettingsDto(string Theme, string Locale, bool AutoSync);

// Value objects
public record struct AccountId(Guid Value);

// Entities (classes only)
public class SyncAccount 
{
    public string Email { get; init; } = string.Empty;
    public DateTimeOffset LastSync { get; set; }
}
```

## Architecture

### Vertical Slice Architecture
- Organize features by business capability, not technical layer
- Each slice contains everything needed for that feature (UI, logic, data access, tests)
- Slice structure: `Features/{FeatureName}/{Commands|Queries|Views|Models|Tests}`
- Avoid cross-slice dependencies; use domain events or mediator pattern for communication

### Database Design
- **SQLite** with **EF Core**
- **Natural keys** preferred (e.g., email address for accounts)
- **GUIDs** only when natural keys don't exist - MUST be shadow properties
- Entity configurations via `IEntityTypeConfiguration<T>` in separate files
- Migrations applied automatically at app startup

Example entity configuration:
```csharp
public class SyncAccountConfiguration : IEntityTypeConfiguration<SyncAccount>
{
    public void Configure(EntityTypeBuilder<SyncAccount> builder)
    {
        builder.HasKey(x => x.Email); // Natural key
        builder.Property<Guid>("Id").ValueGeneratedOnAdd(); // Shadow property
    }
}
```

### Microsoft Graph API
- **Kiota V5** for all Microsoft Graph API access
- Centralize API client configuration in `Infrastructure/GraphApi/`
- Support multiple authenticated sessions for multi-account scenarios

## Testing

### Test-Driven Development (TDD)
- **MANDATORY**: Write tests BEFORE implementation
- Test file naming: `{ClassName}Should.cs` or `{FeatureName}Should.cs`
- **xUnit V3** with **Microsoft Testing Platform**
- **Shouldly** for assertions (fluent, readable assertion syntax)
- **NSubstitute** for mocking - use sparingly, only when mocking cannot be easily avoided
- Minimum coverage: unit tests for all business logic, integration tests for slices
- Prefer functional design that minimizes need for mocking

### Test Organization
- Place tests in separate test projects corresponding to features: `Features/{FeatureName}/Tests/`
- Separate unit tests from integration tests in separate projects

## Build and Test

```bash
# Restore dependencies
dotnet restore

# Build solution
dotnet build

# Run all tests
dotnet test

# Run application
dotnet run --project src/AStar.Dev.OneDrive.Sync.Client/AStar.Dev.OneDrive.Sync.Client.csproj

# Apply EF migrations (handled automatically at startup)
dotnet ef migrations add {MigrationName} --project src/Infrastructure
```

## Branching

- Development MUST be performed on a branch, never directly on `main`.
- Use branch prefixes by change type: `feature/`, `chore/`, `bug/`, `refactor/`, `docs/`, `test/`.
- Keep branch scope focused to a single feature/fix to support small, reviewable PRs.

## TDD / PR Process

- **TDD is mandatory**: follow RED → GREEN → REFACTOR for EVERY change.
- **RED review required**: write failing tests first and have tests reviewed before implementation.
- **Commit after RED review**: commit the approved failing tests before writing production code.
- **GREEN review required**: implement the minimal code needed to make tests pass, then request review.
- **Commit after GREEN review**: commit passing implementation and tests once approved.
- **REFACTOR review required**: refactor only after GREEN approval, keep tests passing throughout.
- **Commit after REFACTOR review**: commit refactors separately after approval.
- Keep PRs aligned with this sequence so reviewers can validate intent and progression.

## PR and Commit Naming

- PR titles MUST start with type prefixes: `feature:`, `chore:`, `bug:`, `refactor:`, `docs:`, `test:`.
- PR descriptions SHOULD include explicit sections for `RED`, `GREEN`, and `REFACTOR` changes.
- Use separate commits per TDD phase to preserve reviewability (`test:` for RED, type-specific for GREEN, `refactor:` for REFACTOR).
- For GREEN commits, use `bug(...)` when fixing broken behavior and `feature(...)` when delivering net-new behavior.
- Commit messages should be short, imperative, and scope-first (example: `feature(sync): add account sync command handler`).
- Never combine unrelated slices/features in a single commit or PR.

### PR Description Template

```markdown
## Summary
- {What this PR changes}

## RED
- Tests added in failing state:
- Review feedback addressed:
- Commit(s):

## GREEN
- Minimal implementation to make RED tests pass:
- Review feedback addressed:
- Commit(s):

## REFACTOR
- Refactors performed with tests still green:
- Review feedback addressed:
- Commit(s):

## Validation
- `dotnet build`
- `dotnet test`

## Notes
- {Risks, follow-ups, or TBD items}
```

### Commit Sequence Example

```text
test(sync): add failing tests for account sync command handler
feature(sync): implement account sync command handler to satisfy tests
refactor(sync): simplify sync pipeline and remove duplication
```

## Project Conventions

### UI Layouts
Application supports 4 distinct layouts:
1. **Dashboard** - Overview of sync status, accounts, recent activity
2. **Explorer** - File/folder browsing interface
3. **Terminal** - Command-line style interface for advanced operations
4. **TBD** - Fourth layout to be determined

### Theming
7 theme variants must be supported:
- Light
- Dark
- Auto (follows system theme)
- Colourful
- High Contrast
- Professional
- Hacker

Theme switching must be real-time without app restart. Store theme preference in SQLite.

### Globalization/Localization
- Initial language: **en-GB** (British English)
- Use resource files (`.resx`) or JSON-based localization
- All user-facing strings MUST be localizable
- Date/time formats: use `CultureInfo` with en-GB settings
- Prepare infrastructure for additional languages

### GDPR Compliance
- User data stored locally (SQLite) with encryption for sensitive data
- Implement data export functionality (user can download all their data)
- Implement data deletion (right to be forgotten)
- Log user consent for data processing
- No telemetry without explicit user consent

### Functional Programming Patterns
Use `AStar.Dev.Functional.Extensions`:
- `Option<T>` instead of nulls where appropriate
- `Result<T>` or `Result<T, TError>` for operation outcomes
- `Either<TLeft, TRight>` for discriminated unions
- Avoid exceptions for control flow; prefer Result types
- Use pattern matching extensively

Example:
```csharp
public async Task<Result<SyncResult, SyncError>> SyncAccountAsync(AccountId accountId)
{
    return await GetAccount(accountId)
        .Map(account => ValidateAccount(account))
        .BindAsync(account => PerformSync(account))
        .Match(
            success => Result.Success<SyncResult, SyncError>(success),
            error => Result.Failure<SyncResult, SyncError>(error)
        );
}
```

### Multi-Account Support
- Users can authenticate and manage multiple OneDrive accounts simultaneously
- Each account has independent sync configuration
- UI must clearly distinguish which account is active in context
- Store account tokens securely (use OS credential manager where possible)

## Security

### Authentication
- OAuth 2.0 via Microsoft Graph API (handled by Kiota)
- Store refresh tokens encrypted in SQLite database
- Implement token refresh logic before expiry

### Sensitive Data
- **Column-level encryption** for sensitive fields (email addresses, real names, access tokens, refresh tokens)
- Never log access tokens, refresh tokens, or user file contents
- Sanitize logs for GDPR compliance
- Use EF Core value converters or custom encryption providers for encrypted columns

## Dependencies

### Core NuGet Packages
- `Avalonia` - UI framework
- `Microsoft.Kiota.Bundle` (v5.x) - Graph API client
- `Microsoft.EntityFrameworkCore.Sqlite` - Database
- `AStar.Dev.Functional.Extensions` - Functional programming
- `Microsoft.Extensions.DependencyInjection` - DI container
- `xunit.v3` - Testing framework
- `Microsoft.Testing.Platform` - Test execution platform
- `Shouldly` - Assertion library
- `NSubstitute` - Mocking library (use sparingly)

### Project Structure (Initial)
```
src/
  AStar.Dev.OneDrive.Sync.Client/                   # Main Avalonia application
  AStar.Dev.OneDrive.Sync.Client.Core/              # Domain models, interfaces
  AStar.Dev.OneDrive.Sync.Client.Infrastructure/    # EF Core, Graph API, file system
  Features/                                         # Vertical slices
test/
  AStar.Dev.OneDrive.Sync.Client.Tests.Unit/        # Test projects mirror src structure
  AStar.Dev.OneDrive.Sync.Client.Tests.Integration/ # Integration tests
```

## Notes for AI Agents

- Test data should be realistic and cover edge cases, not just happy paths. Builder pattern can be used for complex test data setup / shared test data.
- Prefer immutability - Use `record` or `record struct` unless it's an EF entity
- Avoid nulls - Use `Option<T>` from functional extensions
- No primitive obsession - Wrap primitives in value objects (e.g., `AccountId`, `FilePath`)
- GDPR awareness - Consider data privacy in EVERY feature
- Multi-account context - Always consider which account is active
- Theme-aware UI - Use Avalonia theme bindings, never hardcode colors
- Localize all strings - No hardcoded user-facing text

## Technology Decisions

- **Testing**: xUnit V3 + Microsoft Testing Platform + Shouldly + NSubstitute (sparingly)
- **DI Container**: Microsoft.Extensions.DependencyInjection
- **Encryption**: Column-level for sensitive data (emails, names, tokens)
- **Token Storage**: SQLite with encrypted columns
- **Fourth UI Layout**: TBD (placeholder for future feature)

## Future Considerations

- Determine fourth UI layout based on user needs and usage patterns
- Evaluate OS credential manager integration if cross-device token sync is required
- Consider additional localization languages based on user demographics
