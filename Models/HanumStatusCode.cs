
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Hanum.Community.Models;

public enum HanumStatusCode {
    /// <summary>
    /// 성공
    /// </summary>
    Success,
    /// <summary>
    /// 서버 오류
    /// </summary>
    Error,
    /// <summary>
    /// 잘못된 요청
    /// </summary>
    InvalidRequest,

    ////////////////////////////////////////////
    //               사용자 관련               //
    ////////////////////////////////////////////
    /// <summary>
    /// 사용자가 존재하지 않음
    /// </summary>
    UserNotFound,

    ////////////////////////////////////////////
    //              커뮤니티 관련              //
    ////////////////////////////////////////////

    /// <summary>
    /// 이미 차단된 사용자
    /// </summary>
    AlreadyBlocked,
    /// <summary>
    /// 자신을 차단할 수 없음
    /// </summary>
    CanNotBlockYourself,

    /// <summary>
    /// 공개 범위가 잘못됨.
    /// 사용자가 공개 범위에 대한 권한이 없음.
    /// </summary>
    InvalidScopeOfDisclosure,
    /// <summary>
    /// 게시글이 존재하지 않음
    /// </summary>
    ArticleNotFound,
    /// <summary>
    /// 댓글이 존재하지 않음
    /// </summary>
    CommentNotFound,
    /// <summary>
    /// 답글이 존재하지 않음
    /// </summary>
    ReplyNotFound,
}

public partial class HanumStatusCodeConverter : JsonConverter<HanumStatusCode> {
    public override HanumStatusCode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        var value = UnderBarToUppperCaseRegex().Replace(reader.GetString()!.ToLower(), m => m.Groups[1].Value.ToUpper());
        return Enum.Parse<HanumStatusCode>(string.IsNullOrWhiteSpace(value) ? "" : char.ToUpper(value[0]) + value[1..]);
    }

    public override void Write(Utf8JsonWriter writer, HanumStatusCode value, JsonSerializerOptions options) {
        writer.WriteStringValue(UppperCaseToUnderBarRegex().Replace(value.ToString(), "_$1").ToUpper());
    }

    [GeneratedRegex(@"(?<!^)([A-Z])")]
    private static partial Regex UppperCaseToUnderBarRegex();

    [GeneratedRegex(@"_(.)")]
    private static partial Regex UnderBarToUppperCaseRegex();
}