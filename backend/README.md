This repository contains the initial structure for the **ItStepRepo** project.  
It includes folder for backend to support collaborative development.

# MoodboardAI — Backend

Initial backend project skeleton for the **MoodboardAI** platform.

## Project structure
- **backend/** – server-side logic and API implementation  

```
backend/
├── MoodboardAI.sln
├── src/
│   └── MoodboardAI.Api/        # ASP.NET Core Web API project (Swagger enabled)
└── tests/
    └── MoodboardAI.Tests/      # xUnit test project
```

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

## How to run locally

1. Restore dependencies:

   ```bash
   cd backend
   dotnet restore
   ```

2. Build the solution:

   ```bash
   dotnet build
   ```

3. Run the API project:

   ```bash
   dotnet run --project src/MoodboardAI.Api
   ```

4. Open Swagger UI in your browser (address printed in the console, by default):

   ```
   http://localhost:5246/swagger
   ```

## How to run tests

```bash
dotnet test
```
