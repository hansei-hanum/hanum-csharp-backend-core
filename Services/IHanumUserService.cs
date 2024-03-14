using Hanum.Core.Models;

namespace Hanum.Core.Services;

public interface IHanumUserService {
    /// <summary>
    /// 사용자 정보를 가져옵니다.
    /// </summary>
    /// <param name="id">사용자 ID</param>
    /// <param name="useCache">캐시 사용 여부</param>
    /// <returns>사용자 정보</returns>
    public Task<HanumUser?> GetUserAsync(ulong id, bool useCache = true);
    /// <summary>
    /// 사용자를 검색합니다.
    /// </summary>
    /// <param name="name">사용자 이름</param>
    /// <param name="offset">검색 오프셋</param>
    /// <param name="limit">검색 제한 수</param>
    /// <param name="useCache">캐시 사용 여부</param>
    /// <returns>사용자 정보</returns>
    public Task<HanumUser[]> FindUsersAsync(string name, int offset = 0, int limit = 5, bool useCache = true);
    /// <summary>
    /// 사용자가 존재하는지 확인합니다.
    /// </summary>
    /// <param name="id">사용자 ID</param>
    /// <returns>사용자가 존재하는지 여부</returns>
    public Task<bool> ExistsAsync(ulong id);
}
