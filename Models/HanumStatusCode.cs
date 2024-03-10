using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Hanum.Core.Models;

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
    /// <summary>
    /// 권한 없음
    /// </summary>
    NotAllowed,

    ////////////////////////////////////////////
    //               사용자 관련               //
    ////////////////////////////////////////////
    /// <summary>
    /// 사용자가 존재하지 않음
    /// </summary>
    UserNotFound,

    ////////////////////////////////////////////
    //                페이 관련                //
    ////////////////////////////////////////////
    /// <summary>
    /// 부스가 존재하지 않음
    /// </summary>
    BoothNotFound,
    /// <summary>
    /// 결제가 존재하지 않음
    /// </summary>
    PaymentNotFound,
    /// <summary>
    /// 해당 잔고는 개인잔고가 아닙니다.
    /// </summary>
    NotAPersonalBalance,
    /// <summary>
    /// 해당 부스 잔고가 존재하지 않습니다.
    /// </summary>
    BoothBalanceNotFound,
    /// <summary>
    /// 해당 잔고는 부스 잔고가 아닙니다.
    /// </summary>
    NotABoothOperationalBalance,
    /// <summary>
    /// 해당 결제내역이 존재하지 않습니다.
    /// </summary>
    PaymentRecordNotFound,
    /// <summary>
    /// 이미 결제가 취소되었습니다.
    /// </summary>
    PaymentAlreadyCancelled,
    /// <summary>
    /// 결제 취소 상태를 업데이트하지 못했습니다.
    /// </summary>
    PaymentCancellationStatusNotUpdated,
    /// <summary>
    /// 송금자와 수신자가 일치합니다.
    /// </summary>
    SenderIdEqualsReceiverId,
    /// <summary>
    /// 송금액이 올바른지 확인하십시오.
    /// </summary>
    InvalidTransferAmount,
    /// <summary>
    /// 송금자ID가 잘못되었습니다.
    /// </summary>
    InvalidSenderId,
    /// <summary>
    /// 송금자의 잔액이 부족합니다.
    /// </summary>
    InsufficientSenderBalance,
    /// <summary>
    /// 수신자ID가 잘못되었습니다.
    /// </summary>
    InvalidReceiverId,
    /// <summary>
    /// 송금자 금액을 업데이트하지 못했습니다.
    /// </summary>
    SenderBalanceNotUpdated,
    /// <summary>
    /// 수신자 금액을 업데이트하지 못했습니다.
    /// </summary>
    ReceiverBalanceNotUpdated,

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