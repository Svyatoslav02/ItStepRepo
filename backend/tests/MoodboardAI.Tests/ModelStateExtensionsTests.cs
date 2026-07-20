using Microsoft.AspNetCore.Mvc.ModelBinding;
using MoodboardAI.Api.Extensions;
using Xunit;

namespace MoodboardAI.Tests;

/// <summary>
/// Unit tests for <see cref="ModelStateExtensions"/>, which standardizes how
/// validation errors are turned into an <see cref="MoodboardAI.Api.Models.ErrorResponse"/>
/// across all controllers.
/// </summary>//test
public class ModelStateExtensionsTests
{
    [Fact]
    public void ToErrorResponse_WithValidationError_ReturnsFirstMessage()
    {
        var modelState = new ModelStateDictionary();
        modelState.AddModelError("Prompt", "Prompt is required.");
        modelState.AddModelError("Prompt", "Prompt must be at least 3 characters long.");

        var errorResponse = modelState.ToErrorResponse();

        Assert.Equal("Prompt is required.", errorResponse.Message);
    }

    [Fact]
    public void ToErrorResponse_WithNoErrors_ReturnsDefaultMessage()
    {
        var modelState = new ModelStateDictionary();

        var errorResponse = modelState.ToErrorResponse();

        Assert.Equal("Invalid request.", errorResponse.Message);
    }

    [Fact]
    public void ToErrorResponse_AcrossMultipleFields_ReturnsFirstFieldsFirstError()
    {
        var modelState = new ModelStateDictionary();
        modelState.AddModelError("InterestIds", "At least 3 interests are required.");
        modelState.AddModelError("SomeOtherField", "Some other error.");

        var errorResponse = modelState.ToErrorResponse();

        Assert.Equal("At least 3 interests are required.", errorResponse.Message);
    }
}
