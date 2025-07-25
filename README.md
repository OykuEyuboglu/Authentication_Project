# 🔐 Authentication Project (.NET 8 Web API)

This project is an ASP.NET Core Web API implementation that provides JWT-based user authentication and authorization.

## 🚀 Features

- User Registration (/register)
- User Login (/login)
- JWT token generation and validation
- Retrieve authenticated user information (/getperson)
- List all registered users (/allusers)
- Token verification and expiration handling
- Secure password hashing (PBKDF2 + HMACSHA512)
- Data annotations for validation ([Required], [EmailAddress], [MinLength])
- ModelState-based error handling

## 🧩 Project Layers

- `DTOs`: Data Transfer Objects for input/output models
- `Services`: Business logic and token management
- `Handlers`: Custom password hashing logic
- `Controllers`: API endpoints
- `Contexts`: Entity Framework Core DbContext
- `Program.cs`: DI, JWT, and middleware configuration

## ⚙️ Technologies

- ASP.NET Core 8.0
- Entity Framework Core
- SQL Server
- JWT (Json Web Token)
- Swagger (for API testing and documentation)
- Testable with Curl / Postman

## 🔐 API Endpoints

| Endpoint | Description | Authorization |
|----------|----------|--------|
| `POST /api/Auth/register` | Registers a new user | Public |
| `POST /api/Auth/login` | Authenticates user and returns JWT | Public |
| `GET /api/Auth/getperson` | Returns current authenticated user | Requires Auth |
| `GET /api/Auth/allusers` | Lists all registered users | Requires Auth |

## 🛠 Setup Instructions

- Clone or download the repository.
- Open appsettings.json and configure:
              - Jwt:Key, Issuer, Audience
              - ConnectionStrings for your SQL Server
- Run database migrations (if applicable):
              - dotnet ef database update
- Run the project (Ctrl + F5 or dotnet run)
- Use Swagger UI for interactive API testing.

## 🧪 Testing (with Curl)

curl -X POST "https://localhost:7163/api/Auth/login" ^
 -H "Content-Type: application/json" ^
 -d "{\"email\":\"user1@gmail.com\", \"password\":\"user11234\"}"

 You can also use Postman or similar tools for full testing support.
