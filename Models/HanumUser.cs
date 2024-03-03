namespace Hanum.Core.Models;

public class HanumUser {
    public ulong Id { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Picture { get; set; }
    public bool IsSuspended { get; set; }
    public DateTime CreatedAt { get; set; }
    public HanumUserVerification? Verification { get; set; }
}

public class HanumUserVerification {
    public string Type { get; set; } = null!;
    public string? Department { get; set; }
    public int Grade { get; set; }
    public int Classroom { get; set; }
    public int Number { get; set; }
}