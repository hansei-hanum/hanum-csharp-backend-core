
using System.Text.Json.Serialization;
using Hanum.Community.Models;

namespace Hanum.Core.Models.Responses;

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
    /// 페이지
    /// </summary>
    public required int Page { get; set; }
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
/// <typeparam name="TData">데이터타입</typeparam>
public class APIPagenationResponse<TData> : APIResponse<APIPagenationData<TData>> {
    public static APIPagenationResponse<TData> FromDbResult(DbPagenationResult<TData> data) {
        return new APIPagenationResponse<TData> {
            Code = HanumStatusCode.Success,
            Data = new() {
                Items = data.Items,
                Page = data.Page,
                Limit = data.Limit,
                Total = data.Total
            }
        };
    }

    public new static APIPagenationResponse<TData> FromData(APIPagenationData<TData> data) {
        return new APIPagenationResponse<TData> {
            Code = HanumStatusCode.Success,
            Data = data
        };
    }

    public static new APIPagenationResponse<TData> FromError(HanumStatusCode code) {
        return new APIPagenationResponse<TData> {
            Code = code
        };
    }

    public new static APIPagenationResponse<TData> FromError(HanumStatusCode code, APIPagenationData<TData> data) {
        return new APIPagenationResponse<TData> {
            Code = code,
            Data = data
        };
    }

    public new static APIPagenationResponse<TData> FromError(APIPagenationData<TData> data) {
        return new APIPagenationResponse<TData> {
            Code = HanumStatusCode.Error,
            Data = data
        };
    }
}
