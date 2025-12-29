using System.Security.Claims;

public static class ClaimExtensions
{
	public static int? GetCompanyIdSafe(this ClaimsPrincipal user)
	{
		var value = user.Claims.FirstOrDefault(x => x.Type == "companyId")?.Value;
		if (string.IsNullOrWhiteSpace(value))
			return null;

		if (!int.TryParse(value, out var companyId))
			return null;

		return companyId;
	}

	public static int GetUserId(this ClaimsPrincipal user)
	{
		var value = user.Claims
			.FirstOrDefault(x =>
				x.Type == ClaimTypes.NameIdentifier ||
				x.Type == "sub")?.Value;

		if (string.IsNullOrWhiteSpace(value))
			return 0;

		return int.TryParse(value, out var userId) ? userId : 0;
	}

	public static string GetRole(this ClaimsPrincipal user)
		=> user.Claims.FirstOrDefault(x => x.Type == "role")?.Value ?? "";
	public static int GetRoleId(this ClaimsPrincipal user)
	{
		var value = user.Claims
			.FirstOrDefault(x => x.Type == "roleId")?.Value;

		if (string.IsNullOrWhiteSpace(value))
			return 0;

		return int.TryParse(value, out var roleId) ? roleId : 0;
	}
	public static int GetDepartmentId(this ClaimsPrincipal user)
	{
		var value = user.Claims
			.FirstOrDefault(x => x.Type == "departmentId")?.Value;

		if (string.IsNullOrWhiteSpace(value))
			return 0;

		return int.TryParse(value, out var departmentId) ? departmentId : 0;
	}
	public static bool IsGlobalAdmin(this ClaimsPrincipal user)
		=> user.GetRole() == "GLOBAL ADMIN";

	public static bool IsSuperAdmin(this ClaimsPrincipal user)
		=> user.GetRole() == "SUPER ADMIN" || user.GetRole() == "GLOBAL ADMIN";

	public static bool IsAdmin(this ClaimsPrincipal user)
		=> user.GetRole() == "ADMIN"
		|| user.GetRole() == "SUPER ADMIN"
		|| user.GetRole() == "GLOBAL ADMIN";
	

	
}
