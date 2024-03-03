using Hanum.Core.Models;
using Hanum.Core.Protos.Auth;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Hanum.Core.Services;

internal class InternalHanumUser() : HanumUser {
    public InternalHanumUser(User user) : this() {
        Id = unchecked((ulong)user.Id);
        PhoneNumber = user.Phone;
        Name = user.Name;
        Picture = user.Profile;
        IsSuspended = user.IsSuspended;
        CreatedAt = DateTime.Parse(user.CreatedAt);
        Verification = user.Verification == null ? null : new InternalHanumUserVerification(user.Verification);
    }
}

internal class InternalHanumUserVerification() : HanumUserVerification {
    public InternalHanumUserVerification(Verification verification) : this() {
        Type = verification.Type;
        Department = verification.Department;
        Grade = verification.Grade;
        Classroom = verification.Classroom;
        Number = verification.Number;
    }
}

public interface IHanumUserService {
    public Task<HanumUser?> GetUserAsync(ulong id, bool force = false);
}

public class HanumUserService(
    [FromKeyedServices("hanumUserCache")]
    IMemoryCache cache,
    AuthService.AuthServiceClient authServiceClient,
    IConfiguration configuration) : IHanumUserService {
    public async Task<HanumUser?> GetUserAsync(ulong id, bool force = false) {
        if (!force && cache.TryGetValue(id, out HanumUser? user)) {
            return user!;
        }

        var result = await authServiceClient.GetUserAsync(new() {
            Userid = unchecked((long)id)
        });

        if (result.User == null)
            return null;

        user = new InternalHanumUser(result.User);
        cache.Set(id, user, TimeSpan.FromMinutes(configuration.GetValue("Hanum.UserCache.ExpirationMinutes", 10)));
        return user;
    }
}

public static class HanumUserServiceExtensions {
    public static IServiceCollection AddHanumUserService(this IServiceCollection services) {
        services.AddKeyedSingleton<IMemoryCache, MemoryCache>("hanumUserCache");
        services.AddSingleton<IHanumUserService, HanumUserService>();
        return services;
    }
}