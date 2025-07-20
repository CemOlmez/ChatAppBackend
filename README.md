# ChatApp Backend

A real-time chat backend built with ASP.NET Core, Entity Framework, PostgreSQL, SignalR, and Redis. This service supports secure authentication, private/group messaging, file uploads, and WebSocket-based real-time communication.

---

## 🧠 Features

- ✅ JWT-based authentication with refresh token rotation
- ✅ Private 1:1 messaging (auto-creates DM group if missing)
- ✅ Group creation and user management
- ✅ File upload (max 10MB to `wwwroot/uploads`)
- ✅ Soft-delete and update messages
- ✅ Full-text search within groups
- ✅ Redis caching for message history
- ✅ Real-time messaging using SignalR
- ✅ RESTful API with Swagger documentation

---

## 📁 Project Structure

```
ChatApp/
├── ChatApp.API              # Web API (Controllers, SignalR Hubs)
│   ├── Controllers/
│   ├── DTOs/                # API-only DTOs like FileUploadDTO
│   ├── Hubs/
│   └── Swagger/
├── ChatApp.Application      # Application logic (interfaces, DTOs)
│   ├── Interfaces/
│   └── DTOs/
├── ChatApp.Domain           # Domain entities
├── ChatApp.Infrastructure   # EF, Services, Caching (Redis)
│   ├── DbContext/
│   ├── Services/
│   └── Cache/         # RedisCacheService
└── README.md
```

---

## 🛠️ Tech Stack

| Layer       | Tech                         |
|------------|------------------------------|
| Backend     | ASP.NET Core (Web API)        |
| Real-Time   | SignalR                       |
| Database    | PostgreSQL + Entity Framework |
| Caching     | Redis (StackExchange.Redis)  |
| Auth        | JWT + Refresh Tokens         |
| File Upload | Local file system             |
| Docs        | Swagger                       |
| Container   | Docker + Docker Compose       |
| CI/CD       | GitHub Actions (planned)      |

---

## 🚀 Getting Started

### ✅ Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/)
- PostgreSQL
- Redis

---

### 🧾 1. Clone the Repository

```bash
git clone https://github.com/your-username/ChatApp.git
cd ChatApp
```

---

### ⚙️ 2. Configure `appsettings.json`

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=ChatDb;Username=postgres;Password=yourpassword"
},
"Redis": {
  "ConnectionString": "localhost:6379"
},
"Jwt": {
  "Key": "your-secret-key",
  "Issuer": "your-app",
  "Audience": "your-app-users"
}
```

---

### 🐳 3. Run with Docker Compose

A `docker-compose.yml` file is included to run PostgreSQL, Redis, and the backend container.

```bash
docker-compose up --build
```

---

## 📡 API Usage

### 🔐 Auth

| Endpoint                   | Method | Description                     |
|---------------------------|--------|---------------------------------|
| `/api/auth/register`      | POST   | Register a new user             |
| `/api/auth/login`         | POST   | Login and receive tokens        |
| `/api/auth/refresh-token` | POST   | Get new JWT via refresh token   |
| `/api/auth/logout`        | POST   | Revoke refresh token            |

---

### 💬 Messages

| Endpoint                          | Method | Description                      |
|----------------------------------|--------|----------------------------------|
| `/api/messages`                  | POST   | Send a message (DM or group)     |
| `/api/messages/group/{groupId}`  | GET    | Fetch paginated group messages   |
| `/api/messages/{id}`             | PUT    | Edit a message                   |
| `/api/messages/{id}`             | DELETE | Soft delete a message            |
| `/api/messages/group/{groupId}/search` | GET | Search messages in group         |

---

### 👥 Groups

| Endpoint           | Method | Description              |
|--------------------|--------|--------------------------|
| `/api/groups`      | POST   | Create a new group       |
| `/api/groups`      | GET    | Get all groups           |
| `/api/groups/{id}` | GET    | Get group by ID          |

---

### 🙋 Users

| Endpoint                    | Method | Description                  |
|-----------------------------|--------|------------------------------|
| `/api/users/contacts`       | GET    | Get contact list             |
| `/api/users/add-contact`    | POST   | Add user to contact list     |
| `/api/users/{id}`           | GET    | Get user by ID               |

---

## ⚙️ CI/CD Overview

Planned CI/CD with **GitHub Actions**:

- ✅ Linting
- ✅ Testing with xUnit
- ✅ Build and deploy on push
- Future: container deploy to Azure / Railway

---

## ☁️ Deployment Steps

### 🧪 Docker Deployment (Local)
Docker Deployment (Local)
Docker Deployment (Local)

1. Clone the repo and navigate to the root directory.
2. Ensure Docker Desktop is running.
3. Build and Run the App:

```bash
docker-compose up --build
```

This will spin up:
PostgreSQL on port 5432
Redis on port 6379
ASP.NET Core API on http://localhost:8080

4. Access Swagger:: `http://localhost:8080/swagger`

### ☁️ VPS / Cloud Deployment

1. Set environment variables:
   - `DefaultConnection` string
   - `Redis:ConnectionString`
   - `Jwt` credentials
2. Ensure NGINX/SSL reverse proxy setup.
3. Publish `.NET` backend and host with Kestrel or Docker on:
   - Railway (simplest)
   - Azure App Service + Redis
   - AWS Lightsail + PostgreSQL + Redis

---

## 🧠 Assumptions & Tradeoffs

- ✅ Used local disk storage for simplicity (swap for S3/Blob in real deployment).
- ✅ No admin UI panel — API only.
- ✅ Redis used only for message cache; MongoDB not required in this phase.
- ✅ JWT secret hardcoded in development only.
- ✅ Focus was on clean, testable service logic, not frontend or mobile integration.
- ✅ SignalR is configured but can be scaled via Azure SignalR or Redis backplane if needed.

---

## 📧 Delivery

1. Pushed to public GitHub repo.
2. Link shared via email to `hr@bluesense.com`.
3. This README includes:
   - ✅ Setup
   - ✅ API usage
   - ✅ CI/CD Overview
   - ✅ Deployment steps
   - ✅ Assumptions

---

## ✅ Good luck reviewers!
Treat it like production — that’s what we did.
