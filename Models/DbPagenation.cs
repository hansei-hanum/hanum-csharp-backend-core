
namespace Hanum.Core.Models.Responses;

public class DbPagenationResult<T>() {
    public IEnumerable<T> Items { get; set; } = [];
    /// <summary>
    /// 페이지
    /// </summary>
    public int Page { get; set; }
    /// <summary>
    /// 페이지당 항목수
    /// </summary>
    public int Limit { get; set; }
    /// <summary>
    /// 전체 항목수
    /// </summary>
    public int Total { get; set; }
    /// <summary>
    /// 전체 페이지수
    /// </summary>
    public int TotalPage => Limit > 0 ? (int)Math.Ceiling((double)Total / Limit) : 0;

    public DbPagenationResult(IEnumerable<T> items, int page, int limit, int total) : this() {
        Items = items;
        Page = page;
        Limit = limit;
        Total = total;
    }
}
