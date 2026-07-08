using Microsoft.AspNetCore.Mvc.ModelBinding;
using MoodboardAI.Api.Models;

namespace MoodboardAI.Api.Extensions;

/// <summary>
/// Extension helpers for building standardized <see cref="ErrorResponse"/>
/// instances from ASP.NET Core model validation state.
/// </summary>
/// <remarks>
/// Centralizes the "first validation error message" logic that was
/// previously duplicated across controllers so every endpoint returns
/// validation errors in the exact same shape.
/// </remarks>
public static class ModelStateExtensions
{
    private const string DefaultMessage = "Invalid request.";

    /// <summary>
    /// Extracts the first model validation error message, if any.
    /// </summary>
    public static string GetFirstErrorMessage(this ModelStateDictionary modelState)
    {
        return modelState.Values
            .SelectMany(value => value.Errors)
            .Select(error => error.ErrorMessage)
            .FirstOrDefault(message => !string.IsNullOrWhiteSpace(message))
            ?? DefaultMessage;
    }

    /// <summary>
    /// Builds a standardized <see cref="ErrorResponse"/> from the first model validation error.
    /// </summary>
    public static ErrorResponse ToErrorResponse(this ModelStateDictionary modelState)
    {
        return new ErrorResponse { Message = modelState.GetFirstErrorMessage() };
    }
}
