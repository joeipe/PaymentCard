# PaymentCard

## 🌐 API Endpoint

Once running locally, the base API endpoint is:
https://localhost:7123/swagger/index.html

## 💱 Exchange Rates Data Source

This API retrieves currency exchange rates using the **U.S. Treasury Reporting Rates of Exchange** dataset provided by Fiscal Data (U.S. Department of the Treasury).

**Source URL (HTTP Client):**  
https://fiscaldata.treasury.gov/datasets/treasury-reporting-rates-exchange/treasury-reporting-rates-of-exchange

> This endpoint is consumed by the internal HttpClient to fetch official reference exchange rates for currency conversion and financial calculations.

## 🔐 JWT Development Tokens (Reference)

For **local development**, this project uses the built-in `dotnet user-jwts` tooling to generate and manage JWT tokens.

### Create a Development JWT Token

Run the following command:

```bash
dotnet user-jwts create --audience pc-api
