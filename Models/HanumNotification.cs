
namespace Hanum.Core.Models;

public class HanumNotification {
    public required string Title { get; set; }
    public required string Content { get; set; }
    public string? Image { get; set; } = null!;
    public string? Action { get; set; } = null!;
}
