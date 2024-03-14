using Hanum.Core.Models;

namespace Hanum.Core.Services;

public interface IHanumNotificationService {
    /// <summary>
    /// 알림을 보냅니다.
    /// </summary>
    /// <param name="body">알림 내용</param>
    /// <param name="userId">사용자 ID</param>
    /// <param name="topic">토픽</param>
    /// <param name="stack">알림 목록에 쌓을지 여부</param>
    /// <returns>성공 여부</returns>
    Task<bool> SendNotificationAsync(HanumNotification body, ulong? userId = null, string? topic = null, bool stack = false);
    /// <summary>
    /// 알림을 보냅니다.
    /// </summary>
    /// <param name="body">알림 내용</param>
    /// <param name="userIds">사용자 ID 목록</param>
    /// <param name="stack">알림 목록에 쌓을지 여부</param>
    /// <returns>성공 여부</returns>
    Task<bool> SendNotificationAsync(HanumNotification body, ulong[] userIds, bool stack = false);
}
