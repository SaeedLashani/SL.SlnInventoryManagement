# SL.InventoryManagement

A production-ready Inventory Management System built with Clean Architecture and CQRS pattern, deployed on Azure with automated CI/CD pipeline.

## Tech Stack

**Backend**
- .NET 8, ASP.NET Core
- Clean Architecture, CQRS, MediatR
- FluentValidation, AutoMapper

**Database & Storage**
- PostgreSQL + Entity Framework Core
- Redis (Caching)
- Elasticsearch (Full-text search)

**Security**
- JWT Bearer Authentication
- BCrypt password hashing

**DevOps & Cloud**
- Docker, Docker Compose
- Azure Container Apps
- Azure Container Registry
- Azure Database for PostgreSQL
- Azure Cache for Redis
- GitHub Actions CI/CD

**Testing**
- xUnit, Moq, FluentAssertions

## Architecture

```
SL.InventoryManagement/
├── Domain          — Entities, business rules
├── Application     — CQRS, Commands, Queries, Validators
├── Infrastructure  — JWT, Redis, Elasticsearch
├── Persistence     — EF Core, Repositories, Migrations
└── Presentation
    └── API         — Controllers, Middleware
```

## Getting Started

Run with Docker Compose:
```bash
docker-compose -f docker-compose.prod.yml up --build
```

API available at: `http://localhost:8080/swagger`

Run locally (VS):
```bash
docker-compose up -d postgres redis elasticsearch
# Then F5 in Visual Studio
```

## API Endpoints

### Auth
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | /api/auth/register | Register new user |
| POST | /api/auth/login | Login and get JWT token |

### Products
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | /api/products | Create product |
| GET | /api/products | Get all (cached) |
| GET | /api/products/{id} | Get by ID (cached) |
| GET | /api/products/search?query= | Search via Elasticsearch |
| PATCH | /api/products/{id}/stock-in | Add stock |
| PATCH | /api/products/{id}/stock-out | Remove stock |

## CI/CD Pipeline

Every push to master branch:
1. Build Docker image
2. Push to Azure Container Registry
3. Deploy to Azure Container Apps

## Live Demo

`https://ca-inventory.whitebeach-0cd7c11f.germanywestcentral.azurecontainerapps.io/swagger`