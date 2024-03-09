using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

using Hanum.Core.Models;
using Hanum.Core.Protos.Auth;

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
        Type = Enum.Parse<HanumUserVerificationType>(verification.Type, true);
        Department = verification.Department;
        Grade = verification.Grade;
        Classroom = verification.Classroom;
        Number = verification.Number;
        if (ushort.TryParse(verification.GraduatedAt, out ushort graduationYear))
            GraduationYear = graduationYear;
    }
}

public interface IHanumUserService {
    /// <summary>
    /// 사용자 정보를 가져옵니다.
    /// </summary>
    /// <param name="id">사용자 ID</param>
    /// <param name="useCache">캐시 사용 여부</param>
    /// <returns>사용자 정보</returns>
    public Task<HanumUser?> GetUserAsync(ulong id, bool useCache = true);
    /// <summary>
    /// 사용자가 존재하는지 확인합니다.
    /// </summary>
    /// <param name="id">사용자 ID</param>
    /// <returns>사용자가 존재하는지 여부</returns>
    public Task<bool> ExistsAsync(ulong id);
}

public class HanumUserService(
    [FromKeyedServices("hanumUserCache")]
    IMemoryCache cache,
    AuthService.AuthServiceClient authServiceClient,
    IConfiguration configuration) : IHanumUserService {
    public async Task<HanumUser?> GetUserAsync(ulong id, bool useCache = true) {
        if (useCache && cache.TryGetValue(id, out HanumUser? user)) {
            return user!;
        }

        var result = await authServiceClient.GetUserAsync(new() {
            Userid = unchecked((long)id)
        });

        if (!result.Success)
            return null;

        user = new InternalHanumUser(result.User);

        if (useCache)
            cache.Set(id, user, TimeSpan.FromMinutes(configuration.GetValue("Hanum.UserCache.ExpirationMinutes", 10)));

        return user;
    }

    public async Task<bool> ExistsAsync(ulong id) {
        return await GetUserAsync(id) != null;
    }
}

public static class HanumUserServiceExtensions {
    public static IServiceCollection AddHanumUserService(this IServiceCollection services) {
        services.AddKeyedSingleton<IMemoryCache, MemoryCache>("hanumUserCache");
        services.AddSingleton<IHanumUserService, HanumUserService>();
        return services;
    }
}