using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MoodboardAI.Api.Controllers;
using MoodboardAI.Api.Models;
using Xunit;

namespace MoodboardAI.Tests;

/// <summary>
/// Unit tests that validate the behavior of the <see cref="MoodboardController"/> model validation and responses.
/// </summary>
public class MoodboardControllerValidationTests
{

    /// <summary>
    /// Creates a MoodboardController instance and applies validation errors from the specified request to ModelState.
    /// </summary>
    /// <param name="request">The request model to validate.</param>
    /// <returns>A MoodboardController instance with populated ModelState validation errors.</returns>
    private static MoodboardController CreateController(MoodboardRequest request)
    {
        var controller = new MoodboardController();
        var context = new ValidationContext(request);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(request, context, results, validateAllProperties: true);

        foreach (var result in results)
        {
            foreach (var memberName in result.MemberNames)
            {
                controller.ModelState.AddModelError(memberName, result.ErrorMessage ?? "Invalid value.");
            }
        }

        return controller;
    }

    /// <summary>
    /// Ensures an empty prompt causes model validation to fail and returns a BadRequest with an error message.
    /// </summary>
    [Fact]
    public void Generate_EmptyPrompt_ReturnsBadRequest()
    {
        var request = new MoodboardRequest { Prompt = "" };
        var controller = CreateController(request);

        var result = controller.Generate(request);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        var error = Assert.IsType<ErrorResponse>(badRequest.Value);
        Assert.False(string.IsNullOrWhiteSpace(error.Message));
    }

    /// <summary>
    /// Ensures a prompt that is too short triggers validation and returns a BadRequest with an error message.
    /// </summary>
    [Fact]
    public void Generate_TooShortPrompt_ReturnsBadRequest()
    {
        var request = new MoodboardRequest { Prompt = "ab" };
        var controller = CreateController(request);

        var result = controller.Generate(request);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        var error = Assert.IsType<ErrorResponse>(badRequest.Value);
        Assert.False(string.IsNullOrWhiteSpace(error.Message));
    }

    /// <summary>
    /// Ensures a valid prompt returns an Ok response with a MoodboardResponse containing the same prompt.
    /// </summary>
    [Fact]
    public void Generate_ValidPrompt_ReturnsOk()
    {
        var request = new MoodboardRequest { Prompt = "Cozy autumn cottage in the woods" };
        var controller = CreateController(request);

        var result = controller.Generate(request);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<MoodboardResponse>(okResult.Value);
        Assert.Equal(request.Prompt, response.Prompt);
    }
}
