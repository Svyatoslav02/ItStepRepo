# Environment Variables

This document describes all environment variables used in the
**Moodboard AI** project. It covers both the backend (ASP.NET Core)
and the frontend (React) parts of the application.

> This document reflects the **initial** set of variables at the
> start of the project. As development progresses, the list may grow —
> please keep this file and `.env.example` in sync with any new
> variables you introduce.

---

## Setting up your local environment

1. Copy the example file to create your own `.env` file:

   ```bash
   cp .env.example .env
   ```

2. Open `.env` and fill in your own values (see the sections below for
   where to obtain each key).

3. Make sure `.env` is **not** committed to git — it must be listed in
   `.gitignore` (see the "Security" section below).

4. Restart the application (backend and/or frontend) so the new
   values are picked up.

> **How the backend picks up `.env`:** the ASP.NET Core app loads it
> automatically on startup — it searches the current working directory
> and its parents for a `.env` file (see `LoadDotEnvFile()` in
> `backend/src/MoodboardAI.Api/Program.cs`), so it works whether you run
> `dotnet run` from the repo root, `backend/`, or the project folder
> itself. No manual `export`/shell sourcing is required. Real OS/CI
> environment variables always take priority over `.env` values. See
> [`database-setup.md`](./database-setup.md) for the database-specific
> part of this workflow.

---

## Backend (ASP.NET Core)

### `OPENAI_API_KEY`

- **Description:** access key for the OpenAI API (or a compatible
  provider such as Groq), used to generate AI-based suggestions and
  descriptions for moodboards.
- **Where to get it:**
    - OpenAI: create an account at [platform.openai.com](https://platform.openai.com),
      go to *API keys* → *Create new secret key*.
    - Groq: create an account at [console.groq.com](https://console.groq.com),
      go to *API Keys*.
- **Format:** a string, usually starting with `sk-...`
- **Required:** yes — the AI features will not work without this key.

### `OPENAI_API_BASE_URL` *(optional)*

- **Description:** the base URL of the API. Needed only if you use an
  alternative provider (for example, Groq, which exposes an
  OpenAI-compatible API but with a different endpoint).
- **Example values:**
    - OpenAI (default): `https://api.openai.com/v1`
    - Groq: `https://api.groq.com/openai/v1`

### `UNSPLASH_ACCESS_KEY`

- **Description:** access key for the Unsplash API, used to search for
  and fetch images for moodboards.
- **Where to get it:**
    1. Sign up at [unsplash.com/developers](https://unsplash.com/developers).
    2. Create a new Application.
    3. Copy the **Access Key** field (not the Secret Key).
- **Format:** alphanumeric string.
- **Required:** yes — image search will not work without it.

### `CONNECTIONSTRINGS__DEFAULTCONNECTION`

- **Description:** connection string for the PostgreSQL database.
- **Format:**
  ```
  Host=localhost;Port=5432;Database=moodboard_ai;Username=postgres;Password=your_password
  ```
- **Note:** in ASP.NET Core, the double underscore `__` represents
  nesting in `appsettings.json` (`ConnectionStrings:DefaultConnection`).
  This lets environment variables override values from
  `appsettings.json`.
- **Required:** yes.

### `ASPNETCORE_ENVIRONMENT`

- **Description:** specifies the runtime environment of the
  application.
- **Possible values:** `Development`, `Staging`, `Production`.
- **Required:** should be set explicitly (defaults to `Production` if
  not set).

### `JWT_SECRET` *(if JWT authentication is used)*

- **Description:** secret key used to sign JWT authentication tokens.
- **Format:** a long random string (32+ characters recommended).
- **How to generate:**
  ```bash
  openssl rand -base64 32
  ```
- **Required:** yes, if user authentication is implemented.

---

## Frontend (React)

> In Vite/Create React App projects, variables exposed to the client
> must be prefixed with `VITE_` (Vite) or `REACT_APP_` (CRA),
> depending on the build tool used.

### `VITE_API_BASE_URL`

- **Description:** base URL of the backend API that the frontend
  communicates with.
- **Example value:** `http://localhost:5000/api` (for local
  development), or the production URL after deployment.
- **Required:** yes.

---

## Security

- **Never** commit a `.env` file containing real values to git.
  The `.env` file must be listed in `.gitignore`.
- Only `.env.example` — a template with placeholder values and no real
  secrets — should be stored in the repository.
- If a real secret (API key, password, connection string) was
  accidentally committed:
    1. **Revoke** that key immediately in the relevant service (OpenAI,
       Unsplash, etc.) and generate a new one.
    2. Remove the secret from the git history (e.g. using
       `git filter-repo` or BFG Repo-Cleaner), not just from the latest
       commit.
- Production secrets should be stored separately from the codebase —
  using the hosting provider's secret manager (e.g. server environment
  variables, Azure Key Vault, GitHub Actions Secrets), not in files
  inside the repository.
- Each team member should use their own (test/personal) API keys for
  local development — do not share a single key over chat or other
  unsecured channels.
