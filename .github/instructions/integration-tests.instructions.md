---
applyTo: "src/PaymentCard.IntegrationTests/**/*.cs"
---

# Integration Test Conventions
- These rules apply only to the `PaymentCard.IntegrationTests` project.
- Use xUnit as the test framework and FluentAssertions for assertions.
- Test the API through HTTP calls using `HttpClient` created from `ApiWebApplicationFactory`; don't call application or data layers directly.
- Use `IClassFixture<ApiWebApplicationFactory>` and create the `HttpClient` in the test class constructor.
- Keep endpoint tests in the `Endpoints` folder, with one class per endpoint group named `{Feature}EndpointTests` (e.g., `CardsEndpointTests`).
- Name test methods `{HTTP_VERB}_{description_in_snake_case}` (e.g., `GET_get_cards`, `POST_create_card`).
- Follow the Arrange-Act-Assert pattern, using `// Arrange`, `// Act`, and `// Assert` comments.
- Use `[Theory]` with `[InlineData]` for endpoint URLs, matching the existing tests.
- Use request and response types from `PaymentCard.Contracts`. Deserialize responses with `JsonSerializerOptions { PropertyNameCaseInsensitive = true }`.
- Assert the HTTP status code and the response content where relevant.
- Don't use Moq in this project unless a dependency can't be exercised through the test host.