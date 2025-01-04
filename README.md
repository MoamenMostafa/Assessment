# Full-Stack Application with .NET Core and Angular

## Overview
This project is a full-stack application built using .NET Core for the backend and Angular for the frontend, leveraging PrimeNG components for UI design. The backend is containerized with Docker and uses a SQL Server database and Redis cache, all orchestrated through Docker Compose.

## Features
- **Backend:**
  - .NET Core Web API for endpoints.
  - Implemented a clean architecture pattern with separate layers:
    - **API Layer:** Handles incoming requests and routes them to appropriate services.
    - **Business Layer:** Contains business logic and service implementations.
    - **Data Access Layer:** Interfaces with the SQL Server database.
  - Integrated Redis as a caching mechanism to enhance performance.

- **Frontend:**
  - Built with Angular.
  - Utilized PrimeNG components for a responsive and modern UI.
  - Supports full CRUD operations with the backend API.

- **Containerization:**
  - Dockerized the backend, redis cache and database.
  - Used Docker Compose to run multiple containers (backend, SQL Server, and Redis) in the same network.

## Additional Features
- **SQL Queries:**
  - Includes SQL files with queries for populating dummy data into the database tables.

- **PowerShell Task:**
  - A PowerShell script is provided to streamline development:
    - Starts the Docker Compose setup.
    - Automatically opens the Angular application in the default browser.

## Technology Stack
- **Backend:**
  - .NET Core 8.0+ Web API
  - SQL Server (via Docker container)
  - Redis (via Docker container)

- **Frontend:**
  - Angular
  - PrimeNG for UI components

- **Containerization:**
  - Docker
  - Docker Compose

## Project Setup

### Prerequisites
Ensure you have the following installed on your machine:
- Docker and Docker Compose
- Node.js and npm
- .NET SDK (8.0 or later)

### Backend Setup
1. Navigate to the backend project directory.
2. Build the Docker image for the backend:
   ```bash
   docker build -t backend-api .

### Screenshots

Backend API in Swagger

Frontend - Products List

Frontend - Categories List
