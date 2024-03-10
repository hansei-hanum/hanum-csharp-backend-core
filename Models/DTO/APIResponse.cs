using System.Text.Json.Serialization;
using Hanum.Core.Models;

namespace Hanum.Pay.Models.DTO.Responses;

/// <summary>
/// API응답
/// </summary>
public class APIResponse {
    /// <summary>
    /// 응답코드
    /// </summary>
    [JsonPropertyName("message")]
    [JsonConverter(typeof(HanumStatusCodeConverter))]
    public required HanumStatusCode Code { get; set; }
    [JsonInclude]
    private object? Data { get; set; } = null;

    public static APIResponse FromSuccess() {
        return new APIResponse {
            Code = HanumStatusCode.Success
        };
    }

    public static APIResponse FromError(HanumStatusCode code) {
        return new APIResponse {
            Code = code
        };
    }
}

/// <summary>
/// API응답
/// </summary>
/// <typeparam name="TData">데이터타입</typeparam>
public class APIResponse<TData> {
    /// <summary>
    /// 응답코드
    /// </summary>
    [JsonPropertyName("message")]
    [JsonConverter(typeof(HanumStatusCodeConverter))]
    public required HanumStatusCode Code { get; set; }
    /// <summary>
    /// 응답데이터
    /// </summary>
    public TData? Data { get; set; } = default;

    public static APIResponse<TData> FromData(TData data) {
        return new APIResponse<TData> {
            Code = HanumStatusCode.Success,
            Data = data
        };
    }

    public static APIResponse<TData> FromError(HanumStatusCode code) {
        return new APIResponse<TData> {
            Code = code
        };
    }

    public static APIResponse<TData> FromError(HanumStatusCode code, TData data) {
        return new APIResponse<TData> {
            Code = code,
            Data = data
        };
    }

    public static APIResponse<TData> FromError(TData data) {
        return new APIResponse<TData> {
            Code = HanumStatusCode.Error,
            Data = data
        };
    }
}

/// <summary>
/// API페이징응답
/// </summary>
public class APIPagenationData<TItem> {
    /// <summary>
    /// 항목목록
    /// </summary>
    public required IEnumerable<TItem> Items { get; set; }
    /// <summary>
    /// 페이지당 항목수
    /// </summary>
    public required int Limit { get; set; }
    /// <summary>
    /// 전체 항목수
    /// </summary>
    public required int Total { get; set; }
    /// <summary>
    /// 전체 페이지수
    /// </summary>
    public int TotalPage => Limit > 0 ? (int)Math.Ceiling((double)Total / Limit) : 0;
}

/// <summary>
/// API페이징응답
/// </summary>
public class APIOffsetBasedPagenationData<TItem> : APIPagenationData<TItem> {
    /// <summary>
    /// 페이지
    /// </summary>
    public required int Page { get; set; }
}

/// <summary>
/// API 커서 페이징 응답
/// </summary>
public class APICursorBasedPagenationData<TCursor, TItem> : APIPagenationData<TItem> where TCursor : struct {
    /// <summary>
    /// 커서
    /// </summary>
    public required TCursor? Cursor { get; set; }
    /// <summary>
    /// 다음 커서
    /// </summary>
    public required TCursor? NextCursor { get; set; }
}

/// <summary>
/// API 오프셋 기반 페이징 응답
/// </summary>
/// <typeparam name="TItem">데이터타입</typeparam>
public class APIOffsetBasedPagenationResponse<TItem> : APIResponse<APIOffsetBasedPagenationData<TItem>> {
    public static APIOffsetBasedPagenationResponse<TItem> FromDbResult(DbOffsetBasedPagenationResult<TItem> data) {
        return new APIOffsetBasedPagenationResponse<TItem> {
            Code = HanumStatusCode.Success,
            Data = new() {
                Items = data.Items,
                Page = data.Page,
                Limit = data.Limit,
                Total = data.Total
            }
        };
    }

    public new static APIOffsetBasedPagenationResponse<TItem> FromData(APIOffsetBasedPagenationData<TItem> data) {
        return new APIOffsetBasedPagenationResponse<TItem> {
            Code = HanumStatusCode.Success,
            Data = data
        };
    }

    public static new APIOffsetBasedPagenationResponse<TItem> FromError(HanumStatusCode code) {
        return new APIOffsetBasedPagenationResponse<TItem> {
            Code = code
        };
    }

    public new static APIOffsetBasedPagenationResponse<TItem> FromError(HanumStatusCode code, APIOffsetBasedPagenationData<TItem> data) {
        return new APIOffsetBasedPagenationResponse<TItem> {
            Code = code,
            Data = data
        };
    }

    public new static APIOffsetBasedPagenationResponse<TItem> FromError(APIOffsetBasedPagenationData<TItem> data) {
        return new APIOffsetBasedPagenationResponse<TItem> {
            Code = HanumStatusCode.Error,
            Data = data
        };
    }
}

/// <summary>
/// API 커서 페이징 응답
/// </summary>
/// <typeparam name="TCursor">커서타입</typeparam>
/// <typeparam name="TItem">데이터타입</typeparam>
public class APICursorBasedPagenationResponse<TCursor, TItem> : APIResponse<APICursorBasedPagenationData<TCursor, TItem>> where TCursor : struct {
    public static APICursorBasedPagenationResponse<TCursor, TItem> FromDbResult(DbCursorBasedPagenationResult<TCursor, TItem> data) {
        return new APICursorBasedPagenationResponse<TCursor, TItem> {
            Code = HanumStatusCode.Success,
            Data = new() {
                Items = data.Items,
                Limit = data.Limit,
                Total = data.Total,
                Cursor = data.Cursor,
                NextCursor = data.NextCursor
            }
        };
    }

    public new static APICursorBasedPagenationResponse<TCursor, TItem> FromData(APICursorBasedPagenationData<TCursor, TItem> data) {
        return new APICursorBasedPagenationResponse<TCursor, TItem> {
            Code = HanumStatusCode.Success,
            Data = data
        };
    }

    public static new APICursorBasedPagenationResponse<TCursor, TItem> FromError(HanumStatusCode code) {
        return new APICursorBasedPagenationResponse<TCursor, TItem> {
            Code = code
        };
    }

    public new static APICursorBasedPagenationResponse<TCursor, TItem> FromError(HanumStatusCode code, APICursorBasedPagenationData<TCursor, TItem> data) {
        return new APICursorBasedPagenationResponse<TCursor, TItem> {
            Code = code,
            Data = data
        };
    }

    public new static APICursorBasedPagenationResponse<TCursor, TItem> FromError(APICursorBasedPagenationData<TCursor, TItem> data) {
        return new APICursorBasedPagenationResponse<TCursor, TItem> {
            Code = HanumStatusCode.Error,
            Data = data
        };
    }
}
