
namespace Hanum.Core.Models;

/// <summary>
/// 데이터베이스 페이징 결과
/// </summary>
/// <typeparam name="TItem">아이템</typeparam>
public class DbPaginationResult<TItem> {
    public required IEnumerable<TItem> Items { get; set; } = [];
    /// <summary>
    /// 페이지당 항목수
    /// </summary>
    public required int Limit { get; set; }
    /// <summary>
    /// 전체 항목수
    /// </summary>
    public required int Total { get; set; }
    /// <summary>
    /// 전체 페이지수
    /// </summary>
    public int TotalPage => Limit > 0 ? (int)Math.Ceiling((double)Total / Limit) : 0;
}

/// <summary>
/// 데이터베이스 오프셋 기반 페이징 결과
/// </summary>
/// <typeparam name="TItem">아이템</typeparam>
public class DbOffsetBasedPaginationResult<TItem> : DbPaginationResult<TItem> {
    /// <summary>
    /// 페이지
    /// </summary>
    public required int Page { get; set; }
}

/// <summary>
/// 데이터베이스 커서 기반 페이징 결과
/// </summary>
/// <typeparam name="TCursor">커서 타입</typeparam>
/// <typeparam name="TItem">아이템 타입</typeparam>
public class DbCursorBasedPaginationResult<TCursor, TItem> : DbPaginationResult<TItem> where TCursor : struct {
    /// <summary>
    /// 커서
    /// </summary>
    public required TCursor? Cursor { get; set; }
    /// <summary>
    /// 다음 커서
    /// </summary>
    public required TCursor? NextCursor { get; set; }
}
