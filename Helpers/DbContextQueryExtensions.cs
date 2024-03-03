using Microsoft.EntityFrameworkCore;

namespace Hanum.Core.Helpers;

public static class DbContextQueryExtensions {
    public static async Task SetServerCurrentTimestampAsync<T, TKey>(this DbContext context, TKey primaryKey, string field) where T : class where TKey : struct {
        var entity = context.Model.FindEntityType(typeof(T)) ?? throw new ArgumentException("Entity not found", nameof(T));
        var column = entity.FindProperty(field)?.GetColumnName() ?? throw new ArgumentException("Field not found", nameof(field));
        var primary = entity.FindPrimaryKey()?.Properties.Where(p => p.ClrType == typeof(TKey)).FirstOrDefault()
            ?? throw new ArgumentException("Primary key not found", nameof(primaryKey));

#pragma warning disable EF1002
        await context.Database.ExecuteSqlRawAsync($"update `{entity.GetTableName()}` set `{column}` = current_timestamp() where `{primary.Name}` = @p0", primaryKey);
#pragma warning restore EF1002
    }
}