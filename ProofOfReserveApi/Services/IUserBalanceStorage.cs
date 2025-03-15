using ProofOfReserveApi.Models;

namespace ProofOfReserveApi.Services;

public interface IUserBalanceStorage
{
    void AddOrUpdateUser(string userData);
    UserBalance? GetUser(int userId);
    bool UserExists(int userId);
    List<UserBalance> GetAllUsers();
}