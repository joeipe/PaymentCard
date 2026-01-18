# PaymentCard

## 🌐 API Endpoint

Once running locally, the base API endpoint is:
https://localhost:7123/swagger/index.html

## 🔐 JWT Development Tokens (Reference)

For **local development**, this project uses the built-in `dotnet user-jwts` tooling to generate and manage JWT tokens.

### Create a Development JWT Token

Run the following command:

```bash
dotnet user-jwts create --audience pc-api
