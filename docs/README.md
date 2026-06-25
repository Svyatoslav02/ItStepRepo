# ItStepRepo

## Overview
This repository contains the source code and documentation for the **MoodBoard**.  
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
#### Run the backend

   ```bash
   cd backend
   dotnet restore
   dotnet run --project src/MoodboardAI.Api
   ```
---
### Frontend
---
#### Navigate to frontend/
#### Install dependencies and run:
```bash
npm install
npm run dev
```



