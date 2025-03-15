using ProofOfReserveApi.Services;

namespace ProofOfReserveApi.Extensions;

public static class DataSeedExtension
{
    public static void PushInitialData(this WebApplication app, int usersCount = 8)
    {
        var storage = app.Services.GetRequiredService<IUserBalanceStorage>();

        for(int i = 1; i <= usersCount; i++)
        {
            string dataString = $"({i},{i}{i}{i}{i})";
            storage.AddOrUpdateUser(dataString);
        }
    }
}
