
# Testing Guidelines

- Use xUnit V3 for all tests.
- Follow TDD: write tests that express behaviour before implementing the solution.
- Tests must be deterministic, isolated, and intention‑revealing.
- Use Arrange / Act / Assert with blank-line separation of sections, NOT comments.
- Prefer descriptive test names: MethodName_Should_Expected_When_Condition.
- Cover edge cases, null paths, and error conditions.
- Avoid mocking unless necessary; prefer real objects or simple fakes.
- Keep tests small, focused, and NEVER test implementation details, only observable behaviour.


