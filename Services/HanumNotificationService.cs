using Hanum.Core.Models;
using Hanum.Core.Protos.Authv2;

namespace Hanum.Core.Services;

public class HanumNotificationService(AuthServiceV2.AuthServiceV2Client authServiceClient) : IHanumNotificationService {
    public async Task<bool> SendNotificationAsync(HanumNotification body, ulong? userId = null, string? topic = null, bool stack = false) {
        if (userId == null && topic == null)
            throw new ArgumentException("userId and topic cannot be null at the same time");

        if (body.Title == null && body.Content == null)
            throw new ArgumentException("Title and Content cannot be null at the same time");

        SendPushRequest request = new() {
            SaveinList = stack
        };

        if (body.Title != null)
            request.Title = body.Title;

        if (body.Content != null)
            request.Body = body.Content;

        if (body.Image != null)
            request.Image = body.Image;

        if (body.Action != null)
            request.Link = body.Action;

        if (userId != null)
            request.Userid = userId.Value;

        if (topic != null)
            request.Topic = topic;

        return (await authServiceClient.SendPushAsync(request)).Success;
    }

    public async Task<bool> SendNotificationAsync(HanumNotification body, ulong[] userIds, bool stack = false) {
        ArgumentNullException.ThrowIfNull(userIds, nameof(userIds));
        return (await Task.WhenAll(userIds.Select(userId => SendNotificationAsync(body, userId: userId, stack: stack)))).All(result => result);
    }
}

public static class HanumNotificationServiceExtensions {
    public static IServiceCollection AddHanumNotificationService(this IServiceCollection services) =>
        services.AddTransient<IHanumNotificationService, HanumNotificationService>();
}