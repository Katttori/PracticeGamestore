# PracticeGamestore

PracticeGamestore is a sample project for managing a game store, built with ASP.NET Core. It demonstrates a layered architecture with API, Business, and DataAccess projects.

## Project Structure

- **PracticeGamestore.API/**: ASP.NET Core Web API project. Contains controllers, middlewares, filters, and API configuration.
- **PracticeGamestore.Business/**: Business logic layer. Contains services, DTOs, mappers, and business rules.
- **PracticeGamestore.DataAccess/**: Data access layer. Contains Entity Framework Core context, entities, repositories, and migrations.
- **PracticeGamestore.Tests/**: Unit and integration tests for the solution.

## API Endpoints Overview

The API exposes the following main endpoints (see Controllers directory for details):

- **/api/auth**: User authentication (login, register, token refresh)
- **/api/blacklist**: Manage blacklisted users
- **/api/country**: Country management (CRUD)
- **/api/file**: File upload and download
- **/api/game**: Game management (CRUD, search, filter)
- **/api/genre**: Genre management (CRUD)
- **/api/order**: Order management (CRUD, user orders)
- **/api/platform**: Platform management (CRUD)
- **/api/publisher**: Publisher management (CRUD)
- **/api/user**: User management (CRUD, profile, password)

Each endpoint supports standard RESTful operations (GET, POST, PUT, DELETE) where applicable. See controller source files for detailed request/response models and business logic.

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server (or update connection string for your DB)

### Setup
1. Clone the repository:
   ```sh
   git clone <repository-url>
   ```
2. Navigate to the API project:
   ```sh
   cd PracticeGamestore/PracticeGamestore.API
   ```
3. Restore dependencies:
   ```sh
   dotnet restore
   ```
4. Apply database migrations:
   ```sh
   dotnet ef database update
   ```
5. Run the API:
   ```sh
   dotnet run
   ```

The API will be available at `https://localhost:5001` by default.

## Features
- User authentication and authorization
- Game, genre, platform, publisher, and order management
- File uploads
- Custom middlewares and filters
- Validation and error handling

## Development
- Update `appsettings.json` for your environment.
- Add new features in the Business and DataAccess layers before exposing them via API controllers.
- Run tests in the `PracticeGamestore.Tests` project.