# Task Management API

A RESTful API for task management with authentication and authorization.

---

## Tech Stack

* ASP.NET Core Web API (.NET 10)
* Entity Framework Core
* PostgreSQL
* JWT Authentication
* Serilog (logging)
* Swagger (API documentation)

---

## Features

### Authentication

* User registration
* Login with JWT token generation
* Secure password hashing

### Authorization

* Role-based access control (`user`, `admin`)
* Endpoint protection with `[Authorize]`
* Data access restrictions based on user identity

### Task Management

* Full CRUD operations
* Tasks are assigned to users
* Pagination
* Filtering
* Sorting

### User Management (Admin only)

* Get all users
* Get user by ID / email
* Update user data

### Additional

* Global exception handling middleware
* Structured logging with Serilog
* Swagger UI with JWT authentication support

---

## Architecture

The project follows a layered architecture:

* **Controllers** – handle HTTP requests
* **Services** – contain business logic
* **Data (DbContext)** – database access
* **DTOs** – separate API contracts from domain models
* **Mappings** – convert entities to DTOs
* **Middleware** – global error handling

Dependency Injection is used across the application.

---

## Authentication in Swagger

1. Call `POST /api/auth/login`
2. Copy the returned JWT token
3. Click **Authorize** in Swagger UI
4. Paste:

```text
Bearer <your_token>
```

---

## How to Run

```bash
dotnet restore
dotnet build
dotnet run
```

## Run with Docker

```bash
docker compose up --build
```

## Swagger UI:

```text
http://localhost:xxxx/swagger
```

---

## Roles

| Role  | Permissions                       |
| ----- | --------------------------------- |
| user  | Access only own tasks             |
| admin | Access all tasks and manage users |

---

## Key Highlights

* Clean separation of concerns (Controller / Service / Data)
* Secure authentication with JWT
* Role-based authorization
* Efficient database querying using `IQueryable`
* Production-like logging and error handling

---

## What This Project Demonstrates

* Backend API development with ASP.NET Core
* Database design and ORM usage (EF Core)
* Authentication & authorization
* Clean architecture principles
* Real-world API features (pagination, filtering, sorting)

---