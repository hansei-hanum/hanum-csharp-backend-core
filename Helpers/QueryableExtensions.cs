
namespace Hanum.Core.Helpers;

public static class QueryableExtensions {
    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int page, int pageSize) =>
        query.Skip(Math.Max(0, page - 1) * pageSize).Take(pageSize);
}
