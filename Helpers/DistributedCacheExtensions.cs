using System.Text.Json;

using Microsoft.Extensions.Caching.Distributed;

namespace Hanum.Core.Helpers;

public static class DistributedCacheExtensions {
    public static async Task<T?> GetAsync<T>(this IDistributedCache cache, string key) {
        var bytes = await cache.GetAsync(key);
        if (bytes == null) {
            return default;
        }
        return JsonSerializer.Deserialize<T>(bytes);
    }

    public static async Task SetAsync<T>(this IDistributedCache cache, string key, T value) {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(value);
        await cache.SetAsync(key, bytes);
    }
}
