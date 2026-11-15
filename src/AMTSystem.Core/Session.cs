using AMTSystem.Core.Models;

namespace AMTSystem.Core;

public static class Session
{
    public static User? CurrentUser { get; private set; }
    public static bool IsAdmin => CurrentUser?.Role == "Admin";

    public static void Start(User user) => CurrentUser = user;
    public static void End() => CurrentUser = null;
}
