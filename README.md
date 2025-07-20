#  ChatApp Backend

Real-time chat application built with ASP.NET Core, Entity Framework, PostgreSQL, SignalR, and Redis. Supports private/group messaging, file uploads, and JWT-based authentication.

---

## Project Structure

ChatApp/
│
├── ChatApp.API              # ASP.NET Core Web API (Controllers, Hubs)
│   ├── Controllers/
│   ├── DTOs/                # API-specific DTOs (e.g., FileUploadDTO)
│   ├── Hubs/                # SignalR Hub
│   └── Swagger/             # Swagger config
│
├── ChatApp.Application      # Application layer (interfaces, core DTOs)
│   ├── Interfaces/
│   └── DTOs/
│       ├── User/
│       ├── Group/
│       └── Message/
│
├── ChatApp.Domain           # Domain models (Entities)
│
├── ChatApp.Infrastructure   # EF DbContext, Services, Redis cache
│   ├── DbContext/
│   ├── Services/
│   └── Persistence/         # RedisCacheService
│
└── README.md

 ## Features:
✅ JWT Authentication

✅ Private 1:1 messaging (auto-creates DM group if missing)

✅ Group creation & management

✅ Message search & soft-delete

✅ File upload (10MB max)

✅ Redis caching for message history

✅ SignalR integration for real-time messaging

✅ RESTful API with Swagger UI

## Technologies
Area	Stack
Backend	ASP.NET Core Web API
Real-Time	SignalR
Database	PostgreSQL + Entity Framework
Caching	Redis (StackExchange.Redis)
Auth	JWT Bearer
File Storage	Local file system (wwwroot/uploads)
Docs	Swagger


## Setup

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/)
- PostgreSQL
- Redis


 ## 1.Clone the Repo

git clone https://github.com/your-username/ChatApp.git
cd ChatApp

## 2.Set Up PostgreSQL

Create a database and update appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=ChatDb;Username=postgres;Password=yourpassword"
}

## 3.Set Up Redis

Update in appsettings.json:
"Redis": {
  "ConnectionString": "localhost:6379"
}

Configure JWT
Add to appsettings.json:
"Jwt": {
  "Key": "your-secret-key",
  "Issuer": "your-app",
  "Audience": "your-app-users"
}



## API Usage

## Auth

| Endpoint                  | Method | Description                     |
| ------------------------- | ------ | ------------------------------- |
| `/api/auth/register`      | POST   | Register a new user             |
| `/api/auth/login`         | POST   | Login and receive tokens        |
| `/api/auth/refresh-token` | POST   | Refresh JWT using refresh token |
| `/api/auth/logout`        | POST   | Revoke refresh token            |

## Messages

## Groups

## Users

## CI/CD overview(can be epty for now)

## Depolyment steps

## Assumptions and tradeoffs