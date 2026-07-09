# Standardized Error Responses

All API errors — validation failures, authentication/authorization failures,
not-found routes, and unhandled server errors — are returned using the same
JSON shape:

```json
{
  "message": "Human readable description of what went wrong."
}
```

This is the `ErrorResponse` model (`Models/ErrorResponse.cs`).

## Status codes

| Status | When it's returned                                                          |
| ------ | ---------------------------------------------------------------------------- |
| 400    | The request body failed model validation (e.g. missing/invalid fields).      |
| 401    | The request is missing a valid `Authorization: Bearer <token>` header, or the token is invalid/expired, on an endpoint that requires authentication. |
| 403    | The caller is authenticated but not allowed to perform the action.           |
| 404    | The requested route does not match any endpoint.                             |
| 500    | An unhandled exception occurred while processing the request.                |

## Implementation notes

* Controllers build validation error responses via the shared
  `ModelState.ToErrorResponse()` extension (`Extensions/ModelStateExtensions.cs`)
  instead of duplicating the "first error message" logic in each controller.
* `Middleware/ExceptionHandlingMiddleware.cs` catches any unhandled exception
  and converts it into a 500 `ErrorResponse`, logging the original exception
  for diagnostics without leaking internal details to the client.
* JWT authentication failures (missing/invalid/expired token) and
  authorization failures are mapped to `ErrorResponse` via
  `JwtBearerEvents.OnChallenge` / `OnForbidden` in `Program.cs`, so
  `[Authorize]`-protected endpoints never return ASP.NET Core's default
  empty 401/403 body.
* Unmatched routes fall back to a standardized 404 `ErrorResponse` via
  `app.MapFallback(...)` in `Program.cs`.
