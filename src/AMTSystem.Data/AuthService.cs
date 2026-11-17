using AMTSystem.Core.Models;
using AMTSystem.Data.Repositories;

namespace AMTSystem.Core;

public static class AuthService
{
    // Hash of a throwaway password, so "user not found" takes as long as "wrong password"
    private static readonly string DummyHash = PasswordHasher.Hash("dummy-password-for-timing");

    public static User? Login(string username, string password)
    {
        var user = UserRepository.GetByUsername(username.Trim());

        if (user is null)
        {
            PasswordHasher.Verify(password, DummyHash);   // burn the same time
            return null;
        }

        if (!PasswordHasher.Verify(password, user.PasswordHash))
            return null;

        // Check status AFTER the password, and keep the error generic in the UI
        if (user.Status != "Active")        // adjust to your actual status values
            return null;

        if (PasswordHasher.NeedsRehash(user.PasswordHash))
            UserRepository.UpdatePasswordHash(user.Id, PasswordHasher.Hash(password));

        user.PasswordHash = "";             // don't keep the hash in memory/session
        return user;
    }
}
