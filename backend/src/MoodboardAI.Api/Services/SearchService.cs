using Microsoft.EntityFrameworkCore;
using MoodboardAI.Api.Data;
using MoodboardAI.Api.DTOs.Search;
using MoodboardAI.Api.Models;

namespace MoodboardAI.Api.Services;

/// <summary>
/// EF Core-backed implementation of <see cref="ISearchService"/>.
/// </summary>
public class SearchService : ISearchService
{
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchService"/> class.
    /// </summary>
    public SearchService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<SearchResponseDto> SearchAsync(string? query, Guid? categoryId, Guid? tagId, int page, int pageSize)
    {
        if (page < 1)
        {
            page = 1;
        }

        if (pageSize < 1)
        {
            pageSize = 10;
        }

        if (pageSize > 100)
        {
            pageSize = 100;
        }

        var pinsQuery = _dbContext.Pins
            .AsNoTracking()
            .Include(pin => pin.Category)
            .Include(pin => pin.PinTags)
                .ThenInclude(pinTag => pinTag.Tag)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
        {
            var normalizedQuery = query.Trim().ToLower();

            pinsQuery = pinsQuery.Where(pin =>
                pin.Title.ToLower().Contains(normalizedQuery) ||
                (pin.Description != null && pin.Description.ToLower().Contains(normalizedQuery)) ||
                pin.Category.Name.ToLower().Contains(normalizedQuery) ||
                pin.PinTags.Any(pinTag => pinTag.Tag.Name.ToLower().Contains(normalizedQuery)));
        }

        if (categoryId.HasValue)
        {
            pinsQuery = pinsQuery.Where(pin => pin.CategoryId == categoryId.Value);
        }

        if (tagId.HasValue)
        {
            pinsQuery = pinsQuery.Where(pin => pin.PinTags.Any(pinTag => pinTag.TagId == tagId.Value));
        }

        var totalCount = await pinsQuery.CountAsync();

        var pins = await pinsQuery
            .OrderByDescending(pin => pin.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new SearchResponseDto
        {
            Items = pins.Select(ToSearchResultDto).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    /// <inheritdoc />
    public async Task<List<SearchResultDto>> GetTrendingAsync(int count)
    {
        if (count < 1)
        {
            count = 10;
        }

        if (count > 100)
        {
            count = 100;
        }

        var pins = await _dbContext.Pins
            .AsNoTracking()
            .Include(pin => pin.Category)
            .Include(pin => pin.PinTags)
                .ThenInclude(pinTag => pinTag.Tag)
            .Include(pin => pin.Likes)
            .OrderByDescending(pin => pin.Likes.Count)
            .ThenByDescending(pin => pin.CreatedAt)
            .Take(count)
            .ToListAsync();

        return pins.Select(ToSearchResultDto).ToList();
    }

    /// <inheritdoc />
    public async Task<List<CategoryDto>> GetCategoriesAsync()
    {
        return await _dbContext.Categories
            .AsNoTracking()
            .OrderBy(category => category.Name)
            .Select(category => new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Icon = category.Icon
            })
            .ToListAsync();
    }

    /// <summary>
    /// Maps a <see cref="Pin"/> entity (with Category and PinTags/Tag loaded)
    /// to a <see cref="SearchResultDto"/>.
    /// </summary>
    private static SearchResultDto ToSearchResultDto(Pin pin) => new()
    {
        Id = pin.Id,
        Title = pin.Title,
        ImageUrl = pin.ImageUrl,
        Category = pin.Category.Name,
        Tags = pin.PinTags.Select(pinTag => pinTag.Tag.Name).ToList(),
        CreatedAt = pin.CreatedAt
    };
}