using System.Linq.Expressions;
using System.Reflection;

using Microsoft.EntityFrameworkCore;

using Hanum.Pay.Models.DTO.Responses;

namespace Hanum.Core.Helpers;

public static class QueryableExtensions {
    private static TCursor? GetInstanceCursor<TCursor, TItem>(TItem? item, string cursorPropertyName) where TCursor : struct =>
        item == null ? null : (TCursor?)(typeof(TItem).GetProperty(cursorPropertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ?? throw new InvalidOperationException("Cursor property not found")).GetValue(item);

    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int page, int limit) =>
        query.Skip(Math.Max(0, page - 1) * limit).Take(limit);

    public static IEnumerable<T> Paginate<T>(this IEnumerable<T> query, int page, int limit) =>
        query.Skip(Math.Max(0, page - 1) * limit).Take(limit);

    public static IQueryable<T> Paginate<TCursor, T>(this IQueryable<T> query, TCursor? cursor, int limit,
        bool isAscending = true, string cursorPropertyName = "Id") where TCursor : struct {
        if (cursor == null) {
            return query.Take(limit);
        }

        var cursorProperty = typeof(T).GetProperty(cursorPropertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ?? throw new InvalidOperationException("Cursor property not found");
        var parameter = Expression.Parameter(typeof(T));
        var property = Expression.Property(parameter, cursorProperty);
        var lambda = isAscending
            ? Expression.Lambda(Expression.GreaterThan(property, Expression.Constant(cursor)), parameter)
            : Expression.Lambda(Expression.LessThan(property, Expression.Constant(cursor)), parameter);

        return query.Where((Expression<Func<T, bool>>)lambda).Take(limit);
    }

    public static async Task<DbOffsetBasedPagenationResult<TItem>> ToOffsetPagenation<TDbItem, TItem>(this IQueryable<TDbItem> query, Func<TDbItem, TItem> selectFunc, int page, int limit) =>
        new() {
            Items = (await query.Paginate(page, limit).ToListAsync()).Select(selectFunc),
            Limit = limit,
            Total = await query.CountAsync(),
            Page = page
        };

    public static async Task<DbOffsetBasedPagenationResult<TItem>> ToOffsetPagenation<TDbItem, TDbSelectedItem, TItem>(this IQueryable<TDbItem> query, Expression<Func<TDbItem, TDbSelectedItem>> selectQuery, Func<TDbSelectedItem, TItem> selectFunc, int page, int limit) =>
        new() {
            Items = (await query.Paginate(page, limit).Select(selectQuery).ToListAsync()).Select(selectFunc),
            Limit = limit,
            Total = await query.CountAsync(),
            Page = page
        };

    public static async Task<DbOffsetBasedPagenationResult<TItem>> ToOffsetPagenation<TDbItem, TItem>(this IQueryable<TDbItem> query, Func<TDbItem, Task<TItem>> selectFunc, int page, int limit) =>
        new() {
            Items = await Task.WhenAll((await query.Paginate(page, limit).ToListAsync()).Select(selectFunc)),
            Limit = limit,
            Total = await query.CountAsync(),
            Page = page
        };

    public static async Task<DbOffsetBasedPagenationResult<TItem>> ToOffsetPagenation<TDbItem, TDbSelectedItem, TItem>(this IQueryable<TDbItem> query, Expression<Func<TDbItem, TDbSelectedItem>> selectQuery, Func<TDbSelectedItem, Task<TItem>> selectFunc, int page, int limit) =>
        new() {
            Items = await Task.WhenAll((await query.Paginate(page, limit).Select(selectQuery).ToListAsync()).Select(selectFunc)),
            Limit = limit,
            Total = await query.CountAsync(),
            Page = page
        };


    public static async Task<DbCursorBasedPagenationResult<TCursor, TItem>> TCursorPagenation<TCursor, TDbItem, TItem>(this IQueryable<TDbItem> query, Func<TDbItem, TItem> selectFunc, TCursor? cursor, int limit, bool isAscending = true, string cursorPropertyName = "Id") where TCursor : struct {
        var items = await query.Paginate(cursor, limit, isAscending, cursorPropertyName).ToListAsync();
        return new() {
            Items = items.Select(selectFunc),
            Limit = limit,
            Total = await query.CountAsync(),
            Cursor = cursor,
            NextCursor = GetInstanceCursor<TCursor, TDbItem>(items.LastOrDefault(), cursorPropertyName)!
        };
    }

    public static async Task<DbCursorBasedPagenationResult<TCursor, TItem>> TCursorPagenation<TCursor, TDbItem, TDbSelectedItem, TItem>(this IQueryable<TDbItem> query, Expression<Func<TDbItem, TDbSelectedItem>> selectQuery, Func<TDbSelectedItem, TItem> selectFunc, TCursor? cursor, int limit, bool isAscending = true, string cursorPropertyName = "Id") where TCursor : struct {
        var items = await query.Paginate(cursor, limit, isAscending, cursorPropertyName).Select(selectQuery).ToListAsync();
        return new() {
            Items = items.Select(selectFunc),
            Limit = limit,
            Total = await query.CountAsync(),
            Cursor = cursor,
            NextCursor = GetInstanceCursor<TCursor, TDbSelectedItem>(items.LastOrDefault(), cursorPropertyName)!
        };
    }

    public static async Task<DbCursorBasedPagenationResult<TCursor, TItem>> ToCursorPagenation<TCursor, TDbItem, TItem>(this IQueryable<TDbItem> query, Func<TDbItem, Task<TItem>> selectFunc, TCursor? cursor, int limit, bool isAscending = true, string cursorPropertyName = "Id") where TCursor : struct {
        var items = await query.Paginate(cursor, limit, isAscending, cursorPropertyName).ToListAsync();
        return new() {
            Items = await Task.WhenAll(items.Select(selectFunc)),
            Limit = limit,
            Total = await query.CountAsync(),
            Cursor = cursor,
            NextCursor = GetInstanceCursor<TCursor, TDbItem>(items.LastOrDefault(), cursorPropertyName)!
        };
    }

    public static async Task<DbCursorBasedPagenationResult<TCursor, TItem>> ToCursorPagenation<TCursor, TDbItem, TDbSelectedItem, TItem>(this IQueryable<TDbItem> query, Expression<Func<TDbItem, TDbSelectedItem>> selectQuery, Func<TDbSelectedItem, Task<TItem>> selectFunc, TCursor? cursor, int limit, bool isAscending = true, string cursorPropertyName = "Id") where TCursor : struct {
        var items = await query.Paginate(cursor, limit, isAscending, cursorPropertyName).Select(selectQuery).ToListAsync();
        return new() {
            Items = await Task.WhenAll(items.Select(selectFunc)),
            Limit = limit,
            Total = await query.CountAsync(),
            Cursor = cursor,
            NextCursor = GetInstanceCursor<TCursor, TDbSelectedItem>(items.LastOrDefault(), cursorPropertyName)!
        };
    }
}
