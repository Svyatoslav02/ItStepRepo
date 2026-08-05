using System.ComponentModel.DataAnnotations;
using MoodboardAI.Api.Models;
using Xunit;

namespace MoodboardAI.Tests;

/// <summary>
/// Unit tests validating data annotation constraints on the Pin, Category,
/// and Tag entities used for the content feed and search domain.
/// </summary>
public class ContentDomainEntityConstraintsTests
{
    private static List<ValidationResult> Validate(object entity)
    {
        var context = new ValidationContext(entity);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(entity, context, results, validateAllProperties: true);
        return results;
    }

    [Fact]
    public void Pin_EmptyTitle_FailsValidation()
    {
        var pin = new Pin { Title = "", ImageUrl = "https://example.com/img.jpg" };
        Assert.NotEmpty(Validate(pin));
    }

    [Fact]
    public void Pin_EmptyImageUrl_FailsValidation()
    {
        var pin = new Pin { Title = "Cozy room", ImageUrl = "" };
        Assert.NotEmpty(Validate(pin));
    }

    [Fact]
    public void Pin_ValidRequiredFields_PassesValidation()
    {
        var pin = new Pin { Title = "Cozy room", ImageUrl = "https://example.com/img.jpg" };
        Assert.Empty(Validate(pin));
    }

    [Fact]
    public void Category_EmptyName_FailsValidation()
    {
        var category = new Category { Name = "", Icon = "interior" };
        Assert.NotEmpty(Validate(category));
    }

    [Fact]
    public void Tag_EmptyName_FailsValidation()
    {
        var tag = new Tag { Name = "" };
        Assert.NotEmpty(Validate(tag));
    }
}