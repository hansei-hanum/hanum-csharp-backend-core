
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Hanum.Core.Helpers;

public static class HanumControllerBaseExtensions {
    public static ulong GetHanumUserClaim(this ControllerBase controller) =>
        ulong.Parse(controller.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}