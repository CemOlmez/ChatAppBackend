
# ChatApp Backend

A real-time chat backend built with ASP.NET Core, Entity Framework, PostgreSQL, SignalR, and Redis. This service supports secure authentication, private/group messaging, file uploads, and WebSocket-based real-time communication.

---
Demo (deployed on Render):[BACKEND DEMO](https://chatappbackend-gz8t.onrender.com/swagger)

## Features

- JWT-based authentication with refresh token rotation
- Private 1:1 messaging (auto-creates DM group if missing)
- Group creation and user management
- File upload (max 10MB to `wwwroot/uploads`)
- Soft-delete and update messages
- Full-text search within groups
- Redis caching for message history
- Real-time messaging using SignalR
- RESTful API with Swagger documentation

---

## Project Structure

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
│   └── Cache/               # RedisCacheService
└── README.md
```

---

## Tech Stack

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

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/)
- PostgreSQL
- Redis

---

### 1. Clone the Repository

```bash
git clone https://github.com/your-username/ChatApp.git
```

---

### 2. Configure `appsettings.json` (Optional)

This project includes default values that match the `docker-compose.yml` setup.

If you're using Docker Compose, you do **not** need to modify `appsettings.json`.

Only change this if:
- You want to connect to a custom PostgreSQL or Redis server.
- You're not using Docker for your local setup.

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

### 3. Run with Docker Compose

## Local Deployment with Docker

1. **Clone the Repository**  
   ```bash
   git clone https://github.com/your-username/ChatApp.git

   ```

2. **Ensure Docker is Installed & Running**  
   [Install Docker](https://www.docker.com/products/docker-desktop/)

3. **Run with Docker Compose**  
   ```bash
   docker-compose up --build
   ```

4. **Access the API via Swagger:**  
   [http://localhost:8080/swagger](http://localhost:8080/swagger)

---

## Cloud Deployment (Render)

> The app is already deployed and accessible here:  
> 🔗 **[Live Swagger](https://chatappbackend-gz8t.onrender.com/swagger/index.html)**

To deploy your own version:

1. **Push the project to GitHub**  
2. **Create a new Web Service on [Render](https://render.com/)**  
   - Connect your GitHub repo
   - Choose "Docker" deployment
   - Set the root directory and build context to `.`

3. **Add Environment Variables in Render Dashboard**

| Key                          | Value                         |
|------------------------------|-------------------------------|
| `ConnectionStrings__DefaultConnection` | your PostgreSQL connection string |
| `Redis__ConnectionString`   | your Redis connection string   |
| `Jwt__Key`                  | your JWT secret key            |
| `Jwt__Issuer`               | your app issuer                |
| `Jwt__Audience`             | your app audience              |

4. **Deploy** and access the app from your Render subdomain.

---

## API Usage

### Auth

| Endpoint                   | Method | Description                     |
|---------------------------|--------|---------------------------------|
| `/api/auth/register`      | POST   | Register a new user             |
| `/api/auth/login`         | POST   | Login and receive tokens        |
| `/api/auth/refresh-token` | POST   | Get new JWT via refresh token   |
| `/api/auth/logout`        | POST   | Revoke refresh token            |

### Messages

| Endpoint                               | Method | Description                                                         |
| -------------------------------------- | ------ | ------------------------------------------------------------------- |
| `/api/messages`                        | POST   | Send a message to a group or user (auto-creates DM if not exists)   |
| `/api/messages/upload`                 | POST   | Upload a file (max 10MB), returns public file URL                   |
| `/api/messages/group/{groupId}`        | GET    | Fetch paginated messages for a group (default: page=1, pageSize=50) |
| `/api/messages/group/{groupId}/search` | GET    | Search messages in a group by keyword                               |
| `/api/messages/{id}`                   | PUT    | Edit a message's content by message ID                              |
| `/api/messages/{id}`                   | DELETE | Soft delete a message by ID                                         |


### Groups

| Endpoint                               | Method | Description                                     |
| -------------------------------------- | ------ | ----------------------------------------------- |
| `/api/groups`                          | POST   | Create a new group (optionally with users)      |
| `/api/groups`                          | GET    | Get all groups                                  |
| `/api/groups/{id}`                     | GET    | Get a group by its ID                           |
| `/api/groups/{groupId}/users`          | GET    | Get all users in a specific group               |
| `/api/groups/{groupId}/users/{userId}` | POST   | Add a user to a specific group (if not present) |


### Users

| Endpoint                       | Method | Description                                          |
| ------------------------------ | ------ | ---------------------------------------------------- |
| `/api/users/all`               | GET    | Get a list of all users                              |
| `/api/users/{userId}/contacts` | GET    | Get contact list (users with DM groups) for a user   |
| `/api/users/add-contact`       | POST   | Add a contact (creates DM group if it doesn't exist) |

---

## CI/CD Overview

CI/CD was not fully implemented due to time constraints and because this was my first time working with cloud deployment and pipelines. While GitHub Actions or similar tools could be integrated for automatic build and deployment, the current project relies on manual deployment via Docker and Render. 

## Testing

Testing wasn't added in this version due to time limitations and my current learning stage. I've not worked with testing frameworks like xUnit or NUnit yet, but I plan to implement proper unit and integration tests in future iterations as I learn more about them. The current focus was to get the backend working end-to-end with real features and cloud deployment.

## Assumptions & Tradeoffs

- Used local file storage for file uploads to simplify the setup. In production, cloud storage (e.g. AWS S3, Azure Blob) should be used.
- SignalR is set up for real-time messaging but is not yet load-balanced or scaled. In production, a backplane (e.g. Redis or Azure SignalR) is needed.
- JWT secret is hardcoded for local use only. In production, secrets should be securely stored (e.g. in environment variables or secret managers).
- No frontend UI was developed — this is a backend-only project.
- No automated testing or CI/CD pipelines were set up — focus was on functionality and deployment.
- Docker Compose was used locally for PostgreSQL and Redis. On Render, those services need to be hosted separately or connected via cloud services.

---

