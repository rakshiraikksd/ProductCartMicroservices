# Microservices Project with YARP API Gateway

This project demonstrates a microservices architecture with three services and an API Gateway using YARP (Yet Another Reverse Proxy).

## Architecture

- **API Gateway** (Port 5000): YARP-based reverse proxy that routes requests to appropriate microservices
- **Product Service** (Port 5001): Manages product information
- **Catalog Service** (Port 5002): Manages product categories
- **Cart Service** (Port 5003): Manages shopping cart functionality

## Services Overview

### Product Service
- **Base URL**: `http://localhost:5001/api/product`
- **Endpoints**:
  - `GET /` - Get all products
  - `GET /{id}` - Get product by ID
  - `POST /` - Create new product
  - `PUT /{id}` - Update product
  - `DELETE /{id}` - Delete product
  - `GET /health` - Health check

### Catalog Service
- **Base URL**: `http://localhost:5002/api/catalog`
- **Endpoints**:
  - `GET /` - Get all categories
  - `GET /{id}` - Get category by ID
  - `POST /` - Create new category
  - `PUT /{id}` - Update category
  - `DELETE /{id}` - Delete category (soft delete)
  - `GET /search?query={query}` - Search categories
  - `GET /health` - Health check

### Cart Service
- **Base URL**: `http://localhost:5003/api/cart`
- **Endpoints**:
  - `GET /{userId}` - Get user's cart
  - `POST /{userId}/items` - Add item to cart
  - `PUT /{userId}/items/{itemId}` - Update cart item
  - `DELETE /{userId}/items/{itemId}` - Remove item from cart
  - `DELETE /{userId}` - Clear cart
  - `GET /{userId}/summary` - Get cart summary
  - `GET /health` - Health check

### API Gateway
- **Base URL**: `http://localhost:5000`
- **Routes**:
  - `/api/product/*` → Product Service
  - `/api/catalog/*` → Catalog Service
  - `/api/cart/*` → Cart Service

## Prerequisites

- Docker Desktop
- .NET 8.0 SDK (for local development)

## Running with Docker Compose

1. **Build and run all services**:
   ```bash
   docker-compose up --build
   ```

2. **Run in background**:
   ```bash
   docker-compose up -d --build
   ```

3. **Stop all services**:
   ```bash
   docker-compose down
   ```

## Local Development

1. **Build the solution**:
   ```bash
   dotnet build
   ```

2. **Run individual services**:
   ```bash
   # Product Service
   cd ProductService
   dotnet run

   # Catalog Service
   cd CatalogService
   dotnet run

   # Cart Service
   cd CartService
   dotnet run

   # API Gateway
   cd ApiGateway
   dotnet run
   ```

## Testing the Services

### Via API Gateway (Recommended)
```bash
# Get all products
curl http://localhost:5000/api/product

# Get all categories
curl http://localhost:5000/api/catalog

# Get user cart
curl http://localhost:5000/api/cart/user123

# Add item to cart
curl -X POST http://localhost:5000/api/cart/user123/items \
  -H "Content-Type: application/json" \
  -d '{"productId": 1, "productName": "Laptop", "price": 999.99, "quantity": 1}'
```

### Direct Service Access
```bash
# Product Service
curl http://localhost:5001/api/product

# Catalog Service
curl http://localhost:5002/api/catalog

# Cart Service
curl http://localhost:5003/api/cart/user123
```

## Swagger Documentation

Each service includes Swagger documentation:
- API Gateway: http://localhost:5000/swagger
- Product Service: http://localhost:5001/swagger
- Catalog Service: http://localhost:5002/swagger
- Cart Service: http://localhost:5003/swagger

## Health Checks

All services include health check endpoints:
- API Gateway: http://localhost:5000/api/health
- Product Service: http://localhost:5001/api/product/health
- Catalog Service: http://localhost:5002/api/catalog/health
- Cart Service: http://localhost:5003/api/cart/health

## Docker Configuration

Each service is containerized with:
- Multi-stage builds for optimized images
- .NET 8.0 runtime
- Proper port exposure
- Health check endpoints

## Network Configuration

- All services communicate via Docker network `microservices-network`
- API Gateway routes requests using YARP configuration
- Services are accessible both individually and through the gateway

## Project Structure

```
├── Microservices.sln
├── docker-compose.yml
├── README.md
├── ApiGateway/
├── ProductService/
├── CatalogService/
└── CartService/
```

Each service follows the same structure:
- `Controllers/` - API controllers
- `Models/` - Data models
- `Program.cs` - Application entry point
- `appsettings.json` - Configuration
- `Dockerfile` - Container definition


