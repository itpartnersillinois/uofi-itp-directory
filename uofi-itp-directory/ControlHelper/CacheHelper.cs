using Microsoft.AspNetCore.Components.Authorization;
using uofi_itp_directory_data.Cache;
using uofi_itp_directory_data.Security;

namespace uofi_itp_directory.ControlHelper {

    public static class CacheHelper {

        public static bool ClearCache(AuthenticationState? authenticationState, CacheHolder? cacheHolder) => authenticationState == null || cacheHolder == null
                ? false
                : cacheHolder.ClearCache(authenticationState.User?.Identity?.Name ?? "");

        public static AreaOfficeThinObject? GetCachedArea(AuthenticationState? authenticationState, CacheHolder? cacheHolder) => authenticationState == null || cacheHolder == null
                ? null
                : cacheHolder.GetArea(authenticationState.User?.Identity?.Name ?? "");

        public static int? GetCachedEmployee(AuthenticationState? authenticationState, CacheHolder? cacheHolder) => authenticationState == null || cacheHolder == null
        ? null
        : cacheHolder.GetEmployee(authenticationState.User?.Identity?.Name ?? "");

        public static AreaOfficeThinObject? GetCachedOffice(AuthenticationState? authenticationState, CacheHolder? cacheHolder) => authenticationState == null || cacheHolder == null
        ? null
        : cacheHolder.GetOffice(authenticationState.User?.Identity?.Name ?? "");

        public static void SetCachedArea(AuthenticationState? authenticationState, CacheHolder? cacheHolder, AreaOfficeThinObject area) {
            if (authenticationState != null && cacheHolder != null) {
                cacheHolder.SetArea(authenticationState.User?.Identity?.Name ?? "", area);
            }
        }

        public static void SetCachedEmployee(AuthenticationState? authenticationState, CacheHolder? cacheHolder, int? employeeId) {
            if (authenticationState != null && cacheHolder != null && employeeId.HasValue) {
                cacheHolder.SetEmployeeId(authenticationState.User?.Identity?.Name ?? "", employeeId.Value);
            }
        }

        public static void SetCachedOffice(AuthenticationState? authenticationState, CacheHolder? cacheHolder, AreaOfficeThinObject office) {
            if (authenticationState != null && cacheHolder != null) {
                cacheHolder.SetOffice(authenticationState.User?.Identity?.Name ?? "", office);
            }
        }
    }
}