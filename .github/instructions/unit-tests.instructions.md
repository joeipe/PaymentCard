---
applyTo: "src/PaymentCard.UnitTests/**/*.cs"
---

# Unit Test Conventions
- These rules apply only to the `PaymentCard.UnitTests` project.
- Use xUnit (`[Fact]`, `[Theory]`) as the test framework.
- Use Moq for mocking dependencies such as repositories, `IUnitOfWork`, and `ILogger<T>`. Use `NullLogger<T>` or `NullLoggerFactory` when logging doesn't need to be verified.
- Prefer FluentAssertions for assertions.
- Follow the Arrange-Act-Assert pattern, using `// Arrange`, `// Act`, and `// Assert` comments.
- Name test classes `{ClassName}Should` (e.g., `CurrencyServiceShould`) so that tests read as sentences.
- Place tests in folders that mirror the feature and layer being tested (e.g., `Cards/CommandHandlers`, `Services`).
- Each test class should cover a single class under test.
- Name test methods using `{Scenario}_{ExpectedResult}` or `{MethodName}_{ExpectedResult}_{Condition}` (e.g., `UpdateNonExistingCard_ShouldNotCallSave`, `GetExchangeRatesAsync_Throws_OnServerError`).
- Name the object under test `sut`.
- Create mocks in the test class constructor or as fields, and set up only the behavior each test needs.
- Verify interactions with `Mock.Verify` when a side effect matters (e.g., `SaveAsync` is called or not called).
- Use a hand-written `HttpMessageHandler` such as `TestMessageHandler` to test HTTP clients instead of mocking `HttpClient`.
- Cover the success path and the not-found, empty, and error paths for each handler or service.