namespace MoodboardAI.Api.DTOs.Common;

public class PaginationParams
{
    private const int MaxPageSize = 100;
    private int _pageSize = 20;

    /// <summary>
    /// Номер сторінки
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Розмір сторінки
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : (value <= 0 ? 20 : value);
    }

    /// <summary>
    /// Поле для сортування
    /// </summary>
    public string? SortBy { get; set; }

    /// <summary>
    /// Напрямок сортування
    /// </summary>
    public string SortOrder { get; set; } = "desc";
}