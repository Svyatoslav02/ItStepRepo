# API Standards

## CORS

Allowed origins are configured in `appsettings.json` under `Cors:AllowedOrigins`.

**Dev:** `http://localhost:5173` (Vite)  
**Prod:** to be added when deployed

Policy name: `FrontendPolicy`. Configured in `Program.cs`.

---

## Pagination

All list endpoints use the same query parameters and response shape.

**Query parameters:**
- `page` — 1-based page number (default: 1)
- `pageSize` — items per page (default: 10, max: 100)

**Response envelope:**
```json
{
  "items": [],
  "page": 1,
  "pageSize": 10,
  "totalCount": 143
}
```

Implemented via `PaginationQuery` DTO and `ToPagedResultAsync` extension method in `QueryableExtensions`.

---

## Rate Limiting

| Policy | Endpoints | Limit |
|---|---|---|
| `AuthPolicy` | `POST /api/auth/login`, `POST /api/auth/register` | 5 req/min |
| `SearchPolicy` | `GET /api/v1/search` | 30 req/min |

Rejected requests return `429 Too Many Requests` with standard `ErrorResponse` body.

**Rationale:** 5 req/min on auth protects against brute-force. 30 req/min on search prevents scraping.

---

## Error Format

All errors return the same JSON shape:

```json
{ "message": "Human-readable error description." }
```

Unhandled exceptions are caught by `ExceptionHandlingMiddleware` and return 500 with the same shape.

---

## API Versioning

All new endpoints are prefixed with `/api/v1/`. Existing endpoints (`/api/auth`, `/api/users`) remain unversioned for backward compatibility.

Future breaking changes should be introduced under `/api/v2/`.