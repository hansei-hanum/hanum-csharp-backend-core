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
    /// <summary>
    /// 졸업년도
    /// </summary>
    public string? GraduationYear { get; set; }

    public string GetTypeName() {
        return Type switch {
            "GRADUATED" => "졸업생",
            "STUDENT" => "재학생",
            "TEACHER" => "교직원",
            _ => "인증되지 않음"
        };
    }

    public string GetDepartment() {
        return Type switch {
            "CLOUD_SECURITY" => "클라우드보안과",
            "NETWORK_SECURITY" => "네트워크보안과",
            "METAVERSE_GAME" => "메타버스게임과",
            "HACKING_SECURITY" => "해킹보안과",
            "GAME" => "게임과",
            _ => "알 수 없음"
        };
    }

    public override string ToString() {
        return Type switch {
            "GRADUATED" => $"{GetTypeName()} {GraduationYear}년 졸업생",
            "STUDENT" => $"{GetTypeName()} {Department} {Grade}학년 {Classroom}반 재학생",
            "TEACHER" => "한세사이버보안고등학교 교직원",
            _ => "인증되지 않음"
        };
    }
}