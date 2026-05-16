# RuleWay.ECommerce

RuleWay.ECommerce is a .NET Web API project developed as an e-commerce merchandising management test case.

The project includes product CRUD operations, category management, product filtering, validation, soft delete, and automatic product live status calculation.

## Features

- Product CRUD
- Category CRUD
- Product filtering by search keyword and stock range
- FluentValidation-based request validation
- Global validation filter
- Global exception handling with ProblemDetails
- Soft delete
- EF Core global query filters
- Computed `IsLive` column (calculated by the database)
- Scalar API Reference

## Technology Stack

- .NET 10
- C# 14
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server / LocalDB
- FluentValidation
- Scalar.AspNetCore

## Business Rules

- Product title is required.
- Product title maximum length is 200 characters.
- A product can have only one category.
- A product must have a category to be live.
- A product cannot be live if its stock quantity is lower than the category minimum stock quantity.
- Product filtering supports search keyword, minimum stock, and maximum stock.
- Search keyword checks product title, product description, and category name.

## Getting Started

### Prerequisites

- .NET SDK 10
- SQL Server LocalDB or SQL Server
- Visual Studio, VS Code, or Rider

### Clone the Repository

```sh
git clone https://github.com/RidvanOzturk/RuleWay.ECommerce.git
cd RuleWay.ECommerce
```

### Configure Database

The project uses SQL Server / LocalDB. Check the connection string in:

`RuleWay.ECommerce.Api/appsettings.json`

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=RuleWayECommerceDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

Replace it with your own SQL Server connection string if needed.

### Restore and Build

```sh
dotnet restore
dotnet build
```

### Apply Migrations

```sh
dotnet ef database update --project RuleWay.ECommerce.Infrastructure --startup-project RuleWay.ECommerce.Api
```

### Run the API

```sh
dotnet run --project RuleWay.ECommerce.Api
```

The API will start on the port shown in the console.

### Scalar API Reference

Open Scalar in your browser:

`https://localhost:<port>/scalar/v1`

You can test all endpoints from Scalar.

### Seed Data

The project includes sample seed data.

Categories:

- Electronics
- Books

Products:

- iPhone 15
- Clean Code

## Project Structure

- RuleWay.ECommerce.Api — Controllers, filters, middleware, and API configuration.
- RuleWay.ECommerce.Application — DTOs, services, mappings, validators, abstractions, and application registration.
- RuleWay.ECommerce.Domain — Entities, common base classes, and custom exceptions.
- RuleWay.ECommerce.Infrastructure — DbContext, EF Core configurations, migrations, seed data, and infrastructure registration.

## Notes

- Controllers are kept thin; business logic is handled in services.
- Validation is handled globally with FluentValidation.
- Soft delete and audit fields are handled centrally in the DbContext.
- `IsLive` is calculated by the database and is not set by the client.
