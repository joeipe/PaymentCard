# Generate Service Class

Generate a service class for the specified domain area.

## Inputs
- Domain name: {DomainName}
- Follow any entity or feature details provided in the request.

## Requirements
- Name the class `{DomainName}Service`.
- Place it in the project layer responsible for its work, following the project structure:
  - Application services and use cases: `PaymentCard.Application`
  - Domain logic: `PaymentCard.Domain`
  - Database access: `PaymentCard.Data`
  - External integrations: `PaymentCard.Infrastructure`
- Inspect and use the existing entity classes relevant to the domain. Do not invent entities, methods, or business rules that are not supported by the codebase or request.
- Include only methods appropriate to the domain and consistent with nearby code.
- Add XML documentation comments to all public methods.
- Use block-scoped namespaces, preserve nullable reference type support, and use the `Async` suffix for asynchronous methods.
- Follow the existing naming, formatting, error-handling, and dependency patterns. Avoid introducing new architectural patterns.
- Refer to an existing service as a pattern only if its file can be resolved in the workspace.