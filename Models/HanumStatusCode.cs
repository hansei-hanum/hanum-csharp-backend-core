
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Hanum.Community.Models;

public enum HanumStatusCode {
    /// <summary>
    /// 성공
    /// </summary>
    Success,

    ////////////////////////////////////////////
    //              커뮤니티 관련              //
    ////////////////////////////////////////////

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