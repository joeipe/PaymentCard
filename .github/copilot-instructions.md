# Copilot Instructions

## Project Conventions
- This is a .NET 10 solution containing an ASP.NET Core API and supporting class library projects.
- Follow the existing project structure and keep responsibilities in the appropriate layer:
  - `PaymentCard.API`: application startup, configuration, and HTTP endpoints.
  - `PaymentCard.Application`: use cases, query/command handlers, and application services.
  - `PaymentCard.Domain`: domain entities and domain logic.
  - `PaymentCard.Data`: database access and repositories.
  - `PaymentCard.Infrastructure`: external services and integrations.
  - `PaymentCard.Contracts`: types shared across application boundaries.
- Follow the existing patterns in the relevant project and feature. Avoid introducing new architectural patterns or broad refactors for a localized change.
- Use block-scoped namespaces, matching the existing C# files.
- Preserve nullable reference type support and use asynchronous APIs for I/O-bound operations.

## Naming Conventions
- Use descriptive PascalCase names for types, methods, and properties.
- Name service classes `{Domain}Service` (e.g., `CurrencyConversionService`).
- Name interfaces with an `I` prefix (e.g., `ICurrencyService`).
- Name domain entities in PascalCase with no suffix (e.g., `Card`).
- Use the `Async` suffix for asynchronous methods.
- Put extension methods in dedicated static classes named `{TypeName}Extensions` (e.g., `CardExtensions`).
- Use underscore-prefixed names for private fields (e.g., `_currencyService`).

## Documentation and Formatting
- Add XML documentation when needed for a public API or when requested; don't add it automatically to every public method.
- Match the surrounding code's formatting, naming, and error-handling patterns.
- Keep comments focused on non-obvious intent.