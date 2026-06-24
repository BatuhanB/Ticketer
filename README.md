# Ticketer

Ticketer is a .NET 10 solution implementing a clean architecture for ticket management. The solution is split into separate projects for domain, application logic, infrastructure concerns, and presentation (UI/API).

## Solution structure

- `Ticketer.Domain` — Domain entities, value objects and domain logic.
- `Ticketer.Application` — Application services, use cases, DTOs and interfaces used by higher layers.
- `Ticketer.Infrastructure` — Implementations for persistence, external services and cross-cutting concerns (EF Core, repositories, data access).
- `Ticketer.Presentation` — Web API or UI project that hosts the application (the runtime entry point).

Each project has its own `.csproj` at the repository root and targets .NET 10.

## Requirements

- .NET 10 SDK (https://dotnet.microsoft.com)
- Optional: Docker (for running databases or supporting services)

## Build and run

From the repository root:

1. Restore and build:

   ```bash
   dotnet restore
   dotnet build -c Release
   ```

2. Run the application (presentation project is the entry point):

   ```bash
   cd Ticketer.Presentation
   dotnet run
   ```

Check console output for the hosted URLs. If the project exposes Swagger, it is usually available at `/swagger`.

## Database and configuration

- Configuration is loaded from `appsettings.json` in `Ticketer.Presentation` and can be overridden with environment variables.
- If EF Core is used and migrations live in `Ticketer.Infrastructure`, apply migrations with:

  ```bash
  dotnet ef database update --project Ticketer.Infrastructure --startup-project Ticketer.Presentation
  ```

  Adjust the `--project` and `--startup-project` options if migrations or startup project are located elsewhere.

## Tests

If test projects exist in the solution run:

```bash
dotnet test
```

## Contributing

- Fork, create a feature branch, add tests, and open a PR with a clear description.
- Keep changes small and focused.

## CI / CD

Pipelines should at minimum run `dotnet restore`, `dotnet build`, and `dotnet test`. Configure any secrets (connection strings, API keys) through your CI provider.

## License

Add a `LICENSE` file to the repository root and specify the chosen license here.

If you want examples (sample requests, environment variables, seed scripts) or want the README tailored with specific endpoints or setup steps for a local DB, tell me which details to include and I will update the file.