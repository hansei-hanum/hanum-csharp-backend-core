namespace Hanum.Core.Models;

public class HanumUser {
    /// <summary>
    /// 사용자 ID
    /// </summary>
    public ulong Id { get; set; }
    /// <summary>
    /// 사용자 전화번호
    /// </summary>
    public string PhoneNumber { get; set; } = null!;
    /// <summary>
    /// 사용자 이름
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// 사용자 핸들
    /// </summary>
    public string? Handle { get; set; }
    /// <summary>
    /// 사용자 프로필 사진
    /// </summary>
    public string? Picture { get; set; }
    /// <summary>
    /// 사용자가 정지되었는지 여부
    /// </summary>
    public bool IsSuspended { get; set; }
    /// <summary>
    /// 사용자 생성일
    /// </summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// 사용자 인증 정보
    /// </summary>
    public HanumUserVerification? Verification { get; set; }
}

public class HanumUserVerification {
    /// <summary>
    /// 인증 종류
    /// </summary>
    public string Type { get; set; } = null!;
    /// <summary>
    /// 학과
    /// </summary>
    public string? Department { get; set; }
    /// <summary>
    /// 학년
    /// </summary>
    public int Grade { get; set; }
    /// <summary>
    /// 반
    /// </summary>
    public int Classroom { get; set; }
    /// <summary>
    /// 번호
    /// </summary>
    public int Number { get; set; }
}