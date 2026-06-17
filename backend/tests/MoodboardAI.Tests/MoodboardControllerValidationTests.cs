using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MoodboardAI.Api.Controllers;
using MoodboardAI.Api.Models;
using Xunit;

namespace MoodboardAI.Tests;

public class MoodboardControllerValidationTests
{
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
