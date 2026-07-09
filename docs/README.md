# ItStepRepo

## Overview
This repository contains the source code and documentation for **MoodboardAI**.
It includes backend services, frontend application, and environment configuration files.

## Project Structure
ItStepRepo/
├── backend/                  # .NET solution and backend docs
│   └── src/MoodboardAI.Api/  # ASP.NET Core Web API (controllers, models, services)
├── frontend/
│   └── moodboard-frontend/   # Frontend application (UI, components, pages)
├── docs/                     # Documentation (environment setup, guides)
└── README.md                 # Root documentation

## Getting Started
1. Clone the repository:
   ```bash
   git clone https://github.com/Svyatoslav02/ItStepRepo.git
   ```
2. Navigate to the project folder:

   ```bash
   cd ItStepRepo
   ```

### Backend
---
#### Set up the database

The backend requires a PostgreSQL database and your own local
connection string — see [`docs/database-setup.md`](./database-setup.md)
for the full step-by-step guide (connecting, migrations, troubleshooting).

#### Run the backend

   ```bash
   cd backend
   dotnet restore
   dotnet run --project src/MoodboardAI.Api
   ```
---
### Frontend
---
#### Run the frontend

   ```bash
   cd frontend/moodboard-frontend
   npm install
   npm run dev
   ```



