# Task Management API

A simple task management API built with .NET Core 8.0, using SQL Server as the database and Entity Framework Core (Code-First). The project is fully containerized using Docker and features automated unit testing, continuous integration, and deployment using GitHub Actions.

## 🛠️ Technologies Used

- **Framework**: ASP.NET Core 8.0
- **Database**: SQL Server (via Docker)
- **ORM**: Entity Framework Core (Code-First)
- **Testing**: Unit Tests with Moq & xUnit
- **Containerization**: Docker & Docker Compose
- **API Documentation**: Swagger
- **CI/CD**: GitHub Actions

## ⚙️ Features

- **CRUD Operations**: Create, Read, Update, and Delete tasks.
- **Filtering**: Filter tasks by status, priority, and due date.
- **Pagination**: Paginated results for task lists.
- **Error Handling**: Robust logging for error tracking.

## 🚀 Setup & Running

### Prerequisites
- Docker
- .NET SDK 8.0+

### Steps

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/TaskManagementAPI.git
   cd TaskManagementAPI
2. Build and start the Docker containers:
   ```bash
   docker-compose up --build
3. Access the API documentation (Swagger):
   ```bash
   http://localhost:8080/swagger
4. To stop the containers:
   ```bash
   docker-compose down
   
## 🧪 Running Unit Tests

1. Navigate to the test folder:
   ```bash
   cd TaskManagementAPI.Tests
2. Run the tests:
   ```bash
   dotnet test

## 🐳 Docker Commands

Start & Build: `docker-compose up --build`
Stop Containers: `docker-compose down`
View Running Containers: `docker ps`
Clean Stopped Containers: `docker container prune`

## 🧑‍💻 CI/CD with GitHub Actions

This project includes a GitHub Actions workflow for continuous integration and deployment.
Workflow Summary:

  1. Build the application.
  2. Run unit tests.
  3. If successful, deploy the application.

The workflow is located at .github/workflows/ci.yml.

## Possible Improvements

While the Task Management API is functional, there are several architectural improvements and best practices that can be implemented to enhance its modularity, maintainability, and scalability. Here are a few suggestions:

### 1. Separation of Concerns
A clearer separation of concerns helps in decoupling different parts of the application and makes it easier to maintain and scale.

#### a. **Data Layer**:
Move all database-related operations to a dedicated **Data Layer** project. This will include:
- **Repositories**: Interfaces and their implementations for handling data operations.
- **Data Context**: EF Core DbContext and migrations should live here.
  
#### b. **Service Layer**:
The **Service Layer** can hold business logic and validations, making it easier to modify business rules without touching the API or data layer. This also simplifies unit testing by allowing mocks of both data and external service dependencies.

### 2. DTO Layer for API Contracts
Currently, DTOs and models are mixed. A separate **DTO Layer** can handle all data transfer objects, which act as contracts between the API and the consumers.

This way, you decouple the internal models (used by EF Core or domain logic) from the external API contracts, allowing for more flexible changes in internal logic without breaking the API.

### 3. Implement Dependency Injection for Better Testability
You are already using Dependency Injection (DI) via constructor injection. Consider formalizing a **Dependency Injection layer** where all service and repository injections are handled. This will help in managing dependencies more effectively.


### 4. Configuration Settings
Move sensitive and environment-specific configurations like connection strings, JWT secrets, and other sensitive information to **environment variables** using `IConfiguration` and `appsettings.json` with better organization.

- Use Azure Key Vault, AWS Secrets Manager, or Docker secrets for **production-grade security**.
- Use `.env` files for local development.

### 5. Asynchronous Programming and Performance
Ensure all database operations and heavy-lifting processes are properly handled asynchronously to improve performance and scalability.

- Review all service calls to ensure proper use of `async/await`.
- Implement **caching** (e.g., Redis) to optimize performance for frequently accessed data.

### 6. API Versioning and OpenAPI Standards
- Implement **API versioning** to future-proof your API and avoid breaking changes when modifying endpoints.
- Leverage **OpenAPI/Swagger** more deeply by documenting different versions and customizing responses for various status codes.

### 7. Unit Tests and Integration Tests
Increase test coverage, especially for edge cases and error scenarios:
- **Unit Tests**: Ensure all services and repositories are properly unit tested, particularly business logic.
- **Integration Tests**: Mock the database to create isolated integration tests.

*Example Tools*: 
- **xUnit** or **NUnit** for unit testing.
- **Moq** for mocking dependencies.
  
You could create a dedicated testing project:

### 8. Containerization Improvements
- Split Dockerfiles for **development** and **production** environments, optimizing builds using multi-stage builds.
- Implement **Docker Healthchecks** to ensure services like SQL Server are fully up before launching dependent services.

### 9. Continuous Integration/Continuous Deployment (CI/CD)
Your current CI/CD pipeline can be enhanced:
- Implement **test coverage reports** as part of the build.
- Set up **automatic deployment** to a cloud environment (e.g., Azure, AWS) once tests pass.

*Example GitHub Action Improvements*:
- Add `codecov` integration for test coverage.
- Set up stages for **test**, **build**, and **deploy**.

### 10. Logging and Monitoring
Implement advanced logging and monitoring for production:
- Use **structured logging** (e.g., Serilog) with integrations like **Elasticsearch** for centralized logging.
- Integrate **Application Insights** or **Prometheus** for monitoring.

---

These improvements will help make the project more modular, testable, scalable, and maintainable. 



