namespace MoodboardAI.Api.Models;

public class PaginationQuery
{
    private int _pageSize = 20;
    public int Page { get; set; } = 1;
    
    public int PageSize
    {
        get
        {
            return _pageSize;
        }

        set
        {
            if (value > 100)
            {
                _pageSize = 100;
            }
            else
            {
                _pageSize = value;
            }
        }
    }
}