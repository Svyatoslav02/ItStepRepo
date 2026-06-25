# Backend

## Overview
The backend is built with **ASP.NET Core** and provides RESTful APIs for the MoodBoard project.

## Structure
MoodboardAI.Api/
├── Controllers/   # API controllers
├── Models/        # Models (requests/responses)
├── Services/      # Business logic services
├── Docs/          # API docs and contracts
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


