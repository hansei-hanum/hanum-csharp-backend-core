
namespace Hanum.Core.Models;

public class HanumNotification {
    /// <summary>
    /// 알림 제목
    /// </summary>
    public string? Title { get; set; } = null;
    /// <summary>
    /// 알림 내용
    /// </summary>
    public string? Content { get; set; } = null;
    /// <summary>
    /// 알림 이미지
    /// </summary>
    public string? Image { get; set; } = null;
    /// <summary>
    /// 알림 링크
    /// </summary>
    public string? Action { get; set; } = null;
}
