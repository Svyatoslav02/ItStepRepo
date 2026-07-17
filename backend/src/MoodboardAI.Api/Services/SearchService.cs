using Microsoft.EntityFrameworkCore;
using MoodboardAI.Api.Data;
using MoodboardAI.Api.DTOs.Search;
using MoodboardAI.Api.Models;
using System.Net.NetworkInformation;

namespace MoodboardAI.Api.Services;

/// <summary>
/// Implements pin search, trending, and category listing, backed by
/// <see cref="ApplicationDbContext"/>.
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
    public async Task<SearchResponseDto> SearchAsync(string? query, Guid? categoryId, string? tag, int page, int pageSize)
    {
        page = page < 1 ? 1 : page;
        pageSize = Math.Clamp(pageSize, 1, 100);

        var pinsQuery = BuildFilteredQuery(query, categoryId, tag);

        var totalCount = await pinsQuery.CountAsync();

        var pins = await pinsQuery
            .OrderByDescending(pin => pin.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new SearchResponseDto
        {
            Results = pins.Select(ToSearchResultDto).ToList(),
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    /// <inheritdoc />
    public async Task<List<SearchResultDto>> GetTrendingAsync(int count)
    {
        count = Math.Clamp(count, 1, 50);

        // No engagement metrics (views/likes) exist yet, so "trending" is
        // approximated by recency. Revisit once such metrics are tracked.
        var pins = await _dbContext.Pins
            .Include(pin => pin.Category)
            .Include(pin => pin.PinTags)
                .ThenInclude(pinTag => pinTag.Tag)
            .OrderByDescending(pin => pin.CreatedAt)
            .Take(count)
            .ToListAsync();

        return pins.Select(ToSearchResultDto).ToList();
    }

    /// <inheritdoc />
    public async Task<List<SearchCategoryDto>> GetCategoriesAsync()
    {
        return await _dbContext.Categories
            .OrderBy(category => category.Name)
            .Select(category => new SearchCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Icon = category.Icon
            })
            .ToListAsync();
    }

    /// <summary>
    /// Builds the filtered (but not yet paginated) pins query shared by
    /// <see cref="SearchAsync"/>.
    /// </summary>
    private IQueryable<Pin> BuildFilteredQuery(string? query, Guid? categoryId, string? tag)
    {
        var pinsQuery = _dbContext.Pins
            .Include(pin => pin.Category)
            .Include(pin => pin.PinTags)
                .ThenInclude(pinTag => pinTag.Tag)
            .AsQueryable();

        if (categoryId.HasValue)
        {
            pinsQuery = pinsQuery.Where(pin => pin.CategoryId == categoryId.Value);
        }

        if (!string.IsNullOrWhiteSpace(tag))
        {
            var normalizedTag = tag.Trim().ToLowerInvariant();
            pinsQuery = pinsQuery.Where(pin =>
                pin.PinTags.Any(pinTag => pinTag.Tag.Name.ToLower() == normalizedTag));
        }

        if (!string.IsNullOrWhiteSpace(query))
        {
            var normalizedQuery = query.Trim().ToLowerInvariant();
            pinsQuery = pinsQuery.Where(pin =>
                pin.Title.ToLower().Contains(normalizedQuery) ||
                (pin.Description != null && pin.Description.ToLower().Contains(normalizedQuery)) ||
                pin.Category.Name.ToLower().Contains(normalizedQuery) ||
                pin.PinTags.Any(pinTag => pinTag.Tag.Name.ToLower().Contains(normalizedQuery)));
        }

        return pinsQuery;
    }

    /// <summary>
    /// Maps a <see cref="Pin"/> (with <see cref="Pin.Category"/> and
    /// <see cref="Pin.PinTags"/> loaded) to a <see cref="SearchResultDto"/>.
    /// </summary>
    private static SearchResultDto ToSearchResultDto(Pin pin)
    {
        return new SearchResultDto
        {
            Id = pin.Id,
            Title = pin.Title,
            ImageUrl = pin.ImageUrl,
            Category = pin.Category.Name,
            Tags = pin.PinTags
                .Select(pinTag => pinTag.Tag.Name)
                .OrderBy(name => name)
                .ToList(),
            CreatedAt = pin.CreatedAt
        };
    }
}