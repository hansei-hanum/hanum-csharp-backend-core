using Hanum.Core.Models;
using Hanum.Core.Protos.Auth;

namespace Hanum.Core.Services;

public interface IHanumNotificationService {
    Task<bool> SendNotificationAsync(HanumNotification body, ulong? userId = null, string? topic = null);
    Task<bool> SendNotificationAsync(HanumNotification body, params ulong[] userIds);
}

public class HanumNotificationService(AuthService.AuthServiceClient authServiceClient) : IHanumNotificationService {
    public async Task<bool> SendNotificationAsync(HanumNotification body, ulong? userId = null, string? topic = null) {
        if (userId == null && topic == null)
            throw new ArgumentException("userId and topic cannot be null at the same time");

        if (body.Title == null && body.Content == null)
            throw new ArgumentException("Title and Content cannot be null at the same time");

        SendPushRequest request = new() {
            Title = body.Title,
            Body = body.Content,
            Image = body.Image,
            Link = body.Action,
        };

        if (userId != null)
            request.Userid = unchecked((long)userId);

        if (topic != null)
            request.Topic = topic;

        return (await authServiceClient.SendPushAsync(request)).Success;
    }

    public async Task<bool> SendNotificationAsync(HanumNotification body, params ulong[] userIds) {
        ArgumentNullException.ThrowIfNull(userIds, nameof(userIds));
        return (await Task.WhenAll(userIds.Select(userId => SendNotificationAsync(body, userId)))).All(result => result);
    }
}

public static class HanumNotificationServiceExtensions {
    public static IServiceCollection AddHanumNotificationService(this IServiceCollection services) =>
        services.AddTransient<IHanumNotificationService, HanumNotificationService>();
}