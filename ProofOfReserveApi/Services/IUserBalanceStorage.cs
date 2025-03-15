using ProofOfReserveApi.Models;

namespace ProofOfReserveApi.Services;

/// <summary>
/// The storage of user balances.
/// </summary>
public interface IUserBalanceStorage
{
    /// <summary>
    /// Ads new user or updates balance of existing user.
    /// </summary>
    /// <param name="userData">The user data.</param>
    void AddOrUpdateUser(string userData);

    /// <summary>
    /// Gets user data.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>The user balance. (null if not found.)</returns>
    UserBalance? GetUser(int userId);

    /// <summary>
    /// Checks does user exists in storage.
    /// </summary>
    /// <param name="userId">The user ID to check.</param>
    /// <returns>true if exists, otherwise false.</returns>
    bool UserExists(int userId);

    /// <summary>
    /// Gets all users from database.
    /// </summary>
    /// <returns>The all users from database.</returns>
    List<UserBalance> GetAllUsers();
}