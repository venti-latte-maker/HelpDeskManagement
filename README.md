# HelpDeskManagement

A simple help desk ticketing system built as a two-project ASP.NET Core solution:

- **HelpDesk.Api** — REST API backed by Entity Framework Core / SQL Server, exposing CRUD operations for tickets.
- **HelpDesk.Mvc** — ASP.NET Core MVC front end that consumes the API to list, view, create, edit, delete, and filter tickets.
- **HelpDesk.Tests** — test project.

## Solution structure

```
HelpDeskManagement/
├── HelpDesk.Api/
│   ├── Controllers/TicketController.cs   # REST endpoints
│   ├── Models/Ticket.cs                  # EF entity
│   ├── Models/TicketDbContext.cs         # DbContext
│   ├── Repositories/                     # ITicketRepository / TicketRepository
│   └── Migrations/                       # EF Core migrations
├── HelpDesk.Mvc/
│   ├── Controllers/TicketController.cs   # MVC actions (Index, Details, Create, Update, Delete, Status)
│   ├── Models/TicketCreateEditViewModel.cs
│   ├── Services/TicketService.cs         # Typed HttpClient wrapper around the API
│   └── Views/Ticket/                     # Index, Create, Update, Details, Delete, Status
└── HelpDesk.Tests/
```

## Prerequisites

- .NET SDK (matching the target framework in the `.csproj` files)
- SQL Server (local or remote) reachable with the connection string in `HelpDesk.Api/appsettings.json`

## Configuration

Update the connection string in `HelpDesk.Api/appsettings.json` to point at your own SQL Server instance:

```json
"ConnectionStrings": {
  "TicketConnection": "Server=<your-server>; Database=TicketDb; Integrated Security = true; TrustServerCertificate = true"
}
```

Then apply migrations from `HelpDesk.Api`:

```
dotnet ef database update
```

## Running the app

Both projects must run at the same time — the MVC app calls the API over HTTP.

**Visual Studio:** Solution → Properties → set *Multiple startup projects* → set both `HelpDesk.Api` and `HelpDesk.Mvc` to *Start*, using the plain **http** launch profile for the API (not IIS Express — IIS Express uses different, dynamically assigned ports that won't match what the MVC app is configured to call).

**CLI:** run each project in its own terminal:

```
dotnet run --project HelpDesk.Api
dotnet run --project HelpDesk.Mvc
```

By default:
- `HelpDesk.Api` listens on `http://localhost:5214`
- `HelpDesk.Mvc` listens on `http://localhost:5271`

If your API starts on a different port, update the base address in `HelpDesk.Mvc/Program.cs`:

```csharp
builder.Services.AddHttpClient<TicketService>(
    c => c.BaseAddress = new Uri("http://localhost:5214/api/Ticket/"));
```

## API endpoints (`HelpDesk.Api`)

| Method | Route                     | Description                          |
|--------|---------------------------|---------------------------------------|
| GET    | `/api/Ticket/All`         | Get all tickets                       |
| GET    | `/api/Ticket/{id}`        | Get a ticket by id                    |
| GET    | `/api/Ticket/Status/{status}` | Get tickets filtered by status    |
| POST   | `/api/Ticket`             | Create a ticket                       |
| PUT    | `/api/Ticket/{id}`        | Update a ticket                       |
| DELETE | `/api/Ticket/{id}`        | Delete a ticket                       |

Valid `Status` values: `Open`, `In Progress`, `Closed`
Valid `Priority` values: `Low`, `Medium`, `High`

## MVC features

- List all tickets, view details, create, edit, and delete (with confirmation).
- Filter tickets by status via the `Status` action/view.
- Client- and server-side validation on the ticket form.

## Known limitations

- No authentication/authorization — all endpoints and pages are open.
- The API connection string is environment-specific and must be set per machine.
- Status and priority values are validated against a fixed, hardcoded list rather than a configurable enum/table.
