using System.ComponentModel.DataAnnotations;
using MoodboardAI.Api.DTOs.Interests;
using Xunit;

namespace MoodboardAI.Tests;

/// <summary>
/// Unit tests that validate the data annotation rules on
/// <see cref="SaveUserInterestsRequestDto"/>, in particular the
/// "at least 3 interests" requirement for POST /api/users/me/interests.
/// </summary>
public class SaveUserInterestsRequestDtoValidationTests
{
    private static List<ValidationResult> Validate(SaveUserInterestsRequestDto request)
    {
        var context = new ValidationContext(request);
        var results = new List<ValidationResult>();

        Validator.TryValidateObject(request, context, results, validateAllProperties: true);

        return results;
    }

    [Fact]
    public void EmptyInterestIds_FailsValidation()
    {
        var request = new SaveUserInterestsRequestDto { InterestIds = new List<Guid>() };

        var results = Validate(request);

        Assert.NotEmpty(results);
    }

    [Fact]
    public void TwoInterestIds_FailsValidation()
    {
        var request = new SaveUserInterestsRequestDto
        {
            InterestIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
        };

        var results = Validate(request);

        Assert.NotEmpty(results);
    }

    [Fact]
    public void ThreeInterestIds_PassesValidation()
    {
        var request = new SaveUserInterestsRequestDto
        {
            InterestIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() }
        };

        var results = Validate(request);

        Assert.Empty(results);
    }

    [Fact]
    public void MoreThanThreeInterestIds_PassesValidation()
    {
        var request = new SaveUserInterestsRequestDto
        {
            InterestIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() }
        };

        var results = Validate(request);

        Assert.Empty(results);
    }
}
