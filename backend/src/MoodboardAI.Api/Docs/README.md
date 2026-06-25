# Backend

## Overview
The backend is built with **ASP.NET Core** and provides RESTful APIs for the MoodBoard project.

## Structure
backend/
├── Controllers/   # API controllers
├── Models/        # Entity models
├── Services/      # Business logic services
├── DTOs/          # Data transfer objects
└── Program.cs     # Application entry point


## Setup
1. Navigate to the backend folder:
    ```bash
    cd backend
    ```
2. Restore dependencies:
    ```bash
    dotnet restore 
    ```
3. Run migrations:
    ```bash
    dotnet ef database update
    ```
4. Start the server:
    ```bash
   dotnet run
    ```


