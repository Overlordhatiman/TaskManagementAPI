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
